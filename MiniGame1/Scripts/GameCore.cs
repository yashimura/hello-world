using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using Mix2App.UI.Events;

namespace Mix2App.MiniGame1 {
    public class GameCore: MonoBehaviour {
        #region Events
        private UnityIntEvent GetScoreEvent = new UnityIntEvent();
        private UnityIntEvent TimeChangeEvent = new UnityIntEvent();

        public void AddGetScoreAction(UnityAction<int> listener) {
            GetScoreEvent.AddListener(listener);
        }

        public void AddTimeChangeAction(UnityAction<int> listener) {
            TimeChangeEvent.AddListener(listener);
        }
        
        private UnityEvent GameFailEvent = new UnityEvent();
        private UnityEvent GameFinishEvent = new UnityEvent();

        public void AddGameFailAction(UnityAction listener) {
            GameFailEvent.AddListener(listener);
        }

        public void AddGameFinishAction(UnityAction listener) {
            GameFinishEvent.AddListener(listener);
        }
        #endregion

        [Tooltip("List of game items")]
        [SerializeField] private GameItem[] GameItems = null;

        /// <summary>
        /// Call this, when Game failed (finish by bomb)
        /// </summary>
        private void GameFail() {
            GameFailEvent.Invoke();
        }

        /// <summary>
        /// Call this, when game finish normaly (by time)
        /// </summary>
        private void GaimFinish() {
            GameFinishEvent.Invoke();
        }

        /// <summary>
        /// Called this, when character get score
        /// </summary>
        /// <param name="score">getted score</param>
        private void CharacterGetScore(int score) {
            GetScoreEvent.Invoke(score);
        }

        /// <summary>
        /// Called this, when remain time changed
        /// </summary>
        /// <param name="remain_time"></param>
        private void ChangeRemainTime(int remain_time) {
            TimeChangeEvent.Invoke(remain_time);
        }

        public void Config(object[] parameter) {
            // Prepare scene (as title menu BG etc.)
        }

        public void GameStart() {
            // Called, when game started
        }

        public void GamePause() {
            // Called, when game paused
        }

        public void GameResume() {
            // Called, when game resumed
        }

        public void GameAbort() {
            // Called, when game aborted or finish.
        }

        public void GetBoxAnimation() {
            // Called, when user get box for this game
        }

        public void GameStartAnimation() {
            // Called, when game game started and need to play animation
        }
			
        public GameItem GameItemGet(int number){
			return GameItems [number];
		}
		public void GameScoreSet(int score){
			CharacterGetScore (score);
		}
    }
}
