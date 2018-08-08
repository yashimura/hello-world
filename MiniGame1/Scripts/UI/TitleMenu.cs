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
    public class TitleMenu: UIWindow {
        [SerializeField] private Button StartButton;
        [SerializeField] private Button HelpButton;

        [SerializeField] private EventReceiver_AnimationFinished _EndAnimationFinished;

        [SerializeField] private Animator MainWindowAnimator;

        private UnityEvent StartEvent = new UnityEvent();

        public TitleMenu AddStartAction(UnityAction action) {
            StartEvent.AddListener(action);
            return this;
        }

        public TitleMenu AddHelpAction(UnityAction action) {
            HelpButton.onClick.AddListener(action);
            return this;
        }

        public override void Setup() {
            base.Setup();

            StartButton.onClick.AddListener(() => {
                MainWindowAnimator.SetBool("Exit", true);
                _EndAnimationFinished.Subscribe(() => {                    
                    this.Close();
                    StartEvent.Invoke();
                });
            });
        }

#if UNITY_EDITOR
        public override void DebugCheck() {
            base.DebugCheck();

            CheckFieldAssigned(StartButton, HelpButton, _EndAnimationFinished, MainWindowAnimator);
        }
#endif
    }
}
