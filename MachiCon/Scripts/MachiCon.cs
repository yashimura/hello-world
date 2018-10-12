using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;
using Mix2App.Lib.Sounds;
using Mix2App.Lib.Net;



namespace Mix2App.MachiCon{
	public class MachiCon : MonoBehaviour,IReceiver {
		[SerializeField] private Message	MesDisp;						// メッセージ関連
		[SerializeField] private GameObject EventCurtain;					// カーテン
		[SerializeField] private GameObject EventTitle;						// たまキュンパーティータイトル
		[SerializeField] private GameObject EventTitleClock;				// たまキュンパーティーカウントダウンマーク
		[SerializeField] private GameObject EventTitleText;					// たまキュンパーティーカウントダウン時間
		[SerializeField] private Sprite[] EventTitleSprite;
		[SerializeField] private GameObject[] CharaTamago;					// たまごっち
		[SerializeField] private GameObject[] CharaTamagochi;				// たまごっち（Image）
		[SerializeField] private GameObject EventPhase;						// 幕間
		[SerializeField] private GameObject EventPhaseRing;					// 幕間ナンバーリング
		[SerializeField] private GameObject EventPhasePTStar;				// 幕間ナンバーエフェクト
		[SerializeField] private GameObject EventPhaseCount;				// 幕間ナンバー
		[SerializeField] private Sprite[] EventPhaseSprite;					// 幕間ナンバーデーター
		[SerializeField] private GameObject EventSoudan;					// 相談
		[SerializeField] private GameObject EventSoudanChara;				// 旧相談たまごっち
		[SerializeField] private GameObject EventSoudanTarget;				// 旧相談対象たまごっち
		[SerializeField] private GameObject EventSoudanTamago;				// 新相談たまごっち（chara,target）
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
		[SerializeField] private GameObject	EventEndMarriage;				// 結婚式へ
		[SerializeField] private GameObject EventFutago;					// 双子選択
		[SerializeField] private Button[] EventFutagoButton;				// 双子選択ボタン
		[SerializeField] private GameObject[] CharaFutago;					// 双子のたまごっち
		[SerializeField] private GameObject[] CharaFutagoImage;				// 双子のたまごっち（Image）
		[SerializeField] private GameObject[] CharaFutagoName;				// 双子の名前
		[SerializeField] private GameObject TamagoEffect;
		[SerializeField] private GameObject	PrgCanvas;
		[SerializeField] private Sprite[] StampImage;						// スタンプイメージ


		//private object[]		mparam;

		private int playerNumber = 0;					// ０〜７（０〜３：男の子、４〜７：女の子）
		private int targetNumber = 0;					// ０〜７（０〜３：男の子、４〜７：女の子）
		private bool playerResultFlag = false;			// カップル成立:true 不成立:false

		private bool buttonFlag = false;
		//private bool btnYesNo = false;					// Yes:true No:false
		private int sceneNumber = 0;					// シーンナンバー
		//private bool screenMode = false;

		private bool startEndFutagoFlag = false;
		private bool buttonFutagoFlag = false;
		private int buttonFutagoNumber = 0;

		private bool kokuhakuTimeEndFlag = true;
		private float applealWaitTime;



		private CharaBehaviour[] cbTamagoChara = new CharaBehaviour[8];
		private CharaBehaviour[] cbFutagoChara = new CharaBehaviour[2];

		// 相性度（０〜１００）（男の子、女の子）
		private int[,] loveManWoman = new int[4, 4]{ { 40,40,40,40 }, { 40,40,40,40 }, { 40,40,40,40 }, { 40,40,40,40 } };
		// 成否判定の種（０〜１００）（男の子、女の子）
		private int[,] loveManWomanFix;

		private readonly float APPEAL_LIMIT_TIME = 5.0f;			// アピールタイムのキー入力待ち時間（秒）

		private statusJobCount	jobCount = statusJobCount.machiconJobCount020;
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
			kokuhakuCount420,
			kokuhakuCount430,
			kokuhakuCount440,
		};

		private PartyData mpdata;
		private AskData[] maskdatas;
		private bool mresult;
		private int mkind;
		private User muser1;
		private int mkind1;
		private User muser2;
		private int mkind2;
		private int[] mkindTable = new int[8];

		void Awake(){
			Debug.Log ("MachiCon Awake");
			mresult=false;
			loveManWomanFix=null;
			mkind=0;
			mkind1=0;
			mkind2=0;
			muser1=null;
			muser2=null;
			maskdatas = new AskData[3];
			for (int i = 0; i < maskdatas.Length; i++) {
				maskdatas [i].askIndex = 0;
				maskdatas [i].askUid = 0;
				maskdatas [i].result = 2;
			}
		}

		public void receive(params object[] parameter){
			Debug.Log ("MachiCon receive");
			//mparam = parameter;

			// イベントたまキュンか？
			// コラボたまキュンか？
			// 通常たまキュンか？


		}
			
		void Start() {
			Debug.Log ("MachiCon start");

			//ルーム情報取得、メンバーマッチング処理
			//パラメタは設計書参照
			GameCall call = new GameCall(CallLabel.GET_ROOM_INFO);
			call.AddListener(mgetroominf);
			ManagerObject.instance.connect.send(call);

		}
		
		void mgetroominf(bool success,object data)
		{
			// dataの内容は設計書参照
			// dataを変更したいときはConnectManagerDriverのGetRoomInfo()を変更する
			mpdata = (PartyData)data;
			StartCoroutine(mstart());
		}

		private PartyResultData prdata;
		void mgetroomres(bool success,object data)
		{
			mresult=true;
			// dataの内容は設計書参照
			// dataを変更したいときはConnectManagerDriverのGetRoomResult()を変更する
			prdata = (PartyResultData)data;
//			loveManWoman = prdata.loves;
//			loveManWomanFix = prdata.fixs;
		}

		IEnumerator mstart()
		{

			playerNumber = mpdata.playerIndex;
			muser1 = mpdata.members[playerNumber].user;
			mkind1 = mpdata.members[playerNumber].index;
			playerResultFlag = false;

			EventSoudanYesNew.GetComponent<Button> ().onClick.AddListener (ButtneYesClick);
			EventSoudanNoNew.GetComponent<Button> ().onClick.AddListener (ButtonNoClick);

			jobCount = statusJobCount.machiconJobCount020;							// 双子選択画面を飛ばす場合



//			muser1.chara2 = new TamaChara (29);



			// めいんBGMを登録
			ManagerObject.instance.sound.playBgm (11);

			for (int i = 0; i < 8; i++) {
				mkindTable [i] = mpdata.members [i].index;
			}
				
			MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);

			if (muser1.chara2 != null) {
				jobCount = statusJobCount.machiconJobCount000;						// 双子がいる時は、出席者選択画面を開く
				startEndFutagoFlag = false;
				EventFutago.SetActive (true);

				EventFutagoButton[0].onClick.AddListener (ButtonFutagoChara1);
				EventFutagoButton[1].onClick.AddListener (ButtonFutagoChara2);

				CharaFutagoName[0].GetComponent<Text>().text = muser1.GetCharaAt (0).cname;
				CharaFutagoName[1].GetComponent<Text>().text = muser1.GetCharaAt (1).cname;

				for (int i = 0; i < 2; i++) {
					cbFutagoChara [i] = CharaFutago [i].GetComponent<CharaBehaviour> ();
				}
				yield return cbFutagoChara [0].init (muser1.GetCharaAt(0));
				cbFutagoChara [0].gotoAndPlay (MotionLabel.IDLE);
				yield return cbFutagoChara [1].init (muser1.GetCharaAt(1));
				cbFutagoChara [1].gotoAndPlay (MotionLabel.IDLE);

				startEndFutagoFlag = true;
			}

			for (int i = 0; i < 8; i++) {
				cbTamagoChara [i] = CharaTamago [i].GetComponent<CharaBehaviour> ();
			}
			yield return cbTamagoChara[0].init (mpdata.members[0].user.GetCharaAt(mkindTable[0]));// 男の子１のたまごっちを登録する
			yield return cbTamagoChara[1].init (mpdata.members[1].user.GetCharaAt(mkindTable[1]));// 男の子２のたまごっちを登録する
			yield return cbTamagoChara[2].init (mpdata.members[2].user.GetCharaAt(mkindTable[2]));// 男の子３のたまごっちを登録する
			yield return cbTamagoChara[3].init (mpdata.members[3].user.GetCharaAt(mkindTable[3]));// 男の子４のたまごっちを登録する
			yield return cbTamagoChara[4].init (mpdata.members[4].user.GetCharaAt(mkindTable[4]));// 女の子１のたまごっちを登録する
			yield return cbTamagoChara[5].init (mpdata.members[5].user.GetCharaAt(mkindTable[5]));// 女の子２のたまごっちを登録する
			yield return cbTamagoChara[6].init (mpdata.members[6].user.GetCharaAt(mkindTable[6]));// 女の子３のたまごっちを登録する
			yield return cbTamagoChara[7].init (mpdata.members[7].user.GetCharaAt(mkindTable[7]));// 女の子４のたまごっちを登録する

