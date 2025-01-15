using System;

/// <summary>
/// Crosspoint for events.
/// </summary>
public class EventManager
{
    public static event Action OnMoveQueue;

    public static void MoveQueue()
    {
        OnMoveQueue?.Invoke();
    }
}
