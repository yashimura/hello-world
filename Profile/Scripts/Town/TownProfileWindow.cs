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
using Mix2App.Lib;
using Mix2App.Lib.Events;
using Mix2App.Lib.Model;
using Mix2App.Lib.View;
using System.Collections;

namespace Mix2App.Profile.Town {
    /// <summary>
    /// Window for profile from town.
    /// </summary>
    public class TownProfileWindow: ProfileWindow {
        [Header("Dialogs")]
        [Tooltip("Window to select user character, if user have two characters")]
        [SerializeField, Required] private CharacterSelectDialog CharacterSelectDialogPrefab;

        [Tooltip("Window to select OTHER USER character, if user have two characters")]
        [SerializeField, Required] private CharacterSelectDialog SelfCharacterSelectDialogPrefab;

        [Tooltip("Dialog to confirm propose")]
        [SerializeField, Required] private ConfirmDialog ProposeConfirm;

        [Header("Buttons")]
        [SerializeField, Required] private Button ProposeButton = null;
        [SerializeField, Required] private Button ProfileExchangeButton = null;
        [SerializeField, Required] private ConfirmDialog ProfileExchangeDialog = null;

        [SerializeField, Required] private Button ProfileUpdateButton = null;
        [SerializeField, Required] private ConfirmDialog ProfileUpdateDialog = null;


        [Header("User ID")]
        [SerializeField, Required] private GameObject UserIDBalloon1 = null;
        [SerializeField, Required] private GameObject UserIDBalloon2 = null;
        [SerializeField, Required] private Text UserIDText = null;
        [SerializeField, Required] private Button UserIDButton = null;

		[Header("Object")]
		[SerializeField] private GameObject BaseObj = null;
	


		private GameObject[] ProposeWindow;

        private User mUserPc;
		private User mUserNpc;
		private int mCharaPcNum;
		private int mCharaNpcNum;



		private void ProposeStep3(int _mCharaPcNum, int _mCharaNpcNum) {
			ProposeWindow [2].transform.localPosition = new Vector3 (5000, 0, 0);
			ProposeWindow [0].transform.localPosition = new Vector3 (0, 0, 0);				// 最終確認画面

			mCharaPcNum = _mCharaPcNum;
			mCharaNpcNum = _mCharaNpcNum;
        }

		private void ProposeStep2(int _mCharaPcNum) {
			ProposeWindow [1].transform.localPosition = new Vector3 (5000, 0, 0);

			mCharaPcNum = _mCharaPcNum;
			mCharaNpcNum = 0;

			if (mUserNpc.chara2 != null) {
				ProposeWindow [2].transform.localPosition = new Vector3 (0, 0, 0);			// 相手の双子選択画面
			} else {
				ProposeStep3 (mCharaPcNum, 0);
			}
        }

        private void Propose() {
			BaseObj.transform.localPosition = new Vector3 (5000, 0, 0);

			mCharaPcNum = 0;
			mCharaNpcNum = 0;

			if (mUserPc.chara2 != null) {
				ProposeWindow [1].transform.localPosition = new Vector3 (0, 0, 0);			// 自分の双子選択画面
			} else {
				ProposeStep2 (0);
			}
        }

