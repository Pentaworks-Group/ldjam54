using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class SpaceJunkBehaviour : GravityBehaviour
    {
        private void Start()
        {
            gameObject.tag = "Junk";
        }

        private void Update()
        {
            Vector3 t = transform.position;
            if (t.x > 100 || t.x < -100 || t.z > 100 || t.z < -100)
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
