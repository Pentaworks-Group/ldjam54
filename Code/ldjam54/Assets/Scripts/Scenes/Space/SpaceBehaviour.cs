using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Constants;
using Assets.Scripts.Core.Model;
using Assets.Scripts.Scenes.Space.InputHandling;

using TMPro;

using UnityEngine;
using UnityEngine.Events;

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
        public List<SpaceJunkBehaviour> spaceJunkBehaviours { get; private set; } = new List<SpaceJunkBehaviour>();

        [SerializeField]
        private GravityManagerBehaviour starBehaviour;
        public GravityManagerBehaviour StarBehaviour
        {
            get
            {
                return this.starBehaviour;
            }
        }


        private float timeUpdate = 0;


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
                skyboxShaderUpdater.Skybox = Base.Core.Game.State.Skybox;
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
            if (timeUpdate < 0)
            {
                timeElapsedDisplay.text = Base.Core.Game.State.TimeElapsed.ToString("F1");
                timeUpdate = 1;
            }
            else
            {
                timeUpdate -= Time.deltaTime;
            }
        }

        public void SaveGame()
        {
            Base.Core.Game.PlayButtonSound();
            SerializeToGameState();
            Base.Core.Game.SaveGame();
            //callback.Invoke();
        }


        public void SerializeToGameState()
        {
            SerializeSpaceCrafts();
            SerializeSpaceJunks();
        }

        private void SerializeSpaceJunks()
        {
            //Base.Core.Game.State.SpaceJunks.Clear();
            foreach (var spaceJunk in spaceJunkBehaviours)
            {
                spaceJunk.SerializeSpaceJunk();
                //Base.Core.Game.State.SpaceJunks.Add(spaceJunk.spaceJunk);
            }
        }

        private void SerializeSpaceCrafts()
        {
            //Base.Core.Game.State.PlayerSpacecraft.Clear();
            foreach (var spaceCraft in playerSpacecraftBehaviours)
            {
                spaceCraft.SerializeState();
                //Base.Core.Game.State.PlayerSpacecraft.Add(spaceCraft.spacecraft);
            }
        }

        public void RegisterSpaceJunk(SpaceJunkBehaviour spaceJunkBehaviour)
        {
            spaceJunkBehaviours.Add(spaceJunkBehaviour);
        }

        public void RemoveSpaceJunk(SpaceJunkBehaviour spaceJunkBehaviour)
        {
            spaceJunkBehaviours.Remove(spaceJunkBehaviour);
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
