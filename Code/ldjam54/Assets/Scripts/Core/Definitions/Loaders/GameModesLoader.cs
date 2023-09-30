using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Core.Definitions.Loaders
{
    public class GameModesLoader : ResourceLoader<GameMode>
    {
        private readonly Dictionary<String, Star> starCache;
        private readonly Dictionary<String, Spacecraft> spacecraftCache;

        public GameModesLoader(Dictionary<String, GameMode> targetCache, Dictionary<String, Star> starCache, Dictionary<String, Spacecraft> spacecraftCache) : base(targetCache)
        {
            this.starCache = starCache;
            this.spacecraftCache = spacecraftCache;
        }

        protected override List<GameMode> LoadDefinition(List<GameMode> loadedGameModes)
        {
            if (loadedGameModes?.Count > 0)
            {
                foreach (var loadedGameMode in loadedGameModes)
                {
                    var newGameMode = new GameMode()
                    {
                        Reference = loadedGameMode.Reference,
                        Name = loadedGameMode.Name,
                    };

                    CheckItems(loadedGameMode.Stars, newGameMode.Stars, HandleStarOverride);
                    CheckItems(loadedGameMode.Spacecrafts, newGameMode.Spacecrafts, HandleSpacecraftOverride);
                    CheckItems(loadedGameMode.PlayerSpacecrafts, newGameMode.PlayerSpacecrafts, HandleSpacecraftOverride);

                    targetCache[loadedGameMode.Reference] = newGameMode;
                }
            }

            return loadedGameModes;
        }

        private void HandleStarOverride(Star actualStar, Star loadedStar)
        {
            if (loadedStar.IsReferenced)
            {
                if (this.starCache.TryGetValue(loadedStar.Reference, out var referencedStar))
                {
                    if (loadedStar.IsValueOverride)
                    {
                        if (loadedStar.Gravity.HasValue)
                        {
                            actualStar.Gravity = loadedStar.Gravity.Value;
                        }
                        else
                        {
                            actualStar.Gravity = referencedStar.Gravity.Value;
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
                        actualStar.Gravity = referencedStar.Gravity;
                        actualStar.Models = referencedStar.Models.ToList();
                        actualStar.Materials = referencedStar.Materials.ToList();
                    }
                }
            }
            else
            {
                actualStar.Gravity = loadedStar.Gravity;
                actualStar.Models = loadedStar.Models.ToList();
                actualStar.Materials = loadedStar.Materials.ToList();
            }
        }

        private void HandleSpacecraftOverride(Spacecraft actualSpacecraft, Spacecraft loadedSpacecraft)
        {
            if (loadedSpacecraft.IsReferenced)
            {
                if (this.spacecraftCache.TryGetValue(loadedSpacecraft.Reference, out var referencedSpacecraft))
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

                        if (loadedSpacecraft.WeaponEnergyConsumption.HasValue)
                        {
                            actualSpacecraft.WeaponEnergyConsumption = loadedSpacecraft.WeaponEnergyConsumption.Value;
                        }
                        else
                        {
                            actualSpacecraft.WeaponEnergyConsumption = referencedSpacecraft.WeaponEnergyConsumption;
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
                        actualSpacecraft.WeaponEnergyConsumption = referencedSpacecraft.WeaponEnergyConsumption;
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
                actualSpacecraft.WeaponEnergyConsumption = loadedSpacecraft.WeaponEnergyConsumption;
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

                    targetItems.Add(targetItem);
                }
            }
        }
    }
}
