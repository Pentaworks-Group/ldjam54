using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Constants;
using Assets.Scripts.Core.Model;
using Assets.Scripts.Scenes.Space.InputHandling;

using TMPro;

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

        [SerializeField]
        private List<TextMeshProUGUI> killCountDisplays;

        [SerializeField]
        private TextMeshProUGUI timeElapsedDisplay;


        public List<SpacecraftBehaviour> playerSpacecraftBehaviours { get; private set; }

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
        }

        private void Start()
        {
            if (TryGetComponent<SkyboxShaderUpdater>(out var skyboxShaderUpdater))
            {
                var gameState = Base.Core.Game.State;

                skyboxShaderUpdater.Skybox = gameState.Skybox;
                skyboxShaderUpdater.UpdateSkybox();
            }

            var keyBindings = new List<Dictionary<String, KeyCode>>()
            {
                GetKeybindingsWASD(),
                GetKeybindingsArrows()
            };

            playerSpacecraftBehaviours = new List<SpacecraftBehaviour>();

            var playerSpacecrafts = Base.Core.Game.State.PlayerSpacecraft;

            for (int i = 0; i < playerSpacecrafts.Count; i++)
            {
                var spacecraft = playerSpacecrafts[i];

                var junkKillCounter = killCountDisplays[i];

                var behaviour = SpawnShip(spacecraft, keyBindings[i], this.InputPadBehaviours[i], "Player-" + (i + 1), this.colors[i], junkKillCounter);
                playerSpacecraftBehaviours.Add(behaviour);
            }

            var aiSpacecrafts = Base.Core.Game.State.Spacecrafts;

            if (aiSpacecrafts.Count > 0)
            {
                throw new NotSupportedException("AI Ships are notyet supported!");
            }

            //for (int i = 0; i < aiSpacecrafts.Count; i++)
            //{                
            //var spacecraft = aiSpacecrafts[i];

            //var junkKillCounter = killCountDisplays[i];

            //var behaviour = SpawnShip(spacecraft, keyBindings[i], this.InputPadBehaviours[i], "AI-" + (i + 1), this.colors[i], junkKillCounter);
            //playerSpacecraftBehaviours.Add(behaviour);
            //}
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
                { KeyActions.Accelerate, KeyCode.W },
                { KeyActions.Decelerate, KeyCode.S },
                { KeyActions.TurnLeft, KeyCode.A },
                { KeyActions.TurnRight, KeyCode.D },
                { KeyActions.FireProjectile, KeyCode.Space }
            };

            return keyDict;
        }

        private Dictionary<String, KeyCode> GetKeybindingsArrows()
        {
            var keyDict = new Dictionary<String, KeyCode>()
            {
                { KeyActions.Accelerate, KeyCode.UpArrow },
                { KeyActions.Decelerate, KeyCode.DownArrow },
                { KeyActions.TurnLeft, KeyCode.LeftArrow },
                { KeyActions.TurnRight, KeyCode.RightArrow },
                { KeyActions.FireProjectile, KeyCode.RightControl }
            };

            return keyDict;
        }

        private SpacecraftBehaviour SpawnShip(Spacecraft spacecraft, Dictionary<String, KeyCode> keybindings, InputPadBehaviour padBehaviour, String shipName, Color color, TextMeshProUGUI junkKillCounter)
        {
            var ship = Instantiate(ShipTemplate, ShipTemplate.transform.parent.parent.Find("Instances"));

            var shipBehaviour = ship.GetComponent<SpacecraftBehaviour>();

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

        public void TriggerGameOver(SpacecraftBehaviour spaceShipBehaviour)
        {
            Base.Core.Game.State.DeadShips.Add(spaceShipBehaviour.gameObject.name, spaceShipBehaviour.DeathMessage);
            playerSpacecraftBehaviours.Remove(spaceShipBehaviour);
            StartCoroutine(CheckSurvivorCount());
        }

        IEnumerator CheckSurvivorCount()
        {
            yield return new WaitForSeconds(3);

            if (playerSpacecraftBehaviours.Count <= Base.Core.Game.State.Mode.RequiredSurvivors)
            {
                Base.Core.Game.ChangeScene(Constants.SceneNames.GameOver);
            }
        }
    }
}
