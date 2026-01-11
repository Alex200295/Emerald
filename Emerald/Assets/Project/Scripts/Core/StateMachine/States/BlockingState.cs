using UnityEngine;

/// <summary>
/// État de blocage - réduit les dégâts reçus et consomme de l'endurance.
/// </summary>
public class BlockingState : State
{
    private const string BLOCK_IDLE_ANIM = "Block_Idle";
    private const string BLOCK_HIT_ANIM = "Block_Hit";
    private const float DAMAGE_REDUCTION = 0.75f; // Réduit 75% des dégâts

    public BlockingState(CombatStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        // Jouer l'animation de blocage
        animator.CrossFade(BLOCK_IDLE_ANIM, 0.1f);

        // Ralentir le mouvement pendant le blocage
        controller.PlayerMovement.SpeedMultiplier = 0.5f;
    }

    public override void OnUpdate()
    {
        // Consommer de l'endurance chaque frame
        if (controller.staminaConfig != null)
        {
            float cost = controller.staminaConfig.blockCostPerSecond * Time.deltaTime;

            if (!controller.TryConsumeStamina(cost))
            {
                // Plus d'endurance, retour à idle
                controller.ChangeState(controller.idleState);
            }
        }
    }

    public override void OnBlockStop()
    {
        // Le joueur relâche le bouton de blocage
        controller.ChangeState(controller.idleState);
    }

    public override void OnDodge()
    {
        // Peut esquiver depuis le blocage
        if (controller.staminaConfig != null)
        {
            if (controller.TryConsumeStamina(controller.staminaConfig.dodgeCost))
            {
                controller.ChangeState(controller.dodgingState);
            }
        }
    }

    public override void OnHurt(float damage, Vector3 hitDirection)
    {
        // Réduire les dégâts
        float reducedDamage = damage * (1f - DAMAGE_REDUCTION);

        // Consommer de l'endurance supplémentaire sur hit
        float staminaDrain = damage * 0.5f;
        if (!controller.TryConsumeStamina(staminaDrain))
        {
            // Guard break - stagger
            controller.ChangeState(controller.staggeredState);
            return;
        }

        // Jouer l'animation de block hit
        animator.CrossFade(BLOCK_HIT_ANIM, 0.05f);

        // Appliquer un léger knockback
        var rb = controller.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(hitDirection * 3f, ForceMode.Impulse);
        }

        // Ne pas appeler base.OnHurt car on gère le hit nous-mêmes
    }

    public override void OnExit()
    {
        // Restaurer la vitesse normale
        controller.PlayerMovement.SpeedMultiplier = 1f;
    }
}