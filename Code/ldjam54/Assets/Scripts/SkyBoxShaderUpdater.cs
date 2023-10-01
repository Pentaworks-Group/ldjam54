using UnityEngine;

namespace Assets.Scripts
{
    public class SkyBoxShaderUpdater : MonoBehaviour
    {
        public float Seed = 57.95f;

        private int seedProperty = -1;
        private float startingSeed;
        private Material skxboxMaterial;

        private void OnDisable()
        {
            skxboxMaterial.SetFloat(seedProperty, startingSeed);
        }

        private void Awake()
        {
            seedProperty = Shader.PropertyToID("_Seed");

            if (seedProperty >= 0)
            {
                skxboxMaterial = RenderSettings.skybox;
                startingSeed = skxboxMaterial.GetFloat(seedProperty);

                skxboxMaterial.SetFloat(seedProperty, Seed);
            }
        }
    }
}
