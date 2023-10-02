using UnityEngine;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class Rotator : MonoBehaviour
    {
        void Update()
        {
            transform.Rotate(0, 20*Time.deltaTime, 0, UnityEngine.Space.Self);
        }
    }
}
