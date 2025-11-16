using UnityEngine;

/// <summary>
/// Contrôleur d'animations du joueur.
/// Gère les transitions d'animations basées sur l'état du mouvement.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator animator;

    [Header("Paramètres d'animation")]
    [SerializeField] private float speedDampTime = 0.1f;
    [SerializeField] private float directionDampTime = 0.1f;

    // Hash des paramètres d'animation pour optimisation
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int IsSprintingHash = Animator.StringToHash("IsSprinting");

    private bool hasAnimator = false;

    /// <summary>
    /// Initialisation des références.
    /// </summary>
    private void Awake()
    {
        // Auto-récupération de l'Animator si non assigné
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Auto-récupération du PlayerMovement si non assigné
        if (playerMovement == null)
        {
            playerMovement = GetComponentInParent<PlayerMovement>();
        }

        // Vérification de la présence de l'Animator
        if (animator != null)
        {
            hasAnimator = true;
        }
        else
        {
            Debug.LogWarning("Animator manquant sur " + gameObject.name + ". Les animations ne seront pas jouées.");
        }
    }

    /// <summary>
    /// Met à jour les paramètres de l'Animator basés sur l'état du mouvement.
    /// </summary>
    private void Update()
    {
        if (!hasAnimator || playerMovement == null) return;

        UpdateAnimationParameters();
    }

    /// <summary>
    /// Met à jour tous les paramètres de l'Animator.
    /// </summary>
    private void UpdateAnimationParameters()
    {
        // Vitesse de déplacement normalisée (0-1)
        float normalizedSpeed = Mathf.Clamp01(playerMovement.CurrentSpeed / 8f);
        animator.SetFloat(SpeedHash, normalizedSpeed, speedDampTime, Time.deltaTime);

        // État au sol
        animator.SetBool(IsGroundedHash, playerMovement.IsGrounded);
    }

    /// <summary>
    /// Déclenche l'animation de saut.
    /// Appelé par des événements de gameplay.
    /// </summary>
    public void TriggerJump()
    {
        if (hasAnimator)
        {
            animator.SetTrigger(JumpHash);
        }
    }

    /// <summary>
    /// Définit l'état de sprint.
    /// </summary>
    /// <param name="isSprinting">Le joueur sprinte-t-il?</param>
    public void SetSprinting(bool isSprinting)
    {
        if (hasAnimator)
        {
            animator.SetBool(IsSprintingHash, isSprinting);
        }
    }

    /// <summary>
    /// Joue une animation spécifique par son nom.
    /// </summary>
    /// <param name="stateName">Nom de l'état d'animation</param>
    /// <param name="layer">Layer de l'Animator (défaut: 0)</param>
    public void PlayAnimation(string stateName, int layer = 0)
    {
        if (hasAnimator)
        {
            animator.Play(stateName, layer);
        }
    }

    /// <summary>
    /// Définit un paramètre booléen de l'Animator.
    /// </summary>
    public void SetBool(string paramName, bool value)
    {
        if (hasAnimator)
        {
            animator.SetBool(paramName, value);
        }
    }

    /// <summary>
    /// Définit un paramètre float de l'Animator.
    /// </summary>
    public void SetFloat(string paramName, float value)
    {
        if (hasAnimator)
        {
            animator.SetFloat(paramName, value);
        }
    }

    /// <summary>
    /// Déclenche un trigger de l'Animator.
    /// </summary>
    public void SetTrigger(string paramName)
    {
        if (hasAnimator)
        {
            animator.SetTrigger(paramName);
        }
    }

    /// <summary>
    /// Réinitialise un trigger de l'Animator.
    /// </summary>
    public void ResetTrigger(string paramName)
    {
        if (hasAnimator)
        {
            animator.ResetTrigger(paramName);
        }
    }
}
