using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class SpaceBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject GameOverText;

        [SerializeField]
        private GameObject ShipTemplate;


        private void Awake()
        {
            var ship = Instantiate(ShipTemplate);
            var shipBehaviour = ship.GetComponent<SpaceShipBehaviour>();
            var spaceCraft = Base.Core.Game.State.Spacecraft;
            var keyDict = new Dictionary<String, KeyCode>{
                { "Accelerate", KeyCode.W },
            { "DeAccelerate", KeyCode.S },
            { "TurnLeft", KeyCode.A },
            { "TurnRight", KeyCode.D },
            { "FireProjectile", KeyCode.Space }
            };
            shipBehaviour.SpawnShip(spaceCraft, keyDict);
            ship.SetActive(true);
        }

        public void Restart()
        {
            Time.timeScale = 1;
            Base.Core.Game.ChangeScene(Constants.SceneNames.Space);
        }

        public void TriggerGameOver()
        {
            Time.timeScale = 0;
            GameOverText.SetActive(true);
        }
    }
}
