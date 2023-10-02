using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Scenes.Space.InputHandling;

using GameFrame.Core.Extensions;

using TMPro;

using UnityEngine;

using Vector3 = UnityEngine.Vector3;

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

        [SerializeField]
        private List<TextMeshProUGUI> killCountDisplays;

        [SerializeField]
        private TextMeshProUGUI timeElapsedDisplay;


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
                var junkKillCounter = killCountDisplays[i];

                var behaviour = SpawnShip(keyBindings[i], this.InputPadBehaviours[i], "Player" + (i + 1), this.colors[i], junkKillCounter);
                spaceShipBehaviours.Add(behaviour);
            }
        }

        private void Update()
        {
            Base.Core.Game.State.TimeElapsed += Time.deltaTime;
            timeElapsedDisplay.text = Base.Core.Game.State.TimeElapsed.ToString("F1");
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

        private SpaceShipBehaviour SpawnShip(Dictionary<String, KeyCode> keybindings, InputPadBehaviour padBehaviour, String shipName, Color color, TextMeshProUGUI junkKillCounter)
        {
            var ship = Instantiate(ShipTemplate, ShipTemplate.transform.parent.parent.Find("Instances"));

            var spacecraft = Base.Core.Game.State.Spacecraft;

            var shipBehaviour = ship.GetComponent<SpaceShipBehaviour>();

            padBehaviour.Init(shipBehaviour);

            junkKillCounter.transform.parent.gameObject.SetActive(true);

            shipBehaviour.SpawnShip(spacecraft, keybindings, shipName, color, junkKillCounter);
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
            Base.Core.Game.State.DeadShips.Add(spaceShipBehaviour.gameObject.name, spaceShipBehaviour.DeathMessage);
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
