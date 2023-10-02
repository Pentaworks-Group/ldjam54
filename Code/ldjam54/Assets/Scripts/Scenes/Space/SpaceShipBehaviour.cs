using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Model;

using GameFrame.Core.Extensions;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using Vector3 = UnityEngine.Vector3;

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
        private ParticleSystem burnerParticles;
        private TextMeshProUGUI junkKillCountDisplay;

        private Spacecraft spacecraft;

        private readonly Dictionary<KeyCode, ContinousKey> keyInteraction = new Dictionary<KeyCode, ContinousKey>();

        private bool isDead = false;
        private int junkKillCount = 0;

        public String DeathMessage { get; private set; }

        void Update()
        {
            if (!isDead)
            {
                CheckKeys();
                HandleEnergy();

                if (spacecraft.WeaponCooldown > 0)
                {
                    spacecraft.WeaponCooldown -= Time.deltaTime;
                }

                spacecraft.Position = this.transform.position.ToFrame();
                spacecraft.Velocity = Rb.velocity.ToFrame();
                
                UpdateJunkKillDisplay();
                OutOfBoundCheck();
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            GameObject other = collider.gameObject;
            var tag = other.tag;

            switch (tag)
            {
                case "Junk":
                    TriggerGameOver("You joined the space junk gang");
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
                    TriggerGameOver("Nobody knows what hit you, not even the programers");
                    break;
            }
        }

        private void OutOfBoundCheck()
        {
            Vector3 t = transform.position;

            if (t.x > 100 || t.x < -100 || t.z > 100 || t.z < -100)
            {
                TriggerGameOver("Tried to think out of the box");
            }
        }

        private void HandleEnergy()
        {
            spacecraft.CurrentEnergy -= spacecraft.BaseEnergyConsumption * Time.deltaTime;

            HarvestEnergy();

            if (spacecraft.CurrentEnergy < 0)
            {
                TriggerGameOver("You forgot to eat and ran out of energy");
            }

            energyBar.anchorMax = new Vector2((float)(spacecraft.CurrentEnergy / spacecraft.EnergyCapacity), 1);
        }

        private void CheckKeys()
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
        }

        private void InitShip()
        {
            gameObject.tag = "Ship";

            var position = spacecraft.Position.ToUnity();

            if (position == default)
            {
                var vec = new Vector3(UnityEngine.Random.Range(-100, 100), 0, UnityEngine.Random.Range(-100, 100));

                vec = vec.normalized * (float)Base.Core.Game.State.Mode.ShipSpawnDistance;

                spacecraft.Position = vec.ToFrame();
                position = vec;
            }

            transform.position = position;

            Vector3 velocity = spacecraft.Velocity.ToUnity();

            if (velocity == default)
            {
                var dircenter = gravityCenter.transform.position - Rb.transform.position;

                Vector3 left = Vector3.Cross(dircenter, Vector3.down).normalized;

                left *= 2;

                velocity = left;

                spacecraft.Velocity = velocity.ToFrame();
            }

            Rb.velocity = velocity;
        }

        public void SpawnShip(Spacecraft spacecraft, Dictionary<String, KeyCode> keybindinds, String shipName, Color color, TextMeshProUGUI junkKillDisplay)
        {
            this.spacecraft = spacecraft;

            InitGravity();

            InitShip();

            junkKillCountDisplay = junkKillDisplay;
            this.gameObject.name = shipName;

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

            spacecraft.CurrentEnergy += energyGain;
            spacecraft.CurrentEnergy = Math.Min(spacecraft.CurrentEnergy, spacecraft.EnergyCapacity);
        }

        private void TriggerGameOver(String deathMessage)
        {
            if (isDead == false)
            {
                this.DeathMessage = deathMessage;
                spaceBehaviour.TriggerGameOver(this);
                gameObject.transform.Find("Explosion").gameObject.SetActive(true);
                gameObject.transform.Find("Model").gameObject.SetActive(false);
                gameObject.transform.Find("Canvas").gameObject.SetActive(false);
                Destroy(gameObject, 3);
                spacecraft.Velocity = GameFrame.Core.Math.Vector3.Zero;
                Rb.velocity = Vector3.zero;
                Rb.constraints = RigidbodyConstraints.FreezeAll;
                isDead = true;
            }
        }

        public void Accelerate()
        {
            if (!isDead && spacecraft.CurrentEnergy > spacecraft.AccelerationEnergyConsumption)
            {
                Rb.AddForce(Rb.transform.forward * (float)spacecraft.Acceleration);

                spacecraft.Velocity = Rb.velocity.ToFrame();
                spacecraft.CurrentEnergy -= spacecraft.AccelerationEnergyConsumption;

                GameFrame.Base.Audio.Effects.PlayAt(GameFrame.Base.Resources.Manager.Audio.Get("RocketEngine_Firing_Middle"), this.transform.position);
                burnerParticles.Play();
            }
        }

        public void DeAccelerate()
        {
            if (!isDead && spacecraft.CurrentEnergy > spacecraft.DecelerationEnergyConsumption)
            {
                Rb.AddForce(Rb.transform.forward * (float)spacecraft.Deceleration);

                spacecraft.Velocity = Rb.velocity.ToFrame();
                spacecraft.CurrentEnergy -= spacecraft.DecelerationEnergyConsumption;

                GameFrame.Base.Audio.Effects.PlayAt(GameFrame.Base.Resources.Manager.Audio.Get("RCS_Firing_Middle"), this.transform.position);
            }
        }

        public void TurnLeft()
        {
            if (!isDead && spacecraft.CurrentEnergy > spacecraft.TurnRateEnergyConsuption)
            {
                Rb.AddRelativeTorque(new Vector3(0, (float)-spacecraft.TurnRate, 0));

                spacecraft.Velocity = Rb.velocity.ToFrame();
                spacecraft.CurrentEnergy -= spacecraft.TurnRateEnergyConsuption;

                GameFrame.Base.Audio.Effects.PlayAt(GameFrame.Base.Resources.Manager.Audio.Get("RCS_Firing_Start"), this.transform.position);
            }
        }

        public void TurnRight()
        {
            if (spacecraft.CurrentEnergy > spacecraft.TurnRateEnergyConsuption)
            {
                Rb.AddRelativeTorque(new Vector3(0, (float)spacecraft.TurnRate, 0));

                spacecraft.Velocity = Rb.velocity.ToFrame();                
                spacecraft.CurrentEnergy -= spacecraft.TurnRateEnergyConsuption;

                GameFrame.Base.Audio.Effects.PlayAt(GameFrame.Base.Resources.Manager.Audio.Get("RCS_Firing_Start"), this.transform.position);
            }
        }

        public void FireProjectile()
        {
            if (!isDead && spacecraft.CurrentEnergy > spacecraft.WeaponEnergyConsumption && spacecraft.WeaponCooldown <= 0)
            {
                projectileSpawnerBehaviour.SpawnProjectile(this);
                spacecraft.CurrentEnergy -= spacecraft.WeaponEnergyConsumption;
                spacecraft.WeaponCooldown = spacecraft.WeaponsRateOfFire;

                GameFrame.Base.Audio.Effects.PlayAt(GameFrame.Base.Resources.Manager.Audio.Get("ProjectileFired"), this.transform.position);
            }
        }

        public void IncreaseJunkKillCount()
        {
            junkKillCount++;
        }

        private void UpdateJunkKillDisplay()
        {
            junkKillCountDisplay.text = junkKillCount.ToString();
        }
    }
}
