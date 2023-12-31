using System;

namespace Assets.Scripts.Core.Model
{
    public class Star
    {
        public Double Gravity { get; set; }
        public Double Mass { get; set; }
        public float LightRange { get; set; }
        public float LightIntensity { get; set; }
        public String Material { get; set; }
        public String Model { get; set; }        
    }
}
