using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Indicateur UI qui suit la cible verrouillée à l'écran.
/// </summary>
public class LockOnIndicatorUI : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private LockOnSystem lockOnSystem;
    [SerializeField] private Image indicatorImage;

    [Header("Configuration")]
    [SerializeField] private float smoothSpeed = 15f;
    [SerializeField] private Vector2 offset = Vector2.zero;

    private Camera mainCamera;
    private RectTransform rectTransform;
    private Canvas parentCanvas;

    private void Awake()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();

        if (indicatorImage == null)
        {
            indicatorImage = GetComponent<Image>();
        }

        if (indicatorImage != null)
        {
            indicatorImage.enabled = false;
        }

    }

    private void LateUpdate()
    {
        if (lockOnSystem == null)
        {
            if (indicatorImage != null)
                indicatorImage.enabled = false;
            return;
        }

        // Afficher/cacher selon l'état du lock-on
        if (lockOnSystem.IsLockedOn && lockOnSystem.CurrentTarget != null)
        {
            if (indicatorImage != null && !indicatorImage.enabled)
            {
                indicatorImage.enabled = true;
            }

            UpdatePosition();
        }
        else
        {
            if (indicatorImage != null && indicatorImage.enabled)
            {
                indicatorImage.enabled = false;
            }
        }
    }

    private void UpdatePosition()
    {
        // Convertir position world vers screen
        Vector3 targetWorldPos = lockOnSystem.GetTargetPosition();
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetWorldPos);

        // Ignorer si derrière la caméra
        if (screenPos.z < 0)
        {
            gameObject.SetActive(false);
            return;
        }

        // Convertir screen vers canvas space
        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            screenPos,
            parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
            out canvasPos
        );

        // Appliquer offset
        canvasPos += offset;

        // Smooth movement
        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            canvasPos,
            Time.deltaTime * smoothSpeed
        );
    }

    /// <summary>
    /// Configure le système de lock-on à suivre.
    /// </summary>
    public void SetLockOnSystem(LockOnSystem system)
    {
        lockOnSystem = system;
    }
}