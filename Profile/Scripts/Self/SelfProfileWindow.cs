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
using System.Collections.Generic;

namespace Mix2App.Profile {
    /// <summary>
    /// Window profile for user self.
    /// </summary>
    public class SelfProfileWindow: ProfileWindow {
        [Header("Setup profile")]
        [SerializeField, Required] private ProfileSetupWindow ProfileSetupWindowPrefab = null;
        [SerializeField, Required] private Button SetupProfileButton = null;

        [Header("Growth Record")]
        [SerializeField, Required] private GrowthRecordWindow GrowthRecordWindowPrefab = null;
        [SerializeField, Required] private Button ShowGrowthRecordButton = null;

        [Header("Friends")]
        [SerializeField, Required] private FriendListWindow FriendListWindowPrefab = null;
        [SerializeField, Required] private Button ShowFriendListButton = null;
        
        [Header("User ID")]
        [SerializeField, Required] private GameObject UserIDBalloon1 = null;
        [SerializeField, Required] private GameObject UserIDBalloon2 = null;
        [SerializeField, Required] private Text UserIDText = null;
        [SerializeField, Required] private Button UserIDButton = null;

        [Header("Object")]
        [SerializeField] private GameObject BaseObj = null;

        private void SetupProfile() {
            // プロフィールのアチーブ情報をクリアする
            UIFunction.ProfileAchieveClear();

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

            UserIDButton.onClick.AddListener(UserIDButtonClick);
/*
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
*/            
        }

        void UserIDButtonClick()
        {
            if (UserIDBalloon1.activeSelf && !UserIDBalloon2.activeSelf)
            {
                UserIDBalloon1.SetActive(false);
                UserIDBalloon2.SetActive(true);
                UserIDText.text = UserData.code;
                Lib.ManagerObject.instance.sound.playSe(11);
            }
            else if (!UserIDBalloon1.activeSelf && UserIDBalloon2.activeSelf)
            {
                UserIDBalloon1.SetActive(true);
                UserIDBalloon2.SetActive(false);
                Lib.ManagerObject.instance.sound.playSe(11);
            }
        }

        public override ProfileWindow SetupUserData(User user_data) {
            ProfileWindow wnd = base.SetupUserData(user_data);

            UserIDBalloon1.SetActive((user_data.code!=null&&user_data.code!=""));
            UserIDBalloon2.SetActive(false);

            if (UIFunction.TutorialFlagGet())
            {
                UIFunction.ButtonClickModeChenage(BaseObj.transform.Find("CLIENT/Button rect/SetupProfileButton").GetComponent<Button>(), false);
                UIFunction.ButtonClickModeChenage(BaseObj.transform.Find("CLIENT/Button rect/Buttons/FamilyTreeButton").GetComponent<Button>(), false);
                UIFunction.ButtonClickModeChenage(BaseObj.transform.Find("CLIENT/Button rect/Buttons/GrowthRecordButton").GetComponent<Button>(), false);
                UIFunction.ButtonClickModeChenage(BaseObj.transform.Find("CLIENT/Button rect/Buttons/ProfileViewButton").GetComponent<Button>(), false);
                UIFunction.ButtonClickModeChenage(BaseObj.transform.Find("BackButton").GetComponent<Button>(), false);

                UIFunction.ButtonClickModeChenage(GameObject.Find("kuchiguse_btn").GetComponent<Button>(), false);
                UIFunction.ButtonClickModeChenage(BaseObj.transform.Find("CLIENT/id_btn").GetComponent<Button>(), false);

            }


            return wnd;
        }


        private void Update()
        {
            if (UIFunction.TutorialFlagGet())
            {
                if(UIFunction.TutorialCountGet() == UIFunction.TUTORIAL_COUNTER.ProposeButtonTrueStart)
                {
                    UIFunction.TutorialCountSet(UIFunction.TUTORIAL_COUNTER.ProposeButtonTrueEnd);
                    UIFunction.ButtonClickModeChenage(BaseObj.transform.Find("BackButton").GetComponent<Button>(), true);
                }
            }

            if (UIFunction.ProfileAchieveFlagGet())
            {
                UIFunction.ProfileAchieveFlagSet(false);

                List<AchieveData> achievesMargeData = UIFunction.ProfileAchieveMargeDataGet();

                if ((achievesMargeData != null) && (achievesMargeData.Count != 0))
                {
                    //達成アチーブがある場合は、アチーブ成功画面を呼び出す
                    int CameraDepth = (int)(GameObject.Find("Main Camera").transform.GetComponent<Camera>().depth + 1);
                    ManagerObject.instance.view.add(SceneLabel.ACHIEVE_CLEAR,
                            achievesMargeData,
                            CameraDepth);
                }

            }


        }

    }
}
