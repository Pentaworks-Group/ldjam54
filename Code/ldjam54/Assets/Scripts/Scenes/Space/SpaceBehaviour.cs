using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Scenes.Space.InputHandling;

using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class SpaceBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject ShipTemplate;

        [SerializeField]
        private List<Color> colors;

        [SerializeField]
        private List<InputPadBehaviour> InputPadBehaviours;

        public List<SpaceShipBehaviour> spaceShipBehaviours { get; private set; }

        [SerializeField]
        private GravityManagerBehaviour starBehaviour;
        public GravityManagerBehaviour StarBehaviour
        {
            get
            {
                return this.starBehaviour;
            }
        }

        private void Awake()
        {
            if (Base.Core.Game.State == default)
            {
                Base.Core.Game.Start();
            }

            if (TryGetComponent<SkyboxShaderUpdater>(out var skyboxShaderUpdater))
            {
                skyboxShaderUpdater.Skybox = Base.Core.Game.State.Skybox;
                skyboxShaderUpdater.UpdateSkybox();
            }
        }

        private void Start()
        {
            var keyBindings = new List<Dictionary<String, KeyCode>>()
            {
                GetKeybindingsWASD(),
                GetKeybindingsArrows()
            };

            int length = Base.Core.Game.State.Mode.Spacecrafts.Count;

            spaceShipBehaviours = new List<SpaceShipBehaviour>();
            for (int i = 0; i < length; i++)
            {
                var behaviour = SpawnShip(keyBindings[i], InputPadBehaviours[i], "Player" + (i + 1), colors[i]);
                spaceShipBehaviours.Add(behaviour);
            }
        }

        private Dictionary<String, KeyCode> GetKeybindingsWASD()
        {
            var keyDict = new Dictionary<String, KeyCode>()
            {
                { "Accelerate", KeyCode.W },
                { "DeAccelerate", KeyCode.S },
                { "TurnLeft", KeyCode.A },
                { "TurnRight", KeyCode.D },
                { "FireProjectile", KeyCode.Space }
            };

            return keyDict;
        }

        private Dictionary<String, KeyCode> GetKeybindingsArrows()
        {
            var keyDict = new Dictionary<String, KeyCode>()
            {
                { "Accelerate", KeyCode.UpArrow },
                { "DeAccelerate", KeyCode.DownArrow },
                { "TurnLeft", KeyCode.LeftArrow },
                { "TurnRight", KeyCode.RightArrow },
                { "FireProjectile", KeyCode.RightControl }
            };

            return keyDict;
        }

        private SpaceShipBehaviour SpawnShip(Dictionary<String, KeyCode> keybindings, InputPadBehaviour padBehaviour, String shipName, Color color)
        {
            var ship = Instantiate(ShipTemplate, ShipTemplate.transform.parent.parent.Find("Instances"));

            var vec = new Vector3(UnityEngine.Random.Range(-100, 100), 0, UnityEngine.Random.Range(-100, 100));

            vec = vec.normalized * (float)Base.Core.Game.State.Mode.ShipSpawnDistance;

            ship.transform.position = vec;

            var shipBehaviour = ship.GetComponent<SpaceShipBehaviour>();

            padBehaviour.Init(shipBehaviour);
            var spaceCraft = Base.Core.Game.State.Spacecraft;

            shipBehaviour.SpawnShip(spaceCraft, keybindings, shipName, color);
            ship.SetActive(true);

            return shipBehaviour;
        }

        public void Restart()
        {
            Time.timeScale = 1;
            Base.Core.Game.ChangeScene(Constants.SceneNames.Space);
            Time.timeScale = 1;
        }

        public void TriggerGameOver(SpaceShipBehaviour spaceShipBehaviour)
        {
            Base.Core.Game.State.DeadShips.Add(spaceShipBehaviour.gameObject.name, spaceShipBehaviour.deathMessage);
            spaceShipBehaviours.Remove(spaceShipBehaviour);
            StartCoroutine(CheckSurvivorCount());
        }

        IEnumerator CheckSurvivorCount()
        {
            yield return new WaitForSeconds(3);

            if (spaceShipBehaviours.Count <= Base.Core.Game.State.Mode.RequiredSurvivors)
            {
                Base.Core.Game.ChangeScene(Constants.SceneNames.GameOver);
            }
        }
    }
}
