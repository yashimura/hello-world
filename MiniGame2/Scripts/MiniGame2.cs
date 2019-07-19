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

namespace Mix2App.MiniGame2{
	public class MiniGame2 : MonoBehaviour,IReceiver,IReadyable {
		[SerializeField] private GameObject[] CharaTamagoMain = null;           // たまごっち（プレイヤー）
        [SerializeField] private GameObject[] CharaTamagoNpc = null;			// たまごっち（ゲスト）
		[SerializeField] private GameObject[] CharaTamagoGuest = null;			// たまごっち（お客さん）
		[SerializeField] private GameObject MinigameRoot = null;
		[SerializeField] private GameObject EventTitle = null;					// タイトル画面
		[SerializeField] private GameObject EventStart = null;					// スタート画面
		[SerializeField] private GameObject EventGame = null;					// ゲーム画面
		[SerializeField] private GameObject EventEnd = null;					// 終了画	面
		[SerializeField] private GameObject EventResult = null;					// 結果画面
		[SerializeField] private GameObject EventItemget = null;				// アイテム入手画面
		[SerializeField] private GameObject EventHelp = null;					// 遊び方説明画面
		[SerializeField] private GameObject ButtonStart = null;					// タイトル スタートボタン
		[SerializeField] private GameObject ButtonHelp = null;					// タイトル ヘルプボタン
		[SerializeField] private GameObject ButtonClose = null;					// タイトル 閉じるボタン
		[SerializeField] private GameObject ButtonYameru = null;				// ゲーム やめるボタン
		[SerializeField] private GameObject ButtonTakuhai = null;				// アイテム入手 宅配ボタン
		[SerializeField] private GameObject ButtonTojiru = null;				// アイテム入手 閉じるボタン
		[SerializeField] private GameObject ButtonModoru = null;				// 結果 戻るボタン
		[SerializeField] private GameObject ButtonHelpModoru = null;			// 遊び方説明画面 戻るボタン
		[SerializeField] private GameObject[] ButtonMenu = null;				// ゲーム メニューボタン（８個）
		[SerializeField] private GameObject EventGameMenu = null;
		[SerializeField] private GameObject EventGameFukidashi = null;
		[SerializeField] private GameObject EventGameScore = null;
		[SerializeField] private Sprite[] MenuImage = null;						// ０：ハンバーガー、１：ホットケーキ、２：ピザ、３：ステーキ、４：パスタ、５：パフェ、６：エビフライ、７：カレー
		[SerializeField] private Sprite[] FukidashiImage = null;				// ０：吹き出し１、１：吹き出し２、２：吹き出し３、３：吹き出し４
		[SerializeField] private Sprite[] CheckImage = null;					// ０：丸、１：バツ
		[SerializeField] private Sprite[] EventEndSprite = null;                // 終了時の演出スプライト

        [SerializeField] private GameObject EventColaboShop = null;             // コラボショップタイトル画面



        private object[]		mparam;

		private CharaBehaviour[] cbCharaTamagoMain = new CharaBehaviour[2];		// プレイヤー
		private CharaBehaviour[] cbCharaTamagoNpc = new CharaBehaviour[4];		// ゲスト（最大４人）
		private CharaBehaviour[] cbCharaTamagoGuest = new CharaBehaviour[12];	// お客様
		private bool startEndFlag = false;
		private int waitCount;
		private int nowScore;													// 得点
		private int nowScore2;
		private float nowTime1;													// 残り時間（カウントダウン用）
		private int	nowTime2;													// 残り時間（制限時間）
		private bool[] NpcDispFlag = new bool[4];
		private TamaChara[] NpcBaseTamaChara = new TamaChara[4];
		private bool futagoFlag;
        private int GuestMaxNumber;

		private readonly float MENU_IDOU_SPEED = 30.0f;							// メニューの移動速度
		private readonly int GAME_PLAY_TIME = 60;								// ゲームプレイ制限時間
		private readonly int GAME_SCORE_POINT1 = 10;							// ２択時の得点
		private readonly int GAME_SCORE_POINT2 = 20;							// ４択時の得点
		private readonly int GAME_SCORE_POINT3 = 50;							// ６択時の得点
		private readonly int GAME_SCORE_POINT4 = 100;							// ８択時の得点

		private readonly int GAME_MENU_DIFFICULTY1 = 3;							// ４択メニューに切り替える正解数
		private readonly int GAME_MENU_DIFFICULTY2 = 6;							// ６択メニューに切り替える正解数	
		private readonly int GAME_MENU_DIFFICULTY3 = 9;							// ８択メニューに切り替える正解数

		private readonly int GAME_CLEAR_MESSAGE1 = 100;							// 得点未満でゲームクリアメッセージ１を表示
		private readonly int GAME_CLEAR_MESSAGE2 = 400;							// 得点未満でゲームクリアメッセージ２を表示
		private readonly int GAME_CLEAR_MESSAGE3 = 700;							// 得点未満でゲームクリアメッセージ３を表示
		private readonly int GAME_CLEAR_MESSAGE4 = 800;							// 得点未満でゲームクリアメッセージ４を表示



		private statusJobCount	jobCount = statusJobCount.minigame2JobCount000;
		private enum statusJobCount{
			minigame2JobCount000,
			minigame2JobCount010,
			minigame2JobCount020,
			minigame2JobCount030,
			minigame2JobCount040,
			minigame2JobCount050,
			minigame2JobCount060,
			minigame2JobCount070,
			minigame2JobCount080,
			minigame2JobCount090,
			minigame2JobCount100,
			minigame2JobCount110,
			minigame2JobCount120,
		}

		private User muser1;//自分
        private string mSceneLabel;
        private int mMinigameID;


		void Awake(){
			Debug.Log ("MiniGame2 Awake");

			mparam=null;
			muser1=null;

			mready = false;

			NpcDispFlag [0] = false;
			NpcDispFlag [1] = false;
			NpcDispFlag [2] = false;
			NpcDispFlag [3] = false;

			futagoFlag = false;
		}

		public void receive(params object[] parameter){
			Debug.Log ("MiniGame2 receive");
			mparam = parameter;
			if (mparam==null) {
                mparam = new object[] {
                    "Town",
                    3
                };
			}

            mSceneLabel = (string)mparam[0];
            mMinigameID = (int)mparam[1];

            GameCall call = new GameCall (CallLabel.GET_MINIGAME_INFO,mMinigameID);
			call.AddListener (mGetMinigameInfo);
			ManagerObject.instance.connect.send (call);
		}

		private bool mready = false;
		public bool ready(){
			return mready;
		}

		void Start() {
			Debug.Log ("MiniGame2 Start");
		}

		private MinigameData mData;
		private MinigameResultData mResultData;
		void mGetMinigameInfo(bool success,object data){
			Debug.Log(success + "/" + data);
			if (success) {
				mData = (MinigameData)data;
				StartCoroutine (mStart ());
			} else {
				if ((int)data == 4) {
                    MinigameRoot.transform.Find("bg").gameObject.SetActive(true);
                    mready = true;
					ManagerObject.instance.view.dialog ("alert", new object[]{ "minigame2",(int)data}, mGetMinigameInfoCallBack);
				}
			}
		}
		private void mGetMinigameInfoCallBack(int num){
            if (mSceneLabel == "PapaMama")
            {
                Debug.Log("パパママモードへ・・・");
                ManagerObject.instance.view.change(SceneLabel.PAPA_MAMA);
            }
            else
            {
                Debug.Log("たまタウンへ・・・");
                ManagerObject.instance.view.change(SceneLabel.TOWN);
            }
        }


