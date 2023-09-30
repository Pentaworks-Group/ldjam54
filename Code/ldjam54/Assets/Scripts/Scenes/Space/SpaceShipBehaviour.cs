using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class SpaceShipBehaviour : MonoBehaviour
    {
        private Dictionary<KeyCode, Action> keyBindings = new Dictionary<KeyCode, Action>();

        private Rigidbody rb;

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
        }

        private void Accelerate()
        {
            //rb.AddRelativeForce(new Vector3(1, 0, 0));
            rb.AddRelativeForce(rb.transform.forward * -1);

            Debug.Log("Accelerate: " + rb.transform.forward);
        }

        private void DeAccelerate()
        {
            //rb.AddRelativeForce(new Vector3(-1, 0, 0));
            rb.AddRelativeForce(rb.transform.forward);
            Debug.Log("DeAccelerate: " + rb.transform.forward);
        }

        private void TurnLeft()
        {
            rb.AddRelativeTorque(new Vector3(0, 0, 1));
            Debug.Log("TurnLeft");
        }

        private void TurnRight()
        {
            rb.AddRelativeTorque(new Vector3(0, 0, -1));
            Debug.Log("TurnRight");
        }
    }
}
