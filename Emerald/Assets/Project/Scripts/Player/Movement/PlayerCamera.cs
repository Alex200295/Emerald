using UnityEngine;

/// <summary>
/// Contrôle la caméra troisième personne.
/// Gère la rotation horizontale et verticale basée sur les mouvements de souris.
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    // ==== PARAMÈTRES DE LA CAMÉRA ====
    [Header("Sensibilité de la souris")]
    [SerializeField] private float mouseSensitivityX = 200f;
    [SerializeField] private float mouseSensitivityY = 200f;

    [Header("Limites de rotation verticale")]
    [SerializeField] private float minVerticalAngle = -40f;
    [SerializeField] private float maxVerticalAngle = 80f;

    [Header("Configuration de la caméra")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float cameraHeight = 2f;

    // ==== VARIABLES DE ROTATION ====
    private float rotationX = 0f;
    private float rotationY = 0f;

    // ==== COMPOSANTS ====
    private Transform playerTransform;

    /// <summary>
    /// Initialisation des références.
    /// </summary>
    private void Awake()
    {
        playerTransform = transform.parent;

        if (playerTransform == null)
        {
            Debug.LogError("PlayerCamera doit être enfant du GameObject Player!");
        }
    }

    /// <summary>
    /// Initialisation de la position de la caméra.
    /// </summary>
    private void Start()
    {
        // Initialiser les angles de rotation avec la rotation actuelle
        Vector3 angles = transform.eulerAngles;
        rotationX = angles.y;
        rotationY = angles.x;

        // Positionner la caméra
        UpdateCameraPosition();
    }

    /// <summary>
    /// Gère la rotation de la caméra basée sur l'entrée de la souris.
    /// </summary>
    /// <param name="input">Vecteur de mouvement de la souris</param>
    public void HandleCameraRotation(Vector2 input)
    {
        // Calculer les rotations basées sur l'entrée de la souris
        rotationX += input.x * mouseSensitivityX * Time.deltaTime;
        rotationY -= input.y * mouseSensitivityY * Time.deltaTime;

        // Limiter la rotation verticale (empêche les retournements)
        rotationY = Mathf.Clamp(rotationY, minVerticalAngle, maxVerticalAngle);

        // Appliquer la rotation et la position de la caméra
        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        UpdateCameraPosition();
    }

    /// <summary>
    /// Met à jour la position de la caméra derrière le personnage.
    /// </summary>
    private void UpdateCameraPosition()
    {
        // Déterminer le point cible (si défini, sinon utiliser le joueur)
        Transform target = cameraTarget != null ? cameraTarget : playerTransform;

        if (target != null)
        {
            // Calculer la position de la caméra derrière et au-dessus du joueur
            Vector3 targetPosition = target.position + Vector3.up * cameraHeight;
            Vector3 direction = transform.rotation * -Vector3.forward;
            transform.position = targetPosition + direction * cameraDistance;
        }
    }

    /// <summary>
    /// Visualisation de la configuration de la caméra dans l'éditeur.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (playerTransform != null)
        {
            Transform target = cameraTarget != null ? cameraTarget : playerTransform;

            // Dessiner la position cible
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(target.position + Vector3.up * cameraHeight, 0.5f);

            // Dessiner la distance de la caméra
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(target.position + Vector3.up * cameraHeight, transform.position);
        }
    }
}