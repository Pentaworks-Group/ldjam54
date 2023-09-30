using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Definitions.Loaders;

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions, SavedGamePreview>
    {
        private readonly Dictionary<String, GameMode> availableGameModes = new Dictionary<String, GameMode>();
        private readonly Dictionary<String, Star> availableStars = new Dictionary<String, Star>();
        private readonly Dictionary<String, Spacecraft> availableSpacecrafts = new Dictionary<String, Spacecraft>();

        protected override GameState InitializeGameState()
        {
            return new GameState()
            {
                CreatedOn = DateTime.Now,
            };
        }

        protected override PlayerOptions InitialzePlayerOptions()
        {
            return new PlayerOptions()
            {
                EffectsVolume = 0.7f,
                AmbienceVolume = 0.3f,
                BackgroundVolume = 0.25f,
            };
        }

        protected override void OnGameStart()
        {
            base.OnGameStart();

            LoadGameSettings();
        }

        private void LoadGameSettings()
        {
            new ResourceLoader<Spacecraft>(this.availableSpacecrafts).LoadDefinition("Spacecrafts.json");
            new ResourceLoader<Star>(this.availableStars).LoadDefinition("Stars.json");
            new GameModesLoader(this.availableGameModes, this.availableStars, this.availableSpacecrafts).LoadDefinition("GameModes.json");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GameStart()
        {
            Base.Core.Game.Startup();
        }
    }
}