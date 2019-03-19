using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;
using Mix2App.UI;

namespace Mix2App.Marriage{
	public class Marriage : MonoBehaviour,IReceiver {

		[SerializeField] private GameObject CameraObj = null;
		[SerializeField] private GameObject CameraObjMarriage = null;

		[SerializeField] private GameObject EventMarriage = null;
		[SerializeField] private GameObject EventWait = null;

		[SerializeField] private GameObject EventStart = null;			// 開始画面
		[SerializeField] private GameObject EventTown = null;			// たまタウン
		[SerializeField] private GameObject EventFinale = null;			// 結婚式
		[SerializeField] private GameObject EventGarden = null;			// 庭園
		[SerializeField] private GameObject EventBeach = null;			// 海岸
		[SerializeField] private GameObject EventPark = null;			// 遊園地

		[SerializeField] private GameObject man_stay = null;			// 男の子、草原での停止
		[SerializeField] private GameObject man_walk0 = null;			// 男の子、たまタウンの歩き
		[SerializeField] private GameObject man_walk1 = null;			// 男の子、遊園地の歩き
		[SerializeField] private GameObject man_walk2 = null;			// 男の子、海岸の歩き
		[SerializeField] private GameObject man_sit = null;				// 男の子、庭園での座り
		[SerializeField] private GameObject man_kiss = null;			// 男の子、庭園でのキッス
		[SerializeField] private GameObject man_happy = null;			// 男の子、結婚式での喜び

		[SerializeField] private GameObject woman_stay = null;			// 女の子、草原での停止
		[SerializeField] private GameObject woman_walk0 = null;			// 女の子、たまタウンの歩き
		[SerializeField] private GameObject woman_walk1 = null;			// 女の子、遊園地の歩き
		[SerializeField] private GameObject woman_walk2 = null;			// 女の子、海岸の歩き
		[SerializeField] private GameObject woman_sit = null;			// 女の子、庭園での座り
		[SerializeField] private GameObject woman_kiss = null;			// 女の子、庭園でのキッス
		[SerializeField] private GameObject woman_happy = null;			// 女の子、結婚式での喜び

		[SerializeField] private GameObject omedetouPosition = null;
		[SerializeField] private GameObject man_happy2 = null;			// 男の子、結婚式の双子の子
		[SerializeField] private GameObject woman_happy2 = null;		// 女の子、結婚式の双子の子
		[SerializeField] private GameObject[] petChara = null;				

		[SerializeField] private GameObject manChara1 = null;			// 男の子
		[SerializeField] private GameObject womanChara1 = null;			// 女の子
		[SerializeField] private GameObject manChara2 = null;			// 男の子の双子
		[SerializeField] private GameObject womanChara2 = null;			// 女の子の双子

		[SerializeField] private GameObject petChara1 = null;			// 男の子のペット
		[SerializeField] private GameObject petChara2 = null;			// 女の子のペット

		[SerializeField] private GameObject PanelWhite = null;
		[SerializeField] private GameObject PanelWhite2 = null;

		[SerializeField] private GameObject EventNet = null;			// 通信画面一式
		[SerializeField] private GameObject EventJyunbi = null;			// たまごっちみーつ準備画面
		[SerializeField] private GameObject EventJyunbi2 = null;		// たまごっちみーつ失敗説明画面
		[SerializeField] private GameObject EventSippai = null;			// 通信失敗画面
		[SerializeField] private GameObject EventSeikou = null;			// 通信成功画面
        [SerializeField] private GameObject EventBLESippai = null;      // BLEエラー画面

		//[SerializeField] private Button ButtonJyunbi;
		[SerializeField] private Button ButtonSippaiEnd = null;
		[SerializeField] private Button ButtonSippaiRetry = null;
		[SerializeField] private Button ButtonSeikou = null;

		[SerializeField] private GameObject EventEgg1 = null;			// 
		[SerializeField] private GameObject EventEgg2 = null;			// 

		[SerializeField] private GameObject EventEggBar = null;			//


		private object[]		mparam;

		private float 			panelWhiteA;

		private CharaBehaviour	cbMan1;								// 男の子キャラ
		private CharaBehaviour	cbMan2;								// 男の子の双子キャラ
		private CharaBehaviour	cbWoman1;							// 女の子キャラ
		private CharaBehaviour	cbWoman2;							// 女の子の双子キャラ
		private PetBehaviour	pbPet1;								// ペット１
		private PetBehaviour	pbPet2;								// ペット２

		private Vector3 posOmedetou;


		private bool startEndFlag = false;
		private int waitCount;
		private bool waitFlag = false;

		private bool jyunbiButtonFlag = false;
		private bool _CoroutineFlagStart;
		private bool _CoroutineFlagTown;
		private bool _CoroutineFlagPark;
		private bool _CoroutineFlagBeach;
		private bool _CoroutineFlagFinale;

