using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Model;

namespace Assets.Scripts.Core
{
    public class GameState : GameFrame.Core.GameState
    {
        public Spacecraft Spacecraft { get; set; }
        public GameMode Mode { get; set; }
        public Double NextJunkSpawn { get; set; }              
        public Dictionary<String, String> DeadShips { get; set; } = new Dictionary<String, String>();
    }
}
