using System;
using System.Collections.Generic;

namespace Assets.Scripts.Definitions
{
    public class GameMode : BaseDefinition
    {
        public String Name { get; set; }
        public List<Star> Stars { get; set; }
        public List<Spacecraft> PlayerSpacecrafts { get; set; } = new List<Spacecraft>();
        public List<Spacecraft> Spacecrafts { get; set; } = new List<Spacecraft>();
    }
}
