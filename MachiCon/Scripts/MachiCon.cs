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
		[SerializeField] private Text[]	fukidashiMes;						// 吹き出しメッセージ
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
		[SerializeField] private GameObject EventSoudanAvaterNameText;		// アバターネームテキスト
		[SerializeField] private GameObject EventSoudanTamagoNameText;		// たまごっちネームテキスト
		[SerializeField] private GameObject	EventSoudanYesOld;				// 旧Yesボタン
		[SerializeField] private GameObject	EventSoudanNoOld;				// 旧Noボタン
		[SerializeField] private GameObject	EventSoudanYesNew;				// 新Yesボタン
		[SerializeField] private GameObject	EventSoudanNoNew;				// 新Noボタン
		[SerializeField] private GameObject EventSoudanKumo;				// 雲
		[SerializeField] private GameObject EventAppeal;					// アピールタイム
		[SerializeField] private GameObject EventKokuhaku;					// 


		private object[]		mparam;

		private int playerNumber = 0;					// ０〜７（０〜３：男の子、４〜７：女の子）
		private int targetNumber = 0;					// ０〜７（０〜３：男の子、４〜７：女の子）
		private int targetNumber2 = 0;

		private bool buttonFlag = false;
		private bool btnYesNo = false;					// Yes:true No:false
		private int sceneNumber = 0;					// シーンナンバー

		private CharaBehaviour[] tamagoChara = new CharaBehaviour[8];


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
			
		void Start(){
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

			playerNumber = Random.Range(0,8);

			EventSoudanYesNew.GetComponent<Button> ().onClick.AddListener (ButtneYesClick);
			EventSoudanNoNew.GetComponent<Button> ().onClick.AddListener (ButtonNoClick);

			jobCount = statusJobCount.machiconJobCount000;

			for (int i = 0; i < 8; i++) {
				tamagoChara [i] = CharaTamago [i].GetComponent<CharaBehaviour> ();
			}
			tamagoChara[0].init (new TamaChara (16));
			tamagoChara[1].init (new TamaChara (17));
			tamagoChara[2].init (new TamaChara (16));
			tamagoChara[3].init (new TamaChara (17));
			tamagoChara[4].init (new TamaChara (18));
			tamagoChara[5].init (new TamaChara (19));
			tamagoChara[6].init (new TamaChara (18));
			tamagoChara[7].init (new TamaChara (19));



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

					tamagoChara [posNumber].gotoAndPlay ("walk");
					jobCount = statusJobCount.machiconJobCount050;
					break;
				}
			case	statusJobCount.machiconJobCount050:
				{	// たまごっち入場
					float pos = FlameInPositionChange ();
					if (pos >= 1.0f) {
						jobCount = statusJobCount.machiconJobCount060;
						waitTime = 11;
						tamagoChara [posNumber].gotoAndPlay ("idle");
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

						tamagoChara [posNumber].gotoAndPlay ("walk");
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
					tamagoChara [posNumber].gotoAndPlay ("idle");
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
					waitTime = 60;
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
					}
					AppealPositionChangeLoop ();			// アピールタイムのキャラ移動

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
					}
					break;
				}
			case	statusJobCount.machiconJobCount260:
				{
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

			fukidashiMes [0].text = mesAvater + mesRet + mesTamago + mesRet;
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
			}
		}
			
		// アピールタイムのキャラ移動
		private void AppealPositionChangeLoop(){
			for (int i = 0; i < 8; i++) {
				posTamago [i].x += Random.Range (-0.005f, 0.005f);
				posTamago [i].y += Random.Range (-0.005f, 0.005f);
			}
		}



		// 作業中（自動移動をどのよういするかを思案中）
		private void ApplealPositionChangeMain(){
		
		
		
		}



		// 告白タイムの初期配置
		private void KokuhakuPositionInit(){
			float[,] pos = new float[8, 2] {
				{  5.5f, -3.5f },
				{  4.5f, -2.0f },
				{  3.5f, -0.5f },
				{  2.5f,  1.0f },
				{ -5.5f, -3.5f },
				{ -4.5f, -2.0f },
				{ -3.5f, -0.5f },
				{ -2.5f,  1.0f },
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
			}
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
			switch (targetNumber) {
			case	0:
				{
					EventSoudanAvaterNameText.GetComponent<Text> ().text = "アバターネーム１";
					EventSoudanTamagoNameText.GetComponent<Text> ().text = "たまごっちネーム";
					break;
				}
			case	1:
				{
					EventSoudanAvaterNameText.GetComponent<Text> ().text = "アバターネーム２";
					EventSoudanTamagoNameText.GetComponent<Text> ().text = "たまごっちネーム";
					break;
				}
			case	2:
				{
					EventSoudanAvaterNameText.GetComponent<Text> ().text = "アバターネーム３";
					EventSoudanTamagoNameText.GetComponent<Text> ().text = "たまごっちネーム";
					break;
				}
			case	3:
				{
					EventSoudanAvaterNameText.GetComponent<Text> ().text = "アバターネーム４";
					EventSoudanTamagoNameText.GetComponent<Text> ().text = "たまごっちネーム";
					break;
				}
			case	4:
				{
					EventSoudanAvaterNameText.GetComponent<Text> ().text = "アバターネーム５";
					EventSoudanTamagoNameText.GetComponent<Text> ().text = "たまごっちネーム";
					break;
				}
			case	5:
				{
					EventSoudanAvaterNameText.GetComponent<Text> ().text = "アバターネーム６";
					EventSoudanTamagoNameText.GetComponent<Text> ().text = "たまごっちネーム";
					break;
				}
			case	6:
				{
					EventSoudanAvaterNameText.GetComponent<Text> ().text = "アバターネーム７";
					EventSoudanTamagoNameText.GetComponent<Text> ().text = "たまごっちネーム";
					break;
				}
			case	7:
				{
					EventSoudanAvaterNameText.GetComponent<Text> ().text = "アバターネーム８";
					EventSoudanTamagoNameText.GetComponent<Text> ().text = "たまごっちネーム";
					break;
				}
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
