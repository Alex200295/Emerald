using UnityEngine;

/// <summary>
/// Interface pour tout objet pouvant recevoir des dégâts.
/// Implémentez cette interface sur les ennemis, objets destructibles, etc.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Appelé quand l'objet reçoit des dégâts.
    /// </summary>
    /// <param name="damageInfo">Informations sur les dégâts reçus</param>
    void TakeDamage(DamageInfo damageInfo);

    /// <summary>
    /// Retourne la position du centre de l'entité pour les calculs de direction.
    /// </summary>
    Vector3 GetCenterPosition();
}