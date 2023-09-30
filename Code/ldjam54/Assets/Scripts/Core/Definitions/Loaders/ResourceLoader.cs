using System;
using System.Collections.Generic;
using System.IO;

using GameFrame.Core.Extensions;

using UnityEngine;

namespace Assets.Scripts.Core.Definitions.Loaders
{
    public class ResourceLoader<TDefinition> where TDefinition : BaseDefinition
    {
        protected readonly Dictionary<String, TDefinition> targetCache;

        public ResourceLoader(Dictionary<String, TDefinition> targetCache)
        {
            if (targetCache == default)
            {
                throw new ArgumentNullException(nameof(targetCache), "Definition cache is required and may not be null!");
            }

            this.targetCache = targetCache;
        }

        public virtual void LoadDefinition(String resourceName)
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, resourceName);

            LoadAsset<List<TDefinition>>(filePath, (loadedSpacecrafts) => { return LoadDefinition(loadedSpacecrafts); });
        }

        protected virtual void LoadAsset<TItem>(String filePath, Func<TItem, TItem> onLoadedCallback)
        {
            var gameO = new GameObject();

            var mono = gameO.AddComponent<EmptyLoadingBehaviour>();

            _ = mono.StartCoroutine(GameFrame.Core.Json.Handler.DeserializeObjectFromStreamingAssets(filePath, onLoadedCallback));

            GameObject.Destroy(gameO);
        }

        protected virtual List<TDefinition> LoadDefinition(List<TDefinition> sourceList)
        {
            if (sourceList == default)
            {
                throw new ArgumentNullException(nameof(sourceList), "Definition source list may not be null!");
            }

            if (sourceList?.Count > 0)
            {
                foreach (var loadedDefinition in sourceList)
                {
                    if (loadedDefinition.Reference.HasValue())
                    {
                        this.targetCache[loadedDefinition.Reference] = loadedDefinition;
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(BaseDefinition.Reference), "Reference of Definition may not be Null, Empty or WhiteSpace!");
                    }
                }
            }

            return sourceList;
        }
    }
}
