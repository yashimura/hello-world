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
		public ManagerObject manager;//ライブラリ
		[SerializeField] private GameCore	pgGameCore;
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

		[SerializeField] private GameObject baseSizePanel;




		private object[]		mparam;



		private CharaBehaviour[] cbCharaTamago = new CharaBehaviour[3];		// 
		private Vector3[] posTamago = new Vector3[3] {
			new Vector3 (0.0f, -10.0f, 0.0f),
			new Vector3 (0.0f, -10.0f, 0.0f),
			new Vector3 (0.0f, -10.0f, 0.0f),
		};
		private bool startEndFlag = false;
		private int waitCount = 0;
		private int charaAnimeFlag = 0;										// 0:idel,1:r-idou,2:l-idou
		private int itemIdouFlag = 0;										// 0;init,1:d-idou,2:
		private int itemGetNumber = 0;
		private int charaJumpCheckFlag = 0;									// jumpFlag
		private int charaJumpCheckFlag2 = 0;								// jumpFlag2

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
		}

		IEnumerator Start(){
			Debug.Log ("MiniGame1 Start");

			//単体動作テスト用
			//パラメタ詳細は設計書参照
			if (mparam==null) {
				mparam = new object[] {
					manager.player,
				};
			}
			muser1 = (User)mparam[0];		// たまごっち

			jobCount = statusJobCount.minigame1JobCount000;
			startEndFlag = false;

			ButtonStart.GetComponent<Button> ().onClick.AddListener (ButtonStartClick);
			ButtonClose.GetComponent<Button> ().onClick.AddListener (ButtonCloseClick);
			ButtonHelp.GetComponent<Button> ().onClick.AddListener (ButtonHelpClick);
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

			float num;
			if (use_screen_x >= use_screen_y) {
				num = use_screen_x / use_screen_y;
			} else {
				num = use_screen_y / use_screen_x;
			}

			if ((num > 1.33f) && (num < 1.34f)) {
				baseSizePanel.GetComponent<Transform> ().transform.localScale = new Vector3 (1.15f, 1.15f, 1.0f);	// 3:4の時のみ画面を拡大表示
			} else {
				baseSizePanel.GetComponent<Transform> ().transform.localScale = new Vector3 (1.15f, 1.15f, 1.0f);	// 3:4の時以外の画面を拡大表示
			}



			cbCharaTamago[0] = CharaTamago[0].GetComponent<CharaBehaviour> ();		// プレイヤー
			cbCharaTamago[1] = CharaTamago[1].GetComponent<CharaBehaviour> ();		// 応援キャラ１
			cbCharaTamago[2] = CharaTamago[2].GetComponent<CharaBehaviour> ();		// 応援キャラ２
			yield return cbCharaTamago[0].init (muser1.chara1);
			yield return cbCharaTamago[1].init (new TamaChara (17));
			yield return cbCharaTamago[2].init (new TamaChara (18));



			startEndFlag = true;
		}

		void Destroy(){
			Debug.Log ("MiniGame1 Destroy");
		}

		void Update(){
			switch (jobCount) {
			case statusJobCount.minigame1JobCount000:
				{
					if (startEndFlag) {
						EventTitle.SetActive (true);
						jobCount = statusJobCount.minigame1JobCount010;
						TamagoCharaPositionInit ();
					}
					break;
				}
			case statusJobCount.minigame1JobCount010:
				{
					break;
				}
			case statusJobCount.minigame1JobCount020:
				{
					float[,] pos = new float[3, 2] {
						{  0.0f, -12.8f },											// プレイヤーの初期位置（ダミーなので画面外に）
						{ -4.7f, -11.5f },											// 応援キャラ１の初期位置（ダミーなので画面外に）
						{  5.0f, -10.9f },											// 応援キャラ２の初期位置（ダミーなので画面外に）
					};
					EventTitle.SetActive (false);
					EventStart.SetActive (true);
					jobCount = statusJobCount.minigame1JobCount030;

					for (int i = 0; i < 3; i++) {
						posTamago [i].x = pos [i, 0];
						posTamago [i].y = pos [i, 1];
					}
					cbCharaTamago [0].gotoAndPlay ("idle");
					cbCharaTamago [1].gotoAndPlay ("idle");
					cbCharaTamago [2].gotoAndPlay ("sit");
					TamagoAnimeSprite (EventStart);									// たまごっちのアニメをImage,SpriteRendererに反映する
					break;
				}
			case statusJobCount.minigame1JobCount030:
				{
					if (EventStart.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.minigame1JobCount040;
						EventStart.SetActive (false);
						EventGame.SetActive (true);
						GameMainInit ();
						TamagoAnimeSprite (EventGame);								// たまごっちのアニメをImage,SpriteRendererに反映する
					}
					TamagoAnimeSprite (EventStart);									// たまごっちのアニメをImage,SpriteRendererに反映する
					break;
				}
			case statusJobCount.minigame1JobCount040:
				{
					TamagoAnimeSprite (EventGame);									// たまごっちのアニメをImage,SpriteRendererに反映する
					if (GameMainLoop ()) {											// ゲーム処理
						jobCount = statusJobCount.minigame1JobCount050;
						waitCount = 45;
					}
					break;
				}
			case statusJobCount.minigame1JobCount050:
				{
					TamagoAnimeSprite (EventGame);									// たまごっちのアニメをImage,SpriteRendererに反映する
					waitCount--;
					if (waitCount == 0) {											// 驚きを見せるためのウエィト
						jobCount = statusJobCount.minigame1JobCount060;
						EventGame.SetActive (false);
						EventEnd.SetActive (true);
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
							cbCharaTamago [i].gotoAndPlay ("idle");
						}
						TamagoAnimeSprite (EventResult);							// たまごっちのアニメをImage,SpriteRendererに反映する
					}
					break;
				}
			case statusJobCount.minigame1JobCount070:
				{
					TamagoAnimeSprite (EventResult);								// たまごっちのアニメをImage,SpriteRendererに反映する
							
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


		private void ButtonStartClick(){
			jobCount = statusJobCount.minigame1JobCount020;							// スタートボタンが押されたのでゲーム開始
		}

		private void ButtonCloseClick(){
			Debug.Log ("たまタウンへ・・・");
		}

		private void ButtonHelpClick(){
		}

		private void ButtonYameruClick(){
			gameMainLoopFlag = true;												// ゲームメインを終了する
		}

		private void ButtonTakuhaiClick(){
		}

		private void ButtonTojiruClick(){
			resultItemGetFlag = true;												// アイテム入手画面を閉じる
		}

		private void ButtonModoruClick(){
			resultMainLoopFlag = true;												// 
		}

		// たまごっちのアニメをImage,SpriteRendererに反映する
		private void TamagoAnimeSprite(GameObject obj){
			obj.transform.Find ("chara").gameObject.GetComponent<SpriteRenderer> ().sprite = CharaTamago [0].GetComponent<SpriteRenderer> ().sprite;
			obj.transform.Find ("chara2").gameObject.GetComponent<Image> ().sprite = CharaTamago [1].GetComponent<SpriteRenderer> ().sprite;
			obj.transform.Find ("chara3").gameObject.GetComponent<Image> ().sprite = CharaTamago [2].GetComponent<SpriteRenderer> ().sprite;
		}
			


		// ゲームメイン初期化
		private void GameMainInit(){
			charaAnimeFlag = 0;
			itemIdouFlag = 0;
			itemGetNumber = 0;
			charaJumpCheckFlag = 0;
			charaJumpCheckFlag2 = 0;
			itemDownSpeed = 0.0f;
			itemXbase = 0.0f;
			itemYNumber = 0;
			scoreYIdouNumber = 0;
			gameMainLoopFlag = false;
		}

		private float itemDownSpeed = 0.0f;
		private float itemXbase = 0.0f;
		private int itemYNumber = 0;
		private float[] itemYJumpTable = new float[]{
			2.0f,1.8f,1.8f,1.5f,1.5f,1.1f,1.1f,1.1f,0.6f,0.6f,0.6f,0.6f,0.0f,0.0f,0.0f,
			0.0f,0.0f,0.0f,-0.6f,-0.6f,-0.6f,-0.6f,-1.1f,-1.1f,-1.1f,-1.5f,-1.5f,-1.8f,-1.8f,-2.0f,
			-2.2f,-2.4f,-2.6f,-2.9f,-3.2f,-3.5f,-3.5f,-3.5f,-3.5f,-3.5f,
		};
		private float[] tamagoYJumpTable = new float[] {
			-6.0f,-5.0f,-5.0f,-3.8f,-3.8f,-3.8f,-2.4f,-2.4f,-2.4f,-2.4f,-1.2f,-1.2f,-1.2f,-1.2f,-1.2f,0.0f,0.0f,0.0f,
			0.0f,0.0f,0.0f,1.2f,1.2f,1.2f,1.2f,1.2f,2.4f,2.4f,2.4f,2.4f,3.8f,3.8f,3.8f,5.0f,5.0f,6.0f,
		};
		private float[] scoreYIdouTable = new float[]{
			0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.5f,0.5f,0.5f,1.0f,1.0f,1.0f,1.8f,1.8f,1.8f,2.8f,2.8f,2.8f,3.0f,
		};
		private int scoreYIdouNumber = 0;
		private GameItem gameitem;													// spriteと得点の構造体
		private bool gameMainLoopFlag = false;
		// ゲームメイン処理
		private bool GameMainLoop(){
			GameObject charaObj = EventGame.transform.Find ("chara").gameObject;	// 操作キャラのGameObjectを抽出
			Vector3 pos = charaObj.transform.localPosition;							// 操作キャラの座標を抽出

			if (Input.GetMouseButtonDown (0)) {				
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (ray.direction.x * 600 >= pos.x) {
					if (charaAnimeFlag != 1) {
						if (cbCharaTamago [0].nowlabel != "walk") {
							cbCharaTamago [0].gotoAndPlay ("walk");
						}
						charaObj.GetComponent<SpriteRenderer> ().flipX = true;
						charaAnimeFlag = 1;
					}
				} else {
					if (charaAnimeFlag != 2) {
						if (cbCharaTamago [0].nowlabel != "walk") {
							cbCharaTamago [0].gotoAndPlay ("walk");
						}
						charaObj.GetComponent<SpriteRenderer> ().flipX = false;
						charaAnimeFlag = 2;
					}
				}
				if (charaJumpCheckFlag2 == 0) {
					if (charaJumpCheckFlag == 0) {
						charaJumpCheckFlag = 30;
					} else {
						charaJumpCheckFlag2 = tamagoYJumpTable.Length;				// ジャンプ開始
					}
				}
			}	

			if (charaJumpCheckFlag != 0) {
				charaJumpCheckFlag--;
			}
			if (charaJumpCheckFlag2 != 0) {											// ジャンプ処理
				charaJumpCheckFlag2--;
				pos.y += tamagoYJumpTable [charaJumpCheckFlag2];
			}

			switch (charaAnimeFlag) {
			case	1:																// 右移動
				{
					pos.x += 2.5f;
					if (pos.x >= 330.0f) {
						pos.x = 330.0f;												// 画面端に来たら停止
						cbCharaTamago [0].gotoAndPlay ("idle");
						charaAnimeFlag = 0;
					}
					break;
				}
			case	2:																// 左移動
				{
					pos.x -= 2.5f;
					if (pos.x <= -330.0f) {
						pos.x = -330.0f;											// 画面端に来たら停止
						cbCharaTamago [0].gotoAndPlay ("idle");
						charaAnimeFlag = 0;
					}
					break;
				}
			default:
				{
					break;
				}
			}

			charaObj.transform.localPosition = pos;									// 操作キャラの座標を設定


			GameObject itemObj = EventGame.transform.Find ("item").gameObject;
			Vector3 posItem = itemObj.transform.localPosition;						// 落下アイテムの座標を抽出

			GameObject scoreObj = EventGame.transform.Find ("score").gameObject;
			Vector3 posScore = scoreObj.transform.localPosition;					// スコア表示の座標を抽出

			switch (itemIdouFlag) {
			case	0:																// 落下アイテムの初期化
				{
					int itemRnd = Random.Range (0, 100);
					int number = 0;

					posItem.y = 418;
					posItem.x = (Random.Range (-33, 33) * 10);
					itemIdouFlag = 1;
					itemXbase = posItem.x;
					itemYNumber = 0;

					itemDownSpeed = (600 / (itemTable [itemGetNumber, 1] * 60));

					float totalNum = 0;
					for (int i = 0; i < 6; i++) {
						totalNum += itemTable [itemGetNumber, 2 + i];
						if (itemRnd <= totalNum) {
							number = i;
							break;
						}
					}

					gameitem = pgGameCore.GameItemGet (number);
					itemObj.GetComponent<SpriteRenderer> ().sprite = gameitem.ItemImage;
					break;
				}
			case	1:																// 落下アイテムの落下処理
				{
					posItem.y -= itemDownSpeed;
					if (posItem.y <= -195.0f) {
						posItem.y = -195.0f;										// 地面に落ちたのでアイテムは消える
						itemIdouFlag = 2;
					}

					if (HitCheck (pos, posItem)) {									// たまごっちとアイテムの当たり判定
						if (gameitem.Score < 0) {
							gameMainLoopFlag = true;								// お邪魔アイテムに触ったので終了
							cbCharaTamago [0].gotoAndPlay ("shock");
							break;
						}

						posScore.x = pos.x;											// アイテムをゲットしたので得点を表示する
						posScore.y = pos.y - 26.0f;
						posScore.y = pos.y + 10.0f;
						scoreYIdouNumber = scoreYIdouTable.Length;
						scoreObj.GetComponent<Text> ().text = gameitem.Score.ToString ();
//						pgGameCore.GameScoreSet (gameitem.Score);

						itemIdouFlag = 0;
						Debug.Log ("アイテムゲット");
						itemGetNumber++;											// アイテムの番号を次にする
						if (itemGetNumber == 30) {
							itemGetNumber = 0;
						}
					}

					break;
				}
			case	2:																// 地面に落ちたアイテムは少し跳ねる
				{
					if (itemXbase > 0) {
						posItem.x -= 1.2f;
					} else {
						posItem.x += 1.2f;
					}
					posItem.y += itemYJumpTable [itemYNumber];
					itemYNumber++;
					if (itemYNumber == itemYJumpTable.Length) {
						itemIdouFlag = 0;
					}
					break;
				}
			case	3:
				{
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

			return gameMainLoopFlag;
		}

		// たまごっちとアイテムの当たり判定
		private bool HitCheck(Vector3 tamagoPos,Vector3 itemPos){
			if (((tamagoPos.x - 50.0f) < itemPos.x) && (itemPos.x < (tamagoPos.x + 50.0f))) {
				if (((tamagoPos.y - 100.0f) < itemPos.y) && (itemPos.y < (tamagoPos.y + 100.0f))) {
					return true;
				}
			}

			return false;
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


		private Vector2[] tamagoCharaPositionInitTable = new Vector2[] {
			new Vector2 (   0.0f, -227.0f),		// スタート画面のプレイヤーキャラの初期位置
			new Vector2 (-235.0f, -124.0f),		// スタート画面の応援キャラ１の初期位置
			new Vector2 ( 236.0f,  -97.0f),		// スタート画面の応援キャラ２の初期位置
			new Vector2 (   0.0f, -227.0f),		// ゲーム画面のプレイヤーキャラの初期位置
			new Vector2 (-235.0f, -124.0f),		// ゲーム画面の応援キャラ１の初期位置
			new Vector2 ( 236.0f,  -97.0f),		// ゲーム画面の応援キャラ２の初期位置
			new Vector2 (  60.0f, -230.0f),		// 結果画面のプレイヤーキャラの初期位置
			new Vector2 (-202.0f, -175.0f),		// 結果画面の応援キャラ１の初期位置
			new Vector2 ( -88.0f, -175.0f),		// 結果画面の応援キャラ２の初期位置
			new Vector2 (   0.0f,  420.0f),		// 結果画面の宝箱の初期位置
			new Vector2 (   0.0f,  420.0f),		// 結果画面のメッセージの初期位置
			new Vector2 (   0.0f,  418.0f),		// ゲーム画面の落下アイテムの初期位置
			new Vector2 (   0.0f,  999.0f),		// ゲーム画面のスコアの初期位置
		};
		private void TamagoCharaPositionInit(){
			TamagoCharaPositionInitSub (EventStart.transform.Find ("chara").gameObject, 0);
			TamagoCharaPositionInitSub (EventStart.transform.Find ("chara2").gameObject, 1);
			TamagoCharaPositionInitSub (EventStart.transform.Find ("chara3").gameObject, 2);
			TamagoCharaPositionInitSub (EventGame.transform.Find ("chara").gameObject, 3);
			TamagoCharaPositionInitSub (EventGame.transform.Find ("chara2").gameObject, 4);
			TamagoCharaPositionInitSub (EventGame.transform.Find ("chara3").gameObject, 5);
			TamagoCharaPositionInitSub (EventResult.transform.Find ("chara").gameObject, 6);
			TamagoCharaPositionInitSub (EventResult.transform.Find ("chara2").gameObject, 7);
			TamagoCharaPositionInitSub (EventResult.transform.Find ("chara3").gameObject, 8);
			TamagoCharaPositionInitSub (EventResult.transform.Find ("treasure").gameObject, 9);
			TamagoCharaPositionInitSub (EventResult.transform.Find ("result_text").gameObject, 10);
			TamagoCharaPositionInitSub (EventGame.transform.Find ("item").gameObject, 11);
			TamagoCharaPositionInitSub (EventGame.transform.Find ("score").gameObject, 12);
		}
		private void TamagoCharaPositionInitSub (GameObject obj, int num){
			Vector3 pos = new Vector3 (0.0f, 0.0f, 0.0f);

			pos = obj.transform.localPosition;
			pos.x = tamagoCharaPositionInitTable [num].x;
			pos.y = tamagoCharaPositionInitTable [num].y;
			obj.transform.localPosition = pos;
		}



	}
}