		private float eggBarScale;

		private statusJobCount	jobCount = statusJobCount.marriageJobCount000;
		private enum statusJobCount{
			marriageJobCount000,
			marriageJobCount010,
			marriageJobCount011,
			marriageJobCount020,
			marriageJobCount030,
			marriageJobCount040,
			marriageJobCount041,
			marriageJobCount042,
			marriageJobCount050,
			marriageJobCount051,
			marriageJobCount052,
			marriageJobCount060,
			marriageJobCount061,
			marriageJobCount062,
			marriageJobCount070,
			marriageJobCount071,
			marriageJobCount072,
			marriageJobCount080,
			marriageJobCount081,
			marriageJobCount082,
			marriageJobCount090,
			marriageJobCount100,
			marriageJobCount110, 
			marriageJobCount120,
			marriageJobCount130,
			marriageJobCount140,
			marriageJobCount150,
		}

		private User muser1;//自分
		private User muser2;//相手
		private int mkind;//結婚種類
		private int mkind1;//兄弟種類
		private int mkind2;//兄弟種類
		private int mBleSuccess;//結婚通信結果
        private int mBleErrorCode;

		private bool man2AttendFlag;
		private bool woman2AttendFlag;
		private bool manPetAttendFlag;
		private bool womanPetAttendFlag;


		private bool retryFlag;

        void Awake(){
            Debug.Log("Marriage Awake");
            mparam = null;
            muser1 = null;
            muser2 = null;
            mBleSuccess = 0;
            mBleErrorCode = 0;
            mkind = 0;
            mkind1 = 0;
            mkind2 = 0;
        }

		public void receive(params object[] parameter){
			Debug.Log ("Marriage receive");
			mparam = parameter;
		}

