using System;

using Assets.Scripts.Core.Definitions;

namespace Assets.Scripts.Scenes.GameModes
{
    public class GameModePreviewListItem
    {
        public String Description { get; set; }
        public String Name { get; set; }
        public GameModePreview GameMode { get; set; }

        public GameModePreviewListItem(GameModePreview gameModePreview) {
            Name = gameModePreview.Name;
            Description = gameModePreview.Description;
            GameMode = gameModePreview;
        }

        public String GetKey()
        {
            return GameMode.Reference;
        }
    }
}
