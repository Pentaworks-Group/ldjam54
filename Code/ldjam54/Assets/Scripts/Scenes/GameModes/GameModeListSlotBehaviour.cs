using System;

using Assets.Scripts.Core;

using GameFrame.Core.UI.List;

using TMPro;

using Unity.VisualScripting;

using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.GameModes
{
    public class GameModeListSlotBehaviour : ListSlotBehaviour<GameModeListItem>
    {
        private TMP_Text nameText;
        private TMP_Text descriptionText;


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

        public void OnPointerClick(PointerEventData eventdata)
        {
            Game.SelectedGameMode = content.GameMode;
        }
    }
}
