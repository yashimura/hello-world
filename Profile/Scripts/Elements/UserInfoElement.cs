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
using Mix2App.Lib.Events;

namespace Mix2App.Profile {
    /// <summary>
    /// Block of user-info elements:
    /// Avatar, UserName, Characters, CharaName, Prefecture
    /// </summary>
    public class UserInfoElement : UIElement
    {
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


        [SerializeField] private GameObject IineObj = null;
        [SerializeField] private Sprite[] sexImage = null;
        [SerializeField] private string ProfileMode = "";



        private string KuchiguseText_Data;
        private User userData;

        /// <summary>
        /// Update user data
        /// </summary>
        /// <param name="user"></param>
        public void SetupProfile(User user)
        {
            AvatarPrefab.SetupAvatar(user.avatar);
            UserNameText.text = UIFunction.UserNicknameRetInsert(UIFunction.UserNicknameChange(user.nickname));

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

            if (IineObj == null)
            {
                return;
            }

            userData = user;

            if (user.chara1.IsTamgo)
            {
                // 卵の時は、表示位置を少し右にずらす
                Vector3 _pos = TamaNameText.gameObject.transform.localPosition;
                _pos.x += 40.0f;
                TamaNameText.gameObject.transform.localPosition = _pos;
            }


            IineObj.transform.Find("seibetsu").gameObject.SetActive(!user.chara1.IsTamgo);                              // 卵の時は性別を表示しない
            IineObj.transform.Find("seibetsu").gameObject.GetComponent<Image>().sprite = sexImage[user.chara1.sex];     // 性別
            IineObj.transform.Find("9999").gameObject.GetComponent<Text>().text = user.like.ToString();                 // いいねしてもらった数
            IineObj.transform.Find("99999").gameObject.GetComponent<Text>().text = user.likeTotal.ToString();           // いいねしてもらった総数

            if ((ProfileMode != "Town") || (user.likeRemain == 0))
            {
                // ボタンは表示しない
                IineObj.transform.Find("iine_off").gameObject.SetActive(true);
                IineObj.transform.Find("iine_button").gameObject.SetActive(false);
            }
            else
            {
                // ボタンを表示する
                IineObj.transform.Find("iine_off").gameObject.SetActive(false);
                IineObj.transform.Find("iine_button").gameObject.SetActive(true);
                IineObj.transform.Find("iine_button/Button_iine").gameObject.GetComponent<Button>().onClick.AddListener(IineButtonClick);
            }

            if (UIFunction.TutorialFlagGet())
            {
                // チュートリアル中はいいねボタンを無効化
                UIFunction.ButtonClickModeChenage(IineObj.transform.Find("iine_button/Button_iine").GetComponent<Button>(),false);
            }
        }

        private void IineButtonClick()
        {
            Lib.ManagerObject.instance.sound.playSe(13);

            // いいねが押されたのでライブラリに状況を送信する
            GameCall call = new GameCall(CallLabel.SEND_LIKE, userData);
            call.AddListener(SendLike);
            Lib.ManagerObject.instance.connect.send(call);

            // タウンプロファイルから抜ける。（基本いいねボタンは、タウンプロファイルにしかない予定）
            Town.TownSceneCore.SceneClose();

            /*
                        // プロファイルから抜ける場合
                        Lib.ManagerObject.instance.view.back();
                        // タウンプロファイルから抜ける場合
                        Town.TownSceneCore.SceneClose();
            */
        }

        void SendLike(bool success, object data)
        {

        }


        /// <summary>
        /// Setup prefecture number
        /// </summary>
        /// <param name="place"></param>
        public void SetupPrefecture(int place)
        {
            PrefectureElementPrefab.SetPrefecture(place);
        }

