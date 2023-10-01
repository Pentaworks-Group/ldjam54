using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Model
{
    public class GameMode
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public Double ShipSpawnDistance { get; set; }
        public Double JunkSpawnInterval { get; set; } = -1;
        public GameFrame.Core.Math.Range JunkSpawnPosition { get; set; }
        public GameFrame.Core.Math.Range JunkSpawnForce { get; set; }
        public GameFrame.Core.Math.Range JunkSpawnTorque { get; set; }
        public Double JunkSpawnInitialDistance { get; set; }
        public int RequiredSurvivors { get; set; }
        public Star Star { get; set; }
        public Spacecraft PlayerSpacecraft { get; set; }
        public List<Spacecraft> Spacecrafts { get; set; }
    }
}
