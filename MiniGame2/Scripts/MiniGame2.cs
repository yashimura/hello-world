using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;




namespace Mix2App.MiniGame2{
	public class MiniGame2 : MonoBehaviour,IReceiver {
		[SerializeField] private GameObject[] CharaTamagoMain;					// たまごっち
		[SerializeField] private GameObject[] CharaTamago;						// たまごっち
		[SerializeField] private GameObject baseSizePanel;
		[SerializeField] private GameObject EventTitle;							// タイトル画面
		[SerializeField] private GameObject EventStart;							// スタート画面
		[SerializeField] private GameObject	EventGame;							// ゲーム画面
		[SerializeField] private GameObject EventEnd;							// 終了画	面
		[SerializeField] private GameObject EventResult;						// 結果画面
		[SerializeField] private GameObject EventItemget;						// アイテム入手画面
		[SerializeField] private GameObject EventHelp;							// 遊び方説明画面
		[SerializeField] private GameObject ButtonStart;						// タイトル スタートボタン
		[SerializeField] private GameObject ButtonHelp;							// タイトル ヘルプボタン
		[SerializeField] private GameObject ButtonClose;						// タイトル 閉じるボタン
		[SerializeField] private GameObject ButtonYameru;						// ゲーム やめるボタン
		[SerializeField] private GameObject ButtonTakuhai;						// アイテム入手 宅配ボタン
		[SerializeField] private GameObject ButtonTojiru;						// アイテム入手 閉じるボタン
		[SerializeField] private GameObject ButtonModoru;						// 結果 戻るボタン
		[SerializeField] private GameObject ButtonHelpModoru;					// 遊び方説明画面 戻るボタン
		[SerializeField] private GameObject[] ButtonMenu;						// ゲーム メニューボタン（８個）
		[SerializeField] private GameObject EventGameMenu;
		[SerializeField] private GameObject EventGameFukidashi;
		[SerializeField] private GameObject EventGameScore;
		[SerializeField] private GameObject[] EventStartTamago;
		[SerializeField] private GameObject[] EventGameTamago;
		[SerializeField] private GameObject[] EventEndTamago;
		[SerializeField] private GameObject[] EventResultTamago;
		[SerializeField] private Sprite[] MenuImage;							// ０：カツ丼、１：プリン、２：サンド、３：ステーキ、４：パスタ、５：オムライス、６：ご飯、７：寿司
		[SerializeField] private Sprite[] FukidashiImage;						// ０：吹き出し１、１：吹き出し２、２：吹き出し３、３：吹き出し４
		[SerializeField] private Sprite[] CheckImage;							// ０：丸、１：バツ



		private object[]		mparam;

		private CharaBehaviour[] cbCharaTamagoMain = new CharaBehaviour[3];		// プレイヤーとゲスト
		private CharaBehaviour[] cbCharaTamago = new CharaBehaviour[12];		// お客様
//		private bool startEndFlag = false;
		private bool screenModeFlag = true;
		private int waitCount;
		private int nowScore;													// 得点
		private float nowTime1;													// 残り時間（カウントダウン用）
		private int	nowTime2;													// 残り時間（制限時間）

		private readonly float MENU_IDOU_SPEED = 25.0f;							// メニューの移動速度
		private readonly int GAME_PLAY_TIME = 60;								// ゲームプレイ制限時間
		private readonly int GAME_SCORE_POINT1 = 10;							// ２択時の得点
		private readonly int GAME_SCORE_POINT2 = 20;							// ４択時の得点
		private readonly int GAME_SCORE_POINT3 = 50;							// ６択時の得点
		private readonly int GAME_SCORE_POINT4 = 100;							// ８択時の得点

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
		}

		private User muser1;//自分

		void Awake(){
			Debug.Log ("MiniGame2 Awake");

			mparam=null;
			muser1=null;
		}

		public void receive(params object[] parameter){
			Debug.Log ("MiniGame2 receive");
			mparam = parameter;
		}

