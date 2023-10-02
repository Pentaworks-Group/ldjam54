using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{

    public class ProjectileSpawnerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject template;

        private float speed = 5f;

        private Transform instanceParent;

        private void Awake()
        {
            instanceParent = this.transform.Find("Instances");
        }

        //private void Update()
        //{
        //    if (lastSpawn > spawnInterval)
        //    {
        //        SpawnRandomRock();
        //        lastSpawn = 0;
        //    }
        //    else
        //    {
        //        lastSpawn += Time.deltaTime;
        //    }
        //}

        public void SpawnProjectile(SpacecraftBehaviour source)
        {
            var newProjectile = Instantiate(template, instanceParent);
            var sourceTransform = source.transform;
            newProjectile.transform.SetPositionAndRotation(sourceTransform.position + sourceTransform.forward * 0.9f, sourceTransform.rotation);
            newProjectile.SetActive(true);

            var projectileBehaviour = newProjectile.GetComponent<ProjectileBehaviour>();
            projectileBehaviour.Init(source);

            var rb = projectileBehaviour.Rb;
            rb.mass = 0.01f;
            rb.AddForce(source.transform.forward * speed);
        }

    }
}
