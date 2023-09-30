using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class SpaceBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject GameOverText;


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
