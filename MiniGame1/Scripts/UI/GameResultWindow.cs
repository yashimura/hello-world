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
    public class GameResultWindow:UIWindow {
        [SerializeField] private Text ScoreText;

        public GameResultWindow SetScore(int score) {
            ScoreText.text = score.ToString();
            return this;
        }
    }
}
