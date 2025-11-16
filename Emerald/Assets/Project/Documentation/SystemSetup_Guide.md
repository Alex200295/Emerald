# Guide de Configuration Système - Player Setup Complet

## Vue d'ensemble

Ce guide vous explique comment configurer entièrement le système Player dans votre projet Emerald, incluant :
1. Organisation de la hiérarchie
2. Scripts nécessaires
3. Configuration du Player Input
4. Configuration de l'Animator Controller
5. Meilleures pratiques

---

## 1. Organisation de la Hiérarchie

### ✅ Organisation recommandée (après auto-configuration)

```
Scene
├── Main Camera (doit rester à la racine)
│   └── CinemachineBrain
│
└── Player (prefab)
    ├── PlayerController
    ├── PlayerMovement
    ├── PlayerCameraController
    ├── CharacterController
    ├── PlayerInput
    │
    ├── CameraFollowTarget (créé automatiquement)
    │
    ├── CM vcam_Player (créé automatiquement)
    │   └── CinemachineCamera
    │       └── CinemachineThirdPersonFollow
    │
    ├── GroundCheck (créé automatiquement)
    │
    └── PlayerModel
        ├── Animator
        └── PlayerAnimationController
```

### 📌 Règles importantes

#### Main Camera
- ✅ **DOIT rester à la racine de la scène** (pas enfant de Player)
- ✅ **DOIT avoir un CinemachineBrain**
- ✅ **Ne PAS inclure dans le prefab Player**

**Pourquoi ?**
- Unity cherche la Main Camera au niveau racine
- Le CinemachineBrain contrôle le blend entre différentes caméras virtuelles
- Permet de changer de caméra virtuelle sans bouger la caméra physique

#### CM vcam_Player (Caméra virtuelle)
- ✅ **Enfant du Player** (nouvelle organisation améliorée)
- ✅ **Position locale à (0, 0, 0)**
- ✅ **Rotation locale à (0, 0, 0)**

**Avantages :**
- Pas d'objets libres dans la scène
- Facile à gérer dans le prefab
- Suit automatiquement le Player si placé dans une scène

**Note :** La caméra virtuelle NE suit PAS la rotation du Player car elle suit le `CameraFollowTarget` qui a sa propre rotation.

#### CameraFollowTarget
- ✅ **Enfant du Player**
- ✅ **Position locale configurée en hauteur** (1.35m par défaut)
- ✅ **Gère sa propre rotation indépendamment du Player**

#### GroundCheck
- ✅ **Enfant du Player**
- ✅ **Position locale sous les pieds** (auto-calculée)

### ❌ À ÉVITER

```
❌ BAD: Main Camera enfant du Player
Player
└── Main Camera  ⚠️ NE PAS FAIRE

❌ BAD: CM vcam_Player sans parent défini
(racine de scène)
└── CM vcam_Player  ⚠️ Acceptable mais moins organisé
```

---

## 2. Scripts Nécessaires

### Sur le GameObject "Player" (racine)

| Script | Nécessaire? | Rôle |
|--------|-------------|------|
| **CharacterController** | ✅ OUI | Composant Unity pour la physique du personnage |
| **PlayerInput** | ✅ OUI | Unity Input System - gère la capture des inputs |
| **PlayerController** | ✅ OUI | Orchestre tous les autres composants |
| **PlayerMovement** | ✅ OUI | Gère le mouvement physique |
| **PlayerCameraController** | ✅ OUI | Gère la rotation de la caméra |

**Tous ces scripts sont nécessaires !** Ils travaillent ensemble :

```
PlayerInput → PlayerController → PlayerMovement
                             ↓
                      PlayerCameraController
```

#### CharacterController
- Composant Unity natif
- Gère les collisions
- Permet le mouvement fluide sans rigidbody

**Configuration recommandée :**
```
Height: 2.0
Radius: 0.3
Center: (0, 0, 0)
Slope Limit: 45
Step Offset: 0.3
```

#### PlayerInput
- Composant Unity Input System
- Reçoit les inputs du joueur
- **DOIT avoir le PlayerInputActions assigné**

**Configuration requise :**
```
Actions: PlayerInputActions
Default Map: Player
Behavior: Send Messages ou Invoke Unity Events
```

### Sur le GameObject "PlayerModel" (enfant)

| Script | Nécessaire? | Rôle |
|--------|-------------|------|
| **Animator** | ✅ OUI | Composant Unity pour les animations |
| **PlayerAnimationController** | ✅ OUI | Script custom pour gérer les animations |

---

## 3. Configuration du Player Input

### Étape 1 : Vérifier le PlayerInputActions

