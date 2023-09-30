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

        [SerializeField]
        private SpaceBehaviour spaceBehaviour;

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

        public void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
            //if (collision.relativeVelocity.magnitude > 2)
            //    audioSource.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            spaceBehaviour.TriggerGameOver();
        }


        private void Accelerate()
        {
            //rb.AddRelativeForce(new Vector3(1, 0, 0));
            rb.AddForce(rb.transform.forward);
        }

        private void DeAccelerate()
        {
            //rb.AddRelativeForce(new Vector3(-1, 0, 0));
            rb.AddForce(rb.transform.forward * -0.1f);
        }

        private void TurnLeft()
        {
            rb.AddRelativeTorque(new Vector3(0, 1, 0));
        }

        private void TurnRight()
        {
            rb.AddRelativeTorque(new Vector3(0, -1, 0));
        }
    }
}
