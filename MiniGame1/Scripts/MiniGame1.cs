using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;







namespace Mix2App.MiniGame1{
	public class MiniGame1 : MonoBehaviour,IReceiver {
		[SerializeField] private GameCore	pgGameCore;
		[SerializeField] private GameObject MinigameRoot;
		[SerializeField] private GameObject[] CharaTamago;					// たまごっち
		[SerializeField] private GameObject EventTitle;						// タイトル画面
		[SerializeField] private GameObject EventStart;						// スタート画面
		[SerializeField] private GameObject EventGame;						// ゲーム画面
		[SerializeField] private GameObject EventEnd;						// 終了画	面
		[SerializeField] private GameObject EventResult;					// 結果画面
		[SerializeField] private GameObject	EventItemget;					// アイテム入手画面
		[SerializeField] private GameObject	EventHelp;						// 遊び方説明画面
		[SerializeField] private GameObject ButtonStart;					// タイトル スタートボタン
		[SerializeField] private GameObject ButtonHelp;						// タイトル ヘルプボタン
		[SerializeField] private GameObject ButtonClose;					// タイトル 閉じるボタン
		[SerializeField] private GameObject ButtonYameru;					// ゲーム やめるボタン
		[SerializeField] private GameObject ButtonTakuhai;					// アイテム入手 宅配ボタン
		[SerializeField] private GameObject ButtonTojiru;					// アイテム入手 閉じるボタン
		[SerializeField] private GameObject ButtonModoru;					// 結果 戻るボタン
		[SerializeField] private GameObject ButtonHelpModoru;				// 遊び方説明画面 戻るボタン

		[SerializeField] private GameObject baseSizePanel;

		[Tooltip("四季の画像データ（春、夏、秋、冬）")]
		[SerializeField] private SeasonImg[] SeasonData;

		[SerializeField] private GameObject PrefabItem;
		[SerializeField] private GameObject PrefabScore;
		[SerializeField] private GameObject PrefabMessage;

		[SerializeField] private Sprite[] EventEndSprite;

		private object[]		mparam;



		private CharaBehaviour[] cbCharaTamago = new CharaBehaviour[4];		// 
		private bool startEndFlag = false;
		private int waitCount = 0;
		private int nowScore;													// 得点
		private int nowScore2;
		private float nowTime1;													// 残り時間（カウントダウン用）
		private int	nowTime2;													// 残り時間（制限時間）

		private int charaAnimeFlag = 0;										// 0:idel,1:r-idou,2:l-idou
		private int itemIdouFlag = 0;										// 0;init,1:d-idou,2:
		private int itemGetNumber = 0;
		private int charaJumpCheckFlag = 0;									// jumpFlag
//		private int charaJumpCheckFlag2 = 0;								// jumpFlag2
		private float charaJumpCheckFlag2 = 0.0f;

		private statusJobCount	jobCount = statusJobCount.minigame1JobCount000;
		private enum statusJobCount{
			minigame1JobCount000,
			minigame1JobCount010,
			minigame1JobCount020,
			minigame1JobCount030,
			minigame1JobCount040,
			minigame1JobCount050,
			minigame1JobCount060,
			minigame1JobCount070,
			minigame1JobCount080,
			minigame1JobCount090,
			minigame1JobCount100,
			minigame1JobCount110,
		}

		// 出現間隔（秒）,落下速度（秒）,アイテム1（％）,アイテム2（％）,アイテム3（％）,アイテム4（％）,アイテム5（％）,おじゃま（％）
		private float[,] itemTable = new float[30, 8] {
			{ 2.0f, 6.0f, 60.0f, 40.0f, 00.0f, 00.0f, 00.0f, 00.0f },
			{ 2.0f, 6.0f, 60.0f, 40.0f, 00.0f, 00.0f, 00.0f, 00.0f },
			{ 2.0f, 5.8f, 50.0f, 50.0f, 00.0f, 00.0f, 00.0f, 00.0f },
			{ 2.0f, 5.8f, 50.0f, 50.0f, 00.0f, 00.0f, 00.0f, 00.0f },
			{ 2.0f, 5.6f, 40.0f, 40.0f, 20.0f, 00.0f, 00.0f, 00.0f },
			{ 2.0f, 5.6f, 40.0f, 40.0f, 20.0f, 00.0f, 00.0f, 00.0f },
			{ 1.8f, 5.4f, 40.0f, 35.0f, 20.0f, 00.0f, 00.0f, 05.0f },
			{ 1.8f, 5.4f, 40.0f, 35.0f, 20.0f, 00.0f, 00.0f, 05.0f },
			{ 1.8f, 5.2f, 30.0f, 25.0f, 20.0f, 15.0f, 00.0f, 10.0f },
			{ 1.8f, 5.2f, 30.0f, 25.0f, 20.0f, 15.0f, 00.0f, 10.0f },
			{ 1.8f, 5.0f, 25.0f, 25.0f, 25.0f, 10.0f, 05.0f, 10.0f },
			{ 1.8f, 5.0f, 25.0f, 25.0f, 25.0f, 10.0f, 05.0f, 10.0f },
			{ 1.5f, 4.8f, 25.0f, 25.0f, 20.0f, 15.0f, 05.0f, 10.0f },
			{ 1.5f, 4.8f, 25.0f, 25.0f, 20.0f, 15.0f, 05.0f, 10.0f },
			{ 1.5f, 4.6f, 20.0f, 20.0f, 20.0f, 15.0f, 10.0f, 15.0f },
			{ 1.5f, 4.6f, 20.0f, 20.0f, 20.0f, 15.0f, 10.0f, 15.0f },
			{ 1.2f, 4.4f, 15.0f, 15.0f, 20.0f, 20.0f, 15.0f, 15.0f },
			{ 1.2f, 4.4f, 15.0f, 15.0f, 20.0f, 20.0f, 15.0f, 15.0f },
			{ 1.2f, 4.2f, 10.0f, 15.0f, 20.0f, 20.0f, 20.0f, 15.0f },
			{ 1.2f, 4.2f, 10.0f, 15.0f, 20.0f, 20.0f, 20.0f, 15.0f },
			{ 1.0f, 4.0f, 10.0f, 10.0f, 15.0f, 15.0f, 30.0f, 20.0f },
			{ 1.0f, 4.0f, 10.0f, 10.0f, 15.0f, 15.0f, 30.0f, 20.0f },
			{ 1.0f, 4.0f, 10.0f, 10.0f, 15.0f, 15.0f, 30.0f, 20.0f },
			{ 1.0f, 4.0f, 10.0f, 10.0f, 15.0f, 15.0f, 30.0f, 20.0f },
			{ 1.0f, 4.0f, 10.0f, 10.0f, 15.0f, 15.0f, 30.0f, 20.0f },
			{ 1.0f, 4.0f, 10.0f, 10.0f, 15.0f, 15.0f, 30.0f, 20.0f },
			{ 1.0f, 4.0f, 10.0f, 10.0f, 15.0f, 15.0f, 30.0f, 20.0f },
			{ 1.0f, 4.0f, 10.0f, 10.0f, 15.0f, 15.0f, 30.0f, 20.0f },
			{ 1.0f, 4.0f, 10.0f, 10.0f, 15.0f, 15.0f, 30.0f, 20.0f },
			{ 1.0f, 4.0f, 10.0f, 10.0f, 15.0f, 15.0f, 30.0f, 20.0f },
		};


