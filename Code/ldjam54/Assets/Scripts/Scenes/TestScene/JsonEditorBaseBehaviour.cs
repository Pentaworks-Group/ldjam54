using System;
using System.Collections.Generic;
using System.IO;

using Assets.Scripts.Core.Definitions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Unity.VisualScripting.Antlr3.Runtime.Misc;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public abstract class JsonEditorBaseBehaviour : MonoBehaviour, IJsonEditorSlotParent
    {
        private String defaultJson;
        private String rawJson;
        private JObject defaultJsonObject;

        private Dictionary<String, GameObject> templatesDict;

        private Dictionary<String, JsonEditorSlotBaseBehaviour> slots;

        protected Dictionary<String, Func<List<String>>> dropDownDataProvider;

        private Transform slotsParent;

        private float slotSize = 0.05f;

        [SerializeField]
        private Button GenerateJsonButton;

        private void Awake()
        {
            slotsParent = transform.Find("SlotContainer/Slots");
            defaultJson = GetFileContent("DefaultGameMode.json");
            FillTemplatesDict();
            FillDropdownDataProvider();
            PrepareEitor();
            GenerateJsonButton.interactable = false;
        }

        public void GenereateModeJson()
        {
            JObject customJsonObject = new JObject();
            foreach (var item in slots)
            {
                customJsonObject[item.Key] = item.Value.GenerateToken();
            }

            String json = customJsonObject.ToString();
            Debug.Log(json);
            var tt = GameFrame.Core.Json.Handler.Deserialize<GameMode>(json);
            Debug.Log(tt);
        }

        public List<String> GetDropDownOptions(String name)
        {
            return dropDownDataProvider[name].Invoke();
        }

        protected abstract void FillDropdownDataProvider();

        private void FillTemplatesDict()
        {
            templatesDict = new Dictionary<String, GameObject>();
            foreach (Transform tran in transform.Find("SlotContainer/Templates"))
            {
                var behave = tran.GetComponent<JsonEditorSlotBaseBehaviour>();
                foreach (var type in behave.UsedForPropertyTypes())
                {
                    templatesDict.Add(type, behave.gameObject);
                }
            }
        }

        private void PrepareEitor()
        {
            slots = new Dictionary<String, JsonEditorSlotBaseBehaviour>();
            GameMode foo = new GameMode();
            foreach (var prop in foo.GetType().GetProperties())
            {
                if (prop.Name == "IsReferenced")
                {
                    continue;
                }
                if (templatesDict.TryGetValue(prop.PropertyType.Name, out GameObject template))
                {
                    SpawnSlot(prop.Name, template);
                }
                else
                {
                    var nullType = Nullable.GetUnderlyingType(prop.PropertyType);
                    if (nullType != null)
                    {
                        if (templatesDict.TryGetValue(nullType.Name, out GameObject template1))
                        {
                            SpawnSlot(prop.Name, template1);
                        }
                        else
                        {
                            Debug.Log("Unknown Type " + prop.PropertyType.Name + " for " + prop.Name);
                        }
                    }
                    else if ((prop.PropertyType.IsGenericType && (prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))))
                    {
                        templatesDict.TryGetValue("List", out GameObject listTemplate);
                        Type itemType = prop.PropertyType.GetGenericArguments()[0];
                        if (templatesDict.TryGetValue(itemType.Name, out GameObject slotTemplate))
                        {
                            var listBehaviour = (JsonEditorSlotListBehaviour)SpawnSlot(prop.Name, listTemplate);
                            listBehaviour.InitList(slotTemplate);
                        }
                        else
                        {
                            Debug.Log("Unknown Type " + itemType.Name + " for " + prop.Name);
                        }
                    }
                    else
                    {
                        Debug.Log("Unknown Type " + prop.PropertyType.Name + " for " + prop.Name);
                    }
                }
            }
            UpdateGraphics();
        }

        public void UpdateGraphics()
        {
            float sizeSum = 0;
            foreach (var item in slots)
            {
                var slotBehaviour = item.Value;
                var rect = slotBehaviour.GetComponent<RectTransform>();
                float size = slotBehaviour.Size() * slotSize;
                rect.anchorMax = new Vector2(1, 1 - sizeSum);
                sizeSum += size;
                rect.anchorMin = new Vector2(0, 1 - sizeSum);
            }
        }


        private JsonEditorSlotBaseBehaviour SpawnSlot(string name, GameObject template)
        {
            var slot = Instantiate(template, slotsParent);
            var templateBehaviour = slot.GetComponent<JsonEditorSlotBaseBehaviour>();
            slots[name] = templateBehaviour;
            templateBehaviour.InitSlotBehaviour(this, name, this);
            slot.SetActive(true);
            return templateBehaviour;
        }

        private String GetFileContent(String resourceName)
        {
            var filePath = $"{Application.streamingAssetsPath}/{resourceName}";
            return File.ReadAllText(filePath);
        }

        public static T Deserialize<T>(string rawJson, JsonSerializerSettings serializerSettings = null)
        {
            if (serializerSettings == null)
            {
                serializerSettings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore
                };
            }

            return JsonConvert.DeserializeObject<T>(rawJson, serializerSettings);
        }

        private void UpdateValidState()
        {
            foreach (var item in slots)
            {
                if (!item.Value.HasValidValues())
                {
                    GenerateJsonButton.interactable = false;
                    return;
                }
            }
            GenerateJsonButton.interactable = true;
        }

        public void UpdateByChild(String childName)
        {
            UpdateValidState();
        }
    }
}
