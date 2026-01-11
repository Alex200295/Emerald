using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Contrôleur principal de la state machine de combat.
/// Gère les transitions entre états et relaie les inputs.
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public class CombatStateController : MonoBehaviour
{
    [Header("Configuration")]
    public StaminaConfig staminaConfig;
    public WeaponData currentWeapon;

    [Header("État actuel (Debug)")]
    [SerializeField] private string currentStateName;

    // États disponibles
    public IdleState idleState;
    public AttackingState attackingState;
    public BlockingState blockingState;
    public DodgingState dodgingState;
    public StaggeredState staggeredState;

    private State currentState;

    // Système d'endurance
    [SerializeField] private float currentStamina;
    private float lastStaminaUseTime;

    // Système de poise
    private float currentPoise = 100f;
    private float maxPoise = 100f;
    private float poiseRegenRate = 50f;
    private float lastPoiseHitTime;

    // Références
    private PlayerMovement playerMovement;
    private Animator animator;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogError("ANIMATOR INTROUVABLE !");
        else
            Debug.Log($"Animator trouvé : {animator.name}");

        playerInput = GetComponent<PlayerInput>();

        // Initialiser les états
        idleState = new IdleState(this);
        attackingState = new AttackingState(this);
        blockingState = new BlockingState(this);
        dodgingState = new DodgingState(this);
        staggeredState = new StaggeredState(this);

        // Initialiser l'endurance
        if (staminaConfig != null)
            currentStamina = staminaConfig.maxStamina;
    }

    private void Start()
    {
        ChangeState(idleState);
        SetupInputActions();
    }

    private void SetupInputActions()
    {
        if (playerInput == null) return;

        var actions = playerInput.actions;
        actions["LightAttack"].performed += ctx => currentState.OnLightAttack();
        actions["HeavyAttack"].performed += ctx => currentState.OnHeavyAttack();
        actions["Block"].performed += ctx => currentState.OnBlockStart();
        actions["Block"].canceled += ctx => currentState.OnBlockStop();
        actions["Dodge"].performed += ctx => currentState.OnDodge();
    }

    private void Update()
    {
        currentState?.OnUpdate();
        RegenerateStamina();
        RegeneratePoise();
    }

    private void FixedUpdate()
    {
        currentState?.OnFixedUpdate();
    }

    public void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState?.OnExit();
        currentState = newState;
        currentStateName = currentState?.GetType().Name ?? "None";
        currentState?.OnEnter();
    }

    // Système d'endurance
    public bool TryConsumeStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            lastStaminaUseTime = Time.time;
            return true;
        }
        return false;
    }

    private void RegenerateStamina()
    {
        if (staminaConfig == null) return;
        if (Time.time - lastStaminaUseTime < staminaConfig.regenDelay) return;

        float regenAmount = staminaConfig.regenRate * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina + regenAmount, staminaConfig.maxStamina);
    }

    // Système de poise
    public bool CheckShouldStagger(float poiseDamage)
    {
        currentPoise -= poiseDamage;
        lastPoiseHitTime = Time.time;

        if (currentPoise <= 0)
        {
            currentPoise = 0;
            return true;
        }
        return false;
    }

    private void RegeneratePoise()
    {
        if (Time.time - lastPoiseHitTime < 3f) return;
        currentPoise = Mathf.Min(currentPoise + poiseRegenRate * Time.deltaTime, maxPoise);
    }

    // Propriétés publiques
    public float CurrentStamina => currentStamina;
    public float CurrentPoise => currentPoise;
    public PlayerMovement PlayerMovement => playerMovement;
    public Animator Animator => animator;

    // ==== MÉTHODES POUR ANIMATION EVENTS ====

    /// <summary>
    /// Active la hitbox. Appelé par Animation Event.
    /// </summary>
    public void EnableHitbox()
    {
        if (currentState is AttackingState attackState)
        {
            attackState.EnableHitbox();
        }
    }

    /// <summary>
    /// Désactive la hitbox. Appelé par Animation Event.
    /// </summary>
    public void DisableHitbox()
    {
        if (currentState is AttackingState attackState)
        {
            attackState.DisableHitbox();
        }
    }
}