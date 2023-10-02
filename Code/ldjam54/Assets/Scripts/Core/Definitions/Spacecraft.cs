using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitions
{
    public class Spacecraft : BaseDefinition
    {
        public Double? EnergyCapacity { get; set; }
        public Double? EnergyRechargeRate { get; set; }
        public Double? Health { get; set; }
        public Double? BaseEnergyConsumption { get; set; }
        public Double? Acceleration { get; set; }
        public Double? AccelerationEnergyConsumption { get; set; }
        public Double? TurnRate { get; set; }
        public Double? TurnRateEnergyConsuption { get; set; }
        public Double? Deceleration { get; set; }
        public Double? DecelerationEnergyConsumption { get; set; }
        public Boolean? IsWeaponized { get; set; }
        public Double? WeaponsRateOfFire { get; set; }
        public Double? WeaponEnergyConsumption { get; set; }
        public Double? Mass { get; set; }

        public List<String> Models { get; set; }
        public List<String> Materials { get; set; }
    }
}
