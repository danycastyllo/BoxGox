public class EvnPlayerDied : FrameLord.EventDispatcher.GameEvent
{
    public const string Name = "EvnPlayerDied";

    /// <summary>
    /// Constructor
    /// </summary>
    public EvnPlayerDied()
    {
        eventName = Name;
    }
}