		IEnumerator Start () {
			Debug.Log ("Marriage start");

			//単体動作テスト用
			//パラメタ詳細は設計書参照
			if (mparam==null) {
				mparam = new object[] {
					0,
					ManagerObject.instance.player,
					0,
					new TestUser(1,UserKind.ANOTHER,UserType.MIX2,1,23,0,0,1),
					0
				};
			}

			mkind = (int)mparam[0];
			muser1 = (User)mparam[1];
			mkind1 = (int)mparam[2];
			muser2 = (User)mparam[3];
			mkind2 = (int)mparam[4];

			jobCount = statusJobCount.marriageJobCount000;
			startEndFlag = false;
			waitCount = 1;

			man_stay.SetActive (false);
			woman_stay.SetActive (false);
			man_walk0.SetActive (false);
			woman_walk0.SetActive (false);
			man_walk1.SetActive (false);
			woman_walk1.SetActive (false);
			man_walk2.SetActive (false);
			woman_walk2.SetActive (false);
			man_sit.SetActive (false);
			woman_sit.SetActive (false);
			man_kiss.SetActive (false);
			woman_kiss.SetActive (false);
			man_happy.SetActive (false);
			woman_happy.SetActive (false);
			man_happy2.SetActive (false);
			woman_happy2.SetActive (false);
			for (int i = 0; i < 4; i++) {
				petChara [i].SetActive (false);
			}

			man2AttendFlag = false;
			woman2AttendFlag = false;
			manPetAttendFlag = false;
			womanPetAttendFlag = false;


			jyunbiButtonFlag = false;
			//ButtonJyunbi.onClick.AddListener (ButtonJyunbiClick);
			ButtonSippaiEnd.onClick.AddListener (ButtonSippaiEndClick);
			ButtonSippaiRetry.onClick.AddListener (ButtonSippaiRetryClick);
			ButtonSeikou.onClick.AddListener (ButtonSeikouClick);
            EventBLESippai.transform.Find("Button_blue_tojiru").gameObject.GetComponent<Button>().onClick.AddListener(ButtonBLEClick);

            EventEgg1.transform.Find("Image").gameObject.GetComponent<Button>().onClick.AddListener (ButtonEggClick);

			EventStart.SetActive (false);
			EventTown.SetActive (false);
			EventPark.SetActive (false);
			EventBeach.SetActive (false);
			EventGarden.SetActive (false);
			EventFinale.SetActive (false);



//			muser1.chara2 = muser1.chara1;
//			muser2.chara2 = muser2.chara1;
//			muser2.pet = muser1.pet;



			float use_screen_x = Screen.currentResolution.width;
			float use_screen_y = Screen.currentResolution.height;
			#if UNITY_EDITOR
			use_screen_x = Screen.width;
			use_screen_y = Screen.height;
			#endif

			float num;
			if (use_screen_x >= use_screen_y) {
				num = use_screen_x / use_screen_y;
			} else {
				num = use_screen_y / use_screen_x;
			}

			if ((num > 1.33f) && (num < 1.34f)) {
				EventMarriage.GetComponent<Transform> ().transform.localScale = new Vector3 (1.4f, 1.4f, 1.0f);
				EventWait.GetComponent<Transform> ().transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
			} else {
				EventMarriage.GetComponent<Transform> ().transform.localScale = new Vector3 (1.4f, 1.4f, 1.0f);
				EventWait.GetComponent<Transform> ().transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
			}

			// 男の子と女の子のたまごっちをここで設定する

			cbMan1 = manChara1.GetComponent<CharaBehaviour> ();				// 男の子
			cbMan2 = manChara2.GetComponent<CharaBehaviour> ();				// 男の子の双子
			cbWoman1 = womanChara1.GetComponent<CharaBehaviour> ();			// 女の子
			cbWoman2 = womanChara2.GetComponent<CharaBehaviour> ();			// 女の子の双子
			if (mkind1 == 0) {
				yield return cbMan1.init (muser1.chara1);
				if (muser1.chara2 != null) {
					yield return cbMan2.init (muser1.chara2);
					man2AttendFlag = true;
				}
			} else {
				yield return cbMan1.init (muser1.chara2);
				yield return cbMan2.init (muser1.chara1);
				man2AttendFlag = true;
			}
			if (mkind2 == 0) {
				yield return cbWoman1.init (muser2.chara1);
				if (muser2.chara2 != null) {
					yield return cbWoman2.init (muser2.chara2);
					woman2AttendFlag = true;
				}
			} else {
				yield return cbWoman1.init (muser2.chara2);
				yield return cbWoman2.init (muser2.chara1);
				woman2AttendFlag = true;
			}

			// ペットをここで設定する

			pbPet1 = petChara1.GetComponent<PetBehaviour> ();				// 男の子のペット
			pbPet2 = petChara2.GetComponent<PetBehaviour> ();				// 女の子のペット
			if (muser1.pet != null) {
				yield return pbPet1.init (muser1.pet);
				manPetAttendFlag = true;
			}
			if (muser2.pet != null) {
				yield return pbPet2.init (muser2.pet);
				womanPetAttendFlag = true;
			}


			if (!man2AttendFlag) {
				manChara2.transform.localScale = new Vector3 (0, 0, 0);						// 男の子の親族は出席しないので表示を消しておく
				EventFinale.transform.Find("bg/manChara2").gameObject.SetActive(false);		// 男の子の親族は出席しないので表示を消しておく
			}
			if (!woman2AttendFlag) {
				womanChara2.transform.localScale = new Vector3 (0, 0, 0);					// 女の子の親族は出席しないので表示を消しておく
				EventFinale.transform.Find("bg/womanChara2").gameObject.SetActive(false);	// 女の子の親族は出席しないので表示を消しておく
			}
			if (!manPetAttendFlag) {
				petChara1.transform.localScale = new Vector3 (0, 0, 0);						// 男の子のペットは出席しないので表示を消しておく
				EventFinale.transform.Find("bg/manPet").gameObject.SetActive(false);		// 男の子のペットは出席しないので表示を消しておく
			}
			if (!womanPetAttendFlag) {
				petChara2.transform.localScale = new Vector3 (0, 0, 0);						// 女の子のペットは出席しないので表示を消しておく
				EventFinale.transform.Find("bg/womanPet").gameObject.SetActive(false);		// 女の子のペットは出席しないので表示を消しておく
			}





			startEndFlag = true;
		}

		void Destroy()
		{
			Debug.Log("Marriage Destroy");
		}

		void mblekekkon(bool success,object data)
		{
			Debug.LogFormat("Marriage mblekekkon:{0},{1}",success,data);
            //dataの内容は設計書を参照
            //dataを変えたい場合はConnectManagerDriverのBLEKekkon()を変える
            if (success){
                mBleSuccess = 1;
            } else {
                mBleSuccess = 2;
                mBleErrorCode = (int)data;

                retryFlag = false;
                waitCount = 10;
                jobCount = statusJobCount.marriageJobCount140;
            }
		}

