using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestionnaire centralisé d'événements pour le jeu.
/// Implémente un système d'événements découplé basé sur des types.
/// Pattern Singleton pour accès global.
/// </summary>
public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Rechercher une instance existante
                instance = FindObjectOfType<EventManager>();

                // Créer une nouvelle instance si aucune n'existe
                if (instance == null)
                {
                    GameObject eventManagerObject = new GameObject("EventManager");
                    instance = eventManagerObject.AddComponent<EventManager>();
                    DontDestroyOnLoad(eventManagerObject);
                    Debug.Log("EventManager créé automatiquement.");
                }
            }
            return instance;
        }
    }

    // Dictionnaire des listeners par type d'événement
    private Dictionary<Type, List<Delegate>> eventListeners = new Dictionary<Type, List<Delegate>>();

    // Compteur d'événements pour debug
    private Dictionary<Type, int> eventCounts = new Dictionary<Type, int>();

    [Header("Configuration")]
    [SerializeField] private bool logEvents = false;
    [SerializeField] private bool trackEventCounts = true;

    /// <summary>
    /// Assure qu'il n'y a qu'une seule instance.
    /// </summary>
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Une instance d'EventManager existe déjà. Destruction de ce doublon.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// S'abonne à un événement de type T.
    /// </summary>
    /// <typeparam name="T">Type d'événement</typeparam>
    /// <param name="listener">Fonction de callback</param>
    public void Subscribe<T>(Action<T> listener) where T : GameEvent
    {
        Type eventType = typeof(T);

        if (!eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType] = new List<Delegate>();
        }

        if (!eventListeners[eventType].Contains(listener))
        {
            eventListeners[eventType].Add(listener);

            if (logEvents)
            {
                Debug.Log($"[EventManager] Abonné à {eventType.Name}. Total listeners: {eventListeners[eventType].Count}");
            }
        }
    }

    /// <summary>
    /// Se désabonne d'un événement de type T.
    /// </summary>
    /// <typeparam name="T">Type d'événement</typeparam>
    /// <param name="listener">Fonction de callback</param>
    public void Unsubscribe<T>(Action<T> listener) where T : GameEvent
    {
        Type eventType = typeof(T);

        if (eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType].Remove(listener);

            if (logEvents)
            {
                Debug.Log($"[EventManager] Désabonné de {eventType.Name}. Total listeners: {eventListeners[eventType].Count}");
            }

            // Nettoyer si plus de listeners
            if (eventListeners[eventType].Count == 0)
            {
                eventListeners.Remove(eventType);
            }
        }
    }

    /// <summary>
    /// Déclenche un événement de type T.
    /// </summary>
    /// <typeparam name="T">Type d'événement</typeparam>
    /// <param name="gameEvent">Instance de l'événement</param>
    public void TriggerEvent<T>(T gameEvent) where T : GameEvent
    {
        Type eventType = typeof(T);

        // Tracking
        if (trackEventCounts)
        {
            if (!eventCounts.ContainsKey(eventType))
            {
                eventCounts[eventType] = 0;
            }
            eventCounts[eventType]++;
        }

        // Logging
        if (logEvents)
        {
            Debug.Log($"[EventManager] Événement déclenché: {eventType.Name} (Count: {eventCounts.GetValueOrDefault(eventType, 0)})");
        }

        // Invoquer tous les listeners
        if (eventListeners.ContainsKey(eventType))
        {
            // Copie pour éviter les modifications pendant l'itération
            List<Delegate> listeners = new List<Delegate>(eventListeners[eventType]);

            foreach (var listener in listeners)
            {
                try
                {
                    (listener as Action<T>)?.Invoke(gameEvent);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[EventManager] Erreur lors de l'invocation du listener pour {eventType.Name}: {e.Message}\n{e.StackTrace}");
                }
            }
        }
    }

    /// <summary>
    /// Nettoie tous les listeners d'un type spécifique.
    /// </summary>
    /// <typeparam name="T">Type d'événement</typeparam>
    public void ClearListeners<T>() where T : GameEvent
    {
        Type eventType = typeof(T);
        if (eventListeners.ContainsKey(eventType))
        {
            eventListeners.Remove(eventType);
            if (logEvents)
            {
                Debug.Log($"[EventManager] Tous les listeners de {eventType.Name} ont été supprimés.");
            }
        }
    }

    /// <summary>
    /// Nettoie tous les listeners de tous les types.
    /// </summary>
    public void ClearAllListeners()
    {
        eventListeners.Clear();
        if (logEvents)
        {
            Debug.Log("[EventManager] Tous les listeners ont été supprimés.");
        }
    }

    /// <summary>
    /// Obtient le nombre de listeners pour un type d'événement.
    /// </summary>
    public int GetListenerCount<T>() where T : GameEvent
    {
        Type eventType = typeof(T);
        return eventListeners.ContainsKey(eventType) ? eventListeners[eventType].Count : 0;
    }

    /// <summary>
    /// Obtient le nombre total d'événements déclenchés pour un type.
    /// </summary>
    public int GetEventCount<T>() where T : GameEvent
    {
        Type eventType = typeof(T);
        return eventCounts.GetValueOrDefault(eventType, 0);
    }

    /// <summary>
    /// Affiche des statistiques sur les événements.
    /// </summary>
    public void LogStatistics()
    {
        Debug.Log("=== EventManager Statistics ===");
        Debug.Log($"Total event types: {eventCounts.Count}");

        foreach (var kvp in eventCounts)
        {
            int listenerCount = eventListeners.ContainsKey(kvp.Key) ? eventListeners[kvp.Key].Count : 0;
            Debug.Log($"{kvp.Key.Name}: {kvp.Value} triggers, {listenerCount} listeners");
        }
    }

    /// <summary>
    /// Nettoyage lors de la destruction.
    /// </summary>
    private void OnDestroy()
    {
        if (instance == this)
        {
            ClearAllListeners();
            instance = null;
        }
    }
}
