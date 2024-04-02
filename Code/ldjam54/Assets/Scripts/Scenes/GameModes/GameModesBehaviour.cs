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
        private IList<GameMode> selectedGameModes;


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
        private GameModeListBehaviour BuildInGameModes;
        [SerializeField]
        private GameModePreviewListBehaviour CustomGameModes;

        private bool BuildInSelected = true;
        private const string BuildInGameModeLabel = "BuildIn";
        private const string CustomGameModeLabel = "Custom";



        private GameMode editGameMode;


        private void Start()
        {
            GameModeController = new GameModeController<GameModePreview, GameMode>();
            UpdateSelectedMode();
        }

        public void SelectBuildInGameModes()
        {
            selectedGameModes = Base.Core.Game.AvailableGameModes;
        }

        public IList<GameMode> GetSelectedGameModes()
        {
            if (selectedGameModes == default)
            {
                SelectBuildInGameModes();
            }
            return selectedGameModes;
        }

        public IDictionary<string, GameModePreview> GetGameModePreviews()
        {
            return GameModeController.GameModePreviews;
        }

        public void ToggleGameModeSelection()
        {
            if (BuildInSelected)
            {
                BuildInSelected = false;
                BuildInGameModes.gameObject.SetActive(false);
                CustomGameModes.gameObject.SetActive(true);
                currentGameModeSelectionText.text = CustomGameModeLabel;
            }
            else
            {
                BuildInSelected = true;
                BuildInGameModes.gameObject.SetActive(true);
                CustomGameModes.gameObject.SetActive(false);
                currentGameModeSelectionText.text = BuildInGameModeLabel;
            }
        }

        public void SelectCustomGameMode(string gameModeReference)
        {
            var gameMode = GameModeController.LoadGameMode(gameModeReference);
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

        public void EditCustomGameMode(string gameModeReference)
        {
            var gameMode = GameModeController.LoadGameMode(gameModeReference);
            EditGameMode(gameMode);
        }

        public void EditGameMode(GameMode gameMode)
        {
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

            CustomGameModes.UpdateList();

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
