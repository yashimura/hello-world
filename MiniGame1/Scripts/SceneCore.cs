using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mix2App.Lib.System;
using Mix2App.UI.Events;

namespace Mix2App.MiniGame1 {
    public class SceneCore: MonoBehaviour, IReceiver {        

        private UnityIntEvent ScoreChangeEvent = new UnityIntEvent();
        private UnityEvent GameFinishEvent = new UnityEvent();
        
        [SerializeField] private UICore _UICore;
        [SerializeField] private GameCore _GameCore;
        
        public void receive(params object[] parameter) {
            _GameCore.AddGetScoreAction(ScoreGetted);
            _GameCore.AddTimeChangeAction(_UICore.DisplayTime);

            _UICore.AddGameAbortListener(_GameCore.GameAbort);            
            _UICore.AddGamePauseListener(_GameCore.GamePause);
            _UICore.AddGameResumeListener(_GameCore.GameResume);
            _UICore.AddGameStartAnimationListener(_GameCore.GameStartAnimation);

            _UICore.AddGameStartListener(ForceGameStart);

            _GameCore.Config(parameter);
            _UICore.Config();
        }

        private int Score = 0;

        private void ScoreGetted(int score) {
            Score += score;
            _UICore.DisplayScore(Score);
        }

        private void ForceGameStart() {
            Score = 0;
            _UICore.DisplayScore(Score);
            _GameCore.GameStart();
        }

        private void ForceGameFinish() {

        }

        private void Awake() {
#if UNITY_EDITOR
            DebugCheck();
#endif

            receive();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Check this component 
        /// </summary>
        public virtual void DebugCheck() {
            if (_UICore != null) {
                _UICore.DebugCheck();
            } else
                Debug.LogError("UICore must be assigned!", this);

            if (_GameCore != null) {
                _GameCore.DebugCheck();
            } else
                Debug.LogError("GameCore must be assigned!", this);
        }
#endif
    }
}
