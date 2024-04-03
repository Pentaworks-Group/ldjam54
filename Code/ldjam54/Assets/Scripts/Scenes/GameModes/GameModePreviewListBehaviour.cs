using System;
using System.Collections.Generic;
using System.Linq;

using GameFrame.Core.UI.List;

using UnityEngine;

namespace Assets.Scripts.Scenes.GameModes
{
    public class GameModePreviewListBehaviour : ListContainerBehaviour<GameModePreviewListItem>
    {
        [SerializeField]
        private GameModesBehaviour gameModeBehaviour;

        public override void CustomStart()
        {
            UpdateList();
        }

        public void UpdateList()
        {
            var items = new List<GameModePreviewListItem>();
            var list = gameModeBehaviour.GetSelectedGameModes();
            if (list.Count > 0)
            {
                foreach (var rawItem in list)
                {
                    var listItem = new GameModePreviewListItem(rawItem.Value);
                    items.Add(listItem);
                }
            }

            SetContentList(items);
        }
    }
}