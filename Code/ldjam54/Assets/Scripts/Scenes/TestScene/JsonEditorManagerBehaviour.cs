using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Definitions;

using GameFrame.Core.Json;

using Newtonsoft.Json.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class JsonEditorManagerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private JsonEditorBaseBehaviour editorTemplate;


        [SerializeField]
        private GameObject closeButton;
        [SerializeField]
        private Button saveAndCloseButton;


        private List<JsonEditorBaseBehaviour> openEditors = new List<JsonEditorBaseBehaviour>();



        void Start()
        {
            //editorBaseBehaviour.PrepareEitor(new GameMode());
            //editorBaseBehaviour.PrepareEitor(new Star());
            var gameMode = Base.Core.Game.AvailableGameModes[0];
            var filePath = $"{Application.streamingAssetsPath}/GameModes.json";
            //Handler.DeserializeObjectFromStreamingAssets(filePath, GetJObject);
            var gameO = new GameObject();

            var mono = gameO.AddComponent<EmptyLoadingBehaviour>();
            _ = mono.StartCoroutine(GameFrame.Core.Json.Handler.DeserializeObjectFromStreamingAssets<JArray>(filePath, GetJObject));
        }

        public JArray GetJObject(JArray input)
        {
            var newEditor = CreateNewEditor(null);
            newEditor.PrepareEditor(new GameMode(), (JObject)input[0]);
            return input;
        }

        public void OpenEditor(String objectName, Action<JObject> createdObjectAction = null, JObject jsonObject = null) {
            var newEditor = CreateNewEditor(createdObjectAction);
            var objectToOpen = newEditor.GetCustomObject(objectName);
            newEditor.PrepareEditor(objectToOpen, jsonObject);
        }


        public void OpenEditor(object objectToOpen, Action<JObject> createdObjectAction = null)
        {
            var newEditor = CreateNewEditor(createdObjectAction);
            newEditor.PrepareEditor(objectToOpen);
        }

        private JsonEditorBaseBehaviour CreateNewEditor(Action<JObject> createdObjectAction)
        {
            if (openEditors.Count != 0)
            {
                openEditors.Last().gameObject.SetActive(false);
                closeButton.SetActive(true);
                saveAndCloseButton.gameObject.SetActive(true);
            }
            var newEditor = Instantiate(editorTemplate, this.transform);
            newEditor.Initialise(); //TODO can we remove this?
            newEditor.SetCreatedObjectAction(createdObjectAction);
            newEditor.SetUpdateSaveButtonInteractability(UpdateSaveButtonInteractability);
            newEditor.gameObject.SetActive(true);
            openEditors.Add(newEditor);
            return newEditor;
        }

        public void CloseEditor(bool save)
        {
            var lastIndex = openEditors.Count - 1;
            var last = openEditors[lastIndex];
            last.CloseEditor(save);
            openEditors.RemoveAt(lastIndex);
            Destroy(last.gameObject);
            if (openEditors.Count != 0)
            {
                openEditors.Last().gameObject.SetActive(true);
            } 
            if (openEditors.Count <= 1)
            {
                closeButton.SetActive(false);
                saveAndCloseButton.gameObject.SetActive(false);
            }
        }

        public void UpdateSaveButtonInteractability(bool savePossible)
        {
            saveAndCloseButton.interactable = savePossible;
        }

    }
}
