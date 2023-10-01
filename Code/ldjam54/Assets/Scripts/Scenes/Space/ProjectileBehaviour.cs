using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class ProjectileBehaviour : GravityBehaviour
    {
        private float remainingTimeToLive = 3;

        private void Start()
        {
            gameObject.tag = "Projectile";
        }
        private void Update()
        {
            if (remainingTimeToLive > 0)
            {
                remainingTimeToLive -= Time.deltaTime;
                Rb.transform.rotation = Quaternion.LookRotation(Rb.velocity, transform.up);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(this.gameObject);
        }


    }
}
