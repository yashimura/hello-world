using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Mix2App.UI;

namespace Mix2App.MiniGame1.UI {
    public class GameUI: MonoBehaviour {
        [SerializeField] private int MaxScore = 9999;
        [SerializeField] private int MaxTime = 59;

        [SerializeField, Required] private Text ScoreText;
        [SerializeField, Required] private Text TimeText;
        [SerializeField, Required] private Button BackButton;

        public void SetScoreDisplay(int score) {
            if (score > MaxScore)
                score = MaxScore;

            ScoreText.text = score.ToString();
        }

        public void SetTimeDisplay(int time) {
            if (time > MaxTime)
                time = MaxTime;

            TimeText.text = time.ToString();
        }

        public void AddBackAction(UnityAction action) {
            BackButton.onClick.AddListener(action);
        }

        public void Show() {
            this.gameObject.SetActive(true);
        }

        public void Hide() {
            this.gameObject.SetActive(false);
        }
    }
}
