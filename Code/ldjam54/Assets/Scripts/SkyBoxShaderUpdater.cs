using System;

using Assets.Scripts.Core.Model;

using UnityEngine;

namespace Assets.Scripts
{
    public class SkyboxShaderUpdater : MonoBehaviour
    {
        public SkyboxShader Skybox { get; set; }

        private int seedProperty = -1;
        private float startingSeed;
        private Material skyboxMaterial;

        private void OnDisable()
        {
            skyboxMaterial.SetFloat(seedProperty, startingSeed);
        }

        private void Awake()
        {
            seedProperty = Shader.PropertyToID("_Seed");

            if (seedProperty >= 0)
            {
                skyboxMaterial = RenderSettings.skybox;
                startingSeed = skyboxMaterial.GetFloat(seedProperty);
            }
        }

        public void UpdateSkybox()
        {
            if (this.Skybox != default)
            {
                skyboxMaterial?.SetFloat(seedProperty, Skybox.Seed);
            }
        }
    }
}
