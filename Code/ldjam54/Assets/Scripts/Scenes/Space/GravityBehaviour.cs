using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class GravityBehaviour : MonoBehaviour
    {
        public Rigidbody Rb { get; private set; }

        private void Awake()
        {
            InitGravity();
        }

        public void InitGravity()
        {
            if (Rb == default)
            {
                if (!TryGetComponent<Rigidbody>(out var rigidbody))
                {
                    rigidbody = this.gameObject.AddComponent<Rigidbody>();

                    rigidbody.useGravity = false;
                    rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                }

                Rb = rigidbody;
            }
        }

        void OnEnable()
        {
            GravityManagerBehaviour.RegisterBody(this);
        }

        void OnDisable()
        {
            GravityManagerBehaviour.DeRegisterBody(this);
        }
    }
}
