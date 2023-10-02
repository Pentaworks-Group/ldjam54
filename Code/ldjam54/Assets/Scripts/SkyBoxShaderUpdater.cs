using System;

using Assets.Scripts.Core.Model;

using GameFrame.Core.Extensions;

using UnityEngine;

namespace Assets.Scripts
{
    public class SkyboxShaderUpdater : MonoBehaviour
    {
        private Material skyboxMaterial;
        private int seedProperty = -1;
        private int skyColorProperty = -1;
        private int starColorProperty = -1;
        private int starSizeProperty = -1;
        private int layersProperty = -1;
        private int densityProperty = -1;
        private int densityModulationProperty = -1;
        private int brigntnessProperty = -1;
        private int brightnessModulationProperty = -1;
        private int skyFogColorProperty = -1;
        private int noiseDensityProperty = -1;
        private int noiseParametersProperty = -1;
        private int noiseMaskParametersProperty = -1;
        private int noiseCutParametersProperty = -1;

        private Boolean isAwake;

        public SkyboxShader Skybox { get; set; }

        private void Awake()
        {
            skyboxMaterial = RenderSettings.skybox;

            seedProperty = Shader.PropertyToID("_Seed");
            skyColorProperty = Shader.PropertyToID("_SkyColor");
            starColorProperty = Shader.PropertyToID("_Color");
            starSizeProperty = Shader.PropertyToID("_StarSizeRange");
            layersProperty = Shader.PropertyToID("_Layers");
            densityProperty = Shader.PropertyToID("_Density");
            densityModulationProperty = Shader.PropertyToID("_DensityMod");
            brigntnessProperty = Shader.PropertyToID("_Brightness");
            brightnessModulationProperty = Shader.PropertyToID("_BrightnessMod");
            skyFogColorProperty = Shader.PropertyToID("_SkyFogColor");
            noiseDensityProperty = Shader.PropertyToID("_NoiseDensity");
            noiseParametersProperty = Shader.PropertyToID("_NoiseParams");
            noiseMaskParametersProperty = Shader.PropertyToID("_NoiseMaskParams");
            noiseCutParametersProperty = Shader.PropertyToID("_NoiseMaskParams2");

            this.isAwake = true;
        }

        public void UpdateSkybox()
        {
            if ((isAwake) && (this.Skybox != default))
            {
                UpdateProperty(seedProperty, Skybox.Seed);
                UpdateProperty(skyColorProperty, Skybox.SkyColor.ToUnity());
                UpdateProperty(starColorProperty, Skybox.StarColor.ToUnity());
                UpdateProperty(starSizeProperty, Skybox.StarSize.ToUnity());
                UpdateProperty(layersProperty, Skybox.Layers);
                UpdateProperty(densityProperty, Skybox.Density);
                UpdateProperty(densityModulationProperty, Skybox.DensityModulation);
                UpdateProperty(brigntnessProperty, Skybox.Contrast);
                UpdateProperty(brightnessModulationProperty, Skybox.BrightnessModulation);
                UpdateProperty(skyFogColorProperty, Skybox.SkyFogColor.ToUnity());
                UpdateProperty(noiseDensityProperty, Skybox.NoiseDensity);
                UpdateProperty(noiseParametersProperty, Skybox.NoiseParameters.ToUnity());
                UpdateProperty(noiseMaskParametersProperty, Skybox.BackgroundMaskParameters.ToUnity());
                UpdateProperty(noiseCutParametersProperty, Skybox.BackgroundCutParameters.ToUnity());
            }
        }

        private void UpdateProperty(Int32 propertyIndex, float newValue)
        {
            var currentValue = skyboxMaterial.GetFloat(propertyIndex);

            if (!Equals(currentValue, newValue))
            {
                skyboxMaterial.SetFloat(propertyIndex, newValue);
            }
        }

        private void UpdateProperty(Int32 propertyIndex, Color newValue)
        {
            var currentValue = skyboxMaterial.GetColor(propertyIndex);

            if (!Equals(currentValue,newValue))
            {
                skyboxMaterial.SetColor(propertyIndex, newValue);
            }
        }

        private void UpdateProperty(Int32 propertyIndex, UnityEngine.Vector4 newValue)
        {
            var currentValue = skyboxMaterial.GetVector(propertyIndex);

            if (!Equals(currentValue, newValue))
            {
                skyboxMaterial.SetVector(propertyIndex, newValue);
            }
        }
    }
}
