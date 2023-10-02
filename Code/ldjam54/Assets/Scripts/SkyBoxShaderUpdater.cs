using Assets.Scripts.Core.Model;

using UnityEngine;

namespace Assets.Scripts
{
    public class SkyBoxShaderUpdater : MonoBehaviour
    {
        public SkyboxShader Skybox { get; set; }

        private int seedProperty = -1;
        private float startingSeed;
        private Material skxboxMaterial;

        private void OnDisable()
        {
            skxboxMaterial.SetFloat(seedProperty, startingSeed);
        }

        private void Awake()
        {
            if (Skybox != default)
            {
                seedProperty = Shader.PropertyToID("_Seed");

                if (seedProperty >= 0)
                {
                    skxboxMaterial = RenderSettings.skybox;
                    startingSeed = skxboxMaterial.GetFloat(seedProperty);

                    skxboxMaterial.SetFloat(seedProperty, Skybox.Seed);
                }
            }
        }
    }
}
