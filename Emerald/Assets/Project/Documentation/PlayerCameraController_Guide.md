# Guide de Configuration - PlayerCameraController

## Vue d'ensemble

Le `PlayerCameraController` est un système de caméra pour Unity utilisant **Cinemachine 3.x**. Il fournit une caméra troisième personne entièrement automatisée avec rotation contrôlée par la souris.

### Fonctionnalités

- ✅ **Auto-configuration complète** - Crée automatiquement tous les composants nécessaires
- ✅ **Cinemachine 3.x** - Utilise la dernière version de Cinemachine
- ✅ **Third Person Follow** - Caméra professionnelle type jeu d'action
- ✅ **Rotation fluide** - Contrôle de la souris avec limites verticales
- ✅ **API publique** - Méthodes pour obtenir la direction de la caméra
- ✅ **Gizmos visuels** - Aide au débogage dans l'éditeur

---

## Installation et Configuration

### Prérequis

1. **Cinemachine 3.x** doit être installé dans le projet
   - Via Package Manager : `Window > Package Manager`
   - Rechercher "Cinemachine" et installer la version 3.x

2. **CinemachineBrain** doit être présent sur la Main Camera
   - Sélectionner la Main Camera dans la hiérarchie
   - `Add Component > Cinemachine Brain`

### Option 1 : Configuration Automatique (Recommandé)

Le `PlayerCameraController` s'auto-configure entièrement. Il suffit de l'ajouter au GameObject Player :

1. **Ouvrir le prefab Player** dans l'éditeur
2. **Sélectionner le GameObject racine "Player"**
3. **Ajouter le composant** : `Add Component > Player Camera Controller`
4. **C'est tout !** Le script va automatiquement :
   - Créer un `CameraFollowTarget` à hauteur des yeux (Y = 1.6m)
   - Rechercher ou créer une `Cinemachine Camera`
   - Configurer le `CinemachineThirdPersonFollow` (Body)
   - Configurer les références Follow et LookAt

### Option 2 : Configuration Manuelle

Si vous préférez contrôler manuellement la configuration :

#### Étape 1 : Créer le CameraFollowTarget

1. Dans le prefab Player, **créer un enfant** du GameObject racine
2. Le nommer **"CameraFollowTarget"**
3. Le positionner à **Y = 1.6** (hauteur des yeux)
4. Assigner ce Transform au champ `Camera Follow Target` du script

#### Étape 2 : Créer la Cinemachine Camera

1. Dans la scène : `GameObject > Cinemachine > Cinemachine Camera`
2. Nommer la caméra : **"CM vcam_Player"**
3. Configurer la priorité : **10** (ou plus haut)
4. Assigner cette caméra au champ `Cinemachine Camera` du script

#### Étape 3 : Configurer le Body (Third Person Follow)

1. Sélectionner la Cinemachine Camera
2. Dans l'Inspector, section **Body**
3. Choisir **"Third Person Follow"**
4. Les paramètres seront automatiquement configurés par le script

---

## Migration depuis l'ancien PlayerCamera

Si vous utilisez l'ancien script `PlayerCamera.cs`, suivez ces étapes :

### Étape 1 : Mise à jour du prefab Player

1. **Ouvrir le prefab Player**
2. **Sélectionner le GameObject "CameraHolder"** (ou équivalent)
3. **Supprimer le composant** `PlayerCamera`
4. **Transférer le composant sur le GameObject racine Player** :
   - Sélectionner le GameObject racine "Player"
   - `Add Component > Player Camera Controller`

### Étape 2 : Supprimer l'ancien CameraHolder (optionnel)

L'ancien système utilisait un CameraHolder avec une caméra enfant. Avec Cinemachine, ce n'est plus nécessaire :

1. Vous pouvez **supprimer le GameObject "CameraHolder"** et ses enfants
2. La Main Camera de la scène sera contrôlée par Cinemachine
3. Le `PlayerCameraController` créera automatiquement le `CameraFollowTarget`

### Étape 3 : Vérifier CinemachineBrain

1. **Sélectionner la Main Camera** dans la scène
2. Vérifier qu'elle a un composant **"Cinemachine Brain"**
3. Si absent, l'ajouter : `Add Component > Cinemachine Brain`

### Étape 4 : Tester

1. **Lancer le jeu** en mode Play
2. Vérifier les messages dans la Console :
   ```
   [PlayerCameraController] CameraFollowTarget créé automatiquement à (0.0, 1.6, 0.0)
   [PlayerCameraController] Cinemachine Camera créée automatiquement
   [PlayerCameraController] CinemachineThirdPersonFollow ajouté à la caméra
   [PlayerCameraController] Cinemachine Camera configurée pour suivre CameraFollowTarget
   ```
