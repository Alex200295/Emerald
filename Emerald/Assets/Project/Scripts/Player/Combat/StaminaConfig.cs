using UnityEngine;

/// <summary>
/// Configuration globale du système d'endurance.
/// Permet d'ajuster facilement le balancing sans modifier le code.
/// </summary>
[CreateAssetMenu(fileName = "New Stamina Config", menuName = "Combat/Stamina Config")]
public class StaminaConfig : ScriptableObject
{
    [Header("Capacité")]
    [Tooltip("Endurance maximum du joueur")]
    [Range(50f, 200f)]
    public float maxStamina = 100f;

    [Header("Régénération")]
    [Tooltip("Points d'endurance régénérés par seconde")]
    [Range(5f, 50f)]
    public float regenRate = 20f;

    [Tooltip("Délai en secondes avant que la régénération commence après consommation")]
    [Range(0f, 3f)]
    public float regenDelay = 1f;

    [Tooltip("Multiplicateur de régénération hors combat (idle)")]
    [Range(1f, 3f)]
    public float outOfCombatRegenMultiplier = 1.5f;

    [Header("Coûts des actions")]
    [Tooltip("Coût d'une esquive/roulade")]
    [Range(10f, 50f)]
    public float dodgeCost = 25f;

    [Tooltip("Coût du blocage par seconde")]
    [Range(1f, 10f)]
    public float blockCostPerSecond = 5f;

    [Tooltip("Coût du sprint par seconde")]
    [Range(1f, 15f)]
    public float sprintCostPerSecond = 10f;

    [Header("État d'épuisement")]
    [Tooltip("Seuil d'endurance en-dessous duquel le joueur est considéré épuisé")]
    [Range(0f, 30f)]
    public float exhaustedThreshold = 10f;

    [Tooltip("Durée de la pénalité d'épuisement en secondes")]
    [Range(1f, 5f)]
    public float exhaustedPenaltyDuration = 2f;

    [Tooltip("Multiplicateur de vitesse de mouvement pendant l'épuisement")]
    [Range(0.3f, 0.8f)]
    public float exhaustedSpeedMultiplier = 0.5f;

    [Header("Feedback visuel")]
    [Tooltip("Couleur de la barre d'endurance quand pleine")]
    public Color staminaFullColor = Color.green;

    [Tooltip("Couleur de la barre d'endurance quand basse")]
    public Color staminaLowColor = Color.yellow;

    [Tooltip("Couleur de la barre d'endurance quand épuisée")]
    public Color staminaEmptyColor = Color.red;

    [Tooltip("Effet visuel joué quand le joueur est épuisé")]
    public GameObject exhaustedVFXPrefab;

    [Header("Effets sonores")]
    [Tooltip("Son joué quand l'endurance devient basse")]
    public AudioClip staminaLowSound;

    [Tooltip("Son joué quand le joueur est épuisé")]
    public AudioClip exhaustedSound;

    // Méthodes utilitaires

    /// <summary>
    /// Vérifie si une valeur d'endurance est considérée comme basse.
    /// </summary>
    public bool IsStaminaLow(float currentStamina)
    {
        return currentStamina <= (maxStamina * 0.3f);
    }

    /// <summary>
    /// Vérifie si le joueur est épuisé.
    /// </summary>
    public bool IsExhausted(float currentStamina)
    {
        return currentStamina <= exhaustedThreshold;
    }

    /// <summary>
    /// Obtient la couleur de la barre d'endurance selon le pourcentage.
    /// </summary>
    public Color GetStaminaBarColor(float currentStamina)
    {
        if (IsExhausted(currentStamina))
            return staminaEmptyColor;

        if (IsStaminaLow(currentStamina))
            return staminaLowColor;

        return staminaFullColor;
    }
}