using UnityEngine;

/// <summary>
/// Système de santé simple pour tester les dégâts.
/// </summary>
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Statistiques")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Debug")]
    [SerializeField] private bool logDamage = true;

    [Header("UI")]
    [SerializeField] private EnemyHealthBarUI healthBarUI;
    [SerializeField] private GameObject healthBarPrefab; // Optionnel : prefab à instancier

    private void Start()
    {
        currentHealth = maxHealth;

        // Créer la barre de vie si prefab fourni
        if (healthBarPrefab != null && healthBarUI == null)
        {
            GameObject barInstance = Instantiate(healthBarPrefab); 
            healthBarUI = barInstance.GetComponent<EnemyHealthBarUI>();
            if (healthBarUI != null)
            {
                healthBarUI.SetTarget(transform);
            }
        }

        // Initialiser la barre
        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        currentHealth -= damageInfo.damage;

        if (logDamage)
        {
            Debug.Log($"[EnemyHealth] {gameObject.name} a reçu {damageInfo.damage} dégâts. Santé: {currentHealth}/{maxHealth}");
        }

        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(currentHealth, maxHealth);
        }

        // Appliquer le knockback
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 knockback = damageInfo.knockbackDirection * damageInfo.knockbackForce;
            knockback.y = damageInfo.knockbackUpwardForce * damageInfo.knockbackForce;
            rb.AddForce(knockback, ForceMode.Impulse);
        }

        // Vérifier la mort
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public Vector3 GetCenterPosition()
    {
        Collider col = GetComponent<Collider>();
        return col != null ? col.bounds.center : transform.position;
    }

    private void Die()
    {
        Debug.Log($"[EnemyHealth] {gameObject.name} est mort !");

        if (healthBarUI != null)
        {
            Destroy(healthBarUI.gameObject,2f);
        }

        // TODO: Animation de mort, loot, etc.
        Destroy(gameObject, 2f);
    }

    

}