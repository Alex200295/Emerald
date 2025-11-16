using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// Contrôleur de caméra utilisant Cinemachine 3.x.
/// Gère la rotation de la caméra virtuelle basée sur les entrées du joueur.
/// Compatible avec Cinemachine Camera (CM3).
/// Auto-configure le CameraFollowTarget et la Cinemachine Camera si nécessaire.
/// </summary>
public class PlayerCameraController : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Transform cameraFollowTarget;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [Header("Sensibilité de la souris")]
    [SerializeField] private float mouseSensitivityX = 2f;
    [SerializeField] private float mouseSensitivityY = 2f;

    [Header("Limites de rotation verticale")]
    [SerializeField] private float minVerticalAngle = -40f;
    [SerializeField] private float maxVerticalAngle = 80f;

    [Header("Configuration Cinemachine (si auto-créée)")]
    [SerializeField] private float cameraFollowTargetHeight = 1.35f; // Hauteur au niveau des épaules pour un style RPG immersif
    [SerializeField] private float cameraDistance = 4.5f;
    [SerializeField] private Vector3 shoulderOffset = new Vector3(0.6f, -0.1f, 0f);
    [SerializeField] private float verticalArmLength = 0.3f;
    [SerializeField] private float cameraSide = 1f;

    // Variables de rotation
    private float rotationX = 0f;
    private float rotationY = 0f;

    /// <summary>
    /// Initialisation des références.
    /// </summary>
    private void Awake()
    {
        // Auto-création du CameraFollowTarget si absent
        if (cameraFollowTarget == null)
        {
            GameObject followTarget = new GameObject("CameraFollowTarget");
            followTarget.transform.SetParent(transform);
            followTarget.transform.localPosition = new Vector3(0f, cameraFollowTargetHeight, 0f); // Hauteur configurable (épaules par défaut)
            followTarget.transform.localRotation = Quaternion.identity;
            cameraFollowTarget = followTarget.transform;

            Debug.Log($"[PlayerCameraController] CameraFollowTarget créé automatiquement à hauteur {cameraFollowTargetHeight}m (niveau épaules)");
        }

        // Auto-recherche de la Cinemachine Camera si non assignée
        if (cinemachineCamera == null)
        {
            cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();

            if (cinemachineCamera == null)
            {
                Debug.LogWarning("[PlayerCameraController] Aucune Cinemachine Camera trouvée dans la scène. Création automatique...");
                CreateAndConfigureCinemachineCamera();
            }
            else
            {
                Debug.Log($"[PlayerCameraController] Cinemachine Camera trouvée automatiquement : {cinemachineCamera.name}");
                ConfigureCinemachineCamera();
            }
        }
        else
        {
            ConfigureCinemachineCamera();
        }
    }

    /// <summary>
    /// Configure la Cinemachine Camera pour suivre le joueur.
    /// Configure également le Body (3rd Person Follow) et l'Aim.
    /// </summary>
    private void ConfigureCinemachineCamera()
    {
        if (cinemachineCamera == null || cameraFollowTarget == null) return;

        // Définir le Follow et LookAt
        cinemachineCamera.Follow = cameraFollowTarget;
        cinemachineCamera.LookAt = cameraFollowTarget;

        // Configuration du Body (3rd Person Follow)
        var thirdPersonFollow = cinemachineCamera.GetComponent<CinemachineThirdPersonFollow>();
        if (thirdPersonFollow == null)
        {
            thirdPersonFollow = cinemachineCamera.gameObject.AddComponent<CinemachineThirdPersonFollow>();
            Debug.Log("[PlayerCameraController] CinemachineThirdPersonFollow ajouté à la caméra");
        }

        // Appliquer les paramètres de distance et position
        thirdPersonFollow.CameraDistance = cameraDistance;
        thirdPersonFollow.ShoulderOffset = shoulderOffset;
        thirdPersonFollow.VerticalArmLength = verticalArmLength;
        thirdPersonFollow.CameraSide = cameraSide;
        thirdPersonFollow.Damping = new Vector3(0.1f, 0.25f, 0.3f);

        Debug.Log($"[PlayerCameraController] Cinemachine Camera configurée pour suivre {cameraFollowTarget.name}");
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
        // La Cinemachine Camera suivra automatiquement cette rotation
        cameraFollowTarget.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
    }

    /// <summary>
    /// Crée et configure automatiquement une Cinemachine Camera.
    /// La caméra virtuelle est créée comme enfant du Player pour une meilleure organisation.
    /// </summary>
    private void CreateAndConfigureCinemachineCamera()
    {
        // Créer le GameObject de la caméra virtuelle comme enfant du Player
        GameObject vcamObject = new GameObject("CM vcam_Player");
        vcamObject.transform.SetParent(transform);
        vcamObject.transform.localPosition = Vector3.zero;
        vcamObject.transform.localRotation = Quaternion.identity;

        cinemachineCamera = vcamObject.AddComponent<CinemachineCamera>();

        // Configuration par défaut
        cinemachineCamera.Priority.Value = 10;

        Debug.Log("[PlayerCameraController] Cinemachine Camera créée automatiquement comme enfant du Player");

        // Configurer la caméra
        ConfigureCinemachineCamera();
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
        }
    }
}
