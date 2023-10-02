using Assets.Scripts.Constants;
using Assets.Scripts.Core.Model;

using GameFrame.Core.Extensions;

using UnityEngine;

using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Scenes.Space
{
    public class SpaceJunkBehaviour : GravityBehaviour
    {
        [SerializeField]
        private SpaceBehaviour spaceBehaviour;

        private GameObject modelGameObject;
        public SpaceJunk spaceJunk { private set; get; }
        private bool isDead = false;

        private void Start()
        {
            modelGameObject.tag = GameObjectTags.Junk;
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
                spaceBehaviour.RemoveSpaceJunk(this);
                GameFrame.Base.Audio.Effects.PlayAt("Explosion_Junk", this.transform.position);

                Destroy(gameObject);
            }
        }

        public void SerializeSpaceJunk()
        {
            spaceJunk.Velocity = this.Rb.velocity.ToFrame();
            spaceJunk.Position = transform.position.ToFrame();
            spaceJunk.Rotation = transform.eulerAngles.ToFrame();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isDead == false)
            {
                gameObject.transform.Find("Explosion").gameObject.SetActive(true);
                modelGameObject.SetActive(false);
                Base.Core.Game.State.SpaceJunks.Remove(spaceJunk);
                spaceBehaviour.RemoveSpaceJunk(this);
                GameFrame.Base.Audio.Effects.PlayAt("Explosion_Junk", this.transform.position);
                Destroy(gameObject, 3);
                Rb.velocity = Vector3.zero;
                Rb.constraints = RigidbodyConstraints.FreezeAll;
                isDead = true;
            }
        }
    }
}
