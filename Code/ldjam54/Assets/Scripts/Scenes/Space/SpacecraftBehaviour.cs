using System;
using System.Collections.Generic;

using Assets.Scripts.Constants;
using Assets.Scripts.Core.Model;

using GameFrame.Core.Extensions;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Scenes.Space
{
    public class SpacecraftBehaviour : GravityBehaviour
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
        [SerializeField]
        private ParticleSystem burnerLeftParticles;
        [SerializeField]
        private ParticleSystem burnerRightParticles;
        [SerializeField]
        private ParticleSystem burnerFrontParticles;
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
                spacecraft.Rotation = transform.rotation.eulerAngles.ToFrame();

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
                case GameObjectTags.Junk:
                    TriggerGameOver("You joined the space junk gang");
                    break;

                case GameObjectTags.Ship:
                    TriggerGameOver("Never go alone. Together forever with " + other.transform.parent.name);
                    break;

                case GameObjectTags.Sun:
                    TriggerGameOver("The sun is cosy warm, but you should not go that close");
                    break;

                case GameObjectTags.Projectile:
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
                GameFrame.Base.Audio.Effects.Play("ShootingStars");
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
            transform.Find("Model").gameObject.tag = GameObjectTags.Ship;
            

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

            if (spacecraft.Rotation.HasValue)
            {
                transform.Rotate(spacecraft.Rotation.Value.ToUnity(), UnityEngine.Space.World);
            }
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
                { KeyActions.Accelerate, Accelerate },
                { KeyActions.Decelerate, Decelerate },
                { KeyActions.TurnLeft, TurnLeft },
                { KeyActions.TurnRight, TurnRight },
                { KeyActions.FireProjectile, FireProjectile }
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

                GameFrame.Base.Audio.Effects.PlayAt("Explosion_Spacecraft", this.transform.position);
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

                GameFrame.Base.Audio.Effects.PlayAt("RocketEngine_Firing_Middle", this.transform.position);
                burnerParticles.Play();
            }
        }

        public void Decelerate()
        {
            if (!isDead && spacecraft.CurrentEnergy > spacecraft.DecelerationEnergyConsumption)
            {
                Rb.AddForce(Rb.transform.forward * (float)spacecraft.Deceleration);

                spacecraft.Velocity = Rb.velocity.ToFrame();
                spacecraft.CurrentEnergy -= spacecraft.DecelerationEnergyConsumption;

                GameFrame.Base.Audio.Effects.PlayAt("RCS_Firing_Middle", this.transform.position);
                burnerFrontParticles.Play();
            }
        }

        public void TurnLeft()
        {
            if (!isDead && spacecraft.CurrentEnergy > spacecraft.TurnRateEnergyConsuption)
            {
                Rb.AddRelativeTorque(new Vector3(0, (float)-spacecraft.TurnRate, 0));

                spacecraft.Velocity = Rb.velocity.ToFrame();
                spacecraft.CurrentEnergy -= spacecraft.TurnRateEnergyConsuption;

                GameFrame.Base.Audio.Effects.PlayAt("RCS_Firing_Start", this.transform.position);
                burnerRightParticles.Play();
            }
        }

        public void TurnRight()
        {
            if (spacecraft.CurrentEnergy > spacecraft.TurnRateEnergyConsuption)
            {
                Rb.AddRelativeTorque(new Vector3(0, (float)spacecraft.TurnRate, 0));

                spacecraft.Velocity = Rb.velocity.ToFrame();
                spacecraft.CurrentEnergy -= spacecraft.TurnRateEnergyConsuption;

                GameFrame.Base.Audio.Effects.PlayAt("RCS_Firing_Start", this.transform.position);
                burnerLeftParticles.Play();
            }
        }

        public void FireProjectile()
        {
            if (!isDead && spacecraft.CurrentEnergy > spacecraft.WeaponEnergyConsumption && spacecraft.WeaponCooldown <= 0)
            {
                projectileSpawnerBehaviour.SpawnProjectile(this);
                spacecraft.CurrentEnergy -= spacecraft.WeaponEnergyConsumption;
                spacecraft.WeaponCooldown = spacecraft.WeaponsRateOfFire;

                GameFrame.Base.Audio.Effects.PlayAt("ProjectileFired", this.transform.position);
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
