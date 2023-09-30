using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;

namespace Assets.Scripts.Core
{
    public class GameMode
    {
        public String Name { get; set; }
        public List<Star> Stars { get; set; } = new List<Star>();
        public List<Spacecraft> Spacecrafts { get; set; } = new List<Spacecraft>();
    }
}
