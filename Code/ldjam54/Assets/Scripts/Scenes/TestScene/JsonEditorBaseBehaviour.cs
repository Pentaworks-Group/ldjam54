using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Assets.Scripts.Core.Definitions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public abstract class JsonEditorBaseBehaviour : MonoBehaviour, IJsonEditorSlotParent
    {
        private Dictionary<String, GameObject> templatesDict;

        private Dictionary<String, JsonEditorSlotBaseBehaviour> slots;

        private Dictionary<String, PropertyInfo> possibleProperties;

        protected Dictionary<String, Func<List<String>>> dropDownDataProvider;

        protected Dictionary<String, object> dropDownEditorProvider;

        private Transform slotsParent;

        private float slotSize = 0.05f;

        [SerializeField]
        private Button GenerateJsonButton;

        [SerializeField]
        private JsonEditorItemSelectorBehaviour selectorBehaviour;


        private System.Object DesiredObject;


        private bool initialised = false;


        private void Start()
        {
            Initialise();
        }

        public void Initialise()
        {
            if (!initialised)
            {
                slotsParent = transform.Find("SlotContainer/Slots");
                FillTemplatesDict();
                FillDropdownDataProvider();
                FillEditorObjectProvider();
                GenerateJsonButton.interactable = false;
                initialised = true;
            }
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


        public object GetCustomObject(String name)
        {
            return dropDownEditorProvider[name];
        }

        protected abstract void FillDropdownDataProvider();
        protected abstract void FillEditorObjectProvider();

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

        private void PopulatePossibleProperties()
        {
            possibleProperties = new Dictionary<string, PropertyInfo>();
            foreach (var prop in DesiredObject.GetType().GetProperties())
            {
                if (prop.Name == "IsReferenced")
                {
                    continue;
                }
                possibleProperties.Add(prop.Name, prop);
            }

        }


        public void PrepareEitor(System.Object desiredObject, String json = null)
        {
            Initialise();
            if (desiredObject == null)
            {
                throw new ArgumentNullException("DesiredObject required");
            }
            this.DesiredObject = desiredObject;
            slots = new Dictionary<String, JsonEditorSlotBaseBehaviour>();
            PopulatePossibleProperties();
            if (string.IsNullOrEmpty(json))
            {
                SpawnPropertyTemplatesWithoutJson();
            }
            else
            {
                SpawnPropertyTemplatesWithJson(json);
            }
            UpdateGraphics();
            selectorBehaviour.UpdateOptions();
        }

        private void SpawnPropertyTemplatesWithJson(String json)
        {
            var jsonObject = new JObject(json);
            foreach (var prop in possibleProperties)
            {
                if (!jsonObject.ContainsKey(prop.Key))
                {
                    SpawnPropertyInfo(prop.Value);
                }
            }
        }

        private void SpawnPropertyTemplatesWithoutJson()
        {
            foreach (var prop in possibleProperties)
            {
                SpawnPropertyInfo(prop.Value);
            }
        }


        private void SpawnPropertyInfo(PropertyInfo prop)
        {
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
            if (slots.Count == 0)
            {
                GenerateJsonButton.interactable = false;
                return;
            }
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

        public List<String> GetNotUsedOptions()
        {
            var options = new List<String>();
            foreach (var item in possibleProperties.Keys)
            {
                if (!slots.ContainsKey(item))
                {
                    options.Add(item);
                }
            }
            return options;
        }

        public void SpawnOption(String name)
        {
            var property = possibleProperties[name];
            if (!slots.ContainsKey(name))
            {
                SpawnPropertyInfo(property);
                UpdateGraphics();
            }
        }

        public void UpdateByChild(String childName)
        {
            UpdateValidState();
        }

        public void RemoveChild(JsonEditorSlotBaseBehaviour child)
        {
            slots.Remove(child.name);
            UpdateGraphics();
            UpdateValidState();
            selectorBehaviour.UpdateOptions();
        }

        public void ClearSelected()
        {

            foreach (var item in slots)
            {
                Destroy(item.Value.gameObject);
            }
            slots.Clear();
            UpdateGraphics();
            UpdateValidState();
            selectorBehaviour.UpdateOptions();
        }
    }
}
