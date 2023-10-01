using Assets.Scripts.Core;

using GameFrame.Core.UI.List;

using TMPro;

using UnityEngine;

namespace Assets.Scripts.Scenes.GameModes
{
    public class GameModeListSlotBehaviour : ListSlotBehaviour<GameModeListItem>
    {
        private TMP_Text nameText;
        private TMP_Text descriptionText;

        [SerializeField]
        private GameModesBehaviour gameModeBehaviour;

        public GameModeListItem GameModeItem
        {
            get
            {
                return this.content;
            }
        }

        public override void RudeAwake()
        {
            nameText = transform.Find("Info/NameText").GetComponent<TMP_Text>();
            descriptionText = transform.Find("Info/DescriptionText").GetComponent<TMP_Text>();
        }

        public override void UpdateUI()
        {
            
            nameText.text = content.Name;
            descriptionText.text = content.Description;
        }

        public void SelectThisGameMode()
        {
            Game.SelectedGameMode = content.GameMode;
            gameModeBehaviour.UpdateSelectedMode();
        }
    }
}
