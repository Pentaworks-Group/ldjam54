using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;

namespace Assets.Scripts.Core.Definitions
{
    public class Star : BaseDefinition
    {
        public Double? GravityForce { get; set; }
        public List<String> Models { get; set; }
        public List<String> Materials { get; set; }
    }
}
