using UnityEngine;
using UnityEngine.Events;
using Mix2App.MiniGame1.UI;
using Mix2App.UI;

namespace Mix2App.MiniGame1 {
    public class UICore: MonoBehaviour {
        #region Events
        private UnityEvent GameAbortEvent = new UnityEvent();
        private UnityEvent GamePauseEvent= new UnityEvent();
        private UnityEvent GameResumeEvent = new UnityEvent();
        private UnityEvent GameStartEvent = new UnityEvent();

        private UnityEvent GameStartAnimationEvent = new UnityEvent();
        private UnityEvent GetBoxAnimationEvent = new UnityEvent();

        public void AddGameAbortListener(UnityAction listener) {
            GameAbortEvent.AddListener(listener);
        }

        public void AddGamePauseListener(UnityAction listener) {
            GamePauseEvent.AddListener(listener);
        }

        public void AddGameResumeListener(UnityAction listener) {
            GameResumeEvent.AddListener(listener);
        }

        public void AddGameStartListener(UnityAction listener) {
            GameStartEvent.AddListener(listener);
        }

        public void AddGameStartAnimationListener(UnityAction listener) {
            GameStartAnimationEvent.AddListener(listener);
        }

        public void AddGetBoxAnimationListener(UnityAction listener) {
            GetBoxAnimationEvent.AddListener(listener);
        }
        #endregion
        
        public void Config() {
            SelfGameUI.AddBackAction(()=> {
                SelfGameUI.Hide();
                GameAbortEvent.Invoke();
                StartTitleMenu();
            });
            SelfGameUI.Hide();

            StartTitleMenu();
        }

        public void DisplayScore(int score) {
            SelfGameUI.SetScoreDisplay(score);
        }

        public void DisplayTime(int time) {
            SelfGameUI.SetTimeDisplay(time);
        }


        [SerializeField] private TitleMenu TitleMenuPrefab;
        [SerializeField] private AnimationInsertMenu GameStartAnimationPrefab;
        [SerializeField] private AnimationInsertMenu GameFinishAnimationPrefab;
        [SerializeField] private GameResultWindow GameResultWindowPrefab;

        [SerializeField] private GameUI SelfGameUI;

        public void ShowFinalScore(int score) {
            SelfGameUI.Hide();
            UIManager.ShowModal(GameFinishAnimationPrefab)
                .AddEndAnimationAction(()=> {
                    UIManager.ShowModal(GameResultWindowPrefab)
                        .SetScore(score)
                        .AddCloseAction(()=> {
                            StartTitleMenu();
                        });
                });
        }

        private void StartTitleMenu() {
            UIManager.ShowModal(TitleMenuPrefab).
                AddStartAction(()=> {
                    GameStartAnimationEvent.Invoke();
                    UIManager.ShowModal(GameStartAnimationPrefab)
                    .AddEndAnimationAction(() => {
                        SelfGameUI.Show();
                        GameStartEvent.Invoke();
                    });
                });
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