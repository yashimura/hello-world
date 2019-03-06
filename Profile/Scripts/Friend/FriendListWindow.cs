////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using Mix2App.UI;
using UnityEngine;
using Mix2App.Lib.Model;
using Mix2App.UI.Dialogs;
using Mix2App.Profile.Elements;
using System.Collections.Generic;
using Mix2App.Lib.Events;

namespace Mix2App.Profile {
    /// <summary>
    /// This window show some list with button-links to profiles
    /// </summary>
    public class FriendListWindow: ElementListDialog<UserDataButton, ProfileBookData> {
        [SerializeField, Required] private FriendProfileWindow FriendProfileWindowPrefab = null;

        protected override void ElementAdded(UserDataButton element, int element_number) {
            base.ElementAdded(element, element_number);
            AddButtonTapListner(element.ControlComponent, ()=> {
                UIManager.ShowModal(FriendProfileWindowPrefab)
                        .AddFriendRemoveAction(()=> {
                            GameCall call = new GameCall(CallLabel.DELETE_PROFILE, element.pbookData);
                            Lib.ManagerObject.instance.connect.send(call);

                            GameObject.Destroy(element.gameObject);
                            profbookList.Remove(element.pbookData);
                            Elements.Remove(element);
                        })
                        .SetupUserData(element.pbookData.user);
            });
        }

        private List<ProfileBookData> profbookList;

        /// <summary>
        /// Assign here user data list.        
        /// </summary>
        /// <param name="user_list"></param>
        /// <returns></returns>
        public FriendListWindow SetupFriendList(List<ProfileBookData> user_list) {
            profbookList = user_list;
            AddElements(profbookList.ToArray());
            return this;
        }
    }
}
