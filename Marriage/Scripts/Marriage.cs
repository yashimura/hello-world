using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

namespace Mix2App.Marriage{
	public class Marriage : MonoBehaviour,IReceiver {

		[SerializeField] private GameObject CameraObj;
		[SerializeField] private GameObject CameraObjMarriage;

		[SerializeField] private GameObject EventMarriage;
		[SerializeField] private GameObject	EventWait;

		[SerializeField] private GameObject EventStart;				// 開始画面
		[SerializeField] private GameObject EventTown;				// たまタウン
		[SerializeField] private GameObject	EventFinale;			// 結婚式
		[SerializeField] private GameObject	EventGarden;			// 庭園
		[SerializeField] private GameObject EventBeach;				// 海岸
		[SerializeField] private GameObject EventPark;				// 遊園地

		[SerializeField] private GameObject man_stay;				// 男の子、草原での停止
		[SerializeField] private GameObject man_walk0;				// 男の子、たまタウンの歩き
		[SerializeField] private GameObject	man_walk1;				// 男の子、遊園地の歩き
		[SerializeField] private GameObject	man_walk2;				// 男の子、海岸の歩き
		[SerializeField] private GameObject man_sit;				// 男の子、庭園での座り
		[SerializeField] private GameObject man_kiss;				// 男の子、庭園でのキッス
		[SerializeField] private GameObject	man_happy;				// 男の子、結婚式での喜び

		[SerializeField] private GameObject woman_stay;				// 女の子、草原での停止
		[SerializeField] private GameObject woman_walk0;			// 女の子、たまタウンの歩き
		[SerializeField] private GameObject woman_walk1;			// 女の子、遊園地の歩き
		[SerializeField] private GameObject woman_walk2;			// 女の子、海岸の歩き
		[SerializeField] private GameObject woman_sit;				// 女の子、庭園での座り
		[SerializeField] private GameObject woman_kiss;				// 女の子、庭園でのキッス
		[SerializeField] private GameObject woman_happy;			// 女の子、結婚式での喜び

		[SerializeField] private GameObject omedetouPosition;
		[SerializeField] private GameObject man_happy2;				// 男の子、結婚式の双子の子
		[SerializeField] private GameObject woman_happy2;			// 女の子、結婚式の双子の子
		[SerializeField] private GameObject[] petChara;				

		[SerializeField] private GameObject manChara1;				// 男の子
		[SerializeField] private GameObject womanChara1;			// 女の子
		[SerializeField] private GameObject manChara2;				// 男の子の双子
		[SerializeField] private GameObject womanChara2;			// 女の子の双子

		[SerializeField] private GameObject petChara1;				// 男の子のペット
		[SerializeField] private GameObject petChara2;				// 女の子のペット

		[SerializeField] private GameObject PanelWhite;

		[SerializeField] private GameObject EventNet;				// 通信画面一式
		[SerializeField] private GameObject EventJyunbi;			// たまごっちみーつ準備画面
		[SerializeField] private GameObject EventSippai;			// 通信失敗画面
		[SerializeField] private GameObject EventSeikou;			// 通信成功画面

		[SerializeField] private Button ButtonJyunbi;
		[SerializeField] private Button ButtonSippai;
		[SerializeField] private Button ButtonSeikou;

		[SerializeField] private GameObject EventEgg1;				// 
		[SerializeField] private GameObject EventEgg2;				// 

		[SerializeField] private GameObject EventEggBar;			//


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
			marriageJobCount020,
			marriageJobCount030,
			marriageJobCount040,
			marriageJobCount050,
			marriageJobCount060,
			marriageJobCount070,
			marriageJobCount080,
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

		private bool man2AttendFlag;
		private bool woman2AttendFlag;
		private bool manPetAttendFlag;
		private bool womanPetAttendFlag;


		void Awake(){
			Debug.Log ("Marriage Awake");
			mparam=null;
			muser1=null;
			muser2=null;
			mBleSuccess=0;
			mkind=0;
			mkind1=0;
			mkind2=0;
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
					new TestUser(1,UserKind.ANOTHER,UserType.MIX2,23,0,0,1),
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
			ButtonJyunbi.onClick.AddListener (ButtonJyunbiClick);
			ButtonSippai.onClick.AddListener (ButtonSippaiClick);
			ButtonSeikou.onClick.AddListener (ButtonSeikouClick);
			EventEgg1.transform.Find("Image").gameObject.GetComponent<Button>().onClick.AddListener (ButtonEggClick);





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
			bool ret = (bool)data;
			if (ret) mBleSuccess = 1;
			else mBleSuccess = 2;
		}

