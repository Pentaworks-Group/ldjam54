using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Model;

namespace Assets.Scripts.Core
{
    public class GameState : GameFrame.Core.GameState
    {
        public List<Spacecraft>  PlayerSpacecraft { get; set; } = new List<Spacecraft>();
        public List<SpaceJunk> SpaceJunks { get; set; } = new List<SpaceJunk>();
        public List<Spacecraft>  Spacecrafts { get; set; } = new List<Spacecraft>();
        public GameMode Mode { get; set; }
        public Double NextJunkSpawn { get; set; }
        public Dictionary<String, String> DeadShips { get; set; } = new Dictionary<String, String>();
        public SkyboxShader Skybox { get; set; }
        public Double TimeElapsed { get; set; }
    }
}
