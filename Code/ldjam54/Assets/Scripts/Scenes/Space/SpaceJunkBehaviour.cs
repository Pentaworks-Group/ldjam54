using UnityEngine;

namespace Assets.Scripts.Scenes.Space
{
    public class SpaceJunkBehaviour : GravityBehaviour
    {
        private GameObject model;
        private bool isDead = false;

        private void Start()
        {
            model.tag = "Junk";
        }

        public void SetModel(GameObject model)
        {
            this.model = model;
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
            if (isDead == false)
            {
                gameObject.transform.Find("Explosion").gameObject.SetActive(true);
                model.SetActive(false);
                Destroy(gameObject, 3);
                Rb.velocity = Vector3.zero;
                Rb.constraints = RigidbodyConstraints.FreezeAll;
                isDead = true;
            }
        }
    }
}
