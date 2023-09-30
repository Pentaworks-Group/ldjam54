using System;
using System.Collections.Generic;

using Assets.Scripts.Definitions.Loaders;

using GameFrame.Core.Extensions;

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions, SavedGamePreview>
    {
        private readonly Dictionary<String, Definitions.GameMode> availableGameModes = new Dictionary<String, Definitions.GameMode>();
        private readonly Dictionary<String, Definitions.Star> availableStars = new Dictionary<String, Definitions.Star>();
        private readonly Dictionary<String, Definitions.Spacecraft> availableSpacecrafts = new Dictionary<String, Definitions.Spacecraft>();

        public static Definitions.GameMode SelectedGameMode { get; set; }

        protected override GameState InitializeGameState()
        {
            var gameState = new GameState()
            {
                CreatedOn = DateTime.Now,
                CurrentScene = Constants.SceneNames.Space,
                GameMode = ConvertGameMode(SelectedGameMode),
                Spacecraft = ConvertSpacecraft(SelectedGameMode.PlayerSpacecrafts.GetRandomEntry())
            };

            return gameState;
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
            new ResourceLoader<Definitions.Spacecraft>(this.availableSpacecrafts).LoadDefinition("Spacecrafts.json");
            new ResourceLoader<Definitions.Star>(this.availableStars).LoadDefinition("Stars.json");
            new GameModesLoader(this.availableGameModes, this.availableStars, this.availableSpacecrafts).LoadDefinition("GameModes.json");
        }

        private Model.GameMode ConvertGameMode(Definitions.GameMode selectedGameMode)
        {
            var gameMode = new Model.GameMode()
            {

            };

            return gameMode;
        }

        private Model.Spacecraft ConvertSpacecraft(Definitions.Spacecraft definition)
        {
            var spacecraft = new Model.Spacecraft()
            {
                Health = definition.Health.GetValueOrDefault(),
                EnergyCapacity = definition.EnergyCapacity.GetValueOrDefault(),
                EnergyRechargeRate = definition.EnergyRechargeRate.GetValueOrDefault(),
                Acceleration = definition.Acceleration.GetValueOrDefault(),
                AccelerationEnergyConsumption = definition.AccelerationEnergyConsumption.GetValueOrDefault(),
                IsWeaponized = definition.IsWeaponized.GetValueOrDefault(),
                WeaponsRateOfFire = definition.WeaponsRateOfFire.GetValueOrDefault(),
                WeaponEnegryConsumption = definition.WeaponEnegryConsumption.GetValueOrDefault(),
                Mass = definition.Mass.GetValueOrDefault(),
                Model = definition.Models.GetRandomEntry(),
                Material = definition.Materials.GetRandomEntry(),
            };

            return spacecraft;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GameStart()
        {
            Base.Core.Game.Startup();
        }
    }
}