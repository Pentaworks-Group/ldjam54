using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core;

using GameFrame.Core.UI.List;

using UnityEngine.UI;

namespace Assets.Scripts.Scenes.SavedGames
{
    public class SavedGameListBehaviour : ListContainerBehaviour<KeyValuePair<String, SavedGamePreview>>
    {
        public Button SaveNewButton;
        public Button DeleteAllButton;

        public override void CustomStart()
        {
            UpdateList();
        }

        public void UpdateList()
        {
            List<KeyValuePair<String, SavedGamePreview>> savedGames = Base.Core.Game.GetSavedGamePreviews().OrderBy(kvp => kvp.Key).ToList();

            SetContentList(savedGames);

            if (!Assets.Scripts.Base.Core.Game.IsFileAccessPossible)
            {
                SaveNewButton.interactable = savedGames.Count < 5;
            }

            if (DeleteAllButton != null)
            {
                DeleteAllButton.interactable = savedGames.Count > 0;
            }

        }

        public void OnDeleteAll()
        {
            Base.Core.Game.PlayButtonSound();

            Base.Core.Game.DeleteAllSavedGames();
            UpdateList();
        }

        public void SaveGame()
        {
            Base.Core.Game.PlayButtonSound();

            Base.Core.Game.SaveGame();
            UpdateList();
        }
    }
}
