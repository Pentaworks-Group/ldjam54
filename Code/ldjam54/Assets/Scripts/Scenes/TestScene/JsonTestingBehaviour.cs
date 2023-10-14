using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Definitions;

using UnityEngine;

namespace Assets.Scripts
{
    public class JsonTestingBehaviour : MonoBehaviour
    {
        //private void Awake()
        ////{
        //var tt = GameFrame.Core.Json.Handler.DeserializeTest<GameMode>(GetFileContent("DefaultGameMode.json"), GetFileContent("CustomGameMode.json"));
        //    var ss = GameFrame.Core.Json.Handler.SerializePretty(tt);
        //    Debug.Log(ss);
        //}

        //public virtual void LoadDefinition(String resourceName)
        //{
        //    var filePath = $"{Application.streamingAssetsPath}/{resourceName}";

        //    LoadAsset(filePath, HandleDefinitions);
        //}

        private String GetFileContent(String resourceName)
        {
            var filePath = $"{Application.streamingAssetsPath}/{resourceName}";
            return File.ReadAllText(filePath);
        }

        //private String getDefault()
        //{
        //    return "{\r\n    \"Reference\": \"default\",\r\n    \"Name\": \"Default GameMode\",\r\n    \"Description\": \"You against the SpaceJunk\",\r\n    \"ShipSpawnDistance\": 9,\r\n    \"JunkSpawnInterval\": 2,\r\n    \"JunkSpawnInitialDistance\": 20,\r\n    \"JunkSpawnPosition\": {\r\n        \"Min\": -7,\r\n        \"Max\": 7\r\n    },\r\n    \"JunkSpawnForce\": {\r\n        \"Min\": -6,\r\n        \"Max\": 6\r\n    },\r\n    \"JunkSpawnTorque\": {\r\n        \"Min\": -3,\r\n        \"Max\": 3\r\n    },\r\n    \"Stars\": [\r\n        {\r\n            \"IsReferenced\": true,\r\n            \"Reference\": \"DaSun\"\r\n        }\r\n    ],\r\n    \"PlayerSpacecrafts\": [\r\n        {\r\n            \"IsReferenced\": true,\r\n            \"Reference\": \"Spacecraft1\"\r\n        }\r\n    ],\r\n    \"Spacecrafts\": []\r\n}\r\n";
        //}


        protected void LoadAsset(String filePath)
        {
            //var gameO = new GameObject();

            //var mono = gameO.AddComponent<EmptyLoadingBehaviour>();

           var tt = GameFrame.Core.Json.Handler.DeserializeTest<GameMode>(GetFileContent("DefaultGameMode.json"), GetFileContent("CustomGameMode.json"));

            //GameObject.Destroy(gameO);
        }
    }
}
