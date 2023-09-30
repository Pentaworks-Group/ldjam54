using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Core.Model;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.Space
{
    public class SpaceShipBehaviour : MonoBehaviour
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
        private Rigidbody rb;

        private Spacecraft spacecraft;

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


 
        private void Init()
        {
            var gravityBehaviour = gameObject.GetComponent<GravityBehaviour>();
            gravityBehaviour.Init();
            this.rb = gravityBehaviour.Rb;
            var dircenter = gravityCenter.transform.position - rb.transform.position;
            Vector3 left = Vector3.Cross(dircenter, Vector3.down).normalized;
            rb.velocity = left * 2;
        }

        public void SpawnShip(Spacecraft spacecraft, Dictionary<String, KeyCode> keybindinds)
        {
            Init();
            this.spacecraft = spacecraft;
            energy = spacecraft.EnergyCapacity;
            deAccelerateEnergyUsage = (float)spacecraft.AccelerationEnergyConsumption * 0.1f;
            deAcceleration = (float)spacecraft.Acceleration * -0.1f;

            rb.mass = (float)spacecraft.Mass;

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

        void Update()
        {
            foreach (var keyBinding in keyBindings)
            {
                if (Input.GetKey(keyBinding.Key))
                {
                    keyBinding.Value.Invoke();
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
        }


        private void HarvestEnergy()
        {
            Vector3 direction = rb.position - gravityCenter.Rb.position;
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

        private void Accelerate()
        {
            if (energy > spacecraft.AccelerationEnergyConsumption)
            {
                rb.AddForce(rb.transform.forward * (float)spacecraft.Acceleration);
                energy -= spacecraft.AccelerationEnergyConsumption;
            }
        }

        private void DeAccelerate()
        {
            if (energy > deAccelerateEnergyUsage)
            {
                rb.AddForce(rb.transform.forward * deAcceleration);
                energy -= deAccelerateEnergyUsage;
            }
        }

        private void TurnLeft()
        {
            if (energy > spacecraft.TurnRateEnergyConsuption)
            {
                rb.AddRelativeTorque(new Vector3(0, (float)-spacecraft.TurnRate, 0));
                energy -= spacecraft.TurnRateEnergyConsuption;
            }
        }

        private void TurnRight()
        {
            if (energy > spacecraft.TurnRateEnergyConsuption)
            {
                rb.AddRelativeTorque(new Vector3(0, (float)spacecraft.TurnRate, 0));
                energy -= spacecraft.TurnRateEnergyConsuption;
            }
        }

        private void FireProjectile()
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