        public override void Setup() {
            base.Setup();

            AddButtonTapListner(ProposeButton, Propose);        

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

			{	// プロポーズボタンを表示するかどうかをチェックする
				bool _proposeButtonActiveFlag = UIManager.proposeFlagGet();

                mUserPc = ManagerObject.instance.player;
				mUserNpc = user_data;
				mCharaPcNum = 0;
				mCharaNpcNum = 0;

/*
				if ((mUserPc.utype != UserType.MIX2) ||						// 自分がみーつユーザーでない
					(mUserPc.chara1.grow < 4) ||							// 自分がフレンド期になっていない
					(mUserNpc.utype != UserType.MIX2) ||					// 相手がみーつユーザーでない
					(mUserNpc.chara1.grow < 4)	||							// 相手がフレンド期になっていない
					(mUserPc.chara1.sex == mUserNpc.chara1.sex)				// 同性
				) {
					_proposeButtonActiveFlag = false;						// プロポーズボタンを表示しない
				}
*/
				if(_proposeButtonActiveFlag){
					ProposeButton.gameObject.SetActive (true);				// プロポーズボタンの表示
					ProposeWindow = UIManager.proposeWindowGet ();			// プロポーズ確認画面の登録
					StartCoroutine (TamagochiCharaSet ());					// プロポーズ確認画面のたまごっちの登録
					ProposeButtonSet ();									// プロポーズ確認画面のボタンの有効化
				}
			}

            //他者のユーザーコードは表示しない
            UserIDBalloon1.SetActive(false);
            UserIDBalloon2.SetActive(false);
            
            if (Lib.ManagerObject.instance.player.containsProfiles(user_data.code))
            {
                //close
                ProfileUpdateButton.gameObject.SetActive(true);
                ProfileExchangeButton.gameObject.SetActive(false);
                AddButtonTapListner(ProfileUpdateButton, ()=> {
                    UIManager.ShowModal(ProfileUpdateDialog, false).AddOKAction(() => {
                        GameEventHandler.OnRemoveSceneEvent += onremove;
                        Lib.ManagerObject.instance.view.add(
                            // label, palyer, user, kind, depth
                            SceneLabel.PROFILE_EXCHANGE, Lib.ManagerObject.instance.player, UserData, 1, 2
                        );
                    });
                });
            }
            else
            {
                //close
                if (user_data.ukind!=UserKind.NPC)
                {
                    ProfileUpdateButton.gameObject.SetActive(false);
                    ProfileExchangeButton.gameObject.SetActive(true);
                }
                else
                {
                    ProfileUpdateButton.gameObject.SetActive(false);
                    ProfileExchangeButton.gameObject.SetActive(false);
                }
                AddButtonTapListner(ProfileExchangeButton, ()=> {
                    UIManager.ShowModal(ProfileExchangeDialog, false).AddOKAction(() => {
                        GameEventHandler.OnRemoveSceneEvent += onremove;
                        Lib.ManagerObject.instance.view.add(
                            // label, palyer, user, kind, depth
                            SceneLabel.PROFILE_EXCHANGE, Lib.ManagerObject.instance.player, UserData, 0, 2
                        );
                    });
                });
            }



			if ((!ProfileUpdateButton.gameObject.activeSelf) && (!ProfileExchangeButton.gameObject.activeSelf)) {
				// 「交換」「更新」ボタンが非表示の時、プロボーズボタンの表示位置を「交換」「更新」の位置に変更する
				ProposeButton.gameObject.transform.localPosition = ProfileUpdateButton.gameObject.transform.localPosition;
			}



			return wnd;
        }

        void onremove(string label)
        {
            if (label==SceneLabel.PROFILE_EXCHANGE)
            {
                GameEventHandler.OnRemoveSceneEvent -= onremove;
                ManagerObject.instance.view.delete(SceneLabel.PROFILE_EXCHANGE);
                SetupUserData(UserData);// update view
            }
        }



		IEnumerator TamagochiCharaSet(){
			CharaBehaviour _cb;

			// プレイヤーたまごっちを設定する
			_cb = ProposeWindow[1].transform.Find("Button_proposeL/CharaImg").gameObject.GetComponent<CharaBehaviour> ();
			yield return _cb.init (mUserPc.chara1);
			ProposeWindow [1].transform.Find ("nameflame_l/Text").gameObject.GetComponent<Text> ().text = mUserPc.chara1.cname;
			if (mUserPc.chara2 != null) {
				_cb = ProposeWindow[1].transform.Find("Button_proposeR/CharaImg").gameObject.GetComponent<CharaBehaviour> ();
				yield return _cb.init (mUserPc.chara2);
				ProposeWindow [1].transform.Find ("nameflame_r/Text").gameObject.GetComponent<Text> ().text = mUserPc.chara2.cname;
			}

			// プロポーズ相手のたまごっちを設定する
			_cb = ProposeWindow[2].transform.Find("Button_proposeL/CharaImg").gameObject.GetComponent<CharaBehaviour> ();
			yield return _cb.init (mUserNpc.chara1);
			ProposeWindow [2].transform.Find ("nameflame_l/Text").gameObject.GetComponent<Text> ().text = mUserNpc.chara1.cname;
			if (mUserNpc.chara2 != null) {
				_cb = ProposeWindow[2].transform.Find("Button_proposeR/CharaImg").gameObject.GetComponent<CharaBehaviour> ();
				yield return _cb.init (mUserNpc.chara2);
				ProposeWindow [2].transform.Find ("nameflame_r/Text").gameObject.GetComponent<Text> ().text = mUserNpc.chara2.cname;
			}
		}

