using UnityEngine;
using Cinemachine;

/// <summary>
/// Contrôleur de caméra utilisant Cinemachine.
/// Gère la rotation de la caméra virtuelle basée sur les entrées du joueur.
/// </summary>
public class PlayerCameraController : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Transform cameraFollowTarget;

    [Header("Sensibilité de la souris")]
    [SerializeField] private float mouseSensitivityX = 2f;
    [SerializeField] private float mouseSensitivityY = 2f;

    [Header("Limites de rotation verticale")]
    [SerializeField] private float minVerticalAngle = -40f;
    [SerializeField] private float maxVerticalAngle = 80f;

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
