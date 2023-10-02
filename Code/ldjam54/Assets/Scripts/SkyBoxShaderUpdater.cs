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
                skyboxMaterial.SetFloat(seedProperty, Skybox.Seed);
                skyboxMaterial.SetColor(skyColorProperty, Skybox.SkyColor.ToUnity());
                skyboxMaterial.SetColor(starColorProperty, Skybox.StarColor.ToUnity());
                skyboxMaterial.SetVector(starSizeProperty, Skybox.StarSize.ToUnity());
                skyboxMaterial.SetFloat(layersProperty, Skybox.Layers);
                skyboxMaterial.SetFloat(densityProperty, Skybox.Density);
                skyboxMaterial.SetFloat(densityModulationProperty, Skybox.DensityModulation);
                skyboxMaterial.SetFloat(brigntnessProperty, Skybox.Contrast);
                skyboxMaterial.SetFloat(brightnessModulationProperty, Skybox.BrightnessModulation);
                skyboxMaterial.SetColor(skyFogColorProperty, Skybox.SkyFogColor.ToUnity());
                skyboxMaterial.SetFloat(noiseDensityProperty, Skybox.NoiseDensity);
                skyboxMaterial.SetVector(noiseParametersProperty, Skybox.NoiseParameters.ToUnity());
                skyboxMaterial.SetVector(noiseMaskParametersProperty, Skybox.BackgroundMaskParameters.ToUnity());
                skyboxMaterial.SetVector(noiseCutParametersProperty, Skybox.BackgroundCutParameters.ToUnity());
            }
        }
    }
}
