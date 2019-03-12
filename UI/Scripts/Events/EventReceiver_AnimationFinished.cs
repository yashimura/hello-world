using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Mix2App.UI.Events {
    /// <summary>
    /// Reciever for animation finish action.
    /// </summary>
    public class EventReceiver_AnimationFinished: MonoBehaviour {
        private UnityEvent _onAnimationFinished = new UnityEvent();

        /// <summary>
        /// Add listner to this event receiver
        /// </summary>
        /// <param name="action"></param>
        public void Subscribe(UnityAction action) {
            _onAnimationFinished.AddListener(action);
        }

        /// <summary>
        /// Remove listner to this event receiver
        /// </summary>
        /// <param name="action"></param>
        public void UnSubscribe(UnityAction action) {
            _onAnimationFinished.RemoveListener(action);
        }

        /// <summary>
        /// Call this from animation        
        /// </summary>
        /// <remarks>
        /// Assign only from Unity animation inspector
        /// </remarks>
        private void AnimationFinished() {
            _onAnimationFinished.Invoke();
        }
    }
}