		private User muser1;//自分

		void Awake(){
			Debug.Log ("MiniGame1 Awake");
			mparam=null;
			muser1=null;
		}

		public void receive(params object[] parameter){
			Debug.Log ("MiniGame1 receive");
			mparam = parameter;

			//単体動作テスト用
			//パラメタ詳細は設計書参照
			if (mparam==null) {
				mparam = new object[] {
					1,						// ミニゲームID
					Random.Range(0,4) + 1,	// 季節ID：１：春、２：夏、３：秋、４：冬
				};
			}

			GameCall call = new GameCall (CallLabel.GET_MINIGAME_INFO,1,mparam[0],mparam[1]);
			call.AddListener (mGetMinigameInfo);
			ManagerObject.instance.connect.send (call);
		}

		void Start(){
			Debug.Log ("MiniGame1 Start");
		}

		private MinigameData mData;
		private MinigameResultData mResultData;
		void mGetMinigameInfo(bool success,object data){
			Debug.Log(data);
			mData = (MinigameData)data;
			StartCoroutine(mStart());
		}


		private float useScreenX, useScreenY;

		IEnumerator mStart(){
			muser1 = ManagerObject.instance.player;		// たまごっち

			if (Random.Range (0, 2) == 0) {
				muser1.chara2 = null;
			}

			SeasonImageSet();



			ManagerObject.instance.sound.playBgm (21);

			MinigameRoot.transform.Find ("base/bg").gameObject.SetActive (true);


			jobCount = statusJobCount.minigame1JobCount000;
			startEndFlag = false;

			ButtonStart.GetComponent<Button> ().onClick.AddListener (ButtonStartClick);
			ButtonClose.GetComponent<Button> ().onClick.AddListener (ButtonCloseClick);
			ButtonHelp.GetComponent<Button> ().onClick.AddListener (ButtonHelpClick);
			ButtonHelpModoru.GetComponent<Button> ().onClick.AddListener (ButtonHelpModoruClick);
			ButtonYameru.GetComponent<Button> ().onClick.AddListener (ButtonYameruClick);
			ButtonTakuhai.GetComponent<Button> ().onClick.AddListener (ButtonTakuhaiClick);
			ButtonTojiru.GetComponent<Button> ().onClick.AddListener (ButtonTojiruClick);
			ButtonModoru.GetComponent<Button> ().onClick.AddListener (ButtonModoruClick);


			float use_screen_x = Screen.currentResolution.width;
			float use_screen_y = Screen.currentResolution.height;
			#if UNITY_EDITOR
			use_screen_x = Screen.width;
			use_screen_y = Screen.height;
			#endif

			useScreenX = use_screen_x;
			useScreenY = use_screen_y;

			float num;
			if (use_screen_x >= use_screen_y) {
				num = use_screen_x / use_screen_y;
			} else {
				num = use_screen_y / use_screen_x;
			}

			if ((num > 1.33f) && (num < 1.34f)) {
				baseSizePanel.GetComponent<Transform> ().transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);	// 3:4の時のみ画面を拡大表示
			} else {
				baseSizePanel.GetComponent<Transform> ().transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);	// 3:4の時以外の画面を拡大表示
			}



			cbCharaTamago[0] = CharaTamago[0].GetComponent<CharaBehaviour> ();		// プレイヤー
			cbCharaTamago[1] = CharaTamago[1].GetComponent<CharaBehaviour> ();		// 応援キャラ１
			cbCharaTamago[2] = CharaTamago[2].GetComponent<CharaBehaviour> ();		// 応援キャラ２
			cbCharaTamago[3] = CharaTamago[3].GetComponent<CharaBehaviour> ();		// 双子プレイヤー
			yield return cbCharaTamago[0].init (muser1.chara1);
			yield return cbCharaTamago[1].init (new TamaChara (17));
			yield return cbCharaTamago[2].init (new TamaChara (18));

