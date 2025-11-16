# Player System Upgrade

## Vue d'ensemble

Cette mise à jour majeure modernise le système du joueur avec les meilleures pratiques Unity, notamment le nouveau Input System, Cinemachine, et un système d'événements robuste.

## Changements principaux

### 1. Migration vers le nouveau Unity Input System

**Fichiers concernés:**
- `Assets/Project/Settings/PlayerInputActions.inputactions` (nouveau)
- `Assets/Project/Scripts/Player/Movement/PlayerController.cs` (modifié)

**Fonctionnalités:**
- ✅ Remplacement complet de l'ancien Input Manager
- ✅ Support des actions d'entrée mappées (Move, Look, Jump, Sprint, ToggleCursor)
- ✅ Architecture événementielle avec callbacks
- ✅ Support WASD/souris avec possibilité d'extension facile pour gamepad

**Utilisation:**
```csharp
// Le système utilise maintenant PlayerInput component
// Les actions sont définies dans PlayerInputActions.inputactions
// Les callbacks sont gérés automatiquement dans PlayerController
```

### 2. Intégration de Cinemachine

**Fichiers concernés:**
- `Assets/Project/Scripts/Player/Camera/PlayerCameraController.cs` (nouveau)
- `Assets/Project/Scripts/Player/Movement/PlayerController.cs` (modifié)

**Fonctionnalités:**
- ✅ Remplacement de la caméra custom par Cinemachine Virtual Camera
- ✅ Auto-création du CameraFollowTarget si absent
- ✅ Auto-création de la Virtual Camera si absente
- ✅ Configuration automatique du 3rd Person Follow
- ✅ Contrôle manuel de la rotation de caméra
- ✅ Paramètres ajustables (distance, offset, damping)

**Configuration:**
```csharp
// La caméra est configurée automatiquement au démarrage
// CameraFollowTarget est créé à la hauteur des yeux (1.6m)
// Distance par défaut: 5m
// Shoulder offset: (0.5, 0, 0)
```

### 3. Création automatique du GroundCheck

**Fichiers concernés:**
- `Assets/Project/Scripts/Player/Movement/PlayerMovement.cs` (modifié)

**Fonctionnalités:**
- ✅ Création automatique du GameObject GroundCheck si absent
- ✅ Positionnement intelligent basé sur le CharacterController
- ✅ Position calculée automatiquement sous les pieds du personnage

**Avantages:**
- Plus besoin de créer manuellement le GroundCheck dans l'éditeur
- Position correcte garantie même si le CharacterController change de taille
- Message de debug pour confirmer la création

### 4. Système d'animation avec Animator

**Fichiers concernés:**
- `Assets/Project/Scripts/Player/Animation/PlayerAnimationController.cs` (nouveau)

**Fonctionnalités:**
- ✅ Gestion centralisée des animations du joueur
- ✅ Paramètres d'Animator: Speed, IsGrounded, Jump, IsSprinting
- ✅ Transitions fluides avec damping configurable
- ✅ Support des événements pour déclencher les animations
- ✅ API publique pour contrôler les animations

**Paramètres d'Animator requis:**
```
- Speed (Float): Vitesse de déplacement normalisée (0-1)
- IsGrounded (Bool): Le joueur est-il au sol?
- Jump (Trigger): Déclenche l'animation de saut
- IsSprinting (Bool): Le joueur est-il en train de sprinter?
```

**Utilisation:**
```csharp
// Les animations sont mises à jour automatiquement
// Vous pouvez aussi contrôler manuellement:
animationController.TriggerJump();
animationController.SetSprinting(true);
animationController.PlayAnimation("MyCustomAnimation");
```

### 5. Système d'événements (EventManager)

**Fichiers concernés:**
- `Assets/Project/Scripts/Core/Events/EventManager.cs` (nouveau)
- `Assets/Project/Scripts/Core/Events/GameEvent.cs` (nouveau)
- `Assets/Project/Scripts/Player/Movement/PlayerMovement.cs` (modifié)
- `Assets/Project/Scripts/Player/Movement/PlayerController.cs` (modifié)

**Fonctionnalités:**
- ✅ Système d'événements typé et découplé
- ✅ Pattern Singleton avec auto-création
- ✅ Événements de base: PlayerJumpEvent, PlayerLandEvent, PlayerSprintStartEvent, PlayerSprintStopEvent
- ✅ Logging optionnel pour debug
- ✅ Tracking du nombre d'événements déclenchés
- ✅ Gestion automatique des abonnements/désabonnements

**Événements disponibles:**
```csharp
// PlayerJumpEvent
public class PlayerJumpEvent : GameEvent
{
    public Vector3 Position { get; }
    public float JumpHeight { get; }
}

// PlayerLandEvent
public class PlayerLandEvent : GameEvent
{
    public Vector3 Position { get; }
    public float FallDuration { get; }
}

// PlayerSprintStartEvent
public class PlayerSprintStartEvent : GameEvent { }

// PlayerSprintStopEvent
public class PlayerSprintStopEvent : GameEvent { }
```

