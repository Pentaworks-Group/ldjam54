using System;

namespace Assets.Scripts.Core.Model
{
    public class SkyboxShader
    {
        public float Seed { get; set; }
        public GameFrame.Core.Media.Color SkyColor { get; set; }
        public float Layers { get; set; }
        public float Density { get; set; }
        public float Contrast { get; set; }
        public float BrightnessModulation { get; set; }
        public GameFrame.Core.Media.Color FogColor { get; set; }
        public float NoiseDensity { get; set; }
    }
}
