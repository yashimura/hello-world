using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.System;
using Mix2App.Lib.Model;
using Mix2App.Lib.View;



namespace Mix2App.MachiCon{
	public class MachiCon : MonoBehaviour,IReceiver {
		[SerializeField] private Message	MesDisp;						// メッセージ関連
		[SerializeField] private GameObject EventCurtain;					// カーテン
		[SerializeField] private GameObject EventTitle;						// たまキュンパーティータイトル
		[SerializeField] private GameObject EventTitleClock;				// たまキュンパーティーカウントダウンマーク
		[SerializeField] private GameObject EventTitleText;					// たまキュンパーティーカウントダウン時間
		[SerializeField] private GameObject[] CharaTamago;					// たまごっち
		[SerializeField] private GameObject[] fukidashi;					// 吹き出し
		[SerializeField] private GameObject EventPhase;						// 幕間
		[SerializeField] private GameObject EventPhaseRing;					// 幕間ナンバーリング
		[SerializeField] private GameObject EventPhasePTStar;				// 幕間ナンバーエフェクト
		[SerializeField] private GameObject EventPhaseCount;				// 幕間ナンバー
		[SerializeField] private Sprite[] EventPhaseSprite;					// 幕間ナンバーデーター
		[SerializeField] private GameObject EventSoudan;					// 相談
		[SerializeField] private GameObject EventSoudanChara;				// 相談たまごっち
		[SerializeField] private GameObject EventSoudanTarget;				// 相談対象たまごっち
		[SerializeField] private GameObject EventSoudanAvaterNameOld;		// 旧アバターネーム
		[SerializeField] private GameObject EventSoudanTamagoNameOld;		// 旧たまごっちネーム
		[SerializeField] private GameObject EventSoudanAvaterNameNew;		// 新アバターネーム
		[SerializeField] private GameObject EventSoudanTamagoNameNew;		// 新たまごっちネーム
		[SerializeField] private GameObject	EventSoudanYesOld;				// 旧Yesボタン
		[SerializeField] private GameObject	EventSoudanNoOld;				// 旧Noボタン
		[SerializeField] private GameObject	EventSoudanYesNew;				// 新Yesボタン
		[SerializeField] private GameObject	EventSoudanNoNew;				// 新Noボタン
		[SerializeField] private GameObject EventSoudanKumo;				// 雲
		[SerializeField] private GameObject EventAppeal;					// アピールタイム
		[SerializeField] private GameObject EventKokuhaku;					// 告白タイム
		[SerializeField] private GameObject EventWaitStop;					// ちょっとまった！！
		[SerializeField] private GameObject EventResult;					// 告白結果
		[SerializeField] private GameObject EventLastResult;				// 最終結果
		[SerializeField] private GameObject EventEnd;						// おしまい



		private object[]		mparam;

		private int playerNumber = 0;					// ０〜７（０〜３：男の子、４〜７：女の子）
		private int targetNumber = 0;					// ０〜７（０〜３：男の子、４〜７：女の子）
		private bool playerResultFlag = false;			// カップル成立:true 不成立:false

		private bool buttonFlag = false;
		private bool btnYesNo = false;					// Yes:true No:false
		private int sceneNumber = 0;					// シーンナンバー

		private bool kokuhakuTimeEndFlag = true;

		private CharaBehaviour[] cbTamagoChara = new CharaBehaviour[8];


		// 相性度（０〜１００）（男の子、女の子）
		private int[,] loveManWoman = new int[4, 4]{ { 40,40,40,40 }, { 40,40,40,40 }, { 40,40,40,40 }, { 40,40,40,40 } };
		// 成否判定の種（０〜１００）（男の子、女の子）
		private int[,] loveManWomanFix = new int[4, 4];

		private statusJobCount	jobCount = statusJobCount.machiconJobCount000;
		private enum statusJobCount{
			machiconJobCount000,
			machiconJobCount010,
			machiconJobCount020,
			machiconJobCount030,
			machiconJobCount040,
			machiconJobCount050,
			machiconJobCount060,
			machiconJobCount070,
			machiconJobCount080,
			machiconJobCount090,
			machiconJobCount100,
			machiconJobCount110,
			machiconJobCount120,
			machiconJobCount130,
			machiconJobCount140,
			machiconJobCount150,
			machiconJobCount160,
			machiconJobCount170,
			machiconJobCount180,
			machiconJobCount190,
			machiconJobCount200,
			machiconJobCount210,
			machiconJobCount220,
			machiconJobCount230,
			machiconJobCount240,
			machiconJobCount250,
			machiconJobCount260,
			machiconJobCount270,
			machiconJobCount280,
			machiconJobCount290,
			machiconJobCount300,
			machiconJobCount310,
			machiconJobCount320,
			machiconJobCount330,
			machiconJobCount340,
			machiconJobCount350,
		}

		private statusKokuhakuCount kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount000;
		private enum statusKokuhakuCount{
			kokuhakuCount000,
			kokuhakuCount010,
			kokuhakuCount020,
			kokuhakuCount030,
			kokuhakuCount040,
			kokuhakuCount050,
			kokuhakuCount060,
			kokuhakuCount070,
			kokuhakuCount080,
			kokuhakuCount090,
			kokuhakuCount100,
			kokuhakuCount110,
			kokuhakuCount120,
			kokuhakuCount130,
			kokuhakuCount140,
			kokuhakuCount150,
			kokuhakuCount160,
			kokuhakuCount170,
			kokuhakuCount180,
			kokuhakuCount190,
			kokuhakuCount200,
			kokuhakuCount210,
			kokuhakuCount220,
			kokuhakuCount230,
			kokuhakuCount240,
			kokuhakuCount250,
			kokuhakuCount260,
			kokuhakuCount270,
			kokuhakuCount280,
			kokuhakuCount290,
			kokuhakuCount300,
			kokuhakuCount310,
			kokuhakuCount320,
			kokuhakuCount330,
			kokuhakuCount340,
			kokuhakuCount350,
			kokuhakuCount360,
			kokuhakuCount370,
			kokuhakuCount380,
			kokuhakuCount390,
			kokuhakuCount400,
			kokuhakuCount410,
		};

		void Awake(){
			Debug.Log ("MachiCon Awake");
		}

		public void receive(params object[] parameter){
			Debug.Log ("MachiCon receive");
			mparam = parameter;

			// イベントたまキュンか？
			// コラボたまキュンか？
			// 通常たまキュンか？


		}
			
		IEnumerator Start(){
			Debug.Log ("MachiCon start");

			for (int i = 0; i < 4; i++) {
				for (int i2 = 0; i2 < 4; i2++) {
					// これは本当は、サーバーで決定してもらう方が良い（他の参加ユーザーとの同期を取る為には、サーバー側で管理してもらう必要がある）
					loveManWomanFix [i, i2] = Random.Range (0, 100);
				}
			}

			for (int i = 0; i < 9; i++) {
				fukidashi [i].SetActive (false);
			}
			FukidashiPositionInit ();

			playerNumber = Random.Range (0, 8);
			playerResultFlag = false;

			EventSoudanYesNew.GetComponent<Button> ().onClick.AddListener (ButtneYesClick);
			EventSoudanNoNew.GetComponent<Button> ().onClick.AddListener (ButtonNoClick);

			jobCount = statusJobCount.machiconJobCount000;

			for (int i = 0; i < 8; i++) {
				cbTamagoChara [i] = CharaTamago [i].GetComponent<CharaBehaviour> ();
			}
			yield return cbTamagoChara[0].init (new TamaChara (16));				// 男の子１のたまごっちを登録する
			yield return cbTamagoChara[1].init (new TamaChara (17));				// 男の子２のたまごっちを登録する
			yield return cbTamagoChara[2].init (new TamaChara (16));				// 男の子３のたまごっちを登録する
			yield return cbTamagoChara[3].init (new TamaChara (17));				// 男の子４のたまごっちを登録する
			yield return cbTamagoChara[4].init (new TamaChara (18));				// 女の子１のたまごっちを登録する
			yield return cbTamagoChara[5].init (new TamaChara (19));				// 女の子２のたまごっちを登録する
			yield return cbTamagoChara[6].init (new TamaChara (18));				// 女の子３のたまごっちを登録する
			yield return cbTamagoChara[7].init (new TamaChara (19));				// 女の子４のたまごっちを登録する



		}
	
		void Destroy(){
			Debug.Log ("MachiCon Destroy");
		}

		private float	countTime1;
		private int		countTime2;
		private int		waitTime;
		private Vector3[] posTamago = new Vector3[8] {
			new Vector3 (0.0f, -7.0f, 0.0f),
			new Vector3 (0.0f, -7.0f, 0.0f),
			new Vector3 (0.0f, -7.0f, 0.0f),
			new Vector3 (0.0f, -7.0f, 0.0f),
			new Vector3 (0.0f, -7.0f, 0.0f),
			new Vector3 (0.0f, -7.0f, 0.0f),
			new Vector3 (0.0f, -7.0f, 0.0f),
			new Vector3 (0.0f, -7.0f, 0.0f),
		};
		private int		posNumber;