Le fichier existe déjà : `/Assets/Project/Settings/PlayerInputActions.inputactions`

**Actions configurées :**
- ✅ **Move** (Vector2) → WASD
- ✅ **Look** (Vector2) → Mouse Delta
- ✅ **Jump** (Button) → Space
- ✅ **Sprint** (Button) → Left Shift
- ✅ **ToggleCursor** (Button) → Escape

### Étape 2 : Assigner au composant PlayerInput

1. **Sélectionner le GameObject Player**
2. **Dans l'Inspector, section PlayerInput :**
   ```
   Actions: [Glisser PlayerInputActions.inputactions ici]
   Default Map: Player
   Behavior: Send Messages
   ```

3. **Vérifier que les actions sont bien connectées**

Le `PlayerController` récupère automatiquement les actions dans `SetupInputActions()` :

```csharp
moveAction = playerInput.actions["Move"];
lookAction = playerInput.actions["Look"];
jumpAction = playerInput.actions["Jump"];
sprintAction = playerInput.actions["Sprint"];
toggleCursorAction = playerInput.actions["ToggleCursor"];
```

### Test rapide

1. Play mode
2. Appuyer sur WASD → Le personnage devrait bouger
3. Bouger la souris → La caméra devrait tourner
4. Appuyer sur Espace → Le personnage devrait sauter
5. Maintenir Left Shift → Le personnage devrait sprinter

---

## 4. Configuration de l'Animator Controller

### Animations disponibles

Vous avez le pack **Kevin Iglesias - Human Animations** avec :

**Femme (HumanF) :**
- Idle : `HumanF@Idle01.fbx`
- Run Forward : `HumanF@Run01_Forward.fbx`
- Run (8 directions) : Forward, Backward, Left, Right, ForwardLeft, etc.
- Combat Idle : `HumanF@CombatIdle01.fbx`
- Death : `HumanF@Death01.fbx`
- Attacks (1H, 2H, Polearm, Shield)

**Homme (HumanM) :**
- Même structure que la femme

### Créer un Animator Controller basique

#### Étape 1 : Créer l'Animator Controller

1. **Dans le Project :**
   - Clic droit dans `/Assets/Project/Animations/`
   - `Create > Animator Controller`
   - Nommer : **"PlayerAnimatorController"**

2. **Double-cliquer** pour ouvrir l'Animator window

#### Étape 2 : Créer les paramètres

Dans l'onglet **Parameters** de l'Animator :

| Nom | Type | Valeur par défaut | Description |
|-----|------|-------------------|-------------|
| Speed | Float | 0 | Vitesse normalisée (0-1) |
| IsGrounded | Bool | true | Le joueur est au sol |
| Jump | Trigger | - | Déclenche le saut |
| IsSprinting | Bool | false | Le joueur sprinte |

Ces paramètres correspondent exactement à ceux utilisés dans `PlayerAnimationController.cs` :

```csharp
private static readonly int SpeedHash = Animator.StringToHash("Speed");
private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
private static readonly int JumpHash = Animator.StringToHash("Jump");
private static readonly int IsSprintingHash = Animator.StringToHash("IsSprinting");
```

#### Étape 3 : Ajouter les animations

**Pour une configuration simple (recommandée pour débuter) :**

1. **State "Idle"** (par défaut)
   - Glisser `HumanF@Idle01.fbx` dans l'Animator
   - État orange = état par défaut

2. **State "Run"**
   - Glisser `HumanF@Run01_Forward.fbx`

3. **Transitions**
   - Idle → Run : Condition `Speed > 0.1`
   - Run → Idle : Condition `Speed < 0.1`

**Réglages des transitions :**
```
Has Exit Time: NO
Transition Duration: 0.15
```

#### Étape 4 : Configuration avancée avec Blend Tree (optionnel)

Pour un mouvement plus fluide avec toutes les directions :

1. **Créer un Blend Tree pour le mouvement**
   - Clic droit dans l'Animator → `Create State > From New Blend Tree`
   - Nommer : "Locomotion"

2. **Configurer le Blend Tree**
   - Double-cliquer sur "Locomotion"
   - Blend Type : **"2D Freeform Directional"**
   - Ajouter le paramètre `Speed` comme premier paramètre

3. **Ajouter les animations**
   - Idle (0, 0)
   - Run Forward (0, 1)
   - Run Backward (0, -1)
   - Run Left (-1, 0)
   - Run Right (1, 0)
   - Run ForwardLeft (-0.7, 0.7)
   - Run ForwardRight (0.7, 0.7)
   - Run BackwardLeft (-0.7, -0.7)
   - Run BackwardRight (0.7, -0.7)

