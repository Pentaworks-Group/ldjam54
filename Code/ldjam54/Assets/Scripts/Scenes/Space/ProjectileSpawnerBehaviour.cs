using System.Collections;
using System.Collections.Generic;

using GameFrame.Core.Extensions;

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

        public void SpawnProjectile(Transform source)
        {
            var newProjectile = Instantiate(template, instanceParent);
            newProjectile.transform.SetPositionAndRotation(source.position + source.forward * 0.9f, source.rotation);
            newProjectile.SetActive(true);

            var projectileBehaviour = newProjectile.GetComponent<ProjectileBehaviour>();

            //gravityBehaviour.Init();
            var rb = projectileBehaviour.Rb;
            rb.mass = 0.01f;
            rb.AddForce(source.transform.forward * speed);
        }

    }
}
