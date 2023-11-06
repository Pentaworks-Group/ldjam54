using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core.Definitions;

using UnityEngine;

namespace Assets.Scripts
{
    public class JsonEditorManagerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private JsonEditorBaseBehaviour editorBaseBehaviour;


        [SerializeField]
        private GameObject closeButton;


        private List<JsonEditorBaseBehaviour> openEditors = new List<JsonEditorBaseBehaviour>();

        void Start()
        {
            //editorBaseBehaviour.PrepareEitor(new GameMode());
            //editorBaseBehaviour.PrepareEitor(new Star());
            OpenEditor(new GameMode());
        }


        public void OpenEditor(String objectName) {
            var newEditor = CreateNewEditor();
            var objectToOpen = newEditor.GetCustomObject(objectName);
            newEditor.PrepareEitor(objectToOpen);
        }


        public void OpenEditor(object objectToOpen)
        {
            var newEditor = CreateNewEditor();
            newEditor.PrepareEitor(objectToOpen);
        }

        private JsonEditorBaseBehaviour CreateNewEditor()
        {
            if (openEditors.Count != 0)
            {
                openEditors.Last().gameObject.SetActive(false);
                closeButton.SetActive(true);
            }
            var newEditor = Instantiate(editorBaseBehaviour, this.transform);
            newEditor.Initialise(); //TODO can we remove this?
            newEditor.gameObject.SetActive(true);
            openEditors.Add(newEditor);
            return newEditor;
        }

        public void CloseEditor()
        {
            var lastIndex = openEditors.Count - 1;
            var last = openEditors[lastIndex];
            openEditors.RemoveAt(lastIndex);
            Destroy(last.gameObject);
            if (openEditors.Count != 0)
            {
                openEditors.Last().gameObject.SetActive(true);
            } 
            if (openEditors.Count <= 1)
            {
                closeButton.SetActive(false);
            }

        }

    }
}
