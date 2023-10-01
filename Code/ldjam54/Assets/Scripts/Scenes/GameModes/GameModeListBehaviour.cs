using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core.Model;
using Assets.Scripts.Scenes.GameModes;

using GameFrame.Core.UI.List;

using UnityEngine;

namespace Assets.Scripts.Scenes.GameModes
{
    public class GameModeListBehaviour : ListContainerBehaviour<GameModeListItem>
    {
        [SerializeField]
        private GameModesBehaviour gameModeBehaviour;

        public override void CustomStart()
        {
            UpdateList();
        }

        public void UpdateList()
        {
            var items = new Dictionary<String, GameModeListItem>();
            var list = gameModeBehaviour.GetSelectedGameModes();
            if (list.Count > 0)
            {
                foreach (var rawItem in list)
                {
                    var listItem = new GameModeListItem(rawItem);
                    items[listItem.GetKey()] = listItem;
                }
            }

            SetContentList(items.Values.ToList());
        }
    }
}