		void Update () {
			switch (jobCount) {
			case	statusJobCount.marriageJobCount000:
				{
					EventNet.SetActive (true);
					EventJyunbi.SetActive (true);
					EventJyunbi2.SetActive (false);
					EventSippai.SetActive (false);
					EventSeikou.SetActive (false);

                    EventStart.SetActive(false);
                    EventTown.SetActive(false);
                    EventPark.SetActive(false);
                    EventBeach.SetActive(false);
                    EventGarden.SetActive(false);
                    EventFinale.SetActive(false);
                    
                    jobCount = statusJobCount.marriageJobCount010;
					jyunbiButtonFlag = false;
					waitCount = 1;
					break;
				}
			case	statusJobCount.marriageJobCount010:
				{
					if (jyunbiButtonFlag) {
						jyunbiButtonFlag=false;
						EventJyunbi.SetActive (false);
						EventJyunbi2.SetActive (true);

						jobCount = statusJobCount.marriageJobCount011;
					}
					break;
				}
			case	statusJobCount.marriageJobCount011:
				{
					if (jyunbiButtonFlag) {
//						EventNet.SetActive (false);
//						EventJyunbi2.SetActive (false);

						jobCount = statusJobCount.marriageJobCount020;
					}
					break;
				}
			case	statusJobCount.marriageJobCount020:
				{
					if (startEndFlag) {
						waitCount--;
						if (waitCount == 0) {
							ManagerObject.instance.sound.playBgm (19);

							StartCoroutine ("WaitMain");

							jobCount = statusJobCount.marriageJobCount030;
							cbMan1.gotoAndPlay (MotionLabel.WALK);
							cbWoman1.gotoAndPlay (MotionLabel.WALK);

							//裏でBLE通信する※パラメタは設計書参照
							mBleSuccess = 0;
							GameCall call = new GameCall (CallLabel.BLE_KEKKON, mkind, muser1, mkind1, muser2, mkind2);
							call.AddListener (mblekekkon);
							ManagerObject.instance.connect.send (call);
						}
					}
					break;
				}
			case	statusJobCount.marriageJobCount030:
				{
					MarriageJobTypeOpening ();
					EventNet.SetActive (false);
					EventJyunbi2.SetActive (false);
					EventStart.SetActive (true);
					jobCount = statusJobCount.marriageJobCount040;
					Debug.Log ("草原");

					_CoroutineFlagStart = true;
					StartCoroutine ("StartCharaIdou");
					StartCoroutine ("StartHeartIdou");
					break;
				}
			case	statusJobCount.marriageJobCount040:
				{
					if (EventStart.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.marriageJobCount041;
						panelWhiteA = 0.0f;
					}
					break;
				}
			case	statusJobCount.marriageJobCount041:
				{
					if (WhiteOut ()) {
						EventStart.SetActive (false);
						EventTown.SetActive (true);
						jobCount = statusJobCount.marriageJobCount042;
						_CoroutineFlagStart = false;

						Debug.Log ("たまタウン");

						_CoroutineFlagTown = true;
						StartCoroutine ("TownCharaIdou");

						// 左向きに散歩
						cbMan1.gotoAndPlay (MotionLabel.WALK);
						cbWoman1.gotoAndPlay (MotionLabel.WALK);
					}
					break;
				}
			case	statusJobCount.marriageJobCount042:
				{
					if (WhiteIn ()) {
						jobCount = statusJobCount.marriageJobCount050;
					}
					break;
				}
			case	statusJobCount.marriageJobCount050:
				{
					if (EventTown.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.marriageJobCount051;
						panelWhiteA = 0.0f;
					}
					break;
				}
			case	statusJobCount.marriageJobCount051:
				{
					if (WhiteOut ()) {
						EventTown.SetActive (false);
						EventPark.SetActive (true);
						jobCount = statusJobCount.marriageJobCount052;
						_CoroutineFlagTown = false;

						Debug.Log ("遊園地");

						_CoroutineFlagPark = true;
						StartCoroutine ("ParkCharaIdou");

						// 右向きに散歩
						cbMan1.gotoAndPlay (MotionLabel.WALK);
						cbWoman1.gotoAndPlay (MotionLabel.WALK);
					}
					break;
				}
			case	statusJobCount.marriageJobCount052:
				{
					if (WhiteIn ()) {
						jobCount = statusJobCount.marriageJobCount060;
					}
					break;
				}
			case	statusJobCount.marriageJobCount060:
				{
					if (EventPark.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.marriageJobCount061;
						panelWhiteA = 0.0f;
					}
					break;
				}
			case	statusJobCount.marriageJobCount061:
				{
					if (WhiteOut ()) {
						EventPark.SetActive (false);
						EventBeach.SetActive (true);
						jobCount = statusJobCount.marriageJobCount062;
						_CoroutineFlagPark = false;

						Debug.Log ("海岸");

						_CoroutineFlagBeach = true;
						StartCoroutine ("BeachCharaIdou");

						// 左向きに散歩
						cbMan1.gotoAndPlay (MotionLabel.WALK);
						cbWoman1.gotoAndPlay (MotionLabel.WALK);
					}
					break;
				}
			case	statusJobCount.marriageJobCount062:
				{
					if (WhiteIn ()) {
						jobCount = statusJobCount.marriageJobCount070;
					}
					break;
				}
			case	statusJobCount.marriageJobCount070:
				{
					if (EventBeach.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0) {
						jobCount = statusJobCount.marriageJobCount071;
						panelWhiteA = 0.0f;
					}
					break;
				}
			case	statusJobCount.marriageJobCount071:
				{
					if (WhiteOut ()) {
						EventBeach.SetActive (false);
						EventGarden.SetActive (true);
						jobCount = statusJobCount.marriageJobCount072;
						_CoroutineFlagBeach = false;

						Debug.Log ("庭園");

						// ベンチに向き合って座る
						cbMan1.gotoAndPlay (MotionLabel.SIT2);
						cbWoman1.gotoAndPlay (MotionLabel.SIT2);
					}
					break;
				}
			case	statusJobCount.marriageJobCount072:
				{
					if (WhiteIn ()) {
						jobCount = statusJobCount.marriageJobCount080;
					}
					break;
				}
			case	statusJobCount.marriageJobCount080:
				{
					if (EventGarden.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.marriageJobCount081;
						panelWhiteA = 0.0f;
					}
					break;
				}
			case	statusJobCount.marriageJobCount081:
				{
					if (WhiteOut ()) {
						EventGarden.SetActive (false);
						EventFinale.SetActive (true);
						jobCount = statusJobCount.marriageJobCount082;

						Debug.Log ("結婚式");
						ManagerObject.instance.sound.playBgm (20,false);

						cbMan1.gotoAndPlay (MotionLabel.SHY1);
						cbWoman1.gotoAndPlay (MotionLabel.SHY1);

						posOmedetou = omedetouPosition.transform.localPosition;
						_CoroutineFlagFinale = true;
						StartCoroutine ("FinaleCharaIdou");


						if (man2AttendFlag) {
							cbMan2.gotoAndPlay (MotionLabel.IDLE);
						}
						if (woman2AttendFlag) {
							cbWoman2.gotoAndPlay (MotionLabel.IDLE);
						}
						if (manPetAttendFlag) {
							pbPet1.gotoAndPlay (MotionLabel.IDLE);
						}
						if (womanPetAttendFlag) {
							pbPet2.gotoAndPlay (MotionLabel.IDLE);
						}

					}
					break;
				}
			case	statusJobCount.marriageJobCount082:
				{
					if (WhiteIn ()) {
						jobCount = statusJobCount.marriageJobCount090;
					}
					break;
				}
			case	statusJobCount.marriageJobCount090:
				{
					if (EventFinale.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.marriageJobCount100;
						_CoroutineFlagFinale = false;
					}
					break;
				}
			case	statusJobCount.marriageJobCount100:
				{
					MarriageJobTypeEnding ();

					ManagerObject.instance.sound.playBgm (20);

					jobCount = statusJobCount.marriageJobCount110;
					break;
				}
			case	statusJobCount.marriageJobCount110:
				{
					// ホワイトイン
					panelWhiteA -= 5.0f;
					PanelWhite.GetComponent<Image> ().color = new Color (1.0f, 1.0f, 1.0f, panelWhiteA / 255.0f);
					if (panelWhiteA == 0) {
						jobCount = statusJobCount.marriageJobCount120;
						PanelWhite.SetActive (false);
					}
					break;
				}
			case	statusJobCount.marriageJobCount120:
				{
					retryFlag = false;

					//結婚通信が完了するまで待つ
					if (mBleSuccess>0) {
						if (mBleSuccess==1) {
							//通信成功時はホーム画面へ
							jobCount = statusJobCount.marriageJobCount130;
						} else {
							//通信失敗時はタウン画面へ
							jobCount = statusJobCount.marriageJobCount140;
						}
						waitCount = 10;
						EventEggBar.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
					}

					break;
				}
			case	statusJobCount.marriageJobCount130:
				{
					if (waitCount != 0) {
						waitCount--;
						break;
					}
					ManagerObject.instance.sound.playSe (15);

					// 通信成功したので成功画面を表示
					EventNet.SetActive (true);
					EventJyunbi.SetActive (false);
					EventSippai.SetActive (false);
					EventSeikou.SetActive (true);
                    EventBLESippai.SetActive(false);
                    
                    jobCount = statusJobCount.marriageJobCount150;
					break;
				}
			case	statusJobCount.marriageJobCount140:
				{
					if (waitCount != 0) {
						waitCount--;
						break;
					}
					ManagerObject.instance.sound.playSe (16);

					// 通信失敗したので失敗画面を表示
					EventNet.SetActive (true);
					EventJyunbi.SetActive (false);
					EventSippai.SetActive (true);
					EventSeikou.SetActive (false);
                    EventBLESippai.SetActive(false); 
                    
                    if(mBleErrorCode == 7)
                    {
                        EventBLESippai.SetActive(true);
                    }

                    jobCount = statusJobCount.marriageJobCount150;
					break;
				}
			case	statusJobCount.marriageJobCount150:
				{
					if (retryFlag) {
						EventMarriage.SetActive (true);
						CameraObj.SetActive (true);
						CameraObjMarriage.SetActive (false);

						EventStart.SetActive (false);
						EventTown.SetActive (false);
						EventPark.SetActive (false);
						EventBeach.SetActive (false);
						EventGarden.SetActive (false);
						EventFinale.SetActive (false);

						jobCount = statusJobCount.marriageJobCount000;
					}
					break;
				}
			}


			UIFunction.TamagochiImageMove (EventStart, manChara1, "bg1/manChara/");
			UIFunction.TamagochiImageMove (EventStart, womanChara1, "bg1/womanChara/");

			UIFunction.TamagochiImageMove (EventTown, manChara1, "Chara/manChara/");
			UIFunction.TamagochiImageMove (EventTown, womanChara1, "Chara/womanChara/");

			UIFunction.TamagochiImageMove (EventPark, manChara1, "Chara/manChara/");
			UIFunction.TamagochiImageMove (EventPark, womanChara1, "Chara/womanChara/");

			UIFunction.TamagochiImageMove (EventBeach, manChara1, "Chara/manChara/");
			UIFunction.TamagochiImageMove (EventBeach, womanChara1, "Chara/womanChara/");

			UIFunction.TamagochiImageMove (EventGarden, manChara1, "bg1/manChara/");
			UIFunction.TamagochiImageMove (EventGarden, womanChara1, "bg1/womanChara/");

			UIFunction.TamagochiImageMove (EventFinale, manChara1, "bg/manChara/");
			UIFunction.TamagochiImageMove (EventFinale, womanChara1, "bg/womanChara/");
			UIFunction.TamagochiImageMove (EventFinale, manChara2, "bg/manChara2/");
			UIFunction.TamagochiImageMove (EventFinale, womanChara2, "bg/womanChara2/");

			UIFunction.TamagochiPetImageMove (EventFinale, petChara1, "bg/manPet/");
			UIFunction.TamagochiPetImageMove (EventFinale, petChara2, "bg/womanPet/");
		}


