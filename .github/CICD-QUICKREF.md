# 🚀 CI/CD Quick Reference

## Activation en 3 étapes

### 1️⃣ Obtenir le fichier d'activation
```bash
Actions > "Activate Unity License" > Run workflow > Download .alf
```

### 2️⃣ Obtenir la licence
```
https://license.unity3d.com/manual
Upload .alf → Download .ulf
```

### 3️⃣ Configurer les secrets
```bash
# Convertir en base64
cat Unity_v2022.x.ulf | base64 -w 0

# Ajouter dans GitHub:
Settings > Secrets > New secret
```

**Secrets requis:**
- `UNITY_LICENSE` (base64 du .ulf)
- `UNITY_EMAIL` (votre email Unity)
- `UNITY_PASSWORD` (votre mot de passe Unity)

## Commandes utiles

### Vérifier la structure
```bash
tree -L 2 -I 'Library|Temp|Logs|obj'
```

### Forcer un build
```bash
Actions > "Unity CI/CD Pipeline" > Run workflow
```

### Tester localement (simulation CI)
```bash
# Linux/Mac
docker run -it unityci/editor:2022.3.10f1 bash

# Windows
# Utiliser Unity Editor directement
```

## Temps de build estimés

| Plateforme | Temps | Taille |
|------------|-------|--------|
| Tests | 2-5 min | N/A |
| Windows | 5-15 min | 50-200 MB |
| Linux | 5-15 min | 50-200 MB |
| macOS | 5-15 min | 50-200 MB |
| WebGL | 10-20 min | 20-100 MB |

**Total pipeline complet:** ~30-60 minutes

## Quotas GitHub Actions

- **Free:** 2000 min/mois
- **Pro:** 3000 min/mois
- **Team:** 10000 min/mois

**Optimisation:**
- Limiter aux branches importantes
- Build manuel seulement
- Désactiver certaines plateformes

## Dépannage rapide

### ❌ License error
```bash
# Vérifier le secret
Settings > Secrets > UNITY_LICENSE > Update

# Régénérer si nécessaire
Actions > "Activate Unity License" > Run workflow
```

### ❌ Build failed
```bash
# Vérifier les logs
Actions > Failed workflow > Cliquer sur l'étape rouge

# Causes communes:
# - Erreurs de compilation
# - Tests qui échouent
# - Mémoire insuffisante
```

### ❌ Tests failed
```bash
# Tester localement d'abord
Unity > Window > General > Test Runner

# Vérifier les scènes de test
Build Settings > Add Open Scenes
```

## Liens utiles

- 📚 [Documentation complète](../CICD-SETUP.md)
- 🎮 [GameCI Docs](https://game.ci/docs)
- 🔧 [Unity Actions](https://github.com/game-ci/unity-actions)
- 💬 [GameCI Discord](https://game.ci/discord)