        IEnumerator mStart(){
			Debug.Log ("MiniGame2 mStart");

			muser1 = ManagerObject.instance.player;     // たまごっち



            EventSpriteSet();

            float kk1 = 1024f / 2048f;
            float kk2 = (float)Screen.height / (float)Screen.width;
            float kk3 = (kk2 < kk1) ? kk2 / kk1 : 1.0f;
            EventColaboShop.transform.localScale = new Vector3(kk3, kk3, 1);


            startEndFlag = false;

            EventColaboShop.transform.Find("Button_red_b_game").gameObject.GetComponent<Button>().onClick.AddListener(ButtonStartClick);
            EventColaboShop.transform.Find("Button_blue_close").gameObject.GetComponent<Button>().onClick.AddListener(ButtonCloseClick);
            EventColaboShop.transform.Find("Button_red_b_change").gameObject.GetComponent<Button>().onClick.AddListener(ButtonChangeClick);
            EventColaboShop.transform.Find("Button_red_help").gameObject.GetComponent<Button>().onClick.AddListener(ButtonHelpClick);


			ButtonStart.GetComponent<Button> ().onClick.AddListener (ButtonStartClick);
			ButtonClose.GetComponent<Button> ().onClick.AddListener (ButtonCloseClick);
			ButtonHelp.GetComponent<Button> ().onClick.AddListener (ButtonHelpClick);
			ButtonHelpModoru.GetComponent<Button> ().onClick.AddListener (ButtonHelpModoruClick);
			ButtonYameru.GetComponent<Button> ().onClick.AddListener (ButtonYameruClick);
			ButtonTakuhai.GetComponent<Button> ().onClick.AddListener (ButtonTakuhaiClick);
			ButtonTojiru.GetComponent<Button> ().onClick.AddListener (ButtonTojiruClick);
			ButtonModoru.GetComponent<Button> ().onClick.AddListener (ButtonModoruClick);

			ButtonMenu [0].GetComponent<Button> ().onClick.AddListener (ButtonMenu0Click);
			ButtonMenu [1].GetComponent<Button> ().onClick.AddListener (ButtonMenu1Click);
			ButtonMenu [2].GetComponent<Button> ().onClick.AddListener (ButtonMenu2Click);
			ButtonMenu [3].GetComponent<Button> ().onClick.AddListener (ButtonMenu3Click);
			ButtonMenu [4].GetComponent<Button> ().onClick.AddListener (ButtonMenu4Click);
			ButtonMenu [5].GetComponent<Button> ().onClick.AddListener (ButtonMenu5Click);
			ButtonMenu [6].GetComponent<Button> ().onClick.AddListener (ButtonMenu6Click);
			ButtonMenu [7].GetComponent<Button> ().onClick.AddListener (ButtonMenu7Click);


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
            if ((num > 1.33f) && (num < 1.34f))
            {   // 3:4の時のみ画面を拡大表示
                EventGame.GetComponent<Transform>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            else
            {   // 3:4の時以外の画面を拡大表示
                EventGame.GetComponent<Transform>().transform.localScale = new Vector3(0.95f, 0.95f, 1.0f);
            }

            if (muser1.chara2 == null) {
				futagoFlag = false;
			} else {
				futagoFlag = true;
			}


			for (int i = 0; i < CharaTamagoMain.Length; i++) {
				cbCharaTamagoMain [i] = CharaTamagoMain [i].GetComponent<CharaBehaviour> ();	// プレイヤーキャラ
			}
			for (int i = 0; i < CharaTamagoNpc.Length; i++) {
				cbCharaTamagoNpc [i] = CharaTamagoNpc [i].GetComponent<CharaBehaviour> ();		// NPCキャラ
			}
			for (int i = 0; i < CharaTamagoGuest.Length; i++) {
				cbCharaTamagoGuest [i] = CharaTamagoGuest [i].GetComponent<CharaBehaviour> ();	// お客キャラ
			}
			if ((muser1.utype == UserType.MIX) || (muser1.utype == UserType.MIX2) || (mSceneLabel != "PapaMama")) {
				// 玩具連動キャラ
				yield return cbCharaTamagoMain [0].init (muser1.chara1);						// プレイヤーキャラを登録する
				if (futagoFlag) {
					yield return cbCharaTamagoMain [1].init (muser1.chara2);					// プレイヤーキャラ（双子）を登録する
				}
			} else {
				// 玩具非連動キャラ
				yield return cbCharaTamagoMain [0].init (mData.charaList[0]);	                // プレイヤーキャラを登録する
				futagoFlag = false;
			}
			TamagochiMainAnimeSet (0, MotionLabel.IDLE);
			TamagochiMainAnimeSet (1, MotionLabel.IDLE);
           
            // お客さんの参加数を登録する
            GuestMaxNumber = mData.charaList.Count - 1;
            if(GuestMaxNumber < 10)
            {
                Debug.LogError("お客さんの参加人数が足りない");
            }

            for (int i = 0; i < cbCharaTamagoGuest.Length && i < GuestMaxNumber; i++) {                               // お客キャラを登録する
                yield return cbCharaTamagoGuest[i].init(mData.charaList[i + 1]);
                TamagochiGuestAnimeSet(i, MotionLabel.IDLE);
			}

			StartCoroutine ("TamagochiSortLoop");

            ManagerObject.instance.sound.playBgm(22);
            MinigameRoot.transform.Find("bg").gameObject.SetActive(true);

            startEndFlag = true;
			mready = true;
		}

		void Destroy(){
			Debug.Log ("MiniGame2 Destroy");
			StopCoroutine ("TamagochiSortLoop");
		}
		void OnDestroy(){
			Debug.Log ("MiniGame2 OnDestroy");
			StopCoroutine ("TamagochiSortLoop");
		}

		void Update(){
			switch (jobCount) {
			case	statusJobCount.minigame2JobCount000:
				{
					if (startEndFlag) {
                        EventTitleDisp(true);
						jobCount = statusJobCount.minigame2JobCount010;
						TamagoCharaPositionInit ();
						nowScore = 0;
						nowScore2 = 0;
						nowTime1 = 0.0f;
						nowTime2 = GAME_PLAY_TIME;									// 制限時間

						EventGameScore.SetActive (false);

					}
					break;
				}
			case	statusJobCount.minigame2JobCount010:
				{
					break;
				}
			case	statusJobCount.minigame2JobCount020:
				{
                    EventTitleDisp(false);
					jobCount = statusJobCount.minigame2JobCount030;
					StartCoroutine (tamagochiCharaInit (statusJobCount.minigame2JobCount040));
					break;
				}
			case	statusJobCount.minigame2JobCount030:
				{
					break;
				}
			case	statusJobCount.minigame2JobCount040:
				{
					EventStart.SetActive (true);
					TamagochiLoopInit ();
					jobCount = statusJobCount.minigame2JobCount050;

					ManagerObject.instance.sound.playSe (20);
					break;
				}
			case	statusJobCount.minigame2JobCount050:
				{
					if (EventStart.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.minigame2JobCount060;
						EventStart.SetActive (false);
						EventGame.SetActive (true);
						GameMainInit ();

//						ManagerObject.instance.sound.playBgm (22);
					}
					break;
				}
			case	statusJobCount.minigame2JobCount060:
				{
					if (GameMainLoop ()) {											// ゲーム処理
						jobCount = statusJobCount.minigame2JobCount070;
						waitCount = 45;

						waitResultFlag = false;
						GameCall call = new GameCall (CallLabel.GET_MINIGAME_RESULT,mData,nowScore);
						call.AddListener (mGetMinigameResult);
						ManagerObject.instance.connect.send (call);
					}
					break;
				}
			case	statusJobCount.minigame2JobCount070:
				{
					if (waitResultFlag) {
						waitCount--;
					}
					if (waitCount == 0) {											// 驚きを見せるためのウエィト
						jobCount = statusJobCount.minigame2JobCount080;

						int _num = 0;
						if (nowScore < GAME_CLEAR_MESSAGE1) {
							_num = 0;
						} else if (nowScore < GAME_CLEAR_MESSAGE2) {
							_num = 1;
						} else if (nowScore < GAME_CLEAR_MESSAGE3) {
							_num = 2;
						} else if (nowScore < GAME_CLEAR_MESSAGE4) {
							_num = 3;
						} else {
							_num = 4;
						}
						EventEnd.transform.Find ("gameend").transform.GetComponent<Image> ().sprite = EventEndSprite [_num];

						EventGame.SetActive (false);
						EventEnd.SetActive (true);
					}
					break;
				}
			case	statusJobCount.minigame2JobCount080:
				{
					if (EventEnd.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {	// お疲れ様のアニメが終了するまで待つ
						jobCount = statusJobCount.minigame2JobCount090;
						EventEnd.SetActive (false);
						EventResult.SetActive (true);

						for (int i = 0; i < CharaTamagoGuest.Length; i++) {			// お客さん達を消す
							CharaTamagoGuest [i].transform.localPosition = new Vector3 (5000.0f, 5000.0f, 0.0f);
						}
						for (int i = 0; i < CharaTamagoMain.Length; i++) {			// プレイヤーを消す
							CharaTamagoMain [i].transform.localPosition = new Vector3 (5000.0f, 5000.0f, 0.0f);
							TamagochiMainAnimeSet (i, MotionLabel.IDLE);
						}
						for (int i = 0; i < CharaTamagoNpc.Length; i++) {			// ゲストを消す
							CharaTamagoNpc [i].transform.localPosition = new Vector3 (5000.0f, 5000.0f, 0.0f);
							TamagochiNpcAnimeSet (i, MotionLabel.IDLE);
						}

						resultLoopCount = statusResult.resultJobCount000;
						resultMainLoopFlag = false;
						ResultMainLoop ();											// 結果画面処理
						TamagoAnimeSprite ();										// たまごっちのアニメをImageに反映する
					}
					break;
				}
			case	statusJobCount.minigame2JobCount090:
				{
					if (ResultMainLoop ()) {										// 結果画面処理
						jobCount = statusJobCount.minigame2JobCount000;
						EventResult.SetActive (false);
                        EventTitleDisp(true);
					}
					TamagoAnimeSprite ();											// たまごっちのアニメをImageに反映する
					break;
				}
			case	statusJobCount.minigame2JobCount100:
				{
					break;
				}
			case	statusJobCount.minigame2JobCount110:
				{
					break;
				}
			case	statusJobCount.minigame2JobCount120:
				{
					break;
				}
			}
		}


		private bool waitResultFlag;
		private void mGetMinigameResult(bool success,object data){
			mResultData = (MinigameResultData)data;
			waitResultFlag = true;
		}


		private void ButtonStartClick(){
			jobCount = statusJobCount.minigame2JobCount020;							// スタートボタンが押されたのでゲーム開始

			ManagerObject.instance.sound.playSe (11);
		}
		private void ButtonCloseClick(){
			ManagerObject.instance.sound.playSe (17);
            if(mSceneLabel == "PapaMama"){
                Debug.Log("パパママモードへ・・・");
                ManagerObject.instance.view.change(SceneLabel.PAPA_MAMA);
            } else {
                Debug.Log("たまタウンへ・・・");
                ManagerObject.instance.view.change(SceneLabel.TOWN);
            }
        }
        private void ButtonChangeClick()
        {
            ManagerObject.instance.sound.playSe(11);
            Debug.Log("ポイントショップへ・・・");
            ManagerObject.instance.view.change(SceneLabel.POINT_SHOP, mMinigameID);
        }

        private void ButtonHelpClick(){
			ManagerObject.instance.sound.playSe (11);
			ManagerObject.instance.view.dialog("webview",new object[]{"minigame2", mMinigameID},null);
		}
		private void ButtonHelpModoruClick(){
			EventHelp.SetActive (false);

			ManagerObject.instance.sound.playSe (17);
		}
		private void ButtonYameruClick(){
			gameMainLoopFlag = true;

			ManagerObject.instance.sound.playSe (17);
		}
		private void ButtonTakuhaiClick(){
			Debug.Log ("プレゼントボックスへ・・・");
			ManagerObject.instance.sound.playSe (17);
			ManagerObject.instance.view.change(SceneLabel.ITEMBOX);
		}
		private void ButtonTojiruClick(){
			resultItemGetFlag = true;

			ManagerObject.instance.sound.playSe (17);
		}
		private void ButtonModoruClick(){
			resultMainLoopFlag = true;												// タイトルにもどる

			ManagerObject.instance.sound.playSe (17);
		}

		private void ButtonMenu0Click(){
			_buttonMenuClick (0);
		}
		private void ButtonMenu1Click(){
			_buttonMenuClick (1);
		}
		private void ButtonMenu2Click(){
			_buttonMenuClick (2);
		}
		private void ButtonMenu3Click(){
			_buttonMenuClick (3);
		}
		private void ButtonMenu4Click(){
			_buttonMenuClick (4);
		}
		private void ButtonMenu5Click(){
			_buttonMenuClick (5);
		}
		private void ButtonMenu6Click(){
			_buttonMenuClick (6);
		}
		private void ButtonMenu7Click(){
			_buttonMenuClick (7);
		}

		private int buttonNumber;
		private void _buttonMenuClick(int Number){
			if (buttonCheckFlag) {
				buttonCheckFlag = false;
				buttonNumber = Number;
			}
		}

		private void TamagochiLoopInit(){
			Vector2[] _initTableNpc = new Vector2[] {
				new Vector2 (-450.0f, 370.0f),
				new Vector2 ( 450.0f, 370.0f),
				new Vector2 (-700.0f, 370.0f),
				new Vector2 ( 700.0f, 370.0f),
			};

			for (int i = 0; i < CharaTamagoGuest.Length; i++) {
				CharaTamagoGuest [i].SetActive (true);
			}

			for (int i = 0; i < CharaTamagoMain.Length; i++) {
				CharaTamagoMain [i].SetActive (true);
				TamagochiMainAnimeSet (i, MotionLabel.IDLE);
				Vector3 pos = CharaTamagoMain [i].transform.localPosition;
				if (futagoFlag) {
					if (i == 0) {
						pos.x = -110.0f;
					} else {
						pos.x = 110.0f;
					}
				} else {
					pos.x = 5000.0f * i;
				}
				pos.y = 370.0f;
				CharaTamagoMain [i].transform.localPosition = pos;
			}

			for (int i = 0; i < CharaTamagoNpc.Length; i++) {
				CharaTamagoNpc [i].SetActive (true);
				TamagochiNpcAnimeSet (i, MotionLabel.IDLE);
				Vector3 pos = CharaTamagoNpc [i].transform.localPosition;
				pos.x = _initTableNpc [i].x;
				pos.y = _initTableNpc [i].y;
				CharaTamagoNpc [i].transform.localPosition = pos;

				if (!NpcDispFlag [i]) {
					CharaTamagoNpc [i].transform.localPosition = new Vector3 (5000, 5000, 0);
				}
			}

			for (int i = 0; i < 12; i++) {
				StartCoroutine (TamagochiStartPositionSet (i, i + 7));
			}
		}

		private IEnumerator TamagochiStartPositionSet(int num,int num2){
			Vector3[] _idouPosTable = new Vector3[]{
				new Vector3(260.0f,-250.0f,0.0f),
				new Vector3(510.0f,-250.0f,0.0f),
				new Vector3(760.0f,-250.0f,0.0f),
				new Vector3(760.0f,-550.0f,0.0f),
				new Vector3(510.0f,-550.0f,0.0f),
				new Vector3(260.0f,-550.0f,0.0f),
				new Vector3(10.0f,-550.0f,0.0f),
				new Vector3(-240.0f,-550.0f,0.0f),		// 0	
				new Vector3(-490.0f,-850.0f,0.0f),
				new Vector3(-740.0f,-1150.0f,0.0f),
				new Vector3(-990.0f,-1450.0f,0.0f),
				new Vector3(-1240.0f,-1750.0f,0.0f),
				new Vector3(-1490.0f,-2050.0f,0.0f),
				new Vector3(-1740.0f,-2350.0f,0.0f),
				new Vector3(-1990.0f,-2650.0f,0.0f),
				new Vector3(-2240.0f,-2950.0f,0.0f),
				new Vector3(-2490.0f,-3250.0f,0.0f),
				new Vector3(-2740.0f,-3550.0f,0.0f),
				new Vector3(-2990.0f,-3850.0f,0.0f),
				new Vector3(-3240.0f,-4150.0f,0.0f),
			};

			CharaTamagoGuest [num].transform.localPosition = _idouPosTable [num2 + 1];
			for (int i = 0; i < 8; i++) {
				while (true) {
					TamagochiAnimeFlipSet (num, MotionLabel.WALK, _idouPosTable [num2 - i]);

					CharaTamagoGuest [num].transform.localPosition = Vector3.MoveTowards (CharaTamagoGuest [num].transform.localPosition, _idouPosTable [num2 - i], 700 * Time.deltaTime);
					if ((CharaTamagoGuest [num].transform.localPosition.x == _idouPosTable [num2 - i].x) && (CharaTamagoGuest [num].transform.localPosition.y == _idouPosTable [num2 - i].y)) {
						break;
					}
					yield return null;
				}
			}
			TamagochiGuestAnimeSet (num, MotionLabel.IDLE, false);
		}



		private Vector3[] _NextPos = new Vector3[12];
		private int _NextCounter;
		private bool _NextFlag;
		private IEnumerator TamagochiNextIdou(){
			_NextPos [tamagochiIdouTable [0]] = new Vector3 (-500.0f, -250.0f, 0.0f);

			for (int i = 1; i < GuestMaxNumber; i++) {
				_NextPos [i] = CharaTamagoGuest [tamagochiIdouTable [i - 1]].transform.localPosition;
			}

			StartCoroutine(TamagoNextIdou0(tamagochiIdouTable[0],_NextPos[0]));
			for (int i = 1; i < GuestMaxNumber; i++) {
				StartCoroutine (TamagoNextIdou (tamagochiIdouTable [i], _NextPos [i]));
			}

			yield return null;
		}

		private IEnumerator TamagoNextIdou0(int num,Vector3 pos){
			while(true){
				if (gameMainLoopFlag) {
					break;
				}
				TamagochiAnimeFlipSet (num, MotionLabel.WALK, pos);
				CharaTamagoGuest [num].transform.localPosition = Vector3.MoveTowards (CharaTamagoGuest [num].transform.localPosition, pos, 700 * Time.deltaTime);
				if((CharaTamagoGuest[num].transform.localPosition.x == pos.x) && (CharaTamagoGuest[num].transform.localPosition.y == pos.y)){
					break;
				}
				yield return null;
			}
			_NextCounter++;
			Vector3 pos2 = new Vector3 (-1000.0f, -1500.0f, 0.0f);
			while(true){
				if (gameMainLoopFlag) {
					break;
				}
				TamagochiAnimeFlipSet (num, MotionLabel.WALK, pos2);
				CharaTamagoGuest [num].transform.localPosition = Vector3.MoveTowards (CharaTamagoGuest [num].transform.localPosition, pos2, 700 * Time.deltaTime);
				if((CharaTamagoGuest[num].transform.localPosition.x == pos2.x) && (CharaTamagoGuest[num].transform.localPosition.y == pos2.y)){
					break;
				}
				yield return null;
			}
			TamagochiGuestAnimeSet (num, MotionLabel.IDLE);
		}
		private IEnumerator TamagoNextIdou(int num,Vector3 pos){
			while(true){
				if (gameMainLoopFlag) {
					break;
				}
				TamagochiAnimeFlipSet (num, MotionLabel.WALK, pos);
				CharaTamagoGuest [num].transform.localPosition = Vector3.MoveTowards (CharaTamagoGuest [num].transform.localPosition, pos, 700 * Time.deltaTime);
				if((CharaTamagoGuest[num].transform.localPosition.x == pos.x) && (CharaTamagoGuest[num].transform.localPosition.y == pos.y)){
					break;
				}
				yield return null;
			}
			_NextCounter++;
			TamagochiGuestAnimeSet (num, MotionLabel.IDLE);
		}


		// たまごっちの表示優先順位を変更する
		private IEnumerator TamagochiSortLoop(){
			float[] _posY = new float[12];

			while (true) {
				for (int i = 0; i < 12; i++) {
					_posY [i] = CharaTamagoGuest [i].transform.localPosition.y;
				}

				for (int j = 0; j < 12; j++) {								// たまごっちの表示優先順位の変更
					int k = 0;
					float _checkPos = -1000.0f;
					for (int i = 0; i < 12; i++) {
						if (_checkPos <= _posY [i]) {
							_checkPos = _posY [i];
							k = i;
						}
					}
					_posY [k] = -1000.0f;
					CharaTamagoGuest [k].GetComponent<Canvas> ().sortingOrder = j + 1;
				}

				yield return null;
			}
		}



		private int[] tamagochiIdouTable = new int[12];
		private void TamagochiGameIdouInit(){
			for (int i = 0; i < GuestMaxNumber; i++) {
				tamagochiIdouTable [i] = i;
			}
			_NextCounter = 0;
			_NextFlag = false;
		}
		private bool TamagochiGameIdouLoop(){
			if (!_NextFlag) {
				StartCoroutine ("TamagochiNextIdou");
				_NextFlag = true;
			}

			if (_NextCounter == GuestMaxNumber) {
				int _hzn = tamagochiIdouTable [0];
				for (int num = 0; num < GuestMaxNumber - 1; num++) {
					tamagochiIdouTable [num] = tamagochiIdouTable [num + 1];
				}
				tamagochiIdouTable [GuestMaxNumber - 1] = _hzn;

				_NextCounter = 0;
				_NextFlag = false;
				return true;
			}
			return false;
		}

		private void TamagochiMainAnimeSet(int num,string status){
			string _status = status;
			switch(_status){
			case	MotionLabel.GLAD1:
				{
					switch (Random.Range (0, 3)) {
					case	0:
						{
							_status = MotionLabel.GLAD1;
							break;
						}
					case	1:
						{
							_status = MotionLabel.GLAD2;
							break;
						}
					default:
						{
							_status = MotionLabel.GLAD3;
							break;
						}
					}
					break;
				}
			}

			if (cbCharaTamagoMain [num].nowlabel != _status) {
				cbCharaTamagoMain [num].gotoAndPlay (_status);
			}
		}
		private void TamagochiNpcAnimeSet(int num,string status){
			if (!NpcDispFlag [num]) {
				return;
			}

			string _status = status;
            switch (_status)
            {
                case MotionLabel.GLAD1:
                    {
                        if (mSceneLabel == "PapaMama")
                        {
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    {
                                        _status = MotionLabel.GLAD1;
                                        break;
                                    }
                                case 1:
                                    {
                                        _status = MotionLabel.GLAD2;
                                        break;
                                    }
                                default:
                                    {
                                        _status = MotionLabel.GLAD3;
                                        break;
                                    }
                            }
                        }
                        break;
                    }
            }

			if (cbCharaTamagoNpc [num].nowlabel != _status) {
				cbCharaTamagoNpc [num].gotoAndPlay (_status);
			}
		}
		private void TamagochiGuestAnimeSet(int num,string status,bool flag = false){
			string _status = status;

            switch (_status)
            {
                case MotionLabel.GLAD1:
                    {
                        if (mSceneLabel == "PapaMama")
                        {
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    {
                                        _status = MotionLabel.GLAD1;
                                        break;
                                    }
                                case 1:
                                    {
                                        _status = MotionLabel.GLAD2;
                                        break;
                                    }
                                default:
                                    {
                                        _status = MotionLabel.GLAD3;
                                        break;
                                    }
                            }
                        }
                        break;
                    }
            }

			if (cbCharaTamagoGuest [num].nowlabel != _status) {
				cbCharaTamagoGuest [num].gotoAndPlay (_status);
			}
			if (flag) {
				CharaTamagoGuest [num].transform.localScale = new Vector3 (-6.0f, 6.0f, 1.0f);
			} else {
				CharaTamagoGuest [num].transform.localScale = new Vector3 (6.0f, 6.0f, 1.0f);
			}
		}
		private void TamagochiAnimeFlipSet(int _num,string _anime,Vector3 pos){
			if (CharaTamagoGuest [_num].transform.localPosition.x >= pos.x) {
				TamagochiGuestAnimeSet (_num, _anime, false);
			} else {
				TamagochiGuestAnimeSet (_num, _anime, true);
			}
		}


