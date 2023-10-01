using System;

using Assets.Scripts.Core.Definitions;

namespace Assets.Scripts.Scenes.GameModes
{
    public class GameModeListItem
    {
        public String Description { get; set; }
        public String Name { get; set; }
        public GameMode GameMode { get; set; }

        public GameModeListItem(GameMode gameMode) {
            Name = gameMode.Name;
            Description =  gameMode.Description;
            GameMode = gameMode;
        }

        public String GetKey()
        {
            return GameMode.Reference;
        }
    }
}
