using UnityEngine;

/// <summary>
/// Marque un GameObject comme ciblable par le système de lock-on.
/// Attachez ce component à tous les ennemis.
/// </summary>
public class LockOnTarget : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Point de ciblage (généralement au centre de masse)")]
    [SerializeField] private Transform lockOnPoint;

    [Tooltip("Est-ce que cette cible peut être verrouillée?")]
    [SerializeField] private bool isLockable = true;

    private void Awake()
    {
        // Créer automatiquement un lock-on point si non assigné
        if (lockOnPoint == null)
        {
            GameObject lockPoint = new GameObject("LockOnPoint");
            lockPoint.transform.SetParent(transform);

            // Positionner au centre vertical (hauteur du torse)
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                lockPoint.transform.localPosition = new Vector3(0, col.bounds.extents.y * 0.7f, 0);
            }
            else
            {
                lockPoint.transform.localPosition = new Vector3(0, 1f, 0);
            }

            lockOnPoint = lockPoint.transform;
        }
    }

    /// <summary>
    /// Retourne le point de ciblage pour la caméra.
    /// </summary>
    public Vector3 GetLockOnPosition()
    {
        return lockOnPoint != null ? lockOnPoint.position : transform.position;
    }

    /// <summary>
    /// Vérifie si cette cible peut être verrouillée.
    /// </summary>
    public bool IsLockable()
    {
        return isLockable && gameObject.activeInHierarchy;
    }

    /// <summary>
    /// Active ou désactive la possibilité de verrouiller cette cible.
    /// </summary>
    public void SetLockable(bool lockable)
    {
        isLockable = lockable;
    }

    // Gizmo pour visualiser le lock-on point
    private void OnDrawGizmosSelected()
    {
        if (lockOnPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(lockOnPoint.position, 0.3f);
        }
    }
}