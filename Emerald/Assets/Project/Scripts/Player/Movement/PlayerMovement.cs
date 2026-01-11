using UnityEngine;

/// <summary>
/// Gère le mouvement physique du personnage.
/// Contrôle la marche, le sprint, le saut et la gravité.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // ==== PARAMÈTRES DE MOUVEMENT ====
    [Header("Paramètres de déplacement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Paramètres de saut")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Détection du sol")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    // ==== COMPOSANTS ====
    private CharacterController controller;
    private PlayerCameraController cameraController;

    // ==== VARIABLES D'ÉTAT ====
    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;
    private bool wasSprinting;
    private float airTime;
    private float speedMultiplier = 1f;
    private bool isStrafeMode = false;

    // ==== NOUVEAU : VARIABLES POUR ANIMATIONS ====
    private Vector3 localVelocity;  // Vélocité dans l'espace local du personnage
    internal Vector2 movementInput;  // Dernière entrée de mouvement

    /// <summary>
    /// Initialisation des références.
    /// </summary>
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cameraController = GetComponent<PlayerCameraController>();

        if (cameraController == null)
        {
            Debug.LogWarning("[PlayerMovement] PlayerCameraController manquant.");
        }

        if (groundCheck == null)
        {
            CreateGroundCheck();
        }
    }

    private void CreateGroundCheck()
    {
        GameObject groundCheckObject = new GameObject("GroundCheck");
        groundCheckObject.transform.SetParent(transform);

        if (controller != null)
        {
            float controllerHeight = controller.height;
            Vector3 controllerCenter = controller.center;

            groundCheckObject.transform.localPosition = new Vector3(
                controllerCenter.x,
                controllerCenter.y - (controllerHeight / 2f) + 0.1f,
                controllerCenter.z
            );
        }
        else
        {
            groundCheckObject.transform.localPosition = new Vector3(0f, 0.1f, 0f);
        }

        groundCheck = groundCheckObject.transform;
        Debug.Log("GroundCheck créé automatiquement.");
    }

    public void HandleMovement(Vector2 input, bool isSprinting, bool jump)
    {
        // NOUVEAU : Sauvegarder l'entrée de mouvement pour les animations
        movementInput = input;

        CheckGrounded();
        HandleSprintEvents(isSprinting);

        Vector3 moveDirection = CalculateMoveDirection(input);
        ApplyMovement(moveDirection, isSprinting);

        if (moveDirection.magnitude >= 0.1f && !isStrafeMode)
        {
            RotateCharacter(moveDirection);
        }

        if (jump && isGrounded)
        {
            Jump();
        }

        ApplyGravity();

        // NOUVEAU : Calculer la vélocité locale pour les animations
        CalculateLocalVelocity();

        if (!isGrounded)
        {
            airTime += Time.fixedDeltaTime;
        }

        if (isGrounded && !wasGrounded && airTime > 0.1f)
        {
            EventManager.Instance.TriggerEvent(new PlayerLandEvent(transform.position, airTime));
            airTime = 0f;
        }

        wasGrounded = isGrounded;
    }

    /// <summary>
    /// NOUVEAU : Calcule la vélocité dans l'espace local du personnage.
    /// Cette méthode transforme la vitesse mondiale en vitesse relative
    /// à l'orientation du personnage pour les animations.
    /// </summary>
    private void CalculateLocalVelocity()
    {
        // Obtenir la vélocité mondiale du CharacterController
        Vector3 worldVelocity = controller.velocity;

        // Ignorer la composante verticale (Y)
        worldVelocity.y = 0f;

        // Transformer la vélocité mondiale en vélocité locale
        // InverseTransformDirection convertit un vecteur du monde vers l'espace local
        localVelocity = transform.InverseTransformDirection(worldVelocity);
    }

    private void CheckGrounded()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private Vector3 CalculateMoveDirection(Vector2 input)
    {
        Vector3 forward;
        Vector3 right;

        if (cameraController != null)
        {
            forward = cameraController.GetCameraForward();
            right = cameraController.GetCameraRight();
        }
        else
        {
            Transform cameraTransform = Camera.main != null ? Camera.main.transform : transform;
            forward = cameraTransform.forward;
            right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
        }

        Vector3 moveDirection = forward * input.y + right * input.x;
        return moveDirection;
    }

    private void ApplyMovement(Vector3 direction, bool isSprinting)
    {
        float currentSpeed = (isSprinting ? sprintSpeed : walkSpeed) * speedMultiplier;
        Vector3 movement = direction * currentSpeed * Time.fixedDeltaTime;
        movement.y = velocity.y * Time.fixedDeltaTime;
        controller.Move(movement);
    }

    private void RotateCharacter(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        EventManager.Instance.TriggerEvent(new PlayerJumpEvent(transform.position, jumpHeight));
    }

    /// <summary>
    /// Active ou désactive le mode strafe (mouvement latéral).
    /// En mode strafe, le joueur ne tourne pas automatiquement dans la direction du mouvement.
    /// </summary>
    public void SetStrafeMode(bool enabled)
    {
        isStrafeMode = enabled;
    }

    private void HandleSprintEvents(bool isSprinting)
    {
        if (isSprinting && !wasSprinting)
        {
            EventManager.Instance.TriggerEvent(new PlayerSprintStartEvent());
        }
        else if (!isSprinting && wasSprinting)
        {
            EventManager.Instance.TriggerEvent(new PlayerSprintStopEvent());
        }

        wasSprinting = isSprinting;
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.fixedDeltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }

    // ==== NOUVELLES PROPRIÉTÉS PUBLIQUES POUR LES ANIMATIONS ====
    public float SpeedMultiplier
    {
        get => speedMultiplier;
        set => speedMultiplier = Mathf.Max(0f, value);
    }

    /// <summary>
    /// Vitesse de déplacement horizontal (X) dans l'espace local du personnage.
    /// Valeur positive = droite, négative = gauche.
    /// </summary>
    public float VelocityX => localVelocity.x;

    /// <summary>
    /// Vitesse de déplacement avant/arrière (Z) dans l'espace local du personnage.
    /// Valeur positive = avant, négative = arrière.
    /// </summary>
    public float VelocityZ => localVelocity.z;

    public bool IsGrounded => isGrounded;
    public float CurrentSpeed => controller.velocity.magnitude;
}