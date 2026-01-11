using UnityEngine;

/// <summary>
/// État d'attaque - gère les animations d'attaque et le système de combo.
/// </summary>
public class AttackingState : State
{
    private AttackData currentAttack;
    private bool isHeavyAttack;
    private int comboIndex;
    private float attackStartTime;
    private bool canCombo;
    private bool comboInputBuffered;
    private bool nextIsHeavy;
    private WeaponHitbox weaponHitbox;

    public AttackingState(CombatStateController controller) : base(controller)
    {
        // Trouver la hitbox de l'arme
        weaponHitbox = controller.GetComponentInChildren<WeaponHitbox>();
        if (weaponHitbox == null)
        {
            Debug.LogWarning("[AttackingState] Aucun WeaponHitbox trouvé sur l'arme !");
        }
    }

    /// <summary>
    /// Active la hitbox de l'arme. Appelé par Animation Event.
    /// </summary>
    public void EnableHitbox()
    {
        if (weaponHitbox != null && currentAttack != null)
        {
            weaponHitbox.EnableHitbox(currentAttack, controller.gameObject);
        }
    }

    /// <summary>
    /// Désactive la hitbox de l'arme. Appelé par Animation Event.
    /// </summary>
    public void DisableHitbox()
    {
        if (weaponHitbox != null)
        {
            weaponHitbox.DisableHitbox();
        }
    }

    public void StartAttack(AttackData attack, bool heavy)
    {
        currentAttack = attack;
        isHeavyAttack = heavy;
        attackStartTime = Time.time;
        canCombo = false;
        comboInputBuffered = false;
    }

    public override void OnEnter()
    {
        if (currentAttack == null)
        {
            controller.ChangeState(controller.idleState);
            return;
        }

        // Jouer l'animation
        Debug.Log($"Tentative de jouer l'animation : {currentAttack.animationName}");
        animator.CrossFade(currentAttack.animationName, 0.1f);

        // Désactiver le mouvement pendant l'attaque
        controller.PlayerMovement.enabled = false;


    }

    public override void OnUpdate()
    {
        float timeSinceStart = Time.time - attackStartTime;

        // Activer la fenêtre de combo
        if (!canCombo && timeSinceStart >= currentAttack.GetStartupTime())
        {
            canCombo = true;
        }

        // Vérifier si l'animation est terminée
        if (timeSinceStart >= currentAttack.animationDuration)
        {
            // Si input bufferisé, exécuter le combo
            if (comboInputBuffered && canCombo)
            {
                ExecuteCombo();
            }
            else
            {
                // Retour à idle
                controller.ChangeState(controller.idleState);
            }
        }
    }

    public override void OnLightAttack()
    {
        if (!canCombo)
        {
            comboInputBuffered = true;
            nextIsHeavy = false;
            return;
        }

        // Combo immédiat si dans la fenêtre
        nextIsHeavy = false;
        ExecuteCombo();
    }

    public override void OnHeavyAttack()
    {
        if (!canCombo)
        {
            comboInputBuffered = true;
            nextIsHeavy = true;
            return;
        }

        nextIsHeavy = true;
        ExecuteCombo();
    }

    private void ExecuteCombo()
    {
        if (controller.currentWeapon == null) return;

        // Obtenir la prochaine attaque
        AttackData nextAttack = null;

        if (nextIsHeavy)
        {
            nextAttack = controller.currentWeapon.GetHeavyAttack(comboIndex + 1);
        }
        else
        {
            nextAttack = controller.currentWeapon.GetLightAttack(comboIndex + 1);
        }

        if (nextAttack == null)
        {
            controller.ChangeState(controller.idleState);
            return;
        }

        // Vérifier l'endurance
        if (!controller.TryConsumeStamina(nextAttack.staminaCost))
        {
            controller.ChangeState(controller.idleState);
            return;
        }

        // Continuer le combo
        comboIndex++;
        StartAttack(nextAttack, nextIsHeavy);
        OnEnter(); // Réinitialiser l'état avec la nouvelle attaque
    }

    public override void OnDodge()
    {
        // Vérifier si l'attaque peut être annulée
        float timeSinceStart = Time.time - attackStartTime;

        if (currentAttack.canBeCanceled && timeSinceStart >= currentAttack.GetCancelWindowTime())
        {
            if (controller.TryConsumeStamina(controller.staminaConfig.dodgeCost))
            {
                controller.ChangeState(controller.dodgingState);
            }
        }
    }

    public override void OnExit()
    {
        // Réactiver le mouvement
        controller.PlayerMovement.enabled = true;

        // Reset combo si on sort de l'état
        comboIndex = 0;
    }

    public override void OnHurt(float damage, Vector3 hitDirection)
    {
        // Les attaques lourdes ont de l'hyper armor
        if (isHeavyAttack)
        {
            float timeSinceStart = Time.time - attackStartTime;
            float activeStart = currentAttack.GetStartupTime();
            float activeEnd = activeStart + currentAttack.GetActiveDuration();

            // Hyper armor pendant les frames actives
            if (timeSinceStart >= activeStart && timeSinceStart <= activeEnd)
            {
                return; // Ignore le hit
            }
        }

        // Sinon, comportement par défaut
        base.OnHurt(damage, hitDirection);
    }
}