using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Core.Model;

using UnityEngine;
using UnityEngine.UI;

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

        private Spacecraft spacecraft;

        private Dictionary<KeyCode, ContinousKey> keyInteraction = new Dictionary<KeyCode, ContinousKey>();

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

        public String deathMessage { get; private set; }
        private bool isDead = false;


        void Update()
        {
            foreach (var keyBinding in keyInteraction)
            {

                if (Input.GetKeyDown(keyBinding.Key))
                {
                    keyBinding.Value.KeyDown();
                }
                else if (Input.GetKeyUp(keyBinding.Key))
                {
                    keyBinding.Value.KeyUp();
                }
            }

            energy -= spacecraft.BaseEnergyConsumption * Time.deltaTime;
            HarvestEnergy();
            if (energy < 0)
            {
                TriggerGameOver("You forgot to eat and ran out of energy");
            }
            energyBar.anchorMax = new Vector2((float)(energy / spacecraft.EnergyCapacity), 1);

            if (fireCooldown > 0)
            {
                fireCooldown -= Time.deltaTime;
            }

            Vector3 t = transform.position;
            if (t.x > 100 || t.x < -100 || t.z > 100 || t.z < -100)
            {
                TriggerGameOver("Tried to think out of the box");
            }
        }

        private void InitShip()
        {
            InitGravity();
            var dircenter = gravityCenter.transform.position - Rb.transform.position;
            Vector3 left = Vector3.Cross(dircenter, Vector3.down).normalized;
            Rb.velocity = left * 2;
            gameObject.tag = "Ship";
        }

        public void SpawnShip(Spacecraft spacecraft, Dictionary<String, KeyCode> keybindinds, String shipName, Color color)
        {
            InitShip();
            this.gameObject.name = shipName;
            this.spacecraft = spacecraft;
            energy = spacecraft.EnergyCapacity;
            deAccelerateEnergyUsage = (float)spacecraft.AccelerationEnergyConsumption * 0.1f;
            deAcceleration = (float)spacecraft.Acceleration * -0.1f;

            Rb.mass = (float)spacecraft.Mass;

            MapKeybindings(keybindinds);
            gameObject.transform.Find("Canvas/Image").GetComponent<Image>().color = color;
        }

        private Dictionary<String, Action> GenerateKeyBindingsMap()
        {
            return new Dictionary<String, Action>()
            {
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
                    var continousKey = gameObject.AddComponent<ContinousKey>();
                    continousKey.Init(binding.Value, binding.Value);
                    this.keyInteraction[keyCode] = continousKey;
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

        private void OnTriggerEnter(Collider collider)
        {
            GameObject other = collider.gameObject;
            var tag = other.tag;
            switch (tag)
            {
                case "Junk":
                    TriggerGameOver("You joined the space junk");
                    break;
                case "Ship":
                    TriggerGameOver("Never go alone. Together forever with " + other.name);
                    break;
                case "Sun":
                    TriggerGameOver("The sun is cosy warm, but you should not go that close");
                    break;
                case "Projectile":
                    TriggerGameOver("These little things are not candy");
                    break;
                default:
                    TriggerGameOver("Nobody knows what hit you, even the programers");
                    break;
            }
        }

        private void TriggerGameOver(String deathMessage)
        {
            if (isDead == false)
            {
                this.deathMessage = deathMessage;
                spaceBehaviour.TriggerGameOver(this);
                gameObject.transform.Find("Explosion").gameObject.SetActive(true);
                gameObject.transform.Find("Model").gameObject.SetActive(false);
                gameObject.transform.Find("Canvas").gameObject.SetActive(false);
                Destroy(gameObject, 3);
                Rb.velocity = Vector3.zero;
                Rb.constraints = RigidbodyConstraints.FreezeAll; 
                isDead = true;
            }
        }


        public void Accelerate()
        {
            if (energy > spacecraft.AccelerationEnergyConsumption)
            {
                Rb.AddForce(Rb.transform.forward * (float)spacecraft.Acceleration);
                energy -= spacecraft.AccelerationEnergyConsumption;

                GameFrame.Base.Audio.Effects.PlayAt(GameFrame.Base.Resources.Manager.Audio.Get("RocketEngine_Firing_Middle"), this.transform.position);
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

                GameFrame.Base.Audio.Effects.PlayAt(GameFrame.Base.Resources.Manager.Audio.Get("ProjectileFired"), this.transform.position);
            }
        }
    }
}
