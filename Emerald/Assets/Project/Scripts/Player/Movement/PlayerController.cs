using UnityEngine;

/// <summary>
/// Contrôleur principal du joueur.
/// Gère la capture des entrées utilisateur et orchestre les composants
/// de mouvement et de caméra.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // ==== COMPOSANTS ====
    private CharacterController characterController;
    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;

    // ==== VARIABLES D'ENTRÉE ====
    private Vector2 movementInput;
    private Vector2 cameraInput;
    private bool jumpPressed;
    private bool sprintPressed;

    /// <summary>
    /// Initialisation des références aux composants.
    /// Appelé automatiquement par Unity au démarrage.
    /// </summary>
    private void Awake()
    {
        // Récupération du CharacterController (ajouté automatiquement)
        characterController = GetComponent<CharacterController>();

        // Récupération des composants de mouvement et caméra
        playerMovement = GetComponent<PlayerMovement>();
        playerCamera = GetComponentInChildren<PlayerCamera>();

        // Vérification de la présence des composants requis
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement manquant sur " + gameObject.name);
        }

        if (playerCamera == null)
        {
            Debug.LogError("PlayerCamera manquant sur " + gameObject.name);
        }
    }

    /// <summary>
    /// Initialisation du curseur.
    /// </summary>
    private void Start()
    {
        // Verrouiller et masquer le curseur pour une expérience immersive
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Capture les entrées utilisateur à chaque frame.
    /// Update() est appelé une fois par frame par Unity.
    /// </summary>
    private void Update()
    {
        CaptureInputs();

        // Gestion de la touche Échap pour déverrouiller le curseur
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock();
        }
    }

    /// <summary>
    /// Mise à jour physique à intervalle fixe.
    /// FixedUpdate() garantit une fréquence constante pour la physique.
    /// </summary>
    private void FixedUpdate()
    {
        // Délégation du mouvement au composant PlayerMovement
        if (playerMovement != null)
        {
            playerMovement.HandleMovement(movementInput, sprintPressed, jumpPressed);
            jumpPressed = false; // Réinitialiser après traitement
        }
    }

    /// <summary>
    /// Mise à jour de la caméra après tous les autres calculs.
    /// LateUpdate() est appelé après Update() et FixedUpdate().
    /// </summary>
    private void LateUpdate()
    {
        // Délégation de la rotation de caméra au composant PlayerCamera
        if (playerCamera != null)
        {
            playerCamera.HandleCameraRotation(cameraInput);
        }
    }

    /// <summary>
    /// Capture toutes les entrées clavier et souris.
    /// </summary>
    private void CaptureInputs()
    {
        // Entrées de déplacement (ZQSD / WASD)
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D ou Q/D
        float vertical = Input.GetAxisRaw("Vertical");     // W/S ou Z/S
        movementInput = new Vector2(horizontal, vertical).normalized;

        // Entrées de caméra (mouvement de la souris)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        cameraInput = new Vector2(mouseX, mouseY);

        // Détection du saut (Espace)
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }

        // Détection du sprint (Shift gauche maintenu)
        sprintPressed = Input.GetKey(KeyCode.LeftShift);
    }

    /// <summary>
    /// Bascule entre curseur verrouillé et curseur libre.
    /// </summary>
    private void ToggleCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}