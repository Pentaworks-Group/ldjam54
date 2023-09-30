using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class SpaceShipBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SpaceBehaviour spaceBehaviour;
        [SerializeField]
        private GravityManagerBehaviour gravityCenter;
        [SerializeField]
        private RectTransform energyBar;

        private Dictionary<KeyCode, Action> keyBindings = new Dictionary<KeyCode, Action>();
        private Rigidbody rb;

        private float energy = 50;
        private float maxEnergy = 100;
        private float accelerateEnergyUsage = 10;
        private float deAccelerateEnergyUsage = 5;
        private float turnEnergyUsage = 5;
        private float energyHarvesting = 200f;
        private float consumption = 1;


        private void Awake()
        {

            keyBindings.Add(KeyCode.W, Accelerate);
            keyBindings.Add(KeyCode.S, DeAccelerate);
            keyBindings.Add(KeyCode.A, TurnLeft);
            keyBindings.Add(KeyCode.D, TurnRight);
        }

        private void Start()
        {
            rb = gameObject.GetComponent<GravityBehaviour>().Rb;

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
        }

   
        private void HarvestEnergy()
        {
            Vector3 direction = rb.position - gravityCenter.Rb.position;
            float sqrDistance = direction.sqrMagnitude;

            var energyGain = energyHarvesting * Time.deltaTime / sqrDistance;
            energy += energyGain;
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
                rb.AddForce(rb.transform.forward);
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
                rb.AddRelativeTorque(new Vector3(0, 1, 0));
                energy -= turnEnergyUsage;
            }
        }

        private void TurnRight()
        {
            if (energy > turnEnergyUsage)
            {
                rb.AddRelativeTorque(new Vector3(0, -1, 0));
                energy -= turnEnergyUsage;
            }
        }
    }
}