		private IEnumerator tamagochiCharaInit (statusJobCount flag){
			NpcDispFlag [0] = false;
			NpcDispFlag [1] = false;
			NpcDispFlag [2] = false;
			NpcDispFlag [3] = false;

			// 背景にキャラを表示するかどうかはmData.eventUserListのCountで判断する
			// ｍData.eventIdsによる判定は不要
			if (mData.eventUserList != null && mData.eventUserList.Count > 0) {
				int[] _a = new int[2]{ 0, 1 };
				if (mData.eventUserList.Count > 2) {
					_a [0] = Random.Range (0, mData.eventUserList.Count);
					_a [1] = Random.Range (0, mData.eventUserList.Count);
					if (_a [0] == _a [1]) {
						_a [1]++;
						if (_a [1] == mData.eventUserList.Count) {
							_a [1] = 0;
						}
					}
				}
				// mData.eventUserList.Countも条件判定に加える
				for (int i = 0; i < 2 && i < mData.eventUserList.Count; i++) {
					NpcDispFlag [i] = true;
					NpcBaseTamaChara [i] = mData.eventUserList [_a [i]].chara1;
					if (mData.eventUserList [_a [i]].chara2 != null) {
						NpcDispFlag [i + 2] = true;
						NpcBaseTamaChara [i + 2] = mData.eventUserList [_a [i]].chara2;
					}
				}
			}

			yield return null;

			for (int i = 0; i < 4; i++) {
				if (NpcDispFlag [i]) {
					yield return cbCharaTamagoNpc [i].init (NpcBaseTamaChara [i]);	// 応援キャラを登録する
					TamagochiNpcAnimeSet (i, MotionLabel.IDLE);
				}
			}

			jobCount = flag;
		}



