# Presets de Caméra - PlayerCameraController

## Vue d'ensemble

Ce guide présente des configurations recommandées pour différents styles de jeux. Ajustez les paramètres du `PlayerCameraController` selon le type d'expérience que vous souhaitez créer.

---

## 🎮 Presets par genre

### 1. RPG Immersif (Skyrim / Assassin's Creed) ⭐ **Par défaut**

**Style :** Caméra proche, au niveau des épaules, pour une immersion maximale.

```
Camera Follow Target Height: 1.35
Camera Distance: 4.5
Shoulder Offset: (0.6, -0.1, 0)
Vertical Arm Length: 0.3
Camera Side: 1.0

Mouse Sensitivity X: 2.5
Mouse Sensitivity Y: 2.5
Min Vertical Angle: -40
Max Vertical Angle: 80
```

**Caractéristiques :**
- ✅ Vue au niveau des épaules
- ✅ Distance proche pour l'immersion
- ✅ Offset légèrement vers le bas pour voir le personnage
- ✅ Parfait pour l'exploration et le combat au corps-à-corps

---

### 2. Action/Aventure (The Witcher 3 / God of War)

**Style :** Caméra plus éloignée pour une meilleure vue de l'environnement et des combats.

```
Camera Follow Target Height: 1.4
Camera Distance: 5.5
Shoulder Offset: (0.7, 0, 0)
Vertical Arm Length: 0.5
Camera Side: 1.0

Mouse Sensitivity X: 3.0
Mouse Sensitivity Y: 3.0
Min Vertical Angle: -45
Max Vertical Angle: 70
```

**Caractéristiques :**
- ✅ Vue plus large pour les combats
- ✅ Hauteur légèrement plus élevée
- ✅ Offset d'épaule prononcé
- ✅ Sensibilité augmentée pour des mouvements rapides

---

### 3. Survival Horror (Resident Evil / The Last of Us)

**Style :** Caméra très proche, par-dessus l'épaule, pour tension et claustrophobie.

```
Camera Follow Target Height: 1.45
Camera Distance: 3.5
Shoulder Offset: (0.8, -0.05, 0)
Vertical Arm Length: 0.2
Camera Side: 1.0

Mouse Sensitivity X: 2.0
Mouse Sensitivity Y: 2.0
Min Vertical Angle: -30
Max Vertical Angle: 60
```

**Caractéristiques :**
- ✅ Distance très courte pour la tension
- ✅ Offset d'épaule fort (vue par-dessus l'épaule)
- ✅ Champ de vision réduit
- ✅ Sensibilité modérée pour le contrôle précis

---

### 4. Shooter TPS (Uncharted / Tomb Raider)

**Style :** Caméra optimisée pour le tir et la visée.

```
Camera Follow Target Height: 1.5
Camera Distance: 4.0
Shoulder Offset: (0.9, 0.1, 0)
Vertical Arm Length: 0.25
Camera Side: 1.0

Mouse Sensitivity X: 3.5
Mouse Sensitivity Y: 3.5
Min Vertical Angle: -35
Max Vertical Angle: 75
```

**Caractéristiques :**
- ✅ Hauteur plus élevée pour voir au-dessus des couvertures
- ✅ Offset d'épaule marqué pour la visée
- ✅ Sensibilité élevée pour le tir
- ✅ Bonne visibilité de l'environnement

---

### 5. Platformer 3D (Ratchet & Clank / Crash Bandicoot)

**Style :** Caméra éloignée et haute pour voir les plateformes.

```
Camera Follow Target Height: 1.2
Camera Distance: 6.5
Shoulder Offset: (0.3, 0.3, 0)
Vertical Arm Length: 0.6
Camera Side: 1.0

Mouse Sensitivity X: 2.5
Mouse Sensitivity Y: 2.5
Min Vertical Angle: -50
Max Vertical Angle: 85
```

**Caractéristiques :**
- ✅ Distance très éloignée pour voir les plateformes
- ✅ Hauteur verticale importante
- ✅ Offset réduit pour centrer le personnage
- ✅ Angles verticaux élargis

---

### 6. Exploration Zen (Journey / Abzû)

**Style :** Caméra artistique et cinématique pour l'exploration paisible.

```
Camera Follow Target Height: 1.3
Camera Distance: 6.0
Shoulder Offset: (0.4, 0.2, 0)
Vertical Arm Length: 0.5
Camera Side: 1.0

Mouse Sensitivity X: 1.5
Mouse Sensitivity Y: 1.5
Min Vertical Angle: -60
Max Vertical Angle: 85
```

**Caractéristiques :**
- ✅ Distance large pour apprécier les environnements
- ✅ Sensibilité douce pour des mouvements fluides
- ✅ Angles très ouverts pour observer le monde
- ✅ Parfait pour l'exploration contemplative

---

