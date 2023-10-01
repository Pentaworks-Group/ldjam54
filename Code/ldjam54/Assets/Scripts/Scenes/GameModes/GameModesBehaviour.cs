using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Scenes.Menues;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.GameModes
{
    public class GameModesBehaviour : BaseMenuBehaviour
    {
        private IList<GameMode> selectedGameModes;
        [SerializeField]
        private TextMeshProUGUI nameText;
        [SerializeField]
        private TextMeshProUGUI descriptionText;
        [SerializeField]
        private Button selectedStartButton;


        private void Start()
        {
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

        public void UpdateSelectedMode()
        {
            if (Core.Game.SelectedGameMode != default)
            {
                nameText.text = Core.Game.SelectedGameMode.Name;
                descriptionText.text = Core.Game.SelectedGameMode.Description;
                selectedStartButton.interactable = true;
            } else
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
    }
}
