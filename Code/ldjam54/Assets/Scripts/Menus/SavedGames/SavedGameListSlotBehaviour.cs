using System.Collections.Generic;

using Assets.Scripts.Core;

using GameFrame.Core.UI.List;

using UnityEngine.UI;

namespace Assets.Scripts.Scene.SaveGame
{
    public class SavedGameListSlotBehaviour : ListSlotBehaviour<KeyValuePair<string, SavedGamePreview>>
    {
        private Text createdOn;
        private Text timeStamp;

        public override void RudeAwake()
        {
            createdOn = transform.Find("SlotContainer/Info/Created").GetComponent<Text>();
            timeStamp = transform.Find("SlotContainer/Info/TimeStamp").GetComponent<Text>();
        }

        public SavedGamePreview GetSavedGamedPreview()
        {
            return content.Value;
        }

        public void DisplaySlot(SavedGameDetailBehaviour details)
        {
            details.DisplayDetails(GetSavedGamedPreview());
        }

        public override void UpdateUI()
        {
            SavedGamePreview savedGame = GetSavedGamedPreview();

            createdOn.text = savedGame.CreatedOn;
            timeStamp.text = savedGame.SavedOn;

        }

        public void LoadGame()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.LoadSavedGame(content.Key);
        }
    }
}
