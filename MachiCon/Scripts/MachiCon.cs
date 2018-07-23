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



		private object[]		mparam;

		private int playerNumber = 0;					// ０〜７（０〜３：男の子、４〜７：女の子）
		private int targetNumber = 0;					// ０〜７（０〜３：男の子、４〜７：女の子）
		private int targetNumber2 = 0;

		private bool buttonFlag = false;
		private bool btnYesNo = false;					// Yes:true No:false
		private int sceneNumber = 0;					// シーンナンバー

		private bool kokuhakuTimeEndFlag = true;

		private CharaBehaviour[] cbTamagoChara = new CharaBehaviour[8];


		// 相性度（０〜１００）（男の子、女の子）
		private int[,] loveManWoman = new int[4, 4]{ { 40,40,40,40 }, { 40,40,40,40 }, { 40,40,40,40 }, { 40,40,40,40 } };
		// 成否判定の種（０〜１００）（男の子、女の子）
		private int[,] loveManWomanFix = new int[4, 4];

		private enum loveNumber : int{
			loveNumber1 = 0,				// 一人目
			loveNumber2 = 1,				// 二人目
			loveNumber3 = 2,				// 三人目
			loveNumber4 = 3,				// 四人目
		}
			
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
			machiconJobCount360,
			machiconJobCount370,
			machiconJobCount380,
			machiconJobCount390,
			machiconJobCount400,
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

			playerNumber = Random.Range(0,8);

			EventSoudanYesNew.GetComponent<Button> ().onClick.AddListener (ButtneYesClick);
			EventSoudanNoNew.GetComponent<Button> ().onClick.AddListener (ButtonNoClick);

			jobCount = statusJobCount.machiconJobCount000;

			for (int i = 0; i < 8; i++) {
				cbTamagoChara [i] = CharaTamago [i].GetComponent<CharaBehaviour> ();
			}
			yield return cbTamagoChara[0].init (new TamaChara (16));
			yield return cbTamagoChara[1].init (new TamaChara (17));
			yield return cbTamagoChara[2].init (new TamaChara (16));
			yield return cbTamagoChara[3].init (new TamaChara (17));
			yield return cbTamagoChara[4].init (new TamaChara (18));
			yield return cbTamagoChara[5].init (new TamaChara (19));
			yield return cbTamagoChara[6].init (new TamaChara (18));
			yield return cbTamagoChara[7].init (new TamaChara (19));



		}
	
		void Destroy(){
			Debug.Log ("MachiCon Destroy");
		}

		private float	countTime1;
		private int		countTime2;
		private int		waitTime;
		private Vector3[] posTamago = new Vector3[8]{
			new Vector3(0.0f,-7.0f,0.0f),
			new Vector3(0.0f,-7.0f,0.0f),
			new Vector3(0.0f,-7.0f,0.0f),
			new Vector3(0.0f,-7.0f,0.0f),
			new Vector3(0.0f,-7.0f,0.0f),
			new Vector3(0.0f,-7.0f,0.0f),
			new Vector3(0.0f,-7.0f,0.0f),
			new Vector3(0.0f,-7.0f,0.0f),
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
					EventTitleText.GetComponent<Text> ().text = countTime2.ToString ();
					if (countTime2 == 0) {
						jobCount = statusJobCount.machiconJobCount020;

						EventTitle.GetComponent<Animator> ().SetBool ("title", true);
						waitTime = 90;
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
				{	// カーテンオープン
					EventCurtainPositionChange(10.0f);

					if (EventCurtain.transform.localPosition.y >= 800.0f) {
						EventTitle.SetActive (false);
						EventCurtain.SetActive (false);
						jobCount = statusJobCount.machiconJobCount040;
						posNumber = 0;
					}
					break;
				}
			case	statusJobCount.machiconJobCount040:
				{	// 入場開始
					FlameInMessageDisp ();					// 実況開始
					FlameInDirectionSet ();					// 入場時の向きを設定

					cbTamagoChara [posNumber].gotoAndPlay ("walk");
					jobCount = statusJobCount.machiconJobCount050;
					break;
				}
			case	statusJobCount.machiconJobCount050:
				{	// たまごっち入場
					float pos = FlameInPositionChange ();
					if (pos >= 1.0f) {
						jobCount = statusJobCount.machiconJobCount060;
						waitTime = 11;
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

						waitTime = 40;

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
				{	// カーテンクローズ
					EventCurtainPositionChange (-10.0f);
						
					if (EventCurtain.transform.localPosition.y <= 0.0f) {
						jobCount = statusJobCount.machiconJobCount110;

						EventPhase.SetActive (true);
						EventPhasePTStar.SetActive (true);
						EventPhaseCount.GetComponent<Image> ().sprite = EventPhaseSprite [sceneNumber];
//						EventPhase.transform.Find("phase_count").gameObject.GetComponent<Image>().sprite = EventPhaseSprite[sceneNumber];
						waitTime = 60;

						TargetRandomSet ();		// 相談相手の決定

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
						EventSoudanChara.transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventSoudanTarget.transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventSoudanChara.GetComponent<SpriteRenderer> ().sortingOrder = 0;
						EventSoudanNameSet ();			// 相談する時の相手の名前などを登録する
						EventSoudanOldDispOff ();

						for (int i = 0; i < 8; i++) {
							cbTamagoChara [i].gotoAndPlay ("idle");
						}

						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp07);
					}
					break;
				}
			case	statusJobCount.machiconJobCount150:
				{
					if (EventSoudan.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.machiconJobCount160;
						SoudanMessageDisp ();			// 相談タイトルメッセージ設定
						CharaTamago [targetNumber].GetComponent<SpriteRenderer> ().sortingLayerName = "Chara";	// 優先順位をカーテンの手前にする
						CharaTamago [targetNumber].GetComponent<SpriteRenderer> ().flipX = true;
						posTamago [targetNumber].x = 0.0f;
						posTamago [targetNumber].y = 1.5f;

						EventSoudanPartsDisp (true);
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
					posTamago [playerNumber].y -= 0.5f;				// たまごっちを下降させる
					if (posTamago [playerNumber].y <= -10.0f) {
						jobCount = statusJobCount.machiconJobCount180;
						CharaTamago [playerNumber].GetComponent<SpriteRenderer> ().sortingLayerName = "Default";	// 優先順位をデフォルトに戻す
						AppealPositionChangeInit ();							// アピールタイム初期表示位置設定
						TamagochiFukidashiOff ();
						waitTime = 90;
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
					EventCurtainPositionChange(10.0f);

					if (EventCurtain.transform.localPosition.y >= 800.0f) {
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
					ApplealPositionChangeMain ();			// アピールタイムのキャラ移動

					// 一定時間で実況表示を変える
					if ((waitTime == 200) || (waitTime == 400)) {
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp09);
					}
					break;
				}
			case	statusJobCount.machiconJobCount220:
				{
					EventCurtainPositionChange (-10.0f);

					if (EventCurtain.transform.localPosition.y <= 0.0f) {
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
						EventKokuhaku.SetActive (true);
						waitTime = 60;

						KokuhakuPositionInit ();
						KokuhakuTimeInit ();
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
					EventCurtainPositionChange (10.0f);
					if (EventCurtain.transform.localPosition.y >= 800.0f) {
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
						jobCount = statusJobCount.machiconJobCount270;
					}
					KokuhakuTimeMain ();
					break;
				}
			case	statusJobCount.machiconJobCount270:
				{
					break;
				}
			case	statusJobCount.machiconJobCount280:
				{
					break;
				}
			case	statusJobCount.machiconJobCount290:
				{
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
			case	statusJobCount.machiconJobCount360:
				{
					break;
				}
			case	statusJobCount.machiconJobCount370:
				{
					break;
				}
			case	statusJobCount.machiconJobCount380:
				{
					break;
				}
			case	statusJobCount.machiconJobCount390:
				{
					break;
				}
			case	statusJobCount.machiconJobCount400:
				{
					break;
				}
			}

			for (int i = 0; i < 8; i++) {
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
					mesAvater = "アバターネーム１";
					mesTamago = "たまごっちネーム語尾！";
					break;
				}
			case	1:
				{
					mesAvater = "アバターネーム２";
					mesTamago = "たまごっちネーム語尾！";
					break;
				}
			case	2:
				{
					mesAvater = "アバターネーム３";
					mesTamago = "たまごっちネーム語尾！";
					break;
				}
			case	3:
				{
					mesAvater = "アバターネーム４";
					mesTamago = "たまごっちネーム語尾！";
					break;
				}
			case	4:
				{
					mesAvater = "アバターネーム５";
					mesTamago = "たまごっちネーム語尾！";
					break;
				}
			case	5:
				{
					mesAvater = "アバターネーム６";
					mesTamago = "たまごっちネーム語尾！";
					break;
				}
			case	6:
				{
					mesAvater = "アバターネーム７";
					mesTamago = "たまごっちネーム語尾！";
					break;
				}
			case	7:
				{
					mesAvater = "アバターネーム８";
					mesTamago = "たまごっちネーム語尾！";
					break;
				}
			}

			fukidashi [0].transform.Find ("text").gameObject.GetComponent<Text> ().text = mesAvater + mesRet + mesTamago + mesRet;
		}
		private void FukidashiMessageSetKokuhaku(int num,int targetNum){
			float[,] fukidashiPos = new float[4, 2] {
				{ 125.0f,  125.0f },
				{ 155.0f,   35.0f },
				{ 185.0f,  -65.0f },
				{ 215.0f, -155.0f }
			};

			string mesAvater = "";
			string mesTamago = "";
			string mesRet = "\n";
			string mesNo = "の";
			string mesTo = "さん";

			switch (targetNum) {
			case	4:
				{
					mesAvater = "アバターネーム５";
					mesTamago = "たまごっちネーム";
					break;
				}
			case	5:
				{
					mesAvater = "アバターネーム６";
					mesTamago = "たまごっちネーム";
					break;
				}
			case	6:
				{
					mesAvater = "アバターネーム７";
					mesTamago = "たまごっちネーム";
					break;
				}
			case	7:
				{
					mesAvater = "アバターネーム８";
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

		// たまごっちが画面下から入場する時の移動
		private float FlameInPositionChange(){
			posTamago [posNumber].y += 0.05f;

			return posTamago[posNumber].y;
		}

		private void FlameInPositionChangeJump(){
			float[] posy = new float[] {
				1.0f,1.0f,1.2f,1.4f,1.6f,1.8f,2.0f,1.8f,1.6f,1.4f,1.2f,1.0f
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
				new Vector2(  1.7f / 40.0f, -4.0f / 40.0f ),
				new Vector2(  3.4f / 40.0f, -3.5f / 40.0f ),
				new Vector2(  5.1f / 40.0f, -3.0f / 40.0f ),
				new Vector2(  6.8f / 40.0f, -2.5f / 40.0f ),
				new Vector2( -1.7f / 40.0f, -4.0f / 40.0f ),
				new Vector2( -3.4f / 40.0f, -3.5f / 40.0f ),
				new Vector2( -5.1f / 40.0f, -3.0f / 40.0f ),
				new Vector2( -6.8f / 40.0f, -2.5f / 40.0f ),
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
				{  1.7f, -3.0f },
				{  3.4f, -2.5f },
				{  5.1f, -2.0f },
				{  6.8f, -1.5f },
				{ -1.7f, -3.0f },
				{ -3.4f, -2.5f },
				{ -5.1f, -2.0f },
				{ -6.8f, -1.5f },
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
		private int[] appealPosCharaTable = new int [8]{0,1,2,3,4,5,6,7};
		// 相談で気になる相手ならアピールタイムを一緒に過ごす
		private void AppealCharaPositionChange(){
			for (int i = 0; i < 8; i++) {
				appealPosCharaTable [i] = i;
			}

			AppealCharaCheckTargetNum ();

			for (int i = 0; i < 4; i++) {
				if (btnYesNo) {
					// targetが気になる。のでtargetとアピールタイムを過ごす
					if (playerNumber == i) {
						appealPosCharaTable [targetNumber - 4] = i;
						appealPosCharaTable [i] = targetNumber - 4;

					}
					if (targetNumber == i) {
						appealPosCharaTable [playerNumber - 4] = i;
						appealPosCharaTable [i] = playerNumber - 4;

					}
				} else {
					// targetはどうでも良い。のでtarget以外とアピールタイムを過ごす
					if (playerNumber == i) {
						appealPosCharaTable [targetNumber2 - 4] = i;
						appealPosCharaTable [i] = targetNumber2 - 4;

					}
					if (targetNumber2 == i) {
						appealPosCharaTable [playerNumber - 4] = i;
						appealPosCharaTable [i] = playerNumber - 4;
					}
				}
			}
		}

		private void AppealCharaCheckTargetNum(){
			int[,] numTbl = new int[8, 3] {
				{ 1, 2, 3 },
				{ 0, 2, 3 },
				{ 0, 1, 3 },
				{ 0, 1, 2 },
				{ 5, 6, 7 },
				{ 4, 6, 7 },
				{ 4, 5, 7 },
				{ 4, 5, 6 },
			};

			targetNumber2 = numTbl [targetNumber, Random.Range (0, 3)];
		}

		// アピールタイムの初期配置
		private void AppealPositionChangeInit(){
			float[,] pos = new float[8, 2] {
				{ -1.7f,  0.5f },
				{  4.5f,  0.5f },
				{ -3.5f, -3.0f },
				{  6.5f, -3.0f },
				{ -4.5f,  0.5f },
				{  1.7f,  0.5f },
				{ -6.5f, -3.0f },
				{  3.5f, -3.0f },
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
		private float [] posTamagoSpeedX = new float[8]{
			0.02f,0.02f,0.02f,0.02f,0.02f,0.02f,0.02f,0.02f
		};
		private float [] posTamagoSpeedY = new float[8]{
			0.02f,0.02f,0.02f,0.02f,0.02f,0.02f,0.02f,0.02f
		};
		private bool [] posTamagoIdouXFlag = new bool[8]{
			true,true,true,true,true,true,true,true
		};
		private bool [] posTamagoIdouYFlag = new bool[8]{
			true,true,true,true,true,true,true,true
		};

		private int [] fukidashiTamagoWait = new int[8] {
			60,60,60,60,60,60,60,60
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
						{
							CharaTamago [i].transform.Find ("fukidashi/cake").gameObject.SetActive (true);
							CharaTamago [i].transform.Find ("fukidashi/heart").gameObject.SetActive (false);
							break;
						}
					case	1:
						{
							CharaTamago [i].transform.Find ("fukidashi/cake").gameObject.SetActive (false);
							CharaTamago [i].transform.Find ("fukidashi/heart").gameObject.SetActive (true);
							break;
						}
					default:
						{
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
				CharaTamago [i].transform.Find ("fukidashi/cake").gameObject.SetActive (false);
				CharaTamago [i].transform.Find ("fukidashi/heart").gameObject.SetActive (false);
			}
		}

		// 告白タイムの初期配置
		private void KokuhakuPositionInit(){
			float[,] pos = new float[8, 2] {
				{  4.0f,  1.0f },
				{  4.5f, -0.5f },
				{  5.0f, -2.0f },
				{  5.5f, -3.5f },
				{ -4.0f,  1.0f },
				{ -4.5f, -0.5f },
				{ -5.0f, -2.0f },
				{ -5.5f, -3.5f },
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
		private void EventCurtainPositionChange(float num){
			Vector3 pos;

			pos = EventCurtain.transform.localPosition;
			pos.y += num;
			EventCurtain.transform.localPosition = pos;
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

		private void EventSoudanPartsDisp (bool flag){
			EventSoudanYesNew.SetActive (flag);
			EventSoudanNoNew.SetActive (flag);

			EventSoudanAvaterNameNew.SetActive (flag);
			EventSoudanTamagoNameNew.SetActive (flag);

			EventSoudanKumo.SetActive (flag);
		}

		private void EventSoudanOldDispOff (){
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
					avaterMes = "アバターネーム１";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	1:
				{
					avaterMes = "アバターネーム２";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	2:
				{
					avaterMes = "アバターネーム３";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	3:
				{
					avaterMes = "アバターネーム４";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	4:
				{
					avaterMes = "アバターネーム５";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	5:
				{
					avaterMes = "アバターネーム６";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	6:
				{
					avaterMes = "アバターネーム７";
					tamagoMes = "たまごっちネーム";
					break;
				}
			case	7:
				{
					avaterMes = "アバターネーム８";
					tamagoMes = "たまごっちネーム";
					break;
				}
			}

			EventSoudanAvaterNameNew.transform.Find ("text").gameObject.GetComponent<Text> ().text = avaterMes;
			EventSoudanTamagoNameNew.transform.Find ("text").gameObject.GetComponent<Text> ().text = tamagoMes;
		}





		private int [] KokuhakuManToWomanTable = new int[4] {
			0,1,2,3
		};

		private void KokuhakuTimeInit(){
			KokuhakuManToWomanTable[0] = KokuhakuTimeObjectSelect (0);
			KokuhakuManToWomanTable[1] = KokuhakuTimeObjectSelect (1);
			KokuhakuManToWomanTable[2] = KokuhakuTimeObjectSelect (2);
			KokuhakuManToWomanTable[3] = KokuhakuTimeObjectSelect (3);

			kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount000;
		}

		private int kokuhakuWaitTime = 0;
		private int[] kokuhakuManTable = new int[4]{ 0, 1, 2, 3 };
		private int kokuhakuManNumber = 0;
		private int kokuhakuRivalNumber = 0;

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
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount010:
				{
					if (kokuhakuManTable [kokuhakuManNumber] == 255) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount260;
						break;
					}

					if (KokuhakuTimeIdou1 (kokuhakuManTable[kokuhakuManNumber])) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount020;
						tamagochiPatanChenge (0, kokuhakuManTable[kokuhakuManNumber]);
						FukidashiMessageSetKokuhaku (kokuhakuManTable[kokuhakuManNumber], KokuhakuManToWomanTable [kokuhakuManTable[kokuhakuManNumber]] + 4);
						fukidashi [0].SetActive (true);
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp11);
						kokuhakuWaitTime = 120;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount020:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount030;
						fukidashi [0].SetActive (false);
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount030:
				{
					if (KokuhakuTimeIdou1 (KokuhakuManToWomanTable [kokuhakuManTable[kokuhakuManNumber]] + 4)) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount040;
						tamagochiPatanChenge (1, KokuhakuManToWomanTable [kokuhakuManTable[kokuhakuManNumber]] + 4);
						kokuhakuWaitTime = 120;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount040:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuRivalNumber = KokuhakuRivalCheck ();
						if (kokuhakuRivalNumber == 0) {
							// ライバル出現なし
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount140;
						} else {
							// ライバル出現あり
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount050;
							cbTamagoChara [kokuhakuManNumber].gotoAndPlay ("shock");
						}
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount050:
				{
					if ((kokuhakuRivalNumber & 16) != 0) {
						KokuhakuRivalWaitStop (1, true);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount060;
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount080;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount060:
				{
					KokuhakuRivalWaitStop (1, false);
					if (KokuhakuWaitTimeSubLoop ()) {
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
						EventWaitStop.SetActive (false);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount070;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount070:
				{
					if (KokuhakuTimeIdou1 (1)) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount080;
						tamagochiPatanChenge (0, 1);
					}		
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount080:
				{
					if ((kokuhakuRivalNumber & 32) != 0) {
						KokuhakuRivalWaitStop (2, true);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount090;
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
					KokuhakuRivalWaitStop (2, false);
					if (KokuhakuWaitTimeSubLoop ()) {
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
						EventWaitStop.SetActive (false);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount100;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount100:
				{
					if (KokuhakuTimeIdou1 (2)) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount110;
						tamagochiPatanChenge (0, 2);
					}		
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount110:
				{
					if ((kokuhakuRivalNumber & 64) != 0) {
						KokuhakuRivalWaitStop (3, true);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount120;
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
					KokuhakuRivalWaitStop (3, false);
					if (KokuhakuWaitTimeSubLoop ()) {
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
						EventWaitStop.SetActive (false);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount130;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount130:
				{
					if (KokuhakuTimeIdou1 (3)) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount140;
						tamagochiPatanChenge (0, 3);
					}		
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount140:
				{
					if (KokuhakuTimeIdou2 (kokuhakuManTable[kokuhakuManNumber])) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount150;
						tamagochiPatanChenge (0, kokuhakuManTable[kokuhakuManNumber]);
						CharaTamago [kokuhakuManTable[kokuhakuManNumber]].transform.Find ("fukidashi/heart").gameObject.SetActive (true);
						kokuhakuWaitTime = 60;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount150:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						CharaTamago [kokuhakuManTable [kokuhakuManNumber]].transform.Find ("fukidashi/heart").gameObject.SetActive (false);
						if (kokuhakuRivalNumber != 0) {
							// ライバル出現あり
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount160;
						} else {
							// ライバル出現なし
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount250;
						}
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount160:
				{
					if ((kokuhakuRivalNumber & 16) != 0) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount170;
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount190;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount170:
				{
					if (KokuhakuTimeIdou2 (1)) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount180;
						tamagochiPatanChenge (0, 1);
						CharaTamago [1].transform.Find ("fukidashi/heart").gameObject.SetActive (true);
						kokuhakuWaitTime = 60;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount180:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount190;
						CharaTamago [1].transform.Find ("fukidashi/heart").gameObject.SetActive (false);
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount190:
				{
					if ((kokuhakuRivalNumber & 32) != 0) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount200;
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount220;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount200:
				{
					if (KokuhakuTimeIdou2 (2)) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount210;
						tamagochiPatanChenge (0, 2);
						CharaTamago [2].transform.Find ("fukidashi/heart").gameObject.SetActive (true);
						kokuhakuWaitTime = 60;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount210:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount220;
						CharaTamago [2].transform.Find ("fukidashi/heart").gameObject.SetActive (false);
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount220:
				{
					if ((kokuhakuRivalNumber & 64) != 0) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount230;
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount250;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount230:
				{
					if (KokuhakuTimeIdou2 (3)) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount240;
						tamagochiPatanChenge (0, 3);
						CharaTamago [3].transform.Find ("fukidashi/heart").gameObject.SetActive (true);
						kokuhakuWaitTime = 60;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount240:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount250;
						CharaTamago [3].transform.Find ("fukidashi/heart").gameObject.SetActive (false);
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount250:
				{









					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount260;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount260:
				{
					kokuhakuManTable [kokuhakuManNumber] = 255;
					if ((kokuhakuRivalNumber & 16) != 0) {
						kokuhakuManTable [1] = 255;
					}
					if ((kokuhakuRivalNumber & 32) != 0) {
						kokuhakuManTable [2] = 255;
					}
					if ((kokuhakuRivalNumber & 64) != 0) {
						kokuhakuManTable [3] = 255;
					}


					kokuhakuManNumber++;
					if (kokuhakuManNumber == 4) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount270;
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount010;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount270:
				{
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount280:
				{
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount290:
				{
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount300:
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

		private bool KokuhakuTimeIdou1(int Number){
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

			if (posTable [Number, 3] == 0) {
				if (posTamago [Number].x <= posTable [Number, 1]) {
					posTamago [Number].x = posTable [Number, 1];
					return	true;
				}
			} else {
				if (posTamago [Number].x >= posTable [Number, 1]) {
					posTamago [Number].x = posTable [Number, 1];
					return	true;
				}
			}
			if (posTamago [Number].x == posTable [Number, 0]) {
				cbTamagoChara [Number].gotoAndPlay ("walk");
			}
			posTamago [Number].x += posTable [Number, 2];
			return false;
		}
		private bool KokuhakuTimeIdou2(int Number){
			float[,] posTable = new float[4,4] {
				{  2.0f,  1.0f, -0.05f, 0f },
				{  2.5f,  1.5f, -0.05f, 0f },
				{  3.0f,  2.0f, -0.05f, 0f },
				{  3.5f,  2.5f, -0.05f, 0f },
			};

			if (posTable [Number, 3] == 0) {
				if (posTamago [Number].x <= posTable [Number, 1]) {
					posTamago [Number].x = posTable [Number, 1];
					return	true;
				}
			} else {
				if (posTamago [Number].x >= posTable [Number, 1]) {
					posTamago [Number].x = posTable [Number, 1];
					return	true;
				}
			}
			if (posTamago [Number].x == posTable [Number, 0]) {
				cbTamagoChara [Number].gotoAndPlay ("walk");
			}
			posTamago [Number].x += posTable [Number, 2];
			return false;
		}

		private void KokuhakuEnd(){
		}

		private int KokuhakuTimeObjectSelect(int manNum){
			
			int womanNum = 0;

			womanNum = KokuhakuTimeObjectSelectSub (manNum, 0, 1);

			if (womanNum == 0) {
				womanNum = KokuhakuTimeObjectSelectSub (manNum, 0, 2);
			} else {
				womanNum = KokuhakuTimeObjectSelectSub (manNum, 1, 2);
			}

			switch (womanNum) {
			case	0:
				{
					womanNum = KokuhakuTimeObjectSelectSub (manNum, 0, 3);
					break;
				}
			case	1:
				{
					womanNum = KokuhakuTimeObjectSelectSub (manNum, 1, 3);
					break;
				}
			case	2:
				{
					womanNum = KokuhakuTimeObjectSelectSub (manNum, 2, 3);
					break;
				}
			}

			return womanNum;
		}
		
		private int KokuhakuTimeObjectSelectSub(int manNum1,int womanNum1,int womanNum2){
			int womanNum = 0;

			if(loveManWoman[manNum1,womanNum1] <= loveManWoman[manNum1,womanNum2]){
				if (loveManWoman [manNum1, womanNum1] == loveManWoman [manNum1, womanNum2]) {
					if (Random.Range (0, 2) == 0) {
						womanNum = womanNum1;
					} else {
						womanNum = womanNum2;
					}
				} else {
					womanNum = womanNum2;
				}
			}

			return	womanNum;
		}

		private void tamagochiPatanChenge(int patanNum,int tamagoNum){
			switch (patanNum) {
			case	0:
				{	// 告白者のアニメパターン
					switch (Random.Range (0, 3)) {
					case	0:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("grad2");	// 喜び２
							break;
						}
					case	1:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("shy1");		// 照れ１
							break;
						}
					case	2:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("shy3");		// 照れ３
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
							cbTamagoChara [tamagoNum].gotoAndPlay ("shy1");		// 照れ１
							break;
						}
					case	1:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("shy2");		// 照れ２
							break;
						}
					}
					break;
				}
			}
		}

		private int KokuhakuRivalCheck(){
			int flag = 0;


			switch (kokuhakuManNumber) {
			case	0:
				{
					if (KokuhakuManToWomanTable [0] == KokuhakuManToWomanTable [1]) {
						flag += 16;
					}
					if (KokuhakuManToWomanTable [0] == KokuhakuManToWomanTable [2]) {
						flag += 32;
					}
					if (KokuhakuManToWomanTable [0] == KokuhakuManToWomanTable [3]) {
						flag += 64;
					}
					break;
				}
			case	1:
				{
					if (KokuhakuManToWomanTable [1] == KokuhakuManToWomanTable [2]) {
						flag += 32;
					}
					if (KokuhakuManToWomanTable [1] == KokuhakuManToWomanTable [3]) {
						flag += 64;
					}
					break;
				}
			case	2:
				{
					if (KokuhakuManToWomanTable [2] == KokuhakuManToWomanTable [3]) {
						flag += 64;
					}
					break;
				}
			}

			return	flag;
		}

		private void KokuhakuRivalWaitStop(int Number,bool flag){
			EventWaitStop.SetActive (true);
			cbTamagoChara [Number].gotoAndPlay ("anger");
			EventWaitStop.transform.Find ("panel/image").gameObject.GetComponent<Image> ().sprite = CharaTamago [Number].GetComponent<SpriteRenderer> ().sprite;

			if (flag) {
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp12);
				kokuhakuWaitTime = 120;
			}
		}

			
		private void ButtneYesClick(){
			LoveManWomanNumberSet (10);		// 好感度を１０上げる
			buttonFlag = true;
			btnYesNo = true;
		}
		private void ButtonNoClick(){
			LoveManWomanNumberSet (-10);	// 好感度を１０下げる
			buttonFlag = true;
			btnYesNo = false;
		}

		// 好感度を上下させる（本当はサーバーでやるのが良いと思う）
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
