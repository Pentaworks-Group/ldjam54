using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class GravityManagerBehaviour : MonoBehaviour
    {
        private static List<GravityBehaviour> BodiesToAttract = new List<GravityBehaviour>();

        public Rigidbody Rb { get; private set; }


        float G = 60f;
        const float MaxAcceleration = 200;

        private void Awake()
        {
            Rb = gameObject.AddComponent<Rigidbody>();
            Rb.useGravity = false;
            Rb.constraints = RigidbodyConstraints.FreezeAll;

            var star = Core.Game.SelectedGameMode.Stars[0];
            this.Rb.mass = (float)star.Mass;
            G = (float)star.Gravity;
        }

        private void FixedUpdate()
        {
            foreach (GravityBehaviour body in BodiesToAttract)
            {
                AttractBody(body);
            }
        }

        //public void OnCollisionEnter(Collision collision)
        //{
        //    foreach (ContactPoint contact in collision.contacts)
        //    {
        //        Debug.DrawRay(contact.point, contact.normal, Color.white);
        //    }
        //    //if (collision.relativeVelocity.magnitude > 2)
        //    //    audioSource.Play();
        //}

        //private void OnTriggerEnter(Collider other)
        //{
        //    Debug.Log("dada");
        //}

        public static void RegisterBody(GravityBehaviour gravityBehaviour)
        {
            BodiesToAttract.Add(gravityBehaviour);
        }

        public static void DeRegisterBody(GravityBehaviour gravityBehaviour)
        {
            BodiesToAttract.Remove(gravityBehaviour);
        } 

        private void AttractBody(GravityBehaviour gravityBehaviour)
        {
            Rigidbody rbToAttract = gravityBehaviour.Rb;

            Vector3 direction = Rb.position - rbToAttract.position;
            float sqrDistance = direction.sqrMagnitude;

            if (sqrDistance <= 0f)
                return;

            float forceMagnitude = G * (Rb.mass * rbToAttract.mass) / sqrDistance;
            forceMagnitude = Mathf.Min(forceMagnitude, MaxAcceleration);
            Vector3 force = direction.normalized * forceMagnitude;

            rbToAttract.AddForce(force);
        }
    }
}

