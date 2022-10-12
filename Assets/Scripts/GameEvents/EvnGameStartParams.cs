
namespace SpaceInvaders
{
    public class EvnGameStartParams : FrameLord.EventDispatcher.GameEvent
    {
        public const string Name = "EvnGameStartParams";

        public int numOfLives;

        /// <summary>
        /// Constructor
        /// </summary>
        public EvnGameStartParams(int numOfLives)
        {
            eventName = Name;
            this.numOfLives = numOfLives;
        }
    }
}