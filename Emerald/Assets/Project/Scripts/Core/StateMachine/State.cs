using UnityEngine;

/// <summary>
/// Classe abstraite de base pour tous les états de combat.
/// Définit le contrat que chaque état concret doit implémenter.
/// </summary>
public abstract class State
{
    protected CombatStateController controller;
    protected Animator animator;

    public State(CombatStateController controller)
    {
        this.controller = controller;
        this.animator = controller.Animator;
    }

    // Cycle de vie de l'état
    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
    public virtual void OnExit() { }

    // Handlers d'inputs
    public virtual void OnLightAttack() { }
    public virtual void OnHeavyAttack() { }
    public virtual void OnBlockStart() { }
    public virtual void OnBlockStop() { }
    public virtual void OnDodge() { }

    // Handlers d'événements de combat
    public virtual void OnHurt(float damage, Vector3 hitDirection)
    {
        // Par défaut, vérifie si doit stagger
        if (controller.CheckShouldStagger(damage))
        {
            controller.ChangeState(controller.staggeredState);
        }
    }

    public virtual void OnAnimationComplete() { }
}