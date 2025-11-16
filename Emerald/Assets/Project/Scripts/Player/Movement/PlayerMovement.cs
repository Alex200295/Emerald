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
    private Transform cameraTransform;

    // ==== VARIABLES D'ÉTAT ====
    private Vector3 velocity;
    private bool isGrounded;

    /// <summary>
    /// Initialisation des références.
    /// </summary>
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    /// <summary>
    /// Traite le mouvement du joueur basé sur les entrées.
    /// </summary>
    /// <param name="input">Vecteur de direction normalisé</param>
    /// <param name="isSprinting">Le joueur sprinte-t-il?</param>
    /// <param name="jump">Le joueur demande-t-il un saut?</param>
    public void HandleMovement(Vector2 input, bool isSprinting, bool jump)
    {
        // Vérification si le personnage est au sol
        CheckGrounded();

        // Calcul de la direction de mouvement relative à la caméra
        Vector3 moveDirection = CalculateMoveDirection(input);

        // Appliquer le mouvement horizontal
        ApplyMovement(moveDirection, isSprinting);

        // Faire pivoter le personnage dans la direction du mouvement
        if (moveDirection.magnitude >= 0.1f)
        {
            RotateCharacter(moveDirection);
        }

        // Gérer le saut
        if (jump && isGrounded)
        {
            Jump();
        }

        // Appliquer la gravité
        ApplyGravity();
    }

    /// <summary>
    /// Vérifie si le personnage est au sol via une sphère de détection.
    /// </summary>
    private void CheckGrounded()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }

        // Réinitialiser la vélocité verticale si au sol
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Petite valeur négative pour maintenir le contact
        }
    }

    /// <summary>
    /// Calcule la direction de mouvement relative à la caméra.
    /// </summary>
    /// <param name="input">Entrée directionnelle (normalisée)</param>
    /// <returns>Direction mondiale du mouvement</returns>
    private Vector3 CalculateMoveDirection(Vector2 input)
    {
        // Obtenir la direction avant de la caméra (ignorant l'axe Y)
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Projeter sur le plan horizontal (Y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Calculer la direction désirée
        Vector3 moveDirection = forward * input.y + right * input.x;
        return moveDirection;
    }

    /// <summary>
    /// Applique le mouvement au CharacterController.
    /// </summary>
    /// <param name="direction">Direction du mouvement</param>
    /// <param name="isSprinting">Utiliser la vitesse de sprint?</param>
    private void ApplyMovement(Vector3 direction, bool isSprinting)
    {
        // Choisir la vitesse appropriée
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // Appliquer le mouvement horizontal
        Vector3 movement = direction * currentSpeed * Time.fixedDeltaTime;

        // Appliquer le mouvement vertical (gravité/saut)
        movement.y = velocity.y * Time.fixedDeltaTime;

        // Déplacer le personnage
        controller.Move(movement);
    }

    /// <summary>
    /// Fait pivoter le personnage vers la direction du mouvement.
    /// </summary>
    /// <param name="direction">Direction cible</param>
    private void RotateCharacter(Vector3 direction)
    {
        // Calculer la rotation cible
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Interpoler vers la rotation cible
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }

    /// <summary>
    /// Exécute un saut en appliquant une vélocité verticale.
    /// </summary>
    private void Jump()
    {
        // Formule : v = sqrt(h * -2 * g)
        // où h = hauteur de saut désirée, g = gravité
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    /// <summary>
    /// Applique la gravité au personnage.
    /// </summary>
    private void ApplyGravity()
    {
        velocity.y += gravity * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Visualisation de la sphère de détection du sol dans l'éditeur.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}