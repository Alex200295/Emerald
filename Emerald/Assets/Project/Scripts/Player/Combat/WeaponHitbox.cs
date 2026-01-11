using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gère la hitbox d'une arme et la détection des coups.
/// Attachez ce script à votre GameObject d'arme avec un Collider (Trigger).
/// </summary>
[RequireComponent(typeof(Collider))]
public class WeaponHitbox : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Layer des ennemis que cette arme peut toucher")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("Debug")]
    [SerializeField] private bool showDebugGizmos = true;
    [SerializeField] private Color activeColor = Color.red;
    [SerializeField] private Color inactiveColor = Color.gray;

    // État de la hitbox
    private bool isActive = false;
    private Collider hitboxCollider;

    // Prévention des multi-hits
    private HashSet<Collider> hitTargets = new HashSet<Collider>();

    // Données de l'attaque en cours
    private AttackData currentAttack;
    private GameObject owner;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();

        // Vérifier que c'est bien un Trigger
        if (!hitboxCollider.isTrigger)
        {
            Debug.LogWarning($"[WeaponHitbox] Le Collider sur {gameObject.name} n'est pas configuré en Trigger. Correction automatique.");
            hitboxCollider.isTrigger = true;
        }

        // Désactiver au démarrage
        DisableHitbox();
    }

    /// <summary>
    /// Active la hitbox pour une attaque spécifique.
    /// Appelé par Animation Event.
    /// </summary>
    public void EnableHitbox(AttackData attack, GameObject attacker)
    {
        if (attack == null)
        {
            Debug.LogError("[WeaponHitbox] AttackData est null !");
            return;
        }

        isActive = true;
        currentAttack = attack;
        owner = attacker;
        hitTargets.Clear(); // Réinitialiser pour la nouvelle attaque
        hitboxCollider.enabled = true;

        Debug.Log($"[WeaponHitbox] Activée pour {attack.attackName}");
    }

    /// <summary>
    /// Désactive la hitbox.
    /// Appelé par Animation Event.
    /// </summary>
    public void DisableHitbox()
    {
        isActive = false;
        hitboxCollider.enabled = false;
        hitTargets.Clear();

        Debug.Log("[WeaponHitbox] Désactivée");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignorer si hitbox inactive
        if (!isActive) return;

        // Vérifier le layer
        if (((1 << other.gameObject.layer) & enemyLayer) == 0)
            return;

        // Éviter les multi-hits sur la même cible
        if (hitTargets.Contains(other))
            return;

        // Marquer comme touché
        hitTargets.Add(other);

        // Chercher l'interface IDamageable
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable == null)
            damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            // Calculer la direction du hit
            Vector3 hitDirection = (damageable.GetCenterPosition() - transform.position).normalized;

            // Créer les infos de dégâts
            DamageInfo damageInfo = DamageInfo.FromAttackData(currentAttack, owner, hitDirection);

            // Infliger les dégâts
            damageable.TakeDamage(damageInfo);

            Debug.Log($"[WeaponHitbox] Touché: {other.gameObject.name} - Dégâts: {damageInfo.damage}");

            // Spawner VFX si configuré
            if (currentAttack.hitVFXPrefab != null)
            {
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                GameObject vfx = Instantiate(currentAttack.hitVFXPrefab, hitPoint, Quaternion.identity);

                // Détruire après la durée du particle system
                ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
                float destroyTime = ps != null ? ps.main.duration + ps.main.startLifetime.constantMax : 2f;
                Destroy(vfx, destroyTime);
            }

            // Jouer son d'impact si configuré
            if (currentAttack.hitSound != null)
            {
                AudioSource.PlayClipAtPoint(currentAttack.hitSound, transform.position);
            }
        }
        else
        {
            Debug.LogWarning($"[WeaponHitbox] {other.gameObject.name} n'a pas de composant IDamageable");
        }
    }

    private void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;

        Collider col = hitboxCollider != null ? hitboxCollider : GetComponent<Collider>();
        if (col == null) return;

        Gizmos.color = isActive ? activeColor : inactiveColor;

        // Dessiner selon le type de collider
        if (col is BoxCollider boxCol)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCol.center, boxCol.size);
        }
        else if (col is SphereCollider sphereCol)
        {
            Gizmos.DrawWireSphere(transform.TransformPoint(sphereCol.center), sphereCol.radius);
        }
        else if (col is CapsuleCollider capsuleCol)
        {
            // Approximation simplifiée pour capsule
            Gizmos.DrawWireSphere(transform.TransformPoint(capsuleCol.center), capsuleCol.radius);
        }
    }

    // Propriétés publiques
    public bool IsActive => isActive;
}