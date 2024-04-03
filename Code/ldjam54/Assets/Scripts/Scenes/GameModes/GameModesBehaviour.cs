using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Scenes.Menues;

using GameFrame.Core.Persistence;

using Newtonsoft.Json.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.GameModes
{
    public class GameModesBehaviour : BaseMenuBehaviour
    {
        private GameModeController<GameModePreview, GameMode> GameModeController { get; set; }
        private Dictionary<string, GameModePreview> buildInGameModes;


        [SerializeField]
        private TextMeshProUGUI nameText;
        [SerializeField]
        private TextMeshProUGUI descriptionText;
        [SerializeField]
        private Button selectedStartButton;

        [SerializeField]
        private JsonEditorManagerBehaviour jsonEditorManagerBehaviour;


        [SerializeField]
        private TextMeshProUGUI currentGameModeSelectionText;
        [SerializeField]
        private GameModePreviewListBehaviour GameModeList;

        private bool BuildInSelected = true;
        private const string BuildInGameModeLabel = "BuildIn";
        private const string CustomGameModeLabel = "Custom";



        private GameMode editGameMode;


        private void Start()
        {
            GameModeController = new GameModeController<GameModePreview, GameMode>();
            UpdateSelectedMode();
        }

        public Dictionary<string, GameModePreview> GetBuildInGameModes()
        {
            if (buildInGameModes == null)
            {

                buildInGameModes = new Dictionary<string, GameModePreview>();
                foreach (var gameMode in Base.Core.Game.AvailableGameModes)
                {
                    var gameModePreview = new GameModePreview();
                    gameModePreview.Name = gameMode.Name;
                    gameModePreview.Description = gameMode.Description;
                    gameModePreview.Reference = gameMode.Reference;
                    gameModePreview.GameMode = gameMode;
                    buildInGameModes.Add(gameMode.Reference, gameModePreview);

                }
            }
            return buildInGameModes;
        }

        public Dictionary<string, GameModePreview> GetSelectedGameModes()
        {
            if (BuildInSelected)
            {
                return GetBuildInGameModes();
            } else
            {
                return GameModeController.GameModePreviews;
            }
        }

        public void ToggleGameModeSelection()
        {
            if (BuildInSelected)
            {
                BuildInSelected = false;
                currentGameModeSelectionText.text = CustomGameModeLabel;
            }
            else
            {
                BuildInSelected = true;
                currentGameModeSelectionText.text = BuildInGameModeLabel;
            }
            GameModeList.UpdateList();
        }

        public void SelectCustomGameMode(GameModePreview gameModePreview)
        {
            GameMode gameMode;
            if (gameModePreview.GameMode == default)
            {
                gameMode = GameModeController.LoadGameMode(gameModePreview.Reference);
            }
            else
            {
                gameMode = gameModePreview.GameMode;
            }
            Core.Game.SelectedGameMode = gameMode;
            UpdateSelectedMode();
        }

        public void UpdateSelectedMode()
        {
            if (Core.Game.SelectedGameMode != default)
            {
                nameText.text = Core.Game.SelectedGameMode.Name;
                descriptionText.text = Core.Game.SelectedGameMode.Description;
                selectedStartButton.interactable = true;
            }
            else
            {
                selectedStartButton.interactable = false;
            }
        }

        public void StartSelectedGame()
        {
            if (Core.Game.SelectedGameMode != default)
            {
                Base.Core.Game.Start();
            }
        }

        public void EditGameModePreview(GameModePreview gameModePreview)
        {
            GameMode gameMode;
            if (gameModePreview.GameMode == default)
            {
                gameMode = GameModeController.LoadGameMode(gameModePreview.Reference);
            } else
            {
                gameMode = gameModePreview.GameMode;
            }
            editGameMode = gameMode;
            jsonEditorManagerBehaviour.OpenEditorForThisObject(editGameMode, SaveGameMode);
        }


        public void SaveGameMode(JObject gameModeObject)
        {
            var gameMode = gameModeObject.ToObject<GameMode>();
            if (gameMode.Reference == null || gameMode.Reference == "")
            {
                gameMode.Reference = GetNewUniqueGameModeName();
            }
            else if (gameMode.Reference != editGameMode.Reference && DoesAlreadyGameModeWithNameExist())
            {
                DisplayOverridePrompt();
                return;
            }
            PersistGameMode(gameMode);

            GameModeList.UpdateList();

        }

        private void PersistGameMode(GameMode gameMode)
        {
            GameModeController.PersistObject(gameMode.Reference, gameMode);
        }


        private void DisplayOverridePrompt()
        {

        }

        private bool DoesAlreadyGameModeWithNameExist()
        {
            return false;
        }

        private string GetNewUniqueGameModeName()
        {
            return "111111";
        }
    }
}
