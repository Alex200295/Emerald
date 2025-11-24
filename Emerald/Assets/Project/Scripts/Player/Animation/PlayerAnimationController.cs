using UnityEngine;

/// <summary>
/// Contrôleur d'animations du joueur.
/// Gère les transitions d'animations basées sur l'état du mouvement.
/// Utilise VelocityX et VelocityZ pour un Blend Tree 2D directionnel.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator animator;

    [Header("Paramètres d'animation")]
    [SerializeField] private float velocityDampTime = 0.1f;

    // Hash des paramètres d'animation pour optimisation
    // MODIFIÉ : Utilisation de VelocityX et VelocityZ au lieu de Speed
    private static readonly int VelocityXHash = Animator.StringToHash("VelocityX");
    private static readonly int VelocityZHash = Animator.StringToHash("VelocityZ");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int IsSprintingHash = Animator.StringToHash("IsSprinting");

    private bool hasAnimator = false;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (playerMovement == null)
        {
            playerMovement = GetComponentInParent<PlayerMovement>();
        }

        if (animator != null)
        {
            hasAnimator = true;
        }
        else
        {
            Debug.LogWarning("Animator manquant sur " + gameObject.name);
        }
    }

    private void Update()
    {
        if (!hasAnimator || playerMovement == null) return;

        UpdateAnimationParameters();
    }

    /// <summary>
    /// MODIFIÉ : Met à jour VelocityX et VelocityZ pour le Blend Tree 2D.
    /// </summary>
    private void UpdateAnimationParameters()
    {
        // Récupérer les vélocités locales depuis PlayerMovement
        float velocityX = playerMovement.VelocityX;
        float velocityZ = playerMovement.VelocityZ;

        // Envoyer les valeurs à l'Animator avec lissage (damping)
        animator.SetFloat(VelocityXHash, velocityX, velocityDampTime, Time.deltaTime);
        animator.SetFloat(VelocityZHash, velocityZ, velocityDampTime, Time.deltaTime);

        // État au sol
        animator.SetBool(IsGroundedHash, playerMovement.IsGrounded);
    }

    public void TriggerJump()
    {
        if (hasAnimator)
        {
            animator.SetTrigger(JumpHash);
        }
    }

    public void SetSprinting(bool isSprinting)
    {
        if (hasAnimator)
        {
            animator.SetBool(IsSprintingHash, isSprinting);
        }
    }

    public void PlayAnimation(string stateName, int layer = 0)
    {
        if (hasAnimator)
        {
            animator.Play(stateName, layer);
        }
    }

    public void SetBool(string paramName, bool value)
    {
        if (hasAnimator)
        {
            animator.SetBool(paramName, value);
        }
    }

    public void SetFloat(string paramName, float value)
    {
        if (hasAnimator)
        {
            animator.SetFloat(paramName, value);
        }
    }

    public void SetTrigger(string paramName)
    {
        if (hasAnimator)
        {
            animator.SetTrigger(paramName);
        }
    }

    public void ResetTrigger(string paramName)
    {
        if (hasAnimator)
        {
            animator.ResetTrigger(paramName);
        }
    }
}