### 7. Combat Arena (Dark Souls / Monster Hunter)

**Style :** Caméra centrée sur le personnage, optimisée pour le lock-on et les boss fights.

```
Camera Follow Target Height: 1.4
Camera Distance: 5.0
Shoulder Offset: (0.5, 0, 0)
Vertical Arm Length: 0.4
Camera Side: 1.0

Mouse Sensitivity X: 3.0
Mouse Sensitivity Y: 3.0
Min Vertical Angle: -40
Max Vertical Angle: 70
```

**Caractéristiques :**
- ✅ Distance équilibrée pour voir le personnage et les ennemis
- ✅ Offset modéré pour ne pas perdre de vue les boss
- ✅ Sensibilité élevée pour suivre les ennemis rapides
- ✅ Hauteur optimale pour les combats

---

## 📐 Explication des paramètres

### Camera Follow Target Height

**Contrôle :** Hauteur du point que la caméra suit sur le personnage.

| Valeur | Position | Style |
|--------|----------|-------|
| 1.2-1.3 | Bas du torse | Platformer, vue large |
| 1.35-1.45 | Épaules | RPG, Action standard |
| 1.5-1.6 | Yeux/Tête | Shooter, vue précise |

**Impact :**
- Plus bas → Caméra regarde légèrement vers le haut
- Plus haut → Caméra regarde légèrement vers le bas

---

### Camera Distance

**Contrôle :** Distance de la caméra derrière le personnage.

| Valeur | Distance | Style |
|--------|----------|-------|
| 3.0-4.0 | Très proche | Horror, Tension |
| 4.5-5.5 | Standard | RPG, Action |
| 6.0-8.0 | Éloigné | Platformer, Vue large |

**Impact :**
- Plus proche → Plus immersif, champ de vision réduit
- Plus loin → Vue d'ensemble, meilleure conscience spatiale

---

### Shoulder Offset

**Contrôle :** Décalage horizontal (X) et vertical (Y) de la caméra.

**X (Horizontal) :**
| Valeur | Position | Style |
|--------|----------|-------|
| 0.3-0.5 | Légèrement décalé | Centré, neutre |
| 0.6-0.8 | Décalé moyen | RPG, vue standard |
| 0.9-1.2 | Fortement décalé | Shooter, visée |

**Y (Vertical) :**
| Valeur | Position | Style |
|--------|----------|-------|
| -0.2 à -0.1 | Légèrement bas | Voir plus le personnage |
| 0.0 | Niveau | Standard |
| 0.1 à 0.3 | Légèrement haut | Voir par-dessus obstacles |

---

### Vertical Arm Length

**Contrôle :** Longueur du bras vertical de la caméra (contrôle la hauteur finale).

| Valeur | Effet | Style |
|--------|-------|-------|
| 0.2-0.3 | Caméra basse | Vue rase, tension |
| 0.4-0.5 | Standard | Vue équilibrée |
| 0.6-0.8 | Caméra haute | Platformer, vue d'ensemble |

---

### Camera Side

**Contrôle :** Côté de la caméra (gauche/droite).

| Valeur | Position | Usage |
|--------|----------|-------|
| -1.0 | Épaule gauche | Shooter gaucher |
| 0.0 | Centré | Caméra centrée |
| 1.0 | Épaule droite | Standard (droitier) |

**Note :** Dans un vrai jeu, vous pouvez permettre au joueur de basculer entre -1 et 1 avec un bouton.

---

### Mouse Sensitivity

**Contrôle :** Vitesse de rotation de la caméra.

| Valeur | Vitesse | Style |
|--------|---------|-------|
| 1.0-2.0 | Lent | Exploration zen, contrôle précis |
| 2.5-3.5 | Standard | RPG, Action |
| 4.0-6.0 | Rapide | Shooter, combats rapides |

**Recommandation :** Toujours permettre au joueur de personnaliser la sensibilité !

---

### Vertical Angles (Min/Max)

**Contrôle :** Limites de rotation verticale de la caméra.

**Min Vertical Angle (vers le bas) :**
| Valeur | Effet |
|--------|-------|
| -30° à -35° | Limité, empêche de trop regarder en bas |
| -40° à -45° | Standard |
| -50° à -60° | Large, permet de regarder presque à la verticale |

**Max Vertical Angle (vers le haut) :**
| Valeur | Effet |
|--------|-------|
| 60° à 70° | Limité |
| 75° à 80° | Standard |
| 85° à 89° | Large, permet de regarder le ciel |

---

## 🎨 Comment appliquer un preset

### Méthode 1 : Dans l'éditeur Unity

1. Sélectionner le GameObject **Player** avec le composant `PlayerCameraController`
2. Dans l'Inspector, copier les valeurs du preset souhaité
3. Tester en Play mode
4. Ajuster selon vos préférences

### Méthode 2 : Par script (système de presets)