		private Vector3 posMenu = new Vector3 (0, 0, 0);	// メニュー一覧の表示位置
		private int seikaiCount;							// 正解数
		private int menuCount;								// 表示メニュー数
		private bool buttonCheckFlag;						// メニュー一覧のボタンの有効・無効
		private bool gameMainLoopFlag = false;				// ゲーム処理フラグ

		private statusGameCount gameJobCount;
		private enum statusGameCount{
			minigame2GameCount000,
			minigame2GameCount010,
			minigame2GameCount020,
			minigame2GameCount030,
			minigame2GameCount040,
			minigame2GameCount050,
			minigame2GameCount060,
			minigame2GameCount070,
			minigame2GameCount080,
			minigame2GameCount090,
			minigame2GameCount100,
		}

		private void GameMainInit(){
            for (int i = 0; i < 8; i++)
            {
                ButtonMenu[i].SetActive(false);
                ButtonMenu[i].transform.localScale = new Vector3(4, 4, 1);
            }
            posMenu.x = 1500.0f;
			EventGameMenu.transform.localPosition = posMenu;
			seikaiCount = 0;
			menuCount = 2;
			gameJobCount = statusGameCount.minigame2GameCount000;
			buttonCheckFlag = false;
			TamagochiGameIdouInit ();
			gameMainLoopFlag = false;
			fukidashiOff ();
			for (int i = 0; i < CharaTamagoMain.Length; i++) {
				TamagochiMainAnimeSet (i, MotionLabel.IDLE);
			}
			for (int i = 0; i < CharaTamagoNpc.Length; i++) {
				TamagochiNpcAnimeSet (i, MotionLabel.IDLE);
			}
		}

