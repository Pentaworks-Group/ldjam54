using System;

namespace Assets.Scripts.Core
{
    public class SavedGamePreview : GameFrame.Core.SavedGames.SavedGamePreview<GameState>
    {
        public String CreatedOn { get; set; }
        public String SavedOn { get; set; }
    }
}
