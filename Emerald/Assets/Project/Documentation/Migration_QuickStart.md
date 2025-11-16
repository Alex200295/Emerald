# Guide de Migration Rapide - PlayerCameraController

## TL;DR - Migration en 5 minutes

Vous utilisez l'ancien `PlayerCamera.cs` et voulez passer au nouveau `PlayerCameraController.cs` avec Cinemachine 3.x ? Suivez ces étapes :

---

## Étape 1 : Installer Cinemachine

1. Ouvrir **Window > Package Manager**
2. Rechercher **"Cinemachine"**
3. Installer la version **3.x**

⏱️ Temps estimé : 1 minute

---

## Étape 2 : Ajouter CinemachineBrain

1. Dans la hiérarchie de scène, sélectionner **Main Camera**
2. Dans l'Inspector : **Add Component**
3. Rechercher et ajouter **"Cinemachine Brain"**

⏱️ Temps estimé : 30 secondes

---

## Étape 3 : Mettre à jour le prefab Player

1. Ouvrir le prefab **Player** dans l'éditeur
2. Sélectionner le GameObject **"CameraHolder"**
3. **Supprimer** le composant `PlayerCamera` (ancien)
4. Sélectionner le GameObject racine **"Player"**
5. **Add Component** → `Player Camera Controller` (nouveau)

⏱️ Temps estimé : 1 minute

---

## Étape 4 : Optionnel - Nettoyer

Vous pouvez maintenant supprimer le GameObject **"CameraHolder"** et tous ses enfants, car Cinemachine gère la caméra différemment.

⚠️ **Note :** Gardez uniquement la **Main Camera** dans la scène, pas dans le prefab Player.

⏱️ Temps estimé : 30 secondes

---

## Étape 5 : Tester

1. **Play** le jeu
2. Vérifier dans la Console les messages :
   ```
   [PlayerCameraController] CameraFollowTarget créé automatiquement à (0.0, 1.6, 0.0)
   [PlayerCameraController] Cinemachine Camera créée automatiquement
   [PlayerCameraController] CinemachineThirdPersonFollow ajouté à la caméra
   ```
3. Tester la rotation de la caméra avec la souris
4. Tester le mouvement du joueur

⏱️ Temps estimé : 2 minutes

---

## Vérifications

✅ **Tout fonctionne si :**
- La caméra suit le joueur en troisième personne
- La souris fait tourner la caméra
- Le joueur se déplace dans la direction de la caméra
- Aucune erreur dans la Console

❌ **Problème ? Vérifier :**
- [ ] CinemachineBrain est sur la Main Camera
- [ ] PlayerCameraController est sur le GameObject Player (pas sur un enfant)
- [ ] Le curseur est verrouillé (Escape pour le déverrouiller)

---

## Résumé des changements

### Avant (PlayerCamera)

```
Player
└── CameraHolder
    ├── PlayerCamera.cs ❌
    └── Main Camera
```

### Après (PlayerCameraController)

```
Player
├── PlayerCameraController.cs ✅
└── CameraFollowTarget (auto-créé)

Scène
├── Main Camera
│   └── CinemachineBrain ✅
└── CM vcam_Player (auto-créé)
```

---

## Ajustements recommandés

Après la migration, vous pouvez ajuster ces paramètres dans `PlayerCameraController` :

| Paramètre | Valeur suggérée | Description |
|-----------|-----------------|-------------|
| Mouse Sensitivity X | 2.0 - 5.0 | Plus élevé = rotation plus rapide |
| Mouse Sensitivity Y | 2.0 - 5.0 | Plus élevé = rotation plus rapide |
| Camera Distance | 3.0 - 7.0 | Distance derrière le joueur |
| Shoulder Offset X | 0.3 - 1.0 | Décalage sur l'épaule droite |

---

## En cas de problème

### La caméra ne bouge pas

1. Vérifier que le curseur est verrouillé
2. Appuyer sur **Escape** puis re-cliquer dans le jeu
3. Vérifier les messages dans la Console

### Le joueur ne se déplace pas dans la bonne direction

1. S'assurer que `PlayerMovement` est sur le même GameObject que `PlayerCameraController`
2. Vérifier qu'il n'y a pas de warning dans la Console

### Erreurs de compilation

1. Vérifier que Cinemachine 3.x est bien installé
2. Redémarrer Unity si nécessaire

---

## Aide détaillée

Pour plus de détails, consulter le guide complet :
📄 **PlayerCameraController_Guide.md**

---

**Total estimé : 5 minutes** ⏱️
