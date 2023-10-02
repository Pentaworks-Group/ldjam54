using Assets.Scripts.Core.Model;

using GameFrame.Core.Extensions;

using UnityEngine;

using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Scenes.Space
{
    public class SpaceJunkBehaviour : GravityBehaviour
    {
        private GameObject modelGameObject;
        private SpaceJunk spaceJunk;
        private bool isDead = false;

        private void Start()
        {
            modelGameObject.tag = "Junk";
        }

        public void SetModel(SpaceJunk spaceJunk, GameObject model)
        {
            this.spaceJunk = spaceJunk;
            this.modelGameObject = model;
        }

        private void Update()
        {
            Vector3 position = transform.position;

            if (position.x > 100 || position.x < -100 || position.z > 100 || position.z < -100)
            {
                Base.Core.Game.State.SpaceJunks.Remove(spaceJunk);
                Destroy(gameObject);
            }
            else
            {
                spaceJunk.Velocity = this.Rb.velocity.ToFrame();
                spaceJunk.Position = position.ToFrame();
                spaceJunk.Rotation = transform.eulerAngles.ToFrame();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isDead == false)
            {
                gameObject.transform.Find("Explosion").gameObject.SetActive(true);
                modelGameObject.SetActive(false);
                Base.Core.Game.State.SpaceJunks.Remove(spaceJunk);
                Destroy(gameObject, 3);
                Rb.velocity = Vector3.zero;
                Rb.constraints = RigidbodyConstraints.FreezeAll;
                isDead = true;
            }
        }
    }
}