Vous pouvez créer un système de presets en C# :

```csharp
[System.Serializable]
public class CameraPreset
{
    public string name;
    public float cameraFollowTargetHeight;
    public float cameraDistance;
    public Vector3 shoulderOffset;
    public float verticalArmLength;
    public float cameraSide;
    public float mouseSensitivityX;
    public float mouseSensitivityY;
    public float minVerticalAngle;
    public float maxVerticalAngle;
}

// Dans PlayerCameraController, ajouter :
public void ApplyPreset(CameraPreset preset)
{
    cameraFollowTargetHeight = preset.cameraFollowTargetHeight;
    cameraDistance = preset.cameraDistance;
    shoulderOffset = preset.shoulderOffset;
    verticalArmLength = preset.verticalArmLength;
    cameraSide = preset.cameraSide;
    mouseSensitivityX = preset.mouseSensitivityX;
    mouseSensitivityY = preset.mouseSensitivityY;
    minVerticalAngle = preset.minVerticalAngle;
    maxVerticalAngle = preset.maxVerticalAngle;

    // Reconfigurer la caméra
    if (cinemachineCamera != null)
    {
        ConfigureCinemachineCamera();
    }
}
```

---

## 🔧 Conseils d'ajustement

### Trouver la hauteur parfaite

1. **Lancer le jeu** en Play mode
2. **Sélectionner le Player** dans la hiérarchie
3. **Ajuster `Camera Follow Target Height`** en temps réel
4. **Observer** le changement immédiat
5. **Noter la valeur** qui vous plaît
6. **Arrêter Play mode** et réappliquer la valeur

### Test de la distance

La distance idéale dépend de :
- **Taille du personnage** : Plus grand = distance plus grande
- **Niveau de détail** : Détails fins = distance plus courte
- **Type d'environnement** : Espaces ouverts = distance plus grande

### Cohérence du style

Gardez une cohérence dans votre jeu :
- **Exploration** : Une configuration
- **Combat** : Possibilité de zoomer légèrement
- **Cinématiques** : Configuration spéciale (optionnel)

---

## 🎯 Cas d'usage spéciaux

### Basculer entre première et troisième personne

Pour passer en première personne :
```
Camera Distance: 0.1
Shoulder Offset: (0.05, 0.15, 0)
Camera Follow Target Height: 1.6
```

### Vue cinématique large

Pour les cutscenes ou moments dramatiques :
```
Camera Distance: 8.0
Vertical Arm Length: 0.8
Shoulder Offset: (0.2, 0.5, 0)
```

### Vue de visée (aim mode)

Pour un mode de visée précis :
```
Camera Distance: 3.0
Shoulder Offset: (1.2, 0.15, 0)
Mouse Sensitivity X: 2.0
Mouse Sensitivity Y: 2.0
```

---

## 📊 Tableau récapitulatif

| Style | Target Height | Distance | Shoulder X | Vert. Arm | Sensibilité |
|-------|--------------|----------|------------|-----------|-------------|
| **RPG Immersif** | 1.35 | 4.5 | 0.6 | 0.3 | 2.5 |
| Action/Aventure | 1.4 | 5.5 | 0.7 | 0.5 | 3.0 |
| Survival Horror | 1.45 | 3.5 | 0.8 | 0.2 | 2.0 |
| Shooter TPS | 1.5 | 4.0 | 0.9 | 0.25 | 3.5 |
| Platformer 3D | 1.2 | 6.5 | 0.3 | 0.6 | 2.5 |
| Exploration Zen | 1.3 | 6.0 | 0.4 | 0.5 | 1.5 |
| Combat Arena | 1.4 | 5.0 | 0.5 | 0.4 | 3.0 |

---

## 🚀 Recommandations finales

### Pour un RPG immersif (votre cas)

Les valeurs **par défaut** ont été ajustées pour ce style :
- ✅ Hauteur à 1.35 (épaules)
- ✅ Distance à 4.5 (proche mais confortable)
- ✅ Offset d'épaule à (0.6, -0.1, 0)
- ✅ Sensibilité à 2.5 (réactif mais contrôlable)

### Permettre la personnalisation

Dans un jeu professionnel, permettez toujours au joueur d'ajuster :
1. **Sensibilité** de la souris (essentiel)
2. **Distance** de la caméra (confort personnel)
3. **Hauteur** de la caméra (optionnel)
4. **Côté** de l'épaule (gauche/droite)

### Tester avec votre environnement

Ces presets sont des points de départ. Ajustez selon :
- La taille de vos environnements
- La vitesse de votre personnage
- Le style de gameplay
- Les retours des joueurs

---

**Version :** 1.0.0
**Dernière mise à jour :** 2025-11-16
**Style par défaut :** RPG Immersif (Skyrim/Assassin's Creed)
**Auteur :** Claude AI
