using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitions
{
    public class Star : BaseDefinition
    {
        public Double? Gravity { get; set; }
        public Double? Mass { get; set; }
        public GameFrame.Core.Math.Range LightRange { get; set; }
        public GameFrame.Core.Math.Range LightIntensity { get; set; }
        public List<String> Models { get; set; }
        public List<String> Materials { get; set; }
    }
}
