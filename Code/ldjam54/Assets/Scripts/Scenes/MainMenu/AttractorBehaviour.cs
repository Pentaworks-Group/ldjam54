using System.Collections.Generic;

using Assets.Scripts.Constants;
using Assets.Scripts.Scenes.Space;

using UnityEngine;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class AttractorBehaviour : MonoBehaviour
    {

        private Rigidbody Rb;

        float G = 60f;
        const float MaxAcceleration = 200;


        private List<AttracteeeBehaviour> BodiesToAttract = new List<AttracteeeBehaviour>();

        private void Awake()
        {
            Rb = gameObject.GetComponent<Rigidbody>();
            Rb.useGravity = false;
            Rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void FixedUpdate()
        {
            foreach (AttracteeeBehaviour body in BodiesToAttract)
            {
                AttractBody(body);
            }
        }

        public void RegisterBody(AttracteeeBehaviour gravityBehaviour)
        {
            BodiesToAttract.Add(gravityBehaviour);
        }

        private void AttractBody(AttracteeeBehaviour gravityBehaviour)
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