		void Update(){
			switch (jobCount) {
			case	statusJobCount.machiconJobCount000:
				{
					countTime1 = 0.0f;
					countTime2 = 10;

					MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp01);

					jobCount = statusJobCount.machiconJobCount010;
					break;
				}
			case	statusJobCount.machiconJobCount010:
				{	// カウントダウン
					countTime1 += 1.0f * Time.deltaTime;
					EventTitleClock.GetComponent<Image> ().fillAmount = countTime1;
					if (countTime1 >= 1.0f) {
						countTime1 -= 1.0f;
						countTime2--;
					}
					EventTitleText.GetComponent<Text> ().text = countTime2.ToString ();			// カウントダウン数字
					if (countTime2 == 0) {
						jobCount = statusJobCount.machiconJobCount020;

						EventTitle.GetComponent<Animator> ().SetBool ("title", true);
						waitTime = 60;
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
					}
					break;
				}
			case	statusJobCount.machiconJobCount020:
				{
					if(WaitTimeSubLoop()){
						jobCount = statusJobCount.machiconJobCount030;
					}
					break;
				}
			case	statusJobCount.machiconJobCount030:
				{
					if (EventCurtainPositionChange (10.0f)) {									// カーテンオープン
						EventTitle.SetActive (false);
						EventCurtain.SetActive (false);
						jobCount = statusJobCount.machiconJobCount040;
						posNumber = 0;
					}
					break;
				}
			case	statusJobCount.machiconJobCount040:
				{	// 入場開始
					FlameInMessageDisp ();														// 実況開始
					FlameInDirectionSet ();														// 入場時の向きを設定

					cbTamagoChara [posNumber].gotoAndPlay ("walk");
					jobCount = statusJobCount.machiconJobCount050;
					break;
				}
			case	statusJobCount.machiconJobCount050:
				{	// たまごっち入場
					float pos = FlameInPositionChange ();
					if (pos >= 1.0f) {
						jobCount = statusJobCount.machiconJobCount060;
						waitTime = 11;															// ジャンプの時間なのでここを変える場合FlameInPositionChangeJumpのテーブルも変更する事
						cbTamagoChara [posNumber].gotoAndPlay ("idle");
					}
					break;
				}
			case	statusJobCount.machiconJobCount060:
				{	// ジャンプ
					if (WaitTimeSubLoop ()) {
						jobCount = statusJobCount.machiconJobCount070;
						posTamago [posNumber].y = 1.0f;
						fukidashi [0].SetActive (true);
						FukidashiMessageSet ();
						waitTime = 60;
					} else {
						FlameInPositionChangeJump ();
					}
					break;
				}
			case	statusJobCount.machiconJobCount070:
				{	// コメント表示
					if (WaitTimeSubLoop()) {
						jobCount = statusJobCount.machiconJobCount080;
						fukidashi [0].SetActive (false);
						waitTime = 40;															// 整列までの移動時間なのでここを変える場合AlignmentPositionChangeのテーブルも変更する事
						cbTamagoChara [posNumber].gotoAndPlay ("walk");
					}
					break;
				}
			case	statusJobCount.machiconJobCount080:
				{	// 所定位置への整列
					AlignmentPositionChange ();
					if (WaitTimeSubLoop()) {
						jobCount = statusJobCount.machiconJobCount090;
					}
					break;
				}
			case	statusJobCount.machiconJobCount090:
				{	// 整列位置に停止
					cbTamagoChara [posNumber].gotoAndPlay ("idle");
					AlignmentPositionChangeFinish ();
					posNumber++;
					if (posNumber == 8) {
						jobCount = statusJobCount.machiconJobCount100;
						EventCurtain.SetActive (true);
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp06);
						sceneNumber = 0;
					} else {
						jobCount = statusJobCount.machiconJobCount040;
					}
					break;
				}
			case	statusJobCount.machiconJobCount100:
				{
					if (EventCurtainPositionChange (-10.0f)) {									// カーテンクローズ
						jobCount = statusJobCount.machiconJobCount110;

						EventPhase.SetActive (true);
						EventPhasePTStar.SetActive (true);
						EventPhaseCount.GetComponent<Image> ().sprite = EventPhaseSprite [sceneNumber];
//						EventPhase.transform.Find("phase_count").gameObject.GetComponent<Image>().sprite = EventPhaseSprite[sceneNumber];
						waitTime = 60;

						TargetRandomSet ();														// 相談相手の決定

						EventAppeal.SetActive (false);
					}
					break;
				}
			case	statusJobCount.machiconJobCount110:
				{	// 相談タイム開始
					EventPhasePTStar.transform.RotateAround (EventPhaseRing.transform.position, new Vector3 (0, 0, 1), 360.0f * Time.deltaTime);
					if (WaitTimeSubLoop()) {
						jobCount = statusJobCount.machiconJobCount120;
						EventPhasePTStar.SetActive (false);
						EventPhase.GetComponent<Animator> ().SetBool ("phase", true);
					}
					break;
				}
			case	statusJobCount.machiconJobCount120:
				{
					if (EventPhase.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.machiconJobCount130;
						waitTime = 60;
					}
					break;
				}
			case	statusJobCount.machiconJobCount130:
				{
					if (WaitTimeSubLoop()) {
						jobCount = statusJobCount.machiconJobCount140;
						EventPhase.GetComponent<Animator> ().SetBool ("phaseend", true);
					}
					break;
				}
			case	statusJobCount.machiconJobCount140:
				{
					if (EventPhase.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.machiconJobCount150;
						EventPhase.SetActive (false);
						EventSoudan.SetActive (true);
						EventSoudanNameSet ();													// 相談する時の相手の名前などを登録する
						EventSoudanPartsDispOff ();												// 相談で不必要なパーツを消す

						for (int i = 0; i < 8; i++) {
							cbTamagoChara [i].gotoAndPlay ("idle");
						}
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
					}
					break;
				}
			case	statusJobCount.machiconJobCount150:
				{
					if (EventSoudan.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.machiconJobCount160;
						SoudanMessageDisp ();													// 相談タイトルメッセージ設定
						CharaTamago [targetNumber].GetComponent<SpriteRenderer> ().sortingLayerName = "Chara";	// 優先順位をカーテンの手前にする
						CharaTamago [targetNumber].GetComponent<SpriteRenderer> ().flipX = true;
						posTamago [targetNumber].x = 0.0f;
						posTamago [targetNumber].y = 1.5f;

						EventSoudanPartsDisp (true);
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp07);
					}
					CharaTamago [playerNumber].GetComponent<SpriteRenderer> ().sortingLayerName = "Chara";		// 優先順位をカーテンの手前にする
					CharaTamago [playerNumber].GetComponent<SpriteRenderer> ().flipX = false;
					posTamago [playerNumber].x = 2.0f;
					posTamago [playerNumber].y = ((EventSoudanChara.transform.localPosition.y / 50.0f) + 1.0f);	// たまごっちを上昇させる

					break;
				}
			case	statusJobCount.machiconJobCount160:
				{
					if (buttonFlag) {
						jobCount = statusJobCount.machiconJobCount170;
						EventSoudan.GetComponent<Animator> ().SetBool ("waite", true);
						EventSoudanPartsDisp (false);

						CharaTamago [targetNumber].GetComponent<SpriteRenderer> ().sortingLayerName = "Default";	// 優先順位をデフォルトに戻す
						MesDisp.SoudanMesDisp (Message.SoudanMesTable.SoudanMesDispOff);

						EventAppeal.SetActive (true);
					}
					break;
				}
			case	statusJobCount.machiconJobCount170:
				{
					posTamago [playerNumber].y -= 0.5f;											// たまごっちを下降させる
					if (posTamago [playerNumber].y <= -10.0f) {
						jobCount = statusJobCount.machiconJobCount180;
						CharaTamago [playerNumber].GetComponent<SpriteRenderer> ().sortingLayerName = "Default";	// 優先順位をデフォルトに戻す
						AppealPositionChangeInit ();											// アピールタイム初期表示位置設定
						TamagochiFukidashiOff ();
						waitTime = 90;

						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
					}
					break;
				}
			case	statusJobCount.machiconJobCount180:
				{
					if (WaitTimeSubLoop ()) {
						jobCount = statusJobCount.machiconJobCount190;
					}
					break;
				}
			case	statusJobCount.machiconJobCount190:
				{
					if (EventCurtainPositionChange (10.0f)) {									// カーテンオープン
						jobCount = statusJobCount.machiconJobCount200;
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp08);
						for (int i = 0; i < 8; i++) {
							fukidashi [i + 1].SetActive (true);
						}
					}
					waitTime = 120;
					break;
				}
			case	statusJobCount.machiconJobCount200:
				{
					if (WaitTimeSubLoop ()) {
						jobCount = statusJobCount.machiconJobCount210;
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp09);
						for (int i = 0; i < 8; i++) {
							fukidashi [i + 1].SetActive (false);
						}
						waitTime = 600;
					}
