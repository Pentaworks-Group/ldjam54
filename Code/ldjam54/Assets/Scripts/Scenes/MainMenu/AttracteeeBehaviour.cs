using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class AttracteeeBehaviour : MonoBehaviour
    {
        public Rigidbody Rb { get; private set; }

        private void Awake()
        {
            Rb = gameObject.GetComponent<Rigidbody>();
            Rb.useGravity = false;
            Rb.constraints = RigidbodyConstraints.FreezeAll;
            AttractorBehaviour.RegisterBody(this);
            StartCoroutine(CheckSurvivorCount());
        }


        IEnumerator CheckSurvivorCount()
        {
            yield return new WaitForSeconds(4);


            Rb.constraints = RigidbodyConstraints.None;
            Rb.AddForce(CreateRandomVector(-50, 50));
            Rb.AddTorque(CreateRandomVector(-10, 10));
        }

        private Vector3 CreateRandomVector(float min, float max)
        {
            return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
        }
    }
}
