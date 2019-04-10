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
using Mix2App.Profile.Elements;
using Mix2App.Lib.View;
using Mix2App.Lib.Model;

namespace Mix2App.Profile {
    /// <summary>
    /// Block of user-info elements:
    /// Avatar, UserName, Characters, CharaName, Prefecture
    /// </summary>
    public class UserInfoElement: UIElement {
        private User UserData;

        [Header("Avatar settings")]
        [SerializeField, Required] private AvatarElement AvatarPrefab = null;
        [SerializeField, Required] private Text UserNameText = null;
                
        [Header("Tama character settings")]
        [SerializeField, Required] private CharaBehaviour Chara0 = null;
        [SerializeField, Required] private CharaBehaviour Chara1 = null;
        [SerializeField, Required] private CharaBehaviour Chara2 = null;
        [SerializeField, Required] private Text TamaNameText = null;
        [SerializeField, Required] private PetBehaviour Pet1 = null;

        [Header("Kuchiguse")]
        [SerializeField, Required] private GameObject KuchiguseBalloon1 = null;
        [SerializeField, Required] private GameObject KuchiguseBalloon2 = null;
        [SerializeField, Required] private Text KuchiguseText = null;

        [Header("Prefecture setup")]
        [SerializeField, Required] private PrefectureElement PrefectureElementPrefab = null;

        private string KuchiguseText_Data;

        /// <summary>
        /// Update user data
        /// </summary>
        /// <param name="user"></param>
        public void SetupProfile(User user) {
            AvatarPrefab.SetupAvatar(user.avatar);
            UserNameText.text = user.nickname;
            /* fallback font で対応するので不要
            if (user.utype!=UserType.MIX2&&(user.enableBnIdLink||user.enableLineLink))
            {
                UserNameText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                UserNameText.resizeTextForBestFit = true;
                UserNameText.resizeTextMaxSize = 36;
            }
            else
            {
                UserNameText.font = Resources.Load<Font>("Fonts/tmgc_name");
                UserNameText.resizeTextForBestFit = false;
            }
            */
        }

        /// <summary>
        /// Setup prefecture number
        /// </summary>
        /// <param name="place"></param>
        public void SetupPrefecture(int place) {
            PrefectureElementPrefab.SetPrefecture(place);
        }
        
        private void ClearTamaName() {
            TamaNameText.text = "";
        }
        private void SetupCharacters(TamaChara ch1, TamaChara ch2) {
            if (ch1 != null && ch2 != null) {
                Chara0.gameObject.SetActive(false);
                Chara1.gameObject.SetActive(true);
                Chara2.gameObject.SetActive(true);

                Chara1.init(ch1);
                Chara2.init(ch2);

                TamaNameText.text = ch1.cname + " と " + ch2.cname;
            } else if (ch1 != null) {
                Chara0.gameObject.SetActive(true);
                Chara1.gameObject.SetActive(false);
                Chara2.gameObject.SetActive(false);

                Chara0.init(ch1);

                TamaNameText.text = ch1.cname;
            } else if (ch2 != null) {
                Chara0.gameObject.SetActive(true);
                Chara1.gameObject.SetActive(false);
                Chara2.gameObject.SetActive(false);

                Chara0.init(ch2);

                TamaNameText.text = ch2.cname;
            }

            // ballon set false when wstyle = ""
            KuchiguseBalloon1.SetActive((ch1.wstyle!=null&&ch1.wstyle.Length>0));
            KuchiguseBalloon2.SetActive(false);
            KuchiguseText_Data = ch1.wstyle;
        }
        
        /// <summary>
        /// Assign this to active Kuchiguse button
        /// </summary>
        public void KuchiguseClick() {
            if (KuchiguseBalloon1.activeSelf&&!KuchiguseBalloon2.activeSelf)
            {
                KuchiguseBalloon1.SetActive(false);
                KuchiguseBalloon2.SetActive(true);
                KuchiguseText.text = KuchiguseText_Data;
                Lib.ManagerObject.instance.sound.playSe(11);
            }
            else if (!KuchiguseBalloon1.activeSelf&&KuchiguseBalloon2.activeSelf)
            {
                KuchiguseBalloon1.SetActive(true);
                KuchiguseBalloon2.SetActive(false);
                Lib.ManagerObject.instance.sound.playSe(11);
            }
        }
        
        /// <summary>
        /// Sets user data.
        /// Also update all info fields
        /// </summary>
        /// <param name="data"></param>
        public void SetupUserData(User data) {
            UserData = data;

            SetupProfile(UserData);
            ClearTamaName();
            SetupCharacters(UserData.chara1, UserData.chara2);
            if (UserData.pet != null) {
                try {
                    Pet1.gameObject.SetActive(true);
                    Pet1.init(UserData.pet);
                } catch (System.Exception e) {
                    Debug.LogError(e.Message, this);
                }
            } else
                Pet1.gameObject.SetActive(false);

            // if (data.utype == UserType.GEST)
            //     KuchiguseBalloon.SetActive(false);

            // HOKKAIDO=1 SECRET=0
            int listindex = data.bplace-1;
            if (listindex<0) listindex = 48;
            PrefectureElementPrefab.SetPrefecture(listindex);
        }

        /// <summary>
        /// If some user data changed - update fields
        /// </summary>
        public void UpdateInfo() {
            SetupUserData(UserData);
        }
    }
}
