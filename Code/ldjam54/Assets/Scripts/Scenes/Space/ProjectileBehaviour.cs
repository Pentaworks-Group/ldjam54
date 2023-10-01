using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class ProjectileBehaviour : GravityBehaviour
    {
        private float remainingTimeToLive = 3;
        private void Update()
        {
            if (remainingTimeToLive > 0)
            {
                remainingTimeToLive -= Time.deltaTime;
                Rb.transform.rotation = Quaternion.LookRotation(Rb.velocity, transform.up);
            } else
            {
                Destroy(gameObject);
                Debug.Log("Destroy Projectile");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(this.gameObject);
            Debug.Log("Projectile Collision");
        }


    }
}
