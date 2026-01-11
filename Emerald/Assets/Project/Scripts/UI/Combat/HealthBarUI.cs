using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Composant UI réutilisable pour afficher une barre de vie.
/// </summary>
public class HealthBarUI : MonoBehaviour
{
    [Header("Références UI")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Image backgroundImage;

    [Header("Couleurs")]
    [SerializeField] private Color fullHealthColor = new Color(0.8f, 0.1f, 0.1f); // Rouge foncé
    [SerializeField] private Color lowHealthColor = new Color(1f, 0.3f, 0f); // Orange
    [SerializeField] private Gradient healthGradient;

    [Header("Animation")]
    [SerializeField] private float smoothSpeed = 5f;

    private float targetFillAmount = 1f;
    private float currentFillAmount = 1f;

    private void Awake()
    {
        if (fillImage == null)
        {
            Debug.LogError("[HealthBarUI] Fill Image non assigné !");
        }

        // Créer un gradient par défaut si non configuré
        if (healthGradient == null || healthGradient.colorKeys.Length == 0)
        {
            healthGradient = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[3];
            colorKeys[0] = new GradientColorKey(Color.red, 0f);     // 0% = rouge
            colorKeys[1] = new GradientColorKey(Color.yellow, 0.5f); // 50% = jaune
            colorKeys[2] = new GradientColorKey(Color.green, 1f);   // 100% = vert

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0] = new GradientAlphaKey(1f, 0f);
            alphaKeys[1] = new GradientAlphaKey(1f, 1f);

            healthGradient.SetKeys(colorKeys, alphaKeys);
        }
    }

    private void Update()
    {
        // Animation fluide du fill
        if (Mathf.Abs(currentFillAmount - targetFillAmount) > 0.001f)
        {
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
            fillImage.fillAmount = currentFillAmount;

            // Mettre à jour la couleur
            UpdateColor();
        }
    }

    /// <summary>
    /// Met à jour la barre de vie avec un pourcentage (0-1).
    /// </summary>
    public void SetHealth(float currentHealth, float maxHealth)
    {
        if (maxHealth <= 0)
        {
            Debug.LogWarning("[HealthBarUI] Max health est <= 0");
            return;
        }

        targetFillAmount = Mathf.Clamp01(currentHealth / maxHealth);
    }

    /// <summary>
    /// Met à jour immédiatement sans animation.
    /// </summary>
    public void SetHealthImmediate(float currentHealth, float maxHealth)
    {
        if (maxHealth <= 0) return;

        targetFillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        currentFillAmount = targetFillAmount;
        fillImage.fillAmount = currentFillAmount;
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (fillImage != null)
        {
            fillImage.color = healthGradient.Evaluate(currentFillAmount);
        }
    }

    /// <summary>
    /// Active ou désactive la visibilité de la barre.
    /// </summary>
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}