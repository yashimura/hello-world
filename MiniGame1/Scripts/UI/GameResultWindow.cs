using Mix2App.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Mix2App.MiniGame1.UI {
    public class GameResultWindow:UIWindow {
        [SerializeField, Required] private Text ScoreText;

        public GameResultWindow SetScore(int score) {
            ScoreText.text = score.ToString();
            return this;
        }
    }
}
