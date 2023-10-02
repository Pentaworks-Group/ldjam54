using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class ProjectileBehaviour : GravityBehaviour
    {
        private float remainingTimeToLive = 3;
        private SpaceShipBehaviour source;

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
            var tag = other.tag;
            switch (tag)
            {
                case "Junk":
                    source.IncreaseJunkKillCount();
                    break;
                default:
                    break;
            }
            Destroy(this.gameObject);
        }

        public void Init(SpaceShipBehaviour source)
        {
            this.source = source;
        }
    }
}
