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
        [SerializeField, Required] private Button ProposeButton;
        [SerializeField, Required] private Button ProfileExchangeButton;
        [SerializeField, Required] private ConfirmDialog ProfileExchangeDialog;

        [SerializeField, Required] private Button ProfileUpdateButton;
        [SerializeField, Required] private ConfirmDialog ProfileUpdateDialog;


        [Header("User ID")]
        [SerializeField, Required] private GameObject UserIDBalloon1;
        [SerializeField, Required] private GameObject UserIDBalloon2;
        [SerializeField, Required] private Text UserIDText;
        [SerializeField, Required] private Button UserIDButton;

		[Header("Object")]
		[SerializeField] private GameObject BaseObj;
	
		private User mUserPc;


		private GameObject[] ProposeWindow;
		private User mUserPcHzn;
		private int mCharaPcNumHzn;
		private User mUserNpcHzn;
		private int mCharaNpcNumHzn;

        private void ProposeStep3(User usr1, User usr2, int s_chara_num, int o_chara_num) {
			ProposeWindow [2].transform.localPosition = new Vector3 (5000, 0, 0);
			ProposeWindow [0].transform.localPosition = new Vector3 (0, 0, 0);

			mUserPcHzn = usr1;
			mCharaPcNumHzn = s_chara_num;
			mUserNpcHzn = usr2;
			mCharaNpcNumHzn = o_chara_num;


/*            UIManager.ShowModal(ProposeConfirm).AddOKAction(() => {
                GameCall call = new GameCall(CallLabel.GET_PROPOSE, usr1, s_chara_num, usr2, o_chara_num);
                call.AddListener((bool b, object o) => {
                    if (b) {
                        ManagerObject.instance.view.change("Propose",
                            b,
                            0,
                            usr1,
                            s_chara_num,
                            usr2,
                            o_chara_num);
                    }
                });
                ManagerObject.instance.connect.send(call);
            });
*/
        }




        private void ProposeStep2(User usr, int s_chara_num) {
			ProposeWindow [1].transform.localPosition = new Vector3 (5000, 0, 0);

			mUserPcHzn = usr;
			mCharaPcNumHzn = s_chara_num;

			if (UserData.chara2 != null) {
				ProposeWindow [2].transform.localPosition = new Vector3 (0, 0, 0);
            } else 
                ProposeStep3(usr, UserData, s_chara_num, 0);
        }

        private void Propose() {
			BaseObj.transform.localPosition = new Vector3 (5000, 0, 0);
			if (mUserPc.chara2 != null) {
				ProposeWindow [1].transform.localPosition = new Vector3 (0, 0, 0);
            } else
                ProposeStep2(mUserPc, 0);
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


			if (false) {
				ProposeButton.gameObject.SetActive (true);

				mUserPc = ManagerObject.instance.player;
				ProposeWindow = UIManager.proposeWindowGet ();
				StartCoroutine (TamagochiCharaSet ());
				ProposeButtonSet ();
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
			yield return _cb.init (UserData.chara1);
			ProposeWindow [2].transform.Find ("nameflame_l/Text").gameObject.GetComponent<Text> ().text = UserData.chara1.cname;
			if (UserData.chara2 != null) {
				_cb = ProposeWindow[2].transform.Find("Button_proposeR/CharaImg").gameObject.GetComponent<CharaBehaviour> ();
				yield return _cb.init (UserData.chara2);
				ProposeWindow [2].transform.Find ("nameflame_r/Text").gameObject.GetComponent<Text> ().text = UserData.chara2.cname;
			}
		}

		private void ProposeButtonSet(){
			ProposeWindow[0].transform.Find("Button_blue_iie").gameObject.GetComponent<Button> ().onClick.AddListener (Propose0Button_iie);
			ProposeWindow[0].transform.Find("Button_red_hai").gameObject.GetComponent<Button> ().onClick.AddListener (Propose0Button_hai);

			ProposeWindow [1].transform.Find ("Button_proposeL").gameObject.GetComponent<Button> ().onClick.AddListener (Propose1Button_PC1);
			ProposeWindow [1].transform.Find ("Button_proposeR").gameObject.GetComponent<Button> ().onClick.AddListener (Propose1Button_PC2);
			ProposeWindow [1].transform.Find ("Button_blue_tojiru").gameObject.GetComponent<Button> ().onClick.AddListener (Propose1Button_tojiru);

			ProposeWindow [2].transform.Find ("Button_proposeL").gameObject.GetComponent<Button> ().onClick.AddListener (Propose2Button_NPC1);
			ProposeWindow [2].transform.Find ("Button_proposeR").gameObject.GetComponent<Button> ().onClick.AddListener (Propose2Button_NPC2);
			ProposeWindow [2].transform.Find ("Button_blue_tojiru").gameObject.GetComponent<Button> ().onClick.AddListener (Propose2Button_tojiru);

			ProposeWindow [3].transform.Find ("Button_red_iine").gameObject.GetComponent<Button> ().onClick.AddListener (Propose3Button_iine);
			ProposeWindow [3].transform.Find ("Button_blue_u-n").gameObject.GetComponent<Button> ().onClick.AddListener (Propose3Button_uun);
		}
		private void Propose0Button_iie(){
			ManagerObject.instance.sound.playSe (14);
			ProposeJobOff ();
		}
		private void Propose0Button_hai(){
			ManagerObject.instance.sound.playSe (13);

			GameCall call = new GameCall(CallLabel.GET_PROPOSE, mUserPcHzn, mCharaPcNumHzn, mUserNpcHzn, mCharaNpcNumHzn);
			call.AddListener((bool b, object o) => {
				if (b) {
					ManagerObject.instance.view.change("Propose",
						b,
						1,
						mUserPcHzn,
						mCharaPcNumHzn,
						mUserNpcHzn,
						mCharaNpcNumHzn);
				}
			});
			ManagerObject.instance.connect.send(call);
		}
		private void Propose1Button_PC1(){
			ManagerObject.instance.sound.playSe (12);
			ProposeStep2(mUserPc,0);
		}
		private void Propose1Button_PC2(){
			ManagerObject.instance.sound.playSe (12);
			ProposeStep2(mUserPc,1);
		}
		private void Propose1Button_tojiru(){
			ManagerObject.instance.sound.playSe (17);
			ProposeJobOff ();
		}
		private void Propose2Button_NPC1(){
			ManagerObject.instance.sound.playSe (12);
			ProposeStep3(mUserPcHzn, UserData, mCharaPcNumHzn, 0);
		}
		private void Propose2Button_NPC2(){
			ManagerObject.instance.sound.playSe (12);
			ProposeStep3(mUserPcHzn, UserData, mCharaPcNumHzn, 1);
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
    }
}