		private bool GameMainLoop(){
			switch (gameJobCount) {
			case	statusGameCount.minigame2GameCount000:
				{
					for (int i = 0; i < 8; i++) {
						ButtonMenu [i].SetActive (false);
						ButtonMenu [i].transform.localScale = new Vector3 (4, 4, 1);
					}

					switch (menuCount) {
					case	2:
						{
							ButtonMenu [1].SetActive (true);	// プリン
							ButtonMenu [6].SetActive (true);	// ご飯
							break;
						}
					case	4:
						{
							ButtonMenu [1].SetActive (true);	// プリン
							ButtonMenu [2].SetActive (true);	// サンド
							ButtonMenu [5].SetActive (true);	// オムレツ
							ButtonMenu [6].SetActive (true);	// ご飯
							break;
						}
					case	6:
						{
							ButtonMenu [0].SetActive (true);	// カツ丼
							ButtonMenu [1].SetActive (true);	// プリン
							ButtonMenu [2].SetActive (true);	// サンド
							ButtonMenu [5].SetActive (true);	// オムレツ
							ButtonMenu [6].SetActive (true);	// ご飯
							ButtonMenu [7].SetActive (true);	// 寿司
							break;
						}
					default:
						{
							for (int i = 0; i < 8; i++) {		// 3:ステーキ、4:パスタ
								ButtonMenu [i].SetActive (true);
							}
							break;
						}
					}
					posMenu.x = 1500.0f;
					gameJobCount = statusGameCount.minigame2GameCount010;

					ManagerObject.instance.sound.playSe (27);
					break;
				}
			case	statusGameCount.minigame2GameCount010:
				{
					posMenu.x -= (MENU_IDOU_SPEED * (60 * Time.deltaTime));
					if (posMenu.x <= 0.0f) {
						posMenu.x = 0.0f;
						gameJobCount = statusGameCount.minigame2GameCount020;
						fukidashiInit ();
						buttonCheckFlag = true;
					}
					break;
				}
			case	statusGameCount.minigame2GameCount020:
				{
					if(!buttonCheckFlag){
						if (fukidashiHantei ()) {
							gameJobCount = statusGameCount.minigame2GameCount030;
							for (int i = 0; i < CharaTamagoMain.Length; i++) {
								TamagochiMainAnimeSet (i, MotionLabel.GLAD1);		// 正解したので喜ぶ（プレイヤー）
							}
							for (int i = 0; i < CharaTamagoNpc.Length; i++) {
								TamagochiNpcAnimeSet (i, MotionLabel.GLAD1);		// 正解したので喜ぶ（NPC）
							}
							TamagochiGuestAnimeSet (tamagochiIdouTable[0], MotionLabel.GLAD1);	// 正解したので喜ぶ（注文したお客）
							ScoreInit ();
						} else {
							gameJobCount = statusGameCount.minigame2GameCount100;
							for (int i = 0; i < CharaTamagoMain.Length; i++) {
								TamagochiMainAnimeSet (i, MotionLabel.SHOCK);		// 誤答をしたので驚く（プレイヤー）
							}
						}
					}
					break;
				}
			case	statusGameCount.minigame2GameCount030:
				{
					if (ScoreAnime ()) {
						gameJobCount = statusGameCount.minigame2GameCount040;
						fukidashiOff ();
						for (int i = 0; i < CharaTamagoMain.Length; i++) {
							TamagochiMainAnimeSet (i, MotionLabel.IDLE);
						}
						for (int i = 0; i < CharaTamagoNpc.Length; i++) {
							TamagochiNpcAnimeSet (i, MotionLabel.IDLE);
						}
					}
					break;
				}
			case	statusGameCount.minigame2GameCount040:
				{
//					posMenu.x -= (MENU_IDOU_SPEED * (60 * Time.deltaTime));
//					if (posMenu.x <= -1500.0f) {
						posMenu.x = -1500.0f;
						gameJobCount = statusGameCount.minigame2GameCount050;
//					}
					break;
				}
			case	statusGameCount.minigame2GameCount050:
				{
					if (TamagochiGameIdouLoop()) {
						gameJobCount = statusGameCount.minigame2GameCount060;
					}
					break;
				}
			case	statusGameCount.minigame2GameCount060:
				{
					seikaiCount++;
					if (seikaiCount == GAME_MENU_DIFFICULTY1) {
						menuCount = 4;
					}
					if (seikaiCount == GAME_MENU_DIFFICULTY2) {
						menuCount = 6;
					}
					if (seikaiCount == GAME_MENU_DIFFICULTY3) {
						menuCount = 8;
					}

					gameJobCount = statusGameCount.minigame2GameCount000;
					break;
				}
			case	statusGameCount.minigame2GameCount070:
				{
					break;
				}
			case	statusGameCount.minigame2GameCount080:
				{
					break;
				}
			case	statusGameCount.minigame2GameCount090:
				{
					break;
				}
			case	statusGameCount.minigame2GameCount100:
				{
					gameMainLoopFlag = true;
					break;
				}
			}

			nowTime1 += 1.0f * Time.deltaTime;
			if (nowTime1 >= 1.0f) {
				nowTime1 -= 1.0f;
				if (nowTime2 == 0) {
					gameMainLoopFlag = true;
				} else {
					nowTime2--;
				}
			}

			EventGameMenu.transform.localPosition = posMenu;
			EventGame.transform.Find ("points/Text").gameObject.GetComponent<Text> ().text = nowScore.ToString ();
			EventResult.transform.Find ("points/Text").gameObject.GetComponent<Text> ().text = nowScore2.ToString ();
			EventGame.transform.Find ("timer/Text").gameObject.GetComponent<Text> ().text = nowTime2.ToString ();

			return	gameMainLoopFlag;
		}


