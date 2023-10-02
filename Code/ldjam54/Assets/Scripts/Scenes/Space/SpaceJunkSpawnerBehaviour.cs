using System;
using System.Collections.Generic;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Model;

using GameFrame.Core.Extensions;

using UnityEngine;

using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Scenes.Space
{
    public class SpaceJunkSpawnerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SpaceBehaviour spaceBehaviour;

        private readonly Dictionary<String, GameObject> spaceJunkModels = new Dictionary<String, GameObject>();

        private GameState gameState;
        private Double junkSpawnInterval = -1;

        private Transform instanceParent;
        private GameObject containerTemplate;

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
                spaceJunkModels[child.gameObject.name] = child.gameObject;
            }

            instanceParent = this.transform.Find("Instances");
            containerTemplate = this.transform.Find("Junk").gameObject;

            if (gameState?.SpaceJunks?.Count >0)
            {
                foreach (var spaceJunk in gameState.SpaceJunks)
                {
                    SpawnJunk(spaceJunk);
                }
            }
        }

        private void Update()
        {
            if (junkSpawnInterval > 0)
            {
                if (gameState.NextJunkSpawn < 0)
                {
                    SpawnJunk();
                    gameState.NextJunkSpawn = junkSpawnInterval;
                }
                else
                {
                    gameState.NextJunkSpawn -= Time.deltaTime;
                }
            }
        }

        private void SpawnJunk(SpaceJunk spaceJunk = default)
        {
            var template = default(GameObject);

            if (spaceJunk == default)
            {
                spaceJunk = new SpaceJunk();

                gameState.SpaceJunks.Add(spaceJunk);

                template = this.spaceJunkModels.GetRandomEntry().Value;
                spaceJunk.Model = template.name;
            }
            else
            {
                if (spaceJunkModels.TryGetValue(spaceJunk.Model, out var loadedTemplate))
                {
                    template = loadedTemplate;
                }
                else
                {
                    throw new Exception($"SpaceJunk template '{spaceJunk.Model}' not found!");
                }
            }

            var newSpaceJunk = Instantiate(containerTemplate, instanceParent);

            var modelGameObject = Instantiate(template, newSpaceJunk.transform);

            var spaceJunkBehaviour = newSpaceJunk.GetComponent<SpaceJunkBehaviour>();

            spaceBehaviour.RegisterSpaceJunk(spaceJunkBehaviour);
            spaceJunkBehaviour.SetModel(spaceJunk, modelGameObject);

            if (!spaceJunk.Position.HasValue)
            {
                var randomPosition = CreateRandomVector(gameState.Mode.JunkSpawnPosition.Min, gameState.Mode.JunkSpawnPosition.Max);

                newSpaceJunk.transform.position = randomPosition;

                var rockCollider = modelGameObject.GetComponent<Collider>();

                if (this.spaceBehaviour.StarBehaviour != null)
                {
                    if (this.spaceBehaviour.StarBehaviour.TryGetComponent<Collider>(out var starCollider))
                    {
                        while (starCollider.bounds.Intersects(rockCollider.bounds))
                        {
                            newSpaceJunk.transform.position *= 2f;
                        }
                    }
                }

                if (this.spaceBehaviour?.playerSpacecraftBehaviours?.Count > 0)
                {
                    foreach (var spaceShipBehaviour in this.spaceBehaviour.playerSpacecraftBehaviours)
                    {
                        if (spaceShipBehaviour.TryGetComponent<Collider>(out var spacecraftCollider))
                        {
                            if (spacecraftCollider.bounds.Intersects(rockCollider.bounds))
                            {
                                newSpaceJunk.transform.position *= -1f;
                            }
                        }
                    }
                }

                spaceJunk.Position = newSpaceJunk.transform.position.ToFrame();
            }
            else
            {
                newSpaceJunk.transform.position = spaceJunk.Position.Value.ToUnity();
            }

            newSpaceJunk.SetActive(true);

            var rb = spaceJunkBehaviour.Rb;

            if (!spaceJunk.Velocity.HasValue)
            {
                var initialDistance = (float)gameState.Mode.JunkSpawnInitialDistance / newSpaceJunk.transform.position.sqrMagnitude;

                //  dot product for perpendicularity eg
                rb.velocity = Vector3.Cross(newSpaceJunk.transform.position, Vector3.up) * initialDistance;

                rb.AddForce(CreateRandomVector(gameState.Mode.JunkSpawnForce.Min * initialDistance, gameState.Mode.JunkSpawnForce.Max * initialDistance));
                rb.AddTorque(CreateRandomVector(gameState.Mode.JunkSpawnTorque.Min, gameState.Mode.JunkSpawnTorque.Max));

                spaceJunk.Velocity = rb.velocity.ToFrame();
            }
            else
            {
                rb.velocity = spaceJunk.Velocity.Value.ToUnity();
            }

            if (spaceJunk.Rotation.HasValue)
            {
                transform.Rotate(spaceJunk.Rotation.Value.ToUnity(), UnityEngine.Space.World);
            }
        }

        private Vector3 CreateRandomVector(float min, float max)
        {
            //return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
            return new Vector3(UnityEngine.Random.Range(min, max), 0, UnityEngine.Random.Range(min, max));
        }
    }
}
