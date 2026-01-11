using UnityEngine;

/// <summary>
/// Gestionnaire central de l'UI de combat.
/// Gère les barres du joueur et communique avec les systèmes de combat.
/// </summary>
public class CombatUIManager : MonoBehaviour
{
    [Header("Barres du joueur")]
    [SerializeField] private HealthBarUI playerHealthBar;
    [SerializeField] private StaminaBarUI playerStaminaBar;
    [SerializeField] private HealthBarUI playerPoiseBar; // Optionnel

    [Header("Références")]
    [SerializeField] private CombatStateController combatController;

    // Stats du joueur (à connecter avec votre système de stats)
    private float playerMaxHealth = 100f;
    private float playerCurrentHealth = 100f;

    private void Start()
    {
        // Initialiser les barres
        if (playerHealthBar != null)
        {
            playerHealthBar.SetHealthImmediate(playerCurrentHealth, playerMaxHealth);
        }

        if (playerStaminaBar != null && combatController != null)
        {
            playerStaminaBar.SetStaminaConfig(combatController.staminaConfig);
            playerStaminaBar.SetStamina(combatController.CurrentStamina, combatController.staminaConfig.maxStamina);
        }

        if (playerPoiseBar != null && combatController != null)
        {
            playerPoiseBar.SetHealthImmediate(combatController.CurrentPoise, 100f);
        }
    }

    private void Update()
    {
        UpdatePlayerUI();
    }

    private void UpdatePlayerUI()
    {
        if (combatController == null) return;

        // Mettre à jour l'endurance
        if (playerStaminaBar != null && combatController.staminaConfig != null)
        {
            playerStaminaBar.SetStamina(combatController.CurrentStamina, combatController.staminaConfig.maxStamina);
        }

        // Mettre à jour la poise
        if (playerPoiseBar != null)
        {
            playerPoiseBar.SetHealth(combatController.CurrentPoise, 100f);
        }
    }

    /// <summary>
    /// Met à jour la vie du joueur. Appelez depuis votre système de santé.
    /// </summary>
    public void UpdatePlayerHealth(float current, float max)
    {
        playerCurrentHealth = current;
        playerMaxHealth = max;

        if (playerHealthBar != null)
        {
            playerHealthBar.SetHealth(current, max);
        }
    }

    /// <summary>
    /// Initialise les valeurs de santé. Appelez au démarrage du jeu.
    /// </summary>
    public void InitializePlayerHealth(float maxHealth)
    {
        playerMaxHealth = maxHealth;
        playerCurrentHealth = maxHealth;

        if (playerHealthBar != null)
        {
            playerHealthBar.SetHealthImmediate(maxHealth, maxHealth);
        }
    }
}