using UnityEngine;

/// <summary>
/// �tat d'esquive - fournit des i-frames et un dash rapide.
/// </summary>
public class DodgingState : State
{
    private const string DODGE_ANIM = "Roll";
    private const float DODGE_DURATION = 0.6f;
    private const float DODGE_SPEED = 12f;
    private const float IFRAME_DURATION = 0.4f; // Invincibilit� pendant 0.4s

    private float dodgeStartTime;
    private Vector3 dodgeDirection;
    private Rigidbody rb;

    public DodgingState(CombatStateController controller) : base(controller)
    {
        rb = controller.GetComponent<Rigidbody>();
    }

    public override void OnEnter()
    {
        dodgeStartTime = Time.time;

        // D�terminer la direction de l'esquive
        var moveInput = controller.PlayerMovement.movementInput;

        if (moveInput.magnitude > 0.1f)
        {
            // Esquive dans la direction du mouvement
            dodgeDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            dodgeDirection = controller.transform.TransformDirection(dodgeDirection);
        }
        else
        {
            // Esquive vers l'arri�re si aucun input
            dodgeDirection = -controller.transform.forward;
        }

        // Jouer l'animation
        animator.CrossFade(DODGE_ANIM, 0.05f);

        // D�sactiver le mouvement normal
        controller.PlayerMovement.enabled = false;
    }

    public override void OnFixedUpdate()
    {
        // Appliquer le mouvement de dodge
        if (rb != null)
        {
            rb.linearVelocity = dodgeDirection * DODGE_SPEED;
        }
    }

    public override void OnUpdate()
    {
        float timeSinceStart = Time.time - dodgeStartTime;

        // Fin de l'esquive
        if (timeSinceStart >= DODGE_DURATION)
        {
            controller.ChangeState(controller.idleState);
        }
    }

    public override void OnHurt(float damage, Vector3 hitDirection)
    {
        // I-frames : invincibilit� pendant la dur�e des i-frames
        float timeSinceStart = Time.time - dodgeStartTime;

        if (timeSinceStart <= IFRAME_DURATION)
        {
            return; // Ignore compl�tement les d�g�ts
        }

        // Apr�s les i-frames, comportement normal
        base.OnHurt(damage, hitDirection);
    }

    public override void OnExit()
    {
        controller.PlayerMovement.enabled = true;

        if (rb != null)
            rb.linearVelocity = Vector3.zero;

        // AJOUTE CETTE LIGNE
        animator.CrossFade("Locomotion", 0.2f);
    }
}