using System;
using System.Collections.Generic;
using System.Drawing;

using Newtonsoft.Json.Linq;

using UnityEngine;

namespace Assets.Scripts
{
    public abstract class JsonEditorSlotBaseBehaviour : MonoBehaviour
    {
        private JsonEditorSlotPrefabBehaviour slotPrefabBehaviour;

        protected JsonEditorBaseBehaviour editorBehaviour;

        private IJsonEditorSlotParent parent;

        private bool validValues = false;

        [SerializeField]
        private List<String> UsedForPropertyes;

        private void Awake()
        {
            EnsureSlotPrefabBehaviour();
        }

        public void InitSlotBehaviour(JsonEditorBaseBehaviour editorBehaviour, String name, IJsonEditorSlotParent parent, bool displayName = true)
        {
            this.parent = parent;
            this.editorBehaviour = editorBehaviour;
            EnsureSlotPrefabBehaviour();
            slotPrefabBehaviour.InitSlotBehaviour(name, displayName);
        }

        

        private void EnsureSlotPrefabBehaviour()
        {
            if (slotPrefabBehaviour == null)
            {
                slotPrefabBehaviour = GetComponent<JsonEditorSlotPrefabBehaviour>();
            }
        }

        protected void SetInvalid()
        {
            validValues = false;
            slotPrefabBehaviour.SetInValidColor();
            parent.UpdateByChild(name);
        }

        protected void SetValid()
        {
            validValues = true;
            slotPrefabBehaviour.SetValidColor();
            parent.UpdateByChild(name);
        }

        public bool HasValidValues()
        {
            return  validValues;
        }

        public abstract JToken GenerateToken();

        public abstract int Size();

        public List<String> UsedForPropertyTypes()
        {
            return UsedForPropertyes;
        }

    }
}
