using System.Collections;
using System.Collections.Generic;

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
                Rb = this.gameObject.GetComponent<Rigidbody>();
                if (Rb == default)
                {
                    Rb = this.gameObject.AddComponent<Rigidbody>();
                    Rb.useGravity = false;
                    Rb.constraints = RigidbodyConstraints.FreezePositionY;
                }
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
