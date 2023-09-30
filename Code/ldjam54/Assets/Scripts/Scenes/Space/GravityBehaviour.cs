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
            Rb = gameObject.AddComponent<Rigidbody>();
            Rb.useGravity = false;
            Rb.constraints = RigidbodyConstraints.FreezePositionY;
            Rb.AddForce(new Vector3(0, 0, 100));
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
