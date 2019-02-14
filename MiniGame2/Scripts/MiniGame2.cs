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
		[SerializeField] private GameObject MinigameRoot;
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
		[SerializeField] private Sprite[] MenuImage;							// ０：カツ丼、１：プリン、２：サンド、３：ステーキ、４：パスタ、５：オムライス、６：ご飯、７：寿司
		[SerializeField] private Sprite[] FukidashiImage;						// ０：吹き出し１、１：吹き出し２、２：吹き出し３、３：吹き出し４
		[SerializeField] private Sprite[] CheckImage;							// ０：丸、１：バツ
		[SerializeField] private Sprite[] EventEndSprite;					// 終了時の演出スプライト



		private object[]		mparam;

		private CharaBehaviour[] cbCharaTamagoMain = new CharaBehaviour[3];		// プレイヤーとゲスト２名
		private CharaBehaviour[] cbCharaTamago = new CharaBehaviour[12];		// お客様
		private bool startEndFlag = false;
		private bool screenModeFlag = true;
		private int waitCount;
		private int nowScore;													// 得点
		private int nowScore2;
		private float nowTime1;													// 残り時間（カウントダウン用）
		private int	nowTime2;													// 残り時間（制限時間）

		private readonly float MENU_IDOU_SPEED = 30.0f;							// メニューの移動速度
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
			if (mparam==null) {
				mparam = new object[] {
					1,						// ミニゲームID
					0,						// イベントID
				};
			}


			GameCall call = new GameCall (CallLabel.GET_MINIGAME_INFO,2,mparam[0],mparam[1]);
			call.AddListener (mGetMinigameInfo);
			ManagerObject.instance.connect.send (call);
		}

		void Start() {
			Debug.Log ("MiniGame2 Start");
		}

		private MinigameData mData;
		private MinigameResultData mResultData;
		void mGetMinigameInfo(bool success,object data){
			mData = (MinigameData)data;
			StartCoroutine(mStart());
		}


		IEnumerator mStart(){
			Debug.Log ("MiniGame2 mStart");

			muser1 = ManagerObject.instance.player;		// たまごっち





			ManagerObject.instance.sound.playBgm (22);

			MinigameRoot.transform.Find ("bg").gameObject.SetActive (true);

			startEndFlag = false;

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
				screenModeFlag = true;
			} else {
				screenModeFlag = false;
			}

			for (int i = 0; i < 3; i++) {
				cbCharaTamagoMain [i] = CharaTamagoMain [i].GetComponent<CharaBehaviour> ();
			}
			for (int i = 0; i < 12; i++) {
				cbCharaTamago [i] = CharaTamago [i].GetComponent<CharaBehaviour> ();
			}

			yield return cbCharaTamagoMain [0].init (muser1.chara1);
			yield return cbCharaTamagoMain [1].init (new TamaChara (17));
			yield return cbCharaTamagoMain [2].init (new TamaChara (18));

			TamagochiMainAnimeSet (0, MotionLabel.IDLE);
			TamagochiMainAnimeSet (1, MotionLabel.IDLE);
			TamagochiMainAnimeSet (2, MotionLabel.IDLE);

			yield return cbCharaTamago[0].init (new TamaChara (19));
			yield return cbCharaTamago[1].init (new TamaChara (20));
			yield return cbCharaTamago[2].init (new TamaChara (21));
			yield return cbCharaTamago[3].init (new TamaChara (22));
			yield return cbCharaTamago[4].init (new TamaChara (23));
			yield return cbCharaTamago[5].init (new TamaChara (24));
			yield return cbCharaTamago[6].init (new TamaChara (25));
			yield return cbCharaTamago[7].init (new TamaChara (26));
			yield return cbCharaTamago[8].init (new TamaChara (27));
			yield return cbCharaTamago[9].init (new TamaChara (28));
			yield return cbCharaTamago[10].init (new TamaChara (29));
			yield return cbCharaTamago[11].init (new TamaChara (30));
			TamagochiAnimeSet (0, MotionLabel.IDLE);
			TamagochiAnimeSet (1, MotionLabel.IDLE);
			TamagochiAnimeSet (2, MotionLabel.IDLE);
			TamagochiAnimeSet (3, MotionLabel.IDLE);
			TamagochiAnimeSet (4, MotionLabel.IDLE);
			TamagochiAnimeSet (5, MotionLabel.IDLE);
			TamagochiAnimeSet (6, MotionLabel.IDLE);
			TamagochiAnimeSet (7, MotionLabel.IDLE);
			TamagochiAnimeSet (8, MotionLabel.IDLE);
			TamagochiAnimeSet (9, MotionLabel.IDLE);
			TamagochiAnimeSet (10, MotionLabel.IDLE);
			TamagochiAnimeSet (11, MotionLabel.IDLE);


			StartCoroutine ("TamagochiSortLoop");

			startEndFlag = true;
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
						EventTitle.SetActive (true);
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
					EventTitle.SetActive (false);
					EventStart.SetActive (true);
					TamagochiLoopInit ();
					jobCount = statusJobCount.minigame2JobCount030;

					ManagerObject.instance.sound.playSe (20);
					break;
				}
			case	statusJobCount.minigame2JobCount030:
				{
					if (EventStart.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.minigame2JobCount040;
						EventStart.SetActive (false);
						EventGame.SetActive (true);
						GameMainInit ();

//						ManagerObject.instance.sound.playBgm (22);
					}
					break;
				}
			case	statusJobCount.minigame2JobCount040:
				{
					if (GameMainLoop ()) {											// ゲーム処理
						jobCount = statusJobCount.minigame2JobCount050;
						waitCount = 45;

						waitResultFlag = false;
						GameCall call = new GameCall (CallLabel.GET_MINIGAME_RESULT,mData.mid,nowScore);
						call.AddListener (mGetMinigameResult);
						ManagerObject.instance.connect.send (call);
					}
					break;
				}
			case	statusJobCount.minigame2JobCount050:
				{
					if (waitResultFlag) {
						waitCount--;
					}
					if (waitCount == 0) {											// 驚きを見せるためのウエィト
						jobCount = statusJobCount.minigame2JobCount060;

						int _num = 0;
						if (nowScore < 100) {
							_num = 0;
						} else if (nowScore < 200) {
							_num = 1;
						} else if (nowScore < 300) {
							_num = 2;
						} else if (nowScore < 500) {
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
			case	statusJobCount.minigame2JobCount060:
				{
					if (EventEnd.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {	// お疲れ様のアニメが終了するまで待つ
						jobCount = statusJobCount.minigame2JobCount070;
						EventEnd.SetActive (false);
						EventResult.SetActive (true);

						for (int i = 0; i < CharaTamago.Length; i++) {				// お客さん達を消す
							CharaTamago [i].transform.localPosition = new Vector3 (5000.0f, 5000.0f, 0.0f);
						}
						for (int i = 0; i < CharaTamagoMain.Length; i++) {			// プレイヤーとゲストを消す
							CharaTamagoMain [i].transform.localPosition = new Vector3 (5000.0f, 5000.0f, 0.0f);
							TamagochiMainAnimeSet (i, MotionLabel.IDLE);
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
			Debug.Log ("たまタウンへ・・・");
			ManagerObject.instance.view.change(SceneLabel.TOWN);
		}
		private void ButtonHelpClick(){
			EventHelp.SetActive (true);

			ManagerObject.instance.sound.playSe (11);
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
			Vector2[] _initTable = new Vector2[] {
				new Vector2 (0.0f, 380.0f),
				new Vector2 (-300.0f, 380.0f),
				new Vector2 (300.0f, 380.0f),
			};

			for (int i = 0; i < CharaTamago.Length; i++) {
				CharaTamago [i].SetActive (true);
			}

			for (int i = 0; i < CharaTamagoMain.Length; i++) {
				CharaTamagoMain [i].SetActive (true);
				TamagochiMainAnimeSet (i, MotionLabel.IDLE);
				Vector3 pos = CharaTamagoMain [i].transform.localPosition;
				pos.x = _initTable [i].x;
				pos.y = _initTable [i].y;
				CharaTamagoMain [i].transform.localPosition = pos;
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

			CharaTamago [num].transform.localPosition = _idouPosTable [num2 + 1];
			for (int i = 0; i < 8; i++) {
				while (true) {
					TamagochiAnimeFlipSet (num, MotionLabel.WALK, _idouPosTable [num2 - i]);

					CharaTamago [num].transform.localPosition = Vector3.MoveTowards (CharaTamago [num].transform.localPosition, _idouPosTable [num2 - i], 700 * Time.deltaTime);
					if ((CharaTamago [num].transform.localPosition.x == _idouPosTable [num2 - i].x) && (CharaTamago [num].transform.localPosition.y == _idouPosTable [num2 - i].y)) {
						break;
					}
					yield return null;
				}
			}
			TamagochiAnimeSet (num, MotionLabel.IDLE, false);
						
		}




		private Vector3[] _NextPos = new Vector3[12];
		private int _NextCounter;
		private bool _NextFlag;
		private IEnumerator TamagochiNextIdou(){
			_NextPos [tamagochiIdouTable [0]] = new Vector3 (-500.0f, -250.0f, 0.0f);

			for (int i = 1; i < 12; i++) {
				_NextPos [i] = CharaTamago [tamagochiIdouTable [i - 1]].transform.localPosition;
			}

			StartCoroutine(TamagoNextIdou0(tamagochiIdouTable[0],_NextPos[0]));
			for (int i = 1; i < 12; i++) {
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
				CharaTamago [num].transform.localPosition = Vector3.MoveTowards (CharaTamago [num].transform.localPosition, pos, 700 * Time.deltaTime);
				if((CharaTamago[num].transform.localPosition.x == pos.x) && (CharaTamago[num].transform.localPosition.y == pos.y)){
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
				CharaTamago [num].transform.localPosition = Vector3.MoveTowards (CharaTamago [num].transform.localPosition, pos2, 700 * Time.deltaTime);
				if((CharaTamago[num].transform.localPosition.x == pos2.x) && (CharaTamago[num].transform.localPosition.y == pos2.y)){
					break;
				}
				yield return null;
			}
			TamagochiAnimeSet (num, MotionLabel.IDLE);
		}
		private IEnumerator TamagoNextIdou(int num,Vector3 pos){
			while(true){
				if (gameMainLoopFlag) {
					break;
				}
				TamagochiAnimeFlipSet (num, MotionLabel.WALK, pos);
				CharaTamago [num].transform.localPosition = Vector3.MoveTowards (CharaTamago [num].transform.localPosition, pos, 700 * Time.deltaTime);
				if((CharaTamago[num].transform.localPosition.x == pos.x) && (CharaTamago[num].transform.localPosition.y == pos.y)){
					break;
				}
				yield return null;
			}
			_NextCounter++;
			TamagochiAnimeSet (num, MotionLabel.IDLE);
		}


		// たまごっちの表示優先順位を変更する
		private IEnumerator TamagochiSortLoop(){
			float[] _posY = new float[12];

			while (true) {
				for (int i = 0; i < 12; i++) {
					_posY [i] = CharaTamago [i].transform.localPosition.y;
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
					CharaTamago [k].GetComponent<Canvas> ().sortingOrder = j + 1;
				}

				yield return null;
			}
		}




		private int[] tamagochiIdouTable = new int[12];
		private void TamagochiGameIdouInit(){
			for (int i = 0; i < 12; i++) {
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

			if (_NextCounter == 12) {
				int _hzn = tamagochiIdouTable [0];
				for (int num = 0; num < 11; num++) {
					tamagochiIdouTable [num] = tamagochiIdouTable [num + 1];
				}
				tamagochiIdouTable [11] = _hzn;

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
		private void TamagochiAnimeSet(int num,string status,bool flag = false){
			string _status = status;
			switch (_status) {
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

			if (cbCharaTamago [num].nowlabel != _status) {
				cbCharaTamago [num].gotoAndPlay (_status);
			}
			if (flag) {
				CharaTamago [num].transform.localScale = new Vector3 (-6.0f, 6.0f, 1.0f);
			} else {
				CharaTamago [num].transform.localScale = new Vector3 (6.0f, 6.0f, 1.0f);
			}
		}
		private void TamagochiAnimeFlipSet(int _num,string _anime,Vector3 pos){
			if (CharaTamago [_num].transform.localPosition.x >= pos.x) {
				TamagochiAnimeSet (_num, _anime, false);
			} else {
				TamagochiAnimeSet (_num, _anime, true);
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
			posMenu.x = 1500.0f;
			EventGameMenu.transform.localPosition = posMenu;
			seikaiCount = 0;
			menuCount = 2;
			gameJobCount = statusGameCount.minigame2GameCount000;
			buttonCheckFlag = false;
			TamagochiGameIdouInit ();
			gameMainLoopFlag = false;
			fukidashiOff ();
			for (int i = 0; i < 3; i++) {
				TamagochiMainAnimeSet (i, MotionLabel.IDLE);
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
							for (int i = 0; i < 3; i++) {
								TamagochiMainAnimeSet (i, MotionLabel.GLAD1);
							}
							TamagochiAnimeSet (tamagochiIdouTable[0], MotionLabel.GLAD1);
							ScoreInit ();
						} else {
							gameJobCount = statusGameCount.minigame2GameCount100;
							TamagochiMainAnimeSet (0, MotionLabel.SHOCK);
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
							TamagochiMainAnimeSet (i, MotionLabel.IDLE);
						}
					}
					break;
				}
			case	statusGameCount.minigame2GameCount040:
				{
					posMenu.x -= (MENU_IDOU_SPEED * (60 * Time.deltaTime));
					if (posMenu.x <= -1500.0f) {
						posMenu.x = -1500.0f;
						gameJobCount = statusGameCount.minigame2GameCount050;
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
			EventGameScore.transform.localPosition = new Vector3 (-50.0f, -100.0f, 0);
			scoreAnimeCount = 0;
		}
		private bool ScoreAnime(){
			Vector3 _pos = EventGameScore.transform.localPosition;
			bool _flag = false;

			switch (scoreAnimeCount) {
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

					if ((nowScore == 0) || (!mResultData.rewardFlag)) {
						EventResult.transform.Find ("chara").gameObject.transform.localPosition = new Vector3 (250.0f, -320.0f, 0.0f);
						EventResult.transform.Find ("chara2").gameObject.transform.localPosition = new Vector3 (-400.0f, -320.0f, 0.0f);
						EventResult.transform.Find ("chara3").gameObject.transform.localPosition = new Vector3 (-250.0f, -320.0f, 0.0f);
					} else {
						EventResult.transform.Find ("chara").gameObject.transform.localPosition = new Vector3 (120.0f, -320.0f, 0.0f);
						EventResult.transform.Find ("chara2").gameObject.transform.localPosition = new Vector3 (-280.0f, -320.0f, 0.0f);
						EventResult.transform.Find ("chara3").gameObject.transform.localPosition = new Vector3 (-120.0f, -320.0f, 0.0f);
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
							ManagerObject.instance.sound.playSe (9);
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
					EventItemget.transform.Find ("GotchiView").gameObject.SetActive (false);
					EventItemget.transform.Find ("getitem_text").gameObject.SetActive (false);
					EventItemget.transform.Find ("ItemView").gameObject.SetActive (false);

					if (mResultData.reward.kind == RewardKind.ITEM) {
						// アイテムが褒賞品
						EventItemget.transform.Find ("getitem_text").gameObject.SetActive (true);
						EventItemget.transform.Find ("ItemView").gameObject.SetActive (true);

						ItemBehaviour ibItem = EventItemget.transform.Find ("ItemView").gameObject.GetComponent<ItemBehaviour> ();
						ibItem.init (mResultData.reward.item);
					} else {
						// ごっちポイントが褒賞品
						EventItemget.transform.Find ("getpoints_text").gameObject.SetActive (true);
						EventItemget.transform.Find ("GotchiView").gameObject.SetActive (true);

						GotchiBehaviour gbPoint = EventItemget.transform.Find ("GotchiView").gameObject.GetComponent<GotchiBehaviour> ();
						gbPoint.init (mResultData.reward.point);
					}

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
							for (int i = 0; i < 3; i++) {													// 褒賞品があるのでたまごっちを喜ばす
								TamagochiMainAnimeSet (i, MotionLabel.GLAD1);
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
				pos = EventResult.transform.Find ("chara2").gameObject.transform.localPosition;
				pos.x -= 4.0f;
				pos.y += tamagoYJumpTable [resultTamagoYJumpCount];
				EventResult.transform.Find ("chara2").gameObject.transform.localPosition = pos;
				pos = EventResult.transform.Find ("chara3").gameObject.transform.localPosition;
				pos.x -= 4.0f;
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
			EventResult.transform.Find ("chara2").transform.localScale = new Vector3 (-2.0f, 2.0f, 1.0f);
			EventResult.transform.Find ("chara3").transform.localScale = new Vector3 (-2.0f, 2.0f, 1.0f);

			TamagochiImageMove (EventResult, CharaTamagoMain [0], "chara/");
			TamagochiImageMove (EventResult, CharaTamagoMain [1], "chara2/");
			TamagochiImageMove (EventResult, CharaTamagoMain [2], "chara3/");
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

		private void TamagochiImageMove(GameObject toObj,GameObject fromObj,string toStr){
			for (int i = 0; i < fromObj.transform.Find ("Layers").transform.childCount; i++) {
				toObj.transform.Find (toStr + "CharaImg/Layers/" + fromObj.transform.Find ("Layers").transform.GetChild (i).name).gameObject.transform.SetSiblingIndex (i);
			}

			toObj.transform.Find (toStr + "CharaImg").gameObject.GetComponent<Image> ().enabled = false;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer0").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer0").gameObject.GetComponent<Image> ().enabled;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer1").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer1").gameObject.GetComponent<Image> ().enabled;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer2").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer2").gameObject.GetComponent<Image> ().enabled;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer3").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer3").gameObject.GetComponent<Image> ().enabled;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer4").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer4").gameObject.GetComponent<Image> ().enabled;
				
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer0").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer0").gameObject.GetComponent<Image> ().sprite;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer1").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer1").gameObject.GetComponent<Image> ().sprite;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer2").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer2").gameObject.GetComponent<Image> ().sprite;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer3").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer3").gameObject.GetComponent<Image> ().sprite;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer4").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer4").gameObject.GetComponent<Image> ().sprite;

			toObj.transform.Find (toStr + "CharaImg/Layers/Layer0").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer0").gameObject.transform.localPosition;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer1").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer1").gameObject.transform.localPosition;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer2").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer2").gameObject.transform.localPosition;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer3").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer3").gameObject.transform.localPosition;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer4").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer4").gameObject.transform.localPosition;

			toObj.transform.Find (toStr + "CharaImg/Layers/Layer0").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer0").gameObject.transform.localScale;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer1").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer1").gameObject.transform.localScale;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer2").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer2").gameObject.transform.localScale;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer3").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer3").gameObject.transform.localScale;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer4").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer4").gameObject.transform.localScale;
		}

			
	}


}