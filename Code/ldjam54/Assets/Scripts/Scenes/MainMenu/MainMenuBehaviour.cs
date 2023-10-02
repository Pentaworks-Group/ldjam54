using System;

using Assets.Scripts.Constants;

using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void Quit();

        public void PlayGame()
        {
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
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            Quit();
#elif UNITY_STANDALONE
            Application.Quit();            
#endif
        }

        private void ShowScene(String sceneName)
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.ChangeScene(sceneName);
        }

        private void Awake()
        {
            GameObject.Find("UI/Fitter/VersionText").GetComponent<TMPro.TMP_Text>().text = $"Version: {Application.version}";
            
            if (TryGetComponent<SkyBoxShaderUpdater>(out var skyBoxShaderUpdater))
            {
                skyBoxShaderUpdater.Skybox = Base.Core.Game.Skybox;
            }
        }
    }
}
