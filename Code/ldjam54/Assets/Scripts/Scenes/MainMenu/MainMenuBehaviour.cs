using System;

using Assets.Scripts.Constants;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        public void PlayGame()
        {
            //GameFrame.Base.Audio.Background.ReplaceClips(Base.Core.Game.AudioClipListGame);
            Base.Core.Game.PlayButtonSound();

            Base.Core.Game.Start();
        }

        public void ShowGameModes()
        {
            ShowScene(SceneNames.GameModes);
        }

        public void ShowSavedGames()
        {
            ShowScene(SceneNames.SavedGames);
        }

        public void ShowOptions()
        {
            ShowScene(SceneNames.Options);
        }

        public void ShowCredits()
        {
            ShowScene(SceneNames.Credits);
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

        private void ShowScene(String sceneName)
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.ChangeScene(sceneName);
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
