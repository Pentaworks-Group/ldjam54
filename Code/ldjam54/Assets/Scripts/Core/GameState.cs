using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Model;

namespace Assets.Scripts.Core
{
    public class GameState : GameFrame.Core.GameState
    {
        public Spacecraft Spacecraft { get; set; }
        public List<SpaceJunk> SpaceJunks { get; set; } = new List<SpaceJunk>();
        public GameMode Mode { get; set; }
        public Double NextJunkSpawn { get; set; }
        public Dictionary<String, String> DeadShips { get; set; } = new Dictionary<String, String>();
        public SkyboxShader Skybox { get; set; }
        public Double TimeElapsed { get; set; }
    }
}
