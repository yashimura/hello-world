using UnityEngine;
using UnityEngine.Events;

namespace Mix2App.MiniGame1 {
    public class UICore: MonoBehaviour {
        #region Events
        private UnityEvent GameAbortEvent = new UnityEvent();
        private UnityEvent GamePauseEvent= new UnityEvent();
        private UnityEvent GameResumeEvent = new UnityEvent();

        public void AddGameAbortListener(UnityAction listener) {
            GameAbortEvent.AddListener(listener);
        }

        public void AddGamePauseListener(UnityAction listener) {
            GamePauseEvent.AddListener(listener);
        }

        public void AddGameResumeListener(UnityAction listener) {
            GameResumeEvent.AddListener(listener);
        }
        #endregion

        public void ScoreChange(int score) {

        }

        public void TimeChange(int time) {

        }

        public void GameFinish() {

        }



#if UNITY_EDITOR
        /// <summary>
        /// Check this component 
        /// </summary>
        public virtual void DebugCheck() {
        }
#endif
    }
}