**Note :** Pour l'instant, utilisez la configuration simple. Le Blend Tree peut être ajouté plus tard.

#### Étape 5 : Assigner l'Animator Controller

1. **Sélectionner le GameObject "PlayerModel"** (enfant de Player)
2. **Dans le composant Animator :**
   ```
   Controller: [Glisser PlayerAnimatorController ici]
   Avatar: [Auto-créé depuis le FBX]
   Apply Root Motion: NO ❌
   ```

**IMPORTANT : Apply Root Motion = NO**
- Nous utilisons le CharacterController pour le mouvement
- Le Root Motion entrerait en conflit

#### Étape 6 : Configuration du PlayerAnimationController

1. **Sélectionner le GameObject "PlayerModel"**
2. **Dans le PlayerAnimationController script :**
   ```
   Player Movement: [Auto-trouvé] ou glisser le Player
   Animator: [Auto-trouvé] ou glisser l'Animator
   Speed Damp Time: 0.1
   Direction Damp Time: 0.1
   ```

Le script s'auto-configure normalement, mais vous pouvez vérifier.

---

## 5. Vérification Complète

### Checklist Player Setup

#### GameObject Player (racine)
- [ ] CharacterController présent
- [ ] PlayerInput présent avec PlayerInputActions assigné
- [ ] PlayerController présent
- [ ] PlayerMovement présent
- [ ] PlayerCameraController présent
- [ ] CameraFollowTarget créé (enfant)
- [ ] CM vcam_Player créé (enfant)
- [ ] GroundCheck créé (enfant)

#### GameObject PlayerModel (enfant)
- [ ] Animator présent avec PlayerAnimatorController assigné
- [ ] PlayerAnimationController présent
- [ ] Apply Root Motion = NO

#### Scene
- [ ] Main Camera à la racine (pas dans Player)
- [ ] CinemachineBrain sur Main Camera
- [ ] Prefab Player dans la scène

### Test Final

1. **Play mode**
2. **WASD** → Le personnage bouge et joue l'animation de course
3. **Souris** → La caméra tourne autour du personnage
4. **Space** → Le personnage saute (si animation de saut configurée)
5. **Left Shift** → Le personnage sprinte (vitesse augmentée)
6. **Escape** → Le curseur se déverrouille

---

## 6. Dépannage

### Le personnage ne bouge pas

**Vérifier :**
1. CharacterController est présent
2. PlayerInput a le PlayerInputActions assigné
3. Les actions "Move" sont bien configurées dans PlayerInputActions
4. Le curseur est verrouillé (cliquer dans la Game View)

**Console :**
```
[PlayerMovement] PlayerCameraController manquant...
```
→ S'assurer que PlayerCameraController est sur le même GameObject

### Les animations ne jouent pas

**Vérifier :**
1. Animator Controller est assigné sur l'Animator
2. Les paramètres (Speed, IsGrounded, etc.) existent dans l'Animator
3. Les transitions entre états sont correctes
4. PlayerAnimationController trouve bien le PlayerMovement

**Console :**
```
Animator manquant sur PlayerModel...
```
→ Ajouter le composant Animator

### La caméra ne tourne pas

**Vérifier :**
1. CinemachineBrain est sur la Main Camera
2. CM vcam_Player existe et est configuré
3. Le curseur est verrouillé
4. L'action "Look" est bien bindée à `<Mouse>/delta`

**Console :**
```
[PlayerCameraController] Cinemachine Camera créée automatiquement...
```
→ Tout devrait fonctionner

### La caméra traverse les murs

**Solution :**
1. Sélectionner CM vcam_Player
2. `Add Extension > Cinemachine Collider`
3. Configurer les layers de collision

---

## 7. Prochaines étapes

### Améliorer les animations

1. **Créer un Blend Tree 2D** pour la locomotion directionnelle
2. **Ajouter des animations de saut** (jump start, in air, land)
3. **Ajouter des animations de combat**
4. **Configurer l'Avatar Mask** pour les animations partielles

### Ajouter des fonctionnalités

1. **Système de combat** (attaques, combos)
2. **Système d'inventaire**
3. **Interactions** avec l'environnement
4. **UI** (barre de vie, stamina, etc.)
5. **Système de sauvegarde**

### Optimiser les performances

1. **LOD** pour le modèle du personnage
2. **Object Pooling** pour les effets
3. **Occlusion Culling** pour la scène

---

## 8. Résumé des fichiers créés

