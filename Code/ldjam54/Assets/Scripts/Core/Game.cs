using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core.Definitions.Loaders;

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

        public IList<Definitions.GameMode> AvailableGameModes
        {
            get
            {
                if (this.availableGameModes.Count == 0)
                {
                    LoadGameSettings();
                }

                return this.availableGameModes.Values.ToList();
            }
        }
        public void PlayButtonSound()
        {
            GameFrame.Base.Audio.Effects.Play("Button");
        }

        protected override GameState InitializeGameState()
        {
            if (SelectedGameMode == default)
            {
                if (this.availableGameModes.Count > 0)
                {
                    if (this.availableGameModes.TryGetValue("default", out var defaultGameMode))
                    {
                        SelectedGameMode = defaultGameMode;
                    }
                    else
                    {
                        throw new Exception("No 'default' GameMode defined!");
                    }
                }
                else
                {
                    throw new Exception("Failed to load GameModes!");
                }
            }

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
                Name = selectedGameMode.Name
            };

            gameMode.Star = ConvertStar(selectedGameMode.Stars.GetRandomEntry());
            gameMode.PlayerSpacecraft = ConvertSpacecraft(selectedGameMode.PlayerSpacecrafts.GetRandomEntry());
            gameMode.Spacecrafts = ConvertSpacecrafts(selectedGameMode.Spacecrafts);

            return gameMode;
        }

        private List<Model.Spacecraft> ConvertSpacecrafts(List<Definitions.Spacecraft> definitions)
        {
            var spacecrafts = new List<Model.Spacecraft>();

            foreach (var definition in definitions)
            {
                spacecrafts.Add(ConvertSpacecraft(definition));
            }

            return spacecrafts;
        }

        private Model.Spacecraft ConvertSpacecraft(Definitions.Spacecraft definition)
        {
            var spacecraft = new Model.Spacecraft()
            {
                Health = definition.Health.GetValueOrDefault(),
                EnergyCapacity = definition.EnergyCapacity.GetValueOrDefault(),
                EnergyRechargeRate = definition.EnergyRechargeRate.GetValueOrDefault(),
                BaseEnergyConsumption = definition.BaseEnergyConsumption.GetValueOrDefault(),
                Acceleration = definition.Acceleration.GetValueOrDefault(),
                AccelerationEnergyConsumption = definition.AccelerationEnergyConsumption.GetValueOrDefault(),
                TurnRate = definition.TurnRate.GetValueOrDefault(),
                TurnRateEnergyConsuption = definition.TurnRateEnergyConsuption.GetValueOrDefault(),
                IsWeaponized = definition.IsWeaponized.GetValueOrDefault(),
                WeaponsRateOfFire = definition.WeaponsRateOfFire.GetValueOrDefault(),
                WeaponEnergyConsumption = definition.WeaponEnergyConsumption.GetValueOrDefault(),
                Mass = definition.Mass.GetValueOrDefault(),
                Model = definition.Models.GetRandomEntry(),
                Material = definition.Materials.GetRandomEntry(),
            };

            return spacecraft;
        }

        private Model.Star ConvertStar(Definitions.Star definition)
        {
            var model = new Model.Star()
            {
                Gravity = definition.Gravity.GetValueOrDefault(),
                Model = definition.Models.GetRandomEntry(),
                Material = definition.Materials.GetRandomEntry()
            };

            return model;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GameStart()
        {
            Base.Core.Game.Startup();
        }
    }
}