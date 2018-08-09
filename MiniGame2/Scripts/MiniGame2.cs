using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.System;
using Mix2App.Lib.Model;
using Mix2App.Lib.View;







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

		private object[]		mparam;

		private CharaBehaviour[] cbCharaTamagoMain = new CharaBehaviour[3];		// プレイヤーとゲスト
		private CharaBehaviour[] cbCharaTamago = new CharaBehaviour[12];		// お客様
		private bool startEndFlag = false;


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

		void Awake(){
			Debug.Log ("MiniGame2 Awake");
		}

		public void receive(params object[] parameter){
			Debug.Log ("MiniGame2 receive");
			mparam = parameter;
		}

		IEnumerator Start(){
			Debug.Log ("MiniGame2 Start");
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
//				baseSizePanel.GetComponent<Transform>().transform.localScale = new Vector3 (1.3f, 1.3f, 1.0f);	// 3:4の時のみ画面を拡大表示
			}


			for (int i = 0; i < 3; i++) {
				cbCharaTamagoMain [i] = CharaTamagoMain [i].GetComponent<CharaBehaviour> ();
			}
			for (int i = 0; i < 12; i++) {
				cbCharaTamago [i] = CharaTamago [i].GetComponent<CharaBehaviour> ();
			}

			yield return cbCharaTamagoMain [0].init (new TamaChara (16));
			TamagochiMainAnimeSet (0, "idle");
			yield return cbCharaTamagoMain [1].init (new TamaChara (17));
			TamagochiMainAnimeSet (1, "idle");
			yield return cbCharaTamagoMain [2].init (new TamaChara (18));
			TamagochiMainAnimeSet (2, "idle");

			yield return cbCharaTamago[0].init (new TamaChara (16));
			TamagochiAnimeSet (0, "idle");
			yield return cbCharaTamago[1].init (new TamaChara (17));
			TamagochiAnimeSet (1, "idle");
			yield return cbCharaTamago[2].init (new TamaChara (18));
			TamagochiAnimeSet (2, "idle");
			yield return cbCharaTamago[3].init (new TamaChara (19));
			TamagochiAnimeSet (3, "idle");
			yield return cbCharaTamago[4].init (new TamaChara (16));
			TamagochiAnimeSet (4, "idle");
			yield return cbCharaTamago[5].init (new TamaChara (17));
			TamagochiAnimeSet (5, "idle");
			yield return cbCharaTamago[6].init (new TamaChara (18));
			TamagochiAnimeSet (6, "idle");
			yield return cbCharaTamago[7].init (new TamaChara (19));
			TamagochiAnimeSet (7, "idle");
			yield return cbCharaTamago[8].init (new TamaChara (16));
			TamagochiAnimeSet (8, "idle");
			yield return cbCharaTamago[9].init (new TamaChara (17));
			TamagochiAnimeSet (9, "idle");
			yield return cbCharaTamago[10].init (new TamaChara (18));
			TamagochiAnimeSet (10, "idle");
			yield return cbCharaTamago[11].init (new TamaChara (19));
			TamagochiAnimeSet (11, "idle");



			startEndFlag = true;
		}

		void Destroy(){
			Debug.Log ("MiniGame2 Destroy");
		}

		void Update(){
			switch (jobCount) {
			case	statusJobCount.minigame2JobCount000:
				{
					if (startEndFlag) {
						EventTitle.SetActive (true);
						jobCount = statusJobCount.minigame2JobCount010;
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
					break;
				}
			case	statusJobCount.minigame2JobCount030:
				{
					if (EventStart.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.minigame2JobCount040;
						EventStart.SetActive (false);
						EventGame.SetActive (true);
					}
					TamagochiLoopIdou ();
					break;
				}
			case	statusJobCount.minigame2JobCount040:
				{
					break;
				}
			case	statusJobCount.minigame2JobCount050:
				{
					break;
				}
			case	statusJobCount.minigame2JobCount060:
				{
					break;
				}
			case	statusJobCount.minigame2JobCount070:
				{
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
			jobCount = statusJobCount.minigame2JobCount020;						// スタートボタンが押されたのでゲーム開始
		}
		private void ButtonCloseClick(){
		}
		private void ButtonHelpClick(){
		}
		private void ButtonYameruClick(){
		}
		private void ButtonTakuhaiClick(){
		}
		private void ButtonTojiruClick(){
		}
		private void ButtonModoruClick(){
		}


		private int[] tamagochiIdouFlag = new int[12];
		private int tamagochiIdouNumber = 0;
		private int[] tamagochiIdouNumberTable = new int[12];
		private void TamagochiLoopInit(){
			Vector2[] _initTable = new Vector2[] {
				new Vector2 (0.0f, 3.4f),
				new Vector2 (-2.25f, 3.4f),
				new Vector2 (2.25f, 3.4f),
			};


			for (int i = 0; i < 3; i++) {
				Vector3 pos = CharaTamagoMain [i].transform.localPosition;
				pos.x = _initTable [i].x;
				pos.y = _initTable [i].y;
				CharaTamagoMain [i].transform.localPosition = pos;
			}


			for (int i = 0; i < 12; i++) {
				Vector3 pos = CharaTamago [i].transform.localPosition;
				pos.x = (-7 - (i * 2));
				pos.y = (-6 - (i * 2));
				CharaTamago [i].transform.localPosition = pos;
				tamagochiIdouFlag [i] = 0;
				tamagochiIdouNumber = 0;
				tamagochiIdouNumberTable [i] = i;
			}
		}
		private void TamagochiLoopIdou(){
			Vector3 pos;
			for (int num = 0; num < 12; num++) {
				pos = CharaTamago [num].transform.localPosition;
				switch (tamagochiIdouFlag [num]) {
				case	0:
					{
						pos.x += 0.125f;
						pos.y += 0.125f;
						if (pos.x >= -4.0f) {
							pos.x = -4.0f;
							pos.y = -3.0f;
							tamagochiIdouFlag [num] = 1;
						}
						break;
					}
				case	1:
					{
						pos.x += 0.125f;
						if (pos.x >= 5.0f) {
							pos.x = 5.0f;
							tamagochiIdouFlag [num] = 2;
						}
						break;
					}
				case	2:
					{
						pos.y += 0.125f;
						if (pos.y >= -0.5f) {
							pos.y = -0.5f;
							tamagochiIdouFlag [num] = 3;
						}
						break;
					}
				case	3:
					{
						pos.x -= 0.125f;
						if (pos.x <= 1.8f) {
							pos.x = 1.8f;
							tamagochiIdouFlag [num] = 4;
						}
						break;
					}
				}
				CharaTamago [num].transform.localPosition = pos;
			}
		}

		private void TamagochiMainAnimeSet(int num,string status){
			if(cbCharaTamagoMain[num].nowlabel != status){
				cbCharaTamagoMain [num].gotoAndPlay (status);
			}
		}
		private void TamagochiAnimeSet(int num,string status){
			if (cbCharaTamago [num].nowlabel != status) {
				cbCharaTamago [num].gotoAndPlay (status);
			}
		}

	}
}