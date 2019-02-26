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
using Mix2App.Lib;
using Mix2App.Lib.Events;
using Mix2App.Lib.Model;

namespace Mix2App.Profile {
    /// <summary>
    /// Window profile for user self.
    /// </summary>
    public class SelfProfileWindow: ProfileWindow {
        [Header("Setup profile")]
        [SerializeField, Required] private ProfileSetupWindow ProfileSetupWindowPrefab;
        [SerializeField, Required] private Button SetupProfileButton;

        [Header("Growth Record")]
        [SerializeField, Required] private GrowthRecordWindow GrowthRecordWindowPrefab;
        [SerializeField, Required] private Button ShowGrowthRecordButton;

        [Header("Friends")]
        [SerializeField, Required] private FriendListWindow FriendListWindowPrefab;
        [SerializeField, Required] private Button ShowFriendListButton;
        
        [Header("User ID")]
        [SerializeField, Required] private GameObject UserIDBalloon1;
        [SerializeField, Required] private GameObject UserIDBalloon2;
        [SerializeField, Required] private Text UserIDText;
        [SerializeField, Required] private Button UserIDButton;


        private void SetupProfile() {
            ProfileSetupWindow wnd = UIManager.ShowModal(ProfileSetupWindowPrefab)
                .SetupUserData(UserData);

            wnd.AddCloseAction(() => {
                // dont overwrite directly
                //UserData.avatar = wnd.GetAvatar();
                UserInfoElementPrefab.SetupProfile(UserData);
                UserInfoElementPrefab.SetupPrefecture(wnd.GetPrefecture());
            });
        }
        
        private void ShowGrowthRecord() {
            if (UserData.calbums!=null)
                UIManager.ShowModal(GrowthRecordWindowPrefab)
                    .SetupCharacters(UserData.calbums.ToArray());
        }
        
        private void ShowFriendlist() {
            if (UserData.profiles != null)
                UIManager.ShowModal(FriendListWindowPrefab)
                    .SetupFriendList(UserData.profiles);
        }

        public override void Setup() {
            base.Setup();
            
            AddButtonTapListner(SetupProfileButton, SetupProfile);
            AddButtonTapListner(ShowGrowthRecordButton, ShowGrowthRecord);
            AddButtonTapListner(ShowFriendListButton, ShowFriendlist);

            AddButtonTapListner(UserIDButton, () => {
                if (UserIDBalloon1.activeSelf&&!UserIDBalloon2.activeSelf) 
                {
                    UserIDBalloon1.SetActive(false);
                    UserIDBalloon2.SetActive(true);
                    UserIDText.text = UserData.code;
                } 
                else if (!UserIDBalloon1.activeSelf&&UserIDBalloon2.activeSelf)
                {
                    UserIDBalloon1.SetActive(true);
                    UserIDBalloon2.SetActive(false);
                }
            });
        }

        public override ProfileWindow SetupUserData(User user_data) {
            ProfileWindow wnd = base.SetupUserData(user_data);

            UserIDBalloon1.SetActive((user_data.code!=null&&user_data.code!=""));
            UserIDBalloon2.SetActive(false);
            
            return wnd;
        }
    }
}