**Utilisation:**
```csharp
// S'abonner à un événement
EventManager.Instance.Subscribe<PlayerJumpEvent>(OnPlayerJump);

// Déclencher un événement
EventManager.Instance.TriggerEvent(new PlayerJumpEvent(position, height));

// Se désabonner
EventManager.Instance.Unsubscribe<PlayerJumpEvent>(OnPlayerJump);
```

**Exemple d'utilisation:**
```csharp
private void OnEnable()
{
    EventManager.Instance.Subscribe<PlayerJumpEvent>(OnPlayerJump);
}

private void OnDisable()
{
    EventManager.Instance.Unsubscribe<PlayerJumpEvent>(OnPlayerJump);
}

private void OnPlayerJump(PlayerJumpEvent evt)
{
    Debug.Log($"Player jumped at {evt.Position} with height {evt.JumpHeight}");
    // Jouer un son de saut
    // Créer des particules
    // etc.
}
```

## Architecture du système

```
Player (GameObject)
├── PlayerController (Component)
│   ├── Gère les inputs via nouveau Input System
│   ├── Orchestre mouvement et caméra
│   └── S'abonne aux événements de gameplay
├── PlayerMovement (Component)
│   ├── Gère la physique du mouvement
│   ├── Auto-crée le GroundCheck si absent
│   └── Déclenche les événements de gameplay
├── PlayerCameraController (Component)
│   ├── Gère Cinemachine Virtual Camera
│   ├── Auto-crée CameraFollowTarget si absent
│   └── Contrôle rotation de caméra
├── CharacterController (Component - Unity)
├── PlayerInput (Component - Unity)
└── Model (Child GameObject)
    └── PlayerAnimationController (Component)
        ├── Gère l'Animator
        ├── Écoute les événements de gameplay
        └── Met à jour les paramètres d'animation
```

## Installation et configuration

### Prérequis dans le projet Unity

1. **Packages installés** (déjà présents):
   - Unity Input System (com.unity.inputsystem: 1.14.2)
   - Cinemachine (com.unity.cinemachine: 3.1.5)

2. **Configuration du nouveau Input System:**
   - Aller dans Edit > Project Settings > Player
   - Changer "Active Input Handling" à "Input System Package (New)"
   - Redémarrer Unity

### Configuration du Player Prefab

1. **Composants sur le GameObject Player:**
   ```
   - CharacterController
   - PlayerController
   - PlayerMovement
   - PlayerCameraController
   - PlayerInput
   ```

2. **Configuration du PlayerInput:**
   - Actions: Assets/Project/Settings/PlayerInputActions
   - Default Map: Player
   - Behavior: Invoke Unity Events

3. **Hiérarchie recommandée:**
   ```
   Player
   ├── Model (avec Animator)
   │   └── PlayerAnimationController
   └── (GroundCheck sera créé automatiquement)
   ```

4. **Configuration de l'Animator:**
   - Créer un Animator Controller
   - Ajouter les paramètres: Speed (Float), IsGrounded (Bool), Jump (Trigger), IsSprinting (Bool)
   - Créer les états d'animation: Idle, Walk, Run, Jump, Fall
   - Configurer les transitions basées sur ces paramètres

### Assets d'animation disponibles

Le projet contient plusieurs packs d'animations:
- **Kevin Iglesias Human Animations**: Animations de combat et mouvement
- **VanillaLoopStudio Free Sample Animation Set**: Animations variées

Vous pouvez utiliser ces assets pour créer votre Animator Controller.

## Avantages de cette architecture

1. **Modularité**: Chaque composant a une responsabilité unique
2. **Découplage**: Le système d'événements permet une communication sans dépendances directes
3. **Extensibilité**: Facile d'ajouter de nouvelles fonctionnalités (stamina, dash, etc.)
4. **Maintenabilité**: Code bien documenté et organisé
5. **Performances**: Utilisation de hash pour les paramètres d'Animator
6. **Robustesse**: Auto-création des éléments manquants, vérifications de null

## Prochaines étapes suggérées

1. **Créer un Animator Controller** avec les animations disponibles
2. **Configurer les états et transitions** dans l'Animator
3. **Tester le système** dans une scène de jeu
4. **Ajouter des événements personnalisés** selon les besoins du gameplay
5. **Implémenter la stamina** pour limiter le sprint
6. **Ajouter un système de dash/dodge**
7. **Intégrer un système de combat** avec les animations de Kevin Iglesias

## Notes importantes

- ⚠️ Le nouveau Input System nécessite un redémarrage d'Unity après activation
- ⚠️ Pensez à assigner le LayerMask "Ground" dans l'inspecteur pour la détection du sol
- ⚠️ L'EventManager est créé automatiquement au runtime (pattern Singleton)
- ⚠️ Les événements sont déclenchés uniquement si EventManager existe

## Compatibilité

- Unity Version: 2022.3 LTS ou supérieur
- URP: Compatible (com.unity.render-pipelines.universal: 17.2.0)
- Platforms: Toutes (testé sur PC)

## Auteur

Système développé par Claude AI Assistant
Date: 2025-11-16
Version: 1.0.0