		IEnumerator Start(){
			Debug.Log ("MiniGame2 Start");

			//単体動作テスト用
			//パラメタ詳細は設計書参照
			if (mparam==null) {
				mparam = new object[] {
					ManagerObject.instance.player,
				};
			}
			muser1 = (User)mparam[0];		// たまごっち



//			startEndFlag = false;

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

			if ((num > 1.33f) && (num < 1.34f)) {
				baseSizePanel.GetComponent<Transform> ().transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);		// 3:4の時のみ画面を拡大表示
				screenModeFlag = true;
			} else {
				baseSizePanel.GetComponent<Transform> ().transform.localScale = new Vector3 (1.15f, 1.15f, 1.0f);	// 3:4の以外の時の画面を拡大表示
				screenModeFlag = false;
			}

			for (int i = 0; i < 3; i++) {
				cbCharaTamagoMain [i] = CharaTamagoMain [i].GetComponent<CharaBehaviour> ();
			}
			for (int i = 0; i < 12; i++) {
				cbCharaTamago [i] = CharaTamago [i].GetComponent<CharaBehaviour> ();
			}

			yield return cbCharaTamagoMain [0].init (muser1.chara1);
			TamagochiMainAnimeSet (0, "idle");
			yield return cbCharaTamagoMain [1].init (new TamaChara (17));
			TamagochiMainAnimeSet (1, "idle");
			yield return cbCharaTamagoMain [2].init (new TamaChara (18));
			TamagochiMainAnimeSet (2, "idle");

