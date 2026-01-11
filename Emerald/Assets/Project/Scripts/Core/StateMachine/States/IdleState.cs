using UnityEngine;

/// <summary>
/// État idle - le joueur ne fait rien de spécial en combat.
/// Accepte tous les inputs : attaque, blocage, esquive.
/// </summary>
public class IdleState : State
{
    public IdleState(CombatStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        // Pas d'animation spécifique - le Blend Tree gère idle/walk/run
        animator.CrossFade("Locomotion", 0.2f);
    }

    public override void OnUpdate()
    {
        // L'état idle permet le mouvement normal
        // PlayerMovement gère déjà les déplacements
    }

    public override void OnLightAttack()
    {
        // Vérifier l'endurance
        if (controller.currentWeapon == null) return;

        var attack = controller.currentWeapon.GetLightAttack(0);
        if (attack == null) return;

        if (controller.TryConsumeStamina(attack.staminaCost))
        {
            controller.ChangeState(controller.attackingState);
            controller.attackingState.StartAttack(attack, false);
        }
    }

    public override void OnHeavyAttack()
    {
        if (controller.currentWeapon == null) return;

        var attack = controller.currentWeapon.GetHeavyAttack(0);
        if (attack == null) return;

        if (controller.TryConsumeStamina(attack.staminaCost))
        {
            controller.ChangeState(controller.attackingState);
            controller.attackingState.StartAttack(attack, true);
        }
    }

    public override void OnBlockStart()
    {
        controller.ChangeState(controller.blockingState);
    }

    public override void OnDodge()
    {
        if (controller.staminaConfig == null) return;

        if (controller.TryConsumeStamina(controller.staminaConfig.dodgeCost))
        {
            controller.ChangeState(controller.dodgingState);
        }
    }
}