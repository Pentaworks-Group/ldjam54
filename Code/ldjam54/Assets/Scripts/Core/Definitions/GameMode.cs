using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitions
{
    public class GameMode : BaseDefinition
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public Double? ShipSpawnDistance { get; set; }
        public Double? JunkSpawnInterval { get; set; }
        public Double? JunkSpawnInitialDistance { get; set; }
        public GameFrame.Core.Math.Range JunkSpawnPosition { get; set; }
        public GameFrame.Core.Math.Range JunkSpawnForce { get; set; }
        public GameFrame.Core.Math.Range JunkSpawnTorque { get; set; }
        public int RequiredSurvivors { get; set; }
        public List<Star> Stars { get; set; } = new List<Star>();
        public List<Spacecraft> PlayerSpacecrafts { get; set; } = new List<Spacecraft>();
        public List<Spacecraft> Spacecrafts { get; set; } = new List<Spacecraft>();
    }
}
