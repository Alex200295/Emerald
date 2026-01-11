using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Barre d'endurance avec couleurs configurables depuis StaminaConfig.
/// </summary>
public class StaminaBarUI : MonoBehaviour
{
    [Header("Références UI")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Image backgroundImage;

    [Header("Configuration")]
    [SerializeField] private StaminaConfig staminaConfig;

    [Header("Animation")]
    [SerializeField] private float smoothSpeed = 8f; // Plus rapide que HP

    private float targetFillAmount = 1f;
    private float currentFillAmount = 1f;

    private void Awake()
    {
        if (fillImage == null)
        {
            Debug.LogError("[StaminaBarUI] Fill Image non assigné !");
        }
    }

    private void Update()
    {
        // Animation fluide
        if (Mathf.Abs(currentFillAmount - targetFillAmount) > 0.001f)
        {
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
            fillImage.fillAmount = currentFillAmount;

            // Mettre à jour la couleur
            UpdateColor();
        }
    }

    /// <summary>
    /// Met à jour la barre d'endurance.
    /// </summary>
    public void SetStamina(float currentStamina, float maxStamina)
    {
        if (maxStamina <= 0) return;

        targetFillAmount = Mathf.Clamp01(currentStamina / maxStamina);
    }

    private void UpdateColor()
    {
        if (fillImage == null || staminaConfig == null) return;

        // Utiliser les couleurs du StaminaConfig
        float percentage = currentFillAmount;

        if (staminaConfig.IsExhausted(percentage * staminaConfig.maxStamina))
        {
            fillImage.color = staminaConfig.staminaEmptyColor;
        }
        else if (staminaConfig.IsStaminaLow(percentage * staminaConfig.maxStamina))
        {
            fillImage.color = staminaConfig.staminaLowColor;
        }
        else
        {
            fillImage.color = staminaConfig.staminaFullColor;
        }
    }

    /// <summary>
    /// Configure le StaminaConfig à utiliser.
    /// </summary>
    public void SetStaminaConfig(StaminaConfig config)
    {
        staminaConfig = config;
        UpdateColor();
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}