/////					AppealPositionChangeLoop ();
					break;
				}
			case	statusJobCount.machiconJobCount210:
				{
					ApplealPositionChangeMain ();												// アピールタイムのキャラ移動

					// 一定時間で実況表示を変える
					if ((waitTime == 200) || (waitTime == 400)) {
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp09);
					}
					if (WaitTimeSubLoop ()) {
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
						sceneNumber++;
						if (sceneNumber < 3) {
							buttonFlag = false;
							EventSoudan.SetActive (false);
							jobCount = statusJobCount.machiconJobCount100;
						} else {
							jobCount = statusJobCount.machiconJobCount220;
						}
						TamagochiFukidashiOff ();
					}
					break;
				}
			case	statusJobCount.machiconJobCount220:
				{
					if (EventCurtainPositionChange (-10.0f)) {									// カーテンクローズ
						jobCount = statusJobCount.machiconJobCount230;
						waitTime = 60;
						EventAppeal.SetActive (false);
					}
					break;
				}
			case	statusJobCount.machiconJobCount230:
				{
					if (WaitTimeSubLoop ()) {
						jobCount = statusJobCount.machiconJobCount240;
						EventKokuhaku.SetActive (true);											// 告白タイム表示
						waitTime = 60;
						KokuhakuPositionInit ();												// 告白タイム各キャラクターの初期配置
						KokuhakuTimeInit ();													// 男の子の告白する女の子の番号を登録
					}
					break;
				}
			case	statusJobCount.machiconJobCount240:
				{
					if (WaitTimeSubLoop ()) {
						jobCount = statusJobCount.machiconJobCount250;
					}
					break;
				}
			case	statusJobCount.machiconJobCount250:
				{
					if (EventCurtainPositionChange (10.0f)) {									// カーテンオープン
						jobCount = statusJobCount.machiconJobCount260;
						EventKokuhaku.SetActive (false);
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp10);
						kokuhakuTimeEndFlag = false;
					}
					break;
				}
			case	statusJobCount.machiconJobCount260:
				{
					if (kokuhakuTimeEndFlag) {
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp15);		// 告白処理が終了したのでお別れメッセージを表示
						jobCount = statusJobCount.machiconJobCount270;
					}
					KokuhakuTimeMain ();														// 告白処理ループ
					break;
				}
			case	statusJobCount.machiconJobCount270:
				{
					if (EventCurtainPositionChange (-10.0f)) {									// カーテンクローズ
						jobCount = statusJobCount.machiconJobCount280;

						EventEnd.SetActive (true);												// おしまいを表示準備
						Color color = EventEnd.transform.Find ("text").gameObject.GetComponent<Image> ().color;
						color.a = 0.0f;
						EventEnd.transform.Find ("text").gameObject.GetComponent<Image> ().color = color;
					}
					break;
				}
			case	statusJobCount.machiconJobCount280:
				{
					Color color = EventEnd.transform.Find ("text").gameObject.GetComponent<Image> ().color;
					color.a += 0.02f;															// おしまいをフェードイン
					if (color.a >= 1.0f) {
						color.a = 1.0f;
						jobCount = statusJobCount.machiconJobCount290;
						waitTime = 30;
					}
					EventEnd.transform.Find ("text").gameObject.GetComponent<Image> ().color = color;
					break;
				}
			case	statusJobCount.machiconJobCount290:
				{
					if (WaitTimeSubLoop ()) {
						if (playerResultFlag) {
							Debug.Log ("結婚イベントへ・・・");
						} else {
							Debug.Log ("たまタウンへ・・・");
						}

						jobCount = statusJobCount.machiconJobCount300;
					}
					break;
				}
			case	statusJobCount.machiconJobCount300:
				{
					break;
				}
			case	statusJobCount.machiconJobCount310:
				{
					break;
				}
			case	statusJobCount.machiconJobCount320:
				{
					break;
				}
			case	statusJobCount.machiconJobCount330:
				{
					break;
				}
			case	statusJobCount.machiconJobCount340:
				{
					break;
				}
			case	statusJobCount.machiconJobCount350:
				{
					break;
				}
			}

			for (int i = 0; i < 8; i++) {								// 各キャラの表示位置を登録
				CharaTamago [i].transform.localPosition = posTamago [i];
			}
		}

		private bool WaitTimeSubLoop(){
			waitTime--;
			if (waitTime == 0) {
				return true;
			}
			return false;
		}


		private void FukidashiPositionInit(){
			Vector2[] pos = new Vector2[9] {
				new Vector2 (   0.0f,  120.0f),		// 入場時の吹き出し
				new Vector2 (-105.0f,   60.0f),		// アピールタイムの男の子１の吹き出し
				new Vector2 ( 285.0f,   60.0f),		// アピールタイムの男の子２の吹き出し
				new Vector2 (-215.0f, -150.0f),		// アピールタイムの男の子３の吹き出し
				new Vector2 ( 410.0f, -150.0f),		// アピールタイムの男の子４の吹き出し
				new Vector2 (-280.0f,   60.0f),		// アピールタイムの女の子１の吹き出し
				new Vector2 ( 105.0f,   60.0f),		// アピールタイムの女の子２の吹き出し
				new Vector2 (-400.0f, -150.0f),		// アピールタイムの女の子３の吹き出し
				new Vector2 ( 220.0f, -150.0f),		// アピールタイムの女の子４の吹き出し
			};

			for(int i = 0;i < 9;i++){
				Vector3 pos2 = fukidashi [i].transform.localPosition;
				pos2.x = pos [i].x;
				pos2.y = pos [i].y;
				fukidashi [i].transform.localPosition = pos2;
			}
		}

		// 入場時のたまごっちの吹き出しメッセージ登録
		private void FukidashiMessageSet(){
			string mesAvater = "";
			string mesTamago = "";
			string mesRet = "\n";

			switch (posNumber) {
			case	0:
				{
					mesAvater = "アバターネーム1";
					mesTamago = "たまごっちネームごび！";
					break;
				}
			case	1:
				{
					mesAvater = "アバターネーム2";
					mesTamago = "たまごっちネームごび！";
					break;
				}
			case	2:
				{
					mesAvater = "アバターネーム3";
					mesTamago = "たまごっちネームごび！";
					break;
				}
			case	3:
				{
					mesAvater = "アバターネーム4";
					mesTamago = "たまごっちネームごび！";
					break;
				}
			case	4:
				{
					mesAvater = "アバターネーム5";
					mesTamago = "たまごっちネームごび！";
					break;
				}
			case	5:
				{
					mesAvater = "アバターネーム6";
					mesTamago = "たまごっちネームごび！";
					break;
				}
			case	6:
				{
					mesAvater = "アバターネーム7";
					mesTamago = "たまごっちネームごび！";
					break;
				}
			case	7:
				{
					mesAvater = "アバターネーム8";
					mesTamago = "たまごっちネームごび！";
					break;
				}
			}

			fukidashi [0].transform.Find ("text").gameObject.GetComponent<Text> ().text = mesAvater + mesRet + mesTamago + mesRet;
		}
		// 告白タイムの対象相手を指定する
		// num:男の子の番号（０〜３）、targetNum:告白対象者の番号（４〜８）
		private void FukidashiMessageSetKokuhaku(int num,int targetNum){
			float[,] fukidashiPos = new float[4, 2] {
				{ 125.0f,  125.0f },								// 男の子１の吹き出し位置
				{ 155.0f,   35.0f },								// 男の子２の吹き出し位置
				{ 185.0f,  -65.0f },								// 男の子３の吹き出し位置
				{ 215.0f, -155.0f }									// 男の子４の吹き出し位置
			};

			string mesAvater = "";
			string mesTamago = "";
			string mesRet = "\n";
			string mesNo = "の";
			string mesTo = "さん";

			switch (targetNum) {
			case	4:
				{
					mesAvater = "アバターネーム5";
					mesTamago = "たまごっちネーム";
					break;
				}
			case	5:
				{
					mesAvater = "アバターネーム6";
					mesTamago = "たまごっちネーム";
					break;
				}
			case	6:
				{
					mesAvater = "アバターネーム7";
					mesTamago = "たまごっちネーム";
					break;
				}
			case	7:
				{
					mesAvater = "アバターネーム8";
					mesTamago = "たまごっちネーム";
					break;
				}
			}

			fukidashi [0].transform.Find ("text").gameObject.GetComponent<Text> ().text = mesAvater + mesNo + mesRet + mesTamago + mesTo + mesRet;

			Vector3 pos = fukidashi [0].transform.localPosition;
			pos.x = fukidashiPos [num, 0];
			pos.y = fukidashiPos [num, 1];
			fukidashi [0].transform.localPosition = pos;
		}
		// 告白された事に対する返事のメッセージ（肯定、否定）
		// num:告白された女の子の番号（０〜３）、msgFlag:メッセージの種類（true:肯定、false:否定）
		private void FukidashiMessageKokuhakuReturn(int num,bool msgFlag){
			float[,] fukidashiPos = new float[4, 2] {
				{ -125.0f,  125.0f },								// 女の子１の吹き出し位置
				{ -155.0f,   35.0f },								// 女の子２の吹き出し位置
				{ -185.0f,  -65.0f },								// 女の子３の吹き出し位置
				{ -215.0f, -155.0f }								// 女の子４の吹き出し位置
			};

			if (msgFlag) {		// 告白肯定のメッセージ
				fukidashi [0].transform.Find ("text").gameObject.GetComponent<Text> ().text = MesDisp.KokuhakuMesDisp (Message.KokuhakuMesTable.KokuhakuMesDispOK);
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp13);
			} else {			// 告白否定のメッセージ
				fukidashi [0].transform.Find ("text").gameObject.GetComponent<Text> ().text = MesDisp.KokuhakuMesDisp (Message.KokuhakuMesTable.KokuhakuMesDispNo);
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp14);
			}
				
			Vector3 pos = fukidashi [0].transform.localPosition;
			pos.x = fukidashiPos [num, 0];
			pos.y = fukidashiPos [num, 1];
			fukidashi [0].transform.localPosition = pos;
		}


		// たまごっちが画面下から入場する時の移動
		private float FlameInPositionChange(){
			posTamago [posNumber].y += 0.05f;

			return posTamago[posNumber].y;
		}

		// たまごっちがジャンプしたように見せる
		private void FlameInPositionChangeJump(){
			float[] posy = new float[] {
				1.0f, 1.0f, 1.2f, 1.4f, 1.6f, 1.8f, 2.0f, 1.8f, 1.6f, 1.4f, 1.2f, 1.0f
			};

			posTamago [posNumber].y = posy [waitTime];
		}

		// 入場時の表示向き
		private void FlameInDirectionSet(){
			bool flag = true;
			switch (posNumber) {
			case	0:
			case	1:
			case	2:
			case	3:
				{
					flag = false;
					break;
				}
			case	4:
			case	5:
			case	6:
			case	7:
				{
					flag = true;
					break;
				}
			}
			CharaTamago [posNumber].GetComponent<SpriteRenderer> ().flipX = flag;
		}

		// 入場時の実況
		private void FlameInMessageDisp (){
			switch (posNumber) {
			case	0:
				{
					// 男の子１人目
					MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp02);
					break;
				}
			case	1:
			case	2:
			case	3:
				{
					// 男の子２、３、４人目
					MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp03);
					break;
				}
			case	4:
				{
					// 女の子１人目
					MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp04);
					break;
				}
			case	5:
			case	6:
			case	7:
				{
					// 女の子２、３、４人目
					MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp05);
					break;
				}
			}
		}

		// 相談時のタイトルメッセージ
		private void SoudanMessageDisp(){
			switch (playerNumber) {
			case	0:
			case	1:
			case	2:
			case	3:
				{
					// みーつユーザー男の子
					MesDisp.SoudanMesDisp (Message.SoudanMesTable.SoudanMesDispMan1);

					// みーつユーザー以外男の子
//					MesDisp.SoudanMesDisp (Message.SoudanMesTable.SoudanMesDispMan2);
					break;
				}
			case	4:
			case	5:
			case	6:
			case	7:
				{
					// みーつユーザー女の子
					MesDisp.SoudanMesDisp (Message.SoudanMesTable.SoudanMesDispWoman1);

					// みーつユーザー以外女の子
//					MesDisp.SoudanMesDisp (Message.SoudanMesTable.SoudanMesDispWoman2);
					break;
				}
			}
		}

		// 入場時の整列位置への移動
		private void AlignmentPositionChange(){
			Vector2[] speed = new Vector2[8] {
				new Vector2(  1.7f / 40.0f, -4.0f / 40.0f ),		// 男の子１の整列位置への移動力
				new Vector2(  3.4f / 40.0f, -3.5f / 40.0f ),		// 男の子２の整列位置への移動力
				new Vector2(  5.1f / 40.0f, -3.0f / 40.0f ),		// 男の子３の整列位置への移動力
				new Vector2(  6.8f / 40.0f, -2.5f / 40.0f ),		// 男の子４の整列位置への移動力
				new Vector2( -1.7f / 40.0f, -4.0f / 40.0f ),		// 女の子１の整列位置への移動力
				new Vector2( -3.4f / 40.0f, -3.5f / 40.0f ),		// 女の子２の整列位置への移動力
				new Vector2( -5.1f / 40.0f, -3.0f / 40.0f ),		// 女の子３の整列位置への移動力
				new Vector2( -6.8f / 40.0f, -2.5f / 40.0f ),		// 女の子４の整列位置への移動力
			};

			bool flag = true;

			switch (posNumber) {
			case	0:
			case	1:
			case	2:
			case	3:
				{
					flag = true;
					break;
				}
			case	4:
			case	5:
			case	6:
			case	7:
				{
					flag = false;
					break;
				}
			}
			CharaTamago [posNumber].GetComponent<SpriteRenderer> ().flipX = flag;

			posTamago [posNumber].x += speed [posNumber].x;
			posTamago [posNumber].y += speed [posNumber].y;
		}

		// 入場後の整列位置
		private void AlignmentPositionChangeFinish(){
			float[,] pos = new float[8, 2] {
				{  1.7f, -3.0f },									// 男の子１の整列位置
				{  3.4f, -2.5f },									// 男の子２の整列位置
				{  5.1f, -2.0f },									// 男の子３の整列位置
				{  6.8f, -1.5f },									// 男の子４の整列位置
				{ -1.7f, -3.0f },									// 女の子１の整列位置
				{ -3.4f, -2.5f },									// 女の子２の整列位置
				{ -5.1f, -2.0f },									// 女の子３の整列位置
				{ -6.8f, -1.5f },									// 女の子４の整列位置
			};
			bool flag = true;

			switch (posNumber) {
			case	0:
			case	1:
			case	2:
			case	3:
				{
					flag = false;
					break;
				}
			case	4:
			case	5:
			case	6:
			case	7:
				{
					flag = true;
					break;
				}
			}
			CharaTamago [posNumber].GetComponent<SpriteRenderer> ().flipX = flag;

			posTamago [posNumber].x = pos [posNumber, 0];
			posTamago [posNumber].y = pos [posNumber, 1];
		}

		// アピールタイムの配置順番
		private int[] appealPosCharaTable = new int [8]{ 0, 1, 2, 3, 4, 5, 6, 7 };
		// アピールタイムの配置順番変更
		private void AppealCharaPositionChange(){
			int[,] numTbl = new int[24, 4] {
				{ 0, 1, 2, 3 },
				{ 0, 1, 3, 2 },
				{ 0, 2, 1, 3 },
				{ 0, 2, 3, 1 },
				{ 0, 3, 1, 2 },
				{ 0, 3, 2, 1 },
				{ 1, 0, 2, 3 },
				{ 1, 0, 3, 2 },
				{ 1, 2, 0, 3 },
				{ 1, 2, 3, 0 },
				{ 1, 3, 0, 2 },
				{ 1, 3, 2, 0 },
				{ 2, 0, 1, 3 },
				{ 2, 0, 3, 1 },
				{ 2, 1, 0, 3 },
				{ 2, 1, 3, 0 },
				{ 2, 3, 0, 1 },
				{ 2, 3, 1, 0 },
				{ 3, 0, 1, 2 },
				{ 3, 0, 2, 1 },
				{ 3, 1, 0, 2 },
				{ 3, 1, 2, 0 },
				{ 3, 2, 0, 1 },
				{ 3, 2, 1, 0 },
			};
			int num = Random.Range (0, 24);
			int num2 = Random.Range (0, 24);

			for (int i = 0; i < 4; i++) {
				appealPosCharaTable [i] = numTbl [num, i];
			}

			for (int i = 4; i < 8; i++) {
//				appealPosCharaTable [i] = i;
				appealPosCharaTable [i] = numTbl [num2, i - 4] + 4;
			}
		}

		// アピールタイムの初期配置
		private void AppealPositionChangeInit(){
			float[,] pos = new float[8, 2] {
				{ -1.7f,  0.5f },									// 男の子１の初期位置
				{  4.5f,  0.5f },									// 男の子２の初期位置
				{ -3.5f, -3.0f },									// 男の子３の初期位置
				{  6.5f, -3.0f },									// 男の子４の初期位置
				{ -4.5f,  0.5f },									// 女の子１の初期位置
				{  1.7f,  0.5f },									// 女の子２の初期位置
				{ -6.5f, -3.0f },									// 女の子３の初期位置
				{  3.5f, -3.0f },									// 女の子４の初期位置
			};

			bool flag = true;
			AppealCharaPositionChange ();

			for (int i = 0; i < 8; i++) {
				switch (i) {
				case	0:
				case	1:
				case	2:
				case	3:
					{
						flag = false;
						break;
					}
				case	4:
				case	5:
				case	6:
				case	7:
					{
						flag = true;
						break;
					}
				}
				CharaTamago [i].GetComponent<SpriteRenderer> ().flipX = flag;

				posTamago [i].x = pos [appealPosCharaTable [i], 0];
				posTamago [i].y = pos [appealPosCharaTable [i], 1];

				cbTamagoChara [i].gotoAndPlay ("idle");
			}

			CharaTamago [0].GetComponent<SpriteRenderer> ().sortingOrder = 4;
			CharaTamago [1].GetComponent<SpriteRenderer> ().sortingOrder = 3;
			CharaTamago [2].GetComponent<SpriteRenderer> ().sortingOrder = 2;
			CharaTamago [3].GetComponent<SpriteRenderer> ().sortingOrder = 1;
			CharaTamago [4].GetComponent<SpriteRenderer> ().sortingOrder = 4;
			CharaTamago [5].GetComponent<SpriteRenderer> ().sortingOrder = 3;
			CharaTamago [6].GetComponent<SpriteRenderer> ().sortingOrder = 2;
			CharaTamago [7].GetComponent<SpriteRenderer> ().sortingOrder = 1;

			appealTimeCounter = 0;
		}
			
		// アピールタイムのキャラ移動（そわそわ）
		private void AppealPositionChangeLoop(){
			for (int i = 0; i < 8; i++) {
				posTamago [i].x += Random.Range (-0.005f, 0.005f);
				posTamago [i].y += Random.Range (-0.005f, 0.005f);
			}
		}

		private int	appealTimeCounter = 0;
		private float[] posTamagoSpeedX = new float[8] {
			0.02f, 0.02f, 0.02f, 0.02f, 0.02f, 0.02f, 0.02f, 0.02f
		};
		private float[] posTamagoSpeedY = new float[8] {
			0.02f, 0.02f, 0.02f, 0.02f, 0.02f, 0.02f, 0.02f, 0.02f
		};
		private bool[] posTamagoIdouXFlag = new bool[8] {
			true, true, true, true, true, true, true, true
		};
		private bool[] posTamagoIdouYFlag = new bool[8] {
			true, true, true, true, true, true, true, true
		};
		private int[] fukidashiTamagoWait = new int[8] {
			60, 60, 60, 60, 60, 60, 60, 60
		};
		// 作業中（自動移動をどのよういするかを思案中）
		private void ApplealPositionChangeMain(){
			if (appealTimeCounter == 0) {
				for (int i = 0; i < 8; i++) {
					posTamagoSpeedX [i] = Random.Range (0.01f, 0.03f);
					posTamagoSpeedY [i] = Random.Range (0.01f, 0.03f);
					if (i < 4) {
						posTamagoIdouXFlag [i] = true;
					} else {
						posTamagoIdouXFlag [i] = false;
					}
					if (Random.Range (0, 2) == 0) {
						posTamagoIdouYFlag [i] = true;
					} else {
						posTamagoIdouYFlag [i] = false;
					}

					fukidashiTamagoWait [i] = Random.Range (60, 100);
				}
			}
			AppealPositionChangeLoop ();
			// １→２、２↓４、３↑１、４←３

			for (int i = 0; i < 8; i++) {
				fukidashiTamagoWait [i]--;
				if (fukidashiTamagoWait [i] == 0) {
					fukidashiTamagoWait [i] = Random.Range (60, 100);
					switch (Random.Range (0, 8)) {
					case	0:
						{	// 吹き出しケーキを表示
							CharaTamago [i].transform.Find ("fukidashi/cake").gameObject.SetActive (true);
							CharaTamago [i].transform.Find ("fukidashi/heart").gameObject.SetActive (false);
							break;
						}
					case	1:
						{	// 吹き出しハートを表示
							CharaTamago [i].transform.Find ("fukidashi/cake").gameObject.SetActive (false);
							CharaTamago [i].transform.Find ("fukidashi/heart").gameObject.SetActive (true);
							break;
						}
					default:
						{	// 何も表示しない
							CharaTamago [i].transform.Find ("fukidashi/cake").gameObject.SetActive (false);
							CharaTamago [i].transform.Find ("fukidashi/heart").gameObject.SetActive (false);
							break;
						}
					}
				}
			}

#if false
			for (int i = 0; i < 8; i++) {
				if (appealTimeCounter == 80) {
					tamagoChara [i].gotoAndPlay ("walk");
				}


				if (posTamagoIdouXFlag [i]) {
					posTamago [i].x += posTamagoSpeedX [i];
					CharaTamago [i].GetComponent<SpriteRenderer> ().flipX = posTamagoIdouXFlag [i];
					if (posTamago [i].x >= 7.0f) {
						posTamagoIdouXFlag [i] = false;
						posTamagoSpeedX [i] = Random.Range (0.01f, 0.03f);
					}
				} else {
					posTamago [i].x -= posTamagoSpeedX [i];
					CharaTamago [i].GetComponent<SpriteRenderer> ().flipX = posTamagoIdouXFlag [i];
					if (posTamago [i].x <= -7.0f) {
						posTamagoIdouXFlag [i] = true;
						posTamagoSpeedX [i] = Random.Range (0.01f, 0.03f);
					}
				}

				if (posTamagoIdouYFlag [i]) {
					posTamago [i].y += posTamagoSpeedY [i];
					if (posTamago [i].y >= 1.0f) {
						posTamagoIdouYFlag [i] = false;
						posTamagoSpeedY [i] = Random.Range (0.01f, 0.03f);
					}
				} else {
					posTamago [i].y -= posTamagoSpeedY [i];
					if (posTamago [i].y <= -3.5f) {
						posTamagoIdouYFlag [i] = true;
						posTamagoSpeedY [i] = Random.Range (0.01f, 0.03f);
					}
				}
			}
#endif
			appealTimeCounter++;
		}

		private void TamagochiFukidashiOff(){
			for (int i = 0; i < 8; i++) {
				CharaTamago [i].transform.Find ("fukidashi/cake").gameObject.SetActive (false);			// 吹き出しケーキを消す
				CharaTamago [i].transform.Find ("fukidashi/heart").gameObject.SetActive (false);		// 吹き出しハートを消す
			}
		}

		// 告白タイムの初期配置
		private void KokuhakuPositionInit(){
			float[,] pos = new float[8, 2] {
				{  4.0f,  1.0f },									// 男の子１の初期位置
				{  4.5f, -0.5f },									// 男の子２の初期位置
				{  5.0f, -2.0f },									// 男の子３の初期位置
				{  5.5f, -3.5f },									// 男の子４の初期位置
				{ -4.0f,  1.0f },									// 女の子１の初期位置
				{ -4.5f, -0.5f },									// 女の子２の初期位置
				{ -5.0f, -2.0f },									// 女の子３の初期位置
				{ -5.5f, -3.5f },									// 女の子４の初期位置
			};

			bool flag = true;

			for (int i = 0; i < 8; i++) {
				switch (i) {
				case	0:
				case	1:
				case	2:
				case	3:
					{
						flag = false;
						break;
					}
				case	4:
				case	5:
				case	6:
				case	7:
					{
						flag = true;
						break;
					}
				}
				CharaTamago [i].GetComponent<SpriteRenderer> ().flipX = flag;

				posTamago [i].x = pos [i, 0];
				posTamago [i].y = pos [i, 1];

				cbTamagoChara [i].gotoAndPlay ("idle");
			}

			CharaTamago [0].GetComponent<SpriteRenderer> ().sortingOrder = 1;
			CharaTamago [1].GetComponent<SpriteRenderer> ().sortingOrder = 2;
			CharaTamago [2].GetComponent<SpriteRenderer> ().sortingOrder = 3;
			CharaTamago [3].GetComponent<SpriteRenderer> ().sortingOrder = 4;
			CharaTamago [4].GetComponent<SpriteRenderer> ().sortingOrder = 1;
			CharaTamago [5].GetComponent<SpriteRenderer> ().sortingOrder = 2;
			CharaTamago [6].GetComponent<SpriteRenderer> ().sortingOrder = 3;
			CharaTamago [7].GetComponent<SpriteRenderer> ().sortingOrder = 4;
		}
	
		// カーテンのY座標を上下させる
		// num:移動速度（マイナス:クローズ、プラス:オープン）
		private bool EventCurtainPositionChange(float num){
			Vector3 pos;

			pos = EventCurtain.transform.localPosition;
			pos.y += num;
			EventCurtain.transform.localPosition = pos;

			if (num > 0) {
				if (EventCurtain.transform.localPosition.y >= 800.0f) {
					return true;
				}
			} else {
				if (EventCurtain.transform.localPosition.y <= 0.0f) {
					return true;
				}
			}
			return false;
		}

		// 相談対象相手を算出する
		private void TargetRandomSet(){
			switch (playerNumber) {
			case	0:
			case	1:
			case	2:
			case	3:
				{
					// 自分が男の子なので女の子の中から選ぶ
					targetNumber = Random.Range (0, 4) + 4;
					break;
				}
			case	4:
			case	5:
			case	6:
			case	7:
				{
					// 自分が女の子なので男の子の中から選ぶ
					targetNumber = Random.Range (0, 4);
					break;
				}
			}
		}

		// flag:表示フラグ（true:表示、false:非表示）
		private void EventSoudanPartsDisp (bool flag){
			EventSoudanYesNew.SetActive (flag);
			EventSoudanNoNew.SetActive (flag);

			EventSoudanAvaterNameNew.SetActive (flag);
			EventSoudanTamagoNameNew.SetActive (flag);

			EventSoudanKumo.SetActive (flag);
		}

		private void EventSoudanPartsDispOff (){
			EventSoudanChara.transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
			EventSoudanTarget.transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
			EventSoudanChara.GetComponent<SpriteRenderer> ().sortingOrder = 0;

			EventSoudanAvaterNameOld.transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
			EventSoudanTamagoNameOld.transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);

			EventSoudanYesOld.transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
			EventSoudanNoOld.transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
		}

		private void EventSoudanNameSet (){
			string avaterMes = "";
			string tamagoMes = "";

			switch (targetNumber) {
			case	0:
				{
					avaterMes = "アバターネーム1";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	1:
				{
					avaterMes = "アバターネーム2";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	2:
				{
					avaterMes = "アバターネーム3";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	3:
				{
					avaterMes = "アバターネーム4";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	4:
				{
					avaterMes = "アバターネーム5";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	5:
				{
					avaterMes = "アバターネーム6";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	6:
				{
					avaterMes = "アバターネーム7";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	7:
				{
					avaterMes = "アバターネーム8";
					tamagoMes = "たまごっちネーム";
					break;
				}
			}

			EventSoudanAvaterNameNew.transform.Find ("text").gameObject.GetComponent<Text> ().text = avaterMes;
			EventSoudanTamagoNameNew.transform.Find ("text").gameObject.GetComponent<Text> ().text = tamagoMes;
		}



		// 告白する相手の番号
		private int[] KokuhakuManToWomanTable = new int[4] { 0, 1, 2, 3 };
			
		private void KokuhakuTimeInit(){
			KokuhakuManToWomanTable[0] = KokuhakuTimeObjectSelect (0);		// 男の子１の告白する相手の番号を登録
			KokuhakuManToWomanTable[1] = KokuhakuTimeObjectSelect (1);		// 男の子２の告白する相手の番号を登録
			KokuhakuManToWomanTable[2] = KokuhakuTimeObjectSelect (2);		// 男の子３の告白する相手の番号を登録
			KokuhakuManToWomanTable[3] = KokuhakuTimeObjectSelect (3);		// 男の子４の告白する相手の番号を登録

			kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount000;
		}

		private int kokuhakuWaitTime = 0;
		private int[] kokuhakuManTable = new int[4]{ 0, 1, 2, 3 };			// 告白する男の子の番号テーブル（終了したら２５５になる）
		private int kokuhakuManNumber = 0;									// 現在告白している男の子のテーブル番号
		private bool kokuhakuOkFlag = false;
		private int kokuhakuRivalNumber = 0;								// ライバルが出現するかどうか（16進数で考えて２番目がライバル10h、３番目がライバル20h、４番目がライバル40h）
		private float heartSize = 0.0f;										// 告白の成否判定の時に表示するハートのサイズ
		// 告白処理ループ
		private void KokuhakuTimeMain(){
			switch (kokuhakuTimeLoopCount) {
			case	statusKokuhakuCount.kokuhakuCount000:
				{
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount010;
					kokuhakuManTable [0] = 0;
					kokuhakuManTable [1] = 1;
					kokuhakuManTable [2] = 2;
					kokuhakuManTable [3] = 3;
					kokuhakuManNumber = 0;
					kokuhakuOkFlag = false;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount010:
				{
					if (kokuhakuManTable [kokuhakuManNumber] == 255) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount340;			// ライバル出現したので通常告白はなし
						break;
					}
						
					if (KokuhakuTimeIdou1 (kokuhakuManTable[kokuhakuManNumber])) {				// 男の子は一歩前に移動する
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount020;
						tamagochiPatanChenge (0, kokuhakuManTable[kokuhakuManNumber]);			// 告白者のアニメパターンを登録
						FukidashiMessageSetKokuhaku (kokuhakuManTable[kokuhakuManNumber], KokuhakuManToWomanTable [kokuhakuManTable[kokuhakuManNumber]] + 4);
						fukidashi [0].SetActive (true);
						kokuhakuWaitTime = 120;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount020:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount030;
						fukidashi [0].SetActive (false);
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount030:
				{
					if (KokuhakuTimeIdou1 (KokuhakuManToWomanTable [kokuhakuManTable[kokuhakuManNumber]] + 4)) {		// 告白指定された女の子は一歩前に移動する
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount040;
						tamagochiPatanChenge (1, KokuhakuManToWomanTable [kokuhakuManTable[kokuhakuManNumber]] + 4);	// 告白対象者のアニメパターンを登録
						kokuhakuWaitTime = 120;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount040:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuRivalNumber = KokuhakuRivalCheck ();							// ライバル出現チェック
						if (kokuhakuRivalNumber == 0) {
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount140;		// ライバル出現なし
						} else {
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount050;		// ライバル出現あり
							cbTamagoChara [kokuhakuManNumber].gotoAndPlay ("shock");
						}
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount050:
				{
					if ((kokuhakuRivalNumber & 16) != 0) {
						KokuhakuRivalWaitStop (1, true);										// ちょっと待ったを表示（初期化込み）
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount060;			// ２番目のたまごっちがライバルとして行動
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount080;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount060:
				{
					KokuhakuRivalWaitStop (1, false);											// ちょっと待ったを表示
					if (KokuhakuWaitTimeSubLoop ()) {
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
						EventWaitStop.SetActive (false);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount070;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount070:
				{
					if (KokuhakuTimeIdou1 (1)) {												// ２番目のたまごっち、ライバルの男の子は一歩前に
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount080;
						tamagochiPatanChenge (0, 1);											// 告白者のアニメパターンを登録
					}		
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount080:
				{
					if ((kokuhakuRivalNumber & 32) != 0) {
						KokuhakuRivalWaitStop (2, true);										// ちょっと待ったを表示（初期化込み）
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount090;			// ３番目のたまごっちがライバルとして行動
						if ((kokuhakuRivalNumber & 16) != 0) {
							cbTamagoChara [1].gotoAndPlay ("shock");
						}
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount110;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount090:
				{
					KokuhakuRivalWaitStop (2, false);											// ちょっと待ったを表示
					if (KokuhakuWaitTimeSubLoop ()) {
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
						EventWaitStop.SetActive (false);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount100;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount100:
				{
					if (KokuhakuTimeIdou1 (2)) {												// ３番目のたまごっち、ライバルの男の子は一歩前に
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount110;
						tamagochiPatanChenge (0, 2);											// 告白者のアニメパターンを登録
					}		
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount110:
				{
					if ((kokuhakuRivalNumber & 64) != 0) {
						KokuhakuRivalWaitStop (3, true);										// ちょっと待ったを表示（初期化込み）
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount120;			// ４番目のたまごっちがライバルとして行動
						if ((kokuhakuRivalNumber & 16) != 0) {
							cbTamagoChara [1].gotoAndPlay ("shock");
						}
						if ((kokuhakuRivalNumber & 32) != 0) {
							cbTamagoChara [2].gotoAndPlay ("shock");
						}
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount140;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount120:
				{
					KokuhakuRivalWaitStop (3, false);											// ちょっと待ったを表示
					if (KokuhakuWaitTimeSubLoop ()) {
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
						EventWaitStop.SetActive (false);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount130;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount130:
				{
					if (KokuhakuTimeIdou1 (3)) {												// ４番目のたまごっち、ライバルの男の子は一歩前に
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount140;
						tamagochiPatanChenge (0, 3);											// 告白者のアニメパターンを登録
					}		
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount140:
				{
					kokuhakuWaitTime = 60;
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount150;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount150:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount160;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount160:
				{
					if (KokuhakuTimeIdou2 (kokuhakuManTable[kokuhakuManNumber])) {				// 告白者をもう一歩前に
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount170;
						KokuhakuheartDisp (kokuhakuManTable [kokuhakuManNumber], true);			// 吹き出しハートを表示
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount170:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						KokuhakuheartDisp(kokuhakuManTable [kokuhakuManNumber],false);			// 吹き出しハートを非表示
						if (kokuhakuRivalNumber != 0) {
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount180;		// ライバル出現あり
						} else {
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount270;		// ライバル出現なし
						}
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount180:
				{
					if ((kokuhakuRivalNumber & 16) != 0) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount190;			// ２番目のたまごっちがライバル登録されている
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount210;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount190:
				{
					if (KokuhakuTimeIdou2 (1)) {												// ２番目のたまごっち、もう一歩前に
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount200;
						KokuhakuheartDisp (1, true);											// 吹き出しハートを表示
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount200:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount210;
						KokuhakuheartDisp (1, false);											// 吹き出しハートを非表示
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount210:
				{
					if ((kokuhakuRivalNumber & 32) != 0) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount220;			// ３番目のたまごっちがライバル登録されている
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount240;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount220:
				{
					if (KokuhakuTimeIdou2 (2)) {												// ３番目のたまごっち、もう一歩前に
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount230;
						KokuhakuheartDisp (2, true);											// 吹き出しハートを表示
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount230:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount240;
						KokuhakuheartDisp (2, false);											// 吹き出しハートを非表示
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount240:
				{
					if ((kokuhakuRivalNumber & 64) != 0) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount250;			// ４番目のたまごっちがライバル登録されている
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount270;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount250:
				{
					if (KokuhakuTimeIdou2 (3)) {												// ４番目のたまごっち、もう一歩前に
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount260;
						KokuhakuheartDisp (3, true);											// 吹き出しハートを表示
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount260:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount270;
						KokuhakuheartDisp (3, false);											// 吹き出しハートを非表示
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount270:
				{
					kokuhakuWaitTime = 30;
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount280;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount280:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						loveParamCheck ();														// 告白判定
						loveResultDisp (true);													// 告白結果のハートなどを表示

						FukidashiMessageKokuhakuReturn (KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]], loveParamFlag);		// 告白の返事を表示
						fukidashi [0].SetActive (true);
						
						heartSize = 0.0f;
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount290;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount290:
				{
					if(loveResultDispHeart()){													// 告白結果のハートを拡大表示
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount300;
						kokuhakuWaitTime = 120;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount300:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						loveResultDisp (false);
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
						fukidashi [0].SetActive (false);
						CharaTamago [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].GetComponent<SpriteRenderer> ().flipX = false;
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount310;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount310:
				{
					if (KokuhakuTimeIdou3 (KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4)) {	// 女の子は定位置に戻る
						CharaTamago [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].GetComponent<SpriteRenderer> ().flipX = true;

						if (loveParamFlag) {
							tamagochiPatanChenge (3, KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4);

							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount320;
							KokuhakuTimeIdou4Init (KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4);
							cbTamagoChara [loveParamManNumber].gotoAndPlay ("walk");
						} else {
							cbTamagoChara [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].gotoAndPlay ("idle");
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount330;
						}
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount320:
				{
					if (KokuhakuTimeIdou4 ()) {													// 告白成功したので女の子の前に男の子は移動する
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount330;
						tamagochiPatanChenge (3, loveParamManNumber);

						kokuhakuOkFlag = true;													// 告白成功者がいる
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount330:
				{
					if (KokuhakuTimeIdou5 ()) {													// 告白失敗者は定位置に戻る
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount340;
					}
					break;
				}

			case	statusKokuhakuCount.kokuhakuCount340:
				{
					kokuhakuManTable [kokuhakuManNumber] = 255;									// 告白済みにする
					if ((kokuhakuRivalNumber & 16) != 0) {
						kokuhakuManTable [1] = 255;												// 男の子２はライバルで出現したので告白済みにする
					}
					if ((kokuhakuRivalNumber & 32) != 0) {
						kokuhakuManTable [2] = 255;												// 男の子３はライバルで出現したので告白済みにする
					}
					if ((kokuhakuRivalNumber & 64) != 0) {
						kokuhakuManTable [3] = 255;												// 男の子４はライバルで出現したので告白済みにする
					}

					kokuhakuManNumber++;
					if (kokuhakuManNumber == 4) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount350;
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount010;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount350:
				{
					if (kokuhakuOkFlag) {														// 告白成功者がいる
						EventLastResult.transform.Find ("yes").gameObject.SetActive (true);
						EventLastResult.transform.Find ("yes/panel").gameObject.SetActive (true);
						EventLastResult.transform.Find ("yes/panel2").gameObject.SetActive (false);
					} else {																	// 告白成功者がいない
						EventLastResult.transform.Find ("no").gameObject.SetActive (true);
						EventLastResult.transform.Find ("no/panel").gameObject.SetActive (true);
						EventLastResult.transform.Find ("no/panel2").gameObject.SetActive (false);
					}
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount360;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount360:
				{
					Vector3 pos = new Vector3 (1.0f, 1.0f, 1.0f);
					if (kokuhakuOkFlag) {														// 告白成功者がいる
						pos = EventLastResult.transform.Find ("yes/panel").gameObject.transform.localPosition;
					} else {																	// 告白成功者がいない
						pos = EventLastResult.transform.Find ("no/panel").gameObject.transform.localPosition;
					}
					pos.y += 0.5f;
					if (pos.y >= -348.0f) {
						pos.y = -348.0f;
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount370;
					}

					if (kokuhakuOkFlag) {														// 告白成功者がいる
						EventLastResult.transform.Find ("yes/panel").gameObject.transform.localPosition = pos;
					} else {																	// 告白成功者がいない
						EventLastResult.transform.Find ("no/panel").gameObject.transform.localPosition = pos;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount370:
				{
					if (kokuhakuOkFlag) {														// 告白成功者がいる
						EventLastResult.transform.Find ("yes/panel").gameObject.SetActive (false);
						EventLastResult.transform.Find ("yes/panel2").gameObject.SetActive (true);
					} else {																	// 告白成功者がいない
						EventLastResult.transform.Find ("no/panel").gameObject.SetActive (false);
						EventLastResult.transform.Find ("no/panel2").gameObject.SetActive (true);
					}

					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount380;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount380:
				{
					Vector3 pos = new Vector3 (1.0f, 1.0f, 1.0f);
					if (kokuhakuOkFlag) {														// 告白成功者がいる
						pos = EventLastResult.transform.Find ("yes/panel2").gameObject.transform.localPosition;
					} else {																	// 告白成功者がいない
						pos = EventLastResult.transform.Find ("no/panel2").gameObject.transform.localPosition;
					}
					pos.y += 0.5f;
					if (pos.y >= -325.0f) {
						pos.y = -325.0f;
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount390;
					}

					if (kokuhakuOkFlag) {														// 告白成功者がいる
						EventLastResult.transform.Find ("yes/panel2").gameObject.transform.localPosition = pos;
					} else {																	// 告白成功者がいない
						EventLastResult.transform.Find ("no/panel2").gameObject.transform.localPosition = pos;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount390:
				{
					if (kokuhakuOkFlag) {														// 告白成功者がいる
						EventLastResult.transform.Find ("yes").gameObject.SetActive (false);
					} else {																	// 告白成功者がいない
						EventLastResult.transform.Find ("no").gameObject.SetActive (false);
					}
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount400;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount400:
				{
					kokuhakuTimeEndFlag = true;													// 告白タイム終了
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount410:
				{
					break;
				}
			}
		}
		private bool KokuhakuWaitTimeSubLoop(){
			kokuhakuWaitTime--;
			if (kokuhakuWaitTime == 0) {
				return true;
			}
			return false;
		}

		// num:男の子の番号（０〜３）、flag:表示フラグ（true:表示、false:非表示）
		private void KokuhakuheartDisp(int num,bool flag){
			CharaTamago [num].transform.Find ("fukidashi/heart").gameObject.SetActive (flag);	// 吹き出しハートを表示・非表示
			if (flag) {
				tamagochiPatanChenge (0, num);													// 告白者のアニメパターンを登録
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp11);
				kokuhakuWaitTime = 120;
			} else {
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
			}
		}

		// num:たまごっちの番号（０〜７）
		private bool KokuhakuTimeIdou1(int num){
			float[,] posTable = new float[8,4] {
				{  4.0f,  2.0f, -0.05f, 0f },
				{  4.5f,  2.5f, -0.05f, 0f },
				{  5.0f,  3.0f, -0.05f, 0f },
				{  5.5f,  3.5f, -0.05f, 0f },
				{ -4.0f, -2.0f,  0.05f, 1f },
				{ -4.5f, -2.5f,  0.05f, 1f },
				{ -5.0f, -3.0f,  0.05f, 1f },
				{ -5.5f, -3.5f,  0.05f, 1f },
			};

			if (posTable [num, 3] == 0) {
				if (posTamago [num].x <= posTable [num, 1]) {
					posTamago [num].x = posTable [num, 1];
					return	true;
				}
			} else {
				if (posTamago [num].x >= posTable [num, 1]) {
					posTamago [num].x = posTable [num, 1];
					return	true;
				}
			}
			if (posTamago [num].x == posTable [num, 0]) {
				cbTamagoChara [num].gotoAndPlay ("walk");
			}
			posTamago [num].x += posTable [num, 2];
			return false;
		}
		// num:たまごっち（男の子）の番号（０〜３）
		private bool KokuhakuTimeIdou2(int num){
			float[,] posTable = new float[4,4] {
				{  2.0f,  1.0f, -0.05f, 0f },
				{  2.5f,  1.5f, -0.05f, 0f },
				{  3.0f,  2.0f, -0.05f, 0f },
				{  3.5f,  2.5f, -0.05f, 0f },
			};

			if (posTable [num, 3] == 0) {
				if (posTamago [num].x <= posTable [num, 1]) {
					posTamago [num].x = posTable [num, 1];
					return	true;
				}
			} else {
				if (posTamago [num].x >= posTable [num, 1]) {
					posTamago [num].x = posTable [num, 1];
					return	true;
				}
			}
			if (posTamago [num].x == posTable [num, 0]) {
				cbTamagoChara [num].gotoAndPlay ("walk");
			}
			posTamago [num].x += posTable [num, 2];
			return false;
		}
		// num:たまごっち（女の子）の番号（４〜７）
		private bool KokuhakuTimeIdou3(int num){
			float[,] posTable = new float[8,4] {
				{  0.0f,  0.0f,  0.00f, 0f },
				{  0.0f,  0.0f,  0.00f, 0f },
				{  0.0f,  0.0f,  0.00f, 0f },
				{  0.0f,  0.0f,  0.00f, 0f },
				{ -2.0f, -4.0f, -0.05f, 0f },
				{ -2.5f, -4.5f, -0.05f, 0f },
				{ -3.0f, -5.0f, -0.05f, 0f },
				{ -3.5f, -5.5f, -0.05f, 0f },
			};

			if (posTable [num, 3] == 0) {
				if (posTamago [num].x <= posTable [num, 1]) {
					posTamago [num].x = posTable [num, 1];
					return	true;
				}
			} else {
				if (posTamago [num].x >= posTable [num, 1]) {
					posTamago [num].x = posTable [num, 1];
					return	true;
				}
			}
			if (posTamago [num].x == posTable [num, 0]) {
				cbTamagoChara [num].gotoAndPlay ("walk");
			}
			posTamago [num].x += posTable [num, 2];
			return false;
		}

		private float idou4SpeedX = 0.0f;
		private float idou4SpeedY = 0.0f;
		private float idou4GoalX = 0.0f;
		private float idou4GoalY = 0.0f;
		// num:たまごっち（女の子）の番号（０〜３）
		private void KokuhakuTimeIdou4Init(int num){
			idou4GoalX = posTamago [num].x + 1.5f;												// 女の子の右隣を目標地点にする
			idou4GoalY = posTamago [num].y;

			idou4SpeedX = (idou4GoalX - posTamago [loveParamManNumber].x) / 60.0f;
			idou4SpeedY = (idou4GoalY - posTamago [loveParamManNumber].y) / 60.0f;

			CharaTamago [loveParamManNumber].GetComponent<SpriteRenderer> ().sortingOrder = CharaTamago [num].GetComponent<SpriteRenderer> ().sortingOrder;

			if ((playerNumber == num) || (playerNumber == loveParamManNumber)) {
				playerResultFlag = true;
			}
		}
		private bool KokuhakuTimeIdou4(){
			posTamago [loveParamManNumber].x += idou4SpeedX;
			posTamago [loveParamManNumber].y += idou4SpeedY;
			if (posTamago [loveParamManNumber].x <= idou4GoalX) {
				posTamago [loveParamManNumber].x = idou4GoalX;
				posTamago [loveParamManNumber].y = idou4GoalY;
				return true;
			}

			return false;
		}
		private bool KokuhakuTimeIdou5(){
			float[] posTable = new float[4] {
				4.0f,
				4.5f,
				5.0f,
				5.5f,
			};
			bool retFlag = false;
			int retNum = 0;
			for (int i = 0; i < 4; i++) {
				if (loveParamManCryFlag [i]) {
					posTamago [i].x += 0.10f;
					if (posTamago [i].x >= posTable [i]) {
						posTamago [i].x = posTable [i];
						retFlag = true;
					}
				} else {
					retNum++;
				}
			}
			if (retNum == 4) {
				retFlag = true;
			}

			return retFlag;
		}

		private void KokuhakuEnd(){
		}

		// 告白する相手を選択する
		// manNum:男の子の番号（０〜３）
		private int KokuhakuTimeObjectSelect(int manNum){
			
			int womanNum = 0;

			womanNum = KokuhakuTimeObjectSelectSub (manNum, 0, 1);								// 告白する相手を１番目と２番目で比較

			if (womanNum == 0) {
				womanNum = KokuhakuTimeObjectSelectSub (manNum, 0, 2);							// 告白する相手を１番目と３番目で比較
			} else {
				womanNum = KokuhakuTimeObjectSelectSub (manNum, 1, 2);							// 告白する相手を２番目と３番目で比較
			}

			switch (womanNum) {
			case	0:
				{
					womanNum = KokuhakuTimeObjectSelectSub (manNum, 0, 3);						// 告白する相手を１番目と４番目で比較
					break;
				}
			case	1:
				{
					womanNum = KokuhakuTimeObjectSelectSub (manNum, 1, 3);						// 告白する相手を２番目と４番目で比較
					break;
				}
			case	2:
				{
					womanNum = KokuhakuTimeObjectSelectSub (manNum, 2, 3);						// 告白する相手を３番目と４番目で比較
					break;
				}
			}

			return womanNum;
		}
		// manNum:男の子の番号（０〜３）、womanNum1:女の子の番号（０〜２）、womanNum2:女の子の番号（１〜３）
		private int KokuhakuTimeObjectSelectSub(int manNum,int womanNum1,int womanNum2){
			int womanNum = 0;

			if (loveManWoman [manNum, womanNum1] <= loveManWoman [manNum, womanNum2]) {
				if (loveManWoman [manNum, womanNum1] == loveManWoman [manNum, womanNum2]) {
					if (Random.Range (0, 2) == 0) {												// num1とnum2の相性度が同じ場合ランダムで対象を決定
						womanNum = womanNum1;
					} else {
						womanNum = womanNum2;
					}
				} else {
					womanNum = womanNum2;														// num2の相性度が高い
				}
			} else {
				womanNum = womanNum1;															// num1の相性度が高い
			}

			return	womanNum;
		}

		// たまごっちキャラクターのランダムアニメ
		// patanNum:アニメ種類、tamagoNum:アニメさせるたまごっちの番号（０〜７）
		private void tamagochiPatanChenge(int patanNum,int tamagoNum){
			switch (patanNum) {
			case	0:
				{	// 告白者のアニメパターン
					switch (Random.Range (0, 3)) {
					case	0:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("grad2");					// 喜び２
							break;
						}
					case	1:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("shy1");						// 照れ１
							break;
						}
					case	2:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("shy3");						// 照れ３
							break;
						}
					}
					break;
				}
			case	1:
				{	// 告白対象者のアニメパターン
					switch (Random.Range (0, 2)) {
					case	0:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("shy1");						// 照れ１
							break;
						}
					case	1:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("shy2");						// 照れ２
							break;
						}
					}
					break;
				}
			case	2:
				{	// 告白成功時の告白者のアニメパターン
					switch (Random.Range (0, 3)) {
					case	0:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("grad1");					// 喜び１
							break;
						}
					case	1:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("grad2");					// 喜び２
							break;
						}
					case	2:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("grad3");					// 喜び３
							break;
						}
					}
					break;
				}
			case	3:
				{	// 告白成功時の男女のアニメパターン
					switch (Random.Range (0, 4)) {
					case	0:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("grad1");					// 喜び１
							break;
						}
					case	1:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("grad2");					// 喜び２
							break;
						}
					case	2:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("shy1");						// 照れ１
							break;
						}
					case	3:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("shy2");						// 照れ２
							break;
						}
					}
					break;
				}
			}
		}

		// ライバルキャラを抽出する
		private int KokuhakuRivalCheck(){
			int flag = 0;

			switch (kokuhakuManNumber) {
			case	0:
				{
					if (KokuhakuManToWomanTable [0] == KokuhakuManToWomanTable [1]) {
						flag += 16;																// 男の子２をライバルとする
					}
					if (KokuhakuManToWomanTable [0] == KokuhakuManToWomanTable [2]) {
						flag += 32;																// 男の子３をライバルとする
					}
					if (KokuhakuManToWomanTable [0] == KokuhakuManToWomanTable [3]) {
						flag += 64;																// 男の子４をライバルとする
					}
					break;
				}
			case	1:
				{
					if (KokuhakuManToWomanTable [1] == KokuhakuManToWomanTable [2]) {
						flag += 32;																// 男の子３をライバルとする
					}
					if (KokuhakuManToWomanTable [1] == KokuhakuManToWomanTable [3]) {
						flag += 64;																// 男の子４をライバルとする
					}
					break;
				}
			case	2:
				{
					if (KokuhakuManToWomanTable [2] == KokuhakuManToWomanTable [3]) {
						flag += 64;																// 男の子４をライバルとする
					}
					break;
				}
			}

			return	flag;
		}

		// ちょっとまったを表示する。（たまごっちのスプライトをちょっと待ったの帯たまごっちに登録する）
		// num:男の子の番号（０〜３）、flag:初期化フラグ（true:初期化あり、false:初期化なし）
		private void KokuhakuRivalWaitStop(int num,bool flag){
			if (flag) {
				EventWaitStop.SetActive (true);													// ちょっと待ったの帯を表示
				cbTamagoChara [num].gotoAndPlay ("anger");
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp12);
				kokuhakuWaitTime = 120;
			}
			EventWaitStop.transform.Find ("panel/image").gameObject.GetComponent<Image> ().sprite = CharaTamago [num].GetComponent<SpriteRenderer> ().sprite;
		}

		// 告白判定
		private bool loveParamFlag = true;
		private int loveParamManNumber = 0;
		private bool[] loveParamManCryFlag = new bool[4] { false, false, false, false };
		private void loveParamCheck(){
			for (int i = 0; i < 4; i++) {
				loveParamManCryFlag [i] = false;
			}

			if(loveManWomanFix[kokuhakuManTable [kokuhakuManNumber],KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]] <= loveManWoman[kokuhakuManTable [kokuhakuManNumber],KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]]){
				// 告白成功
				loveParamFlag = true;
				loveParamManNumber = kokuhakuManTable [kokuhakuManNumber];
				tamagochiPatanChenge (2, kokuhakuManTable [kokuhakuManNumber]);					// 告白成功時のアニメパターンを登録
			}
			else{
				// 告白失敗
				loveParamFlag = false;
				cbTamagoChara [kokuhakuManTable [kokuhakuManNumber]].gotoAndPlay ("cry");
				loveParamManCryFlag [kokuhakuManTable [kokuhakuManNumber]] = true;
			}
				
			if(kokuhakuRivalNumber != 0){
				if((kokuhakuRivalNumber & 16) != 0){
					if((loveManWomanFix[1,KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]] <= loveManWoman[1,KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]]) && (!loveParamFlag)){
						// 告白成功
						loveParamFlag = true;
						loveParamManNumber = 1;
						tamagochiPatanChenge (2, 1);											// 告白成功時のアニメパターンを登録
					}
					else{
						// 告白失敗
						cbTamagoChara [1].gotoAndPlay ("cry");
						loveParamManCryFlag [1] = true;
					}
				}
				if((kokuhakuRivalNumber & 32) != 0){
					if((loveManWomanFix[2,KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]] <= loveManWoman[2,KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]]) && (!loveParamFlag)){
						// 告白成功
						loveParamFlag = true;
						loveParamManNumber = 2;
						tamagochiPatanChenge (2, 2);											// 告白成功時のアニメパターンを登録
					}
					else{
						// 告白失敗
						cbTamagoChara [2].gotoAndPlay ("cry");
						loveParamManCryFlag [2] = true;
					}
				}
				if((kokuhakuRivalNumber & 64) != 0){
					if((loveManWomanFix[3,KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]] <= loveManWoman[3,KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]]) && (!loveParamFlag)){
						// 告白成功
						loveParamFlag = true;
						loveParamManNumber = 3;
						tamagochiPatanChenge (2, 3);											// 告白成功時のアニメパターンを登録
					}
					else{
						// 告白失敗
						cbTamagoChara [3].gotoAndPlay ("cry");
						loveParamManCryFlag [3] = true;
					}
				}
			}
		}
		// flag:表示フラグ（true:表示、false:非表示）
		private void loveResultDisp(bool flag){
			if (flag) {
				if (loveParamFlag) {
					// 告白成功
					EventResult.transform.Find ("yes").gameObject.SetActive (true);
				} else {
					// 告白失敗
					EventResult.transform.Find ("no").gameObject.SetActive (true);
				}
			} else {
				Vector3 pos = new Vector3 (0.0f, 0.0f, 0.0f);

				EventResult.transform.Find ("yes").gameObject.SetActive (false);
				EventResult.transform.Find ("yes/heart").gameObject.transform.localScale = pos;
				EventResult.transform.Find ("no").gameObject.SetActive (false);
				EventResult.transform.Find ("no/heart").gameObject.transform.localScale = pos;
			}
		}
		private bool loveResultDispHeart(){
			bool retFlag = false;
			Vector3 pos = new Vector3 (1.0f, 1.0f, 1.0f);

			heartSize += 0.05f;
			if (heartSize >= 2.0f) {
				heartSize = 2.0f;
				retFlag = true;
			}
			pos.x = heartSize;
			pos.y = heartSize;

			if (loveParamFlag) {
				EventResult.transform.Find ("yes/heart").gameObject.transform.localScale = pos;
			} else {
				EventResult.transform.Find ("no/heart").gameObject.transform.localScale = pos;
			}

			return retFlag;
		}



		// Yesボタンが押された時
		private void ButtneYesClick(){
			LoveManWomanNumberSet (10);		// 好感度を１０上げる
			buttonFlag = true;
			btnYesNo = true;
		}
		// Noボタンが押された時
		private void ButtonNoClick(){
			LoveManWomanNumberSet (-10);	// 好感度を１０下げる
			buttonFlag = true;
			btnYesNo = false;
		}

		// 好感度を上下させる（本当はサーバーでやるのが良いと思う）
		// num:好感度の上下値
		private void LoveManWomanNumberSet(int Num){
			int	manNumber = 0;
			int womanNumber = 0;

			switch (playerNumber) {
			case	0:
			case	1:
			case	2:
			case	3:
				{
					manNumber = playerNumber;
					break;
				}
			case	4:
			case	5:
			case	6:
			case	7:
				{
					womanNumber = playerNumber - 4;
					break;
				}
			}

			switch (targetNumber) {
			case	0:
			case	1:
			case	2:
			case	3:
				{
					manNumber = targetNumber;
					break;
				}
			case	4:
			case	5:
			case	6:
			case	7:
				{
					womanNumber = targetNumber - 4;
					break;
				}
			}
			loveManWoman [manNumber, womanNumber] += Num;
		}
	}
}
