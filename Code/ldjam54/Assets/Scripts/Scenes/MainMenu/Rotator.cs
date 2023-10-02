using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Rotator : MonoBehaviour
    {
        void Update()
        {
            transform.Rotate(0, 20*Time.deltaTime, 0, Space.Self);
        }
    }
}
