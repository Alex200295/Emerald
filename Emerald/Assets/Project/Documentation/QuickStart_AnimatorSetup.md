# Quick Start - Configuration de l'Animator Controller

## Création rapide en 5 minutes

### Étape 1 : Créer l'Animator Controller (30 secondes)

1. **Dans Unity**, naviguer vers `/Assets/Project/Animations/`
   - Si le dossier n'existe pas, le créer : Clic droit > Create > Folder
2. **Clic droit** dans le dossier
3. **Create > Animator Controller**
4. **Nommer** : `PlayerAnimatorController`

### Étape 2 : Créer les paramètres (1 minute)

1. **Double-cliquer** sur `PlayerAnimatorController`
2. Dans l'**Animator window**, onglet **Parameters**, cliquer **+**
3. Ajouter les 4 paramètres suivants :

| Clic + | Type | Nom | Valeur |
|--------|------|-----|--------|
| Float | Float | Speed | 0 |
| Bool | Bool | IsGrounded | true |
| Trigger | Trigger | Jump | - |
| Bool | Bool | IsSprinting | false |

### Étape 3 : Ajouter les animations (2 minutes)

#### Pour un personnage féminin (HumanF)

1. **Naviguer** vers `/Assets/Kevin Iglesias/Human Animations/Animations/Female/`

2. **State Idle** (état par défaut)
   - Glisser `Idles/HumanF@Idle01.fbx` dans l'Animator
   - Il devient automatiquement orange (état par défaut)

3. **State Run**
   - Glisser `Movement/Run/HumanF@Run01_Forward.fbx` dans l'Animator
   - Un nouveau state bleu apparaît

#### Pour un personnage masculin (HumanM)

Même procédure avec `/Male/` au lieu de `/Female/`

### Étape 4 : Créer les transitions (1 minute)

1. **Clic droit** sur **Idle** → Make Transition → Cliquer sur **Run**
   - Dans l'Inspector :
     ```
     Has Exit Time: NO ❌
     Transition Duration: 0.15
     ```
   - **Conditions** : Cliquer **+**
     - `Speed` Greater `0.1`

2. **Clic droit** sur **Run** → Make Transition → Cliquer sur **Idle**
   - Dans l'Inspector :
     ```
     Has Exit Time: NO ❌
     Transition Duration: 0.15
     ```
   - **Conditions** : Cliquer **+**
     - `Speed` Less `0.1`

### Étape 5 : Assigner à l'Animator (30 secondes)

1. **Sélectionner le GameObject "PlayerModel"** dans votre prefab Player
2. **Dans le composant Animator** :
   - **Controller** : Glisser `PlayerAnimatorController`
   - **Avatar** : Devrait se remplir automatiquement
   - **Apply Root Motion** : ❌ **NO** (très important !)

### ✅ C'est terminé !

Lancer le jeu :
- Le personnage devrait jouer l'animation Idle quand immobile
- Le personnage devrait jouer l'animation Run quand vous bougez (WASD)

---

## Configuration Avancée (Optionnel)

### Ajouter l'animation de saut