			if (muser1.chara2 != null) {
				yield return cbCharaTamago [3].init (muser1.chara2);
			}




			startEndFlag = true;
		}

		void Destroy(){
			Debug.Log ("MiniGame1 Destroy");
		}
		void OnDestroy(){
			Debug.Log ("MiniGame1 OnDestroy");
		}

		void Update(){
			switch (jobCount) {
			case statusJobCount.minigame1JobCount000:
				{
					if (startEndFlag) {
						EventTitle.SetActive (true);
						jobCount = statusJobCount.minigame1JobCount010;
						TamagoCharaPositionInit ();
						nowScore = 0;
						nowScore2 = 0;
						nowTime1 = 0.0f;
						nowTime2 = 50;												// 制限時間
					}
					break;
				}
			case statusJobCount.minigame1JobCount010:
				{
					break;															// タイトル画面
				}
			case statusJobCount.minigame1JobCount020:
				{
					EventTitle.SetActive (false);
					EventStart.SetActive (true);
					jobCount = statusJobCount.minigame1JobCount030;

					cbCharaTamago [0].gotoAndPlay (MotionLabel.IDLE);
					cbCharaTamago [1].gotoAndPlay (MotionLabel.IDLE);
					cbCharaTamago [2].gotoAndPlay (MotionLabel.SIT);
					if (muser1.chara2 != null) {
						cbCharaTamago [3].gotoAndPlay (MotionLabel.IDLE);
					}

					TamagoAnimeSprite (EventStart);									// たまごっちのアニメを反映する

					ManagerObject.instance.sound.playSe (20);
					break;
				}
			case statusJobCount.minigame1JobCount030:
				{
					if (EventStart.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.minigame1JobCount040;
						EventStart.SetActive (false);

						GameMainInit ();
						EventGame.SetActive (true);
						TamagoAnimeSprite (EventGame);								// たまごっちのアニメを反映する

//						ManagerObject.instance.sound.playBgm (21);
					}
					TamagoAnimeSprite (EventStart);									// たまごっちのアニメを反映する
					break;
				}
			case statusJobCount.minigame1JobCount040:
				{
					TamagoAnimeSprite (EventGame);									// たまごっちのアニメをImage,SpriteRendererに反映する
					if (GameMainLoop ()) {											// ゲーム処理
						jobCount = statusJobCount.minigame1JobCount050;
						waitCount = 45;

						waitResultFlag = false;
						GameCall call = new GameCall (CallLabel.GET_MINIGAME_RESULT,mData.mid,nowScore);
						call.AddListener (mGetMinigameResult);
						ManagerObject.instance.connect.send (call);
					}
					break;
				}
			case statusJobCount.minigame1JobCount050:
				{
					TamagoAnimeSprite (EventGame);									// たまごっちのアニメを反映する
					if (waitResultFlag) {
						waitCount--;
					}
					if (waitCount == 0) {											// 驚きを見せるためのウエィト
						jobCount = statusJobCount.minigame1JobCount060;

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
						ManagerObject.instance.sound.playSe (21);
					}
					break;
				}
			case statusJobCount.minigame1JobCount060:
				{
					if (EventEnd.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {	// お疲れ様のアニメが終了するまで待つ
						jobCount = statusJobCount.minigame1JobCount070;
						EventEnd.SetActive (false);
						EventResult.SetActive (true);

						resultLoopCount = statusResult.resultJobCount000;
						resultMainLoopFlag = false;
						ResultMainLoop ();											// 結果画面処理
						for (int i = 0; i < 3; i++) {
							cbCharaTamago [i].gotoAndPlay (MotionLabel.IDLE);
						}
						if (muser1.chara2 != null) {
							cbCharaTamago [3].gotoAndPlay (MotionLabel.IDLE);
						}

						TamagoAnimeSprite (EventResult);							// たまごっちのアニメを反映する
					}
					break;
				}
			case statusJobCount.minigame1JobCount070:
				{
					TamagoAnimeSprite (EventResult);								// たまごっちのアニメを反映する
							
					if (ResultMainLoop ()) {										// 結果画面処理
						jobCount = statusJobCount.minigame1JobCount000;
						EventResult.SetActive (false);
						EventTitle.SetActive (true);
					}

					break;
				}
			case statusJobCount.minigame1JobCount080:
				{
					break;
				}
			case statusJobCount.minigame1JobCount090:
				{
					break;
				}
			case statusJobCount.minigame1JobCount100:
				{
					break;
				}
			case statusJobCount.minigame1JobCount110:
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
			jobCount = statusJobCount.minigame1JobCount020;							// スタートボタンが押されたのでゲーム開始
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
//			ManagerObject.instance.view.add("webview","minigame1");
		}

		private void ButtonHelpModoruClick(){
			EventHelp.SetActive (false);
			ManagerObject.instance.sound.playSe (17);
		}

		private void ButtonYameruClick(){
			gameMainLoopFlag = true;												// ゲームメインを終了する
			ManagerObject.instance.sound.playSe (17);
		}

		private void ButtonTakuhaiClick(){
			Debug.Log ("プレゼントボックスへ・・・");
			ManagerObject.instance.sound.playSe (17);
			ManagerObject.instance.view.change(SceneLabel.ITEMBOX);
		}

		private void ButtonTojiruClick(){
			resultItemGetFlag = true;												// アイテム入手画面を閉じる
			ManagerObject.instance.sound.playSe (17);
		}

		private void ButtonModoruClick(){
			resultMainLoopFlag = true;												// タイトルにもどる
			ManagerObject.instance.sound.playSe (17);
		}

		// たまごっちのアニメをImage,SpriteRendererに反映する
		private void TamagoAnimeSprite(GameObject obj){
			TamagochiImageMove (obj, CharaTamago [0], "tamago/chara/");
			TamagochiImageMove (obj, CharaTamago [1], "tamago/chara2/");
			TamagochiImageMove (obj, CharaTamago [2], "tamago/chara3/");
			if (muser1.chara2 != null) {
				TamagochiImageMove (obj, CharaTamago [3], "tamago/charaF/");
			}
		}



		// ゲームメイン初期化
		private void GameMainInit(){
			charaAnimeFlag = 0;
			itemIdouFlag = 0;
			itemGetNumber = 0;
			charaJumpCheckFlag = 0;
			charaJumpCheckFlag2 = 0;
			scoreYIdouNumber = 0;
			gameMainLoopFlag = false;

			// アイテム下降メイン処理を登録する
			StartCoroutine (ItemDown ());

			EventGame.transform.Find ("points/Text").gameObject.GetComponent<Text> ().text = nowScore.ToString ();
			EventGame.transform.Find ("timer/Text").gameObject.GetComponent<Text> ().text = nowTime2.ToString ();

			// 初期は左向き
			EventGame.transform.Find ("tamago/chara").gameObject.transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
			if (muser1.chara2 != null) {
				EventGame.transform.Find ("tamago/charaF").gameObject.transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
			}

			scrnOffX = MinigameRoot.gameObject.GetComponent<RectTransform> ().sizeDelta.x / 2;
		}

		private float[] itemYJumpTable = new float[]{
			2.0f,1.8f,1.8f,1.5f,1.5f,1.1f,1.1f,1.1f,0.6f,0.6f,0.6f,0.6f,0.0f,0.0f,0.0f,
			0.0f,0.0f,0.0f,-0.6f,-0.6f,-0.6f,-0.6f,-1.1f,-1.1f,-1.1f,-1.5f,-1.5f,-1.8f,-1.8f,-2.0f,
			-2.2f,-2.4f,-2.6f,-2.9f,-3.2f,-3.5f,-3.5f,-3.5f,-3.5f,-3.5f,
		};
		private float[] tamagoYJumpTable = new float[] {
			-6.0f,-5.0f,-5.0f,-3.8f,-3.8f,-3.8f,-2.4f,-2.4f,-2.4f,-2.4f,-1.2f,-1.2f,-1.2f,-1.2f,-1.2f,0.0f,0.0f,0.0f,
			0.0f,0.0f,0.0f,1.2f,1.2f,1.2f,1.2f,1.2f,2.4f,2.4f,2.4f,2.4f,3.8f,3.8f,3.8f,5.0f,5.0f,6.0f,
		};
		private float[] tamagoYJumpIdouTable = new float[] {
			-320.0f,-314.0f,-309.0f,-304.0f,-300.2f,-296.4f,-292.6f,-290.2f,
			-287.8f,-285.4f,-283.0f,-281.8f,-280.6f,-279.4f,-278.2f,-277.0f,
			-277.0f,-277.0f,-277.0f,-277.0f,
			-277.0f,-278.2f,-279.4f,-280.6f,-281.8f,-283.0f,-285.4f,-287.8f,
			-290.2f,-292.6f,-296.4f,-300.2f,-304.0f,-309.0f,-314.0f,-320.0f,
		};
		private float[] scoreYIdouTable = new float[]{
			0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,
			0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,
			0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.5f,0.5f,0.5f,1.0f,1.0f,1.0f,1.8f,1.8f,1.8f,2.8f,2.8f,2.8f,3.0f,
		};

		private int scoreYIdouNumber = 0;
		private GameItem gameitem;													// spriteと得点の構造体
		private bool gameMainLoopFlag = false;
		private float scrnOffX;
		private int tapPosX;
		// ゲームメイン処理
		private bool GameMainLoop(){
			GameObject charaObj = EventGame.transform.Find ("tamago/chara").gameObject;	// 操作キャラのGameObjectを抽出
			GameObject charaObjF = EventGame.transform.Find ("tamago/charaF").gameObject;
			Vector3 pos = charaObj.transform.localPosition;							// 操作キャラの座標を抽出

			if (Input.GetMouseButtonDown (0)) {				
				Vector3 mousePosition = Input.mousePosition;
				float posScrn = useScreenX / 2;
				tapPosX = (int)(((mousePosition.x - posScrn) / posScrn) * scrnOffX);

				if (muser1.chara2 != null) {
					tapPosX = tapPosX + 75;
				}
				float _posx;
				if (muser1.chara2 == null) {
					_posx = pos.x;
				} else {
					_posx = pos.x - 75.0f;
				}
				if(tapPosX >= _posx){
					if (charaAnimeFlag != 1) {
						if (cbCharaTamago [0].nowlabel != MotionLabel.WALK) {
							cbCharaTamago [0].gotoAndPlay (MotionLabel.WALK);
						}
						charaObj.transform.localScale = new Vector3 (-2.0f, 2.0f, 1.0f);
						charaAnimeFlag = 1;
					}
				} else {
					if (charaAnimeFlag != 2) {
						if (cbCharaTamago [0].nowlabel != MotionLabel.WALK) {
							cbCharaTamago [0].gotoAndPlay (MotionLabel.WALK);
						}
						charaObj.transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
						charaAnimeFlag = 2;
					}
				}

				if (charaJumpCheckFlag2 == 0) {
					if (charaJumpCheckFlag == 0) {
						charaJumpCheckFlag = 15;
					} else {
						charaJumpCheckFlag2 = tamagoYJumpIdouTable.Length;			// ジャンプ開始
						ManagerObject.instance.sound.playSe (24);
					}
				}
			}	



			if (charaJumpCheckFlag != 0) {
				charaJumpCheckFlag--;
			}
			if (charaJumpCheckFlag2 != 0) {											// ジャンプ処理
				charaJumpCheckFlag2 -= (1.0f * (60 * Time.deltaTime));
				if (charaJumpCheckFlag2 <= 0) {
					charaJumpCheckFlag2 = 0.0f;
				}
				pos.y = tamagoYJumpIdouTable[(int)charaJumpCheckFlag2];
			}

			float _idouStopX = (float)tapPosX;
			switch (charaAnimeFlag) {
			case	1:																// 右移動
				{
					if (tapPosX > 600) {
						_idouStopX = 600.0f;
					}

					pos.x += (4.4f * (60 * Time.deltaTime));
					if (pos.x >= _idouStopX) {
						pos.x = _idouStopX;												// 画面端に来たら停止
						cbCharaTamago [0].gotoAndPlay (MotionLabel.IDLE);
						charaAnimeFlag = 0;
					}
					break;
				}
			case	2:																// 左移動
				{
					pos.x -= (4.4f * (60 * Time.deltaTime));
					if (muser1.chara2 == null) {
						if (tapPosX < -600) {
							_idouStopX = -600.0f;
						}
					} else {
						if (tapPosX < -450) {
							_idouStopX = -450.0f;
						}
					}
					if (pos.x <= _idouStopX) {
						pos.x = _idouStopX;											// 画面端に来たら停止
						cbCharaTamago [0].gotoAndPlay (MotionLabel.IDLE);
						charaAnimeFlag = 0;
					}

					break;
				}
			default:
				{
					break;
				}
			}

			if (muser1.chara2 == null) {
				charaObj.transform.localPosition = pos;									// 操作キャラの座標を設定
				charaObjF.transform.localPosition = new Vector3(5000.0f,5000.0f,0.0f);
			}
			else{
				Vector3 _pos = pos;
				charaObj.transform.localPosition = _pos;									// 操作キャラの座標を設定
				_pos.x -= 150.0f;
				charaObjF.transform.localPosition = _pos;
				charaObjF.transform.localScale = charaObj.transform.localScale;
				if (cbCharaTamago [0].nowlabel != cbCharaTamago [3].nowlabel) {
					cbCharaTamago [3].gotoAndPlay (cbCharaTamago [0].nowlabel);
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

			EventGame.transform.Find ("points/Text").gameObject.GetComponent<Text> ().text = nowScore.ToString ();
			EventResult.transform.Find ("points/Text").gameObject.GetComponent<Text> ().text = nowScore2.ToString ();
			EventGame.transform.Find ("timer/Text").gameObject.GetComponent<Text> ().text = nowTime2.ToString ();




			return gameMainLoopFlag;
		}

		// たまごっちとアイテムの当たり判定
		private bool HitCheck(Vector3 tamagoPos,Vector3 itemPos){
			float _posx, _offsetx;
			if (muser1.chara2 == null) {
				_posx = tamagoPos.x;
				_offsetx = 100.0f;
			} else {
				_posx = tamagoPos.x - 75.0f;
				_offsetx = 175.0f;
			}

			if (((_posx - _offsetx) < itemPos.x) && (itemPos.x < (_posx + _offsetx))) {
				if (((tamagoPos.y - 100.0f) < itemPos.y) && (itemPos.y < (tamagoPos.y + 100.0f))) {
					return true;
				}
			}

			return false;
		}



		private IEnumerator ItemDown(){
			int i = 0;

			yield return null;

			iMessageCounter = Random.Range (0, 3) + 2;

			while (true) {
				if (gameMainLoopFlag == true) {
					break;
				}

				// アイテム下降開始
				StartCoroutine (ItemDownSub (i));

				i++;
				if (i == 20) {
					i = 0;
				}

				// 下降するアイテムを出現させたのでウェイト
				yield return new WaitForSeconds (itemTable [itemGetNumber, 0]);
			}
		}


		private GameObject[] prefabObjItem = new GameObject[20];
		private GameObject[] prefabObjScore = new GameObject[20];
		private GameObject[] prefabObjMessage = new GameObject[20];
		private int iMessageCounter;

		private IEnumerator ItemDownSub(int _num){
			int _itemIdouFlag = 0;
			float _itemXbase = 0.0f;
			int _itemYNumber = 0;
			float _itemDownSpeed = 0.0f;
			GameItem _gameitem = pgGameCore.GameItemGet (0);

			// プレハブを登録
			prefabObjItem [_num] = (GameObject)Instantiate (PrefabItem);
			prefabObjItem [_num].transform.SetParent (EventGame.transform.Find ("Items").transform, false);
			prefabObjItem [_num].name = "Item" + _num.ToString ();

			prefabObjScore [_num] = (GameObject)Instantiate (PrefabScore);
			prefabObjScore [_num].transform.SetParent (EventGame.transform.Find ("Scores").transform, false);
			prefabObjScore [_num].name = "Score" + _num.ToString ();

			prefabObjMessage [_num] = (GameObject)Instantiate (PrefabMessage);
			prefabObjMessage [_num].transform.SetParent (EventGame.transform.Find ("Messages").transform, false);
			prefabObjMessage [_num].name = "Message" + _num.ToString ();


			while (true) {
				GameObject charaObj = EventGame.transform.Find ("tamago/chara").gameObject;	// 操作キャラのGameObjectを抽出
				Vector3 pos = charaObj.transform.localPosition;							// 操作キャラの座標を抽出

				GameObject itemObj = prefabObjItem[_num].gameObject;
				Vector3 posItem = itemObj.transform.localPosition;						// 落下アイテムの座標を抽出

				GameObject scoreObj = prefabObjScore [_num].gameObject;
				Vector3 posScore = scoreObj.transform.localPosition;					// スコア表示の座標を抽出

				GameObject messageObj = prefabObjMessage [_num].gameObject;
				Vector3 posMessage = messageObj.transform.localPosition;				// スコア評価オブジェの座標を抽出

				switch (_itemIdouFlag) {
				case	0:																// 落下アイテムの初期化
					{
						int itemRnd = Random.Range (0, 100);
						int number = 0;

						posItem.y = 600;
						posItem.x = (Random.Range (-59, 59) * 10);						// 落下位置を設定
						_itemIdouFlag = 1;
						_itemXbase = posItem.x;
						_itemYNumber = 0;

						_itemDownSpeed = (1000 / (itemTable [itemGetNumber, 1] * 60));	// 落下速度を設定

						float totalNum = 0;
						for (int i = 0; i < 6; i++) {
							totalNum += itemTable [itemGetNumber, 2 + i];
							if (itemRnd <= totalNum) {
								number = i;												// 落下アイテムを決定
								break;
							}
						}

						_gameitem = pgGameCore.GameItemGet (number);					// 落下アイテムの情報を設定

						Sprite _sprite;
						switch (number) {
						case	0:
							{
								_sprite = EventGame.transform.Find ("item_0").gameObject.GetComponent<Image> ().sprite;
								break;
							}
						case	1:
							{
								_sprite = EventGame.transform.Find ("item_1").gameObject.GetComponent<Image> ().sprite;
								break;
							}
						case	2:
							{
								_sprite = EventGame.transform.Find ("item_2").gameObject.GetComponent<Image> ().sprite;
								break;
							}
						case	3:
							{
								_sprite = EventGame.transform.Find ("item_3").gameObject.GetComponent<Image> ().sprite;
								break;
							}
						case	4:
							{
								_sprite = EventGame.transform.Find ("item_4").gameObject.GetComponent<Image> ().sprite;
								break;
							}
						default:
							{
								_sprite = EventGame.transform.Find ("item_5").gameObject.GetComponent<Image> ().sprite;
								break;
							}
						}
						itemObj.GetComponent<Image> ().sprite = _sprite;				// 落下アイテムのスプライトを設定

						break;
					}
				case	1:																// 落下アイテムの落下処理
					{
						posItem.y -= (_itemDownSpeed * (60 * Time.deltaTime));
						if (posItem.y <= -350.0f) {
							posItem.y = -350.0f;										// 地面に落ちたのでアイテムは消える
							_itemIdouFlag = 2;
						}

						if (HitCheck (pos, posItem)) {									// たまごっちとアイテムの当たり判定
							if (_gameitem.Score < 0) {
								ManagerObject.instance.sound.playSe (26);
								gameMainLoopFlag = true;								// お邪魔アイテムに触ったので終了
								cbCharaTamago [0].gotoAndPlay (MotionLabel.SHOCK);
								break;
							} else {
								ManagerObject.instance.sound.playSe (25);
							}

							if (muser1.chara2 == null) {
								posScore.x = pos.x;											// アイテムをゲットしたので得点を表示する
							} else {
								posScore.x = pos.x - 75.0f;
							}
							posScore.y = pos.y - 125.0f;

							scoreYIdouNumber = scoreYIdouTable.Length;
							scoreObj.GetComponent<Text> ().text = _gameitem.Score.ToString ();


							iMessageCounter--;
							if (iMessageCounter == 0) {
								if (muser1.chara2 == null) {
									posMessage.x = pos.x;									// スコア評価オブジェの表示位置を決定
								} else {
									posMessage.x = pos.x - 75.0f;
								}
								posMessage.y = pos.y + 20.0f;
								iMessageCounter = Random.Range (0, 3) + 2;

								switch (Random.Range (0, 3)) {							// スコア評価オブジェの種類を決定
								case	0:
									{
										messageObj.transform.Find ("type01").gameObject.SetActive (true);
										break;
									}
								case	1:
									{
										messageObj.transform.Find ("type02").gameObject.SetActive (true);
										break;
									}
								default:
									{
										messageObj.transform.Find ("type03").gameObject.SetActive (true);
										break;
									}
								}
							}

							nowScore += _gameitem.Score;								// 得点を加算する

							_itemIdouFlag = 3;
							Debug.Log ("アイテムゲット");
							itemGetNumber++;											// アイテムの番号を次にする
							if (itemGetNumber == 30) {
								itemGetNumber = 29;
							}
						}

						break;
					}
				case	2:																// 地面に落ちたアイテムは少し跳ねる
					{
						if (_itemXbase > 0) {
							posItem.x -= 1.2f;
						} else {
							posItem.x += 1.2f;
						}
						posItem.y += itemYJumpTable [_itemYNumber];
						_itemYNumber++;
						if (_itemYNumber == itemYJumpTable.Length) {
							_itemIdouFlag = 0;
						}
						break;
					}
				case	3:
					{
						if (scoreYIdouNumber == 0) {
							_itemIdouFlag = 0;
						}
						break;
					}
				}
				if (scoreYIdouNumber != 0) {
					scoreYIdouNumber--;													// スコア表示
					posScore.y += scoreYIdouTable [scoreYIdouNumber];
				} else {
					posScore.y = 1000.0f;
				}

				itemObj.transform.localPosition = posItem;								// 落下アイテムの座標を設定
				scoreObj.transform.localPosition = posScore;							// スコア表示の座標を設定
				messageObj.transform.localPosition = posMessage;						// スコア評価オブジェの座標を設定

				if (_itemIdouFlag == 0) {
					break;
				}
				if (gameMainLoopFlag == true) {
					break;
				}


				yield return null;
			}

			// プレハブの削除
			GameObject.Destroy (prefabObjItem [_num].gameObject);
			GameObject.Destroy (prefabObjScore [_num].gameObject);
			GameObject.Destroy (prefabObjMessage [_num].gameObject);
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


					if (mData.eventId != 0) {
						if ((nowScore == 0) || (!mResultData.rewardFlag)) {
							EventResult.transform.Find ("tamago/chara").gameObject.transform.localPosition = new Vector3 (250.0f, -320.0f, 0.0f);
							EventResult.transform.Find ("tamago/chara2").gameObject.transform.localPosition = new Vector3 (-400.0f, -320.0f, 0.0f);
							EventResult.transform.Find ("tamago/chara3").gameObject.transform.localPosition = new Vector3 (-250.0f, -320.0f, 0.0f);
						} else {
							EventResult.transform.Find ("tamago/chara").gameObject.transform.localPosition = new Vector3 (120.0f, -320.0f, 0.0f);
							EventResult.transform.Find ("tamago/chara2").gameObject.transform.localPosition = new Vector3 (-280.0f, -320.0f, 0.0f);
							EventResult.transform.Find ("tamago/chara3").gameObject.transform.localPosition = new Vector3 (-120.0f, -320.0f, 0.0f);
						}
					} else {
						if ((nowScore == 0) || (!mResultData.rewardFlag)) {
							if (muser1.chara2 == null) {
								EventResult.transform.Find ("tamago/chara").gameObject.transform.localPosition = new Vector3 (250.0f, -320.0f, 0.0f);
							} else {
								EventResult.transform.Find ("tamago/chara").gameObject.transform.localPosition = new Vector3 (375.0f, -320.0f, 0.0f);
								EventResult.transform.Find ("tamago/charaF").gameObject.transform.localPosition = new Vector3 (225.0f, -320.0f, 0.0f);
							}
						} else {
							if (muser1.chara2 == null) {
								EventResult.transform.Find ("tamago/chara").gameObject.transform.localPosition = new Vector3 (120.0f, -320.0f, 0.0f);
							} else {
								EventResult.transform.Find ("tamago/chara").gameObject.transform.localPosition = new Vector3 (245.0f, -320.0f, 0.0f);
								EventResult.transform.Find ("tamago/charaF").gameObject.transform.localPosition = new Vector3 (95.0f, -320.0f, 0.0f);
							}
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
					EventResult.SetActive (false);															// アイテム入手画面を開く
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
						resultLoopCount = statusResult.resultJobCount110;
						if ((nowScore == 0) || (!mResultData.rewardFlag)) {
						}
						else{
							for (int i = 0; i < 3; i++) {													// 褒賞品があるのでたまごっちを喜ばす
								TamagochiAnimeSet (i, MotionLabel.GLAD1);
							}
						}
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
				pos = EventResult.transform.Find ("tamago/chara").gameObject.transform.localPosition;
				pos.x += 4.0f;
				pos.y += tamagoYJumpTable [resultTamagoYJumpCount];
				EventResult.transform.Find ("tamago/chara").gameObject.transform.localPosition = pos;
				pos = EventResult.transform.Find ("tamago/chara2").gameObject.transform.localPosition;
				pos.x -= 4.0f;
				pos.y += tamagoYJumpTable [resultTamagoYJumpCount];
				EventResult.transform.Find ("tamago/chara2").gameObject.transform.localPosition = pos;
				pos = EventResult.transform.Find ("tamago/chara3").gameObject.transform.localPosition;
				pos.x -= 4.0f;
				pos.y += tamagoYJumpTable [resultTamagoYJumpCount];
				EventResult.transform.Find ("tamago/chara3").gameObject.transform.localPosition = pos;

				if (muser1.chara2 != null) {
					pos = EventResult.transform.Find ("tamago/charaF").gameObject.transform.localPosition;
					pos.x += 4.0f;
					pos.y += tamagoYJumpTable [resultTamagoYJumpCount];
					EventResult.transform.Find ("tamago/charaF").gameObject.transform.localPosition = pos;
				}
			}

			if (muser1.chara2 != null) {
				if (cbCharaTamago [3].nowlabel != cbCharaTamago [0].nowlabel) {
					cbCharaTamago [3].gotoAndPlay (cbCharaTamago [0].nowlabel);
				}
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



		private void TamagochiAnimeSet(int num,string status){
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

			if (cbCharaTamago [num].nowlabel != _status) {
				cbCharaTamago [num].gotoAndPlay (_status);
			}
		}



		private Vector2[] tamagoCharaPositionInitTable = new Vector2[] {
			new Vector2 (   0.0f, -320.0f),		// スタート画面のプレイヤーキャラの初期位置
			new Vector2 (-450.0f, -200.0f),		// スタート画面の応援キャラ１の初期位置
			new Vector2 ( 450.0f, -150.0f),		// スタート画面の応援キャラ２の初期位置
			new Vector2 (   0.0f, -320.0f),		// ゲーム画面のプレイヤーキャラの初期位置
			new Vector2 (-450.0f, -200.0f),		// ゲーム画面の応援キャラ１の初期位置
			new Vector2 ( 450.0f, -150.0f),		// ゲーム画面の応援キャラ２の初期位置
			new Vector2 (   0.0f,  700.0f),		// 結果画面の宝箱の初期位置
			new Vector2 (   0.0f,  700.0f),		// 結果画面のメッセージの初期位置
			new Vector2 (   0.0f,  999.0f),		// NPCキャラ画面外配置

			new Vector2 (  75.0f, -320.0f),		// スタート画面のプレイヤーキャラの初期位置
			new Vector2 ( -75.0f, -320.0f),		// スタート画面のプレイヤーFキャラの初期位置
			new Vector2 (  75.0f, -320.0f),		// ゲーム画面のプレイヤーキャラの初期位置
			new Vector2 ( -75.0f, -320.0f),		// ゲーム画面のプレイヤーFキャラの初期位置

		};
		private void TamagoCharaPositionInit(){
			TamagoCharaPositionInitSub (EventStart.transform.Find ("tamago/chara").gameObject, 0);
			TamagoCharaPositionInitSub (EventStart.transform.Find ("tamago/chara2").gameObject, 1);
			TamagoCharaPositionInitSub (EventStart.transform.Find ("tamago/chara3").gameObject, 2);
			TamagoCharaPositionInitSub (EventGame.transform.Find ("tamago/chara").gameObject, 3);
			TamagoCharaPositionInitSub (EventGame.transform.Find ("tamago/chara2").gameObject, 4);
			TamagoCharaPositionInitSub (EventGame.transform.Find ("tamago/chara3").gameObject, 5);
			TamagoCharaPositionInitSub (EventResult.transform.Find ("treasure").gameObject, 6);
			TamagoCharaPositionInitSub (EventResult.transform.Find ("result_text").gameObject, 7);

			if (mData.eventId == 0) {
				TamagoCharaPositionInitSub (EventStart.transform.Find ("tamago/chara2").gameObject, 8);
				TamagoCharaPositionInitSub (EventStart.transform.Find ("tamago/chara3").gameObject, 8);
				TamagoCharaPositionInitSub (EventGame.transform.Find ("tamago/chara2").gameObject, 8);
				TamagoCharaPositionInitSub (EventGame.transform.Find ("tamago/chara3").gameObject, 8);
				TamagoCharaPositionInitSub (EventResult.transform.Find ("tamago/chara2").gameObject, 8);
				TamagoCharaPositionInitSub (EventResult.transform.Find ("tamago/chara3").gameObject, 8);
			}

			if (muser1.chara2 != null) {
				TamagoCharaPositionInitSub (EventStart.transform.Find ("tamago/chara").gameObject, 9);
				TamagoCharaPositionInitSub (EventStart.transform.Find ("tamago/charaF").gameObject, 10);
				TamagoCharaPositionInitSub (EventGame.transform.Find ("tamago/chara").gameObject, 11);
				TamagoCharaPositionInitSub (EventGame.transform.Find ("tamago/charaF").gameObject, 12);
			}

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


		// 画像データの差し替え
		private void SeasonImageSet(){
			int	_seasonID = mData.seasonId - 1;


			// 背景
			MinigameRoot.transform.Find ("base/bg").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgBG;
			// 雲
			MinigameRoot.transform.Find ("base/bg/cloud1").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgKumo;
			MinigameRoot.transform.Find ("base/bg/cloud2").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgKumo;
			// タイトル
			MinigameRoot.transform.Find ("base/title/title").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgTitle;
			// 紅葉
			MinigameRoot.transform.Find ("base/title/momiji").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgMomiji;
			MinigameRoot.transform.Find ("base/start/momiji").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgMomiji;
			MinigameRoot.transform.Find ("base/game/momiji").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgMomiji;
			MinigameRoot.transform.Find ("base/result/momiji").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgMomiji;
			MinigameRoot.transform.Find ("base/end/momiji").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgMomiji;
			// １０点アイテム
			MinigameRoot.transform.Find ("base/start/item_0").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgItem [0];
			MinigameRoot.transform.Find ("base/game/item_0").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgItem [0];
			// ２０点アイテム
			MinigameRoot.transform.Find ("base/start/item_1").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgItem [1];
			MinigameRoot.transform.Find ("base/game/item_1").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgItem [1];
			// ３０点アイテム
			MinigameRoot.transform.Find ("base/start/item_2").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgItem [2];
			MinigameRoot.transform.Find ("base/game/item_2").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgItem [2];
			// ５０点アイテム
			MinigameRoot.transform.Find ("base/start/item_3").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgItem [3];
			MinigameRoot.transform.Find ("base/game/item_3").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgItem [3];
			// １００点アイテム
			MinigameRoot.transform.Find ("base/start/item_4").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgItem [4];
			MinigameRoot.transform.Find ("base/game/item_4").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgItem [4];
			// お邪魔アイテム
			MinigameRoot.transform.Find ("base/game/item_5").gameObject.GetComponent<Image> ().sprite = SeasonData [_seasonID].ImgItem [5];


		}



	}
}
