using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Scenes.Menues;

using TMPro;

using UnityEngine;

namespace Assets.Scripts.Scenes.GameOver
{
    public class GameOverBehaviour : BaseMenuBehaviour
    {
        [SerializeField]
        private GameObject playerDeathTemplate;

        private void Awake()
        {
            var deaths = Base.Core.Game.State.DeadShips;
            int cnt = 0;
            var rectTemplate = playerDeathTemplate.GetComponent<RectTransform>();
            var minX = rectTemplate.anchorMin.x;
            var minY = rectTemplate.anchorMin.y;
            var maxX = rectTemplate.anchorMax.x;
            var maxY = rectTemplate.anchorMax.y;
            foreach (var death in deaths)
            {
                var deathSlot = Instantiate(playerDeathTemplate, playerDeathTemplate.transform.parent);
                var rect = deathSlot.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(minX, minY - 0.1f * cnt);
                rect.anchorMax = new Vector2(maxX, maxY - 0.1f * cnt);
                deathSlot.transform.Find("Player").GetComponent<TextMeshProUGUI>().text = death.Key;
                deathSlot.transform.Find("Death").GetComponent<TextMeshProUGUI>().text = death.Value;
                deathSlot.SetActive(true);
                cnt++;
            }
        }

        public void Retry()
        {
            Base.Core.Game.Start();
        }
    }
}
