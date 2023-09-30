using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class GravityManagerBehaviour : MonoBehaviour
    {
        private static List<GravityBehaviour> BodiesToAttract = new List<GravityBehaviour>();

        private Rigidbody rb;


        const float G = 60f;

        private void Awake()
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }

        private void FixedUpdate()
        {
            foreach (GravityBehaviour body in BodiesToAttract)
            {
                AttractBody(body);
            }
        }

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

            Vector3 direction = rb.position - rbToAttract.position;
            float sqrDistance = direction.sqrMagnitude;

            if (sqrDistance == 0f)
                return;

            float forceMagnitude = G * (rb.mass * rbToAttract.mass) / sqrDistance;
            Vector3 force = direction.normalized * forceMagnitude;

            rbToAttract.AddForce(force);
        }
    }
}