		private void MarriageJobTypeOpening(){
			EventMarriage.SetActive (true);
			EventWait.SetActive (false);
			CameraObj.SetActive (false);
			CameraObjMarriage.SetActive (true);
			PanelWhite.SetActive (false);
		}


		private void MarriageJobTypeEnding(){
			eggBarScale = 0.0f;

			StartCoroutine ("WaitEggBar");

			EventWait.SetActive (true);
			EventMarriage.SetActive (false);
			CameraObjMarriage.SetActive (true);
			PanelWhite.SetActive (true);
			panelWhiteA = 255.0f;
			PanelWhite.GetComponent<Image> ().color = new Color (1.0f, 1.0f, 1.0f, panelWhiteA / 255.0f);
		}



		private IEnumerator StartCharaIdou(){
			float _posMan;
			float _posWoman;

			_posMan = 650.0f;
			_posWoman = -650.0f;

			while (_CoroutineFlagStart) {
				EventStart.transform.Find ("bg1/manChara").gameObject.transform.localPosition = new Vector3 (_posMan, 50, 0);
				EventStart.transform.Find ("bg1/womanChara").gameObject.transform.localPosition = new Vector3 (_posWoman, 50, 0);

				_posMan -= (1.0f * (60 * Time.deltaTime));
				_posWoman += (1.0f * (60 * Time.deltaTime));

				if ((_posMan <= 200.0f)) {
					if (cbMan1.nowlabel != MotionLabel.SHY4) {
						cbMan1.gotoAndPlay (MotionLabel.SHY4);
						cbWoman1.gotoAndPlay (MotionLabel.SHY4);
					}
				}

				if (_posMan <= 57.0f) {
					_posMan = 57.0f;
					_posWoman = -57.0f;
					if (cbMan1.nowlabel != MotionLabel.SHY1) {
						cbMan1.gotoAndPlay (MotionLabel.SHY1);
						cbWoman1.gotoAndPlay (MotionLabel.SHY1);
					}
				}

				yield return null;
			}
		}

