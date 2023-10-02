using System;
using System.Diagnostics;

namespace Assets.Scripts.Core
{
    public class SavedGamePreview : GameFrame.Core.SavedGames.SavedGamePreview<GameState>
    {
        public String CreatedOn { get; set; }
        public String SavedOn { get; set; }
        public String GameMode { get; set; }
        public String PlayerCount { get; set; }
        public String TimeElapsed { get; set; }

        public override void Init(GameState gameState, String key)
        {
            base.Init(gameState, key);

            CreatedOn = String.Format("{0:G}", gameState.CreatedOn);
            SavedOn = String.Format("{0:G}", gameState.SavedOn);
            GameMode = gameState.Mode.Name;
            PlayerCount = gameState.Mode.Spacecrafts.Count.ToString();
            TimeElapsed = String.Format("{0:#0.0}s", gameState.TimeElapsed);

            UnityEngine.Debug.Log($"X: {gameState.Spacecraft.Position.X} Y: {gameState.Spacecraft.Position.Y} Z: {gameState.Spacecraft.Position.Z}");
        }
    }
}
