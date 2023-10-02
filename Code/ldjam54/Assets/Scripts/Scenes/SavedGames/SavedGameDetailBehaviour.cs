using Assets.Scripts.Core;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.SavedGames
{
    public class SavedGameDetailBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SavedGameListBehaviour listBehaviour;
        
        [SerializeField]
        private TextAutoSizeController textSizeController;

        private TMP_Text createdOnText;
        private TMP_Text timeStampText;
        private TMP_Text GameModeText;
        private TMP_Text playerCountText;
        private TMP_Text timeElapsedText;

        [SerializeField]
        private GameObject container;

        private string key;

        public void Awake()
        {
            createdOnText = transform.Find("DetailsContainer/Created/Value").GetComponent<TMP_Text>();
            timeStampText = transform.Find("DetailsContainer/TimeStamp/Value").GetComponent<TMP_Text>();
            GameModeText = transform.Find("DetailsContainer/GameMode/Value").GetComponent<TMP_Text>();
            playerCountText = transform.Find("DetailsContainer/PlayerCount/Value").GetComponent<TMP_Text>();
            timeElapsedText = transform.Find("DetailsContainer/TimeElapsed/Value").GetComponent<TMP_Text>();

            textSizeController.AddLabel(createdOnText);
            textSizeController.AddLabel(timeStampText);
            textSizeController.AddLabel(GameModeText);
            textSizeController.AddLabel(playerCountText);
            textSizeController.AddLabel(timeElapsedText);

            var overwriteButton = transform.Find("DetailsContainer/Buttons/Override").GetComponent<Button>();

            overwriteButton.gameObject.SetActive(Base.Core.Game.State != default);
            container.SetActive(false);
        }

        public void DisplayDetails(SavedGamePreview preview)
        {
            createdOnText.text = preview.CreatedOn;
            timeStampText.text = preview.SavedOn;
            GameModeText.text = preview.GameMode;
            playerCountText.text = preview.PlayerCount;
            timeElapsedText.text = preview.TimeElapsed;

            this.key = preview.Key;
            container.SetActive(true);
        }

        public void LoadGame()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.LoadSavedGame(key);
        }

        public void OverrideGame()
        {
            Base.Core.Game.PlayButtonSound();

            Base.Core.Game.OverwriteSavedGame(key);
            listBehaviour.UpdateList();
            ClearDetails();
        }

        public void DeleteGame()
        {
            Base.Core.Game.PlayButtonSound();

            Base.Core.Game.DeleteSavedGame(key);
            listBehaviour.UpdateList();
            ClearDetails();
        }

        public void ClearDetails()
        {
            container.SetActive(false);
        }
    }
}
