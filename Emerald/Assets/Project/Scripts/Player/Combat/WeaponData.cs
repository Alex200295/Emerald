using UnityEngine;

/// <summary>
/// Contient toutes les données d'une arme spécifique.
/// Regroupe les statistiques de base et les séquences d'attaques.
/// </summary>
[CreateAssetMenu(fileName = "New Weapon", menuName = "Combat/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Identification")]
    [Tooltip("Nom de l'arme")]
    public string weaponName = "New Weapon";

    [TextArea(3, 5)]
    [Tooltip("Description de l'arme")]
    public string description;

    [Header("Modèle 3D")]
    [Tooltip("Prefab de l'arme à instancier")]
    public GameObject weaponPrefab;

    [Tooltip("Position locale de l'arme dans la main (offset)")]
    public Vector3 gripPositionOffset = Vector3.zero;

    [Tooltip("Rotation locale de l'arme dans la main")]
    public Vector3 gripRotationOffset = Vector3.zero;

    [Header("Statistiques de base")]
    [Tooltip("Multiplicateur de puissance d'attaque (appliqué aux dégâts)")]
    [Range(0.5f, 3f)]
    public float attackPowerMultiplier = 1f;

    [Tooltip("Multiplicateur de vitesse d'attaque (affecte la durée des animations)")]
    [Range(0.5f, 2f)]
    public float attackSpeedMultiplier = 1f;

    [Tooltip("Chance de coup critique (0-1)")]
    [Range(0f, 1f)]
    public float criticalChance = 0.05f;

    [Tooltip("Multiplicateur de dégâts critiques")]
    [Range(1f, 5f)]
    public float criticalDamageMultiplier = 2f;

    [Tooltip("Portée de l'arme en mètres")]
    [Range(0.5f, 5f)]
    public float weaponRange = 2f;

    [Header("Séquences d'attaques")]
    [Tooltip("Chaîne d'attaques légères")]
    public AttackData[] lightAttackCombo;

    [Tooltip("Chaîne d'attaques lourdes")]
    public AttackData[] heavyAttackCombo;

    [Tooltip("Attaque spéciale chargée")]
    public AttackData chargedAttack;

    [Tooltip("Attaque pendant un sprint")]
    public AttackData runningAttack;

    [Tooltip("Attaque après une esquive")]
    public AttackData dodgeAttack;

    [Header("Effets de l'arme")]
    [Tooltip("Effet visuel permanent sur l'arme (ex: flammes, glace)")]
    public GameObject weaponEffectPrefab;

    [Tooltip("Point d'attache pour les effets (tipPosition ou base de lame)")]
    public Transform effectAttachPoint;

    // Méthodes utilitaires

    /// <summary>
    /// Obtient l'attaque légère à l'index spécifié dans la chaîne de combo.
    /// </summary>
    public AttackData GetLightAttack(int comboIndex)
    {
        if (lightAttackCombo == null || lightAttackCombo.Length == 0)
        {
            Debug.LogWarning($"Aucune attaque légère définie pour {weaponName}");
            return null;
        }

        // Boucle si index dépasse la longueur
        int index = comboIndex % lightAttackCombo.Length;
        return lightAttackCombo[index];
    }

    /// <summary>
    /// Obtient l'attaque lourde à l'index spécifié dans la chaîne de combo.
    /// </summary>
    public AttackData GetHeavyAttack(int comboIndex)
    {
        if (heavyAttackCombo == null || heavyAttackCombo.Length == 0)
        {
            Debug.LogWarning($"Aucune attaque lourde définie pour {weaponName}");
            return null;
        }

        int index = comboIndex % heavyAttackCombo.Length;
        return heavyAttackCombo[index];
    }

    /// <summary>
    /// Calcule les dégâts finaux en appliquant le multiplicateur de l'arme.
    /// </summary>
    public float CalculateFinalDamage(float baseDamage, bool isCritical = false)
    {
        float damage = baseDamage * attackPowerMultiplier;

        if (isCritical)
        {
            damage *= criticalDamageMultiplier;
        }

        return damage;
    }

    /// <summary>
    /// Vérifie si le coup est critique basé sur la chance.
    /// </summary>
    public bool RollForCritical()
    {
        return Random.value < criticalChance;
    }
}