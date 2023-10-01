using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Scenes.Space.InputHandling
{
    public class ContinousButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private float interval = 0.01f;
        [SerializeField] private float startTime = 0f;
        [SerializeField] private UnityEvent repeatingMethod;
        [SerializeField] private UnityEvent clickMethod;

        [SerializeField] private float t1t;

        public void OnPointerDown(PointerEventData eventdata)
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

        public void OnPointerUp(PointerEventData eventdata)
        {
            CancelInvoke("RepeatingCall");
        }

        void RepeatingCall()
        {
            repeatingMethod.Invoke();
        }
    }
}
