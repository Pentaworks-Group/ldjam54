using System;

namespace Assets.Scripts.Core.Model
{
    public class Spacecraft
    {
        public Double Health { get; set; }
        public Double EnergyCapacity { get; set; }
        public Double EnergyRechargeRate { get; set; }
        public Double BaseEnergyConsumption { get; set; }
        public Double Acceleration { get; set; }
        public Double AccelerationEnergyConsumption { get; set; }
        public Double TurnRate { get; set; }
        public Double TurnRateEnergyConsuption { get; set; }
        public Boolean IsWeaponized { get; set; }
        public Double WeaponsRateOfFire { get; set; }
        public Double WeaponEnergyConsumption { get; set; }
        public Double Mass { get; set; }

        public String Material { get; set; }
        public String Model { get; set; }
        
    }
}
