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

        private double energy;
        private double energyCapacity;
        private double accelerateEnergyUsage;
        private double deAccelerateEnergyUsage;
        private double turnEnergyUsage = 1;
        private double fireEnergyUsage;
        private double energyRechargeRate = 20000f;
        private double baseEnergyConsumption;

        private double acceleration;

        private double fireCooldown = 0;
        private double fireRate;


 
        private void Start()
        {
            rb = gameObject.GetComponent<GravityBehaviour>().Rb;
            rb.AddForce(new Vector3(0, 0, 100));
        }

        public void SpawnShip(Spacecraft spacecraft, Dictionary<String, KeyCode> keybindinds)
        {
            energy = spacecraft.EnergyCapacity;
            energyCapacity = spacecraft.EnergyCapacity;
            accelerateEnergyUsage = spacecraft.AccelerationEnergyConsumption;
            deAccelerateEnergyUsage = spacecraft.AccelerationEnergyConsumption;
            acceleration = spacecraft.Acceleration;
            //turnEnergyUsage = spacecraft.tu
            fireEnergyUsage = spacecraft.WeaponEnergyConsumption;
            fireRate = spacecraft.WeaponsRateOfFire;

            energyRechargeRate = spacecraft.EnergyRechargeRate;

            rb.mass = (float)spacecraft.Mass;
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
            //energy -= consumption * Time.deltaTime;
            HarvestEnergy();
            if (energy < 0)
            {
                TriggerGameOver();
            }
            energyBar.anchorMax = new Vector2((float)(energy / energyCapacity), 1);
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
            var energyGain = energyRechargeRate * Time.deltaTime / sqrDistance;
            energy += energyGain;
            energy = Math.Min(energy, energyCapacity);
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
            if (energy > accelerateEnergyUsage)
            {
                rb.AddForce(rb.transform.forward * (float)acceleration);
                energy -= accelerateEnergyUsage;
            }
        }

        private void DeAccelerate()
        {
            if (energy > deAccelerateEnergyUsage)
            {
                rb.AddForce(rb.transform.forward * -0.1f * (float)acceleration);
                energy -= deAccelerateEnergyUsage;
            }
        }

        private void TurnLeft()
        {
            if (energy > turnEnergyUsage)
            {
                rb.AddRelativeTorque(new Vector3(0, -1, 0));
                energy -= turnEnergyUsage;
            }
        }

        private void TurnRight()
        {
            if (energy > turnEnergyUsage)
            {
                rb.AddRelativeTorque(new Vector3(0, 1, 0));
                energy -= turnEnergyUsage;
            }
        }

        private void FireProjectile()
        {
            if (energy > fireEnergyUsage && fireCooldown <= 0)
            {
                projectileSpawnerBehaviour.SpawnProjectile(this.transform);
                energy -= fireEnergyUsage;
                fireCooldown = fireRate;
            }
        }
    }
}
