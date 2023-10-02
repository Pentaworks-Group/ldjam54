using System;
using System.Runtime.InteropServices;

using Assets.Scripts.Constants;

using UnityEngine;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void Quit();

        [SerializeField]
        private GameObject Tutorial;
        [SerializeField]
        private GameObject Menu;

        private SkyboxShaderUpdater skyboxShaderUpdater;

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
            ShowScene(SceneNames.Credits, false);
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

        private void ShowScene(String sceneName, Boolean playButtonSound = true)
        {
            if (playButtonSound)
            {
                Base.Core.Game.PlayButtonSound();
            }

            Base.Core.Game.ChangeScene(sceneName);
        }

        public void ShowTutorial()
        {
            Tutorial.SetActive(true);
            Menu.SetActive(false);
        }

        public void HideTutorial()
        {
            Base.Core.Game.PlayButtonSound();
            Tutorial.SetActive(false);
            Menu.SetActive(true);
        }

        private void Awake()
        {
            GameObject.Find("UI/Fitter/Menu/Area/VersionText").GetComponent<TMPro.TMP_Text>().text = $"Version: {Application.version}";

            if (TryGetComponent<SkyboxShaderUpdater>(out var skyboxShaderUpdater))
            {
                this.skyboxShaderUpdater = skyboxShaderUpdater;

                skyboxShaderUpdater.Skybox = Base.Core.Game.Skybox;
            }
        }

        private void Start()
        {
            skyboxShaderUpdater?.UpdateSkybox();
        }
    }
}
