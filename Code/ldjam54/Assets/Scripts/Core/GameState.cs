
using Assets.Scripts.Model;

namespace Assets.Scripts.Core
{
    public class GameState : GameFrame.Core.GameState
    {
        public Spacecraft Spacecraft { get; set; }
        public GameMode GameMode { get; set; }
    }
}
