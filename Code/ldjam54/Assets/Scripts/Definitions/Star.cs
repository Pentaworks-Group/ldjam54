using System;
using System.Collections.Generic;

namespace Assets.Scripts.Definitions
{
    public class Star : BaseDefinition
    {
        public Double? Gravity { get; set; }
        public List<String> Models { get; set; }
        public List<String> Materials { get; set; }
    }
}
