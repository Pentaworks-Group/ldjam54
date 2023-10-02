using System;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Model
{
    public class SkyboxShader
    {
        public float Seed { get; set; }
        public GameFrame.Core.Media.Color SkyColor { get; set; }

        public GameFrame.Core.Media.Color StarColor { get; set; }
        public Vector4 StarSize { get; set; } // Min 0.4, max 3.0, set in Vector4 X/Y

        public float Layers { get; set; } //1.0, 5.0
        public float Density { get; set; } //0.5, 4.0
        public float DensityModulation { get; set; } //1.1, 3.0

        public float Contrast { get; set; } //0.0, 3.0
        public float BrightnessModulation { get; set; } // 1.01, 4.0

        public Int32 IsGalaxyNoiseEnabled { get; set; } // 0/1
        public GameFrame.Core.Media.Color SkyFogColor { get; set; }
        public float NoiseDensity { get; set; } //1.0, 30.0

        public Vector4 NoiseParameters { get; set; }
        public Vector4 BackgroundMaskParameters { get; set; }
        public Vector4 BackgroundCutParameters { get; set; }
    }
}
