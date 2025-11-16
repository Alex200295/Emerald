using UnityEngine;

/// <summary>
/// Contrôleur de caméra simple third-person.
/// Gère la rotation de la caméra basée sur les entrées du joueur.
/// Version simplifiée sans Cinemachine.
/// </summary>
public class PlayerCameraController : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Transform cameraFollowTarget;
    [SerializeField] private Camera mainCamera;

    [Header("Sensibilité de la souris")]
    [SerializeField] private float mouseSensitivityX = 2f;
    [SerializeField] private float mouseSensitivityY = 2f;

    [Header("Limites de rotation verticale")]
    [SerializeField] private float minVerticalAngle = -40f;
    [SerializeField] private float maxVerticalAngle = 80f;

    [Header("Configuration de la caméra")]
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float cameraHeight = 2f;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0.5f, 0f, 0f);
    [SerializeField] private float cameraSmoothSpeed = 10f;

    // Variables de rotation
    private float rotationX = 0f;
    private float rotationY = 0f;

    /// <summary>
    /// Initialisation des références.
    /// </summary>
    private void Awake()
    {
        // Auto-récupération de la Main Camera si non assignée
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Auto-création du CameraFollowTarget si absent
        if (cameraFollowTarget == null)
        {
            GameObject followTarget = new GameObject("CameraFollowTarget");
            followTarget.transform.SetParent(transform);
            followTarget.transform.localPosition = new Vector3(0f, 1.6f, 0f); // Hauteur des yeux
            followTarget.transform.localRotation = Quaternion.identity;
            cameraFollowTarget = followTarget.transform;

            Debug.Log($"CameraFollowTarget créé automatiquement à {cameraFollowTarget.localPosition}");
        }
    }

    /// <summary>
    /// Initialisation de la rotation.
    /// </summary>
    private void Start()
    {
        if (cameraFollowTarget != null)
        {
            Vector3 angles = cameraFollowTarget.eulerAngles;
            rotationX = angles.y;
            rotationY = angles.x;
        }
    }

    /// <summary>
    /// Gère la rotation de la caméra basée sur l'entrée de la souris.
    /// </summary>
    /// <param name="lookInput">Vecteur de mouvement de la souris</param>
    public void HandleCameraRotation(Vector2 lookInput)
    {
        if (cameraFollowTarget == null) return;

        // Calculer les rotations basées sur l'entrée de la souris
        rotationX += lookInput.x * mouseSensitivityX;
        rotationY -= lookInput.y * mouseSensitivityY;

        // Limiter la rotation verticale
        rotationY = Mathf.Clamp(rotationY, minVerticalAngle, maxVerticalAngle);

        // Appliquer la rotation au CameraFollowTarget
        cameraFollowTarget.rotation = Quaternion.Euler(rotationY, rotationX, 0f);

        // Positionner la caméra
        UpdateCameraPosition();
    }

    /// <summary>
    /// Met à jour la position de la caméra pour suivre le joueur.
    /// </summary>
    private void UpdateCameraPosition()
    {
        if (mainCamera == null || cameraFollowTarget == null) return;

        // Calculer la position cible de la caméra
        Vector3 targetPosition = cameraFollowTarget.position
            + cameraFollowTarget.up * cameraHeight
            + cameraFollowTarget.right * cameraOffset.x
            + cameraFollowTarget.up * cameraOffset.y
            - cameraFollowTarget.forward * cameraDistance;

        // Interpoler la position pour un mouvement fluide
        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position,
            targetPosition,
            cameraSmoothSpeed * Time.deltaTime
        );

        // Faire regarder la caméra vers le follow target
        mainCamera.transform.LookAt(cameraFollowTarget.position + cameraFollowTarget.up * cameraHeight);
    }

    /// <summary>
    /// Obtient la direction avant de la caméra (utilisé pour le mouvement).
    /// </summary>
    public Vector3 GetCameraForward()
    {
        if (cameraFollowTarget != null)
        {
            Vector3 forward = cameraFollowTarget.forward;
            forward.y = 0f;
            return forward.normalized;
        }
        return Vector3.forward;
    }

    /// <summary>
    /// Obtient la direction droite de la caméra (utilisé pour le mouvement).
    /// </summary>
    public Vector3 GetCameraRight()
    {
        if (cameraFollowTarget != null)
        {
            Vector3 right = cameraFollowTarget.right;
            right.y = 0f;
            return right.normalized;
        }
        return Vector3.right;
    }

    /// <summary>
    /// Visualisation de la configuration dans l'éditeur.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (cameraFollowTarget != null)
        {
            // Dessiner la position du follow target
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(cameraFollowTarget.position, 0.3f);

            // Dessiner la direction de vue
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(cameraFollowTarget.position, cameraFollowTarget.forward * 2f);

            // Dessiner la position de la caméra
            if (mainCamera != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(cameraFollowTarget.position, mainCamera.transform.position);
                Gizmos.DrawWireSphere(mainCamera.transform.position, 0.2f);
            }
        }
    }
}
