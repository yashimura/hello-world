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

namespace Mix2App.MiniGame1{
	public class MiniGame1 : MonoBehaviour,IReceiver,IReadyable {
		[SerializeField] private GameCore pgGameCore = null;
		[SerializeField] private GameObject MinigameRoot = null;
		[SerializeField] private GameObject[] CharaTamago = null;				// たまごっち
		[SerializeField] private GameObject[] CharaTamagoNPC = null;			// たまごっち
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

		[SerializeField] private GameObject baseSizePanel = null;

//		[Tooltip("四季の画像データ（春、夏、秋、冬）")]
//		[SerializeField] private SeasonImg[] SeasonData;

		[SerializeField] private GameObject PrefabItem = null;
		[SerializeField] private GameObject PrefabScore = null;
		[SerializeField] private GameObject PrefabMessage = null;

		[SerializeField] private Sprite[] EventEndSprite = null;				// 終了時の演出スプライト

        [SerializeField] private GameObject CameraObj = null;



        private object[]		mparam;



		private CharaBehaviour[] cbCharaTamago = new CharaBehaviour[2];		// 
		private CharaBehaviour[] cbCharaTamagoNPC = new CharaBehaviour[4];	// 
		private bool startEndFlag = false;
		private int waitCount = 0;
		private int nowScore;													// 得点
		private int nowScore2;
		private float nowTime1;													// 残り時間（カウントダウン用）
		private int	nowTime2;													// 残り時間（制限時間）

		private int charaAnimeFlag = 0;										// 0:idel,1:r-idou,2:l-idou
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
			minigame1JobCount120,
			minigame1JobCount130,
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
			mready = false;

            GameEventHandler.OnRemoveSceneEvent += AchieveClearDelete;
        }

        public void receive(params object[] parameter){
			Debug.Log ("MiniGame1 receive");
			mparam = parameter;

			//単体動作テスト用
			//パラメタ詳細は設計書参照
			if (mparam==null) {
			}

			GameCall call = new GameCall (CallLabel.GET_MINIGAME_INFO,1);
			call.AddListener (mGetMinigameInfo);
			ManagerObject.instance.connect.send (call);
		}

		private bool mready = false;
		public bool ready(){
			return mready;
		}