			_tamagochiWalkSEFlag = true;
			StartCoroutine ("TamagochiWalkSEPlay");
		}
	
		void Destroy(){
			_tamagochiWalkSEFlag = false;
			Debug.Log ("MachiCon Destroy");
		}

		void OnDestroy(){
			_tamagochiWalkSEFlag = false;
			Debug.Log ("machicon OnDestroy");
		}

		private bool _tamagochiWalkSEFlag;
		private bool _eventAppealSetActiveFlag;
		private IEnumerator TamagochiWalkSEPlay(){
			while (_tamagochiWalkSEFlag) {
				bool flag = false;

				if (!_eventAppealSetActiveFlag) {
					for (int i = 0; i < 8; i++) {
						if ((cbTamagoChara [i].nowlabel == MotionLabel.WALK) || (cbTamagoChara[i].nowlabel == MotionLabel.SHY4)){
							flag = true;
						}
					}
				}
				if (flag) {
					ManagerObject.instance.sound.playSe (1);
				}

				yield return new WaitForSeconds (0.5f);
			}
		}
		private void TamagochiAnimeAllSetIDLE (){
			for (int i = 0; i < 8; i++) {
				cbTamagoChara [i].gotoAndPlay (MotionLabel.IDLE);
			}
		}


		private float	countTime1;
		private int		countTime2;
		private int		waitTime;
		private Vector3[] posTamago = new Vector3[8];
		private int		posNumber;
		private bool	soudanJumpFlag = false;


		void Update(){
			switch (jobCount) {
			case	statusJobCount.machiconJobCount000:
				{
					if (startEndFutagoFlag) {
						jobCount = statusJobCount.machiconJobCount010;

						buttonFutagoFlag = false;
						buttonFutagoNumber = 0;
					}

					TamagochiImageMove (CharaFutagoImage [0], CharaFutago [0], "");
					TamagochiImageMove (CharaFutagoImage [1], CharaFutago [1], "");

					EventTitleLightDisp (false);

					break;
				}
			case	statusJobCount.machiconJobCount010:
				{
					if (buttonFutagoFlag) {
						jobCount = statusJobCount.machiconJobCount020;
						EventFutago.SetActive (false);											// 双子選択画面を消す。
						mkind1 = buttonFutagoNumber;
						mkindTable[playerNumber] = buttonFutagoNumber;
						if (buttonFutagoNumber == 0) {
							cbTamagoChara[playerNumber].init (muser1.GetCharaAt(0));
						} else {
							cbTamagoChara[playerNumber].init (muser1.GetCharaAt(1));
						}
					}

					TamagochiImageMove (CharaFutagoImage [0], CharaFutago [0], "");
					TamagochiImageMove (CharaFutagoImage [1], CharaFutago [1], "");

					break;
				}
			case	statusJobCount.machiconJobCount020:
				{
					countTime1 = 0.0f;
					countTime2 = 10;

					MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp01);
					EventTitle.SetActive (true);

					for (int i = 0; i < 8; i++) {
						posTamago [i] = new Vector3 (0.0f, -420.0f, 0.0f);						// たまごっちの初期位置
					}

					EventTitleLightDisp (true);

					jobCount = statusJobCount.machiconJobCount030;
					break;
				}
			case	statusJobCount.machiconJobCount030:
				{	// カウントダウン
					countTime1 += 1.0f * Time.deltaTime;
					EventTitleClock.GetComponent<Image> ().fillAmount = countTime1;
					if (countTime1 >= 1.0f) {
						countTime1 -= 1.0f;
						countTime2--;
					}
					EventTitleText.GetComponent<Image> ().sprite = EventTitleSprite [countTime2];
					if (countTime2 == 0) {
						jobCount = statusJobCount.machiconJobCount040;

						EventTitle.GetComponent<Animator> ().SetBool ("title", true);
						waitTime = 60;
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
					}
					break;
				}
			case	statusJobCount.machiconJobCount040:
				{
					if(WaitTimeSubLoop()){
						jobCount = statusJobCount.machiconJobCount050;
					}
					break;
				}
			case	statusJobCount.machiconJobCount050:
				{
					if (EventCurtainPositionChange (15.0f)) {									// カーテンオープン
						EventTitle.SetActive (false);
						EventCurtain.SetActive (false);
						jobCount = statusJobCount.machiconJobCount060;
						posNumber = 0;
					}
					EventTitleLightDisp (false);
					break;
				}
			case	statusJobCount.machiconJobCount060:
				{	// 入場開始
					FlameInMessageDisp ();														// 実況開始

					jobCount = statusJobCount.machiconJobCount070;
					break;
				}
			case	statusJobCount.machiconJobCount070:
				{	// たまごっち入場
					StartCoroutine ("OpenningTamagoIdou");										// 画面下から入場して整列位置に停止するまで

					jobCount = statusJobCount.machiconJobCount080;
					break;
				}
			case	statusJobCount.machiconJobCount080:
				{	// 整列位置に停止
					if (_openningTamagoIdouFlag) {					
						posNumber++;
						if (posNumber == 8) {
							jobCount = statusJobCount.machiconJobCount090;
							EventCurtain.SetActive (true);
							MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp06);
							StartCoroutine (MessageDispOff(3.0f));
							sceneNumber = 0;
							WaitTimeSecInit (3);													// 3秒ウエイト
						} else {
							jobCount = statusJobCount.machiconJobCount060;
						}
					}
					break;
				}
			case	statusJobCount.machiconJobCount090:
				{
					if (WaitTimeSecSubLoop ()) {
						jobCount = statusJobCount.machiconJobCount100;
					}
					break;
				}
			case	statusJobCount.machiconJobCount100:
				{
					if (EventCurtainPositionChange (-15.0f)) {									// カーテンクローズ
						jobCount = statusJobCount.machiconJobCount110;
						EventPhase.SetActive (true);
						EventPhasePTStar.SetActive (true);
						EventPhaseCount.GetComponent<Image> ().sprite = EventPhaseSprite [sceneNumber];

						WaitTimeSecInit (1);

						TargetRandomSet ();														// 相談相手の決定

						TamagochiAnimeAllSetIDLE ();
						_eventAppealSetActiveFlag = false;
						EventAppeal.SetActive (false);
						EventAppealTableHeartClear ();											// テーブルハートを消しておく

						ManagerObject.instance.sound.playBgm (12);
					}
					break;
				}
			case	statusJobCount.machiconJobCount110:
				{	// 相談タイム開始
					EventPhasePTStar.transform.RotateAround (EventPhaseRing.transform.position, new Vector3 (0, 0, 1), 360.0f * Time.deltaTime);
					if(WaitTimeSecSubLoopZero()){
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
						StartCoroutine (SoudanTextDisp(statusJobCount.machiconJobCount160));
					}
					break;
				}
			case	statusJobCount.machiconJobCount150:
				{
					//SoudanTextDispが終了するとJobCountがmachiconJobCount160になる
					break;
				}
			case	statusJobCount.machiconJobCount160:
				{
					jobCount = statusJobCount.machiconJobCount170;
					EventSoudan.SetActive (true);
					EventSoudanNameSet ();													// 相談する時の相手の名前などを登録する
					EventSoudanPartsDispOff ();												// 相談で不必要なパーツを消す
					EventSoudanTamago.transform.Find ("chara").gameObject.SetActive (true);
					Vector3 pos = EventSoudanTamago.transform.localPosition;
					pos.y -= 800.0f;
					EventSoudanTamago.transform.localPosition = pos;
					soudanJumpFlag = false;
					for (int i = 0; i < 8; i++) {
						cbTamagoChara [i].gotoAndPlay (MotionLabel.IDLE);
					}
					MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
					break;
				}
			case	statusJobCount.machiconJobCount170:
				{
					if (EventSoudan.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.machiconJobCount180;
						SoudanMessageDisp ();													// 相談タイトルメッセージ設定

						EventSoudanPartsDisp (true);
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp07);

						StartCoroutine ("AppelTimeWait");										// APPEAL_LIMIT_TIME秒間でキー入力待ち終了
					}

					if (soudanJumpFlag) {
						SoudanTamgoCharaJump ();												// ちょっとジャンプ
					} else {
						Vector3 pos = EventSoudanTamago.transform.localPosition;
						pos.y += (30.0f * (60 * Time.deltaTime));								// たまごっちを上昇させる
						if (pos.y >= 0) {
							pos.y = 0;
							soudanJumpFlag = true;												// ここからは、ちょっとジャンプ
							waitTime = soudanJumpTable.Length;
						}
						EventSoudanTamago.transform.localPosition = pos;
					}

					SoudanTamagoCharaSet ();													// 相談シーンのたまごっちのアニメ
					break;
				}
			case	statusJobCount.machiconJobCount180:
				{
					if (buttonFlag) {
						jobCount = statusJobCount.machiconJobCount190;
						EventSoudan.GetComponent<Animator> ().SetBool ("waite", true);
						EventSoudanPartsDisp (false);

						MesDisp.SoudanMesDisp (Message.SoudanMesTable.SoudanMesDispOff);

						_eventAppealSetActiveFlag = true;
						EventAppeal.SetActive (true);
					}
					SoudanTamagoCharaSet ();													// 相談シーンのたまごっちのアニメ
					break;
				}
			case	statusJobCount.machiconJobCount190:
				{
					Vector3 pos = EventSoudanTamago.transform.localPosition;
					pos.y -= (20.0f * (60 * Time.deltaTime));
					EventSoudanTamago.transform.localPosition = pos;							// たまごっちを下降させる
					if(pos.y <= -300.0f){
						jobCount = statusJobCount.machiconJobCount200;
						EventSoudanTamago.transform.Find ("chara").gameObject.SetActive (false);
						AppealPositionChangeInit ();											// アピールタイム初期表示位置設定
						TamagochiFukidashiOff ();
						waitTime = 90;

						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
					}
					break;
				}
			case	statusJobCount.machiconJobCount200:
				{
					if (WaitTimeSubLoop ()) {
						jobCount = statusJobCount.machiconJobCount210;
					}
					break;
				}
			case	statusJobCount.machiconJobCount210:
				{
					if (EventCurtainPositionChange (15.0f)) {									// カーテンオープン
						jobCount = statusJobCount.machiconJobCount220;
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp08);
						MessageDispOff (4.0f);
						for (int i = 0; i < 8; i++) {
							CharaTamagochi [i].transform.Find ("fukidashi/message").gameObject.SetActive (true);
						}
						TablePositionInit ();
						WaitTimeSecInit (2);

						ManagerObject.instance.sound.playBgm (11);
					}
					break;
				}
			case	statusJobCount.machiconJobCount220:
				{
					if (WaitTimeSecSubLoop ()) {
						jobCount = statusJobCount.machiconJobCount230;
						for (int i = 0; i < 8; i++) {
							CharaTamagochi [i].transform.Find ("fukidashi/message").gameObject.SetActive (false);
						}
						WaitTimeSecInit (15);													// アピールタイムは１５秒
						applealWaitTime = 0.0f;
					}
					break;
				}
			case	statusJobCount.machiconJobCount230:
				{
					ApplealPositionChangeMain ();												// アピールタイムのキャラ移動

					// 一定時間で実況表示を変える
					applealWaitTime += (1.0f * (60 * Time.deltaTime));
					if(applealWaitTime > 180.0f){
						applealWaitTime = 0.0f;
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp09);
					}
					if(WaitTimeSecSubLoop()){
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
						sceneNumber++;
						if (sceneNumber < 2) {													// アピールタイムは２回
							buttonFlag = false;
							EventSoudan.SetActive (false);
							jobCount = statusJobCount.machiconJobCount100;						// アピールタイム再開
						} else {
							jobCount = statusJobCount.machiconJobCount240;						// アピールタイム終了
							MesDisp.KokuhakuCurtainJikkyouOnOff(false);
						}
						TamagochiFukidashiOff ();
					}
					break;
				}
			case	statusJobCount.machiconJobCount240:
				{
					if (EventCurtainPositionChange (-15.0f)) {									// カーテンクローズ
						jobCount = statusJobCount.machiconJobCount250;
						waitTime = 60;
						TamagochiAnimeAllSetIDLE ();
						_eventAppealSetActiveFlag = false;
						EventAppeal.SetActive (false);
						EventAppealTableHeartClear ();											// テーブルハートを消しておく

						//相談送信、全メンバーの相談処理を完了後、告白結果取得
						// パラメタは設計書参照
						//TODO masklistは現在何も反映していないので適宜設定を
//						GameCall call = new GameCall(CallLabel.GET_ROOM_RESULT,maskdatas);
						GameCall call = new GameCall(CallLabel.GET_ROOM_RESULT,mpdata.roomId,maskdatas,mpdata.members);
						call.AddListener(mgetroomres);
						ManagerObject.instance.connect.send(call);
					}
					break;
				}
			case	statusJobCount.machiconJobCount250:
				{
					if (WaitTimeSubLoop () && mresult) {
						jobCount = statusJobCount.machiconJobCount260;
						EventKokuhaku.SetActive (true);											// 告白タイム表示
						waitTime = 60;
						KokuhakuPositionInit ();												// 告白タイム各キャラクターの初期配置
						KokuhakuTimeInit ();													// 男の子の告白する女の子の番号を登録

						ManagerObject.instance.sound.playBgm (13);
					}
					break;
				}
			case	statusJobCount.machiconJobCount260:
				{
					if (WaitTimeSubLoop ()) {
						jobCount = statusJobCount.machiconJobCount270;
					}
					break;
				}
			case	statusJobCount.machiconJobCount270:
				{
					if (EventCurtainPositionChange (15.0f)) {									// カーテンオープン
						jobCount = statusJobCount.machiconJobCount280;
						EventKokuhaku.SetActive (false);
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp10);
						MesDisp.KokuhakuCurtainJikkyouOnOff(true);
						kokuhakuTimeEndFlag = false;
					}
					break;
				}
			case	statusJobCount.machiconJobCount280:
				{
					if (kokuhakuTimeEndFlag) {
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp15);		// 告白処理が終了したのでお別れメッセージを表示
						jobCount = statusJobCount.machiconJobCount290;

						ManagerObject.instance.sound.playBgm (11);
					}
					KokuhakuTimeMain ();														// 告白処理ループ
					break;
				}
			case	statusJobCount.machiconJobCount290:
				{
					if (EventCurtainPositionChange (-15.0f)) {									// カーテンクローズ
						jobCount = statusJobCount.machiconJobCount300;

						EventEnd.SetActive (true);												// おしまいを表示準備
						Color color = EventEnd.transform.Find ("text").gameObject.GetComponent<Image> ().color;
						color.a = 0.0f;
						EventEnd.transform.Find ("text").gameObject.GetComponent<Image> ().color = color;
					}
					break;
				}
			case	statusJobCount.machiconJobCount300:
				{
					Color color = EventEnd.transform.Find ("text").gameObject.GetComponent<Image> ().color;
					color.a += 0.02f;															// おしまいをフェードイン
					if (color.a >= 1.0f) {
						color.a = 1.0f;
						jobCount = statusJobCount.machiconJobCount310;
						waitTime = 30;
					}
					EventEnd.transform.Find ("text").gameObject.GetComponent<Image> ().color = color;
					break;
				}
			case	statusJobCount.machiconJobCount310:
				{
					if (WaitTimeSubLoop ()) {
						if (playerResultFlag) {
							EventEndMarriage.SetActive (true);									// 自キャラがカップル成立したのでハートフェードを表示する
							jobCount = statusJobCount.machiconJobCount320;
						} else {
							jobCount = statusJobCount.machiconJobCount330;						// 自キャラがカップル成立していないのでそのままシーンチェンジ
						}
					}
					break;
				}
			case	statusJobCount.machiconJobCount320:
				{
					Vector3 _pos = EventEndMarriage.transform.Find ("panel").gameObject.transform.localPosition;
					_pos.y += (15.0f * (60 * Time.deltaTime));
					if (_pos.y >= 425.0f) {
						_pos.y = 425.0f;
						jobCount = statusJobCount.machiconJobCount330;
					}
					EventEndMarriage.transform.Find ("panel").gameObject.transform.localPosition = _pos;
					break;
				}
			case	statusJobCount.machiconJobCount330:
				{
					if (WaitTimeSubLoop ()) {
						if (playerResultFlag) {
							if ((muser1.utype == UserType.MIX2) && (muser2.utype == UserType.MIX2)){
								Debug.Log ("二人ともみーつユーザーなので、結婚イベントへ・・・");
								ManagerObject.instance.view.change(SceneLabel.MARRIAGE,mkind,muser1,mkind1,muser2,mkind2);
							} else {
								Debug.Log ("みーつユーザー以外なので、デートイベントへ・・・");
								ManagerObject.instance.view.change(SceneLabel.MARRIAGE_DATE,mkind,muser1,mkind1,muser2,mkind2);
							}
						} else {
							Debug.Log ("たまタウンへ・・・");
							ManagerObject.instance.view.change (SceneLabel.TOWN);
						}

						jobCount = statusJobCount.machiconJobCount340;
					}
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
				CharaTamagochi [i].transform.localPosition = posTamago [i];
			}

			for (int i = 0; i < 8; i++) {
				TamagochiImageMove (CharaTamagochi [i], CharaTamago [i], "");
				if(CharaTamago[i].transform.localScale.x <= 0){
					CharaTamagochi [i].transform.localScale = new Vector3 (-2.5f, 2.5f, 1.0f);
					CharaTamagochi [i].transform.Find ("fukidashi").gameObject.transform.localScale = new Vector3 (-0.42f, 0.42f, 1.0f);
				} else {
					CharaTamagochi [i].transform.localScale = new Vector3 (2.5f, 2.5f, 1.0f);
					CharaTamagochi [i].transform.Find ("fukidashi").gameObject.transform.localScale = new Vector3 (0.42f, 0.42f, 1.0f);
				}
			}
		}

		private void EventTitleLightDisp (bool flag)
		{
			EventTitle.transform.Find ("Light1").GetComponent<Image> ().enabled = flag;
			EventTitle.transform.Find ("Light2").GetComponent<Image> ().enabled = flag;
		}


		private void CharaTamagoFlipChange(int num,bool flag){
			Vector3 _scale = new Vector3 (1, 1, 1);
			if (flag) {
				_scale.x = -1;
			} else {
				_scale.x = 1;
			}
			CharaTamago [num].transform.localScale = _scale;
		}

		private bool WaitTimeSubLoop(){
			if (waitTime == 0) {
				return true;
			}
			waitTime--;
			if (waitTime == 0) {
				return true;
			}
			return false;
		}

		private float _waitSecTime1;
		private int _waitSecTime2;
		private void WaitTimeSecInit(int _time){
			_waitSecTime1 = 0.0f;
			_waitSecTime2 = _time;
		}
		private bool WaitTimeSecSubLoop(){
			_waitSecTime1 += 1.0f * Time.deltaTime;
			if (_waitSecTime1 >= 1.0f) {
				_waitSecTime1 -= 1.0f;
				if (_waitSecTime2 == 0) {
					return true;
				} else {
					_waitSecTime2--;
				}
			}

			return false;
		}
		private bool WaitTimeSecSubLoopZero(){
			_waitSecTime1 += 1.0f * Time.deltaTime;
			if (_waitSecTime1 >= 1.0f) {
				_waitSecTime1 -= 1.0f;
				_waitSecTime2--;
				if (_waitSecTime2 == 0) {
					return true;
				}
			}

			return false;
		}

		// 相談シーンのたまごっちをImageに反映させる
		private void SoudanTamagoCharaSet (){
			TamagochiImageMove (EventSoudanTamago, CharaTamago [playerNumber], "chara/");
			TamagochiImageMove (EventSoudanTamago, CharaTamago [targetNumber], "target/");
//			EventSoudanTamago.transform.Find ("chara").gameObject.GetComponent<Image> ().sprite = CharaTamago [playerNumber].GetComponent<SpriteRenderer> ().sprite;
//			EventSoudanTamago.transform.Find ("target").gameObject.GetComponent<Image> ().sprite = CharaTamago [targetNumber].GetComponent<SpriteRenderer> ().sprite;
		}

		private float[] soudanJumpTable = new float[] {
			-12.0f, -9.0f, -9.0f, -6.0f, -6.0f, -6.0f, -3.0f, -3.0f, -3.0f, -3.0f, 0.0f, 0.0f, 0.0f,
			0.0f, 0.0f, 0.0f, 3.0f, 3.0f, 3.0f, 3.0f, 6.0f, 6.0f, 6.0f, 9.0f, 9.0f, 12.0f,
		};
		private void SoudanTamgoCharaJump(){
			Vector3 pos = EventSoudanTamago.transform.localPosition;
			if (waitTime != 0) {
				waitTime--;
				pos.y += soudanJumpTable [waitTime];
			}
			EventSoudanTamago.transform.localPosition = pos;
		}

		// 入場時のたまごっちの吹き出しメッセージ登録
		private void FukidashiMessageSet(){
			string mesAvater = "";
			string mesTamago = "";
			string mesRet = "\n";

			mesTamago = mpdata.members [posNumber].user.GetCharaAt (mkindTable[posNumber]).cname;
			mesAvater = mpdata.members [posNumber].user.nickname;

			if (mesTamago == "" || mesTamago == null) {
				switch (posNumber) {
				case	0:
					{
						mesAvater = "アバターネーム1";
						mesTamago = "たまごっちネーム";
						break;
					}
				case	1:
					{
						mesAvater = "アバターネーム2";
						mesTamago = "たまごっちネーム";
						break;
					}
				case	2:
					{
						mesAvater = "アバターネーム3";
						mesTamago = "たまごっちネーム";
						break;
					}
				case	3:
					{
						mesAvater = "アバターネーム4";
						mesTamago = "たまごっちネーム";
						break;
					}
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
			}

			CharaTamagochi [posNumber].transform.Find ("fukidashi/comment/text").gameObject.GetComponent<Text> ().text = mesAvater + mesRet + mesTamago;

/////			if (mpdata.members [posNumber].user.utype != UserType.MIX2) {
/////				switch (posNumber) {
/////				case	0:
/////				case	1:
/////				case	2:
/////				case	3:
/////					{	//男の子：ハート、音符、笑顔、歓喜、にやり（RND）
/////						int[] _table = new int[5]{ 0, 2, 3, 4, 6 };
/////						CharaTamagochi [posNumber].transform.Find ("fukidashi/stamp/Image").gameObject.GetComponent<Image> ().sprite = StampImage [_table [Random.Range (0, _table.Length)]];
/////						break;
/////					}
/////				case	4:
/////				case	5:
/////				case	6:
/////				case	7:
/////					{	//女の子：ハート、音符、笑顔、歓喜、苦笑い（RND）
/////						int[] _table = new int[5]{ 0, 2, 3, 4, 5 };
/////						CharaTamagochi [posNumber].transform.Find ("fukidashi/stamp/Image").gameObject.GetComponent<Image> ().sprite = StampImage [_table [Random.Range (0, _table.Length)]];
/////						break;
/////					}
/////				}
/////			}

		}
		// 告白タイムの対象相手を指定する
		// num:男の子の番号（０〜３）、targetNum:告白対象者の番号（４〜７）
		private void FukidashiMessageSetKokuhaku(int num,int targetNum){
			string mesAvater = "";
			string mesTamago = "";
			string mesRet = "\n";
			string mesNo = "の";
			string mesTo = "さん";

			mesTamago = mpdata.members [targetNum].user.GetCharaAt (mkindTable[targetNum]).cname;
			mesAvater = mpdata.members [targetNum].user.nickname;

			if (mesTamago == "" || mesTamago == null) {
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
			}

			CharaTamagochi [num].transform.Find ("fukidashi/comment/text").gameObject.GetComponent<Text> ().text = mesAvater + mesNo + mesRet + mesTamago + mesTo;
		}
		// 告白タイムの対象相手を指定する
		// num:女の子の番号（４〜７）、targetNum:告白対象者の番号（０〜３）
		private void FukidashiMessageSetKokuhakuWoman(int num,int targetNum){
			string mesAvater = "";
			string mesTamago = "";
			string mesRet = "\n";
			string mesNo = "の";
			string mesTo = "さん";

			mesTamago = mpdata.members [targetNum].user.GetCharaAt (mkindTable[targetNum]).cname;
			mesAvater = mpdata.members [targetNum].user.nickname;

			if (mesTamago == "" || mesTamago == null) {
				switch (targetNum) {
				case	0:
					{
						mesAvater = "アバターネーム1";
						mesTamago = "たまごっちネーム";
						break;
					}
				case	1:
					{
						mesAvater = "アバターネーム2";
						mesTamago = "たまごっちネーム";
						break;
					}
				case	2:
					{
						mesAvater = "アバターネーム3";
						mesTamago = "たまごっちネーム";
						break;
					}
				case	3:
					{
						mesAvater = "アバターネーム4";
						mesTamago = "たまごっちネーム";
						break;
					}
				}
			}

			CharaTamagochi [num].transform.Find ("fukidashi/comment/text").gameObject.GetComponent<Text> ().text = mesAvater + mesNo + mesRet + mesTamago + mesTo;
		}

		// 告白された事に対する返事のメッセージ（肯定、否定）
		// num:告白された女の子の番号（０〜３）、msgFlag:メッセージの種類（true:肯定、false:否定）
		private void FukidashiMessageKokuhakuReturn(int num,bool msgFlag){
			string _gobiMes = mpdata.members [num + 4].user.GetCharaAt (mkindTable[num + 4]).wend;

			if (msgFlag) {		// 告白肯定のメッセージ
				CharaTamagochi [num + 4].transform.Find ("fukidashi/comment/text").gameObject.GetComponent<Text> ().text = MesDisp.KokuhakuMesDisp (Message.KokuhakuMesTable.KokuhakuMesDispOK,_gobiMes);
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp13);
			} else {			// 告白否定のメッセージ
				CharaTamagochi [num + 4].transform.Find ("fukidashi/comment/text").gameObject.GetComponent<Text> ().text = MesDisp.KokuhakuMesDisp (Message.KokuhakuMesTable.KokuhakuMesDispNo,_gobiMes);
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp14);
			}

			StartCoroutine (MessageDispOff(4.0f));

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

			StartCoroutine (MessageDispOff(3.0f));

		}


		// アプリっちのメッセージ時間停止
		private IEnumerator MessageDispOff(float time){
			yield return new WaitForSeconds (time);
			MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
		}




		// 相談時のタイトルメッセージ
		private void SoudanMessageDisp(){
			string _gobi = mpdata.members [playerNumber].user.GetCharaAt (mkindTable[playerNumber]).wend;

			UserType ut = mpdata.members [playerNumber].user.utype;


			switch (playerNumber) {
			case	0:
			case	1:
			case	2:
			case	3:
				{
					if (ut == UserType.MIX2) {
						// みーつユーザー男の子
						MesDisp.SoudanMesDisp (Message.SoudanMesTable.SoudanMesDispMan1, _gobi);
					} else {
						// みーつユーザー以外男の子
						MesDisp.SoudanMesDisp (Message.SoudanMesTable.SoudanMesDispMan2, _gobi);
					}
					break;
				}
			case	4:
			case	5:
			case	6:
			case	7:
				{
					if (ut == UserType.MIX2) {
						// みーつユーザー女の子
						MesDisp.SoudanMesDisp (Message.SoudanMesTable.SoudanMesDispWoman1, _gobi);
					} else {
						// みーつユーザー以外女の子
						MesDisp.SoudanMesDisp (Message.SoudanMesTable.SoudanMesDispWoman2, _gobi);
					}
					break;
				}
			}
		}
			
		// アピールタイムの初期配置
		private void AppealPositionChangeInit(){
			float[,] pos = new float[8, 2] {
				{   90.0f, -200.0f },									// 男の子１の整列位置
				{  170.0f, -180.0f },									// 男の子２の整列位置
				{  250.0f, -160.0f },									// 男の子３の整列位置
				{  330.0f, -140.0f },									// 男の子４の整列位置
				{  -90.0f, -200.0f },									// 女の子１の整列位置
				{ -170.0f, -180.0f },									// 女の子２の整列位置
				{ -250.0f, -160.0f },									// 女の子３の整列位置
				{ -330.0f, -140.0f },									// 女の子４の整列位置
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
				CharaTamagoFlipChange (i, flag);

				posTamago [i].x = pos [i, 0];
				posTamago [i].y = pos [i, 1];

				cbTamagoChara [i].gotoAndPlay (MotionLabel.IDLE);

				CharaTamagochi [i].GetComponent<Canvas> ().sortingOrder = 1;
			}

			EventAppeal.transform.Find ("table1").gameObject.GetComponent<Canvas> ().sortingOrder = 0;
			EventAppeal.transform.Find ("table2").gameObject.GetComponent<Canvas> ().sortingOrder = 0;
			EventAppeal.transform.Find ("table3").gameObject.GetComponent<Canvas> ().sortingOrder = 0;
			EventAppeal.transform.Find ("table4").gameObject.GetComponent<Canvas> ().sortingOrder = 0;
				
			appealTimeCounter = 0;
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


		private float[] posTamagoTargetX = new float[8];
		private float[] posTamagoTargetY = new float[8];


		private int[] countTableChakusekiTime = new int[8]{
			0,0,0,0,0,0,0,0,
		};


		// アピールタイム動作作業中
		// 移動予定先
		//  男の子 テーブルの右側の４箇所かランダム地点かケーキバイキング
		//  女の子 テーブルの左側の４箇所かランダム地点かケーキバイキング
		private void ApplealPositionChangeMain(){
			if (appealTimeCounter == 0) {
				// アピールタイムの最初の移動はテーブルにつくようにする。
				tamagochiIdouInitFlag = Random.Range (0, 4);
				for (int i = 0; i < 8; i++) {
					TamagochiIdouInit (i,true);
				}
			}

			// 吹き出しをランダム表示
			for (int i = 0; i < 8; i++) {
				fukidashiTamagoWait [i]--;
				if (fukidashiTamagoWait [i] == 0) {
					fukidashiTamagoWait [i] = Random.Range (45, 75);
					switch (Random.Range (0, 12)) {
/*
					case	0:
						{	
							TamagochiFukidashiSet (i, tamagochiFukidashiType.CAKE);		// 吹き出しケーキを表示
							break;
						}
					case	1:
						{	
							TamagochiFukidashiSet (i, tamagochiFukidashiType.HEART);	// 吹き出しハートを表示
							break;
						}
*/						
					case	3:
						{
							TamagochiFukidashiSet (i, tamagochiFukidashiType.MESSAGE);	// 吹き出しメッセージを表示	
							break;
						}
					default:
						{
							TamagochiFukidashiSet (i, tamagochiFukidashiType.OFF);		// 吹き出しを消去
							break;
						}
					}
				}
			}

			EventAppealTableHeartClear ();												// テーブルハートを消しておく

			if (appealTimeCounter >= 0) {
				float[] _posY = new float[12];
				for (int i = 0; i < 8; i++) {
					if ((posTamagoSpeedX [i] != 0.0f) || (posTamagoSpeedY [i] != 0.0f)) {
						TamagochiAnimeSet (i, MotionLabel.WALK);

						if (posTamagoIdouXFlag [i]) {
							posTamago [i].x += (posTamagoSpeedX [i] * (60 * Time.deltaTime));
							if (posTamago [i].x >= posTamagoTargetX [i]) {
								posTamago [i].x = posTamagoTargetX [i];
							}
							CharaTamagoFlipChange (i, posTamagoIdouXFlag [i]);
						} else {
							posTamago [i].x -= (posTamagoSpeedX [i] * (60 * Time.deltaTime));
							if (posTamago [i].x <= posTamagoTargetX [i]) {
								posTamago [i].x = posTamagoTargetX [i];
							}
							CharaTamagoFlipChange (i, posTamagoIdouXFlag [i]);
						}

						if (posTamagoIdouYFlag [i]) {
							posTamago [i].y += (posTamagoSpeedY [i] * (60 * Time.deltaTime));
							if (posTamago [i].y >= posTamagoTargetY [i]) {
								posTamago [i].y = posTamagoTargetY [i];
							}
						} else {
							posTamago [i].y -= (posTamagoSpeedY [i] * (60 * Time.deltaTime));
							if (posTamago [i].y <= posTamagoTargetY [i]) {
								posTamago [i].y = posTamagoTargetY [i];
							}
						}

						if (posTamagoSpeedX [i] != 0.0f) {
							if (posTamago [i].x == posTamagoTargetX [i]) {
								TamagoChakusekiTimeSet (i);
							}
						} else {
							if (posTamago [i].y == posTamagoTargetY [i]) {
								TamagoChakusekiTimeSet (i);
							}
						}
						TamagochiTableHitcheck (i);										// テーブルとの当たり判定
						TamagoHitCheck (i);												// たまごっち同士の当たり判定
					} else {
						if (countTableChakusekiTime [i] != 0) {
							countTableChakusekiTime [i]--;
							TamagochiAnimeSetGRIDandIDLE (i, MotionLabel.IDLE);
							if (i < 4) {
								CharaTamagoFlipChange (i, false);
							} else {
								CharaTamagoFlipChange (i, true);
							}
							TamagoPairCheck (i);
							TamagoCakeCheck (i);
						} else {
							TamagochiIdouInit (i);
						}
					}
					_posY [i] = posTamago [i].y;
				}

				_posY[8] = _tablePostionY[0];
				_posY[9] = _tablePostionY[1];
				_posY[10] = _tablePostionY[2];
				_posY[11] = _tablePostionY[3];

				for (int j = 0; j < 12; j++) {								// たまごっちとテーブルの表示優先順位の変更
					int k = 0;
					float _checkPos = -1000.0f;
					for (int i = 0; i < 12; i++) {
						if (_checkPos <= _posY [i]) {
							_checkPos = _posY [i];
							k = i;
						}
					}
					_posY [k] = -1000.0f;
					switch (k) {
					default:
						{
							CharaTamagochi [k].GetComponent<Canvas> ().sortingOrder = j + 1;
							break;
						}
					case	8:
						{
							EventAppeal.transform.Find ("table1").gameObject.GetComponent<Canvas> ().sortingOrder = j + 1;
							break;
						}
					case	9:
						{
							EventAppeal.transform.Find ("table2").gameObject.GetComponent<Canvas> ().sortingOrder = j + 1;
							break;
						}
					case	10:
						{
							EventAppeal.transform.Find ("table3").gameObject.GetComponent<Canvas> ().sortingOrder = j + 1;
							break;
						}
					case	11:
						{
							EventAppeal.transform.Find ("table4").gameObject.GetComponent<Canvas> ().sortingOrder = j + 1;
							break;
						}
					}
				}
			}

			appealTimeCounter++;
		}

		private void TamagochiAnimeSet(int num,string status){
			if (cbTamagoChara [num].nowlabel != status) {
				cbTamagoChara [num].gotoAndPlay (status);
			}
		}

		private void TamagochiAnimeSetGRIDandIDLE(int num,string status){
			if((cbTamagoChara[num].nowlabel != MotionLabel.GLAD1) && (cbTamagoChara[num].nowlabel != MotionLabel.IDLE)){
				cbTamagoChara [num].gotoAndPlay (status);
			}
		}


		// 停止した時のウェイト時間
		// num:たまごっちの番号（０〜７）
		private void TamagoChakusekiTimeSet(int num){
			switch (posTamagoTargetType [num]) {
			case	0:
			case	1:
			case	2:
			case	3:
				{	// テーブルに着席した時の停止
					countTableChakusekiTime [num] = Random.Range (120, 180);
					break;
				}
			default:
				{	// それ以外の停止
					countTableChakusekiTime [num] = Random.Range (30, 90);
					break;
				}
			case	50:
				{	// 衝突判定後の停止
					countTableChakusekiTime [num] = 0;
					break;
				}
			}

			posTamago [num].x = posTamagoTargetX [num];
			posTamago [num].y = posTamagoTargetY [num];
			posTamagoSpeedX [num] = 0.0f;
			posTamagoSpeedY [num] = 0.0f;
		}

		// たまごっちとテーブルの当たり判定
		private void TamagochiTableHitcheck(int num){
			for(int i = 0;i < 4;i++){
				if (((posTamago[num].x - 40.0f) < _tablePostionX[i]) && (_tablePostionX[i] < (posTamago[num].x + 40.0f))) {
					if (((posTamago[num].y - 12.0f) < (_tablePostionY[i])) && ((_tablePostionY[i]) < (posTamago[num].y + 12.0f))) {
						HitCheckIdouSet(num,_tablePostionX[i],_tablePostionY[i]);
						break;
					}
				}
			}
		}

		// たまごっち同士の当たり判定
		private void TamagoHitCheck(int num){
			for (int num2 = 0; num2 < 8; num2++) {
				if (num == num2) {
					continue;
				}
				if (((posTamago [num].x - 55.0f) < posTamago [num2].x) && (posTamago [num2].x < (posTamago [num].x + 55.0f))) {
					if (((posTamago [num].y - 15.0f) < posTamago [num2].y) && (posTamago [num2].y < (posTamago [num].y + 15.0f))) {

						HitCheckIdouSet (num, posTamago [num2].x, posTamago [num2].y);
						break;
					}
				}
			}
		}

		// 衝突した後の移動先決定
		// num:たまごっちの番号
		// xPos:たまごっちと衝突した対象のX座標
		// ypos:たまごっちと衝突した対象のY座標
		private void HitCheckIdouSet(int num,float xPos,float yPos){
			float _idouX = Random.Range (0.0f, 1.0f) * 30.0f;
			float _idouY = Random.Range (0.0f, 1.0f) * 30.0f;

			if (posTamago [num].x < xPos) {
				posTamagoTargetX [num] = posTamago [num].x - _idouX;
			} else {
				posTamagoTargetX [num] = posTamago [num].x + _idouX;
			}

			if (posTamago [num].y < yPos) {
				posTamagoTargetY [num] = posTamago [num].y - _idouY;
			} else {
				posTamagoTargetY [num] = posTamago [num].y + _idouY;
			}

//			countTableChakusekiTime [num] = 0;
			posTamagoTargetType [num] = 50;

			if (posTamagoTargetX [num] < posTamago [num].x) {
				posTamagoIdouXFlag [num] = false;
				posTamagoSpeedX [num] = (posTamago [num].x - posTamagoTargetX [num]) / 30;
			} else {
				posTamagoIdouXFlag [num] = true;
				posTamagoSpeedX [num] = (posTamagoTargetX [num] - posTamago [num].x) / 30;
			}

			if (posTamagoTargetY [num] < posTamago [num].y) {
				posTamagoIdouYFlag [num] = false;
				posTamagoSpeedY [num] = (posTamago [num].y - posTamagoTargetY [num]) / 30;
			} else {
				posTamagoIdouYFlag [num] = true;
				posTamagoSpeedY [num] = (posTamagoTargetY [num] - posTamago [num].y) / 30;
			}
		}

		// テーブルハートを消しておく
		private void EventAppealTableHeartClear (){
			EventAppeal.transform.Find ("table1/heart").gameObject.SetActive (false);
			EventAppeal.transform.Find ("table2/heart").gameObject.SetActive (false);
			EventAppeal.transform.Find ("table3/heart").gameObject.SetActive (false);
			EventAppeal.transform.Find ("table4/heart").gameObject.SetActive (false);
		}

		private float[] _tablePostionX = new float[4];
		private float[] _tablePostionY = new float[4];

		private void TablePositionInit(){
			_tablePostionX[0] = EventAppeal.transform.Find ("table1").gameObject.transform.localPosition.x;
			_tablePostionX[1] = EventAppeal.transform.Find ("table2").gameObject.transform.localPosition.x;
			_tablePostionX[2] = EventAppeal.transform.Find ("table3").gameObject.transform.localPosition.x;
			_tablePostionX[3] = EventAppeal.transform.Find ("table4").gameObject.transform.localPosition.x;

			_tablePostionY[0] = EventAppeal.transform.Find ("table1").gameObject.transform.localPosition.y + 35.0f;
			_tablePostionY[1] = EventAppeal.transform.Find ("table2").gameObject.transform.localPosition.y + 35.0f;
			_tablePostionY[2] = EventAppeal.transform.Find ("table3").gameObject.transform.localPosition.y + 35.0f;
			_tablePostionY[3] = EventAppeal.transform.Find ("table4").gameObject.transform.localPosition.y + 35.0f;

			for (int i = 0; i < 4; i++) {
				posTamagoTargetTableMan [i].x = _tablePostionX [i] + 70.0f;
				posTamagoTargetTableMan [i].y = _tablePostionY [i];
				posTamagoTargetTableWoman [i].x = _tablePostionX [i] - 70.0f;
				posTamagoTargetTableWoman [i].y = _tablePostionY [i];
			}
		}

		private Vector2[] posTamagoTargetCakeTable = new Vector2[] {
			new Vector2 (160.0f, 105.0f),
			new Vector2 (-160.0f, 105.0f),
			new Vector2 (230.0f, 60.0f),
			new Vector2 (-230.0f, 60.0f),
			new Vector2 (300.0f, 15.0f),
			new Vector2 (-300.0f, 15.0f),
			new Vector2 (370.0f, -50.0f),
			new Vector2 (-370.0f, -50.0f),
		};
		private Vector2[] posTamagoTargetTableMan = new Vector2[4];
		private Vector2[] posTamagoTargetTableWoman = new Vector2[4];
		private int[] posTamagoTargetType = new int[8];
		private int tamagochiIdouInitFlag = 0;
		private void TamagochiIdouInit(int num,bool flag = false){
			float _randSpeed = Random.Range(120,180);
			int _randPoint;

			if (flag) {
				// 最初テーブルにつくようにランダムを廃止
				int[,]	_randPointTable = new int[4, 8] {
					{ 0, 2, 3, 1, 3, 1, 0, 2 },
					{ 0, 2, 1, 3, 1, 3, 0, 2 },
					{ 2, 0, 3, 1, 3, 1, 2, 0 },
					{ 2, 0, 1, 3, 1, 3, 2, 0 },
				};
				float[,] _randSpeedTable = new float[4, 8] {
					{ 160.0f, 200.0f, 300.0f, 100.0f, 300.0f, 100.0f, 160.0f, 200.0f },
					{ 160.0f, 200.0f, 100.0f, 300.0f, 100.0f, 300.0f, 160.0f, 200.0f },
					{ 160.0f, 180.0f, 100.0f, 240.0f, 100.0f, 240.0f, 160.0f, 180.0f },
					{ 130.0f, 180.0f, 100.0f, 240.0f, 100.0f, 240.0f, 130.0f, 180.0f },
				};

				_randPoint = _randPointTable [tamagochiIdouInitFlag, num];
				_randSpeed = _randSpeedTable [tamagochiIdouInitFlag, num];
			} else {
				_randPoint = Random.Range (0, 8);
			}

			switch (_randPoint) {
			case	0:
			case	1:
			case	2:
			case	3:
				{		// テーブル
					if (num < 4) {
						posTamagoTargetX [num] = posTamagoTargetTableMan [_randPoint].x;
						posTamagoTargetY [num] = posTamagoTargetTableMan [_randPoint].y;
					} else {
						posTamagoTargetX [num] = posTamagoTargetTableWoman [_randPoint].x;
						posTamagoTargetY [num] = posTamagoTargetTableWoman [_randPoint].y;
					}
					break;
				}
			case	4:
				{		// ケーキバイキング
					int _rand = Random.Range (0, posTamagoTargetCakeTable.Length);
					posTamagoTargetX [num] = posTamagoTargetCakeTable [_rand].x;
					posTamagoTargetY [num] = posTamagoTargetCakeTable [_rand].y;
					break;
				}
			default:
				{		// ランダム
					int _rand = Random.Range (-32, 32);
					int _rand2 = Random.Range (-20, 2);
					posTamagoTargetX [num] = _rand * 10;
					posTamagoTargetY [num] = _rand2 * 10;
					if ((_randPoint == 5) || (_randPoint == 6)) {
						int _rand3 = Random.Range (0, 8);
						switch (_rand3) {
						case	0:
							{
								posTamagoTargetX [num] = 0.0f;
								posTamagoTargetY [num] = 105.0f;
								break;
							}
						case	1:
							{
								posTamagoTargetX [num] = 80.0f;
								posTamagoTargetY [num] = 105.0f;
								break;
							}
						case	2:
							{
								posTamagoTargetX [num] = -80.0f;
								posTamagoTargetY [num] = 105.0f;
								break;
							}
						}
					}
					break;
				}
			}
			posTamagoTargetType [num] = _randPoint;					// 移動先のナンバー登録

			float x1 = System.Math.Abs(posTamago [num].x - posTamagoTargetX[num]);
			if (x1 < 1.0f) {
				posTamagoTargetX [num] = posTamago [num].x;
			}
			float y1 = System.Math.Abs (posTamago [num].y - posTamagoTargetY [num]);
			if (y1 < 1.0f) {
				posTamagoTargetY [num] = posTamago [num].y;
			}
				
			if (posTamagoTargetX [num] < posTamago [num].x) {
				posTamagoIdouXFlag [num] = false;
				posTamagoSpeedX [num] = (posTamago [num].x - posTamagoTargetX [num]) / _randSpeed;
			} else {
				posTamagoIdouXFlag [num] = true;
				posTamagoSpeedX [num] = (posTamagoTargetX [num] - posTamago [num].x) / _randSpeed;
			}

			if (posTamagoTargetY [num] < posTamago [num].y) {
				posTamagoIdouYFlag [num] = false;
				posTamagoSpeedY [num] = (posTamago [num].y - posTamagoTargetY [num]) / _randSpeed;
			} else {
				posTamagoIdouYFlag [num] = true;
				posTamagoSpeedY [num] = (posTamagoTargetY [num] - posTamago [num].y) / _randSpeed;
			}
		}

		private void TamagoPairCheck(int num){
			int _target = posTamagoTargetType [num];
			bool _flag = false;

			if ((_target < 4) && (posTamago[num].x == posTamagoTargetX[num]) && (posTamago[num].y == posTamagoTargetY[num])) {
				if (num < 4) {
					for (int i = 4; i < 8; i++) {
						if ((countTableChakusekiTime [i] != 0) && (posTamagoSpeedX [i] == 0) && (posTamagoSpeedY [i] == 0) && (_target == posTamagoTargetType[i]) && (posTamago[i].x == posTamagoTargetX[i]) && (posTamago[i].y == posTamagoTargetY[i])) {
							TamagochiAnimeSet (i, MotionLabel.GLAD1);
							TamagochiAnimeSet (num, MotionLabel.GLAD1);

							if (countTableChakusekiTime [num] < countTableChakusekiTime [i]) {
								if (countTableChakusekiTime [num] != 0) {
									countTableChakusekiTime [i] = countTableChakusekiTime [num];
								}
							} else {
								if (countTableChakusekiTime [i] != 0) {
									countTableChakusekiTime [num] = countTableChakusekiTime [i];
								}
							}
#if false
							if (countTableChakusekiTime [num] < countTableChakusekiTime [i]) {
								if (countTableChakusekiTime [num] != 0) {
									TamagochiFukidashiSet (num, tamagochiFukidashiType.HEART);		// 吹き出しハートを表示
									TamagochiFukidashiSet (i, tamagochiFukidashiType.HEART);		// 吹き出しハートを表示
									fukidashiTamagoWait [num] = countTableChakusekiTime [num];
									fukidashiTamagoWait [i] = countTableChakusekiTime [num];
								}
							} else {
								if (countTableChakusekiTime [i] != 0) {
									TamagochiFukidashiSet (num, tamagochiFukidashiType.HEART);		// 吹き出しハートを表示
									TamagochiFukidashiSet (i, tamagochiFukidashiType.HEART);		// 吹き出しハートを表示
									fukidashiTamagoWait [num] = countTableChakusekiTime [i];
									fukidashiTamagoWait [i] = countTableChakusekiTime [i];
								}
							}
#endif
							_flag = true;
						}
					}
				} else {
					for (int i = 0; i < 4; i++) {
						if ((countTableChakusekiTime [i] != 0) && (posTamagoSpeedX [i] == 0) && (posTamagoSpeedY [i] == 0) && (_target == posTamagoTargetType[i]) && (posTamago[i].x == posTamagoTargetX[i]) && (posTamago[i].y == posTamagoTargetY[i])) {
							TamagochiAnimeSet (i, MotionLabel.GLAD1);
							TamagochiAnimeSet (num, MotionLabel.GLAD1);

							if (countTableChakusekiTime [num] < countTableChakusekiTime [i]) {
								if (countTableChakusekiTime [num] != 0) {
									countTableChakusekiTime [i] = countTableChakusekiTime [num];
								}
							} else {
								if (countTableChakusekiTime [i] != 0) {
									countTableChakusekiTime [num] = countTableChakusekiTime [i];
								}
							}
#if false
							if (countTableChakusekiTime [num] < countTableChakusekiTime [i]) {
								if (countTableChakusekiTime [num] != 0) {
									TamagochiFukidashiSet (num, tamagochiFukidashiType.HEART);		// 吹き出しハートを表示
									TamagochiFukidashiSet (i, tamagochiFukidashiType.HEART);		// 吹き出しハートを表示
									fukidashiTamagoWait [num] = countTableChakusekiTime [num];
									fukidashiTamagoWait [i] = countTableChakusekiTime [num];
								}
							} else {
								if (countTableChakusekiTime [i] != 0) {
									TamagochiFukidashiSet (num, tamagochiFukidashiType.HEART);		// 吹き出しハートを表示
									TamagochiFukidashiSet (i, tamagochiFukidashiType.HEART);		// 吹き出しハートを表示
									fukidashiTamagoWait [num] = countTableChakusekiTime [i];
									fukidashiTamagoWait [i] = countTableChakusekiTime [i];
								}
							}
#endif
							_flag = true;
						}
					}
				}
			}

			if (_flag) {
				switch (_target) {
				case	0:
					{
						EventAppeal.transform.Find ("table1/heart").gameObject.SetActive (true);
						break;
					}
				case	1:
					{
						EventAppeal.transform.Find ("table2/heart").gameObject.SetActive (true);
						break;
					}
				case	2:
					{
						EventAppeal.transform.Find ("table3/heart").gameObject.SetActive (true);
						break;
					}
				case	3:
					{
						EventAppeal.transform.Find ("table4/heart").gameObject.SetActive (true);
						break;
					}
				}
			}
		}
		private void TamagoCakeCheck (int num){
			int _target = posTamagoTargetType [num];

			if ((_target == 4) && (posTamago [num].x == posTamagoTargetX [num]) && (posTamago [num].y == posTamagoTargetY [num]) && (countTableChakusekiTime[num] != 0)) {
				TamagochiFukidashiSet (num, tamagochiFukidashiType.CAKE);		// 吹き出しケーキを表示
				fukidashiTamagoWait[num] = countTableChakusekiTime[num];
			}
		}



		private void TamagochiFukidashiOff(){
			for (int i = 0; i < 8; i++) {
				TamagochiFukidashiSet (i, tamagochiFukidashiType.OFF);
			}
		}


		private enum tamagochiFukidashiType{
			CAKE,
			HEART,
			MESSAGE,
			OFF,
		};

		private void TamagochiFukidashiSet(int num,tamagochiFukidashiType type){
			switch (type) {
			case	tamagochiFukidashiType.CAKE:
				{
					CharaTamagochi [num].transform.Find ("fukidashi/cake").gameObject.SetActive (true);		// 吹き出しケーキを表示
					CharaTamagochi [num].transform.Find ("fukidashi/heart").gameObject.SetActive (false);	// 吹き出しハートを消す
					CharaTamagochi [num].transform.Find ("fukidashi/message").gameObject.SetActive (false);	// 吹き出しコメントを消す
					CharaTamagochi [num].transform.Find ("fukidashi/stamp").gameObject.SetActive (false);	// 吹き出しスタンプを消す
					break;
				}
			case	tamagochiFukidashiType.HEART:
				{
					CharaTamagochi [num].transform.Find ("fukidashi/cake").gameObject.SetActive (false);	// 吹き出しケーキを消す
					CharaTamagochi [num].transform.Find ("fukidashi/heart").gameObject.SetActive (true);	// 吹き出しハートを表示
					CharaTamagochi [num].transform.Find ("fukidashi/message").gameObject.SetActive (false);	// 吹き出しコメントを消す
					CharaTamagochi [num].transform.Find ("fukidashi/stamp").gameObject.SetActive (false);	// 吹き出しスタンプを消す
					break;
				}
			case	tamagochiFukidashiType.MESSAGE:
				{
					CharaTamagochi [num].transform.Find ("fukidashi/cake").gameObject.SetActive (false);	// 吹き出しケーキを消す
					CharaTamagochi [num].transform.Find ("fukidashi/heart").gameObject.SetActive (false);	// 吹き出しハートを消す
					CharaTamagochi [num].transform.Find ("fukidashi/message").gameObject.SetActive (true);	// 吹き出しコメントを表示
					CharaTamagochi [num].transform.Find ("fukidashi/stamp").gameObject.SetActive (false);	// 吹き出しスタンプを消す
					break;
				}
			case	tamagochiFukidashiType.OFF:
				{
					CharaTamagochi [num].transform.Find ("fukidashi/cake").gameObject.SetActive (false);	// 吹き出しケーキを消す
					CharaTamagochi [num].transform.Find ("fukidashi/heart").gameObject.SetActive (false);	// 吹き出しハートを消す
					CharaTamagochi [num].transform.Find ("fukidashi/message").gameObject.SetActive (false);	// 吹き出しコメントを消す
					CharaTamagochi [num].transform.Find ("fukidashi/stamp").gameObject.SetActive (false);	// 吹き出しスタンプを消す
					break;
				}
			}
		}




		// 告白タイムの初期配置
		private void KokuhakuPositionInit(){
			float[,] pos = new float[8, 2] {
				{  200.0f,   40.0f },									// 男の子１の初期位置
				{  230.0f,  -40.0f },									// 男の子２の初期位置
				{  260.0f, -120.0f },									// 男の子３の初期位置
				{  290.0f, -200.0f },									// 男の子４の初期位置
				{ -200.0f,   40.0f },									// 女の子１の初期位置
				{ -230.0f,  -40.0f },									// 女の子２の初期位置
				{ -260.0f, -120.0f },									// 女の子３の初期位置
				{ -290.0f, -200.0f },									// 女の子４の初期位置
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
				CharaTamagoFlipChange (i, flag);

				posTamago [i].x = pos [i, 0];
				posTamago [i].y = pos [i, 1];

				cbTamagoChara [i].gotoAndPlay (MotionLabel.IDLE);
			}

			CharaTamagochi [0].GetComponent<Canvas> ().sortingOrder = 1;
			CharaTamagochi [1].GetComponent<Canvas> ().sortingOrder = 2;
			CharaTamagochi [2].GetComponent<Canvas> ().sortingOrder = 3;
			CharaTamagochi [3].GetComponent<Canvas> ().sortingOrder = 4;
			CharaTamagochi [4].GetComponent<Canvas> ().sortingOrder = 1;
			CharaTamagochi [5].GetComponent<Canvas> ().sortingOrder = 2;
			CharaTamagochi [6].GetComponent<Canvas> ().sortingOrder = 3;
			CharaTamagochi [7].GetComponent<Canvas> ().sortingOrder = 4;
		}
	
		// カーテンのY座標を上下させる
		// num:移動速度（マイナス:クローズ、プラス:オープン）
		private bool EventCurtainPositionChange(float num){
			Vector3 pos;

			pos = EventCurtain.transform.localPosition;
			pos.y += (num * (60 * Time.deltaTime));
			EventCurtain.transform.localPosition = pos;

			if (num > 0) {
				if (EventCurtain.transform.localPosition.y >= 1220.0f) {
					pos.y = 1220.0f;
					EventCurtain.transform.localPosition = pos;
					return true;
				}
			} else {
				if (EventCurtain.transform.localPosition.y <= 0.0f) {
					pos.y = 0.0f;
					EventCurtain.transform.localPosition = pos;
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

			maskdatas [sceneNumber].askIndex = targetNumber;							// 相談相手ののメンバーindex(0～7)
			maskdatas [sceneNumber].askUid = mpdata.members [targetNumber].user.id;		// 相談相手のユーザーID
			maskdatas [sceneNumber].result = 2;											// 相談返答 時間切れ:2
		}

		// flag:表示フラグ（true:表示、false:非表示）
		private void EventSoudanPartsDisp (bool flag){
			EventSoudanYesNew.SetActive (flag);
			EventSoudanNoNew.SetActive (flag);

			EventSoudanAvaterNameNew.SetActive (flag);
			EventSoudanTamagoNameNew.SetActive (flag);

			EventSoudanKumo.SetActive (flag);

			EventSoudanTamago.transform.Find ("target").gameObject.SetActive (flag);
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


			tamagoMes = mpdata.members [targetNumber].user.GetCharaAt (mkindTable[targetNumber]).cname;
			avaterMes = mpdata.members [targetNumber].user.nickname;

			if (tamagoMes == "" || tamagoMes == null) {
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
			}

			EventSoudanAvaterNameNew.transform.Find ("text").gameObject.GetComponent<Text> ().text = avaterMes;
			EventSoudanTamagoNameNew.transform.Find ("text").gameObject.GetComponent<Text> ().text = tamagoMes;
		}



		// 告白する相手の番号
		private int[] KokuhakuManToWomanTable = new int[4] { 0, 1, 2, 3 };
		private int[] KokuhakuSeikouNumber = new int[4]{ -1, -1, -1, -1 };
		private void KokuhakuTimeInit(){
			// 男の子の告白する相手の番号をPartyResultDataから抽出する
			for (int i = 0; i < prdata.ansList.Count; i++) {
				KokuhakuManToWomanTable [prdata.ansList [i].firstIndex] = prdata.ansList [i].targetIndex - 4;
				if (prdata.ansList [i].rivalIndexs != null) {
					for (int i2 = 0; i2 < prdata.ansList [i].rivalIndexs.Length; i2++) {
						KokuhakuManToWomanTable [prdata.ansList [i].rivalIndexs [i2]] = prdata.ansList [i].targetIndex - 4;
					}
				}
				KokuhakuSeikouNumber [prdata.ansList [i].targetIndex - 4] = prdata.ansList [i].successIndex;
			}
/*			
			KokuhakuManToWomanTable[0] = KokuhakuTimeObjectSelect (0);		// 男の子１の告白する相手の番号を登録
			KokuhakuManToWomanTable[1] = KokuhakuTimeObjectSelect (1);		// 男の子２の告白する相手の番号を登録
			KokuhakuManToWomanTable[2] = KokuhakuTimeObjectSelect (2);		// 男の子３の告白する相手の番号を登録
			KokuhakuManToWomanTable[3] = KokuhakuTimeObjectSelect (3);		// 男の子４の告白する相手の番号を登録
*/
			kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount000;
		}

		private int kokuhakuWaitTime = 0;
		private int[] kokuhakuManTable = new int[4]{ 0, 1, 2, 3 };			// 告白する男の子の番号テーブル（終了したら２５５になる）
		private int kokuhakuManNumber = 0;									// 現在告白している男の子のテーブル番号
		private bool kokuhakuOkFlag = false;
		private int kokuhakuRivalNumber = 0;								// ライバルが出現するかどうか（16進数で考えて２番目がライバル10h、３番目がライバル20h、４番目がライバル40h）
		private float heartSize = 0.0f;										// 告白の成否判定の時に表示するハートのサイズ
		private string[] kokuhakuManMessage = new string[4];
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

					kokuhakuManMessage [0] = MesDisp.KokuhakuMesDispMan (mpdata.members [0].user.GetCharaAt (mkindTable[0]).wend);	// 告白宣言メッセージをここで決定
					kokuhakuManMessage [1] = MesDisp.KokuhakuMesDispMan (mpdata.members [1].user.GetCharaAt (mkindTable[1]).wend);
					kokuhakuManMessage [2] = MesDisp.KokuhakuMesDispMan (mpdata.members [2].user.GetCharaAt (mkindTable[2]).wend);
					kokuhakuManMessage [3] = MesDisp.KokuhakuMesDispMan (mpdata.members [3].user.GetCharaAt (mkindTable[3]).wend);
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount010:
				{
					if (kokuhakuManTable [kokuhakuManNumber] == 255) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount380;			// ライバル出現したので通常告白はなし
						break;
					}

					StartCoroutine (KokuhakuAttackIdou(kokuhakuManTable [kokuhakuManNumber],true,true));	// 男の子は一歩前に移動する
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount020;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount020:
				{
					if (_KokuhakuAttackIdouFlag) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount030;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount030:
				{
					StartCoroutine (KokuhakuAttackIdou(KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4,false,false));	// 告白指定された女の子は一歩前に移動する
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount040;

					ManagerObject.instance.sound.stopBgm ();
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount040:
				{
					if (_KokuhakuAttackIdouFlag) {
						kokuhakuRivalNumber = KokuhakuRivalCheck ();							// ライバル出現チェック
						if (kokuhakuRivalNumber == 0) {
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount170;		// ライバル出現なし
						} else {
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount050;		// ライバル出現あり
							StartCoroutine (TamagochiRivalJump (kokuhakuManNumber));
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
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount090;
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
					StartCoroutine (KokuhakuAttackIdou (1, true, false));						// ２番目のたまごっち、ライバルの男の子は一歩前に
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount080;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount080:
				{
					if (_KokuhakuAttackIdouFlag) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount090;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount090:
				{
					if ((kokuhakuRivalNumber & 32) != 0) {
						KokuhakuRivalWaitStop (2, true);										// ちょっと待ったを表示（初期化込み）
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount100;			// ３番目のたまごっちがライバルとして行動
						if ((kokuhakuRivalNumber & 16) != 0) {
							StartCoroutine (TamagochiRivalJump (1));
						}
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount130;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount100:
				{
					KokuhakuRivalWaitStop (2, false);											// ちょっと待ったを表示
					if (KokuhakuWaitTimeSubLoop ()) {
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
						EventWaitStop.SetActive (false);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount110;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount110:
				{
					StartCoroutine (KokuhakuAttackIdou (2, true, false));						// ３番目のたまごっち、ライバルの男の子は一歩前に
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount120;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount120:
				{
					if (_KokuhakuAttackIdouFlag) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount130;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount130:
				{
					if ((kokuhakuRivalNumber & 64) != 0) {
						KokuhakuRivalWaitStop (3, true);										// ちょっと待ったを表示（初期化込み）
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount140;			// ４番目のたまごっちがライバルとして行動
						if ((kokuhakuRivalNumber & 16) != 0) {
							if (cbTamagoChara [1].nowlabel != MotionLabel.SHOCK) {
								StartCoroutine (TamagochiRivalJump (1));
							}
						}
						if ((kokuhakuRivalNumber & 32) != 0) {
							if (cbTamagoChara [2].nowlabel != MotionLabel.SHOCK) {
								StartCoroutine (TamagochiRivalJump (2));
							}
						}
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount170;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount140:
				{
					KokuhakuRivalWaitStop (3, false);											// ちょっと待ったを表示
					if (KokuhakuWaitTimeSubLoop ()) {
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
						EventWaitStop.SetActive (false);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount150;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount150:
				{
					StartCoroutine (KokuhakuAttackIdou (3, true, false));						// ４番目のたまごっち、ライバルの男の子は一歩前に
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount160;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount160:
				{
					if (_KokuhakuAttackIdouFlag) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount170;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount170:
				{
					kokuhakuWaitTime = 60;
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount180;

					{
						for (int i = 0; i < 4; i++) {
							if (cbTamagoChara [i].nowlabel == MotionLabel.SHOCK) {
								// ライバル出現でSHOCK状態になっているキャラをデフォルトに戻す。
								TamagochiPatanChange (0, i);	// 告白者のアニメパターンを登録
							}
						}
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount180:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount190;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount190:
				{
					StartCoroutine (KokuhakuAttackIdou2 (kokuhakuManTable [kokuhakuManNumber]));	// 告白者を一歩前に
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount200;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount200:
				{
					if(_KokuhakuAttackIdouFlag){
						if (kokuhakuRivalNumber != 0) {
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount210;		// ライバル出現あり
						} else {
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount300;		// ライバル出現なし
						}
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount210:
				{
					if ((kokuhakuRivalNumber & 16) != 0) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount220;			// ２番目のたまごっちがライバル登録されている
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount240;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount220:
				{
					StartCoroutine (KokuhakuAttackIdou2 (1));									// ２番目のたまごっちをもう一歩前に
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount230;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount230:
				{
					if(_KokuhakuAttackIdouFlag){
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount240;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount240:
				{
					if ((kokuhakuRivalNumber & 32) != 0) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount250;			// ３番目のたまごっちがライバル登録されている
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount270;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount250:
				{
					StartCoroutine (KokuhakuAttackIdou2 (2));									// ３番目のたまごっちをもう一歩前に
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount260;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount260:
				{
					if(_KokuhakuAttackIdouFlag){
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount270;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount270:
				{
					if ((kokuhakuRivalNumber & 64) != 0) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount280;			// ４番目のたまごっちがライバル登録されている
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount300;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount280:
				{
					StartCoroutine (KokuhakuAttackIdou2 (3));									// ４番目のたまごっちをもう一歩前に
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount290;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount290:
				{
					if(_KokuhakuAttackIdouFlag){
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount300;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount300:
				{
					kokuhakuWaitTime = 30;
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount310;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount310:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						LoveParamCheck ();														// 告白判定
						if (loveParamFlag) {
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount320;
							kokuhakuWaitTime = 0;
							StartCoroutine ("KokuhakuHartIdou");
						} else {
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount330;
						}
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount320:
				{
					if (kokuhakuWaitTime != 0) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount330;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount330:
				{
					FukidashiMessageKokuhakuReturn (KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]], loveParamFlag);		// 告白の返事を表示
					CharaTamagochi [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].transform.Find ("fukidashi/comment").gameObject.SetActive (true);
					cbTamagoChara [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].gotoAndPlay (MotionLabel.GLAD1);

					if (loveParamManNumber != -1) {
						// 告白成功者がいるので喜ぶ
						TamagochiPatanChange (2, KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4);
						TamagochiPatanChange (2, loveParamManNumber);
					} else {
						// 告白成功者がいないので女の子は普通に・・・
						cbTamagoChara [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].gotoAndPlay (MotionLabel.IDLE);
					}

					heartSize = 0.0f;
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount340;
					kokuhakuWaitTime = 120;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount340:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						LoveResultDisp (true);													// 告白結果のハートなどを表示
						CharaTamagochi[KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].transform.Find("fukidashi/comment").gameObject.SetActive(false);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount350;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount350:
				{
					if (LoveResultDispHeart ()) {												// 告白結果のハートを拡大表示
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount360;
						kokuhakuWaitTime = 120;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount360:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						LoveResultDisp (false);
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
						CharaTamagoFlipChange (KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4, false);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount370;

						StartCoroutine ("KokuhakuReturnIdou");
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount370:
				{
					if (_KokuhakuReturnIdouFlag) {
						CharaTamagoFlipChange (KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4, true);
						if (loveParamFlag) {
							cbTamagoChara[KokuhakuManToWomanTable[kokuhakuManTable[kokuhakuManNumber]] + 4].gotoAndPlay(MotionLabel.GLAD1);
							cbTamagoChara [loveParamManNumber].gotoAndPlay (MotionLabel.GLAD1);

							kokuhakuOkFlag = true;													// 告白成功者がいる
							if ((playerNumber == KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4) || (playerNumber == loveParamManNumber)) {
								playerResultFlag = true;
								if (playerNumber == KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4) {
									muser2 = mpdata.members [loveParamManNumber].user;
									mkind2 = mkindTable [loveParamManNumber];
								} else {
									muser2 = mpdata.members [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].user;
									mkind2 = mkindTable [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4];
								}
							}
						} else {
							cbTamagoChara [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].gotoAndPlay (MotionLabel.IDLE);
						}
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount380;
					}

					break;
				}

			case	statusKokuhakuCount.kokuhakuCount380:
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
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount390;
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount010;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount390:
				{
					ManagerObject.instance.sound.playBgm (11);

					if (kokuhakuOkFlag) {														// 告白成功者がいる
						EventLastResult.transform.Find ("yes").gameObject.SetActive (true);
						EventLastResult.transform.Find ("yes/panel").gameObject.SetActive (true);
						Vector3 pos = EventLastResult.transform.Find ("yes/panel").gameObject.transform.localPosition;
						pos.y = -1100.0f;
						EventLastResult.transform.Find ("yes/panel").gameObject.transform.localPosition = pos;
					} else {																	// 告白成功者がいない
						EventLastResult.transform.Find ("no").gameObject.SetActive (true);
						EventLastResult.transform.Find ("no/panel").gameObject.SetActive (true);
						Vector3 pos = EventLastResult.transform.Find ("no/panel").gameObject.transform.localPosition;
						pos.y = -1100.0f;
						EventLastResult.transform.Find ("no/panel").gameObject.transform.localPosition = pos;
					}
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount400;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount400:
				{
					Vector3 pos = new Vector3 (1.0f, 1.0f, 1.0f);
					if (kokuhakuOkFlag) {														// 告白成功者がいる
						pos = EventLastResult.transform.Find ("yes/panel").gameObject.transform.localPosition;
					} else {																	// 告白成功者がいない
						pos = EventLastResult.transform.Find ("no/panel").gameObject.transform.localPosition;
					}
					pos.y += (15.0f * (60 * Time.deltaTime));
					if (pos.y >= 2300.0f) {
						pos.y = 2300.0f;
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount410;
					}

					if (kokuhakuOkFlag) {														// 告白成功者がいる
						EventLastResult.transform.Find ("yes/panel").gameObject.transform.localPosition = pos;
					} else {																	// 告白成功者がいない
						EventLastResult.transform.Find ("no/panel").gameObject.transform.localPosition = pos;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount410:
				{
					if (kokuhakuOkFlag) {														// 告白成功者がいる
						EventLastResult.transform.Find ("yes").gameObject.SetActive (false);
					} else {																	// 告白成功者がいない
						EventLastResult.transform.Find ("no").gameObject.SetActive (false);
					}
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount420;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount420:
				{
					kokuhakuTimeEndFlag = true;													// 告白タイム終了
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount430:
				{
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount440:
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
		private void KokuhakuMessageDisp(int num,bool flag){
			CharaTamagochi [num].transform.Find ("fukidashi/comment/text").gameObject.GetComponent<Text> ().text = kokuhakuManMessage [num];
			CharaTamagochi [num].transform.Find ("fukidashi/comment").gameObject.SetActive (flag);	// 吹き出しコメントを表示・非表示
			if (flag) {
				TamagochiPatanChange (0, num);														// 告白者のアニメパターンを登録
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp11);
			} else {
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
			}
		}

		private void KokuhakuEnd(){
		}
/*
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
*/
		// たまごっちキャラクターのランダムアニメ
		// patanNum:アニメ種類、tamagoNum:アニメさせるたまごっちの番号（０〜７）
		private void TamagochiPatanChange(int patanNum,int tamagoNum){
			switch (patanNum) {
			case	0:
				{	// 告白者のアニメパターン
					switch (Random.Range (0, 3)) {
					case	0:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay (MotionLabel.GLAD2);			// 喜び２
							break;
						}
					case	1:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay (MotionLabel.SHY1);			// 照れ１
							break;
						}
					case	2:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay (MotionLabel.SHY3);			// 照れ３
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
							cbTamagoChara [tamagoNum].gotoAndPlay (MotionLabel.SHY1);			// 照れ１
							break;
						}
					case	1:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay (MotionLabel.SHY2);			// 照れ２
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
							cbTamagoChara [tamagoNum].gotoAndPlay (MotionLabel.GLAD1);			// 喜び１
							break;
						}
					case	1:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay (MotionLabel.GLAD2);			// 喜び２
							break;
						}
					case	2:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay (MotionLabel.GLAD3);			// 喜び３
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
							cbTamagoChara [tamagoNum].gotoAndPlay (MotionLabel.GLAD1);			// 喜び１
							break;
						}
					case	1:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay (MotionLabel.GLAD2);			// 喜び２
							break;
						}
					case	2:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay (MotionLabel.SHY1);			// 照れ１
							break;
						}
					case	3:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay (MotionLabel.SHY2);			// 照れ２
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
				ManagerObject.instance.sound.playSe (7);

				EventWaitStop.SetActive (true);													// ちょっと待ったの帯を表示
				cbTamagoChara [num].gotoAndPlay (MotionLabel.ANGER);
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp12);
				kokuhakuWaitTime = 120;
			}
			TamagochiImageMove (EventWaitStop, CharaTamago [num], "panel/image/");
		}
			
		// 告白判定
		private bool loveParamFlag = true;
		private int loveParamManNumber = 0;
		private bool[] loveParamManCryFlag = new bool[4] { false, false, false, false };
		private void LoveParamCheck(){
			for (int i = 0; i < 4; i++) {
				loveParamManCryFlag [i] = false;
			}

			loveParamManNumber = -1;

//			if(loveManWomanFix[kokuhakuManTable [kokuhakuManNumber],KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]] <= loveManWoman[kokuhakuManTable [kokuhakuManNumber],KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]]){
			if(KokuhakuSeikouNumber[KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]] == kokuhakuManTable[kokuhakuManNumber]){
				// 告白成功
				loveParamFlag = true;
				loveParamManNumber = kokuhakuManTable [kokuhakuManNumber];
				TamagochiPatanChange (2, kokuhakuManTable [kokuhakuManNumber]);					// 告白成功時のアニメパターンを登録
			}
			else{
				// 告白失敗
				loveParamFlag = false;
				cbTamagoChara [kokuhakuManTable [kokuhakuManNumber]].gotoAndPlay (MotionLabel.CRY);
				loveParamManCryFlag [kokuhakuManTable [kokuhakuManNumber]] = true;
			}
				
			if(kokuhakuRivalNumber != 0){
				if((kokuhakuRivalNumber & 16) != 0){
//					if((loveManWomanFix[1,KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]] <= loveManWoman[1,KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]]) && (!loveParamFlag)){
					if(KokuhakuSeikouNumber[KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]] == 1){
						// 告白成功
						loveParamFlag = true;
						loveParamManNumber = 1;
						TamagochiPatanChange (2, 1);											// 告白成功時のアニメパターンを登録
					}
					else{
						// 告白失敗
						cbTamagoChara [1].gotoAndPlay (MotionLabel.CRY);
						loveParamManCryFlag [1] = true;
					}
				}
				if((kokuhakuRivalNumber & 32) != 0){
//					if((loveManWomanFix[2,KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]] <= loveManWoman[2,KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]]) && (!loveParamFlag)){
					if(KokuhakuSeikouNumber[KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]] == 2){
						// 告白成功
						loveParamFlag = true;
						loveParamManNumber = 2;
						TamagochiPatanChange (2, 2);											// 告白成功時のアニメパターンを登録
					}
					else{
						// 告白失敗
						cbTamagoChara [2].gotoAndPlay (MotionLabel.CRY);
						loveParamManCryFlag [2] = true;
					}
				}
				if((kokuhakuRivalNumber & 64) != 0){
//					if((loveManWomanFix[3,KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]] <= loveManWoman[3,KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]]) && (!loveParamFlag)){
					if(KokuhakuSeikouNumber[KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]] == 3){
						// 告白成功
						loveParamFlag = true;
						loveParamManNumber = 3;
						TamagochiPatanChange (2, 3);											// 告白成功時のアニメパターンを登録
					}
					else{
						// 告白失敗
						cbTamagoChara [3].gotoAndPlay (MotionLabel.CRY);
						loveParamManCryFlag [3] = true;
					}
				}
			}
		}
		// flag:表示フラグ（true:表示、false:非表示）
		private void LoveResultDisp(bool flag){
			if (flag) {
				if (loveParamFlag) {
					// 告白成功
					EventResult.transform.Find ("yes").gameObject.SetActive (true);

					ManagerObject.instance.sound.playJingle (14);
				} else {
					// 告白失敗
					EventResult.transform.Find ("no").gameObject.SetActive (true);

					ManagerObject.instance.sound.playJingle (15);
				}
			} else {
				Vector3 pos = new Vector3 (0.0f, 0.0f, 0.0f);

				EventResult.transform.Find ("yes").gameObject.SetActive (false);
				EventResult.transform.Find ("yes/heart").gameObject.transform.localScale = pos;
				EventResult.transform.Find ("no").gameObject.SetActive (false);
				EventResult.transform.Find ("no/heart").gameObject.transform.localScale = pos;
/*
				if (loveParamFlag) {
					EventResult.transform.Find("hart_pt").gameObject.SetActive(true);
					EventResult.transform.Find ("hart_pt").gameObject.GetComponent<ParticleSystem> ().Play ();
				}
*/				
			}
		}
		private bool LoveResultDispHeart(){
			bool retFlag = false;
			Vector3 pos = new Vector3 (1.0f, 1.0f, 1.0f);

			heartSize += 0.1f;
			if (heartSize >= 4.0f) {
				heartSize = 4.0f;
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



		// 画面下から入場して整列位置に停止するまで
		private bool _openningTamagoIdouFlag;
		private IEnumerator OpenningTamagoIdou(){
			Vector3[] _openningTamagoPositionTable = new Vector3[] {
				new Vector3(   90.0f, -200.0f, 0.0f),						// 男の子１の整列位置
				new Vector3(  175.0f, -180.0f, 0.0f),						// 男の子２の整列位置
				new Vector3(  260.0f, -160.0f, 0.0f),						// 男の子３の整列位置
				new Vector3(  345.0f, -140.0f, 0.0f),						// 男の子４の整列位置
				new Vector3(  -90.0f, -200.0f, 0.0f),						// 女の子１の整列位置
				new Vector3( -175.0f, -180.0f, 0.0f),						// 女の子２の整列位置
				new Vector3( -260.0f, -160.0f, 0.0f),						// 女の子３の整列位置
				new Vector3( -345.0f, -140.0f, 0.0f),						// 女の子４の整列位置
			};

			Vector3 _pos = new Vector3 (0.0f, 0.0f, 0.0f);
			_openningTamagoIdouFlag = false;

			StartCoroutine ("OpenningTamagoSort");

			cbTamagoChara [posNumber].gotoAndPlay (MotionLabel.WALK);
			switch (posNumber) {
			case	0:
			case	1:
			case	2:
			case	3:
				{
					CharaTamagoFlipChange (posNumber, false);
					break;
				}
			case	4:
			case	5:
			case	6:
			case	7:
				{
					CharaTamagoFlipChange (posNumber, true);
					break;
				}
			}

			while (true) {																		// 画面下から画面中央まで入場
				posTamago [posNumber] = Vector3.MoveTowards (posTamago [posNumber], _pos, (200.0f * Time.deltaTime));
				if (posTamago [posNumber].y == _pos.y) {
					break;
				}
				yield return null;
			}

			cbTamagoChara [posNumber].gotoAndPlay (MotionLabel.IDLE);

			yield return new WaitForSeconds (0.1f);

			ManagerObject.instance.sound.playSe (24);

			_pos.y += 30.0f;
			while (true) {																		// ジャンプ上昇
				posTamago [posNumber] = Vector3.MoveTowards (posTamago [posNumber], _pos, (250.0f * Time.deltaTime));
				if (posTamago [posNumber].y == _pos.y) {
					break;
				}
				yield return null;
			}

			_pos.y -= 30.0f;
			while (true) {																		// ジャンプ下降
				posTamago [posNumber] = Vector3.MoveTowards (posTamago [posNumber], _pos, (250.0f * Time.deltaTime));
				if (posTamago [posNumber].y == _pos.y) {
					break;
				}
				yield return null;
			}

			FukidashiMessageSet ();																// 自己紹介メッセージ登録
/////			if (mpdata.members [posNumber].user.utype == UserType.MIX2) {
				CharaTamagochi [posNumber].transform.Find ("fukidashi/comment").gameObject.SetActive (true);
				yield return new WaitForSeconds (2.0f);
				CharaTamagochi [posNumber].transform.Find ("fukidashi/comment").gameObject.SetActive (false);
/////			} else {
/////				CharaTamagochi [posNumber].transform.Find ("fukidashi/stamp").gameObject.SetActive (true);
/////				yield return new WaitForSeconds (2.0f);
/////				CharaTamagochi [posNumber].transform.Find ("fukidashi/stamp").gameObject.SetActive (false);
/////			}

			cbTamagoChara [posNumber].gotoAndPlay (MotionLabel.WALK);							// 整列位置に移動
			switch (posNumber) {
			case	0:
			case	1:
			case	2:
			case	3:
				{
					CharaTamagoFlipChange (posNumber, true);
					break;
				}
			case	4:
			case	5:
			case	6:
			case	7:
				{
					CharaTamagoFlipChange (posNumber, false);
					break;
				}
			}
			while (true) {																		// 初期整列位置に移動
				posTamago [posNumber] = Vector3.MoveTowards (posTamago [posNumber], _openningTamagoPositionTable [posNumber], (200.0f * Time.deltaTime));
				if ((posTamago [posNumber].x == _openningTamagoPositionTable [posNumber].x) && (posTamago [posNumber].y == _openningTamagoPositionTable [posNumber].y)) {
					break;
				}
				yield return null;
			}
			switch (posNumber) {
			case	0:
			case	1:
			case	2:
			case	3:
				{
					CharaTamagoFlipChange (posNumber, false);
					break;
				}
			case	4:
			case	5:
			case	6:
			case	7:
				{
					CharaTamagoFlipChange (posNumber, true);
					break;
				}
			}
			cbTamagoChara [posNumber].gotoAndPlay (MotionLabel.IDLE);

			_openningTamagoIdouFlag = true;
		}
		private IEnumerator OpenningTamagoSort(){
			float[] _posY = new float[8];

			while (true) {
				if (_openningTamagoIdouFlag) {
					break;
				}

				for (int i = 0; i < 8; i++) {
					_posY[i] = posTamago [i].y;
				}

				for (int j = 0; j < 8; j++) {								// たまごっちの表示優先順位の変更
					int k = 0;
					float _checkPos = -1000.0f;
					for (int i = 0; i < 8; i++) {
						if (_checkPos <= _posY [i]) {
							_checkPos = _posY [i];
							k = i;
						}
					}
					_posY [k] = -1000.0f;
					CharaTamagochi [k].GetComponent<Canvas> ().sortingOrder = j + 1;
				}

				yield return null;
			}
		}



		// ライバルが出現してびっくりしてその場で少しジャンプする。
		private IEnumerator TamagochiRivalJump(int num){
			Vector3 _pos = posTamago [num];

			cbTamagoChara [num].gotoAndPlay (MotionLabel.SHOCK);

			_pos.y += 30.0f;
			while (true) {
				posTamago [num] = Vector3.MoveTowards (posTamago [num], _pos, (250.0f * Time.deltaTime));
				if (_pos.y == posTamago [num].y) {
					break;
				}
				yield return null;
			}
			_pos.y -= 30.0f;
			while (true) {
				posTamago [num] = Vector3.MoveTowards (posTamago [num], _pos, (250.0f * Time.deltaTime));
				if (_pos.y == posTamago [num].y) {
					break;
				}
				yield return null;
			}
			for (int i = 0; i < 2; i++) {
				_pos.y += 5.0f;
				while (true) {
					posTamago [num] = Vector3.MoveTowards (posTamago [num], _pos, (25.0f * Time.deltaTime));
					if (_pos.y == posTamago [num].y) {
						break;
					}
					yield return null;
				}
				_pos.y -= 5.0f;
				while (true) {
					posTamago [num] = Vector3.MoveTowards (posTamago [num], _pos, (25.0f * Time.deltaTime));
					if (_pos.y == posTamago [num].y) {
						break;
					}
					yield return null;
				}
			}
		}
			


		private bool _kokuhakuHeartJumpFlag;
		private IEnumerator KokuhakuHaertJump(){									// 少しだけその場でジャンプ
			Vector3 _pos = posTamago [loveParamManNumber];

			_kokuhakuHeartJumpFlag = true;

			cbTamagoChara [loveParamManNumber].gotoAndPlay (MotionLabel.GLAD1);

			_pos.y += 30.0f;
			while (true) {
				posTamago [loveParamManNumber] = Vector3.MoveTowards (posTamago [loveParamManNumber], _pos, (250.0f * Time.deltaTime));
				if (_pos.y == posTamago [loveParamManNumber].y) {
					break;
				}
				yield return null;
			}
			_pos.y -= 30.0f;
			while (true) {
				posTamago [loveParamManNumber] = Vector3.MoveTowards (posTamago [loveParamManNumber], _pos, (250.0f * Time.deltaTime));
				if (_pos.y == posTamago [loveParamManNumber].y) {
					break;
				}
				yield return null;
			}

			_kokuhakuHeartJumpFlag = false;
		}
		private IEnumerator KokuhakuHartIdou(){
			Vector3 _scale;
			Vector3 _pos;
			Vector3 _posMan;

			FukidashiMessageSetKokuhakuWoman (KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4, loveParamManNumber);
			CharaTamagochi[KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].transform.Find("fukidashi/comment").gameObject.SetActive(true);
			cbTamagoChara [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].gotoAndPlay (MotionLabel.SHY1);

			_scale = new Vector3 (0.0f, 0.0f, 0.0f);
			TamagoEffect.transform.Find ("Heart").gameObject.SetActive (true);
			_pos = posTamago [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4];
			TamagoEffect.transform.Find ("Heart").gameObject.transform.localPosition = _pos;

			while (true) {														// ハートを拡大
				_scale.x += (0.04f * (60 * Time.deltaTime));
				if (_scale.x >= 1.0f) {
					_scale.x = 1.0f;
				}
				_scale.y = _scale.z = _scale.x;
				TamagoEffect.transform.Find ("Heart").gameObject.transform.localScale = _scale;

				if (_scale.x == 1.0f) {
					break;
				}
				yield return null;
			}

			ManagerObject.instance.sound.playSe (31);

			while (true) {														// ハートを女の子から男の子へ飛ばす
				_pos = Vector3.MoveTowards (_pos, posTamago [loveParamManNumber], (200.0f * Time.deltaTime));
				TamagoEffect.transform.Find ("Heart").gameObject.transform.localPosition = _pos;
				if (_pos.x >= posTamago [loveParamManNumber].x) {
					break;
				}
				yield return null;
			}

			ManagerObject.instance.sound.playSe (24);
			StartCoroutine ("KokuhakuHaertJump");
			while (true) {														// ハートを縮小
				_scale.x -= (0.04f * (60 * Time.deltaTime));
				if (_scale.x <= 0.0f) {
					_scale.x = 0.0f;
				}
				_scale.y = _scale.z = _scale.x;
				TamagoEffect.transform.Find ("Heart").gameObject.transform.localScale = _scale;

				if (_scale.x == 0.0f) {
					break;
				}
				yield return null;
			}

			TamagoEffect.transform.Find ("Heart").gameObject.SetActive (false);
			CharaTamagochi [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].transform.Find ("fukidashi/comment").gameObject.SetActive (false);

			while (_kokuhakuHeartJumpFlag) {
				yield return null;
			}

			// 画面中央に男の子と女の子を集合させる
			cbTamagoChara [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].gotoAndPlay (MotionLabel.SHY4);
			cbTamagoChara [loveParamManNumber].gotoAndPlay (MotionLabel.SHY4);
			_pos = new Vector3 (-45.0f, -70.0f, 0.0f);
			_posMan = new Vector3 (45.0f, -70.0f, 0.0f);
			while (true) {
				posTamago [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4] = Vector3.MoveTowards (posTamago [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4], _pos, (200.0f * Time.deltaTime));
				posTamago [loveParamManNumber] = Vector3.MoveTowards (posTamago [loveParamManNumber], _posMan, (200.0f * Time.deltaTime));

				if ((posTamago [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].x == _pos.x) && (posTamago [loveParamManNumber].x == _posMan.x)) {
					break;
				}

				yield return null;
			}

			kokuhakuWaitTime = 1;
		}


		private Vector3[] _KokuhakuPositionTable = new Vector3[]{
			new Vector3(  200.0f,   40.0f, 0.0f),									// 男の子１の初期位置
			new Vector3(  230.0f,  -40.0f, 0.0f),									// 男の子２の初期位置
			new Vector3(  260.0f, -120.0f, 0.0f),									// 男の子３の初期位置
			new Vector3(  290.0f, -200.0f, 0.0f),									// 男の子４の初期位置
			new Vector3( -200.0f,   40.0f, 0.0f),									// 女の子１の初期位置
			new Vector3( -230.0f,  -40.0f, 0.0f),									// 女の子２の初期位置
			new Vector3( -260.0f, -120.0f, 0.0f),									// 女の子３の初期位置
			new Vector3( -290.0f, -200.0f, 0.0f),									// 女の子４の初期位置
		};

		private bool _KokuhakuAttackIdouFlag;
		// 一歩前に
		// num:たまごっち、sex:男女、mode:初期設定あり
		private IEnumerator KokuhakuAttackIdou(int _num,bool _sex,bool _mode){
			Vector3 _pos = _KokuhakuPositionTable [_num];
			if (_sex) {
				_pos.x -= 50.0f;
			} else {
				_pos.x += 50.0f;
			}
			_KokuhakuAttackIdouFlag = false;

			TamagochiAnimeSet (_num, MotionLabel.WALK);

			while (true) {
				posTamago [_num] = Vector3.MoveTowards (posTamago [_num], _pos, (100.0f * Time.deltaTime));
				if (posTamago [_num].x == _pos.x) {
					break;
				}
				yield return null;
			}

			if (_sex) {
				TamagochiPatanChange (0, _num);			// 告白者のアニメパターンを登録
			} else {
				TamagochiPatanChange (1, _num);			// 告白対象者のアニメパターンを登録
			}

			if (_mode) {
				FukidashiMessageSetKokuhaku (_num, KokuhakuManToWomanTable [_num] + 4);
				CharaTamagochi [_num].transform.Find ("fukidashi/comment").gameObject.SetActive (true);
				yield return new WaitForSeconds (2.0f);
				CharaTamagochi [_num].transform.Find ("fukidashi/comment").gameObject.SetActive (false);
			}

			_KokuhakuAttackIdouFlag = true;
		}
		// もう一歩前に
		// num:たまごっち
		private IEnumerator KokuhakuAttackIdou2(int _num){
			Vector3 _pos = _KokuhakuPositionTable [_num];
			_pos.x -= 100.0f;
			_KokuhakuAttackIdouFlag = false;

			TamagochiAnimeSet (_num, MotionLabel.WALK);

			while (true) {
				posTamago [_num] = Vector3.MoveTowards (posTamago [_num], _pos, (100.0f * Time.deltaTime));
				if (posTamago [_num].x == _pos.x) {
					break;
				}
				yield return null;
			}

			KokuhakuMessageDisp (_num, true);		// 吹き出しを表示
			yield return new WaitForSeconds(2.0f);
			KokuhakuMessageDisp (_num, false);		// 吹き出しを非表示

			_KokuhakuAttackIdouFlag = true;
		}

		private bool _KokuhakuReturnIdouFlag;
		private bool _kokuhakuWomanReturnFlag;
		private bool _kokuhakuManGotoWomanFlag;
		private bool _kokuhakuManReturnFlag;
		private IEnumerator KokuhakuReturnIdou(){
			_KokuhakuReturnIdouFlag = false;
			StartCoroutine ("KokuhakuWomanReturn");									// 告白を受けた女の子は、女の子の定位置に移動する
			StartCoroutine ("KokuhakuManGotoWoman");								// 告白成功した男の子は、女の子の定位置の右横に移動する
			StartCoroutine ("KokuhakuManReturn");									// 告白失敗した男の子は、男の子の定位置に戻る

			while (true) {
				if ((_kokuhakuWomanReturnFlag) && (_kokuhakuManGotoWomanFlag) && (_kokuhakuManReturnFlag)) {
					break;
				}
				yield return null;
			}

			_KokuhakuReturnIdouFlag = true;
		}
		private IEnumerator KokuhakuWomanReturn(){
			int _num = KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4;
			Vector3 _pos = _KokuhakuPositionTable [_num];
			if (loveParamFlag) {
				// 告白成功した女の子は通常位置より左に整列
				_pos.x -= 100.0f;
			}
			_kokuhakuWomanReturnFlag = false;
			while (true) {
				TamagochiAnimeSet (_num, MotionLabel.WALK);
				posTamago [_num] = Vector3.MoveTowards (posTamago [_num], _pos, (200.0f * Time.deltaTime));
				if (posTamago [_num].x == _pos.x) {
					break;
				}
				yield return null;
			}
			_kokuhakuWomanReturnFlag = true;
		}
		private IEnumerator KokuhakuManGotoWoman(){
			int _num = KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4;
			Vector3 _pos = _KokuhakuPositionTable [_num];
			_pos.x += 90.0f;
			_pos.x -= 100.0f;

			_kokuhakuManGotoWomanFlag = false;

			while (loveParamFlag) {
				TamagochiAnimeSet (loveParamManNumber, MotionLabel.WALK);
				posTamago [loveParamManNumber] = Vector3.MoveTowards (posTamago [loveParamManNumber], _pos, (200.0f * Time.deltaTime));
				if (posTamago [loveParamManNumber].x <= 0) {
					CharaTamagochi [loveParamManNumber].GetComponent<Canvas> ().sortingOrder = CharaTamagochi [_num].GetComponent<Canvas> ().sortingOrder;
				}
				if (posTamago [loveParamManNumber].x == _pos.x) {
					break;
				}
				yield return null;
			}

			_kokuhakuManGotoWomanFlag = true;
		}
		private IEnumerator KokuhakuManReturn(){
			int _retNum;
			_kokuhakuManReturnFlag = false;

			while (true) {
				_retNum = 0;
				for (int i = 0; i < 4; i++) {
					if (loveParamManCryFlag [i]) {
						posTamago [i] = Vector3.MoveTowards (posTamago [i], _KokuhakuPositionTable [i], (200.0f * Time.deltaTime));
						if (posTamago [i].x == _KokuhakuPositionTable [i].x) {
							_retNum++;
						}
					} else {
						_retNum++;
					}
				}
				if (_retNum == 4) {
					break;
				}
				yield return null;
			}

			_kokuhakuManReturnFlag = true;
		}

		// 相談タイムの表示
		private IEnumerator	SoudanTextDisp(statusJobCount num){
			float	panelWhiteA = 0.0f;
			while(true){
				panelWhiteA += (5.0f * (60 * Time.deltaTime));
				if(panelWhiteA >= 255.0f){
					panelWhiteA = 255.0f;
				}

				PrgCanvas.transform.Find("soudan/txt_soudan").gameObject.GetComponent<Image> ().color = new Color (1.0f, 1.0f, 1.0f, panelWhiteA / 255.0f);
				if (panelWhiteA == 255.0f) {
					break;
				}
				yield return null;
			}

			yield return new WaitForSeconds (2.0f);
	
			while(true){
				panelWhiteA -= (5.0f * (60 * Time.deltaTime));
				if(panelWhiteA <= 0.0f){
					panelWhiteA = 0.0f;
				}

				PrgCanvas.transform.Find("soudan/txt_soudan").gameObject.GetComponent<Image> ().color = new Color (1.0f, 1.0f, 1.0f, panelWhiteA / 255.0f);
				if (panelWhiteA == 0.0f) {
					break;
				}
				yield return null;
			}
			jobCount = num;

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




		private void ButtonFutagoChara1(){
			ManagerObject.instance.sound.playSe (13);

			buttonFutagoFlag = true;
			buttonFutagoNumber = 0;
		}
		private void ButtonFutagoChara2(){
			ManagerObject.instance.sound.playSe (13);

			buttonFutagoFlag = true;
			buttonFutagoNumber = 1;
		}





		private IEnumerator AppelTimeWait(){
			yield return new WaitForSeconds (APPEAL_LIMIT_TIME);
			buttonFlag = true;
		}

		// Yesボタンが押された時
		private void ButtneYesClick(){
			ManagerObject.instance.sound.playSe (13);

			LoveManWomanNumberSet (10);		// 好感度を１０上げる
			buttonFlag = true;
			//btnYesNo = true;
		}
		// Noボタンが押された時
		private void ButtonNoClick(){
			ManagerObject.instance.sound.playSe (14);

			LoveManWomanNumberSet (-10);	// 好感度を１０下げる
			buttonFlag = true;
			//btnYesNo = false;
		}

		// 好感度を上下させる（本当はサーバーでやるのが良いと思う）
		// num:好感度の上下値
		private void LoveManWomanNumberSet(int Num){
/*			
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
*/


			int _result;
			if (Num == 10) {
				_result = 1;
			} else {
				_result = 0;
			}

			maskdatas [sceneNumber].askIndex = targetNumber;							// 相談相手ののメンバーindex(0～7)
			maskdatas [sceneNumber].askUid = mpdata.members [targetNumber].user.id;		// 相談相手のユーザーID
			maskdatas [sceneNumber].result = _result;									// 相談返答 はい:1、いいえ:0
		}
	}
}
