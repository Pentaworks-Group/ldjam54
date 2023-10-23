using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;

using UnityEngine;

namespace Assets.Scripts
{
    public class JsonEditorManagerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private JsonEditorBaseBehaviour editorBaseBehaviour;

        void Start()
        {
            //editorBaseBehaviour.PrepareEitor(new GameMode());
            //editorBaseBehaviour.PrepareEitor(new Star());
            editorBaseBehaviour.PrepareEitor(new Spacecraft());
        }


    }
}