		private IEnumerator StartHeartIdou(){
			EventStart.transform.Find ("heartPG").gameObject.SetActive (false);
			EventStart.transform.Find ("heartPG").transform.localPosition = new Vector3 (3.0f, -15.0f, 0.0f);
			while (true) {
				if ((EventStart.transform.Find ("bg1/manChara").gameObject.transform.localPosition.x == 57.0f) && (EventStart.transform.Find ("bg1/womanChara").gameObject.transform.localPosition.x == -57.0f)) {
					break;
				}
				yield return null;
			}
			for (int i = 0; i < 4; i++) {
				EventStart.transform.Find ("heartPG").gameObject.SetActive (true);
				StartCoroutine (StartHeartScaleUp (0.01f));
				while (true) {
					Vector3 _pos = new Vector3 (0.0f, 5.0f, 0.0f);
					EventStart.transform.Find ("heartPG").transform.localPosition = Vector3.MoveTowards (EventStart.transform.Find ("heartPG").transform.localPosition, _pos, 12 * Time.deltaTime);
					if (EventStart.transform.Find ("heartPG").transform.localPosition.y == _pos.y) {
						break;
					}
					yield return null;
				}
				EventStart.transform.Find ("heartPG").gameObject.SetActive (false);
				EventStart.transform.Find ("heartPG").transform.localPosition = new Vector3 (3.0f, -15.0f, 0.0f);
				yield return new WaitForSeconds (0.8f);
			}
		}
		private IEnumerator StartHeartScaleUp(float _size){
			EventStart.transform.Find("heartPG").transform.localScale = new Vector3 (_size,_size, 1.0f);
			while (true) {
				_size += 0.01f;
				if (_size >= 0.1f) {
					EventStart.transform.Find ("heartPG").transform.localScale = new Vector3 (0.1f, 0.1f, 1.0f);
					break;
				}
				EventStart.transform.Find ("heartPG").transform.localScale = new Vector3 (_size, _size, 1.0f);
				yield return null;
			}
		}


