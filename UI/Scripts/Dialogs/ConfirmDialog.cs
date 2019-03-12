////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mix2App.UI.Dialogs {
    /// <summary>
    /// Simple confirm dialog
    /// </summary>
    [UnityEngine.AddComponentMenu("UI/Windows/ConfirmDialog")]
    public class ConfirmDialog: UIWindow {
        [SerializeField, Required] private Button OKButton = null;

        private UnityEvent OKWindowAction = new UnityEvent();
        
        /// <summary>
        /// Call when you click OK
        /// </summary>
        public void OK() {
            if (OKWindowAction != null)
                OKWindowAction.Invoke();
            Close();
        }

        /// <param name="action">Callback function</param>
        /// <returns>self</returns>
        public ConfirmDialog AddOKAction(UnityAction action) {
            OKWindowAction.AddListener(action);
            return this;
        }

        /// <param name="action">Callback function</param>
        /// <returns>self</returns>
        public ConfirmDialog AddCancelAction(UnityAction action) {
            AddCloseAction(action);
            return this;
        }

        public override void Setup() {
            base.Setup();
            AddButtonSEListner(OKButton, SE_OK, OK);
        }
    }
}