		void Update () {
			switch (jobCount) {
			case	statusJobCount.marriageJobCount000:
				{
					EventNet.SetActive (true);
					EventJyunbi.SetActive (true);
					EventSippai.SetActive (false);
					EventSeikou.SetActive (false);

					jobCount = statusJobCount.marriageJobCount010;
					break;
				}
			case	statusJobCount.marriageJobCount010:
				{
					if (jyunbiButtonFlag) {
						EventNet.SetActive (false);
						EventJyunbi.SetActive (false);
						EventSippai.SetActive (false);
						EventSeikou.SetActive (false);

						jobCount = statusJobCount.marriageJobCount020;
					}
					break;
				}

			case	statusJobCount.marriageJobCount020:
				{
					if (startEndFlag) {
						waitCount--;
						if (waitCount == 0) {
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
					EventStart.GetComponent<Transform> ().transform.localScale = new Vector3 (3.897087f, 3.897087f, 3.897113f);
					EventTown.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
					EventBeach.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
					EventGarden.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
					EventFinale.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
					jobCount = statusJobCount.marriageJobCount040;
					Debug.Log ("草原");

					_CoroutineFlagStart = true;
					StartCoroutine ("StartCharaIdou");
					break;
				}
			case	statusJobCount.marriageJobCount040:
				{
					if (EventStart.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						EventStart.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventTown.GetComponent<Transform> ().transform.localScale = new Vector3 (0.8574497f, 0.8574497f, 0.0f);
						jobCount = statusJobCount.marriageJobCount050;
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
			case	statusJobCount.marriageJobCount050:
				{
					if (EventTown.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						EventTown.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventPark.GetComponent<Transform> ().transform.localScale = new Vector3 (3.897114f, 3.897114f, 3.897114f);
						jobCount = statusJobCount.marriageJobCount060;
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
			case	statusJobCount.marriageJobCount060:
				{
					if (EventPark.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						EventPark.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventBeach.GetComponent<Transform> ().transform.localScale = new Vector3 (0.8574491f, 0.8574491f, 0.0f);
						jobCount = statusJobCount.marriageJobCount070;
						_CoroutineFlagPark = false;

						Debug.Log ("海岸");

						_CoroutineFlagBeach = true;
						StartCoroutine ("BeachCharaIdou");

						// 左向きに散歩
						cbMan1.gotoAndPlay (MotionLabel.SHY4);
						cbWoman1.gotoAndPlay (MotionLabel.SHY4);
					}
					break;
				}
			case	statusJobCount.marriageJobCount070:
				{
					if (EventBeach.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0) {
						EventBeach.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventGarden.GetComponent<Transform> ().transform.localScale = new Vector3 (1.5f, 1.5f, 0.0f);
						jobCount = statusJobCount.marriageJobCount080;
						_CoroutineFlagBeach = false;

						Debug.Log ("庭園");

						// ベンチに向き合って座る
						cbMan1.gotoAndPlay (MotionLabel.SIT2);
						cbWoman1.gotoAndPlay (MotionLabel.SIT2);
					}
					break;
				}
			case	statusJobCount.marriageJobCount080:
				{
					if (EventGarden.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						EventGarden.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventFinale.GetComponent<Transform> ().transform.localScale = new Vector3 (1.3f, 1.3f, 0.0f);
						jobCount = statusJobCount.marriageJobCount090;

						Debug.Log ("結婚式");

						posOmedetou = omedetouPosition.transform.localPosition;
						_CoroutineFlagFinale = true;
						StartCoroutine ("FinaleCharaIdou");

						// 結婚式場で整列
						cbMan1.gotoAndPlay (MotionLabel.SHY1);
						cbWoman1.gotoAndPlay (MotionLabel.SHY1);


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
					//waitと結婚通信が完了するまで待つ
					if (waitFlag&&mBleSuccess>0) {
						if (mBleSuccess==1) {
							//通信成功時ははホーム画面へ
							jobCount = statusJobCount.marriageJobCount130;
//							ManagerObject.instance.view.change("Home");
						} else {
							//通信失敗時は最初からやりなおし
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
					// 通信成功したので成功画面を表示
					EventNet.SetActive (true);
					EventJyunbi.SetActive (false);
					EventSippai.SetActive (false);
					EventSeikou.SetActive (true);
					jobCount = statusJobCount.marriageJobCount150;
					break;
				}
			case	statusJobCount.marriageJobCount140:
				{
					if (waitCount != 0) {
						waitCount--;
						break;
					}
					// 通信失敗したので失敗画面を表示
					EventNet.SetActive (true);
					EventJyunbi.SetActive (false);
					EventSippai.SetActive (true);
					EventSeikou.SetActive (false);
					jobCount = statusJobCount.marriageJobCount150;
					break;
				}
			case	statusJobCount.marriageJobCount150:
				{
					break;
				}
			}


			TamagochiImageMove (EventStart, manChara1, "bg1/manChara");
			TamagochiImageMove (EventStart, womanChara1, "bg1/womanChara");

			TamagochiImageMove (EventTown, manChara1, "Chara/manChara");
			TamagochiImageMove (EventTown, womanChara1, "Chara/womanChara");

			TamagochiImageMove (EventPark, manChara1, "Chara/manChara");
			TamagochiImageMove (EventPark, womanChara1, "Chara/womanChara");

			TamagochiImageMove (EventBeach, manChara1, "Chara/manChara");
			TamagochiImageMove (EventBeach, womanChara1, "Chara/womanChara");

			TamagochiImageMove (EventGarden, manChara1, "bg1/manChara");
			TamagochiImageMove (EventGarden, womanChara1, "bg1/womanChara");

			TamagochiImageMove (EventFinale, manChara1, "bg/manChara");
			TamagochiImageMove (EventFinale, womanChara1, "bg/womanChara");
			TamagochiImageMove (EventFinale, manChara2, "bg/manChara2");
			TamagochiImageMove (EventFinale, womanChara2, "bg/womanChara2");

			TamagochiPetImageMove (EventFinale, petChara1, "bg/manPet");
			TamagochiPetImageMove (EventFinale, petChara2, "bg/womanPet");


/*
			EventStart.transform.Find ("bg1/manChara").gameObject.GetComponent<Image> ().sprite = manChara1.GetComponent<SpriteRenderer> ().sprite;
			EventStart.transform.Find ("bg1/womanChara").gameObject.GetComponent<Image> ().sprite = womanChara1.GetComponent<SpriteRenderer> ().sprite;

			EventTown.transform.Find ("Chara/manChara").gameObject.GetComponent<Image> ().sprite = manChara1.GetComponent<SpriteRenderer> ().sprite;
			EventTown.transform.Find ("Chara/womanChara").gameObject.GetComponent<Image> ().sprite = womanChara1.GetComponent<SpriteRenderer> ().sprite;

			EventPark.transform.Find ("Chara/manChara").gameObject.GetComponent<Image> ().sprite = manChara1.GetComponent<SpriteRenderer> ().sprite;
			EventPark.transform.Find ("Chara/womanChara").gameObject.GetComponent<Image> ().sprite = womanChara1.GetComponent<SpriteRenderer> ().sprite;

			EventBeach.transform.Find ("Chara/manChara").gameObject.GetComponent<Image> ().sprite = manChara1.GetComponent<SpriteRenderer> ().sprite;
			EventBeach.transform.Find ("Chara/womanChara").gameObject.GetComponent<Image> ().sprite = womanChara1.GetComponent<SpriteRenderer> ().sprite;

			EventGarden.transform.Find ("bg1/manChara").gameObject.GetComponent<Image> ().sprite = manChara1.GetComponent<SpriteRenderer> ().sprite;
			EventGarden.transform.Find ("bg1/womanChara").gameObject.GetComponent<Image> ().sprite = womanChara1.GetComponent<SpriteRenderer> ().sprite;

			EventFinale.transform.Find ("bg/manChara").gameObject.GetComponent<Image> ().sprite = manChara1.GetComponent<SpriteRenderer> ().sprite;
			EventFinale.transform.Find ("bg/womanChara").gameObject.GetComponent<Image> ().sprite = womanChara1.GetComponent<SpriteRenderer> ().sprite;
			EventFinale.transform.Find ("bg/manChara2").gameObject.GetComponent<Image> ().sprite = manChara2.GetComponent<SpriteRenderer> ().sprite;
			EventFinale.transform.Find ("bg/womanChara2").gameObject.GetComponent<Image> ().sprite = womanChara2.GetComponent<SpriteRenderer> ().sprite;
			EventFinale.transform.Find ("bg/manPet").gameObject.GetComponent<Image> ().sprite = petChara1.GetComponent<SpriteRenderer> ().sprite;
			EventFinale.transform.Find ("bg/womanPet").gameObject.GetComponent<Image> ().sprite = petChara2.GetComponent<SpriteRenderer> ().sprite;
*/
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

			_posMan = 550.0f;
			_posWoman = -550.0f;

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
					if (cbMan1.nowlabel != MotionLabel.SHY3) {
						cbMan1.gotoAndPlay (MotionLabel.SHY3);
						cbWoman1.gotoAndPlay (MotionLabel.SHY3);
					}
				}

				yield return null;
			}
		}

		private IEnumerator TownCharaIdou(){
			while (_CoroutineFlagTown) {
				Vector3 _pos = EventTown.transform.Find ("Chara").gameObject.transform.localPosition;
				_pos.x -= (0.6f * (60 * Time.deltaTime));
				EventTown.transform.Find ("Chara").gameObject.transform.localPosition = _pos;
				yield return null;
			}
		}

		private IEnumerator ParkCharaIdou(){
			while(_CoroutineFlagPark){
				Vector3 _pos = EventPark.transform.Find ("Chara").gameObject.transform.localPosition;
				_pos.x += (0.075f * (60 * Time.deltaTime));
				EventPark.transform.Find ("Chara").gameObject.transform.localPosition = _pos;
				yield return null;
			}
		}

		private IEnumerator BeachCharaIdou(){
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

			_posCharaMan = EventFinale.transform.Find ("bg/manChara").gameObject.transform.localPosition;
			_posCharaWoman = EventFinale.transform.Find ("bg/womanChara").gameObject.transform.localPosition;
			_posCharaMan2 = EventFinale.transform.Find ("bg/manChara2").gameObject.transform.localPosition;
			_posCharaWoman2 = EventFinale.transform.Find ("bg/womanChara2").gameObject.transform.localPosition;
			_posPet1 = EventFinale.transform.Find ("bg/manPet").gameObject.transform.localPosition;
			_posPet2 = EventFinale.transform.Find ("bg/womanPet").gameObject.transform.localPosition;

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
			Vector3 _posWoman = new Vector3 (-57.0f, -120.0f, 0);

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

			cbMan1.gotoAndPlay (MotionLabel.KISS);
			cbWoman1.gotoAndPlay (MotionLabel.KISS);

		}



		private void TamagochiImageMove(GameObject toObj,GameObject fromObj,string toStr){
			for (int i = 0; i < fromObj.transform.Find ("Layers").transform.childCount; i++) {
				toObj.transform.Find (toStr + "/CharaImg/Layers/" + fromObj.transform.Find ("Layers").transform.GetChild (i).name).gameObject.transform.SetSiblingIndex (i);
			}

			toObj.transform.Find (toStr + "/CharaImg").gameObject.GetComponent<Image> ().enabled = false;
			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer0").gameObject.GetComponent<Image> ().enabled = true;
			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer1").gameObject.GetComponent<Image> ().enabled = true;
			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer2").gameObject.GetComponent<Image> ().enabled = true;
			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer3").gameObject.GetComponent<Image> ().enabled = true;

			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer0").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer0").gameObject.GetComponent<Image> ().sprite;
			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer1").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer1").gameObject.GetComponent<Image> ().sprite;
			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer2").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer2").gameObject.GetComponent<Image> ().sprite;
			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer3").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer3").gameObject.GetComponent<Image> ().sprite;

			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer0").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer0").gameObject.transform.localPosition;
			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer1").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer1").gameObject.transform.localPosition;
			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer2").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer2").gameObject.transform.localPosition;
			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer3").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer3").gameObject.transform.localPosition;

			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer0").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer0").gameObject.transform.localScale;
			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer1").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer1").gameObject.transform.localScale;
			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer2").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer2").gameObject.transform.localScale;
			toObj.transform.Find (toStr + "/CharaImg/Layers/Layer3").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer3").gameObject.transform.localScale;
		}

		private void TamagochiPetImageMove(GameObject toObj,GameObject fromObj,string toStr){
			for (int i = 0; i < fromObj.transform.Find ("Layers").transform.childCount; i++) {
				toObj.transform.Find (toStr + "/PetImg/Layers/" + fromObj.transform.Find ("Layers").transform.GetChild (i).name).gameObject.transform.SetSiblingIndex (i);
			}

			toObj.transform.Find (toStr + "/PetImg").gameObject.GetComponent<Image> ().enabled = false;
			toObj.transform.Find (toStr + "/PetImg/Layers/Layer").gameObject.GetComponent<Image> ().enabled = true;
			toObj.transform.Find (toStr + "/PetImg/Layers/Layer (1)").gameObject.GetComponent<Image> ().enabled = true;

			toObj.transform.Find (toStr + "/PetImg/Layers/Layer").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer").gameObject.GetComponent<Image> ().sprite;
			toObj.transform.Find (toStr + "/PetImg/Layers/Layer (1)").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer (1)").gameObject.GetComponent<Image> ().sprite;

			toObj.transform.Find (toStr + "/PetImg/Layers/Layer").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer").gameObject.transform.localPosition;
			toObj.transform.Find (toStr + "/PetImg/Layers/Layer (1)").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer (1)").gameObject.transform.localPosition;

			toObj.transform.Find (toStr + "/PetImg/Layers/Layer").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer").gameObject.transform.localScale;
			toObj.transform.Find (toStr + "/PetImg/Layers/Layer (1)").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer (1)").gameObject.transform.localScale;
		}



		private void ButtonJyunbiClick(){
			jyunbiButtonFlag = true;
		}
		private void ButtonSippaiClick(){
			ManagerObject.instance.view.change("Town");
		}
		private void ButtonSeikouClick(){
			ManagerObject.instance.view.change("Home");
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
