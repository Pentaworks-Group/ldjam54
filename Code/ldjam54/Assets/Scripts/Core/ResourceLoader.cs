using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Definitions;

using GameFrame.Core.Extensions;

using UnityEngine;

namespace Assets.Scripts.Assets.Scripts.Core
{
    internal class ResourceLoader
    {
        public void LoadSpacecraft<TDefinition>(String resourceName, Dictionary<String, TDefinition> cache) where TDefinition: BaseDefinition
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, resourceName);

            LoadAsset<List<TDefinition>>(filePath, (loadedSpacecrafts) => { return LoadDefinition(loadedSpacecrafts, cache); });
        }

        private void LoadStars()
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, "Stars.json");

            LoadAsset<List<Star>>(filePath, (loadedStars) => { return LoadDefinition(loadedStars, availableStars); });
        }

        private void LoadGameModes()
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, "GameModes.json");

            LoadAsset<List<GameMode>>(filePath, SetGameModes);
        }

        private void LoadAsset<TItem>(String filePath, Func<TItem, TItem> onLoadedCallback)
        {
            var gameO = new GameObject();

            var mono = gameO.AddComponent<EmptyLoadingBehaviour>();

            mono.StartCoroutine(GameFrame.Core.Json.Handler.DeserializeObjectFromStreamingAssets(filePath, onLoadedCallback));

            GameObject.Destroy(gameO);
        }

        private List<TDefinition> LoadDefinition<TDefinition>(List<TDefinition> sourceList, Dictionary<String, TDefinition> targetCache) where TDefinition : BaseDefinition
        {
            if (sourceList == default)
            {
                throw new ArgumentNullException(nameof(sourceList), "Definition source list may not be null!");
            }

            if (targetCache == default)
            {
                throw new ArgumentNullException(nameof(targetCache), "Definition cache is required and may not be null!");
            }

            if (sourceList?.Count > 0)
            {
                foreach (var loadedDefinition in sourceList)
                {
                    if (loadedDefinition.Reference.HasValue())
                    {
                        targetCache[loadedDefinition.Reference] = loadedDefinition;
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(BaseDefinition.Reference), "Reference of Definition may not be Null, Empty or WhiteSpace!");
                    }
                }
            }

            return sourceList;
        }

        private List<GameMode> SetGameModes(List<GameMode> loadedGameModes)
        {
            if (loadedGameModes?.Count > 0)
            {
                foreach (var loadedGameMode in loadedGameModes)
                {
                    var newGameMode = new GameMode();

                    CheckItems(loadedGameMode.Spacecrafts, newGameMode.Spacecrafts, HandleSpacecraftOverride);
                    CheckItems(loadedGameMode.Stars, newGameMode.Stars, HandleStarOverride);
                }
            }

            return loadedGameModes;
        }

        private void HandleStarOverride(Star actualStar, Star loadedStar)
        {
            if (loadedStar.IsReferenced)
            {
                if (this.availableStars.TryGetValue(loadedStar.Reference, out var referencedStar))
                {
                    if (loadedStar.IsValueOverride)
                    {
                        if (loadedStar.GravityForce.HasValue)
                        {
                            actualStar.GravityForce = loadedStar.GravityForce.Value;
                        }
                        else
                        {
                            actualStar.GravityForce = referencedStar.GravityForce.Value;
                        }

                        if (loadedStar.Models != default)
                        {
                            actualStar.Models = loadedStar.Models.ToList();
                        }
                        else
                        {
                            actualStar.Models = referencedStar.Models.ToList();
                        }

                        if (loadedStar.Materials != default)
                        {
                            actualStar.Materials = loadedStar.Materials.ToList();
                        }
                        else
                        {
                            actualStar.Materials = referencedStar.Materials.ToList();
                        }
                    }
                    else
                    {
                        actualStar.GravityForce = referencedStar.GravityForce;
                        actualStar.Models = referencedStar.Models.ToList();
                        actualStar.Materials = referencedStar.Materials.ToList();
                    }
                }
            }
            else
            {
                actualStar.GravityForce = loadedStar.GravityForce;
                actualStar.Models = loadedStar.Models.ToList();
                actualStar.Materials = loadedStar.Materials.ToList();
            }
        }

        private void HandleSpacecraftOverride(Spacecraft actualSpacecraft, Spacecraft loadedSpacecraft)
        {
            if (loadedSpacecraft.IsReferenced)
            {
                if (this.availableSpacecrafts.TryGetValue(loadedSpacecraft.Reference, out var referencedSpacecraft))
                {
                    if (loadedSpacecraft.IsValueOverride)
                    {
                        if (loadedSpacecraft.EnergyCapacity.HasValue)
                        {
                            actualSpacecraft.EnergyCapacity = loadedSpacecraft.EnergyCapacity.Value;
                        }
                        else
                        {
                            actualSpacecraft.EnergyCapacity = referencedSpacecraft.EnergyCapacity.Value;
                        }

                        if (loadedSpacecraft.EnergyRechargeRate.HasValue)
                        {
                            actualSpacecraft.EnergyRechargeRate = loadedSpacecraft.EnergyRechargeRate.Value;
                        }
                        else
                        {
                            actualSpacecraft.EnergyRechargeRate = referencedSpacecraft.EnergyRechargeRate;
                        }

                        if (loadedSpacecraft.Health.HasValue)
                        {
                            actualSpacecraft.Health = loadedSpacecraft.Health.Value;
                        }
                        else
                        {
                            actualSpacecraft.Health = referencedSpacecraft.Health;
                        }

                        if (loadedSpacecraft.AccelerationEnergyConsumption.HasValue)
                        {
                            actualSpacecraft.AccelerationEnergyConsumption = loadedSpacecraft.AccelerationEnergyConsumption.Value;
                        }
                        else
                        {
                            actualSpacecraft.AccelerationEnergyConsumption = referencedSpacecraft.AccelerationEnergyConsumption;
                        }

                        if (loadedSpacecraft.IsWeaponized.HasValue)
                        {
                            actualSpacecraft.IsWeaponized = loadedSpacecraft.IsWeaponized.Value;
                        }
                        else
                        {
                            actualSpacecraft.IsWeaponized = referencedSpacecraft.IsWeaponized;
                        }

                        if (loadedSpacecraft.WeaponsRateOfFire.HasValue)
                        {
                            actualSpacecraft.WeaponsRateOfFire = loadedSpacecraft.WeaponsRateOfFire.Value;
                        }
                        else
                        {
                            actualSpacecraft.WeaponsRateOfFire = referencedSpacecraft.WeaponsRateOfFire;
                        }

                        if (loadedSpacecraft.WeaponEnegryConsumption.HasValue)
                        {
                            actualSpacecraft.WeaponEnegryConsumption = loadedSpacecraft.WeaponEnegryConsumption.Value;
                        }
                        else
                        {
                            actualSpacecraft.WeaponEnegryConsumption = referencedSpacecraft.WeaponEnegryConsumption;
                        }

                        if (loadedSpacecraft.Mass.HasValue)
                        {
                            actualSpacecraft.Mass = loadedSpacecraft.Mass.Value;
                        }
                        else
                        {
                            actualSpacecraft.Mass = referencedSpacecraft.Mass;
                        }

                        if (loadedSpacecraft.Models != default)
                        {
                            actualSpacecraft.Models = loadedSpacecraft.Models.ToList();
                        }
                        else
                        {
                            actualSpacecraft.Models = referencedSpacecraft.Models.ToList();
                        }

                        if (loadedSpacecraft.Materials != default)
                        {
                            actualSpacecraft.Materials = loadedSpacecraft.Materials.ToList();
                        }
                        else
                        {
                            actualSpacecraft.Materials = referencedSpacecraft.Materials.ToList();
                        }
                    }
                    else
                    {
                        actualSpacecraft.EnergyCapacity = referencedSpacecraft.EnergyCapacity;
                        actualSpacecraft.Health = referencedSpacecraft.Health;
                        actualSpacecraft.AccelerationEnergyConsumption = referencedSpacecraft.AccelerationEnergyConsumption;
                        actualSpacecraft.IsWeaponized = referencedSpacecraft.IsWeaponized;
                        actualSpacecraft.WeaponsRateOfFire = referencedSpacecraft.WeaponsRateOfFire;
                        actualSpacecraft.WeaponEnegryConsumption = referencedSpacecraft.WeaponEnegryConsumption;
                        actualSpacecraft.Mass = referencedSpacecraft.Mass;
                        actualSpacecraft.Models = referencedSpacecraft.Models.ToList();
                        actualSpacecraft.Materials = referencedSpacecraft.Materials.ToList();
                    }
                }
            }
            else
            {
                actualSpacecraft.EnergyCapacity = loadedSpacecraft.EnergyCapacity;
                actualSpacecraft.Health = loadedSpacecraft.Health;
                actualSpacecraft.AccelerationEnergyConsumption = loadedSpacecraft.AccelerationEnergyConsumption;
                actualSpacecraft.IsWeaponized = loadedSpacecraft.IsWeaponized;
                actualSpacecraft.WeaponsRateOfFire = loadedSpacecraft.WeaponsRateOfFire;
                actualSpacecraft.WeaponEnegryConsumption = loadedSpacecraft.WeaponEnegryConsumption;
                actualSpacecraft.Mass = loadedSpacecraft.Mass;
                actualSpacecraft.Models = loadedSpacecraft.Models.ToList();
                actualSpacecraft.Materials = loadedSpacecraft.Materials.ToList();
            }
        }

        private void CheckItems<TItem>(List<TItem> loadedItems, List<TItem> targetItems, Action<TItem, TItem> handleOverides) where TItem : BaseDefinition, new()
        {
            if (loadedItems?.Count > 0)
            {
                foreach (var item in loadedItems)
                {
                    var targetItem = new TItem();

                    handleOverides(targetItem, item);
                }
            }
        }
    }
}
