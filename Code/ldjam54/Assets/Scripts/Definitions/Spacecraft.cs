using System;
using System.Collections.Generic;

namespace Assets.Scripts.Definitions
{
    public class Spacecraft : BaseDefinition
    {        
        public Double? EnergyCapacity { get; set; }
        public Double? EnergyRechargeRate { get; set; }
        public Double? Health { get; set; }
        public Double? Acceleration { get; set; }
        public Double? AccelerationEnergyConsumption { get; set; }
        public Boolean? IsWeaponized { get; set; }
        public Double? WeaponsRateOfFire { get; set; }
        public Double? WeaponEnegryConsumption { get; set; }
        public Double? Mass { get; set; }

        public List<String> Models { get; set; }
        public List<String> Materials { get; set; }
    }
}
