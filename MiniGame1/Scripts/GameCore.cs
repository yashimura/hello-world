using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using Mix2App.MiniGame1.Events;

namespace Mix2App.MiniGame1 {
    public class GameCore: MonoBehaviour {
        #region Events
        private UnityGameItemEvent GetItemEvent = new UnityGameItemEvent();
        private UnityIntEvent TimeChangeEvent = new UnityIntEvent();

        public void AddGetItemListener(UnityAction<GameItem> listener) {
            GetItemEvent.AddListener(listener);
        }

        public void AddTimeChangeListener(UnityAction<int> listener) {
            TimeChangeEvent.AddListener(listener);
        }
        #endregion

        [Tooltip("List of game items")]
        [SerializeField] private GameItem[] GameItems;

        /// <summary>
        /// Called this, when character get item
        /// </summary>
        /// <param name="item">getted item</param>
        private void CharacterGetItem(GameItem item) {
            GetItemEvent.Invoke(item);
        }

        /// <summary>
        /// Called this, when remain time changed
        /// </summary>
        /// <param name="remain_time"></param>
        private void ChangeRemainTime(int remain_time) {
            TimeChangeEvent.Invoke(remain_time);
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
            // Called, when game aborted
        }


		public GameItem GameItemGet(int number){
			return GameItems [number];
		}
#if UNITY_EDITOR
        /// <summary>
        /// Check this component 
        /// </summary>
        public virtual void DebugCheck() {
            if (GameItems.Length == 0)
                Debug.LogError("Please assign some items!", this);
        }
#endif
    }
}
