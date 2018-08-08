using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mix2App.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Mix2App.UI.Events;

namespace Mix2App.MiniGame1.UI {
    public class GameUI: MonoBehaviour {
        [SerializeField] private int MaxScore = 9999;
        [SerializeField] private int MaxTime = 59;

        [SerializeField] private Text ScoreText;
        [SerializeField] private Text TimeText;
        [SerializeField] private Button BackButton;

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

#if UNITY_EDITOR
        public void DebugCheck() {
            if (ScoreText == null)
                Debug.LogError("Score Text must be assigned!");

            if (TimeText == null)
                Debug.LogError("Time Text must be assigned!");

            if (BackButton == null)
                Debug.LogError("Back Button must be assigned!");
        }
#endif
    }
}