		private int fukidashiNumber;
		private int fukidashiMenuNumber;

		private void fukidashiInit(){
			fukidashiNumber = Random.Range (0, 4);
			switch (menuCount) {
			case	2:
				{
					int[] _table = { 1, 6 };
					fukidashiMenuNumber = _table [Random.Range (0, 2)];
					break;
				}
			case	4:
				{
					int[] _table = { 1, 2, 5, 6 };
					fukidashiMenuNumber = _table [Random.Range (0, 4)];
					break;
				}
			case	6:
				{
					int[] _table = { 0, 1, 2, 5, 6, 7 };
					fukidashiMenuNumber = _table [Random.Range (0, 6)];
					break;
				}
			default:{
					fukidashiMenuNumber = Random.Range (0, 8);
					break;
				}
			}
			EventGameFukidashi.transform.Find ("fukidashi").gameObject.GetComponent<Image> ().sprite = FukidashiImage [fukidashiNumber];
			EventGameFukidashi.transform.Find ("menu").gameObject.GetComponent<Image> ().sprite = MenuImage [fukidashiMenuNumber];			// ０：カツ丼、１：プリン、２：サンド、３：ステーキ、４：パスタ、５：オムライス、６：ご飯、７：寿司

			EventGameFukidashi.transform.Find ("fukidashi").gameObject.SetActive (true);
			EventGameFukidashi.transform.Find ("menu").gameObject.SetActive (true);
			EventGameFukidashi.transform.Find ("hantei").gameObject.SetActive (false);

		}
		private bool fukidashiHantei(){
			bool _flag;
			if (buttonNumber == fukidashiMenuNumber) {
				int _score;

				EventGameFukidashi.transform.Find ("hantei").gameObject.GetComponent<Image> ().sprite = CheckImage [0];						// ０：丸、１：バツ
				_flag = true;

				switch (menuCount) {
				case	2:
					{
						_score = GAME_SCORE_POINT1;
						break;
					}
				case	4:
					{
						_score = GAME_SCORE_POINT2;
						break;
					}
				case	6:
					{
						_score = GAME_SCORE_POINT3;
						break;
					}
				default:
					{
						_score = GAME_SCORE_POINT4;
						break;
					}
				}
				EventGameScore.transform.Find ("score").gameObject.GetComponent<Text> ().text = _score.ToString();
				nowScore += _score;

				ManagerObject.instance.sound.playSe (28);
			} else {
				EventGameFukidashi.transform.Find ("hantei").gameObject.GetComponent<Image> ().sprite = CheckImage [1];						// ０：丸、１：バツ
				_flag = false;

				ManagerObject.instance.sound.playSe (29);
			}
//			EventGameFukidashi.transform.Find ("hantei").gameObject.SetActive (true);

			ButtonMenu [buttonNumber].transform.Find ("hantei").gameObject.GetComponent<Image> ().sprite = EventGameFukidashi.transform.Find ("hantei").gameObject.GetComponent<Image> ().sprite;
			ButtonMenu [buttonNumber].transform.Find ("hantei").gameObject.SetActive (true);

			return _flag;
		}
		private void fukidashiOff(){
			EventGameFukidashi.transform.Find ("fukidashi").gameObject.SetActive (false);
			EventGameFukidashi.transform.Find ("menu").gameObject.SetActive (false);
			EventGameFukidashi.transform.Find ("hantei").gameObject.SetActive (false);

			for (int i = 0; i < 8; i++) {
				ButtonMenu [i].transform.Find ("hantei").gameObject.SetActive (false);
			}
		}

		private int scoreAnimeCount;
		private int scoreAnimeWait;
		private void ScoreInit(){
			EventGameScore.SetActive (true);
			Vector3 _pos = ButtonMenu [buttonNumber].transform.localPosition;
			_pos.y -= 100.0f;
			EventGameScore.transform.localPosition = _pos;						// スコアは、正解したメニューの上に表示する
			scoreAnimeCount = 10;
		}
		private bool ScoreAnime(){
//			Vector3 _pos = EventGameScore.transform.localPosition;
			bool _flag = false;

			switch (scoreAnimeCount) {
/*
			case	0:
				{
					_pos.y += 5.0f;
					if (_pos.y >= -50.0f) {
						scoreAnimeCount++;
						scoreAnimeWait = 30;
					}
					break;
				}
			case	1:
				{
					scoreAnimeWait--;
					if (scoreAnimeWait == 0) {
						scoreAnimeCount++;
						_flag = true;
						EventGameScore.SetActive (false);
					}
					break;
				}
*/				
			case	10:
				{
					scoreAnimeCount = 11;
					StartCoroutine (scoreAnimeLoop (12));
					break;
				}
			case	11:
				{
					break;											// scoreAnimeLoopの終了をまつ
				}
			case	12:
				{
					_flag = true;
					EventGameScore.SetActive (false);
					break;
				}
			}

//			EventGameScore.transform.localPosition = _pos;
			return _flag;
		}

		// スコアの表示とメニューの消去をここで行う
		IEnumerator scoreAnimeLoop(int _num){
			Vector3 _scl = ButtonMenu [0].transform.localScale;

			while (true) {
				_scl.x -= 0.5f;
				_scl.y -= 0.5f;
				if (_scl.x <= 0.0f) {
					break;
				}
				for (int i = 0; i < 8; i++) {
					if (i != buttonNumber) {
						ButtonMenu [i].transform.localScale = _scl;
					}
				}

				yield return null;
			}

			for (int i = 0; i < 8; i++) {
				if (i != buttonNumber) {
					ButtonMenu [i].SetActive (false);
				}
			}

			yield return new WaitForSeconds (0.5f);

			_scl = ButtonMenu [buttonNumber].transform.localScale;

			while (true) {
				_scl.x -= 0.5f;
				_scl.y -= 0.5f;
				if (_scl.x <= 0.0f) {
					break;
				}
				ButtonMenu [buttonNumber].transform.localScale = _scl;

				Vector3 _pos = EventGameScore.transform.localPosition;
				_pos.y += 5.0f;
				EventGameScore.transform.localPosition = _pos;

				yield return null;
			}
			ButtonMenu [buttonNumber].SetActive (false);

			yield return new WaitForSeconds (0.1f);

			scoreAnimeCount = _num;
		}
			


