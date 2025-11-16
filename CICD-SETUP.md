# Guide de Configuration CI/CD pour Unity avec GitHub Actions

## 📋 Table des matières
1. [Introduction](#introduction)
2. [Prérequis](#prérequis)
3. [Configuration initiale](#configuration-initiale)
4. [Activation de la licence Unity](#activation-de-la-licence-unity)
5. [Configuration des secrets GitHub](#configuration-des-secrets-github)
6. [Workflows disponibles](#workflows-disponibles)
7. [Plateformes de build](#plateformes-de-build)
8. [Optimisations](#optimisations)
9. [Dépannage](#dépannage)

---

## 🎯 Introduction

Ce projet utilise **GitHub Actions** avec **GameCI** pour automatiser:
- ✅ Tests unitaires automatiques
- 🏗️ Builds multi-plateformes (Windows, Linux, macOS, WebGL)
- 📊 Rapports de couverture de code
- 🚀 Déploiement automatique sur GitHub Pages (WebGL)
- 🎮 Distribution des builds

## 📦 Prérequis

- Compte Unity (Personal, Plus ou Pro)
- Compte GitHub
- Projet Unity (version 6000.2.11f1 / Unity 6 ou supérieure)
- Git installé localement

## ⚙️ Configuration initiale

### 1. Configurer Git pour Unity

Assurez-vous que votre projet Unity a les bonnes configurations Git:

```bash
# Configurer Git LFS pour les gros fichiers
git lfs install
git lfs track "*.psd"
git lfs track "*.fbx"
git lfs track "*.png"
git lfs track "*.jpg"
git lfs track "*.mp3"
git lfs track "*.wav"
git lfs track "*.mp4"
```

### 2. Configurer Unity pour le contrôle de version

Dans Unity Editor:
1. `Edit > Project Settings > Editor`
2. **Version Control Mode**: Visible Meta Files
3. **Asset Serialization Mode**: Force Text
4. **Line Endings For New Scripts**: Unix

Ces paramètres assurent une meilleure compatibilité avec Git.

## 🔑 Activation de la licence Unity

### Méthode 1: Activation manuelle (Recommandée pour débuter)

1. **Exécuter le workflow d'activation**:
   - Allez dans `Actions` sur GitHub
   - Sélectionnez "Activate Unity License"
   - Cliquez sur "Run workflow"

2. **Télécharger le fichier .alf**:
   - Une fois le workflow terminé, téléchargez l'artifact `Unity_Activation_File.alf`

3. **Obtenir la licence**:
   - Allez sur https://license.unity3d.com/manual
   - Uploadez le fichier `.alf`
   - Remplissez le formulaire
   - Téléchargez le fichier `.ulf` généré

4. **Convertir la licence en base64**:

   **Linux/Mac**:
   ```bash
   cat Unity_v2022.x.ulf | base64 -w 0 > unity_license_base64.txt
   ```

   **Windows (PowerShell)**:
   ```powershell
   [Convert]::ToBase64String([IO.File]::ReadAllBytes("Unity_v2022.x.ulf")) | Out-File unity_license_base64.txt
   ```

5. **Copier le contenu** de `unity_license_base64.txt`

### Méthode 2: Activation automatique (Unity Pro/Plus uniquement)

Si vous avez Unity Pro/Plus avec un serial key:
- Pas besoin de fichier .ulf
- Utilisez directement vos identifiants Unity

## 🔐 Configuration des secrets GitHub

Allez dans `Settings > Secrets and variables > Actions > New repository secret`

### Pour Unity Personal/Plus:

| Secret | Description | Exemple |
|--------|-------------|---------|
| `UNITY_LICENSE` | Contenu base64 du fichier .ulf | `PD94bWwgdmVyc2lvbj0iMS4w...` |
| `UNITY_EMAIL` | Email de votre compte Unity | `votre@email.com` |
| `UNITY_PASSWORD` | Mot de passe Unity | `VotreMotDePasse123!` |

### Pour Unity Pro avec Serial Key:

| Secret | Description |
|--------|-------------|
| `UNITY_SERIAL` | Votre clé de série Unity Pro |
| `UNITY_EMAIL` | Email de votre compte Unity |
| `UNITY_PASSWORD` | Mot de passe Unity |

## 🚀 Workflows disponibles

### 1. `main.yml` - Pipeline CI/CD principal

**Déclencheurs**:
- Push sur `main`, `develop`, ou branches `claude/**`
- Pull requests vers `main` ou `develop`
- Exécution manuelle

**Jobs**:
1. **checkLicense**: Vérifie que la licence est configurée
2. **testRunner**: Exécute les tests Unity
3. **buildWindows**: Build pour Windows 64-bit
4. **buildLinux**: Build pour Linux 64-bit
5. **buildMacOS**: Build pour macOS
6. **buildWebGL**: Build pour WebGL
7. **deployWebGL**: Déploie WebGL sur GitHub Pages (main uniquement)

### 2. `activate-unity-license.yml` - Activation de licence

**Usage**: Exécution manuelle unique pour obtenir le fichier d'activation

## 🎮 Plateformes de build

### Plateformes activées par défaut:

| Plateforme | Target | Taille moyenne | Temps de build |
|------------|--------|----------------|----------------|
| Windows | StandaloneWindows64 | ~50-200 MB | 5-15 min |
| Linux | StandaloneLinux64 | ~50-200 MB | 5-15 min |
| macOS | StandaloneOSX | ~50-200 MB | 5-15 min |
| WebGL | WebGL | ~20-100 MB | 10-20 min |

### Ajouter d'autres plateformes:

Pour Android:
```yaml
buildAndroid:
  name: Build for Android
  runs-on: ubuntu-latest
  needs: testRunner
  steps:
    - uses: actions/checkout@v4
    - uses: game-ci/unity-builder@v4
      with:
        targetPlatform: Android
        androidAppBundle: false
        androidKeystoreName: user.keystore
```

Pour iOS (nécessite macOS runner):
```yaml
buildIOS:
  name: Build for iOS
  runs-on: macos-latest
  needs: testRunner
  steps:
    - uses: actions/checkout@v4
    - uses: game-ci/unity-builder@v4
      with:
        targetPlatform: iOS
```

## ⚡ Optimisations

### 1. Cache de la Library

Le workflow utilise déjà le cache pour accélérer les builds:
```yaml
- uses: actions/cache@v3
  with:
    path: Library
    key: Library-${{ hashFiles('Assets/**') }}
```

### 2. Builds parallèles

Les builds sont exécutés en parallèle après les tests, économisant du temps.

### 3. Limiter les builds

Pour économiser les minutes GitHub Actions, modifiez les déclencheurs:

```yaml
on:
  push:
    branches:
      - main  # Seulement sur main
  workflow_dispatch:  # Exécution manuelle uniquement
```

### 4. Builds conditionnels

Construire seulement WebGL sur `main`:
```yaml
buildWebGL:
  if: github.ref == 'refs/heads/main'
```

## 🐛 Dépannage

### Erreur: "License not found"

**Solution**:
1. Vérifiez que le secret `UNITY_LICENSE` est bien configuré
2. Vérifiez que c'est bien le contenu base64 complet
3. Pas d'espaces ou de retours à la ligne supplémentaires

### Erreur: "Activation failed"

**Solution**:
- Pour Unity Personal: Vérifiez `UNITY_EMAIL` et `UNITY_PASSWORD`
- Essayez de régénérer le fichier .ulf
- Vérifiez que votre version Unity correspond (`UNITY_VERSION`)

### Build échoue avec "Out of memory"

**Solution**:
```yaml
- uses: game-ci/unity-builder@v4
  with:
    customParameters: '-executeMethod YourBuildMethod -buildTarget StandaloneWindows64 -quit -nographics'
```

### Tests échouent

**Solution**:
1. Testez localement d'abord: `Unity > Window > General > Test Runner`
2. Vérifiez les dépendances de vos tests
3. Assurez-vous que vos scènes de test sont incluses dans le build

### GitHub Actions dépasse les minutes gratuites

**Solutions**:
- Limitez les builds aux branches importantes
- Désactivez certaines plateformes
- Utilisez `workflow_dispatch` pour builds manuels uniquement
- Envisagez GitHub Pro pour plus de minutes

## 📊 Accéder aux builds

### Artifacts

Après chaque build réussi:
1. Allez dans `Actions`
2. Sélectionnez le workflow run
3. Scrollez vers le bas jusqu'à "Artifacts"
4. Téléchargez le build souhaité

### GitHub Pages (WebGL)

Si activé, votre jeu WebGL sera disponible à:
```
https://[votre-username].github.io/[nom-du-repo]/
```

Pour activer GitHub Pages:
1. `Settings > Pages`
2. Source: `Deploy from a branch`
3. Branch: `gh-pages` / `root`

## 🔄 Workflow typique

1. **Développement local**
   ```bash
   git checkout -b feature/ma-fonctionnalite
   # ... développement ...
   git add .
   git commit -m "feat: ajout de ma fonctionnalité"
   git push origin feature/ma-fonctionnalite
   ```

2. **Pull Request**
   - Les tests s'exécutent automatiquement
   - Vérifiez les résultats avant merge

3. **Merge vers main**
   - Tous les builds sont générés
   - WebGL est déployé automatiquement

4. **Release**
   - Téléchargez les artifacts
   - Créez une release GitHub avec les builds

## 📚 Ressources utiles

- [GameCI Documentation](https://game.ci/docs)
- [Unity Manual](https://docs.unity3d.com/)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Unity Test Framework](https://docs.unity3d.com/Packages/com.unity.test-framework@latest)

## 🎉 Félicitations!

Votre projet Unity est maintenant configuré avec un pipeline CI/CD professionnel! 🚀

Pour toute question ou problème, ouvrez une issue sur le repository.