		private void ProposeButtonSet(){
			// 最終確認画面
			ProposeWindow [0].transform.Find ("Button_blue_iie").gameObject.GetComponent<Button> ().onClick.AddListener (Propose0Button_iie);
			ProposeWindow [0].transform.Find ("Button_red_hai").gameObject.GetComponent<Button> ().onClick.AddListener (Propose0Button_hai);

			// 自分の双子の選択画面
			ProposeWindow [1].transform.Find ("Button_proposeL").gameObject.GetComponent<Button> ().onClick.AddListener (Propose1Button_PC1);
			ProposeWindow [1].transform.Find ("Button_proposeR").gameObject.GetComponent<Button> ().onClick.AddListener (Propose1Button_PC2);
			ProposeWindow [1].transform.Find ("Button_blue_tojiru").gameObject.GetComponent<Button> ().onClick.AddListener (Propose1Button_tojiru);

			//相手の双子の選択画面
			ProposeWindow [2].transform.Find ("Button_proposeL").gameObject.GetComponent<Button> ().onClick.AddListener (Propose2Button_NPC1);
			ProposeWindow [2].transform.Find ("Button_proposeR").gameObject.GetComponent<Button> ().onClick.AddListener (Propose2Button_NPC2);
			ProposeWindow [2].transform.Find ("Button_blue_tojiru").gameObject.GetComponent<Button> ().onClick.AddListener (Propose2Button_tojiru);

			// 告白された状況の選択画面（未実装）
			ProposeWindow [3].transform.Find ("Button_red_iine").gameObject.GetComponent<Button> ().onClick.AddListener (Propose3Button_iine);
			ProposeWindow [3].transform.Find ("Button_blue_u-n").gameObject.GetComponent<Button> ().onClick.AddListener (Propose3Button_uun);
		}
		private void Propose0Button_iie(){
			ManagerObject.instance.sound.playSe (14);
			ProposeJobOff ();
		}
		private void Propose0Button_hai(){
            int CameraDepth = (int)(UIManager.cameraWindowGet().transform.GetComponent<Camera>().depth + 1);

            ManagerObject.instance.sound.playSe (13);

/*
			GameCall call = new GameCall(CallLabel.GET_PROPOSE, mUserPc, mCharaPcNum, mUserNpc, mCharaNpcNum);
			call.AddListener((bool b, object o) => {
                ManagerObject.instance.view.change(SceneLabel.PROPOSE,
//					b,									// プロポーズの成否
                    1,                                  // プロポーズをするので１（プロポーズされた場合は０）
                    mUserPc,                            // 自分のユーザー情報
                    mCharaPcNum,                        // 自分の兄弟のどちらでプロポーズしたか
                    mUserNpc,                           // 相手のユーザー情報
                    mCharaNpcNum,						// 相手の兄弟のどちらにプロポーズしたか
                    CameraDepth);
			});
			ManagerObject.instance.connect.send(call);
*/
            GameEventHandler.OnRemoveSceneEvent += ProposeDelete;
            ManagerObject.instance.view.add(SceneLabel.PROPOSE,
                    1,                                  // プロポーズをするので１（プロポーズされた場合は０）
                    mUserPc,                            // 自分のユーザー情報
                    mCharaPcNum,                        // 自分の兄弟のどちらでプロポーズしたか
                    mUserNpc,                           // 相手のユーザー情報
                    mCharaNpcNum,                       // 相手の兄弟のどちらにプロポーズしたか
                    CameraDepth);
        }
        private void Propose1Button_PC1(){
			ManagerObject.instance.sound.playSe (12);
			ProposeStep2(0);							// 自分のたまごっちのchara1を選択
		}
		private void Propose1Button_PC2(){
			ManagerObject.instance.sound.playSe (12);
			ProposeStep2(1);							// 自分のたまごっちのchara2を選択
		}
		private void Propose1Button_tojiru(){
			ManagerObject.instance.sound.playSe (17);
			ProposeJobOff ();
		}
		private void Propose2Button_NPC1(){
			ManagerObject.instance.sound.playSe (12);
			ProposeStep3(mCharaPcNum, 0);				// 相手のたまごっちのchara1を選択
		}
		private void Propose2Button_NPC2(){
			ManagerObject.instance.sound.playSe (12);
			ProposeStep3(mCharaPcNum, 1);				// 相手のたまごっちのchara2を選択
		}
		private void Propose2Button_tojiru(){
			ManagerObject.instance.sound.playSe (17);
			ProposeJobOff ();
		}
		private void Propose3Button_iine(){
			ManagerObject.instance.sound.playSe (13);
		}
		private void Propose3Button_uun(){
			ManagerObject.instance.sound.playSe (14);
		}

		private void ProposeJobOff(){
			ProposeWindow [0].transform.localPosition = new Vector3 (5000, 0, 0);
			ProposeWindow [1].transform.localPosition = new Vector3 (5000, 0, 0);
			ProposeWindow [2].transform.localPosition = new Vector3 (5000, 0, 0);
			BaseObj.transform.localPosition = new Vector3 (0, 0, 0);
		}

        void ProposeDelete(string label)
        {
            if (label == SceneLabel.PROPOSE)
            {
                GameEventHandler.OnRemoveSceneEvent -= ProposeDelete;
                ManagerObject.instance.view.delete(SceneLabel.PROPOSE);
                ProposeButton.gameObject.SetActive(false);              // プロポーズボタンの非表示
                ProposeJobOff();
            }
        }
    }
}

