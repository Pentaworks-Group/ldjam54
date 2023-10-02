using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core.Definitions.Loaders;
using Assets.Scripts.Core.Model;

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

        public SkyboxShader Skybox { get; private set; }

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
                Mode = ConvertGameMode(SelectedGameMode),
                PlayerSpacecrafts = ConvertSpacecrafts(SelectedGameMode.PlayerSpacecrafts),
                Spacecrafts = ConvertSpacecrafts(SelectedGameMode.Spacecrafts),
                Skybox = GenerateRandomSkybox()
            };

            return gameState;
        }

        protected override PlayerOptions InitialzePlayerOptions()
        {
            return new PlayerOptions()
            {
                EffectsVolume = 0.7f,
                AmbienceVolume = 0.10f,
                BackgroundVolume = 0.05f,
            };
        }

        protected override void OnGameStart()
        {
            LoadGameSettings();

            InitializeAudioClips();

            this.Skybox = GenerateRandomSkybox();
        }

        private void LoadGameSettings()
        {
            new ResourceLoader<Definitions.Spacecraft>(this.availableSpacecrafts).LoadDefinition("Spacecrafts.json");
            new ResourceLoader<Definitions.Star>(this.availableStars).LoadDefinition("Stars.json");
            new GameModesLoader(this.availableGameModes, this.availableStars, this.availableSpacecrafts).LoadDefinition("GameModes.json");
        }

        private void InitializeAudioClips()
        {
            InitializeBackgroundAudio();
        }

        private void InitializeBackgroundAudio()
        {
            var backgroundAudioClips = new List<AudioClip>()
            {
                GameFrame.Base.Resources.Manager.Audio.Get("Background1"),
                GameFrame.Base.Resources.Manager.Audio.Get("Background2"),
                GameFrame.Base.Resources.Manager.Audio.Get("Background3"),
            };

            GameFrame.Base.Audio.Background.Play(backgroundAudioClips);
        }

        private Model.GameMode ConvertGameMode(Definitions.GameMode selectedGameMode)
        {
            var gameMode = new Model.GameMode()
            {
                Name = selectedGameMode.Name,
                Description = selectedGameMode.Description,
                JunkSpawnInterval = selectedGameMode.JunkSpawnInterval.GetValueOrDefault(-1),
                JunkSpawnInitialDistance = selectedGameMode.JunkSpawnInitialDistance.GetValueOrDefault(),
                JunkSpawnPosition = selectedGameMode.JunkSpawnPosition?.Copy(),
                JunkSpawnForce = selectedGameMode.JunkSpawnForce.Copy(),
                JunkSpawnTorque = selectedGameMode.JunkSpawnTorque.Copy(),
                ShipSpawnDistance = selectedGameMode.ShipSpawnDistance.GetValueOrDefault(),
                RequiredSurvivors = selectedGameMode.RequiredSurvivors,
            };

            gameMode.Star = ConvertStar(selectedGameMode.Stars.GetRandomEntry());
            gameMode.Spacecrafts = ConvertSpacecrafts(selectedGameMode.Spacecrafts);
            gameMode.PlayerSpacecrafts = ConvertSpacecrafts(selectedGameMode.PlayerSpacecrafts);

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
                Deceleration = definition.Deceleration.GetValueOrDefault(),
                DecelerationEnergyConsumption = definition.DecelerationEnergyConsumption.GetValueOrDefault(),
                TurnRate = definition.TurnRate.GetValueOrDefault(),
                TurnRateEnergyConsuption = definition.TurnRateEnergyConsuption.GetValueOrDefault(),
                IsWeaponized = definition.IsWeaponized.GetValueOrDefault(),
                WeaponsRateOfFire = definition.WeaponsRateOfFire.GetValueOrDefault(),
                WeaponsEnegryConsumption = definition.WeaponsEnegryConsumption.GetValueOrDefault(),
                Mass = definition.Mass.GetValueOrDefault(),
                Model = definition.Models.GetRandomEntry(),
                Material = definition.Materials.GetRandomEntry(),
                CurrentEnergy = definition.EnergyCapacity.GetValueOrDefault()
            };

            return spacecraft;
        }

        private Model.Star ConvertStar(Definitions.Star definition)
        {
            var model = new Model.Star()
            {
                Gravity = definition.Gravity.GetValueOrDefault(),
                Mass = definition.Mass.GetValueOrDefault(),
                LightRange = definition.LightRange.GetRandom(),
                LightIntensity = definition.LightIntensity.GetRandom(),
                Model = definition.Models.GetRandomEntry(),
                Material = definition.Materials.GetRandomEntry()
            };

            return model;
        }

        private SkyboxShader GenerateRandomSkybox()
        {
            var skyboxShader = new SkyboxShader()
            {
                NoiseParameters = new GameFrame.Core.Math.Vector4(0.75f, 6.0f, 0.795f, 2.08f),
                BackgroundMaskParameters = new GameFrame.Core.Math.Vector4(0.33f, 6.0f, 0.628f, 2.11f),
                BackgroundCutParameters = new GameFrame.Core.Math.Vector4(0.07f, -0.001f, 0.51f, 2.5f),
                IsGalaxyNoiseEnabled = 1,
                // Edit these
                Seed = UnityEngine.Random.Range(0, 1000),
                SkyColor = new GameFrame.Core.Media.Color(0.0f, 0.06f, 0.12f, 1.0f),
                StarColor = new GameFrame.Core.Media.Color(1, 1, 1, 1),
                StarSize = new GameFrame.Core.Math.Vector4(UnityEngine.Random.Range(0.4f, 3f), UnityEngine.Random.Range(0.4f, 3f), 0f, 0f),
                Layers = UnityEngine.Random.Range(1f, 5f),
                Density = UnityEngine.Random.Range(0.5f, 4f),
                DensityModulation = UnityEngine.Random.Range(0.5f, 4f),
                Contrast = UnityEngine.Random.Range(0f, 3f),
                BrightnessModulation = UnityEngine.Random.Range(1.01f, 4f),
                SkyFogColor = new GameFrame.Core.Media.Color(0.0f, 0.33f, 0.34f, 1.0f),
                NoiseDensity = UnityEngine.Random.Range(1f, 30f),
            };

            return skyboxShader;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GameStart()
        {
            Base.Core.Game.Startup();
        }
    }
}