3. La caméra devrait suivre le joueur en troisième personne

---

## Paramètres du Script

### Références

| Paramètre | Type | Description |
|-----------|------|-------------|
| **Camera Follow Target** | Transform | Le point que la caméra va suivre (auto-créé si vide) |
| **Cinemachine Camera** | CinemachineCamera | La caméra virtuelle Cinemachine (auto-trouvée si vide) |

### Sensibilité de la souris

| Paramètre | Type | Valeur par défaut | Description |
|-----------|------|-------------------|-------------|
| **Mouse Sensitivity X** | float | 2.0 | Sensibilité horizontale de la souris |
| **Mouse Sensitivity Y** | float | 2.0 | Sensibilité verticale de la souris |

### Limites de rotation verticale

| Paramètre | Type | Valeur par défaut | Description |
|-----------|------|-------------------|-------------|
| **Min Vertical Angle** | float | -40° | Angle minimum (vers le bas) |
| **Max Vertical Angle** | float | 80° | Angle maximum (vers le haut) |

### Configuration Cinemachine (si auto-créée)

| Paramètre | Type | Valeur par défaut | Description |
|-----------|------|-------------------|-------------|
| **Camera Distance** | float | 5.0 | Distance de la caméra derrière le joueur |
| **Shoulder Offset** | Vector3 | (0.5, 0, 0) | Décalage par rapport à l'épaule |
| **Vertical Arm Length** | float | 0.4 | Hauteur du bras vertical de la caméra |
| **Camera Side** | float | 1.0 | Côté de la caméra (-1 = gauche, 1 = droite) |

---

## API Publique

Le script expose des méthodes publiques pour d'autres scripts :

### HandleCameraRotation(Vector2 lookInput)

Gère la rotation de la caméra basée sur l'entrée de la souris.

```csharp
// Appelée depuis PlayerController dans LateUpdate()
cameraController.HandleCameraRotation(lookInput);
```

**Paramètres :**
- `lookInput` : Vecteur de mouvement de la souris (X = horizontal, Y = vertical)

### GetCameraForward() : Vector3

Obtient la direction avant de la caméra (utilisé pour le mouvement du joueur).

```csharp
Vector3 forward = cameraController.GetCameraForward();
// Retourne un vecteur normalisé sur le plan horizontal (Y = 0)
```

**Retour :**
- Direction avant de la caméra projetée sur le plan horizontal

### GetCameraRight() : Vector3

Obtient la direction droite de la caméra (utilisé pour le mouvement du joueur).

```csharp
Vector3 right = cameraController.GetCameraRight();
// Retourne un vecteur normalisé sur le plan horizontal (Y = 0)
```

**Retour :**
- Direction droite de la caméra projetée sur le plan horizontal

---

## Intégration avec PlayerMovement

Le script `PlayerMovement` a été mis à jour pour utiliser `PlayerCameraController` :

```csharp
// Ancienne méthode (deprecated)
Vector3 forward = Camera.main.transform.forward;

// Nouvelle méthode (recommandée)
Vector3 forward = cameraController.GetCameraForward();
Vector3 right = cameraController.GetCameraRight();
```

### Avantages

1. **Performance** : Pas besoin de rechercher `Camera.main` à chaque frame
2. **Cohérence** : Utilise la même référence de caméra partout
3. **Modularité** : Le système de mouvement dépend du système de caméra
4. **Flexibilité** : Facile de changer la logique de la caméra sans toucher au mouvement

---

## Débogage

### Gizmos dans l'éditeur

Lorsque vous sélectionnez le GameObject Player dans l'éditeur :

- **Sphère cyan** : Position du CameraFollowTarget
- **Rayon bleu** : Direction de vue de la caméra

### Messages de débogage

Le script affiche des messages préfixés `[PlayerCameraController]` :

```
[PlayerCameraController] CameraFollowTarget créé automatiquement à (0.0, 1.6, 0.0)
[PlayerCameraController] Cinemachine Camera trouvée automatiquement : CM vcam_Player
[PlayerCameraController] CinemachineThirdPersonFollow ajouté à la caméra
[PlayerCameraController] Cinemachine Camera configurée pour suivre CameraFollowTarget
```

### Problèmes courants

#### 1. La caméra ne suit pas le joueur

**Solution :**
- Vérifier que `CinemachineBrain` est sur la Main Camera
- Vérifier que la Cinemachine Camera a une priorité > 0
- Vérifier dans la Console les messages d'auto-configuration

#### 2. La rotation de la caméra ne fonctionne pas