		private IEnumerator TownCharaIdou(){
			EventTown.transform.Find ("Chara").gameObject.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);

			while (_CoroutineFlagTown) {
				Vector3 _pos = EventTown.transform.Find ("Chara").gameObject.transform.localPosition;
				_pos.x -= (0.6f * (60 * Time.deltaTime));
				EventTown.transform.Find ("Chara").gameObject.transform.localPosition = _pos;
				yield return null;
			}
		}

		private IEnumerator ParkCharaIdou(){
			EventPark.transform.Find ("Chara").gameObject.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);

			while(_CoroutineFlagPark){
				Vector3 _pos = EventPark.transform.Find ("Chara").gameObject.transform.localPosition;
				_pos.x += (0.075f * (60 * Time.deltaTime));
				EventPark.transform.Find ("Chara").gameObject.transform.localPosition = _pos;
				yield return null;
			}
		}

		private IEnumerator BeachCharaIdou(){
			EventBeach.transform.Find ("Chara").gameObject.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);

			while (_CoroutineFlagBeach) {
				Vector3 _pos = EventBeach.transform.Find ("Chara").gameObject.transform.localPosition;
				_pos.x -= (0.6f * (60 * Time.deltaTime));
				EventBeach.transform.Find ("Chara").gameObject.transform.localPosition = _pos;
				yield return null;
			}
		}

		private Vector3 _posCharaMan;
		private Vector3 _posCharaWoman;
		private IEnumerator FinaleCharaIdou(){
			Vector3 _posCharaMan2;
			Vector3 _posCharaWoman2;
			Vector3 _posPet1;
			Vector3 _posPet2;
			float	_posCharaMan2y;
			float	_posCharaWoman2y;
			float	_posPet1y;
			float	_posPet2y;

			_posCharaMan = new Vector3 (70.0f, -120.0f, 0.0f);
			_posCharaWoman = new Vector3 (-50.0f, -120.0f, 0.0f);
			_posCharaMan2 = new Vector3 (60.0f, 30.0f, 0.0f);
			_posCharaWoman2 = new Vector3 (-40.0f, 30.0f, 0.0f);
			_posPet1 = new Vector3 (65.0f, -15.0f, 0.0f);
			_posPet2 = new Vector3 (-45.0f, -15.0f, 0.0f);

			_posCharaMan2y = _posCharaMan2.y;
			_posCharaWoman2y = _posCharaWoman2.y;
			_posPet1y = _posPet1.y;
			_posPet2y = _posPet2.y;

			StartCoroutine ("FinaleCharaKiss");

			while (_CoroutineFlagFinale) {
				if (posOmedetou.y != omedetouPosition.transform.localPosition.y) {
					_posCharaMan2.y = _posCharaMan2y + ((omedetouPosition.transform.localPosition.y - posOmedetou.y) / 2.0f);
					_posCharaWoman2.y = _posCharaWoman2y + ((omedetouPosition.transform.localPosition.y - posOmedetou.y) / 2.0f);

					_posPet1.y = _posPet1y + ((omedetouPosition.transform.localPosition.y - posOmedetou.y) / 3.0f);
					_posPet2.y = _posPet2y + ((omedetouPosition.transform.localPosition.y - posOmedetou.y) / 3.0f);
				} else {
					_posCharaMan2.y = _posCharaMan2y;
					_posCharaWoman2.y = _posCharaWoman2y;
					_posPet1.y = _posPet1y;
					_posPet2.y = _posPet2y;
				}

				EventFinale.transform.Find ("bg/manChara").gameObject.transform.localPosition = _posCharaMan;
				EventFinale.transform.Find ("bg/womanChara").gameObject.transform.localPosition = _posCharaWoman;
				EventFinale.transform.Find ("bg/manChara2").gameObject.transform.localPosition = _posCharaMan2;
				EventFinale.transform.Find ("bg/womanChara2").gameObject.transform.localPosition = _posCharaWoman2;
				EventFinale.transform.Find ("bg/manPet").gameObject.transform.localPosition = _posPet1;
				EventFinale.transform.Find ("bg/womanPet").gameObject.transform.localPosition = _posPet2;

				yield return null;
			}
		}
		private IEnumerator FinaleCharaKiss(){
			yield return new WaitForSeconds (9.0f);

			Vector3 _posMan = new Vector3 (57.0f, -120.0f, 0);
			//Vector3 _posWoman = new Vector3 (-57.0f, -120.0f, 0);

			while (_CoroutineFlagFinale) {
				_posCharaMan = Vector3.MoveTowards (_posCharaMan, _posMan, (5.0f * Time.deltaTime));
				_posCharaWoman = Vector3.MoveTowards (_posCharaWoman, _posMan, (5.0f * Time.deltaTime));
				if (_posCharaMan.x == _posMan.x) {
					break;
				}
				yield return null;
			}

			cbMan1.gotoAndPlay (MotionLabel.SIT2);
			cbWoman1.gotoAndPlay (MotionLabel.SIT2);

			yield return new WaitForSeconds (2.0f);

//			cbMan1.gotoAndPlay (MotionLabel.KISS);
//			cbWoman1.gotoAndPlay (MotionLabel.KISS);
			cbMan1.gotoAndStop(MotionLabel.KISS);
			cbWoman1.gotoAndStop (MotionLabel.KISS);



		}

		private bool WhiteOut(){
			panelWhiteA += (4.0f * (60 * Time.deltaTime));
			if(panelWhiteA >= 255.0f){
				panelWhiteA = 255.0f;
			}
			PanelWhite2.GetComponent<Image> ().color = new Color (1.0f, 1.0f, 1.0f, panelWhiteA / 255.0f);
			if (panelWhiteA == 255.0f) {
				return true;
			}
			return false;
		}

		private bool WhiteIn(){
			panelWhiteA -= (4.0f * (60 * Time.deltaTime));
			if(panelWhiteA <= 0.0f){
				panelWhiteA = 0.0f;
			}
			PanelWhite2.GetComponent<Image> ().color = new Color (1.0f, 1.0f, 1.0f, panelWhiteA / 255.0f);
			if (panelWhiteA == 0.0f) {
				return true;
			}
			return false;
		}



		public void ButtonJyunbiClick(){
			ManagerObject.instance.sound.playSe (13);
			jyunbiButtonFlag = true;
		}
		private void ButtonSippaiEndClick(){
			ManagerObject.instance.sound.playSe (17);
			ManagerObject.instance.view.change(SceneLabel.TOWN);
		}
		private void ButtonSippaiRetryClick(){
			ManagerObject.instance.sound.playSe (13);
			retryFlag = true;
		}
		private void ButtonSeikouClick(){
			ManagerObject.instance.sound.playSe (17);
			ManagerObject.instance.view.change(SceneLabel.HOME);
		}
        private void ButtonBLEClick(){
            ManagerObject.instance.sound.playSe(13);
            EventBLESippai.SetActive(false);
        }

        private void ButtonEggClick(){
			EventEgg1.SetActive (false);
			StartCoroutine ("WaitEgg");
		}



		// 卵を押された時のアニメーション
		private IEnumerator WaitEgg(){
			EventEgg2.SetActive (true);
			while(true){
				if (EventEgg2.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
					break;
				}
				yield return null;
			}
			EventEgg1.SetActive (true);
			EventEgg2.SetActive (false);
		}


		private IEnumerator WaitEggBar(){
			while (true) {
				if ((waitFlag) && (mBleSuccess > 0)) {
					break;
				}
				if (eggBarScale >= 1.0f) {
					eggBarScale = 1.0f;
				}
				EventEggBar.transform.localScale = new Vector3 (eggBarScale, 1.0f, 1.0f);
				if (eggBarScale == 1.0f) {
					break;
				}
//				eggBarScale += 0.002f;
//				yield return new WaitForSeconds (0.1f);
				eggBarScale += 0.2f;
				yield return new WaitForSeconds (10.0f);
			}
		}



		private IEnumerator WaitMain(){
			waitFlag = false;
			yield return new WaitForSeconds (120.0f);
			waitFlag = true;
		}
	}
}
