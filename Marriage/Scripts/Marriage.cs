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

		private Vector3 posMan1 = new Vector3(-7.0f,-1.5f,0.0f);
		private Vector3 posWoman1 = new Vector3 (-9.0f, -1.5f, 0.0f);
		private Vector3 posMan2 = new Vector3 (1.2f, 50.0f, 0.0f);
		private Vector3 posWoman2 = new Vector3 (-1.2f, 50.0f, 0.0f);
		private Vector3 posOmedetou = new Vector3 (0.0f, 0.0f, 0.0f);
		private Vector3 posPet1 = new Vector3 (0.0f, 0.0f, 0.0f);
		private Vector3 posPet2 = new Vector3 (0.0f, 0.0f, 0.0f);

		private bool startEndFlag = false;
		private int waitCount;
		private bool waitFlag = false;
		private bool screenModeFlag = false;

		private bool jyunbiButtonFlag = false;

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


			// 表示位置初期化
			posInit ();

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
				screenModeFlag = true;
			} else {
				screenModeFlag = false;
			}

			// 男の子と女の子のたまごっちをここで設定する

			cbMan1 = manChara1.GetComponent<CharaBehaviour> ();				// 男の子
			cbMan2 = manChara2.GetComponent<CharaBehaviour> ();				// 男の子の双子
			cbWoman1 = womanChara1.GetComponent<CharaBehaviour> ();			// 女の子
			cbWoman2 = womanChara2.GetComponent<CharaBehaviour> ();			// 女の子の双子
			if (mkind1 == 0) {
				yield return cbMan1.init (muser1.chara1);
				if (muser1.chara2 != null)
					yield return cbMan2.init (muser1.chara2);
			} else {
				yield return cbMan1.init (muser1.chara2);
				yield return cbMan2.init (muser1.chara1);
			}
			if (mkind2 == 0) {
				yield return cbWoman1.init (muser2.chara1);
				if (muser2.chara2 != null)
					yield return cbWoman2.init (muser2.chara2);
			} else {
				yield return cbWoman1.init (muser2.chara2);
				yield return cbWoman2.init (muser2.chara1);
			}

			// ペットをここで設定する

			pbPet1 = petChara1.GetComponent<PetBehaviour> ();				// 男の子のペット
			pbPet2 = petChara2.GetComponent<PetBehaviour> ();				// 女の子のペット
			if (muser1.pet!=null) yield return pbPet1.init (muser1.pet);
			if (muser2.pet!=null) yield return pbPet2.init (muser2.pet);



			if ((mkind1 == 0) && (muser1.chara2 == null)) {
				manChara2.transform.localScale = new Vector3 (0, 0, 0);		// 男の子の親族は出席しないので表示を消す。
			}
			if ((mkind2 == 0) && (muser2.chara2 == null)) {
				womanChara2.transform.localScale = new Vector3 (0, 0, 0);	// 女の子の親族は出席しないので表示を消す。
			}
			if (muser1.pet == null) {
				petChara1.transform.localScale = new Vector3 (0, 0, 0);		// 男の子のペットは出席しないので表示を消す。
			}
			if (muser2.pet == null) {
				petChara2.transform.localScale = new Vector3 (0, 0, 0);		// 女の子のペットは出席しないので表示を消す。
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

		private float _posMan;
		private float _posWoman;

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
//					if (startEndFlag) {
						waitCount--;
						if (waitCount == 0) {
							StartCoroutine ("WaitMain");

							jobCount = statusJobCount.marriageJobCount030;
							cbMan1.gotoAndPlay (MotionLabel.WALK);
							cbWoman1.gotoAndPlay (MotionLabel.WALK);

							//裏でBLE通信する※パラメタは設計書参照
							mBleSuccess=0;
							GameCall call = new GameCall(CallLabel.BLE_KEKKON, mkind, muser1, mkind1, muser2, mkind2);
							call.AddListener(mblekekkon);
							ManagerObject.instance.connect.send(call);
							
						}
//					}
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

					_posMan = 550.0f;
					_posWoman = -550.0f;
					break;
				}
			case	statusJobCount.marriageJobCount040:
				{
					if (EventStart.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						EventStart.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventTown.GetComponent<Transform> ().transform.localScale = new Vector3 (0.8574497f, 0.8574497f, 0.0f);
						jobCount = statusJobCount.marriageJobCount050;

						Debug.Log ("たまタウン");

						// 左向きに散歩
						cbMan1.gotoAndPlay (MotionLabel.WALK);
						cbWoman1.gotoAndPlay (MotionLabel.WALK);
						cbMan1.GetComponent<SpriteRenderer> ().flipX = false;
						cbWoman1.GetComponent<SpriteRenderer> ().flipX = false;

						posMan1.y = -3.5f;
						posWoman1.y = -3.5f;

					} else {

						EventStart.transform.Find ("bg1/manChara").gameObject.transform.localPosition = new Vector3 (_posMan, 0, 0);
						EventStart.transform.Find ("bg1/womanChara").gameObject.transform.localPosition = new Vector3 (_posWoman, 0, 0);

						_posMan -= (1.0f * (60 * Time.deltaTime));
						_posWoman += (1.0f * (60 * Time.deltaTime));

						if ((_posMan <= 200.0f)) {
							if (cbMan1.nowlabel != MotionLabel.SHY4) {
								cbMan1.gotoAndPlay (MotionLabel.SHY4);
								cbWoman1.gotoAndPlay (MotionLabel.SHY4);
							}
						}

						if (_posMan <= 55.0f) {
							_posMan = 55.0f;
							_posWoman = -55.0f;

							if (cbMan1.nowlabel != MotionLabel.IDLE) {
								cbMan1.gotoAndPlay (MotionLabel.IDLE);
								cbWoman1.gotoAndPlay (MotionLabel.IDLE);
							}
						}

						EventStart.transform.Find ("bg1/manChara").gameObject.GetComponent<Image> ().sprite = cbMan1.GetComponent<SpriteRenderer> ().sprite;
						EventStart.transform.Find ("bg1/womanChara").gameObject.GetComponent<Image> ().sprite = cbWoman1.GetComponent<SpriteRenderer> ().sprite;
						posMan1.y = -500.0f;
						posWoman1.y = -500.0f;
/*						
						if(screenModeFlag){
							posMan1.y = (EventStart.transform.Find ("bg1").gameObject.transform.localPosition.y / 57.0f) - 12.0f;
							posWoman1.y = (EventStart.transform.Find ("bg1").gameObject.transform.localPosition.y / 57.0f) - 13.0f;
						}
						else{
							posMan1.y = (EventStart.transform.Find ("bg1").gameObject.transform.localPosition.y / 47.0f) + 0.7f;
							posWoman1.y = (EventStart.transform.Find ("bg1").gameObject.transform.localPosition.y / 47.0f) + 0.7f;
						}
*/
					}
					break;
				}
			case	statusJobCount.marriageJobCount050:
				{
					if (EventTown.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						EventTown.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventPark.GetComponent<Transform> ().transform.localScale = new Vector3 (3.897114f, 3.897114f, 3.897114f);
						jobCount = statusJobCount.marriageJobCount060;

						Debug.Log ("遊園地");

						// 右向きに散歩
						cbMan1.gotoAndPlay (MotionLabel.WALK);
						cbWoman1.gotoAndPlay (MotionLabel.WALK);
						cbMan1.GetComponent<SpriteRenderer> ().flipX = true;
						cbWoman1.GetComponent<SpriteRenderer> ().flipX = true;

						posInit ();
					} else {
						posMan1.x = (man_walk0.transform.localPosition.x / 66.0f) + 1.8f - 0.8f;
						posWoman1.x = (woman_walk0.transform.localPosition.x / 66.0f) + 1.3f;
					}
					break;
				}
			case	statusJobCount.marriageJobCount060:
				{
					if (EventPark.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						EventPark.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventBeach.GetComponent<Transform> ().transform.localScale = new Vector3 (0.8574491f, 0.8574491f, 0.0f);
						jobCount = statusJobCount.marriageJobCount070;

						Debug.Log ("海岸");

						// 左向きに散歩
						cbMan1.gotoAndPlay (MotionLabel.WALK);
						cbWoman1.gotoAndPlay (MotionLabel.WALK);
						cbMan1.GetComponent<SpriteRenderer> ().flipX = false;
						cbWoman1.GetComponent<SpriteRenderer> ().flipX = false;
					} else {
						posMan1.x = (man_walk1.transform.localPosition.x / 10.0f) - 0.75f;
						posWoman1.x = (woman_walk1.transform.localPosition.x / 10.0f) - 0.75f + 2.0f;
					}
					break;
				}
			case	statusJobCount.marriageJobCount070:
				{
					if (EventBeach.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0) {
						EventBeach.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventGarden.GetComponent<Transform> ().transform.localScale = new Vector3 (1.5f, 1.5f, 0.0f);
						jobCount = statusJobCount.marriageJobCount080;

						Debug.Log ("庭園");

						// ベンチに向き合って座る
						posMan1.x = 1.0f;
						posWoman1.x = -1.0f;
						cbMan1.gotoAndPlay (MotionLabel.SIT);
						cbWoman1.gotoAndPlay (MotionLabel.SIT);
						cbMan1.GetComponent<SpriteRenderer> ().flipX = false;
						cbWoman1.GetComponent<SpriteRenderer> ().flipX = true;
					} else {
						posMan1.x = (man_walk2.transform.localPosition.x / 66.0f) + 1.8f - 0.8f;
						posWoman1.x = (woman_walk2.transform.localPosition.x / 66.0f) + 1.3f;
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

						// 結婚式場で整列
						cbMan1.gotoAndPlay (MotionLabel.IDLE);
						cbWoman1.gotoAndPlay (MotionLabel.IDLE);
						cbMan2.gotoAndPlay (MotionLabel.IDLE);
						cbWoman2.gotoAndPlay (MotionLabel.IDLE);
						pbPet1.gotoAndPlay (MotionLabel.IDLE);
						pbPet2.gotoAndPlay (MotionLabel.IDLE);
						cbMan1.GetComponent<SpriteRenderer> ().flipX = false;
						cbWoman1.GetComponent<SpriteRenderer> ().flipX = true;
						cbMan2.GetComponent<SpriteRenderer> ().flipX = false;
						cbWoman2.GetComponent<SpriteRenderer> ().flipX = true;
						pbPet1.GetComponent<SpriteRenderer> ().flipX = false;
						pbPet2.GetComponent<SpriteRenderer> ().flipX = true;

						posOmedetou = omedetouPosition.transform.localPosition;
					} else {
						if(screenModeFlag){
							posMan1.y = (man_sit.transform.localPosition.y / 44.0f) - 0.5f;
							posWoman1.y = (man_sit.transform.localPosition.y / 44.0f) - 0.5f;
						}
						else{
							posMan1.y = (man_sit.transform.localPosition.y / 43.7f) - 0.5f;
							posWoman1.y = (man_sit.transform.localPosition.y / 43.7f) - 0.5f;
						}
					}
					break;
				}
			case	statusJobCount.marriageJobCount090:
				{
					if (EventFinale.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.marriageJobCount100;

						posMan1.y = 500.0f;					// 男の子を画面外に
						posMan2.y = 500.0f;					// 女の子を画面外に
						posWoman1.y = 500.0f;				// 男の子の双子を画面外に
						posWoman2.y = 500.0f;				// 女の子の双子を画面外に

						posPet1.y = 500.0f;					// ペット１を画面外に
						posPet2.y = 500.0f;					// ペット２を画面外に
					} else {
						if (screenModeFlag) {
							posMan1.y = (man_happy.transform.localPosition.y / 51.0f) + 0.2f - 1.0f;
							posWoman1.y = (woman_happy.transform.localPosition.y / 51.0f) + 0.2f - 1.0f;
							posMan2.y = (man_happy.transform.localPosition.y / 51.0f) + 3.25f - 2.8f;
							posWoman2.y = (woman_happy.transform.localPosition.y / 51.0f) + 3.25f - 2.8f;
							posPet1.y = (man_happy.transform.localPosition.y / 51.0f) + 3.35f - 2.8f;
							posPet2.y = (woman_happy.transform.localPosition.y / 51.0f) + 3.35f - 2.8f;
						} else {
							posMan1.y = (man_happy.transform.localPosition.y / 51.0f) + 0.2f - 1.0f;
							posWoman1.y = (woman_happy.transform.localPosition.y / 51.0f) + 0.2f - 1.0f;
							posMan2.y = (man_happy.transform.localPosition.y / 51.0f) + 3.25f - 2.8f;
							posWoman2.y = (woman_happy.transform.localPosition.y / 51.0f) + 3.25f - 2.8f;
							posPet1.y = (man_happy.transform.localPosition.y / 51.0f) + 3.35f - 2.8f;
							posPet2.y = (woman_happy.transform.localPosition.y / 51.0f) + 3.35f - 2.8f;
						}
						if (posOmedetou.y != omedetouPosition.transform.localPosition.y) {
							posMan2.y += ((omedetouPosition.transform.localPosition.y - posOmedetou.y) / 50.0f);
							posWoman2.y += ((omedetouPosition.transform.localPosition.y - posOmedetou.y) / 50.0f);

							posPet1.y += ((omedetouPosition.transform.localPosition.y - posOmedetou.y) / 80.0f);
							posPet2.y += ((omedetouPosition.transform.localPosition.y - posOmedetou.y) / 80.0f);
						}
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

			manChara1.transform.localPosition = posMan1;
			manChara2.transform.localPosition = posMan2;
			womanChara1.transform.localPosition = posWoman1;
			womanChara2.transform.localPosition = posWoman2;

			petChara1.transform.localPosition = posPet1;
			petChara2.transform.localPosition = posPet2;
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

		private void posInit(){
			posMan1 = new Vector3 (-7.0f, -1.5f - 1.0f, 0.0f);
			posWoman1 = new Vector3 (-9.0f, -1.5f - 1.0f, 0.0f);
			posMan2 = new Vector3 (1.2f, 50.0f, 0.0f);
			posWoman2 = new Vector3 (-1.2f, 50.0f, 0.0f);

			posPet1 = new Vector3 (1.1f, 50.0f, 0.0f);
			posPet2 = new Vector3 (-1.1f, 50.0f, 0.0f);
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
				yield return new WaitForSeconds(0.01f);
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
