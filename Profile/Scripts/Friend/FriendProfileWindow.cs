////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;
using UnityEngine.UI;
using Mix2App.UI;
using Mix2App.UI.Dialogs;
using UnityEngine.Events;

namespace Mix2App.Profile {
    /// <summary>
    /// Window for friend profile.
    /// User can edit from this window.
    /// </summary>
    public class FriendProfileWindow: ProfileWindow {
        private UnityEvent FriendRemoveEvent = new UnityEvent();

        [Header("Dialogs")]
        [Tooltip("Remove friend confirm dialog")]
        [SerializeField, Required] private ConfirmDialog RemoveFriendConfirmPrefab = null;
        [SerializeField, Required] private Button RemoveFriendButton = null;

        public FriendProfileWindow AddFriendRemoveAction(UnityAction action) {
            FriendRemoveEvent.AddListener(action);
            return this;
        }
        
        public void RemoveFriend() {
            UIManager.ShowModal(RemoveFriendConfirmPrefab,false).AddOKAction(() => {
                FriendRemoveEvent.Invoke();
                Close(); // Close this window after delete friend
            });
        }

        public override void Setup() {
            base.Setup();

            AddButtonTapListner(RemoveFriendButton, RemoveFriend);
        }
    }
}
