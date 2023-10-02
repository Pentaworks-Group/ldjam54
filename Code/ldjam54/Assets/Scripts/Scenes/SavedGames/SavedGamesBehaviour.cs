
using Assets.Scripts.Scenes.Menues;

using UnityEngine;

namespace Assets.Scripts.Scenes.SavedGames
{
    public class SavedGamesBehaviour : BaseMenuBehaviour
    {
        private void Awake()
        {
            var tt = transform.GetComponentsInChildren<TextAutoSizeController>(true);
            Debug.Log(tt.Length);
        }
    }
}
