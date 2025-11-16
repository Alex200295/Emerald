using System;
using UnityEngine;

/// <summary>
/// Classe de base pour tous les événements du jeu.
/// Permet de créer des événements typés et découplés.
/// </summary>
public abstract class GameEvent
{
    /// <summary>
    /// Timestamp de l'événement.
    /// </summary>
    public float Timestamp { get; private set; }

    protected GameEvent()
    {
        Timestamp = Time.time;
    }
}

/// <summary>
/// Événement déclenché lorsque le joueur saute.
/// </summary>
public class PlayerJumpEvent : GameEvent
{
    public Vector3 Position { get; private set; }
    public float JumpHeight { get; private set; }

    public PlayerJumpEvent(Vector3 position, float jumpHeight)
    {
        Position = position;
        JumpHeight = jumpHeight;
    }
}

/// <summary>
/// Événement déclenché lorsque le joueur atterrit.
/// </summary>
public class PlayerLandEvent : GameEvent
{
    public Vector3 Position { get; private set; }
    public float FallDuration { get; private set; }

    public PlayerLandEvent(Vector3 position, float fallDuration)
    {
        Position = position;
        FallDuration = fallDuration;
    }
}

/// <summary>
/// Événement déclenché lorsque le joueur commence à sprinter.
/// </summary>
public class PlayerSprintStartEvent : GameEvent
{
    public PlayerSprintStartEvent() { }
}

/// <summary>
/// Événement déclenché lorsque le joueur arrête de sprinter.
/// </summary>
public class PlayerSprintStopEvent : GameEvent
{
    public PlayerSprintStopEvent() { }
}

/// <summary>
/// Événement déclenché lorsque le joueur se déplace.
/// </summary>
public class PlayerMoveEvent : GameEvent
{
    public Vector3 Position { get; private set; }
    public Vector3 Direction { get; private set; }
    public float Speed { get; private set; }

    public PlayerMoveEvent(Vector3 position, Vector3 direction, float speed)
    {
        Position = position;
        Direction = direction;
        Speed = speed;
    }
}