        private void ClearTamaName()
        {
            TamaNameText.text = "";
        }
        private void SetupCharacters(TamaChara ch1, TamaChara ch2)
        {
            if (ch1 != null && ch2 != null)
            {
                Chara0.gameObject.SetActive(false);
                Chara1.gameObject.SetActive(true);
                Chara2.gameObject.SetActive(true);

                Chara1.init(ch1);
                Chara2.init(ch2);

                TamaNameText.text = ch1.cname + " と\n" + ch2.cname;
            }
            else if (ch1 != null)
            {
                Chara0.gameObject.SetActive(true);
                Chara1.gameObject.SetActive(false);
                Chara2.gameObject.SetActive(false);

                Chara0.init(ch1);

                TamaNameText.text = ch1.cname;
            }
            else if (ch2 != null)
            {
                Chara0.gameObject.SetActive(true);
                Chara1.gameObject.SetActive(false);
                Chara2.gameObject.SetActive(false);

                Chara0.init(ch2);

                TamaNameText.text = ch2.cname;
            }

            // ballon set false when wstyle = ""
            KuchiguseBalloon1.SetActive((ch1.wstyle != null && ch1.wstyle.Length > 0));
            KuchiguseBalloon2.SetActive(false);
            KuchiguseText_Data = ch1.wstyle;
        }

        /// <summary>
        /// Assign this to active Kuchiguse button
        /// </summary>
        public void KuchiguseClick()
        {
            if (KuchiguseBalloon1.activeSelf && !KuchiguseBalloon2.activeSelf)
            {
                KuchiguseBalloon1.SetActive(false);
                KuchiguseBalloon2.SetActive(true);
                KuchiguseText.text = KuchiguseText_Data;
                Lib.ManagerObject.instance.sound.playSe(11);
            }
            else if (!KuchiguseBalloon1.activeSelf && KuchiguseBalloon2.activeSelf)
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
        public void SetupUserData(User data)
        {
            UserData = data;

            SetupProfile(UserData);
            ClearTamaName();
            SetupCharacters(UserData.chara1, UserData.chara2);
            if (UserData.pet != null)
            {
                try
                {
                    Pet1.gameObject.SetActive(true);
                    Pet1.init(UserData.pet);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.Message, this);
                }
            }
            else
                Pet1.gameObject.SetActive(false);

            // if (data.utype == UserType.GEST)
            //     KuchiguseBalloon.SetActive(false);

            // 日本は 0-99 を割り当て。ひみつは 0（内部コード）
            // 内部コードは、ひみつ=0、ほっかいどう=1　....　かいがい=48
            // 画像の並びは、ほっかいどう=0　....　かいがい=47、ひみつ=48（49-99は、ダミーとして ひみつ とする）
            //
            // 北米は 100-199 を割り当て。SECRETは 100(内部コード)
            // 内部コードは、Secret=100、Alabama=101、　....　Other=165
            // 画像の並びは、Secret=100、Alabama=101、　....　Other=165（166-199は、ダミーとして Secret とする）
            //
            // ※画像の並び最後の要素（内部コードの最終番号 + 1）は、各国の「ひみつ」に当たるものを配置する
            //
            //内部コードを画像の並び順に変換し、出身地画像を設定する
            int listindex = data.bplace;

            if (listindex < 100)
            {
                //日本のユーザー 0-99
                listindex = data.bplace - 1;  // 1 減らす
                if (listindex < 0) listindex = 48;    //ひみつ とする
            }
            else if (listindex < 200)
            {
                //北米のユーザー 100-199
                //listindex = data.bplace;  //そのまま
            }
            else
            {
                //その他（不正）
                listindex = 48;    //ひみつ とする
            }
            PrefectureElementPrefab.SetPrefecture(listindex);
        }

        /// <summary>
        /// If some user data changed - update fields
        /// </summary>
        public void UpdateInfo()
        {
            SetupUserData(UserData);
        }

        private void Update()
        {
            if (UIFunction.TutorialFlagGet())
            {
                if(UIFunction.TutorialCountGet() == UIFunction.TUTORIAL_COUNTER.IineButtonTrueStart)
                {
                    UIFunction.TutorialCountSet(UIFunction.TUTORIAL_COUNTER.IineButtonTrueEnd);

                    // ボタン制御を再開
                    UIFunction.ButtonClickModeChenage(IineObj.transform.Find("iine_button/Button_iine").gameObject.GetComponent<Button>(), true);
                }
            }
        }
    }
}
