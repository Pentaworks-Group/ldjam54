using System;
using System.Collections;
using System.Collections.Generic;

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

        private float energy = 50;
        private float maxEnergy = 200;
        private float accelerateEnergyUsage = 2;
        private float deAccelerateEnergyUsage = 1;
        private float turnEnergyUsage = 1;
        private float fireEnergyUsage = 20;
        private float energyHarvesting = 20000f;
        private float consumption = 5;

        private float fireCooldown = 0;
        private float fireRate = 2;


        private void Awake()
        {

            keyBindings.Add(KeyCode.W, Accelerate);
            keyBindings.Add(KeyCode.S, DeAccelerate);
            keyBindings.Add(KeyCode.A, TurnLeft);
            keyBindings.Add(KeyCode.D, TurnRight);
            keyBindings.Add(KeyCode.Space, FireProjectile);
        }

        private void Start()
        {
            rb = gameObject.GetComponent<GravityBehaviour>().Rb;
            rb.AddForce(new Vector3(0, 0, 100));
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
            energy -= consumption * Time.deltaTime;
            HarvestEnergy();
            if (energy < 0)
            {
                TriggerGameOver();
            }
            energyBar.anchorMax = new Vector2(energy / maxEnergy, 1);
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
            var energyGain = energyHarvesting * Time.deltaTime / sqrDistance;
            energy += energyGain;
            energy = Mathf.Min(energy, maxEnergy);
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
                rb.AddForce(rb.transform.forward * 1);
                energy -= accelerateEnergyUsage;
            }
        }

        private void DeAccelerate()
        {
            if (energy > deAccelerateEnergyUsage)
            {
                rb.AddForce(rb.transform.forward * -0.1f);
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
