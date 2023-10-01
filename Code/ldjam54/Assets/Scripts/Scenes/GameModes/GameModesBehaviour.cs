using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Scenes.Menues;

using UnityEngine;

namespace Assets.Scripts.Scenes.GameModes
{
    public class GameModesBehaviour : BaseMenuBehaviour
    {
        private IList<GameMode> selectedGameModes;



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
    }
}
