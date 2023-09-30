using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Constants;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        public void ShowSavedGames()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.ChangeScene(SceneNames.SavedGames);
        }

        public void ShowOptions()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.ChangeScene(SceneNames.Options);
        }

        public void ShowCredits()
        {
            // No Button sound required as page plays a sound for itself
            Base.Core.Game.ChangeScene(SceneNames.Credits);
        }

        public void PlayGame()
        {
            //GameFrame.Base.Audio.Background.ReplaceClips(Base.Core.Game.AudioClipListGame);
            Base.Core.Game.PlayButtonSound();

            Base.Core.Game.Start();
        }

        public void ShowModes()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.ChangeScene(SceneNames.GameModes);
        }

        public void QuitGame()
        {
#if UNITY_WEBGL
            Debug.Log("Quitti");
            Application.ExternalEval("document.location.reload(true)");
#elif UNITY_STANDALONE
            Application.Quit();
#elif UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Debug.Log("dada");
        }

        private void Awake()
        {
            GameObject.Find("UI/Fitter/VersionText").GetComponent<TMPro.TMP_Text>().text = $"Version: {Application.version}";

            if (GameObject.Find("UI/Fitter/QuitButton").TryGetComponent(out Button quitButton))
            {
                quitButton.interactable = Base.Core.Game.IsFileAccessPossible;
            }

            //GameFrame.Base.Audio.Background.ReplaceClips(Base.Core.Game.AudioClipListMenu);
        }
    }
}