**Solution :**
- Vérifier que `PlayerController` appelle `HandleCameraRotation()` dans `LateUpdate()`
- Vérifier que le curseur est verrouillé (`Cursor.lockState = CursorLockMode.Locked`)
- Ajuster les paramètres de sensibilité

#### 3. La caméra traverse les murs

**Solution :**
- Dans Cinemachine Camera, ajouter une **Cinemachine Collision Extension**
- Configurer les layers de collision
- Ajuster les paramètres de smoothing

#### 4. Le mouvement du joueur ne suit pas la caméra

**Solution :**
- Vérifier que `PlayerMovement` a bien une référence à `PlayerCameraController`
- Vérifier le message dans la Console :
  ```
  [PlayerMovement] PlayerCameraController manquant. La direction de mouvement utilisera Camera.main comme fallback.
  ```
- Le fallback sur `Camera.main` devrait fonctionner, mais c'est moins optimal

---

## Structure du Système

```
Player (GameObject)
├── PlayerController.cs         → Gère les inputs
├── PlayerMovement.cs           → Gère le mouvement physique
├── PlayerCameraController.cs   → Gère la rotation de la caméra ⭐
├── CharacterController         → Composant Unity
├── PlayerInput                 → Unity Input System
│
└── CameraFollowTarget (créé automatiquement)
    └── (point de suivi pour la caméra)

Scène
├── Main Camera
│   └── CinemachineBrain       → Contrôle la caméra principale
│
└── CM vcam_Player (créé automatiquement)
    ├── CinemachineCamera
    └── CinemachineThirdPersonFollow  → Body
```

---

## Workflow recommandé

### Configuration initiale

1. Installer Cinemachine 3.x
2. Ajouter `CinemachineBrain` sur la Main Camera
3. Ajouter `PlayerCameraController` sur le GameObject Player
4. Lancer le jeu → tout s'auto-configure ! ✨

### Ajustement des paramètres

1. Lancer le jeu en mode Play
2. Sélectionner le GameObject Player
3. Ajuster les paramètres en temps réel :
   - Sensibilité de la souris
   - Limites de rotation verticale
   - Distance de la caméra
   - Position de l'épaule
4. Noter les valeurs qui vous conviennent
5. Arrêter le Play mode et réappliquer les valeurs

### Personnalisation avancée

Pour des besoins spécifiques (caméra sur l'épaule gauche, zoom dynamique, etc.) :

1. Créer manuellement la Cinemachine Camera
2. Configurer le Body (Third Person Follow) comme souhaité
3. Assigner la caméra au champ `Cinemachine Camera`
4. Le script conservera votre configuration

---

## Comparaison avec l'ancien système

| Fonctionnalité | PlayerCamera (ancien) | PlayerCameraController (nouveau) |
|----------------|----------------------|----------------------------------|
| Framework | Custom | Cinemachine 3.x |
| Auto-configuration | ❌ Non | ✅ Oui |
| Third Person Follow | ⚠️ Basique | ✅ Professionnel |
| Collision avec murs | ❌ Non | ✅ Avec extension |
| Damping/Smoothing | ⚠️ Basique | ✅ Avancé |
| Compatibilité Timeline | ❌ Non | ✅ Oui |
| Blend entre caméras | ❌ Non | ✅ Oui |
| Extensions | ❌ Non | ✅ Nombreuses |

---

## Prochaines étapes

### Extensions recommandées

1. **Cinemachine Collision** - Éviter les murs
   ```
   Cinemachine Camera > Add Extension > CinemachineCollider
   ```

2. **Cinemachine Impulse Listener** - Shake de caméra
   ```
   Cinemachine Camera > Add Extension > CinemachineImpulseListener
   ```

3. **Cinemachine Follow Zoom** - Zoom dynamique
   ```
   Cinemachine Camera > Add Extension > CinemachineFollowZoom
   ```

### Améliorations possibles

- [ ] Système de ciblage (lock-on)
- [ ] Basculement première/troisième personne
- [ ] Zoom dynamique selon la vitesse
- [ ] Caméra cinématique pour les événements
- [ ] Multiples profils de caméra (exploration, combat, etc.)

---

## Support

Pour toute question ou problème :

1. Vérifier la Console Unity pour les messages de débogage
2. Consulter la documentation Cinemachine : [Unity Cinemachine](https://docs.unity3d.com/Packages/com.unity.cinemachine@3.0/manual/index.html)
3. Vérifier les Gizmos dans l'éditeur
4. Tester avec les valeurs par défaut d'abord

---

**Version :** 1.0.0
**Dernière mise à jour :** 2025-11-16
**Auteur :** Claude AI
**Licence :** Apache License 2.0
