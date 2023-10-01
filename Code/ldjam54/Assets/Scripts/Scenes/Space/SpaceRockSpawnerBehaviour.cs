using System;
using System.Collections.Generic;

using Assets.Scripts.Core;

using GameFrame.Core.Extensions;

using UnityEngine;

using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Scenes.Space
{
    public class SpaceRockSpawnerBehaviour : MonoBehaviour
    {
        private readonly List<GameObject> spaceRockModels = new List<GameObject>();
        
        private GameState gameState;
        private Double junkSpawnInterval = -1;

        private Transform instanceParent;

        private void Awake()
        {
            var gameState = Base.Core.Game.State;

            if (gameState != default)
            {
                this.gameState = gameState;
                this.junkSpawnInterval = gameState.Mode.JunkSpawnInterval;
            }

            foreach (Transform child in this.transform.Find("Templates"))
            {
                spaceRockModels.Add(child.gameObject);
            }

            instanceParent = this.transform.Find("Instances");
        }

        private void Update()
        {
            if (junkSpawnInterval > 0)
            {
                if (gameState.NextJunkSpawn < 0)
                {
                    SpawnRandomRock();
                    gameState.NextJunkSpawn = junkSpawnInterval;
                }
                else
                {
                    gameState.NextJunkSpawn -= Time.deltaTime;
                }
            }
        }

        private void SpawnRandomRock()
        {
            var template = spaceRockModels.GetRandomEntry();

            var newRock = Instantiate(template, instanceParent);

            var randomPosition = CreateRandomVector(gameState.Mode.JunkSpawnPosition.Min, gameState.Mode.JunkSpawnPosition.Max);
            var initialDistance = (float)gameState.Mode.JunkSpawnInitialDistance / randomPosition.sqrMagnitude;

            newRock.transform.position = randomPosition;
            newRock.SetActive(true);

            var rb = newRock.GetComponent<GravityBehaviour>().Rb;

            // ?? dot product for perpendicularity eg
            rb.velocity = Vector3.Cross(randomPosition, Vector3.up) * initialDistance;

            // rb.velocity = CreateRandomVector(-initdist, initdist);
            rb.AddForce(CreateRandomVector(gameState.Mode.JunkSpawnForce.Min * initialDistance, gameState.Mode.JunkSpawnForce.Max * initialDistance));
            rb.AddTorque(CreateRandomVector(gameState.Mode.JunkSpawnTorque.Min, gameState.Mode.JunkSpawnTorque.Max));
        }

        private Vector3 CreateRandomVector(float min, float max)
        {
            //return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
            return new Vector3(UnityEngine.Random.Range(min, max), 0, UnityEngine.Random.Range(min, max));
        }
    }
}
