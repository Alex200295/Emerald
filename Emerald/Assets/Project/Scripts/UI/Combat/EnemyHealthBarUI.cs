using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Barre de vie en world-space qui suit un ennemi.
/// </summary>
public class EnemyHealthBarUI : MonoBehaviour
{
    [Header("Références UI")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Canvas canvas;

    [Header("Configuration")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2.5f, 0);
    [SerializeField] private bool hideWhenFull = true;
    [SerializeField] private float hideDelay = 3f; // Secondes avant de cacher si pleine

    [Header("Couleurs")]
    [SerializeField] private Gradient healthGradient;

    private Transform target;
    private Camera mainCamera;
    private float currentFillAmount = 1f;
    private float lastDamageTime;

    private void Awake()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("[EnemyHealthBarUI] Camera.main est null ! Vérifiez que votre caméra a le tag 'MainCamera'");
        }

        // Créer gradient par défaut
        if (healthGradient == null || healthGradient.colorKeys.Length == 0)
        {
            healthGradient = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0] = new GradientColorKey(Color.red, 0f);
            colorKeys[1] = new GradientColorKey(Color.green, 1f);

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0] = new GradientAlphaKey(1f, 0f);
            alphaKeys[1] = new GradientAlphaKey(1f, 1f);

            healthGradient.SetKeys(colorKeys, alphaKeys);
        }

        // Configurer le canvas en world space
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.WorldSpace;

            if(hideWhenFull) 
            {
                canvas.enabled = false;
            }
        }

       
    }

    private void LateUpdate()
    {
        if (target == null || mainCamera == null) return;

        // Suivre la cible avec offset
        transform.position = target.position + offset;

        // Toujours faire face à la caméra
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);

        // Cacher si pleine et délai écoulé
        if (hideWhenFull && currentFillAmount >= 0.99f)
        {
            if (Time.time - lastDamageTime > hideDelay)
            {
                canvas.enabled = false;
            }
        }
    }

    /// <summary>
    /// Configure la cible à suivre et positionne immédiatement.
    /// </summary>
    public void SetTarget(Transform enemyTransform)
    {
        target = enemyTransform;

        // Position immédiate
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    /// <summary>
    /// Met à jour la barre de vie.
    /// </summary>
    public void SetHealth(float current, float max)
    {
        if (max <= 0) return;

        currentFillAmount = Mathf.Clamp01(current / max);

        if (fillImage != null)
        {
            fillImage.fillAmount = currentFillAmount;
            fillImage.color = healthGradient.Evaluate(currentFillAmount);
        }

        // Afficher et reset le timer
        if (canvas != null)
        {
            if (currentFillAmount < 0.99f)
            {
                canvas.enabled = true;
                lastDamageTime = Time.time;
            }
        }
    }

    public void SetVisible(bool visible)
    {
        if (canvas != null)
        {
            canvas.enabled = visible;
        }
    }
}