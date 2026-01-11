using UnityEngine;

/// <summary>
/// Encapsule toutes les informations sur des dégâts infligés.
/// </summary>
public struct DamageInfo
{
    /// <summary>Montant de dégâts de santé</summary>
    public float damage;

    /// <summary>Type de dégâts (Physique, Feu, etc.)</summary>
    public DamageType damageType;

    /// <summary>Dégâts infligés à la poise</summary>
    public float poiseDamage;

    /// <summary>Force du knockback</summary>
    public float knockbackForce;

    /// <summary>Direction du knockback</summary>
    public Vector3 knockbackDirection;

    /// <summary>Composante verticale du knockback (0-1)</summary>
    public float knockbackUpwardForce;

    /// <summary>Source des dégâts (optionnel)</summary>
    public GameObject attacker;

    /// <summary>Attaque qui a causé les dégâts (optionnel)</summary>
    public AttackData attackData;

    /// <summary>
    /// Crée une structure DamageInfo depuis un AttackData.
    /// </summary>
    public static DamageInfo FromAttackData(AttackData attack, GameObject attacker, Vector3 hitDirection)
    {
        return new DamageInfo
        {
            damage = attack.baseDamage,
            damageType = attack.damageType,
            poiseDamage = attack.poiseDamage,
            knockbackForce = attack.knockbackForce,
            knockbackDirection = hitDirection.normalized,
            knockbackUpwardForce = attack.knockbackUpwardForce,
            attacker = attacker,
            attackData = attack
        };
    }
}