////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Mix2App.UI {
    /// <summary>
    /// Base class for custom Windows
    /// </summary>
    public class UIWindow: UIElement {
        [SerializeField] private Button CloseButton = null;

        private UnityEvent CloseWindowAction = new UnityEvent();

        /// <summary>
        /// Add close window action.
        /// </summary>
        /// <param name="action">Callback function</param>
        /// <returns>self</returns>
        public UIWindow AddCloseAction(UnityAction action) {
            CloseWindowAction.AddListener(action);
            return this;
        }

        /// <summary>
        /// Close and destroy this window.
        /// </summary>
        public void Close() {
            if (CloseWindowAction != null)
                CloseWindowAction.Invoke();
            GameObject.Destroy(this.gameObject);
        }

        protected const int SE_Tap = 11;
        protected const int SE_Choice = 12;
        protected const int SE_OK = 13;
        protected const int SE_Cancel = 17;

        protected static void PlaySE(int se_num) {
            Lib.ManagerObject.instance.sound.playSe(se_num);
        }

        protected static void PlayTapSE() {
            Lib.ManagerObject.instance.sound.playSe(SE_Tap);
        }

        protected static void PlayCloseSE() {
            Lib.ManagerObject.instance.sound.playSe(SE_Cancel);
        }

        protected static void AddButtonTapListner(Button btn, UnityAction act) {
            btn.onClick.AddListener(PlayTapSE);
            btn.onClick.AddListener(act);
        }

        protected static void AddButtonSEListner(Button btn, int se_num, UnityAction act) {
            btn.onClick.AddListener(() => { PlaySE(se_num); });
            btn.onClick.AddListener(act);
        }

        /// <summary>
        /// Make some settings, before window show
        /// </summary>
        public virtual void Setup() {
            CloseButton.onClick.AddListener(Close);
            CloseButton.onClick.AddListener(PlayCloseSE);
        }
    }
}
