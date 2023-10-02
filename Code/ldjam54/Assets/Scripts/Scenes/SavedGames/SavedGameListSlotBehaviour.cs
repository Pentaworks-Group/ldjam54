using System.Collections.Generic;

using Assets.Scripts.Core;

using GameFrame.Core.UI.List;

using TMPro;

using UnityEngine.UI;

namespace Assets.Scripts.Scenes.SavedGames
{
    public class SavedGameListSlotBehaviour : ListSlotBehaviour<KeyValuePair<string, SavedGamePreview>>
    {
        private TextAutoSizeController textAutoSizeController;
        private TMP_Text createdOnText;
        private TMP_Text timeStampText;
        private TMP_Text gameModeText;

        public override void RudeAwake()
        {
            createdOnText = transform.Find("SlotContainer/Info/Created").GetComponent<TMP_Text>();
            timeStampText = transform.Find("SlotContainer/Info/TimeStamp").GetComponent<TMP_Text>();
            gameModeText = transform.Find("SlotContainer/Info/GameMode").GetComponent<TMP_Text>();

            if (TryGetComponent<TextAutoSizeController>(out var textAutoSizeController))
            {
                textAutoSizeController.AddLabel(createdOnText);
                textAutoSizeController.AddLabel(timeStampText);
                textAutoSizeController.AddLabel(gameModeText);

                this.textAutoSizeController = textAutoSizeController;
            }
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

            createdOnText.text = savedGame.CreatedOn;
            timeStampText.text = savedGame.SavedOn;
            gameModeText.text = savedGame.GameMode;

            textAutoSizeController?.Execute();
        }

        public void LoadGame()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.LoadSavedGame(content.Key);
        }
    }
}
