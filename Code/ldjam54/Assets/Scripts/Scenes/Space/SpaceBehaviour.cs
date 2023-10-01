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
        private GameObject GameOverText;

        [SerializeField]
        private GameObject ShipTemplate;

        [SerializeField]
        private List<InputPadBehaviour> InputPadBehaviours;

        public List<SpaceShipBehaviour> spaceShipBehaviours { private set; get; }

        private void Awake()
        {
            if (Base.Core.Game.State == default)
            {
                Base.Core.Game.Start();
            }

        }


        private void Start()
        {
            var keyBindings = new List<Dictionary<String, KeyCode>>();
            keyBindings.Add(GetKeybindingsWASD());
            keyBindings.Add(GetKeybindingsArrows());
            int length = Base.Core.Game.State.Mode.Spacecrafts.Count;
            spaceShipBehaviours = new List<SpaceShipBehaviour>();
            for (int i = 0; i < length; i++)
            {
                var behaviour = SpawnShip(keyBindings[i], InputPadBehaviours[i]);
                spaceShipBehaviours.Add(behaviour);
            }
        }

        private Dictionary<String, KeyCode> GetKeybindingsWASD()
        {
            var keyDict = new Dictionary<String, KeyCode>{
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
            var keyDict = new Dictionary<String, KeyCode>{
                { "Accelerate", KeyCode.UpArrow },
            { "DeAccelerate", KeyCode.DownArrow },
            { "TurnLeft", KeyCode.LeftArrow },
            { "TurnRight", KeyCode.RightArrow },
            { "FireProjectile", KeyCode.RightControl }
            };
            return keyDict;
        }

        private SpaceShipBehaviour SpawnShip(Dictionary<String, KeyCode> keybindings, InputPadBehaviour padBehaviour)
        {
            var ship = Instantiate(ShipTemplate, ShipTemplate.transform.parent.parent.Find("Instances"));

            var vec = new Vector3(UnityEngine.Random.Range(-100, 100), 0, UnityEngine.Random.Range(-100, 100));
            vec = vec.normalized * (float)Core.Game.SelectedGameMode.ShipSpawnDistance;
            ship.transform.position = vec;
            var shipBehaviour = ship.GetComponent<SpaceShipBehaviour>();
            padBehaviour.Init(shipBehaviour);   
            var spaceCraft = Base.Core.Game.State.Spacecraft;
           
            shipBehaviour.SpawnShip(spaceCraft, keybindings);
            ship.SetActive(true);
            return shipBehaviour;
        }


        public void Restart()
        {
            Time.timeScale = 1;
            Base.Core.Game.ChangeScene(Constants.SceneNames.Space);
            Time.timeScale = 1;
        }

        public void TriggerGameOver()
        {
            Base.Core.Game.ChangeScene(Constants.SceneNames.GameOver);
        }
    }
}