		void Start(){
			Debug.Log ("MiniGame1 Start");
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
                    MinigameRoot.transform.Find("base/bg").gameObject.SetActive(true);
                    mready = true;
					ManagerObject.instance.view.dialog ("alert", new object[]{ "minigame1",(int)data}, mGetMinigameInfoCallBack);
				}
			}
		}
		private void mGetMinigameInfoCallBack(int num){
			ManagerObject.instance.view.change(SceneLabel.TOWN);
		}

		private float useScreenX;
		private float useScreenY;
		private bool futagoFlag;
		private bool[] NpcDispFlag = new bool[4] {false,false,false,false};
		private TamaChara[] NpcBaseTamaChara = new TamaChara[4];
		IEnumerator mStart(){
			muser1 = ManagerObject.instance.player;		// たまごっち

//			if (Random.Range (0, 2) == 0) {
//				muser1.chara2 = null;
//			}



			SeasonImageSet();



			NpcDispFlag[0] = false;
			NpcDispFlag[1] = false;
			NpcDispFlag[2] = false;
			NpcDispFlag[3] = false;

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

			if (muser1.chara2 == null) {
				futagoFlag = false;
			} else {
				futagoFlag = true;
			}

			cbCharaTamago[0] = CharaTamago[0].GetComponent<CharaBehaviour> ();		// プレイヤー
			cbCharaTamago[1] = CharaTamago[1].GetComponent<CharaBehaviour> ();		// 双子プレイヤー
			yield return cbCharaTamago[0].init (muser1.chara1);

			if (futagoFlag) {
				yield return cbCharaTamago [1].init (muser1.chara2);
			}

			for (int i = 0; i < 4; i++) {
				cbCharaTamagoNPC[i] = CharaTamagoNPC[i].GetComponent<CharaBehaviour> ();
			}
			mready = true;
			startEndFlag = true;
		}

		void Destroy(){
			Debug.Log ("MiniGame1 Destroy");
		}
		void OnDestroy(){
			Debug.Log ("MiniGame1 OnDestroy");
            GameEventHandler.OnRemoveSceneEvent -= AchieveClearDelete;
        }

        private bool achieveDeleteFlag;
        void AchieveClearDelete(string label)
        {
            if (label == SceneLabel.ACHIEVE_CLEAR)
            {
                achieveDeleteFlag = true;
                ManagerObject.instance.view.delete(SceneLabel.ACHIEVE_CLEAR);
            }
        }

        void Update(){
			switch (jobCount) {
			case statusJobCount.minigame1JobCount000:
				{
					if (startEndFlag) {
						EventTitle.SetActive (true);
						jobCount = statusJobCount.minigame1JobCount010;
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
					EventTitleButtonActive (false);
					jobCount = statusJobCount.minigame1JobCount030;
					StartCoroutine (tamagochiCharaInit (statusJobCount.minigame1JobCount040));
					break;
				}
			case statusJobCount.minigame1JobCount030:
				{
					break;															// NPCキャラセットが完了したら次へ
				}
			case statusJobCount.minigame1JobCount040:
				{
					TamagoCharaPositionInit ();
					EventTitleButtonActive (true);
					EventTitle.SetActive (false);
					EventStart.SetActive (true);
					jobCount = statusJobCount.minigame1JobCount050;

					cbCharaTamago [0].gotoAndPlay (MotionLabel.IDLE);
					if (futagoFlag) {
						cbCharaTamago [1].gotoAndPlay (MotionLabel.IDLE);
					}

					for (int i = 0; i < 4; i++) {
						if (NpcDispFlag [i]) {
							cbCharaTamagoNPC [i].gotoAndPlay (MotionLabel.IDLE);	// 応援キャラのアニメを登録
						}
					}

					TamagoAnimeSprite (EventStart);									// たまごっちのアニメを反映する

					ManagerObject.instance.sound.playSe (20);
					break;
				}
			case statusJobCount.minigame1JobCount050:
				{
					if (EventStart.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.minigame1JobCount060;
						EventStart.SetActive (false);

						GameMainInit ();
						EventGame.SetActive (true);
						TamagoAnimeSprite (EventGame);								// たまごっちのアニメを反映する

//						ManagerObject.instance.sound.playBgm (21);
					}
					TamagoAnimeSprite (EventStart);									// たまごっちのアニメを反映する
					break;
				}
			case statusJobCount.minigame1JobCount060:
				{
					TamagoAnimeSprite (EventGame);									// たまごっちのアニメを反映する
					if (GameMainLoop ()) {											// ゲーム処理
						jobCount = statusJobCount.minigame1JobCount070;
						waitCount = 45;

						waitResultFlag = false;
						GameCall call = new GameCall (CallLabel.GET_MINIGAME_RESULT,mData,nowScore);
						call.AddListener (mGetMinigameResult);
						ManagerObject.instance.connect.send (call);
					}
					break;
				}
			case statusJobCount.minigame1JobCount070:
				{
					TamagoAnimeSprite (EventGame);									// たまごっちのアニメを反映する
					if (waitResultFlag) {
						waitCount--;
					}
					if (waitCount == 0) {											// 驚きを見せるためのウエィト
						jobCount = statusJobCount.minigame1JobCount080;

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
			case statusJobCount.minigame1JobCount080:
				{
					if (EventEnd.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {	// お疲れ様のアニメが終了するまで待つ
						jobCount = statusJobCount.minigame1JobCount090;
						EventEnd.SetActive (false);
						EventResult.SetActive (true);

						resultLoopCount = statusResult.resultJobCount000;
						resultMainLoopFlag = false;
						ResultMainLoop ();											// 結果画面処理
						cbCharaTamago [0].gotoAndPlay (MotionLabel.IDLE);
						if (futagoFlag) {
							cbCharaTamago [1].gotoAndPlay (MotionLabel.IDLE);
						}

						TamagoAnimeSprite (EventResult);							// たまごっちのアニメを反映する
					}
					break;
				}
			case statusJobCount.minigame1JobCount090:
				{
					TamagoAnimeSprite (EventResult);								// たまごっちのアニメを反映する
							
					if (ResultMainLoop ()) {										// 結果画面処理
						jobCount = statusJobCount.minigame1JobCount000;
						EventResult.SetActive (false);
						EventTitle.SetActive (true);
					}

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
			case statusJobCount.minigame1JobCount120:
				{
					break;
				}
			case statusJobCount.minigame1JobCount130:
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
//			EventHelp.SetActive (true);
			ManagerObject.instance.sound.playSe (11);
			ManagerObject.instance.view.dialog("webview",new object[]{"minigame1"},null);
		}
		private void ButtonHelpClickCallBack(int num){
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

		// たまごっちのアニメを反映する
		private void TamagoAnimeSprite(GameObject obj){
			UIFunction.TamagochiImageMove (obj, CharaTamago [0], "tamago/chara/");
			if (futagoFlag) {
				UIFunction.TamagochiImageMove (obj, CharaTamago [1], "tamago/charaF/");
			}

			{
				string[] _name = new string[]{ "tamago/chara0/", "tamago/chara1/", "tamago/chara2/", "tamago/chara3/" };
				for (int i = 0; i < 4; i++) {
					UIFunction.TamagochiImageMove (obj, CharaTamagoNPC [i], _name [i]);
				}
			}
		}

		private void EventTitleButtonActive(bool flag){
			EventTitle.transform.Find ("Button_red_start").gameObject.SetActive (flag);
			EventTitle.transform.Find ("Button_red_help").gameObject.SetActive (flag);
			EventTitle.transform.Find ("Button_red_close").gameObject.SetActive (flag);
			EventTitle.transform.Find ("title").gameObject.SetActive (flag);
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
					yield return cbCharaTamagoNPC [i].init (NpcBaseTamaChara [i]);	// 応援キャラを登録する
				}
			}



			jobCount = flag;
		}


		// ゲームメイン初期化
		private void GameMainInit(){
			charaAnimeFlag = 0;
			itemGetNumber = 0;
			charaJumpCheckFlag = 0;
			charaJumpCheckFlag2 = 0;
			scoreYIdouNumber = 0;
			gameMainLoopFlag = false;

			missCount = 0;

			// アイテム下降メイン処理を登録する
			StartCoroutine (ItemDown ());

			EventGame.transform.Find ("points/Text").gameObject.GetComponent<Text> ().text = nowScore.ToString ();
			EventGame.transform.Find ("timer/Text").gameObject.GetComponent<Text> ().text = nowTime2.ToString ();

			// 初期は左向き
			EventGame.transform.Find ("tamago/chara").gameObject.transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
			if (futagoFlag) {
				EventGame.transform.Find ("tamago/charaF").gameObject.transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
			}

			// 描画エリアの横幅サイズの取得
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


			if (cbCharaTamago [0].nowlabel == MotionLabel.SHOCK) {
				return gameMainLoopFlag;											// お邪魔アイテムにあったたので進行停止
			}


			if (Input.GetMouseButtonDown (0)) {				
				Vector3 mousePosition = Input.mousePosition;
				float posScrn = useScreenX / 2;
				tapPosX = (int)(((mousePosition.x - posScrn) / posScrn) * scrnOffX);

				if (futagoFlag) {
					tapPosX = tapPosX + 75;
				}
				float _posx;
				if (!futagoFlag) {
					_posx = pos.x;
				} else {
					_posx = pos.x - 75.0f;
				}
				if(tapPosX >= _posx){
					if (charaAnimeFlag != 1) {
						if (cbCharaTamago [0].nowlabel != MotionLabel.WALK) {
							cbCharaTamago [0].gotoAndPlay (MotionLabel.WALK);
						}
						charaObj.transform.localScale = new Vector3 (-2.0f, 2.0f, 1.0f);	// 右向き
						charaAnimeFlag = 1;
					}
				} else {
					if (charaAnimeFlag != 2) {
						if (cbCharaTamago [0].nowlabel != MotionLabel.WALK) {
							cbCharaTamago [0].gotoAndPlay (MotionLabel.WALK);
						}
						charaObj.transform.localScale = new Vector3 (2.0f, 2.0f, 1.0f);		// 左向き
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
					if (!futagoFlag) {
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

			if (!futagoFlag) {
				charaObj.transform.localPosition = pos;									// 操作キャラの座標を設定
				charaObjF.transform.localPosition = new Vector3(5000.0f,5000.0f,0.0f);
			}
			else{
				Vector3 _pos = pos;
				charaObj.transform.localPosition = _pos;								// 操作キャラの座標を設定
				_pos.x -= 150.0f;
				charaObjF.transform.localPosition = _pos;
				charaObjF.transform.localScale = charaObj.transform.localScale;
				if (cbCharaTamago [0].nowlabel != cbCharaTamago [1].nowlabel) {
					cbCharaTamago [1].gotoAndPlay (cbCharaTamago [0].nowlabel);
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
			if (!futagoFlag) {
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

		// アイテム下降メイン処理
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
		private int missCount = 0;
		// アイテム下降処理
		private IEnumerator ItemDownSub(int _num){
			int _itemIdouFlag = 0;
			float _itemXbase = 0.0f;
			int _itemYNumber = 0;
			float _itemDownSpeed = 0.0f;
//			GameItem _gameitem = pgGameCore.GameItemGet (0);
			GameItem _gameitem = null;

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

				if ((cbCharaTamago [0].nowlabel == MotionLabel.SHOCK) && (_itemIdouFlag != 5)) {
					_itemIdouFlag = 6;
				}

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
								cbCharaTamago [0].gotoAndPlay (MotionLabel.SHOCK);		// お邪魔アイテムにあったたのでびっくり
								if (futagoFlag) {
									cbCharaTamago [1].gotoAndPlay (MotionLabel.SHOCK);	// 双子がいる場合は、一緒にびっくり
								}

								missCount = 30;											// お邪魔アイテムを一定時間表示する
								_itemIdouFlag = 5;

								break;
							} else {
								ManagerObject.instance.sound.playSe (25);
							}

							// アイテムをゲットしたので得点を表示する
							if (!futagoFlag) {
								posScore.x = pos.x;										// スコアの表示位置を決定
							} else {
								posScore.x = pos.x - 75.0f;
							}
							posScore.y = pos.y - 125.0f;

							scoreYIdouNumber = scoreYIdouTable.Length;
							scoreObj.GetComponent<Text> ().text = _gameitem.Score.ToString ();


							iMessageCounter--;
							if (iMessageCounter == 0) {
								if (!futagoFlag) {
									posMessage.x = pos.x;								// スコア評価オブジェの表示位置を決定
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
				case	5:																// お邪魔アイテムに触ったのでしばらく表示して終了
					{
						missCount--;
						if (missCount == 0) {
							gameMainLoopFlag = true;
						}
						break;
					}
				case	6:
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
            resultJobCount120,
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

					if ((nowScore == 0) || (!mResultData.rewardFlag)) {
						if (!futagoFlag) {
							EventResult.transform.Find ("tamago/chara").gameObject.transform.localPosition = new Vector3 (250.0f, -320.0f, 0.0f);
						} else {
							EventResult.transform.Find ("tamago/chara").gameObject.transform.localPosition = new Vector3 (375.0f, -320.0f, 0.0f);
							EventResult.transform.Find ("tamago/charaF").gameObject.transform.localPosition = new Vector3 (225.0f, -320.0f, 0.0f);
						}
					} else {
						if (!futagoFlag) {
							EventResult.transform.Find ("tamago/chara").gameObject.transform.localPosition = new Vector3 (120.0f, -320.0f, 0.0f);
						} else {
							EventResult.transform.Find ("tamago/chara").gameObject.transform.localPosition = new Vector3 (245.0f, -320.0f, 0.0f);
							EventResult.transform.Find ("tamago/charaF").gameObject.transform.localPosition = new Vector3 (95.0f, -320.0f, 0.0f);
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

                            mResultData.achieves = new List<AchieveData>();
                            AchieveData ad = new AchieveData();
                            ad.aid = 1;
                            ad.atitle = "てすとあちーぶ";
                            ad.reward = new RewardData();
                            ad.reward.kind = RewardKind.GOTCHI_PT;
                            ad.reward.point = 100;
                            ad.akind = 2;//1:デイリー２：累計
                            ad.count = 3;//残り回数※達成しきい値や現在カウントではない
                            mResultData.achieves.Add(ad);

                            //達成アチーブがある場合は、アチーブ成功画面を呼び出す
                            if (mResultData.achieves != null && mResultData.achieves.Count != 0)
                            {
                                achieveDeleteFlag = false;
                                int CameraDepth = (int)(CameraObj.transform.GetComponent<Camera>().depth + 1);
                                ManagerObject.instance.view.add(SceneLabel.ACHIEVE_CLEAR,
                                        mResultData.achieves,
                                        CameraDepth);
                            }
                    }
                    break;
				}
			case	statusResult.resultJobCount090:
				{
                        if (achieveDeleteFlag)
                        {
                            EventResult.SetActive(false);                                                           // アイテム入手画面を開く
                            EventItemget.SetActive(true);
                            ManagerObject.instance.sound.playSe(23);

                            EventItemget.transform.Find("getpoints_text").gameObject.SetActive(false);
                            EventItemget.transform.Find("getitem_text").gameObject.SetActive(false);

                            if (mResultData.reward.kind == RewardKind.ITEM)
                            {
                                // アイテムが褒賞品
                                EventItemget.transform.Find("getitem_text").gameObject.SetActive(true);
                            }
                            else
                            {
                                // ごっちポイントが褒賞品
                                EventItemget.transform.Find("getpoints_text").gameObject.SetActive(true);
                            }

                            RewardBehaviour rbItem = EventItemget.transform.Find("RewardView").gameObject.GetComponent<RewardBehaviour>();
                            rbItem.init(mResultData.reward);

                            resultItemGetFlag = false;
                            resultLoopCount = statusResult.resultJobCount110;


                        }
                        break;
				}
            case    statusResult.resultJobCount100:
                {
                        resultLoopCount = statusResult.resultJobCount110;

                        mResultData.achieves = new List<AchieveData>();
                        AchieveData ad = new AchieveData();
                        ad.aid = 1;
                        ad.atitle = "てすとあちーぶ";
                        ad.reward = new RewardData();
                        ad.reward.kind = RewardKind.GOTCHI_PT;
                        ad.reward.point = 100;
                        ad.akind = 2;//1:デイリー２：累計
                        ad.count = 3;//残り回数※達成しきい値や現在カウントではない
                        mResultData.achieves.Add(ad);

                        //達成アチーブがある場合は、アチーブ成功画面を呼び出す
                        if (mResultData.achieves != null && mResultData.achieves.Count != 0)
                        {
                            int CameraDepth = (int)(CameraObj.transform.GetComponent<Camera>().depth + 1);
                            ManagerObject.instance.view.add(SceneLabel.ACHIEVE_CLEAR,
                                    mResultData.achieves,
                                    CameraDepth);
                        }

                    break;
                }
			case	statusResult.resultJobCount110:
				{
					if (resultItemGetFlag) {
						EventItemget.SetActive (false);														// アイテム入手画面を消す
						EventResult.SetActive (true);														// 結果画面を表示する
						EventResult.transform.Find ("Button_blue_modoru").gameObject.SetActive (true);		// 戻るボタンを表示する
						resultLoopCount = statusResult.resultJobCount120;
						if ((nowScore == 0) || (!mResultData.rewardFlag)) {
						}
						else{
							// 褒賞品があるのでたまごっちを喜ばす
							TamagochiRandomGlad (cbCharaTamago [0]);
							TamagochiRandomGlad (cbCharaTamago [1]);

							for (int i = 0; i < 4; i++) {
								if (NpcDispFlag [i]) {
									TamagochiRandomGlad (cbCharaTamagoNPC [i]);								// 応援キャラを喜ばす
								}
							}
						}
					}
					break;
				}
			case	statusResult.resultJobCount120:
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
				if (futagoFlag) {
					pos = EventResult.transform.Find ("tamago/charaF").gameObject.transform.localPosition;
					pos.x += 4.0f;
					pos.y += tamagoYJumpTable [resultTamagoYJumpCount];
					EventResult.transform.Find ("tamago/charaF").gameObject.transform.localPosition = pos;
				}
			}

			if (futagoFlag) {
				if (cbCharaTamago [1].nowlabel != cbCharaTamago [0].nowlabel) {
					cbCharaTamago [1].gotoAndPlay (cbCharaTamago [0].nowlabel);
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
		// 結果発表画面の得点演出処理
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



		private void TamagochiRandomGlad(CharaBehaviour _cb){
			string _status;
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

			if (_cb.nowlabel != _status) {
				_cb.gotoAndPlay (_status);
			}
		}



		private Vector2[] tamagoCharaPositionInitTable = new Vector2[] {
			new Vector2 (   0.0f, -320.0f),		// スタート画面のプレイヤーキャラの初期位置（シングルモード）
			new Vector2 (   0.0f, -320.0f),		// ゲーム画面のプレイヤーキャラの初期位置（シングルモード）

			new Vector2 (  75.0f, -320.0f),		// スタート画面のプレイヤーキャラの初期位置（双子モード）
			new Vector2 ( -75.0f, -320.0f),		// スタート画面のプレイヤーFキャラの初期位置（双子モード）
			new Vector2 (  75.0f, -320.0f),		// ゲーム画面のプレイヤーキャラの初期位置（双子モード）
			new Vector2 ( -75.0f, -320.0f),		// ゲーム画面のプレイヤーFキャラの初期位置（双子モード）

			new Vector2 (   0.0f,  700.0f),		// 結果画面の宝箱の初期位置
			new Vector2 (   0.0f,  700.0f),		// 結果画面のメッセージの初期位置
			new Vector2 (   0.0f, 5000.0f),		// NPCキャラ画面外配置

			new Vector2 (-470.0f, -200.0f),		// スタート画面の応援キャラ０の初期位置
			new Vector2 ( 470.0f, -200.0f),		// スタート画面の応援キャラ１の初期位置
			new Vector2 (-300.0f, -200.0f),		// スタート画面の応援キャラ２の初期位置
			new Vector2 ( 300.0f, -200.0f),		// スタート画面の応援キャラ３の初期位置
			new Vector2 (-470.0f, -200.0f),		// ゲーム画面の応援キャラ０の初期位置
			new Vector2 ( 470.0f, -200.0f),		// ゲーム画面の応援キャラ１の初期位置
			new Vector2 (-300.0f, -200.0f),		// ゲーム画面の応援キャラ２の初期位置
			new Vector2 ( 300.0f, -200.0f),		// ゲーム画面の応援キャラ３の初期位置
			new Vector2 (-310.0f, -200.0f),		// 結果画面の応援キャラ０の初期位置
			new Vector2 ( 310.0f, -200.0f),		// 結果画面の応援キャラ１の初期位置
			new Vector2 (-480.0f, -200.0f),		// 結果画面の応援キャラ２の初期位置
			new Vector2 ( 480.0f, -200.0f),		// 結果画面の応援キャラ３の初期位置
		};
		private void TamagoCharaPositionInit(){
			if (!futagoFlag) {
				TamagoCharaPositionInitSub (EventStart.transform.Find ("tamago/chara").gameObject, 0);
				TamagoCharaPositionInitSub (EventGame.transform.Find ("tamago/chara").gameObject, 1);
			}
			else{
				TamagoCharaPositionInitSub (EventStart.transform.Find ("tamago/chara").gameObject, 2);
				TamagoCharaPositionInitSub (EventStart.transform.Find ("tamago/charaF").gameObject, 3);
				TamagoCharaPositionInitSub (EventGame.transform.Find ("tamago/chara").gameObject, 4);
				TamagoCharaPositionInitSub (EventGame.transform.Find ("tamago/charaF").gameObject, 5);
			}

			TamagoCharaPositionInitSub (EventResult.transform.Find ("treasure").gameObject, 6);
			TamagoCharaPositionInitSub (EventResult.transform.Find ("result_text").gameObject, 7);

			{
				string[] _name = new string[4]{"tamago/chara0","tamago/chara1","tamago/chara2","tamago/chara3"};
				int[] _start = new int[4]{ 9, 10, 11, 12 };
				int[] _game = new int[4]{ 13, 14, 15, 16 };
				int[] _result = new int[4]{ 17, 18, 19, 20 };

				for (int i = 0; i < 4; i++) {													// 応援キャラの表示位置登録
					if (NpcDispFlag [i]) {
						TamagoCharaPositionInitSub (EventStart.transform.Find (_name[i]).gameObject, _start[i]);
						TamagoCharaPositionInitSub (EventGame.transform.Find (_name[i]).gameObject, _game[i]);
						TamagoCharaPositionInitSub (EventResult.transform.Find (_name[i]).gameObject, _result[i]);
					} else {
						TamagoCharaPositionInitSub (EventStart.transform.Find (_name[i]).gameObject, 8);
						TamagoCharaPositionInitSub (EventGame.transform.Find (_name[i]).gameObject, 8);
						TamagoCharaPositionInitSub (EventResult.transform.Find (_name[i]).gameObject, 8);
					}
				}
			}
		}
		private void TamagoCharaPositionInitSub (GameObject obj, int num){
			Vector3 pos = new Vector3 (0.0f, 0.0f, 0.0f);

			pos = obj.transform.localPosition;
			pos.x = tamagoCharaPositionInitTable [num].x;
			pos.y = tamagoCharaPositionInitTable [num].y;
			obj.transform.localPosition = pos;
		}



		// 画像データの差し替え
		private void SeasonImageSet(){
			SeasonImg _data = mData.assetBundle.LoadAllAssets<SeasonImg> ()[0];

			// 背景
			MinigameRoot.transform.Find ("base/bg").gameObject.GetComponent<Image> ().sprite = _data.ImgBG;
			// 雲
			MinigameRoot.transform.Find ("base/bg/cloud1").gameObject.GetComponent<Image> ().sprite = _data.ImgKumo;
			MinigameRoot.transform.Find ("base/bg/cloud2").gameObject.GetComponent<Image> ().sprite = _data.ImgKumo;
			// タイトル
			MinigameRoot.transform.Find ("base/title/title").gameObject.GetComponent<Image> ().sprite = _data.ImgTitle;
			// 紅葉
			MinigameRoot.transform.Find ("base/title/momiji").gameObject.GetComponent<Image> ().sprite = _data.ImgMomiji;
			MinigameRoot.transform.Find ("base/start/momiji").gameObject.GetComponent<Image> ().sprite = _data.ImgMomiji;
			MinigameRoot.transform.Find ("base/game/momiji").gameObject.GetComponent<Image> ().sprite = _data.ImgMomiji;
			MinigameRoot.transform.Find ("base/result/momiji").gameObject.GetComponent<Image> ().sprite = _data.ImgMomiji;
			MinigameRoot.transform.Find ("base/end/momiji").gameObject.GetComponent<Image> ().sprite = _data.ImgMomiji;
			// １０点アイテム
			MinigameRoot.transform.Find ("base/start/item_0").gameObject.GetComponent<Image> ().sprite = _data.ImgItem [0];
			MinigameRoot.transform.Find ("base/game/item_0").gameObject.GetComponent<Image> ().sprite = _data.ImgItem [0];
			// ２０点アイテム
			MinigameRoot.transform.Find ("base/start/item_1").gameObject.GetComponent<Image> ().sprite = _data.ImgItem [1];
			MinigameRoot.transform.Find ("base/game/item_1").gameObject.GetComponent<Image> ().sprite = _data.ImgItem [1];
			// ３０点アイテム
			MinigameRoot.transform.Find ("base/start/item_2").gameObject.GetComponent<Image> ().sprite = _data.ImgItem [2];
			MinigameRoot.transform.Find ("base/game/item_2").gameObject.GetComponent<Image> ().sprite = _data.ImgItem [2];
			// ５０点アイテム
			MinigameRoot.transform.Find ("base/start/item_3").gameObject.GetComponent<Image> ().sprite = _data.ImgItem [3];
			MinigameRoot.transform.Find ("base/game/item_3").gameObject.GetComponent<Image> ().sprite = _data.ImgItem [3];
			// １００点アイテム
			MinigameRoot.transform.Find ("base/start/item_4").gameObject.GetComponent<Image> ().sprite = _data.ImgItem [4];
			MinigameRoot.transform.Find ("base/game/item_4").gameObject.GetComponent<Image> ().sprite = _data.ImgItem [4];
			// お邪魔アイテム
			MinigameRoot.transform.Find ("base/game/item_5").gameObject.GetComponent<Image> ().sprite = _data.ImgItem [5];
		}



	}
}
