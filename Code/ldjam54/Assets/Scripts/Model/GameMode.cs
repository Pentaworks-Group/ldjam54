using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class GameMode
    {
        public String Name { get; set; }
        public Star Star { get; set; }
        public List<Spacecraft> Spacecrafts { get; set; }
    }
}
