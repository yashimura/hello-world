using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mix2App.Lib.System;
using Mix2App.MiniGame1.Events;

namespace Mix2App.MiniGame1 {
    public class SceneCore: MonoBehaviour, IReceiver {        

        private UnityIntEvent ScoreChangeEvent = new UnityIntEvent();
        private UnityEvent GameFinishEvent = new UnityEvent();


        [SerializeField] private UICore _UICore;
        [SerializeField] private GameCore _GameCore;

        public void receive(params object[] parameter) {
            
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
