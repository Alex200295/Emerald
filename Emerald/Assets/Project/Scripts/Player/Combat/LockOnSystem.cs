using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Système de lock-on permettant de cibler et suivre les ennemis.
/// </summary>
public class LockOnSystem : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Rayon de détection des cibles")]
    [SerializeField] private float lockOnRange = 15f;

    [Tooltip("Distance maximale pour maintenir le lock-on")]
    [SerializeField] private float maxLockDistance = 20f;

    [Tooltip("Layer des ennemis")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("Vitesse de rotation")]
    [Tooltip("Vitesse de rotation du joueur vers la cible")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float lastSwitchTime = 0f;
    [SerializeField] private float switchCooldown = 0.3f;

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;

    // État du système
    private LockOnTarget currentTarget;
    private bool isLockedOn = false;


    // Références
    private Camera mainCamera;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput;

    // Input actions
    private InputAction lockOnAction;
    private InputAction switchTargetAction;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();

        if (mainCamera == null)
        {
            Debug.LogError("[LockOnSystem] Camera.main introuvable !");
        }
    }

    private void Start()
    {
        SetupInputActions();
    }

    private void SetupInputActions()
    {
        if (playerInput == null) return;

        var actions = playerInput.actions;

        // Action pour activer/désactiver le lock-on
        lockOnAction = actions["LockOn"];
        if (lockOnAction != null)
        {
            lockOnAction.performed += ctx => ToggleLockOn();
        }

        // Action pour switch entre les cibles (Right Stick horizontal)
        switchTargetAction = actions["SwitchTarget"];
    }

    private void Update()
    {
        if (isLockedOn)
        {
            UpdateLockOn();
            HandleTargetSwitching();
        }
    }

    private void LateUpdate()
    {
        if (isLockedOn && currentTarget != null)
        {
            RotateTowardsTarget();
        }
    }

    /// <summary>
    /// Active ou désactive le lock-on.
    /// </summary>
    private void ToggleLockOn()
    {
        if (isLockedOn)
        {
            DisableLockOn();
        }
        else
        {
            EnableLockOn();
        }
    }

    /// <summary>
    /// Active le lock-on sur la cible la plus proche du centre de l'écran.
    /// </summary>
    private void EnableLockOn()
    {
        // Trouver toutes les cibles dans le rayon
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, lockOnRange, enemyLayer);

        if (nearbyColliders.Length == 0)
        {
            if (showDebugInfo) Debug.Log("[LockOnSystem] Aucune cible à portée");
            return;
        }

        // Filtrer et obtenir les LockOnTarget valides
        List<LockOnTarget> validTargets = new List<LockOnTarget>();
        foreach (Collider col in nearbyColliders)
        {
            LockOnTarget target = col.GetComponent<LockOnTarget>();
            if (target == null) target = col.GetComponentInParent<LockOnTarget>();

            if (target != null && target.IsLockable())
            {
                validTargets.Add(target);
            }
        }

        if (validTargets.Count == 0)
        {
            if (showDebugInfo) Debug.Log("[LockOnSystem] Aucune cible valide");
            return;
        }

        // Sélectionner la cible la plus proche du centre de l'écran
        currentTarget = GetClosestToScreenCenter(validTargets);

        if (currentTarget != null)
        {
            isLockedOn = true;

            // Notifier le PlayerMovement pour activer le strafe
            if (playerMovement != null)
            {
                playerMovement.SetStrafeMode(true);
            }

            if (showDebugInfo) Debug.Log($"[LockOnSystem] Verrouillé sur {currentTarget.gameObject.name}");
        }
    }

    /// <summary>
    /// Désactive le lock-on.
    /// </summary>
    public void DisableLockOn()
    {
        if (!isLockedOn) return;

        isLockedOn = false;
        currentTarget = null;

        // Désactiver le strafe
        if (playerMovement != null)
        {
            playerMovement.SetStrafeMode(false);
        }

        if (showDebugInfo) Debug.Log("[LockOnSystem] Lock-on désactivé");
    }

    /// <summary>
    /// Vérifie que le lock-on est toujours valide.
    /// </summary>
    private void UpdateLockOn()
    {
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            DisableLockOn();
            return;
        }

        // Vérifier la distance
        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (distance > maxLockDistance)
        {
            if (showDebugInfo) Debug.Log("[LockOnSystem] Cible trop loin, désactivation");
            DisableLockOn();
            return;
        }

        // Vérifier la ligne de vue
        Vector3 directionToTarget = (currentTarget.GetLockOnPosition() - transform.position).normalized;
        if (Physics.Raycast(transform.position + Vector3.up, directionToTarget, distance, ~enemyLayer))
        {
            if (showDebugInfo) Debug.Log("[LockOnSystem] Ligne de vue bloquée");
            DisableLockOn();
        }
    }

    /// <summary>
    /// Gère le switch entre les cibles.
    /// </summary>
    private void HandleTargetSwitching()
    {
        if (switchTargetAction == null) return;

        float switchInput = switchTargetAction.ReadValue<float>();

        // Seuil pour éviter les inputs accidentels
        if (Mathf.Abs(switchInput) < 0.5f) return;

        // Cooldown pour éviter les switch trop rapides
        if (Time.time - lastSwitchTime < switchCooldown) return;

        // Trouver toutes les cibles disponibles
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, lockOnRange, enemyLayer);
        List<LockOnTarget> validTargets = new List<LockOnTarget>();

        foreach (Collider col in nearbyColliders)
        {
            LockOnTarget target = col.GetComponent<LockOnTarget>();
            if (target == null) target = col.GetComponentInParent<LockOnTarget>();

            // CHANGEMENT : Inclure la cible actuelle dans la liste
            if (target != null && target.IsLockable())
            {
                validTargets.Add(target);
            }
        }

        if (validTargets.Count <= 1) return; // Pas besoin de switch si 0 ou 1 cible

        // Trier les cibles par position horizontale à l'écran (gauche à droite)
        validTargets = validTargets.OrderBy(t =>
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(t.GetLockOnPosition());
            return screenPos.x;
        }).ToList();

        // Trouver l'index de la cible actuelle
        int currentIndex = validTargets.IndexOf(currentTarget);
        if (currentIndex == -1) return;

        // Calculer le nouvel index avec wraparound
        int newIndex;
        if (switchInput > 0) // Droite (E)
        {
            newIndex = (currentIndex + 1) % validTargets.Count;
        }
        else // Gauche (Q)
        {
            newIndex = (currentIndex - 1 + validTargets.Count) % validTargets.Count;
        }

        // Switch vers la nouvelle cible
        currentTarget = validTargets[newIndex];
        lastSwitchTime = Time.time;

        if (showDebugInfo) Debug.Log($"[LockOnSystem] Switch vers {currentTarget.gameObject.name} (index {newIndex}/{validTargets.Count})");
    }

    /// <summary>
    /// Trouve la cible la plus proche du centre de l'écran.
    /// </summary>
    private LockOnTarget GetClosestToScreenCenter(List<LockOnTarget> targets)
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        LockOnTarget closest = null;
        float closestDistance = float.MaxValue;

        foreach (LockOnTarget target in targets)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(target.GetLockOnPosition());

            // Ignorer si derrière la caméra
            if (screenPos.z < 0) continue;

            float distance = Vector2.Distance(screenPos, screenCenter);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target;
            }
        }

        return closest;
    }

    /// <summary>
    /// Trouve la cible dans la direction spécifiée (gauche/droite).
    /// </summary>
    private LockOnTarget GetTargetInDirection(List<LockOnTarget> targets, bool right)
    {
        if (currentTarget == null) return null;

        Vector3 currentScreenPos = mainCamera.WorldToScreenPoint(currentTarget.GetLockOnPosition());
        LockOnTarget bestTarget = null;
        float bestScore = float.MaxValue;

        foreach (LockOnTarget target in targets)
        {
            Vector3 targetScreenPos = mainCamera.WorldToScreenPoint(target.GetLockOnPosition());

            // Ignorer si derrière la caméra
            if (targetScreenPos.z < 0) continue;

            float horizontalDiff = targetScreenPos.x - currentScreenPos.x;

            // Vérifier la direction
            if ((right && horizontalDiff <= 0) || (!right && horizontalDiff >= 0))
                continue;

            // Score basé sur la distance horizontale et verticale
            float horizontalDistance = Mathf.Abs(horizontalDiff);
            float verticalDistance = Mathf.Abs(targetScreenPos.y - currentScreenPos.y);
            float score = horizontalDistance + verticalDistance * 0.5f; // Favoriser horizontal

            if (score < bestScore)
            {
                bestScore = score;
                bestTarget = target;
            }
        }

        return bestTarget;
    }

    /// <summary>
    /// Fait tourner le joueur vers la cible verrouillée.
    /// </summary>
    private void RotateTowardsTarget()
    {
        if (currentTarget == null) return;

        Vector3 directionToTarget = (currentTarget.transform.position - transform.position).normalized;
        directionToTarget.y = 0; // Garder rotation seulement sur Y

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Propriétés publiques
    public bool IsLockedOn => isLockedOn;
    public LockOnTarget CurrentTarget => currentTarget;
    public Vector3 GetTargetPosition() => currentTarget != null ? currentTarget.GetLockOnPosition() : Vector3.zero;

    // Debug visuel
    private void OnDrawGizmos()
    {
        if (!showDebugInfo) return;

        // Rayon de lock-on
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lockOnRange);

        // Distance maximale
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxLockDistance);

        // Ligne vers la cible
        if (isLockedOn && currentTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + Vector3.up, currentTarget.GetLockOnPosition());
        }
    }
}