using Mix2App.UI;
using UnityEngine;
using UnityEngine.Events;
using Mix2App.UI.Events;

namespace Mix2App.MiniGame1.UI {
    public class AnimationInsertMenu:UIWindow {
        [SerializeField] private EventReceiver_AnimationFinished _EndAnimationFinished;

        public AnimationInsertMenu AddEndAnimationAction(UnityAction action) {
            _EndAnimationFinished.Subscribe(action);
            return this;
        }

        public override void Setup() {
            base.Setup();

            AddEndAnimationAction(() => { Close(); });
        }
    }
}