		private statusResult	resultLoopCount = statusResult.resultJobCount000;
		private enum statusResult{
			resultJobCount000,
			resultJobCount010,
			resultJobCount020,
			resultJobCount030,
			resultJobCount040,
			resultJobCount050,
			resultJobCount060,
			resultJobCount070,
			resultJobCount080,
			resultJobCount090,
			resultJobCount100,
			resultJobCount110,
		}
		private float[] tamagoYJumpTable = new float[] {
			-6.0f,-5.0f,-5.0f,-3.8f,-3.8f,-3.8f,-2.4f,-2.4f,-2.4f,-2.4f,-1.2f,-1.2f,-1.2f,-1.2f,-1.2f,0.0f,0.0f,0.0f,
			0.0f,0.0f,0.0f,1.2f,1.2f,1.2f,1.2f,1.2f,2.4f,2.4f,2.4f,2.4f,3.8f,3.8f,3.8f,5.0f,5.0f,6.0f,
		};
		private float[] treasureRotationTable = new float[] {						// 落下した宝箱のZRotationアニメ用
			0.0f,
			-0.256f,
			-0.9f,
			-1.79f,
			-2.783f,
			-3.735f,
			-4.504f,
			-4.946f,
			-4.915f
			-4.359f,
			-3.386f,
			-2.118f,
			-0.674f,
			0.823f,
			2.255f,
			3.499f,
			4.436f,
			4.943f,
			4.825f,
			3.84f,
			2.402f,
			0.997f,
			0.105f,
		};
		private int resultLoopWait = 0;
		private int resultTamagoYJumpCount = 0;
		private bool resultItemGetFlag = false;
		private bool resultMainLoopFlag = false;
		// 結果画面のメインループ
		private bool ResultMainLoop(){
			Vector3 pos;

			switch (resultLoopCount) {
			case	statusResult.resultJobCount000:
				{
					EventResult.transform.Find ("result_text").gameObject.SetActive (true);
					EventResult.transform.Find ("point_text").gameObject.SetActive (false);
					EventResult.transform.Find ("points").gameObject.SetActive (false);
					EventResult.transform.Find ("treasure").gameObject.SetActive (false);
					EventResult.transform.Find ("treasure_open").gameObject.SetActive (false);
					EventResult.transform.Find ("Button_blue_modoru").gameObject.SetActive (false);

					if ((nowScore == 0) || (!mResultData.rewardFlag) || (mSceneLabel != "PapaMama")) {
						if (!futagoFlag) {
							EventResult.transform.Find ("chara").gameObject.transform.localPosition = new Vector3 (250.0f, -320.0f, 0.0f);
						} else {
							EventResult.transform.Find ("chara").gameObject.transform.localPosition = new Vector3 (375.0f, -320.0f, 0.0f);
							EventResult.transform.Find ("charaF").gameObject.transform.localPosition = new Vector3 (225.0f, -320.0f, 0.0f);
						}
					} else {
						if (!futagoFlag) {
							EventResult.transform.Find ("chara").gameObject.transform.localPosition = new Vector3 (120.0f, -320.0f, 0.0f);
						} else {
							EventResult.transform.Find ("chara").gameObject.transform.localPosition = new Vector3 (245.0f, -320.0f, 0.0f);
							EventResult.transform.Find ("charaF").gameObject.transform.localPosition = new Vector3 (95.0f, -320.0f, 0.0f);
						}
					}
						
					EventResult.transform.Find ("chara0").gameObject.transform.localPosition = new Vector3 (-310.0f, -200.0f, 0.0f);
					EventResult.transform.Find ("chara1").gameObject.transform.localPosition = new Vector3 ( 310.0f, -200.0f, 0.0f);
					EventResult.transform.Find ("chara2").gameObject.transform.localPosition = new Vector3 (-480.0f, -200.0f, 0.0f);
					EventResult.transform.Find ("chara3").gameObject.transform.localPosition = new Vector3 ( 480.0f, -200.0f, 0.0f);

					for (int i = 0; i < 4; i++) {
						string[] _name = new string[]{ "chara0", "chara1", "chara2", "chara3" };

						if (!NpcDispFlag [i]) {
							EventResult.transform.Find (_name [i]).gameObject.transform.localPosition = new Vector3 (5000, 5000, 0);
						}
					}

					resultLoopCount = statusResult.resultJobCount010;
					break;
				}
			case	statusResult.resultJobCount010:
				{
					pos = EventResult.transform.Find ("result_text").gameObject.transform.localPosition;	// 結果画面の表題の座標を抽出
					pos.y -= 11;
					if (pos.y <= 89) {
						pos.y = 89;
						resultLoopCount = statusResult.resultJobCount020;
						resultLoopWait = 45;
					}
					EventResult.transform.Find ("result_text").gameObject.transform.localPosition = pos;	// 結果画面の表題の座標を設定
					break;
				}
			case	statusResult.resultJobCount020:
				{
					if (ResultWaitTimeSubLoop()) {															// 結果画面の表題を見せる
						resultLoopCount = statusResult.resultJobCount030;
						EventResult.transform.Find ("point_text").gameObject.SetActive (true);
						EventResult.transform.Find ("points").gameObject.SetActive (true);
						EventResult.transform.Find ("treasure").gameObject.SetActive (true);
						resultLoopWait = 90;

						_resultScoreAnimeFLag = false;
						StartCoroutine (ResultScoreAnime ());
					}
					break;
				}
			case	statusResult.resultJobCount030:
				{
					if (_resultScoreAnimeFLag) {
						resultLoopCount = statusResult.resultJobCount040;
					}
					break;
				}
			case	statusResult.resultJobCount040:
				{
					if (ResultWaitTimeSubLoop ()) {															// スコアなどを見せる
						resultLoopCount = statusResult.resultJobCount050;
						if ((nowScore == 0) || (!mResultData.rewardFlag)) {
							resultItemGetFlag = true;														// 褒賞品が手に入らないのでそのまま終了
							resultLoopCount = statusResult.resultJobCount100;
						} else {
                                if (mSceneLabel != "PapaMama")
                                {
                                    resultLoopCount = statusResult.resultJobCount090;
                                }
                                else
                                {
                                    ManagerObject.instance.sound.playSe(9);
                                }

                            }
					}
					break;
				}
			case	statusResult.resultJobCount050:{
					pos = EventResult.transform.Find ("treasure").gameObject.transform.localPosition;		// 落下する宝箱の座標を抽出
					pos.y -= 25;
					if (pos.y <= EventResult.transform.Find ("treasure_open").gameObject.transform.localPosition.y) {
						pos.y = EventResult.transform.Find ("treasure_open").gameObject.transform.localPosition.y;
						resultLoopCount = statusResult.resultJobCount060;
						resultLoopWait = treasureRotationTable.Length;
						resultTamagoYJumpCount = tamagoYJumpTable.Length;

						ManagerObject.instance.sound.playSe (30);
					}
					EventResult.transform.Find ("treasure").gameObject.transform.localPosition = pos;		// 落下する宝箱の座標を設定
					break;
				}
			case	statusResult.resultJobCount060:
				{
					if (ResultWaitTimeSubLoop ()) {
						resultLoopCount = statusResult.resultJobCount070;
					}
					pos = EventResult.transform.Find ("treasure").gameObject.transform.eulerAngles;			// 宝箱を震えさせる
					pos.z = treasureRotationTable [resultLoopWait];
					EventResult.transform.Find ("treasure").gameObject.transform.eulerAngles = pos;

					break;
				}
			case	statusResult.resultJobCount070:
				{
					EventResult.transform.Find ("treasure").gameObject.SetActive (false);					// 宝箱を開いたものに変える
					EventResult.transform.Find ("treasure_open").gameObject.SetActive (true);
					resultLoopCount = statusResult.resultJobCount080;
					resultLoopWait = 60;
					break;
				}
			case	statusResult.resultJobCount080:
				{
					if (ResultWaitTimeSubLoop ()) {															// 開いた宝箱を見せる
						resultLoopCount = statusResult.resultJobCount090;
					}
					break;
				}
			case	statusResult.resultJobCount090:
				{
					EventResult.SetActive (false);
					// アイテム入手画面を開く
					EventItemget.SetActive (true);
					ManagerObject.instance.sound.playSe (23);

					EventItemget.transform.Find ("getpoints_text").gameObject.SetActive (false);
                    EventItemget.transform.Find("getitem_text").gameObject.SetActive(false);
                    EventItemget.transform.Find("getcolabo_text").gameObject.SetActive(false);
                    EventItemget.transform.Find("Button_red_takuhai").gameObject.SetActive(true);
                    EventItemget.transform.Find("Button_blue_tojiru").gameObject.transform.localPosition = new Vector3(250, -330, 0);
                        switch (mResultData.reward.kind)
                        {
                            case RewardKind.ITEM:
                                {
                                    // アイテムが褒賞品
                                    EventItemget.transform.Find("getitem_text").gameObject.SetActive(true);
                                    break;
                                }
                            case RewardKind.GOTCHI_PT:
                                {
                                    // ごっちポイントが褒賞品
                                    EventItemget.transform.Find("getpoints_text").gameObject.SetActive(true);
                                    break;
                                }
                            default:
                                {
                                    // イベントポイントが褒賞品
                                    EventItemget.transform.Find("getcolabo_text").gameObject.SetActive(true);
                                    EventItemget.transform.Find("Button_red_takuhai").gameObject.SetActive(false);
                                    EventItemget.transform.Find("Button_blue_tojiru").gameObject.transform.localPosition = new Vector3(0, -330, 0);

                                    break;
                                }
                        }



                    RewardBehaviour rbItem = EventItemget.transform.Find("RewardView").gameObject.GetComponent<RewardBehaviour>();
                    rbItem.init(mResultData.reward);

                    resultItemGetFlag = false;
					resultLoopCount = statusResult.resultJobCount100;
					break;
				}
			case	statusResult.resultJobCount100:
				{
					if (resultItemGetFlag) {
						EventItemget.SetActive (false);														// アイテム入手画面を消す
						EventResult.SetActive (true);														// 結果画面を表示する
						EventResult.transform.Find ("Button_blue_modoru").gameObject.SetActive (true);		// 戻るボタンを表示する
						if ((nowScore == 0) || (!mResultData.rewardFlag)) {
						}
						else{
							for (int i = 0; i < CharaTamagoMain.Length; i++) {								// 褒賞品があるのでたまごっちを喜ばす
								TamagochiMainAnimeSet (i, MotionLabel.GLAD1);
							}
							for (int i = 0; i < CharaTamagoNpc.Length; i++) {
								TamagochiNpcAnimeSet (i, MotionLabel.GLAD1);
							}
						}
							
						resultLoopCount = statusResult.resultJobCount110;
					}
					break;
				}
			case	statusResult.resultJobCount110:
				{
					break;
				}
			}

			if (resultTamagoYJumpCount != 0) {																// 落ちて来た宝箱に跳ね飛ばされるたまごっち達
				resultTamagoYJumpCount--;
				pos = EventResult.transform.Find ("chara").gameObject.transform.localPosition;
				pos.x += 4.0f;
				pos.y += tamagoYJumpTable [resultTamagoYJumpCount];
				EventResult.transform.Find ("chara").gameObject.transform.localPosition = pos;

				pos = EventResult.transform.Find ("charaF").gameObject.transform.localPosition;
				pos.x += 4.0f;
				pos.y += tamagoYJumpTable [resultTamagoYJumpCount];
				EventResult.transform.Find ("charaF").gameObject.transform.localPosition = pos;
				}

			return resultMainLoopFlag;
		}
		private bool ResultWaitTimeSubLoop(){
			resultLoopWait--;
			if (resultLoopWait == 0) {
				return true;
			}
			return false;
		}

