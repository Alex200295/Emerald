# Emerald 💎

Un jeu vidéo 3D développé avec Unity.

## 🚀 CI/CD avec GitHub Actions

Ce projet est configuré avec un pipeline CI/CD complet pour:
- ✅ Tests automatiques
- 🏗️ Builds multi-plateformes (Windows, Linux, macOS, WebGL)
- 📊 Rapports de couverture de code
- 🌐 Déploiement automatique sur GitHub Pages

### Quick Start

1. **Configuration initiale**: Consultez [CICD-SETUP.md](./CICD-SETUP.md) pour le guide complet
2. **Activer votre licence Unity**: Exécutez le workflow "Activate Unity License" dans Actions
3. **Configurer les secrets**: Ajoutez `UNITY_LICENSE`, `UNITY_EMAIL`, et `UNITY_PASSWORD`
4. **Push votre code**: Le pipeline s'exécute automatiquement!

## 📁 Structure du projet

```
Emerald/
├── Assets/              # Ressources Unity
│   ├── Scenes/         # Scènes du jeu
│   ├── Scripts/        # Scripts C#
│   └── Prefabs/        # Prefabs Unity
├── ProjectSettings/    # Configuration Unity
├── .github/
│   └── workflows/      # GitHub Actions workflows
└── CICD-SETUP.md      # Guide de configuration CI/CD
```

## 🎮 Développement

### Prérequis
- Unity 2022.3.10f1 ou supérieur
- Git avec LFS
- Éditeur de code (VS Code, Rider, Visual Studio)

### Installation locale
```bash
git clone https://github.com/Alex200295/Emerald.git
cd Emerald
# Ouvrir le projet dans Unity Hub
```

## 📖 Documentation

- [Guide CI/CD](./CICD-SETUP.md) - Configuration complète du pipeline
- [Unity Manual](https://docs.unity3d.com/)
- [GameCI Documentation](https://game.ci/docs)

## 📝 License

Voir le fichier [LICENSE](./LICENSE) pour plus de détails.