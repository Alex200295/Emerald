# 🎮 Guide d'Activation de Licence Unity Personal (GRATUITE)

## ✅ Votre configuration est déjà compatible avec Unity Personal !

Les workflows GitHub Actions sont déjà configurés pour Unity Personal (gratuit). Il vous suffit maintenant de générer et configurer votre licence.

---

## 📝 Étapes d'activation (simple et rapide)

### Étape 1 : Générer le fichier d'activation (.alf)

1. Allez sur GitHub dans votre repository : https://github.com/Alex200295/Emerald
2. Cliquez sur l'onglet **Actions**
3. Dans la liste de gauche, cliquez sur **"Activate Unity License"**
4. Cliquez sur le bouton **"Run workflow"** (à droite)
5. Cliquez sur le bouton vert **"Run workflow"**
6. Attendez environ 1 minute que le workflow se termine
7. Une fois terminé, **téléchargez l'artifact** `Unity_Activation_File.alf`

### Étape 2 : Obtenir votre fichier de licence (.ulf)

1. Allez sur : **https://license.unity3d.com/manual**
2. Connectez-vous avec votre compte Unity (le même que Unity Hub)
3. Uploadez le fichier `.alf` que vous venez de télécharger
4. Remplissez le formulaire :
   - **License type** : Personal (FREE)
   - Cochez la case confirmant que vous respectez les conditions
5. Téléchargez le fichier `.ulf` généré par Unity

### Étape 3 : Convertir le fichier .ulf en Base64

**Sur Linux/Mac :**
```bash
cat Unity_v6000.x.ulf | base64 -w 0 > unity_license_base64.txt
```

**Sur Windows (PowerShell) :**
```powershell
[Convert]::ToBase64String([IO.File]::ReadAllBytes("Unity_v6000.x.ulf")) | Out-File unity_license_base64.txt
```

**Sur Windows (Command Prompt) :**
```cmd
certutil -encode Unity_v6000.x.ulf encoded.txt
```
(Puis ouvrez `encoded.txt` et copiez tout sauf les lignes `-----BEGIN CERTIFICATE-----` et `-----END CERTIFICATE-----`)

### Étape 4 : Configurer les secrets GitHub

1. Allez dans votre repo GitHub : https://github.com/Alex200295/Emerald
2. Cliquez sur **Settings** (en haut à droite)
3. Dans le menu de gauche, cliquez sur **Secrets and variables** > **Actions**
4. Cliquez sur **"New repository secret"**

Ajoutez ces 3 secrets :

| Nom du secret | Valeur | Description |
|---------------|--------|-------------|
| `UNITY_LICENSE` | Contenu du fichier `unity_license_base64.txt` | La licence Unity en base64 (TOUT le contenu, même si c'est très long) |
| `UNITY_EMAIL` | votre@email.com | L'email de votre compte Unity |
| `UNITY_PASSWORD` | VotreMotDePasse | Le mot de passe de votre compte Unity |

**⚠️ Important :**
- Pour `UNITY_LICENSE`, copiez TOUT le contenu du fichier base64 (pas de retours à la ligne supplémentaires)
- Vérifiez qu'il n'y a pas d'espaces avant ou après

---

## 🚀 Tester votre configuration

Une fois les secrets configurés, vous pouvez tester :

1. Faites un petit changement dans votre projet (par exemple, modifiez le README)
2. Commitez et poussez sur la branche `main` :
   ```bash
   git add .
   git commit -m "test: vérifier le pipeline CI/CD"
   git push origin main
   ```
3. Allez dans **Actions** sur GitHub
4. Vous devriez voir le pipeline s'exécuter automatiquement !

---

## ❓ Problèmes fréquents

### "License not found" ou "Invalid license"
- Vérifiez que vous avez bien copié TOUT le contenu base64
- Assurez-vous qu'il n'y a pas d'espaces ou de retours à la ligne en trop
- Vérifiez que l'email et le mot de passe sont corrects

### "Activation failed"
- Vérifiez que votre compte Unity est bien en Personal (gratuit)
- Essayez de régénérer le fichier .ulf depuis https://license.unity3d.com/manual
- Vérifiez que la version Unity correspond : 6000.2.11f1 (Unity 6)

### Le workflow ne démarre pas
- Vérifiez que vous avez bien poussé sur une branche autorisée (`main`, `develop`, ou `claude/**`)
- Vérifiez que GitHub Actions est activé dans votre repo (Settings > Actions)

---

## 📊 Ce qui sera automatiquement exécuté

Une fois configuré, à chaque push sur `main`, `develop` ou branches `claude/**` :

✅ Tests unitaires Unity
✅ Build Windows 64-bit
✅ Build Linux 64-bit
✅ Build macOS
✅ Build WebGL
✅ Déploiement WebGL sur GitHub Pages (depuis `main`)

Les builds seront disponibles dans **Actions > Artifacts** après chaque exécution réussie.

---

## 🎉 C'est tout !

Votre projet Unity est maintenant configuré avec un pipeline CI/CD professionnel, entièrement gratuit avec Unity Personal !

**Besoin d'aide ?** Ouvrez une issue sur le repository ou consultez :
- [Guide CI/CD complet](./CICD-SETUP.md)
- [Documentation GameCI](https://game.ci/docs)
- [Unity License Manual](https://license.unity3d.com/manual)