		private bool _resultScoreAnimeFLag;
		private IEnumerator ResultScoreAnime(){
			int[] resultScore = new int[5];
			int resultScoreCount;

			for (int i = 0; i < resultScore.Length; i++) {
				resultScore [i] = 0;
			}
			resultScoreCount = 0;

			while (true) {
				int _mulCnt = 1;
				nowScore2 = 0;
				for (int i = 0; i < resultScore.Length; i++) {
					nowScore2 += resultScore [i] * _mulCnt;
					_mulCnt *= 10;
				}

				if (nowScore2 >= nowScore) {
					nowScore2 = nowScore;
					break;
				} else {
					resultScore [resultScoreCount]++;
					if (resultScore [resultScoreCount] >= 10) {
						resultScore [resultScoreCount] = 0;
					}

					string _mes = "";
					for (int i = 0; i < resultScoreCount + 1; i++) {
						_mes = resultScore [i].ToString () + _mes;
					}

					EventResult.transform.Find ("points/Text").gameObject.GetComponent<Text> ().text = _mes;

					int _nowScore = nowScore;
					int _num = 0;
					for (int i = 0; i < resultScoreCount + 1 ; i++) {
						_num = _nowScore % 10;
						_nowScore /= 10;
					}
					if (resultScore [resultScoreCount] == _num) {
						resultScoreCount++;
					}
				}
			


				yield return new WaitForSeconds (0.1f);
			}

			EventResult.transform.Find ("points/Text").gameObject.GetComponent<Text> ().text = nowScore2.ToString ();
			_resultScoreAnimeFLag = true;
		}



		private void TamagoAnimeSprite(){
			EventResult.transform.Find ("chara").transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
			EventResult.transform.Find ("charaF").transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
			EventResult.transform.Find ("chara0").transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
			EventResult.transform.Find ("chara1").transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
			EventResult.transform.Find ("chara2").transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
			EventResult.transform.Find ("chara3").transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);

			UIFunction.TamagochiImageMove (EventResult, CharaTamagoMain [0], "chara/");
			UIFunction.TamagochiImageMove (EventResult, CharaTamagoMain [1], "charaF/");

			for (int i = 0; i < CharaTamagoNpc.Length; i++) {
				string[] _name = new string[] { "chara0/", "chara1/", "chara2/", "chara3/" };
				if (NpcDispFlag [i]) {
					UIFunction.TamagochiImageMove (EventResult, CharaTamagoNpc [i], _name [i]);
				}
			}
		}

		private Vector2[] tamagoCharaPositionInitTable = new Vector2[] {
			new Vector2 (   0.0f,  700.0f),		// 結果画面の宝箱の初期位置
			new Vector2 (   0.0f,  700.0f),		// 結果画面のメッセージの初期位置
		};
		private void TamagoCharaPositionInit(){
			TamagoCharaPositionInitSub (EventResult.transform.Find ("treasure").gameObject, 0);
			TamagoCharaPositionInitSub (EventResult.transform.Find ("result_text").gameObject, 1);
		}
		private void TamagoCharaPositionInitSub (GameObject obj, int num){
			Vector3 pos = new Vector3 (0.0f, 0.0f, 0.0f);

			pos = obj.transform.localPosition;
			pos.x = tamagoCharaPositionInitTable [num].x;
			pos.y = tamagoCharaPositionInitTable [num].y;
			obj.transform.localPosition = pos;
		}


        private void EventTitleDisp(bool _flag)
        {
            if(mSceneLabel == "PapaMama")
            {
                EventTitle.SetActive(_flag);
            }
            else
            {
                EventColaboShop.SetActive(_flag);
                EventColaboShop.transform.Find("Point/points_text").gameObject.GetComponent<Text>().text = ManagerObject.instance.player.evp.ToString();
            }
        }


        private void EventSpriteSet()
        {
            if(mData.assetBundle == null)
            {
                // アセットバンドルがない場合この下の処理を無視する。
                return;
            }

            MiniGame2EventImg _data = mData.assetBundle.LoadAllAssets<MiniGame2EventImg>()[0];



            MinigameRoot.transform.Find("bg").gameObject.GetComponent<Image>().sprite = _data.ImgBG;
            MinigameRoot.transform.Find("title/title").gameObject.GetComponent<Image>().sprite = _data.ImgTitle;

            EventColaboShop.gameObject.GetComponent<Image>().sprite = _data.ImgBanner;

            // メニューのスプライトを差し替える
            for(int i= 0;i < _data.ImgItem.Length; i++)
            {
                MenuImage[i] = _data.ImgItem[i];
            }
            EventGameMenu.transform.Find("menu1").gameObject.GetComponent<Image>().sprite = _data.ImgItem[0];
            EventGameMenu.transform.Find("menu2").gameObject.GetComponent<Image>().sprite = _data.ImgItem[1];
            EventGameMenu.transform.Find("menu3").gameObject.GetComponent<Image>().sprite = _data.ImgItem[2];
            EventGameMenu.transform.Find("menu4").gameObject.GetComponent<Image>().sprite = _data.ImgItem[3];
            EventGameMenu.transform.Find("menu5").gameObject.GetComponent<Image>().sprite = _data.ImgItem[4];
            EventGameMenu.transform.Find("menu6").gameObject.GetComponent<Image>().sprite = _data.ImgItem[5];
            EventGameMenu.transform.Find("menu7").gameObject.GetComponent<Image>().sprite = _data.ImgItem[6];
            EventGameMenu.transform.Find("menu8").gameObject.GetComponent<Image>().sprite = _data.ImgItem[7];
        }

    }


}