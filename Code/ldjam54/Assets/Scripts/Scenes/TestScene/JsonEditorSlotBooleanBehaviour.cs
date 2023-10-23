using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;

using Newtonsoft.Json.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class JsonEditorSlotBooleanBehaviour : JsonEditorSlotBaseBehaviour
    {

        [SerializeField]
        private Toggle toggle;

        private bool value;


        private void Start()
        {
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        public void OnValueChanged(bool value)
        {
            this.value = value;
            SetValid();
        }

        public override JToken GenerateToken()
        {
            return new JValue(value);
        }

        public override Int32 Size()
        {
            return 1;
        }
    }
}
