using UnityEngine;

/// <summary>
/// Définit les propriétés d'une attaque individuelle.
/// Utilisé pour configurer les attaques légères, lourdes et spéciales.
/// </summary>
[CreateAssetMenu(fileName = "New Attack", menuName = "Combat/Attack Data")]
public class AttackData : ScriptableObject
{
    [Header("Identification")]
    [Tooltip("Nom de l'attaque (ex: Light Attack 1, Heavy Slash)")]
    public string attackName = "New Attack";

    [Tooltip("Type d'attaque")]
    public AttackType attackType = AttackType.Light;

    [Header("Animation")]
    [Tooltip("Nom de l'animation dans l'Animator (doit correspondre exactement)")]
    public string animationName;

    [Tooltip("Durée totale de l'animation en secondes")]
    public float animationDuration = 1f;

    [Header("Timing (Frame Data)")]
    [Tooltip("Frames avant que l'attaque devienne active (startup)")]
    [Range(0, 60)]
    public int startupFrames = 8;

    [Tooltip("Frames pendant lesquels l'attaque inflige des dégâts (active frames)")]
    [Range(1, 60)]
    public int activeFrames = 4;

    [Tooltip("Frames de récupération après l'attaque (recovery)")]
    [Range(0, 120)]
    public int recoveryFrames = 20;

    [Header("Dégâts")]
    [Tooltip("Dégâts de base de l'attaque")]
    public float baseDamage = 20f;

    [Tooltip("Type de dégâts (Physique, Feu, Glace, etc.)")]
    public DamageType damageType = DamageType.Physical;

    [Tooltip("Dégâts infligés à la poise de l'ennemi")]
    public float poiseDamage = 15f;

    [Header("Knockback")]
    [Tooltip("Force du knockback appliqué à la cible")]
    public float knockbackForce = 5f;

    [Tooltip("Direction du knockback (0=forward, 1=up)")]
    [Range(0f, 1f)]
    public float knockbackUpwardForce = 0.2f;

    [Header("Endurance")]
    [Tooltip("Coût en endurance pour effectuer cette attaque")]
    public float staminaCost = 15f;

    [Header("Combo System")]
    [Tooltip("Cette attaque peut-elle être annulée par une esquive/blocage?")]
    public bool canBeCanceled = false;

    [Tooltip("Frame à partir de laquelle l'attaque peut être annulée")]
    [Range(0, 120)]
    public int cancelWindowFrame = 15;

    [Tooltip("Attaques possibles en combo après celle-ci")]
    public AttackData[] possibleFollowUps;

    [Tooltip("Durée de la fenêtre de combo en secondes")]
    public float comboWindowDuration = 0.6f;

    [Header("Effets Visuels")]
    [Tooltip("Effet visuel joué au moment de l'impact")]
    public GameObject hitVFXPrefab;

    [Tooltip("Traînée de l'arme pendant l'attaque")]
    public GameObject weaponTrailPrefab;

    [Header("Effets Sonores")]
    [Tooltip("Son joué au début du swing")]
    public AudioClip swingSound;

    [Tooltip("Son joué lors de l'impact")]
    public AudioClip hitSound;

    // Méthodes utilitaires

    /// <summary>
    /// Calcule le temps en secondes avant que l'attaque devienne active.
    /// </summary>
    public float GetStartupTime()
    {
        return startupFrames / 60f; // Conversion frames to seconds à 60 FPS
    }

    /// <summary>
    /// Calcule la durée pendant laquelle l'attaque inflige des dégâts.
    /// </summary>
    public float GetActiveDuration()
    {
        return activeFrames / 60f;
    }

    /// <summary>
    /// Calcule le temps de récupération après l'attaque.
    /// </summary>
    public float GetRecoveryTime()
    {
        return recoveryFrames / 60f;
    }

    /// <summary>
    /// Calcule le temps après lequel l'attaque peut être annulée.
    /// </summary>
    public float GetCancelWindowTime()
    {
        return cancelWindowFrame / 60f;
    }

    /// <summary>
    /// Vérifie si cette attaque peut enchaîner sur une autre attaque spécifique.
    /// </summary>
    public bool CanComboInto(AttackData nextAttack)
    {
        if (possibleFollowUps == null || possibleFollowUps.Length == 0)
            return false;

        foreach (AttackData followUp in possibleFollowUps)
        {
            if (followUp == nextAttack)
                return true;
        }

        return false;
    }
}

/// <summary>
/// Types d'attaques disponibles.
/// </summary>
public enum AttackType
{
    Light,      // Attaque rapide, peu de dégâts
    Heavy,      // Attaque lente, gros dégâts
    Special,    // Attaque spéciale unique
    Charged     // Attaque chargée
}

/// <summary>
/// Types de dégâts pour le système élémentaire.
/// </summary>
public enum DamageType
{
    Physical,   // Dégâts physiques standard
    Fire,       // Dégâts de feu
    Ice,        // Dégâts de glace
    Lightning,  // Dégâts de foudre
    Magic       // Dégâts magiques purs
}