**Animations disponibles :**
- Jump Start (si disponible dans le pack)
- Jump Loop (en l'air)
- Jump Land (atterrissage)

**Dans l'Animator :**

1. **Créer un state "Jump"**
   - Glisser l'animation de saut

2. **Transition Idle/Run → Jump**
   - Condition : `Jump` (trigger)

3. **Transition Jump → Idle**
   - Has Exit Time : YES ✅
   - Exit Time : 0.9 (90% de l'animation)

### Ajouter le sprint (animation différente)

Si vous avez une animation de sprint séparée :

1. **Créer un state "Sprint"**
   - Glisser l'animation de sprint

2. **Transitions**
   - Run → Sprint : `IsSprinting` = true, `Speed` > 0.1
   - Sprint → Run : `IsSprinting` = false

---

## Dépannage

### Les animations ne jouent pas

**Vérifications :**
1. **Animator Controller** est bien assigné ?
2. **Les 4 paramètres** existent bien dans l'Animator ?
3. **PlayerAnimationController script** est sur PlayerModel ?
4. **Apply Root Motion** est sur NO ?

### Le personnage glisse sans bouger les jambes

**Cause :** Les transitions ont **Has Exit Time = YES**
**Solution :** Mettre **Has Exit Time = NO** sur toutes les transitions (sauf Jump)

### Le personnage reste en Idle même quand il bouge

**Vérifications :**
1. **La transition Idle → Run** existe ?
2. **Condition** : `Speed` Greater `0.1` ?
3. **Has Exit Time** = NO ?

**Dans la Console Unity :**
```
Speed = 0.0  ← Le personnage ne bouge pas
Speed = 0.8  ← Le personnage court
```

Si Speed reste à 0 :
- Vérifier que `PlayerMovement` fonctionne
- Vérifier que `PlayerAnimationController` trouve `PlayerMovement`

---

## Schéma de l'Animator (Simple)

```
┌─────────────────┐
│                 │
│      Idle       │ ◄─── État par défaut (Orange)
│   @Idle01.fbx   │
│                 │
└────────┬────────┘
         │
         │ Speed > 0.1
         ▼
┌─────────────────┐
│                 │
│       Run       │
│ @Run01_Forward  │
│                 │
└────────┬────────┘
         │
         │ Speed < 0.1
         │
         └─────────────► Retour à Idle
```

---

## Schéma Avancé avec Jump et Sprint

```
                   Jump (trigger)
                        │
                        ▼
         ┌──────────────────────────┐
         │         Jump             │
         │  (Exit Time: 90%)        │
         └──────────┬───────────────┘
                    │
        ┌───────────┴───────────┐
        │                       │
        ▼                       ▼
┌──────────────┐         ┌──────────────┐
│              │         │              │
│     Idle     │ ◄────── │     Run      │
│              │         │              │
└──────┬───────┘         └───────┬──────┘
       │    ▲                 │  ▲
       │    │ Speed < 0.1     │  │ IsSprinting = false
       │    │                 │  │
       │    └─────────────────┘  │
       │                         │
       │ Speed > 0.1             │
       │ IsSprinting = true      │
       │                         │
       └─────────────►┌──────────┴──────┐
                      │                 │
                      │     Sprint      │
                      │                 │
                      └─────────────────┘
```

---

## Fichiers du pack Kevin Iglesias

### Animations de base (Female)

```
Kevin Iglesias/Human Animations/Animations/Female/
├── Idles/
│   └── HumanF@Idle01.fbx                    ← IDLE
│
├── Movement/Run/
│   ├── HumanF@Run01_Forward.fbx             ← RUN FORWARD
│   ├── HumanF@Run01_Backward.fbx            ← RUN BACKWARD
│   ├── HumanF@Run01_Left.fbx                ← RUN LEFT
│   ├── HumanF@Run01_Right.fbx               ← RUN RIGHT
│   ├── HumanF@Run01_ForwardLeft.fbx         ← DIAGONALES
│   ├── HumanF@Run01_ForwardRight.fbx
│   ├── HumanF@Run01_BackwardLeft.fbx
│   └── HumanF@Run01_BackwardRight.fbx
│
└── Combat/
    ├── HumanF@CombatIdle01.fbx              ← COMBAT IDLE
    ├── HumanF@CombatDamage01.fbx            ← DAMAGED
    ├── HumanF@Death01.fbx                   ← DEATH
    ├── 1H/
    │   ├── HumanF@CombatIdle1H01.fbx
    │   ├── HumanF@Attack1H01_R.fbx
    │   └── HumanF@Attack1H01_L.fbx
    ├── 2H/
    │   ├── HumanF@CombatIdle2H01.fbx
    │   └── HumanF@Attack2H01.fbx
    └── ...
```

### Animations de base (Male)

Même structure dans `/Male/` au lieu de `/Female/`

---

## Prochaines étapes

### 1. Blend Tree pour mouvement directionnel

Au lieu d'une seule animation Run Forward, utilisez toutes les directions :
- Forward, Backward, Left, Right
- ForwardLeft, ForwardRight, BackwardLeft, BackwardRight

**Avantage :** Le personnage joue l'animation correspondant à sa direction de mouvement.

### 2. Animations de combat

- Ajouter des states pour les attaques
- Utiliser les animations 1H, 2H, Shield, etc.
- Créer un système de combos

### 3. Animations de mort et dégâts

- State "Damaged" pour les hits
- State "Death" avec transition finale
- Pas de retour depuis Death (état final)

---

**Temps total : ~5 minutes**
**Difficulté : ★☆☆☆☆** (Débutant)

Pour plus de détails, consultez **SystemSetup_Guide.md**.
