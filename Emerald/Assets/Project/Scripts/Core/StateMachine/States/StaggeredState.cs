using UnityEngine;

/// <summary>
/// État de stagger - le joueur est sonné et vulnérable.
/// </summary>
public class StaggeredState : State
{
    private const string STAGGER_ANIM = "Hit_Stagger";
    private const float STAGGER_DURATION = 1.2f;

    private float staggerStartTime;

    public StaggeredState(CombatStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        staggerStartTime = Time.time;

        // Jouer l'animation de stagger
        animator.CrossFade(STAGGER_ANIM, 0.05f);

        // Désactiver complètement le contrôle
        controller.PlayerMovement.enabled = false;
    }

    public override void OnUpdate()
    {
        float timeSinceStart = Time.time - staggerStartTime;

        // Fin du stagger
        if (timeSinceStart >= STAGGER_DURATION)
        {
            controller.ChangeState(controller.idleState);
        }
    }

    public override void OnHurt(float damage, Vector3 hitDirection)
    {
        // Pendant le stagger, les hits ne font que des dégâts
        // Pas de nouveau stagger (évite le stagger lock)
        // Ne pas appeler base.OnHurt
    }

    // Aucun input accepté pendant le stagger
    public override void OnLightAttack() { }
    public override void OnHeavyAttack() { }
    public override void OnBlockStart() { }
    public override void OnDodge() { }

    public override void OnExit()
    {
        controller.PlayerMovement.enabled = true;

        // AJOUTE CETTE LIGNE
        animator.CrossFade("Locomotion", 0.2f);
    }
}