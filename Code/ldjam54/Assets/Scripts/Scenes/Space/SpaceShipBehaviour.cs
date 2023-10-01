using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Model;

using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class SpaceShipBehaviour : GravityBehaviour
    {
        [SerializeField]
        private SpaceBehaviour spaceBehaviour;
        [SerializeField]
        private ProjectileSpawnerBehaviour projectileSpawnerBehaviour;
        [SerializeField]
        private GravityManagerBehaviour gravityCenter;
        [SerializeField]
        private RectTransform energyBar;
        [SerializeField]
        private TMPro.TextMeshProUGUI energyText;

        private Dictionary<KeyCode, Action> keyBindings = new Dictionary<KeyCode, Action>();

        private Spacecraft spacecraft;

        private float keyInteractionInterval = 0.01f;
        private Dictionary<KeyCode, float> lastKeyteraction = new Dictionary<KeyCode, float>();

        private double energy;
        //private double energyCapacity;
        //private double accelerateEnergyUsage;
        private float deAcceleration;
        private float deAccelerateEnergyUsage;
        //private double turnEnergyUsage = 1;
        //private double fireEnergyUsage;
        //private double energyRechargeRate = 20000f;
        //private double baseEnergyConsumption;


        private double fireCooldown = 0;

        private void Awake()
        {
            foreach (var keyBinding in keyBindings)
            {
                lastKeyteraction[keyBinding.Key] = -1f;
            }
        }


        void Update()
        {
            foreach (var keyBinding in keyBindings)
            {
                if (lastKeyteraction[keyBinding.Key] <= 0)
                {

                    if (Input.GetKey(keyBinding.Key))
                    {
                        keyBinding.Value.Invoke();
                        lastKeyteraction[keyBinding.Key] = keyInteractionInterval;
                    }
                }
                else
                {
                    lastKeyteraction[keyBinding.Key] -= Time.deltaTime;
                }
            }

            energy -= spacecraft.BaseEnergyConsumption * Time.deltaTime;
            HarvestEnergy();
            if (energy < 0)
            {
                TriggerGameOver();
            }
            energyBar.anchorMax = new Vector2((float)(energy / spacecraft.EnergyCapacity), 1);
            energyText.text = energy.ToString();

            if (fireCooldown > 0)
            {
                fireCooldown -= Time.deltaTime;
            }

            Vector3 t = transform.position;
            if (t.x > 100 || t.x < -100 || t.z > 100 || t.z < -100)
            {
                TriggerGameOver();
            }
        }


        private void InitShip()
        {
            InitGravity();
            var dircenter = gravityCenter.transform.position - Rb.transform.position;
            Vector3 left = Vector3.Cross(dircenter, Vector3.down).normalized;
            Rb.velocity = left * 2;
        }

        public void SpawnShip(Spacecraft spacecraft, Dictionary<String, KeyCode> keybindinds)
        {
            InitShip();
            this.spacecraft = spacecraft;
            energy = spacecraft.EnergyCapacity;
            deAccelerateEnergyUsage = (float)spacecraft.AccelerationEnergyConsumption * 0.1f;
            deAcceleration = (float)spacecraft.Acceleration * -0.1f;

            Rb.mass = (float)spacecraft.Mass;

            MapKeybindings(keybindinds);
        }

        private Dictionary<String, Action> GenerateKeyBindingsMap()
        {
            return new Dictionary<String, Action> {
            { "Accelerate", Accelerate },
            { "DeAccelerate", DeAccelerate },
            { "TurnLeft", TurnLeft },
            { "TurnRight", TurnRight },
            { "FireProjectile", FireProjectile }
        };
        }

        private void MapKeybindings(Dictionary<String, KeyCode> keybindindsInit)
        {
            Dictionary<String, Action> keyBindingsRaw = GenerateKeyBindingsMap();

            foreach (var binding in keyBindingsRaw)
            {
                KeyCode keyCode = keybindindsInit.GetValueOrDefault(binding.Key);
                if (keyCode != default)
                {
                    keyBindings.Add(keyCode, binding.Value);
                }
            }

        }



        private void HarvestEnergy()
        {
            Vector3 direction = Rb.position - gravityCenter.Rb.position;
            float sqrDistance = direction.sqrMagnitude;
            sqrDistance -= 25;
            sqrDistance = Mathf.Max(sqrDistance, .5f);
            sqrDistance = Mathf.Pow(sqrDistance, 2);
            var energyGain = (float)spacecraft.EnergyRechargeRate * Time.deltaTime / sqrDistance;
            energy += energyGain;
            energy = Math.Min(energy, spacecraft.EnergyCapacity);
        }




        private void OnTriggerEnter(Collider other)
        {
            TriggerGameOver();
        }

        private void TriggerGameOver()
        {
            spaceBehaviour.TriggerGameOver();
        }

        public void Accelerate()
        {
            if (energy > spacecraft.AccelerationEnergyConsumption)
            {
                Rb.AddForce(Rb.transform.forward * (float)spacecraft.Acceleration);
                energy -= spacecraft.AccelerationEnergyConsumption;
            }
        }

        public void DeAccelerate()
        {
            if (energy > deAccelerateEnergyUsage)
            {
                Rb.AddForce(Rb.transform.forward * deAcceleration);
                energy -= deAccelerateEnergyUsage;
            }
        }

        public void TurnLeft()
        {
            if (energy > spacecraft.TurnRateEnergyConsuption)
            {
                Rb.AddRelativeTorque(new Vector3(0, (float)-spacecraft.TurnRate, 0));
                energy -= spacecraft.TurnRateEnergyConsuption;
            }
        }

        public void TurnRight()
        {
            if (energy > spacecraft.TurnRateEnergyConsuption)
            {
                Rb.AddRelativeTorque(new Vector3(0, (float)spacecraft.TurnRate, 0));
                energy -= spacecraft.TurnRateEnergyConsuption;
            }
        }

        public void FireProjectile()
        {
            if (energy > spacecraft.WeaponEnergyConsumption && fireCooldown <= 0)
            {
                projectileSpawnerBehaviour.SpawnProjectile(this.transform);
                energy -= spacecraft.WeaponEnergyConsumption;
                fireCooldown = spacecraft.WeaponsRateOfFire;
            }
        }
    }
}