```
Assets/Project/
├── Settings/
│   └── PlayerInputActions.inputactions ✅ (existe déjà)
│
├── Animations/
│   └── PlayerAnimatorController.controller ⚠️ (à créer)
│
└── Scripts/
    └── Player/
        ├── PlayerController.cs ✅
        ├── Movement/
        │   └── PlayerMovement.cs ✅
        ├── Camera/
        │   └── PlayerCameraController.cs ✅
        └── Animation/
            └── PlayerAnimationController.cs ✅
```

---

## 9. Scripts nécessaires - Résumé

### ✅ Scripts que vous DEVEZ garder

| Script | Fichier | Raison |
|--------|---------|--------|
| CharacterController | (Unity built-in) | Physique du personnage |
| PlayerInput | (Unity Input System) | Capture des inputs |
| PlayerController | PlayerController.cs | Orchestre tout |
| PlayerMovement | PlayerMovement.cs | Mouvement physique |
| PlayerCameraController | PlayerCameraController.cs | Contrôle caméra |
| Animator | (Unity built-in) | Joue les animations |
| PlayerAnimationController | PlayerAnimationController.cs | Gère les animations |

### ⚠️ Scripts à supprimer (si présents)

| Script | Raison |
|--------|--------|
| PlayerCamera.cs | Remplacé par PlayerCameraController |

---

## 10. Architecture Logicielle

```
┌─────────────────────────────────────────────────────────────┐
│                       Unity Input System                     │
│                    (Keyboard, Mouse, Gamepad)                │
└──────────────────────────┬──────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│                      PlayerInput Component                   │
│                  (PlayerInputActions.inputactions)           │
└──────────────────────────┬──────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│                      PlayerController.cs                     │
│                   (Orchestre tous les composants)            │
└─────────┬──────────────────────────────────┬────────────────┘
          │                                  │
          ▼                                  ▼
┌──────────────────────────┐    ┌──────────────────────────────┐
│   PlayerMovement.cs      │    │  PlayerCameraController.cs   │
│  - HandleMovement()      │    │  - HandleCameraRotation()    │
│  - CalculateMoveDirection│    │  - GetCameraForward()        │
│  - ApplyGravity()        │    │  - GetCameraRight()          │
└──────────┬───────────────┘    └──────────┬───────────────────┘
           │                               │
           │                               ▼
           │                    ┌────────────────────────────────┐
           │                    │  CinemachineCamera             │
           │                    │  - Follow Target               │
           │                    │  - ThirdPersonFollow           │
           │                    └────────────────────────────────┘
           │
           ▼
┌──────────────────────────────────────────────────────────────┐
│              CharacterController (Unity)                      │
│                 - Move()                                      │
│                 - Physics & Collisions                        │
└───────────────────────────┬──────────────────────────────────┘
                            │
                            ▼
┌──────────────────────────────────────────────────────────────┐
│           PlayerAnimationController.cs                        │
│           - UpdateAnimationParameters()                       │
│           - TriggerJump(), SetSprinting()                     │
└───────────────────────────┬──────────────────────────────────┘
                            │
                            ▼
┌──────────────────────────────────────────────────────────────┐
│                    Animator (Unity)                           │
│              - PlayerAnimatorController.controller            │
│              - Plays animations                               │
└──────────────────────────────────────────────────────────────┘
```

---

## 11. FAQ

### Pourquoi tant de scripts séparés ?

**Séparation des responsabilités (SOLID principles) :**
- **PlayerController** : Gère les inputs et coordonne
- **PlayerMovement** : Gère UNIQUEMENT le mouvement physique
- **PlayerCameraController** : Gère UNIQUEMENT la caméra
- **PlayerAnimationController** : Gère UNIQUEMENT les animations

**Avantages :**
- Code plus facile à maintenir
- Possibilité de tester chaque composant séparément
- Possibilité de réutiliser les composants
- Moins de bugs (chaque script a une seule responsabilité)

### Puis-je tout mettre dans un seul script ?

Techniquement oui, mais **fortement déconseillé** :
- ❌ Code très long (1000+ lignes)
- ❌ Difficile à déboguer
- ❌ Difficile à modifier
- ❌ Impossible à réutiliser
- ❌ Travail en équipe compliqué

**Notre architecture est une best practice Unity !**

### Quelle est la différence entre Animator et PlayerAnimationController ?

- **Animator** : Composant Unity qui joue les animations
- **PlayerAnimationController** : Script custom qui met à jour les paramètres de l'Animator basé sur le gameplay

**Analogie :**
- Animator = Le moteur d'une voiture
- PlayerAnimationController = Le conducteur qui contrôle le moteur

---

**Version :** 1.0.0
**Dernière mise à jour :** 2025-11-16
**Auteur :** Claude AI
**Licence :** Apache License 2.0
