using Assets.Scripts.Constants;

using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class ProjectileBehaviour : GravityBehaviour
    {
        private float remainingTimeToLive = 3;
        private SpacecraftBehaviour source;

        private void Start()
        {
            gameObject.tag = GameObjectTags.Projectile;
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
                case GameObjectTags.Junk:
                    source.IncreaseJunkKillCount();
                    break;
            }

            Destroy(this.gameObject);
        }

        public void Init(SpacecraftBehaviour source)
        {
            this.source = source;
        }
    }
}
