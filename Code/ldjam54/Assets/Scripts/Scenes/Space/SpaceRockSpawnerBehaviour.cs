using System.Collections;
using System.Collections.Generic;

using GameFrame.Core.Extensions;

using UnityEngine;

using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Scenes.Space
{

    public class SpaceRockSpawnerBehaviour : MonoBehaviour
    {
        private List<GameObject> spaceRockModels = new List<GameObject>();

        private float spawnInterval = 1f;
        private float lastSpawn = 5f;

        private Transform instanceParent;


        private void Awake()
        {
            foreach (Transform child in this.transform.Find("Templates"))
            {
                spaceRockModels.Add(child.gameObject);
            }
            instanceParent = this.transform.Find("Instances");
        }

        private void Update()
        {
            if (lastSpawn > spawnInterval)
            {
                SpawnRandomRock();
                lastSpawn = 0;
            }
            else
            {
                lastSpawn += Time.deltaTime;
            }
        }

        private void SpawnRandomRock()
        {
            var template = spaceRockModels.GetRandomEntry();
            var newRock = Instantiate(template, instanceParent);
            var randPosition = CreateRandomVector(-7, 7);
            var initdist = 20/randPosition.sqrMagnitude;
            
            newRock.transform.position = randPosition;
            newRock.SetActive(true);
            
            var rb = newRock.GetComponent<GravityBehaviour>().Rb;

            //rb.isKinematic = false; //Set as it doesnt take velocity into account otherwise. See documentation
            // ?? dot product for perpendicularity eg
            rb.velocity = Vector3.Cross (randPosition, Vector3.up) * initdist;

            // rb.velocity = CreateRandomVector(-initdist, initdist);
            rb.AddForce(CreateRandomVector(-6*initdist, 6*initdist));
            rb.AddTorque(CreateRandomVector(-3, 3));
         }

        private Vector3 CreateRandomVector(float min, float max)
        {
            //return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
            return new Vector3(Random.Range(min, max), 0, Random.Range(min, max));
        }
    }
}
