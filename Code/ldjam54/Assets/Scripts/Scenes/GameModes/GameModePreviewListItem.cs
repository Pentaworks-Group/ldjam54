using System;

using Assets.Scripts.Core.Definitions;

namespace Assets.Scripts.Scenes.GameModes
{
    public class GameModePreviewListItem
    {
        public String Description { get; set; }
        public String Name { get; set; }
        public GameModePreview GameModePreview { get; set; }

        public GameModePreviewListItem(GameModePreview gameModePreview) {
            Name = gameModePreview.Name;
            Description = gameModePreview.Description;
            GameModePreview = gameModePreview;
        }

        public String GetKey()
        {
            return GameModePreview.Reference;
        }
    }
}