			yield return cbCharaTamago[0].init (new TamaChara (19));
			TamagochiAnimeSet (0, "idle");
			yield return cbCharaTamago[1].init (new TamaChara (20));
			TamagochiAnimeSet (1, "idle");
			yield return cbCharaTamago[2].init (new TamaChara (21));
			TamagochiAnimeSet (2, "idle");
			yield return cbCharaTamago[3].init (new TamaChara (22));
			TamagochiAnimeSet (3, "idle");
			yield return cbCharaTamago[4].init (new TamaChara (23));
			TamagochiAnimeSet (4, "idle");
			yield return cbCharaTamago[5].init (new TamaChara (24));
			TamagochiAnimeSet (5, "idle");
			yield return cbCharaTamago[6].init (new TamaChara (25));
			TamagochiAnimeSet (6, "idle");
			yield return cbCharaTamago[7].init (new TamaChara (26));
			TamagochiAnimeSet (7, "idle");
			yield return cbCharaTamago[8].init (new TamaChara (27));
			TamagochiAnimeSet (8, "idle");
			yield return cbCharaTamago[9].init (new TamaChara (28));
			TamagochiAnimeSet (9, "idle");
			yield return cbCharaTamago[10].init (new TamaChara (29));
			TamagochiAnimeSet (10, "idle");
			yield return cbCharaTamago[11].init (new TamaChara (30));
			TamagochiAnimeSet (11, "idle");

//			startEndFlag = true;
		}

		void Destroy(){
			Debug.Log ("MiniGame2 Destroy");
		}

		void Update(){
			switch (jobCount) {
			case	statusJobCount.minigame2JobCount000:
				{
//					if (startEndFlag) {
						EventTitle.SetActive (true);
						TamagochiDesignOff ();
						jobCount = statusJobCount.minigame2JobCount010;
						TamagoCharaPositionInit ();
						nowScore = 0;
						nowTime1 = 0.0f;
						nowTime2 = GAME_PLAY_TIME;									// 制限時間
//					}
					break;
				}
			case	statusJobCount.minigame2JobCount010:
				{
					break;
				}
			case	statusJobCount.minigame2JobCount020:
				{
					EventTitle.SetActive (false);
					EventStart.SetActive (true);
					TamagochiLoopInit ();
					jobCount = statusJobCount.minigame2JobCount030;
					break;
				}
			case	statusJobCount.minigame2JobCount030:
				{
					if (EventStart.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.minigame2JobCount040;
						EventStart.SetActive (false);
						EventGame.SetActive (true);
						GameMainInit ();
					}
					TamagochiStartLoopIdou ();
					break;
				}
			case	statusJobCount.minigame2JobCount040:
				{
					if (GameMainLoop ()) {											// ゲーム処理
						jobCount = statusJobCount.minigame2JobCount050;
						waitCount = 45;
					}
					break;
				}
			case	statusJobCount.minigame2JobCount050:
				{
					waitCount--;
					if (waitCount == 0) {											// 驚きを見せるためのウエィト
						jobCount = statusJobCount.minigame2JobCount060;
						EventGame.SetActive (false);
						EventEnd.SetActive (true);
					}
					break;
				}
			case	statusJobCount.minigame2JobCount060:
				{
					if (EventEnd.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {	// お疲れ様のアニメが終了するまで待つ
						jobCount = statusJobCount.minigame2JobCount070;
						EventEnd.SetActive (false);
						EventResult.SetActive (true);

						for (int i = 0; i < CharaTamago.Length; i++) {				// お客さん達を消す
							CharaTamago [i].transform.localPosition = new Vector3 (-50.0f, 0.0f, 1.0f);
						}
						for (int i = 0; i < CharaTamagoMain.Length; i++) {			// プレイヤーとゲストを消す
							CharaTamagoMain [i].transform.localPosition = new Vector3 (-50.0f, 0.0f, 1.0f);
							TamagochiMainAnimeSet (i, "idle");
						}

						resultLoopCount = statusResult.resultJobCount000;
						resultMainLoopFlag = false;
						ResultMainLoop ();											// 結果画面処理
						TamagoAnimeSprite ();										// たまごっちのアニメをImageに反映する
					}
					break;
				}
			case	statusJobCount.minigame2JobCount070:
				{
					if (ResultMainLoop ()) {										// 結果画面処理
						jobCount = statusJobCount.minigame2JobCount000;
						EventResult.SetActive (false);
						EventTitle.SetActive (true);
					}
					TamagoAnimeSprite ();											// たまごっちのアニメをImageに反映する
					break;
				}
			case	statusJobCount.minigame2JobCount080:
				{
					break;
				}
			case	statusJobCount.minigame2JobCount090:
				{
					break;
				}
			case	statusJobCount.minigame2JobCount100:
				{
					break;
				}
			}
		}

		private void ButtonStartClick(){
			jobCount = statusJobCount.minigame2JobCount020;							// スタートボタンが押されたのでゲーム開始
		}
		private void ButtonCloseClick(){
			Debug.Log ("たまタウンへ・・・");
			ManagerObject.instance.view.change("Town");
		}
		private void ButtonHelpClick(){
			EventHelp.SetActive (true);
		}
		private void ButtonHelpModoruClick(){
			EventHelp.SetActive (false);
		}
		private void ButtonYameruClick(){
			gameMainLoopFlag = true;
		}
		private void ButtonTakuhaiClick(){
			Debug.Log ("宅配サービスへ・・・");
		}
		private void ButtonTojiruClick(){
			resultItemGetFlag = true;
		}
		private void ButtonModoruClick(){
			resultMainLoopFlag = true;												// 
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

		private int[] tamagochiIdouFlag = new int[12];
		private void TamagochiLoopInit(){
			Vector2[] _initTable = new Vector2[] {
				new Vector2 (0.0f, 3.0f),
				new Vector2 (-2.25f, 3.0f),
				new Vector2 (2.25f, 3.0f),
			};

			for (int i = 0; i < CharaTamago.Length; i++) {
				CharaTamago [i].SetActive (true);
			}
			for (int i = 0; i < CharaTamagoMain.Length; i++) {
				CharaTamagoMain [i].SetActive (true);
				TamagochiMainAnimeSet (i, "idle");
			}

			for (int i = 0; i < 3; i++) {
				Vector3 pos = CharaTamagoMain [i].transform.localPosition;
				pos.x = _initTable [i].x;
				pos.y = _initTable [i].y;
				if (screenModeFlag) {
					pos.y -= 0.25f;
				}
				CharaTamagoMain [i].transform.localPosition = pos;
			}

			for (int i = 0; i < 12; i++) {
				Vector3 pos = CharaTamago [i].transform.localPosition;
				pos.x = (-7.0f - (i * 2));
				pos.y = (-6.5f - (i * 2));
				CharaTamago [i].transform.localPosition = pos;
				tamagochiIdouFlag [i] = 0;
			}
		}

		private float[,] _idouTable = new float[12, 8] {
			{  -4.0f,  -3.5f,   6.0f, 255.0f, 255.0f,  -1.5f,   2.0f, 255.0f },
			{  -4.0f,  -3.5f,   6.0f, 255.0f, 255.0f,  -1.5f,   4.0f, 255.0f },
			{  -4.0f,  -3.5f,   6.0f, 255.0f, 255.0f,  -1.5f,   6.0f, 255.0f },
			{  -4.0f,  -3.5f,   6.0f, 255.0f, 255.0f,  -3.5f, 255.0f, 255.0f },
			{  -4.0f,  -3.5f,   4.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f },
			{  -4.0f,  -3.5f,   2.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f },
			{  -4.0f,  -3.5f,   0.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f },
			{  -4.0f,  -3.5f,  -2.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f },
			{  -4.0f,  -3.5f,  -4.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f },
			{ 255.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f },
			{ 255.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f },
			{ 255.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f, 255.0f },
		};
		private void TamagochiStartLoopIdou(){
			Vector3 pos;
			for (int num = 0; num < 12; num++) {
				pos = CharaTamago [num].transform.localPosition;
				switch (tamagochiIdouFlag [num]) {
				case	0:
					{
						if (_idouTable [num, 0] != 255.0f) {
							pos.x += 0.125f;
							pos.y += 0.125f;
							TamagochiAnimeSet (num, "walk", true);

							if (pos.x >= _idouTable [num, 0]) {
								pos.x = _idouTable [num, 0];
								pos.y = _idouTable [num, 1];
								tamagochiIdouFlag [num] = 1;
							}
						} else {
							TamagochiAnimeSet (num, "idle");
						}
						break;
					}
				case	1:
					{
						if (_idouTable [num, 2] != 255.0f) {
							pos.x += 0.125f;
							TamagochiAnimeSet (num, "walk", true);
							if (pos.x >= _idouTable [num, 2]) {
								pos.x = _idouTable [num, 2];
								tamagochiIdouFlag [num] = 2;
							}
						} else {
							TamagochiAnimeSet (num, "idle");
						}
						break;
					}
				case	2:
					{
						if (_idouTable [num, 5] != 255.0f) {
							pos.y += 0.125f;
							TamagochiAnimeSet (num, "walk", false);
							if (pos.y >= _idouTable [num, 5]) {
								pos.y = _idouTable [num, 5];
								tamagochiIdouFlag [num] = 3;
							}
						} else {
							TamagochiAnimeSet (num, "idle");
						}
						break;
					}
				case	3:
					{
						if (_idouTable [num, 6] != 255.0f) {
							pos.x -= 0.125f;
							TamagochiAnimeSet (num, "walk", false);
							if (pos.x <= _idouTable [num, 6]) {
								pos.x = _idouTable [num, 6];
								tamagochiIdouFlag [num] = 4;
								TamagochiAnimeSet (num, "idle");
							}
						} else {
							TamagochiAnimeSet (num, "idle");
						}
						break;
					}
				}
				CharaTamago [num].transform.localPosition = pos;
			}
		}



		private int[] tamagochiIdouTable = new int[12];
		private int tamagoIdouTime = 80;
		private int tamagoIdouTime2 = 40;
		private int tamagoIdouTime3 = 40;
		private int tamagoIdouTimeFlag = 0;
		private int tamagoGameIdouCount;
		private TamagoIdou[] tamagoGameIdouWork = new TamagoIdou[12];
		private void TamagochiGameIdouInit(){
			for (int i = 0; i < 12; i++) {
				tamagochiIdouTable [i] = i;
			}
		}
		private bool TamagochiGameIdouLoop(){
			bool _flag = false;
			Vector3 _pos;

			switch (tamagoGameIdouCount) {
			case	0:
				{
					tamagoIdouTimeFlag = 0;

					tamagoGameIdouWork [tamagochiIdouTable [0]].targetX = -4.0f;
					tamagoGameIdouWork [tamagochiIdouTable [0]].targetY = -1.5f;
					_pos = CharaTamago [tamagochiIdouTable [0]].transform.localPosition;
					tamagoGameIdouWork [tamagochiIdouTable [0]].speedX = (tamagoGameIdouWork [tamagochiIdouTable [0]].targetX - _pos.x) / tamagoIdouTime2;
					tamagoGameIdouWork [tamagochiIdouTable [0]].speedY = (tamagoGameIdouWork [tamagochiIdouTable [0]].targetY - _pos.y) / tamagoIdouTime2;

					if (tamagoGameIdouWork [tamagochiIdouTable [0]].targetX <= _pos.x) {
						TamagochiAnimeSet (tamagochiIdouTable [0], "walk", false);
					} else {
						TamagochiAnimeSet (tamagochiIdouTable [0], "walk", true);
					}

					for (int num = 1; num < 12; num++) {
						_pos = CharaTamago [tamagochiIdouTable [num - 1]].transform.localPosition;
						tamagoGameIdouWork [tamagochiIdouTable [num]].targetX = _pos.x;
						tamagoGameIdouWork [tamagochiIdouTable [num]].targetY = _pos.y;

						_pos = CharaTamago [tamagochiIdouTable [num]].transform.localPosition;
						tamagoGameIdouWork [tamagochiIdouTable [num]].speedX = (tamagoGameIdouWork [tamagochiIdouTable [num]].targetX - _pos.x) / tamagoIdouTime;
						tamagoGameIdouWork [tamagochiIdouTable [num]].speedY = (tamagoGameIdouWork [tamagochiIdouTable [num]].targetY - _pos.y) / tamagoIdouTime;

						if (tamagoGameIdouWork [tamagochiIdouTable [num]].targetX <= _pos.x) {
							TamagochiAnimeSet (tamagochiIdouTable [num], "walk", false);
						} else {
							TamagochiAnimeSet (tamagochiIdouTable [num], "walk", true);
						}
					}

					tamagoGameIdouCount++;
					break;
				}
			case	1:
				{
					for (int num = 0; num < 12; num++) {
						_pos = CharaTamago [tamagochiIdouTable [num]].transform.localPosition;
						_pos.x += tamagoGameIdouWork [tamagochiIdouTable [num]].speedX;
						_pos.y += tamagoGameIdouWork [tamagochiIdouTable [num]].speedY;

						if (tamagoGameIdouWork [tamagochiIdouTable [num]].speedX <= 0) {
							if (_pos.x <= tamagoGameIdouWork [tamagochiIdouTable [num]].targetX) {
								_pos.x = tamagoGameIdouWork [tamagochiIdouTable [num]].targetX;
							}
						} else {
							if (_pos.x >= tamagoGameIdouWork [tamagochiIdouTable [num]].targetX) {
								_pos.x = tamagoGameIdouWork [tamagochiIdouTable [num]].targetX;
							}
						}

						if (tamagoGameIdouWork [tamagochiIdouTable [num]].speedY <= 0) {
							if (_pos.y <= tamagoGameIdouWork [tamagochiIdouTable [num]].targetY) {
								_pos.y = tamagoGameIdouWork [tamagochiIdouTable [num]].targetY;
							}
						} else {
							if (_pos.y >= tamagoGameIdouWork [tamagochiIdouTable [num]].targetY) {
								_pos.y = tamagoGameIdouWork [tamagochiIdouTable [num]].targetY;
							}
						}

						CharaTamago [tamagochiIdouTable [num]].transform.localPosition = _pos;
					}

					_pos = CharaTamago [tamagochiIdouTable [0]].transform.localPosition;
					if ((_pos.x <= -4.0f) && (tamagoIdouTimeFlag == 0)) {
						tamagoGameIdouWork [tamagochiIdouTable [0]].targetX = -8.0f;
						tamagoGameIdouWork [tamagochiIdouTable [0]].targetY = -7.5f;
						_pos = CharaTamago [tamagochiIdouTable [0]].transform.localPosition;
						tamagoGameIdouWork [tamagochiIdouTable [0]].speedX = (tamagoGameIdouWork [tamagochiIdouTable [0]].targetX - _pos.x) / tamagoIdouTime3;
						tamagoGameIdouWork [tamagochiIdouTable [0]].speedY = (tamagoGameIdouWork [tamagochiIdouTable [0]].targetY - _pos.y) / tamagoIdouTime3;
						tamagoIdouTimeFlag = 1;
					}
					if ((_pos.x <= -8.0f) && (_pos.y <= -7.5f) && (tamagoIdouTimeFlag == 1)) {
						_flag = true;

						for (int num = 0; num < 12; num++) {
							_pos = CharaTamago [tamagochiIdouTable [num]].transform.localPosition;
							_pos.x = tamagoGameIdouWork [tamagochiIdouTable [num]].targetX;
							_pos.y = tamagoGameIdouWork [tamagochiIdouTable [num]].targetY;
							CharaTamago [tamagochiIdouTable [num]].transform.localPosition = _pos;
							TamagochiAnimeSet (tamagochiIdouTable [num], "idle");
						}

						int _hzn = tamagochiIdouTable [0];
						for (int num = 0; num < 11; num++) {
							tamagochiIdouTable [num] = tamagochiIdouTable [num + 1];
						}
						tamagochiIdouTable [11] = _hzn;

						tamagoGameIdouCount++;
					}
					break;
				}
			}

			return	_flag;
		}

		private void TamagochiMainAnimeSet(int num,string status){
			string _status = status;
			switch(_status){
			case	"glad1":
				{
					switch (Random.Range (0, 3)) {
					case	0:
						{
							_status = "glad1";
							break;
						}
					case	1:
						{
							_status = "glad2";
							break;
						}
					default:
						{
							_status = "glad3";
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
		private void TamagochiAnimeSet(int num,string status,bool flag = false){
			string _status = status;
			switch(_status){
			case	"glad1":
				{
					switch (Random.Range (0, 3)) {
					case	0:
						{
							_status = "glad1";
							break;
						}
					case	1:
						{
							_status = "glad2";
							break;
						}
					default:
						{
							_status = "glad3";
							break;
						}
					}
					break;
				}
			}

			if (cbCharaTamago [num].nowlabel != _status) {
				cbCharaTamago [num].gotoAndPlay (_status);
			}
			CharaTamago [num].GetComponent<SpriteRenderer> ().flipX = flag;
		}

		private void TamagochiDesignOff (){
			Vector3 _scaleZero = new Vector3 (0, 0, 0);
			for (int i = 0; i < EventStartTamago.Length; i++) {
				EventStartTamago [i].transform.localScale = _scaleZero;
			}
			for (int i = 0; i < EventGameTamago.Length; i++) {
				EventGameTamago [i].transform.localScale = _scaleZero;
			}
			for (int i = 0; i < EventEndTamago.Length; i++) {
				EventEndTamago [i].transform.localScale = _scaleZero;
			}
			for (int i = 0; i < EventResultTamago.Length; i++) {
				EventResultTamago [i].transform.localScale = _scaleZero;
			}
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
			posMenu.x = 1000.0f;
			EventGameMenu.transform.localPosition = posMenu;
			seikaiCount = 0;
			menuCount = 2;
			gameJobCount = statusGameCount.minigame2GameCount000;
			buttonCheckFlag = false;
			TamagochiGameIdouInit ();
			gameMainLoopFlag = false;
			fukidashiOff ();
			for (int i = 0; i < 3; i++) {
				TamagochiMainAnimeSet (i, "idle");
			}
		}

		private bool GameMainLoop(){
			switch (gameJobCount) {
			case	statusGameCount.minigame2GameCount000:
				{
					for (int i = 0; i < 8; i++) {
						ButtonMenu [i].SetActive (false);
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
					posMenu.x = 1000.0f;
					gameJobCount = statusGameCount.minigame2GameCount010;

					break;
				}
			case	statusGameCount.minigame2GameCount010:
				{
					posMenu.x -= MENU_IDOU_SPEED;
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
							for (int i = 0; i < 3; i++) {
								TamagochiMainAnimeSet (i, "glad1");
							}
							TamagochiAnimeSet (tamagochiIdouTable[0], "glad1");
							ScoreInit ();
						} else {
							gameJobCount = statusGameCount.minigame2GameCount100;
							TamagochiMainAnimeSet (0, "shock");
						}
					}
					break;
				}
			case	statusGameCount.minigame2GameCount030:
				{
					if (ScoreAnime ()) {
						gameJobCount = statusGameCount.minigame2GameCount040;
						fukidashiOff ();
						for (int i = 0; i < 3; i++) {
							TamagochiMainAnimeSet (i, "idle");
						}
					}
					break;
				}
			case	statusGameCount.minigame2GameCount040:
				{
					posMenu.x -= MENU_IDOU_SPEED;
					if (posMenu.x <= -1000.0f) {
						posMenu.x = -1000.0f;
						gameJobCount = statusGameCount.minigame2GameCount050;
						tamagoGameIdouCount = 0;
					}
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
					if (seikaiCount == 5) {
						menuCount = 4;
					}
					if (seikaiCount == 10) {
						menuCount = 6;
					}
					if (seikaiCount == 15) {
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
			EventEnd.transform.Find ("points/Text").gameObject.GetComponent<Text> ().text = nowScore.ToString ();
			EventResult.transform.Find ("points/Text").gameObject.GetComponent<Text> ().text = nowScore.ToString ();
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

			} else {
				EventGameFukidashi.transform.Find ("hantei").gameObject.GetComponent<Image> ().sprite = CheckImage [1];						// ０：丸、１：バツ
				_flag = false;
			}
			EventGameFukidashi.transform.Find ("hantei").gameObject.SetActive (true);

			return _flag;
		}
		private void fukidashiOff(){
			EventGameFukidashi.transform.Find ("fukidashi").gameObject.SetActive (false);
			EventGameFukidashi.transform.Find ("menu").gameObject.SetActive (false);
			EventGameFukidashi.transform.Find ("hantei").gameObject.SetActive (false);
		}

		private int scoreAnimeCount;
		private int scoreAnimeWait;
		private void ScoreInit(){
			EventGameScore.SetActive (true);
			EventGameScore.transform.localPosition = new Vector3 (0, 0, 0);
			scoreAnimeCount = 0;
		}
		private bool ScoreAnime(){
			Vector3 _pos = EventGameScore.transform.localPosition;
			bool _flag = false;

			switch (scoreAnimeCount) {
			case	0:
				{
					_pos.y += 5.0f;
					if (_pos.y >= 50.0f) {
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
			}

			EventGameScore.transform.localPosition = _pos;
			return _flag;
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

					EventResult.transform.Find ("chara").gameObject.transform.localPosition = new Vector3 (60.0f, -185.0f, 0.0f);
					EventResult.transform.Find ("chara2").gameObject.transform.localPosition = new Vector3 (-202.0f, -185.0f, 0.0f);
					EventResult.transform.Find ("chara3").gameObject.transform.localPosition = new Vector3 (-88.0f, -185.0f, 0.0f);

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
					}
					break;
				}
			case	statusResult.resultJobCount030:
				{
					if (ResultWaitTimeSubLoop ()) {															// スコアなどを見せる
						resultLoopCount = statusResult.resultJobCount040;
						if (nowScore < 50) {
							resultItemGetFlag = true;														// ５０点未満ならアイテム入手できないのでそのまま終了
							resultLoopCount = statusResult.resultJobCount090;
						}
					}
					break;
				}
			case	statusResult.resultJobCount040:{
					pos = EventResult.transform.Find ("treasure").gameObject.transform.localPosition;		// 落下する宝箱の座標を抽出
					pos.y -= 25;
					if (pos.y <= -180) {
						pos.y = -180;
						resultLoopCount = statusResult.resultJobCount050;
						resultLoopWait = treasureRotationTable.Length;
						resultTamagoYJumpCount = tamagoYJumpTable.Length;
					}
					EventResult.transform.Find ("treasure").gameObject.transform.localPosition = pos;		// 落下する宝箱の座標を設定
					break;
				}
			case	statusResult.resultJobCount050:
				{
					if (ResultWaitTimeSubLoop ()) {
						resultLoopCount = statusResult.resultJobCount060;
					}
					pos = EventResult.transform.Find ("treasure").gameObject.transform.eulerAngles;			// 宝箱を震えさせる
					pos.z = treasureRotationTable [resultLoopWait];
					EventResult.transform.Find ("treasure").gameObject.transform.eulerAngles = pos;

					break;
				}
			case	statusResult.resultJobCount060:
				{
					EventResult.transform.Find ("treasure").gameObject.SetActive (false);					// 宝箱を開いたものに変える
					EventResult.transform.Find ("treasure_open").gameObject.SetActive (true);
					resultLoopCount = statusResult.resultJobCount070;
					resultLoopWait = 60;
					break;
				}
			case	statusResult.resultJobCount070:
				{
					if (ResultWaitTimeSubLoop ()) {															// 開いた宝箱を見せる
						resultLoopCount = statusResult.resultJobCount080;
					}
					break;
				}
			case	statusResult.resultJobCount080:
				{
					EventResult.SetActive (false);															// アイテム入手画面を開く
					EventItemget.SetActive (true);
					resultItemGetFlag = false;
					resultLoopCount = statusResult.resultJobCount090;
					break;
				}
			case	statusResult.resultJobCount090:
				{
					if (resultItemGetFlag) {
						EventItemget.SetActive (false);														// アイテム入手画面を消す
						EventResult.SetActive (true);														// 結果画面を表示する
						EventResult.transform.Find ("Button_blue_modoru").gameObject.SetActive (true);		// 戻るボタンを表示する
						if (nowScore >= 50) {
							for (int i = 0; i < 3; i++) {													// ５０点以上ならアイテム入手したのでたまごっちを喜ばす
								TamagochiMainAnimeSet (i, "glad1");
							}
						}
							
						resultLoopCount = statusResult.resultJobCount100;
					}
					break;
				}
			case	statusResult.resultJobCount100:
				{
					break;
				}
			}

			if (resultTamagoYJumpCount != 0) {																// 落ちて来た宝箱に跳ね飛ばされるたまごっち達
				resultTamagoYJumpCount--;
				pos = EventResult.transform.Find ("chara").gameObject.transform.localPosition;
				pos.x += 2.0f;
				pos.y += tamagoYJumpTable [resultTamagoYJumpCount];
				EventResult.transform.Find ("chara").gameObject.transform.localPosition = pos;
				pos = EventResult.transform.Find ("chara2").gameObject.transform.localPosition;
				pos.x -= 2.0f;
				pos.y += tamagoYJumpTable [resultTamagoYJumpCount];
				EventResult.transform.Find ("chara2").gameObject.transform.localPosition = pos;
				pos = EventResult.transform.Find ("chara3").gameObject.transform.localPosition;
				pos.x -= 2.0f;
				pos.y += tamagoYJumpTable [resultTamagoYJumpCount];
				EventResult.transform.Find ("chara3").gameObject.transform.localPosition = pos;
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

		private void TamagoAnimeSprite(){
			EventResult.transform.Find ("chara").gameObject.GetComponent<Image> ().sprite = CharaTamagoMain [0].GetComponent<SpriteRenderer> ().sprite;
			EventResult.transform.Find ("chara2").gameObject.GetComponent<Image> ().sprite = CharaTamagoMain [1].GetComponent<SpriteRenderer> ().sprite;
			EventResult.transform.Find ("chara3").gameObject.GetComponent<Image> ().sprite = CharaTamagoMain [2].GetComponent<SpriteRenderer> ().sprite;

			EventResult.transform.Find ("chara").transform.localScale = new Vector3 (2.5f, 2.5f, 1.0f);
			EventResult.transform.Find ("chara2").transform.localScale = new Vector3 (-2.5f, 2.5f, 1.0f);
			EventResult.transform.Find ("chara3").transform.localScale = new Vector3 (-2.5f, 2.5f, 1.0f);
		}

		private Vector2[] tamagoCharaPositionInitTable = new Vector2[] {
			new Vector2 (   0.0f,  420.0f),		// 結果画面の宝箱の初期位置
			new Vector2 (   0.0f,  420.0f),		// 結果画面のメッセージの初期位置
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



	}

	struct TamagoIdou
	{
		public float targetX;
		public float targetY;
		public float speedX;
		public float speedY;
	}



}