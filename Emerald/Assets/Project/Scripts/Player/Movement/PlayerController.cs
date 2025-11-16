using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Contrôleur principal du joueur.
/// Gère la capture des entrées utilisateur et orchestre les composants
/// de mouvement et de caméra.
/// Utilise le nouveau Unity Input System.
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // ==== COMPOSANTS ====
    private CharacterController characterController;
    private PlayerMovement playerMovement;
    private PlayerCameraController cameraController;
    private PlayerInput playerInput;
    private PlayerAnimationController animationController;

    // ==== VARIABLES D'ENTRÉE ====
    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool jumpPressed;
    private bool isSprinting;

    // ==== INPUT ACTIONS ====
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction toggleCursorAction;

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
        cameraController = GetComponent<PlayerCameraController>();
        playerInput = GetComponent<PlayerInput>();
        animationController = GetComponentInChildren<PlayerAnimationController>();

        // Vérification de la présence des composants requis
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement manquant sur " + gameObject.name);
        }

        if (cameraController == null)
        {
            Debug.LogError("PlayerCameraController manquant sur " + gameObject.name);
        }

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput manquant sur " + gameObject.name);
        }

        // Configuration des Input Actions
        SetupInputActions();

        // Abonnement aux événements
        SubscribeToEvents();
    }

    /// <summary>
    /// S'abonne aux événements du jeu.
    /// </summary>
    private void SubscribeToEvents()
    {
        EventManager.Instance.Subscribe<PlayerJumpEvent>(OnPlayerJump);
        EventManager.Instance.Subscribe<PlayerSprintStartEvent>(OnPlayerSprintStart);
        EventManager.Instance.Subscribe<PlayerSprintStopEvent>(OnPlayerSprintStop);
    }

    /// <summary>
    /// Callback pour l'événement de saut.
    /// </summary>
    private void OnPlayerJump(PlayerJumpEvent evt)
    {
        if (animationController != null)
        {
            animationController.TriggerJump();
        }
    }

    /// <summary>
    /// Callback pour le début du sprint.
    /// </summary>
    private void OnPlayerSprintStart(PlayerSprintStartEvent evt)
    {
        if (animationController != null)
        {
            animationController.SetSprinting(true);
        }
    }

    /// <summary>
    /// Callback pour la fin du sprint.
    /// </summary>
    private void OnPlayerSprintStop(PlayerSprintStopEvent evt)
    {
        if (animationController != null)
        {
            animationController.SetSprinting(false);
        }
    }

    /// <summary>
    /// Configure les Input Actions et leurs callbacks.
    /// </summary>
    private void SetupInputActions()
    {
        if (playerInput == null) return;

        // Récupération des actions depuis le PlayerInput
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];
        toggleCursorAction = playerInput.actions["ToggleCursor"];

        // Abonnement aux événements
        if (jumpAction != null)
        {
            jumpAction.performed += OnJumpPerformed;
        }

        if (sprintAction != null)
        {
            sprintAction.performed += OnSprintPerformed;
            sprintAction.canceled += OnSprintCanceled;
        }

        if (toggleCursorAction != null)
        {
            toggleCursorAction.performed += OnToggleCursorPerformed;
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
            playerMovement.HandleMovement(movementInput, isSprinting, jumpPressed);
            jumpPressed = false; // Réinitialiser après traitement
        }
    }

    /// <summary>
    /// Mise à jour de la caméra après tous les autres calculs.
    /// LateUpdate() est appelé après Update() et FixedUpdate().
    /// </summary>
    private void LateUpdate()
    {
        // Délégation de la rotation de caméra au composant PlayerCameraController
        if (cameraController != null)
        {
            cameraController.HandleCameraRotation(lookInput);
        }
    }

    /// <summary>
    /// Capture toutes les entrées depuis le nouveau Input System.
    /// </summary>
    private void CaptureInputs()
    {
        if (moveAction != null)
        {
            movementInput = moveAction.ReadValue<Vector2>();
        }

        if (lookAction != null)
        {
            lookInput = lookAction.ReadValue<Vector2>();
        }
    }

    // ==== CALLBACKS DES INPUT ACTIONS ====

    /// <summary>
    /// Callback appelé lorsque le joueur appuie sur la touche de saut.
    /// </summary>
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        jumpPressed = true;
    }

    /// <summary>
    /// Callback appelé lorsque le joueur commence à sprinter.
    /// </summary>
    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        isSprinting = true;
    }

    /// <summary>
    /// Callback appelé lorsque le joueur arrête de sprinter.
    /// </summary>
    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        isSprinting = false;
    }

    /// <summary>
    /// Callback appelé lorsque le joueur bascule le curseur.
    /// </summary>
    private void OnToggleCursorPerformed(InputAction.CallbackContext context)
    {
        ToggleCursorLock();
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

    /// <summary>
    /// Désabonnement des événements lors de la destruction.
    /// </summary>
    private void OnDestroy()
    {
        // Désabonnement des Input Actions
        if (jumpAction != null)
        {
            jumpAction.performed -= OnJumpPerformed;
        }

        if (sprintAction != null)
        {
            sprintAction.performed -= OnSprintPerformed;
            sprintAction.canceled -= OnSprintCanceled;
        }

        if (toggleCursorAction != null)
        {
            toggleCursorAction.performed -= OnToggleCursorPerformed;
        }

        // Désabonnement des événements du jeu
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Unsubscribe<PlayerJumpEvent>(OnPlayerJump);
            EventManager.Instance.Unsubscribe<PlayerSprintStartEvent>(OnPlayerSprintStart);
            EventManager.Instance.Unsubscribe<PlayerSprintStopEvent>(OnPlayerSprintStop);
        }
    }
}
