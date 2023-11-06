using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts
{
    public class JsonEditorSlotCustomObjectBehaviour : MonoBehaviour
    {
        public void OpenNewEditor(JsonEditorManagerBehaviour manager)
        {
            manager.OpenEditor(this.name);
        }
    }
}
