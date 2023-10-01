using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Scenes.Space
{
    public class ContinousKey : MonoBehaviour
    {
        private float interval = 0.1f;

        private float startTime = 0f;

        private Action repeatingMethod;

        private Action clickMethod;

        public void Init(Action repeatingMethod, Action clickMethod)
        {
            this.repeatingMethod = repeatingMethod;
            this.clickMethod = clickMethod; 
        }

        public void KeyDown()
        {
            if (repeatingMethod != default)
            {
                InvokeRepeating("RepeatingCall", startTime, interval);
            }
            if (clickMethod != default)
            {
                clickMethod.Invoke();
            }
        }

        public void KeyUp()
        {
            CancelInvoke("RepeatingCall");
        }

        void RepeatingCall()
        {
            repeatingMethod.Invoke();
        }
    }
}
