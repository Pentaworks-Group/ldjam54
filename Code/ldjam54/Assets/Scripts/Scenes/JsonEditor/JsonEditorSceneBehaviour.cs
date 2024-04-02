using System;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Definitions;

using Newtonsoft.Json.Linq;

using UnityEngine;

namespace Assets.Scripts
{
    public class JsonEditorSceneBehaviour : MonoBehaviour
    {
        [SerializeField]
        private JsonEditorManagerBehaviour EditorManagerBehaviour;

        void Start()
        {
            //editorBaseBehaviour.PrepareEitor(new GameMode());
            //editorBaseBehaviour.PrepareEitor(new Star());
            var gameMode = Base.Core.Game.AvailableGameModes[0];
            var filePath = $"{Application.streamingAssetsPath}/GameModes.json";
            //Handler.DeserializeObjectFromStreamingAssets(filePath, GetJObject);
            var gameO = new GameObject();

            var mono = gameO.AddComponent<EmptyLoadingBehaviour>();
            _ = mono.StartCoroutine(GameFrame.Core.Json.Handler.DeserializeObjectFromStreamingAssets<JArray>(filePath, OpenFirstGameMode));
        }


        private JArray OpenFirstGameMode(JArray gameModes)
        {
            EditorManagerBehaviour.OpenEditor(new GameMode(), (JObject)gameModes[0], ClosingEditor);
            return gameModes;
        }

        private void ClosingEditor(JObject gameMode)
        {
            Debug.Log("Closed");
        }
    }
}
