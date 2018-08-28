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

		public ManagerObject manager;//ライブラリ

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

		//private object[]		mparam;

		private int playerNumber = 0;					// ０〜７（０〜３：男の子、４〜７：女の子）
		private int targetNumber = 0;					// ０〜７（０〜３：男の子、４〜７：女の子）
		private bool playerResultFlag = false;			// カップル成立:true 不成立:false

		private bool buttonFlag = false;
		//private bool btnYesNo = false;					// Yes:true No:false
		private int sceneNumber = 0;					// シーンナンバー
		//private bool screenMode = false;


		private bool kokuhakuTimeEndFlag = true;

		private CharaBehaviour[] cbTamagoChara = new CharaBehaviour[8];


		// 相性度（０〜１００）（男の子、女の子）
		private int[,] loveManWoman = new int[4, 4]{ { 40,40,40,40 }, { 40,40,40,40 }, { 40,40,40,40 }, { 40,40,40,40 } };
		// 成否判定の種（０〜１００）（男の子、女の子）
		private int[,] loveManWomanFix;

		private readonly float APPEAL_LIMIT_TIME = 5.0f;			// アピールタイムのキー入力待ち時間（秒）

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

		private PartyData mpdata;
		private AskData[] maskdatas;
		private bool mresult;
		private int mkind;
		private User muser1;
		private int mkind1;
		private User muser2;
		private int mkind2;

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
			manager.connect.send(call);
		}
		
		void mgetroominf(bool success,object data)
		{
			// dataの内容は設計書参照
			// dataを変更したいときはConnectManagerDriverのGetRoomInfo()を変更する
			mpdata = (PartyData)data;
			StartCoroutine(mstart());
		}

		void mgetroomres(bool success,object data)
		{
			mresult=true;
			// dataの内容は設計書参照
			// dataを変更したいときはConnectManagerDriverのGetRoomResult()を変更する
			PartyResultData prdata = (PartyResultData)data;
			loveManWoman = prdata.fixs;
			loveManWomanFix = prdata.loves;
		}

		IEnumerator mstart()
		{

			playerNumber = mpdata.playerIndex;
			muser1 = mpdata.members[playerNumber].user;
			mkind1 = mpdata.members[playerNumber].index;
			playerResultFlag = false;

			EventSoudanYesNew.GetComponent<Button> ().onClick.AddListener (ButtneYesClick);
			EventSoudanNoNew.GetComponent<Button> ().onClick.AddListener (ButtonNoClick);

			jobCount = statusJobCount.machiconJobCount000;


			float use_screen_x = Screen.currentResolution.width;
			float use_screen_y = Screen.currentResolution.height;
			#if UNITY_EDITOR
			use_screen_x = Screen.width;
			use_screen_y = Screen.height;
			#endif

			/*
			float num;
			if (use_screen_x >= use_screen_y) {
				num = use_screen_x / use_screen_y;
			} else {
				num = use_screen_y / use_screen_x;
			}

			if ((num > 1.33f) && (num < 1.34f)) {
				screenMode = false;													// 3:4
			} else {
				screenMode = true;													// 3:4以外
			}
			*/


			for (int i = 0; i < 8; i++) {
				cbTamagoChara [i] = CharaTamago [i].GetComponent<CharaBehaviour> ();
			}
			yield return cbTamagoChara[0].init (mpdata.members[0].user.GetCharaAt(mpdata.members[0].index));// 男の子１のたまごっちを登録する
			yield return cbTamagoChara[1].init (mpdata.members[1].user.GetCharaAt(mpdata.members[1].index));// 男の子２のたまごっちを登録する
			yield return cbTamagoChara[2].init (mpdata.members[2].user.GetCharaAt(mpdata.members[2].index));// 男の子３のたまごっちを登録する
			yield return cbTamagoChara[3].init (mpdata.members[3].user.GetCharaAt(mpdata.members[3].index));// 男の子４のたまごっちを登録する
			yield return cbTamagoChara[4].init (mpdata.members[4].user.GetCharaAt(mpdata.members[4].index));// 女の子１のたまごっちを登録する
			yield return cbTamagoChara[5].init (mpdata.members[5].user.GetCharaAt(mpdata.members[5].index));// 女の子２のたまごっちを登録する
			yield return cbTamagoChara[6].init (mpdata.members[6].user.GetCharaAt(mpdata.members[6].index));// 女の子３のたまごっちを登録する
			yield return cbTamagoChara[7].init (mpdata.members[7].user.GetCharaAt(mpdata.members[7].index));// 女の子４のたまごっちを登録する


		}
	
		void Destroy(){
			Debug.Log ("MachiCon Destroy");
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
					countTime1 = 0.0f;
					countTime2 = 10;

					MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp01);

					CharaTamagochi [0].GetComponent<Canvas> ().sortingOrder = 4;
					CharaTamagochi [1].GetComponent<Canvas> ().sortingOrder = 3;
					CharaTamagochi [2].GetComponent<Canvas> ().sortingOrder = 2;
					CharaTamagochi [3].GetComponent<Canvas> ().sortingOrder = 1;
					CharaTamagochi [4].GetComponent<Canvas> ().sortingOrder = 4;
					CharaTamagochi [5].GetComponent<Canvas> ().sortingOrder = 3;
					CharaTamagochi [6].GetComponent<Canvas> ().sortingOrder = 2;
					CharaTamagochi [7].GetComponent<Canvas> ().sortingOrder = 1;
					for (int i = 0; i < 8; i++) {
						posTamago [i] = new Vector3 (0.0f, -400.0f, 0.0f);						// たまごっちの初期位置
					}

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
					EventTitleText.GetComponent<Image> ().sprite = EventTitleSprite [countTime2];
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
					if (pos >= 0.0f) {
						jobCount = statusJobCount.machiconJobCount060;
						waitTime = FlameInPositionChangeJumpTable.Length;						// ジャンプの時間
						cbTamagoChara [posNumber].gotoAndPlay ("idle");
					}
					break;
				}
			case	statusJobCount.machiconJobCount060:
				{	// ジャンプ
					waitTime--;
					FlameInPositionChangeJump ();
					if (waitTime == 0) {
						jobCount = statusJobCount.machiconJobCount070;
						posTamago [posNumber].y = 0.0f;
						FukidashiMessageSet ();
						CharaTamagochi[posNumber].transform.Find("fukidashi/comment").gameObject.SetActive(true);
						waitTime = 60;
					}
					break;
				}
			case	statusJobCount.machiconJobCount070:
				{	// コメント表示
					if (WaitTimeSubLoop()) {
						jobCount = statusJobCount.machiconJobCount080;
						CharaTamagochi[posNumber].transform.Find("fukidashi/comment").gameObject.SetActive(false);
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
						waitTime = 60;

						TargetRandomSet ();														// 相談相手の決定

						EventAppeal.SetActive (false);
						EventAppealTableHeartClear ();											// テーブルハートを消しておく
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
						EventSoudanTamago.transform.Find ("chara").gameObject.SetActive (true);
						Vector3 pos = EventSoudanTamago.transform.localPosition;
						pos.y -= 800.0f;
						EventSoudanTamago.transform.localPosition = pos;
						soudanJumpFlag = false;
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

						EventSoudanPartsDisp (true);
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp07);

						StartCoroutine ("AppelTimeWait");										// APPEAL_LIMIT_TIME秒間でキー入力待ち終了
					}

					if (soudanJumpFlag) {
						SoudanTamgoCharaJump ();												// ちょっとジャンプ
					} else {
						Vector3 pos = EventSoudanTamago.transform.localPosition;
						pos.y += 30.0f;															// たまごっちを上昇させる
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
			case	statusJobCount.machiconJobCount160:
				{
					if (buttonFlag) {
						jobCount = statusJobCount.machiconJobCount170;
						EventSoudan.GetComponent<Animator> ().SetBool ("waite", true);
						EventSoudanPartsDisp (false);

						MesDisp.SoudanMesDisp (Message.SoudanMesTable.SoudanMesDispOff);

						EventAppeal.SetActive (true);
					}
					SoudanTamagoCharaSet ();													// 相談シーンのたまごっちのアニメ
					break;
				}
			case	statusJobCount.machiconJobCount170:
				{
					Vector3 pos = EventSoudanTamago.transform.localPosition;
					pos.y -= 20.0f;
					EventSoudanTamago.transform.localPosition = pos;							// たまごっちを下降させる
					if(pos.y <= -300.0f){
						jobCount = statusJobCount.machiconJobCount180;
						EventSoudanTamago.transform.Find ("chara").gameObject.SetActive (false);
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
							CharaTamagochi [i].transform.Find ("fukidashi/message").gameObject.SetActive (true);
						}
						TablePositionInit ();
						waitTime = 120;
					}
					break;
				}
			case	statusJobCount.machiconJobCount200:
				{
					if (WaitTimeSubLoop ()) {
						jobCount = statusJobCount.machiconJobCount210;
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp09);
						for (int i = 0; i < 8; i++) {
							CharaTamagochi [i].transform.Find ("fukidashi/message").gameObject.SetActive (false);
						}
						waitTime = 900;
					}
					break;
				}
			case	statusJobCount.machiconJobCount210:
				{
					ApplealPositionChangeMain ();												// アピールタイムのキャラ移動

					// 一定時間で実況表示を変える
					if((waitTime & 255) == 0){
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
						EventAppealTableHeartClear ();											// テーブルハートを消しておく

						//相談送信、全メンバーの相談処理を完了後、告白結果取得
						// パラメタは設計書参照
						//TODO masklistは現在何も反映していないので適宜設定を
						GameCall call = new GameCall(CallLabel.GET_ROOM_RESULT,maskdatas);
						call.AddListener(mgetroomres);
						manager.connect.send(call);
					}
					break;
				}
			case	statusJobCount.machiconJobCount230:
				{
					if (WaitTimeSubLoop () && mresult) {
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
							EventEndMarriage.SetActive (true);									// 自キャラがカップル成立したのでハートフェードを表示する
							jobCount = statusJobCount.machiconJobCount300;
						} else {
							jobCount = statusJobCount.machiconJobCount310;						// 自キャラがカップル成立していないのでそのままシーンチェンジ
						}
					}
					break;
				}
			case	statusJobCount.machiconJobCount300:
				{
					Vector3 _pos = EventEndMarriage.transform.Find ("panel").gameObject.transform.localPosition;
					_pos.y += 15.0f;
					if (_pos.y >= 450.0f) {
						_pos.y = 450.0f;
						jobCount = statusJobCount.machiconJobCount310;
					}
					EventEndMarriage.transform.Find ("panel").gameObject.transform.localPosition = _pos;
					break;
				}
			case	statusJobCount.machiconJobCount310:
				{
					if (WaitTimeSubLoop ()) {
						if (playerResultFlag) {
							Debug.Log ("結婚イベントへ・・・");
							if (muser1.utype == UserType.MIX2) {
								manager.view.change ("Marriage", mkind, muser1, mkind1, muser2, mkind2);
							} else {
								Debug.Log ("みーつユーザー以外なので、デートイベントへ後で変更すること");
								manager.view.change ("Marriage", mkind, muser1, mkind1, muser2, mkind2);
							}
						} else {
							Debug.Log ("たまタウンへ・・・");
							manager.view.change("Town");
						}

						jobCount = statusJobCount.machiconJobCount320;
					}
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
			}

//			for (int i = 0; i < 8; i++) {								// 各キャラの表示位置を登録
//				CharaTamago [i].transform.localPosition = posTamago [i];
//			}
			for (int i = 0; i < 8; i++) {
				CharaTamagochi [i].transform.localPosition = posTamago [i];
			}

			for (int i = 0; i < 8; i++) {
				CharaTamagochi [i].gameObject.GetComponent<Image> ().sprite = CharaTamago [i].GetComponent<SpriteRenderer> ().sprite;
				if (CharaTamago [i].gameObject.GetComponent<SpriteRenderer> ().flipX == true) {
					CharaTamagochi [i].transform.localScale = new Vector3 (-1.25f, 1.25f, 1.0f);
					CharaTamagochi [i].transform.Find ("fukidashi").gameObject.transform.localScale = new Vector3 (-0.85f, 0.85f, 1.0f);
				} else {
					CharaTamagochi [i].transform.localScale = new Vector3 (1.25f, 1.25f, 1.0f);
					CharaTamagochi [i].transform.Find ("fukidashi").gameObject.transform.localScale = new Vector3 (0.85f, 0.85f, 1.0f);
				}
			}



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

		// 相談シーンのたまごっちをImageに反映させる
		private void SoudanTamagoCharaSet (){
			EventSoudanTamago.transform.Find ("chara").gameObject.GetComponent<Image> ().sprite = CharaTamago [playerNumber].GetComponent<SpriteRenderer> ().sprite;
			EventSoudanTamago.transform.Find ("target").gameObject.GetComponent<Image> ().sprite = CharaTamago [targetNumber].GetComponent<SpriteRenderer> ().sprite;
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

			mesTamago = mpdata.members [posNumber].user.GetCharaAt (mpdata.members [posNumber].index).cname + mpdata.members [posNumber].user.GetCharaAt (mpdata.members [posNumber].index).wend;
			mesAvater = mpdata.members [posNumber].user.nickname;

			if (mesTamago == "" || mesTamago == null) {
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
			}

			CharaTamagochi [posNumber].transform.Find ("fukidashi/comment/text").gameObject.GetComponent<Text> ().text = mesAvater + mesRet + mesTamago;
		}
		// 告白タイムの対象相手を指定する
		// num:男の子の番号（０〜３）、targetNum:告白対象者の番号（４〜８）
		private void FukidashiMessageSetKokuhaku(int num,int targetNum){
			string mesAvater = "";
			string mesTamago = "";
			string mesRet = "\n";
			string mesNo = "の";
			string mesTo = "さん";

			mesTamago = mpdata.members [targetNum].user.GetCharaAt (mpdata.members [targetNum].index).cname;
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

		// 告白された事に対する返事のメッセージ（肯定、否定）
		// num:告白された女の子の番号（０〜３）、msgFlag:メッセージの種類（true:肯定、false:否定）
		private void FukidashiMessageKokuhakuReturn(int num,bool msgFlag){
			string _gobiMes = mpdata.members [num + 4].user.GetCharaAt (mpdata.members [num + 4].index).wend;

			if (msgFlag) {		// 告白肯定のメッセージ
				CharaTamagochi [num + 4].transform.Find ("fukidashi/comment/text").gameObject.GetComponent<Text> ().text = MesDisp.KokuhakuMesDisp (Message.KokuhakuMesTable.KokuhakuMesDispOK,_gobiMes);
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp13);
			} else {			// 告白否定のメッセージ
				CharaTamagochi [num + 4].transform.Find ("fukidashi/comment/text").gameObject.GetComponent<Text> ().text = MesDisp.KokuhakuMesDisp (Message.KokuhakuMesTable.KokuhakuMesDispNo,_gobiMes);
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp14);
			}
		}

		// たまごっちが画面下から入場する時の移動
		private float FlameInPositionChange(){
			posTamago [posNumber].y += 5.0f;

			return posTamago[posNumber].y;
		}

		// たまごっちがジャンプしたように見せる
		private float[] FlameInPositionChangeJumpTable = new float[]{
			0.0f, 10.0f, 20.0f, 28.0f, 34.0f, 38.0f, 41.0f, 43.0f, 41.0f, 38.0f, 34.0f, 28.0f, 20.0f, 10.0f,
		};
		private void FlameInPositionChangeJump(){
			posTamago [posNumber].y = FlameInPositionChangeJumpTable [waitTime];
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
			string _gobi = mpdata.members [playerNumber].user.GetCharaAt (mpdata.members [playerNumber].index).wend;

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

		// 入場時の整列位置への移動
		private void AlignmentPositionChange(){
			Vector2[] speed = new Vector2[8] {
				new Vector2(   90.0f / 40.0f, -200.0f / 40.0f ),		// 男の子１の整列位置への移動力
				new Vector2(  170.0f / 40.0f, -180.0f / 40.0f ),		// 男の子２の整列位置への移動力
				new Vector2(  250.0f / 40.0f, -160.0f / 40.0f ),		// 男の子３の整列位置への移動力
				new Vector2(  330.0f / 40.0f, -140.0f / 40.0f ),		// 男の子４の整列位置への移動力
				new Vector2(  -90.0f / 40.0f, -200.0f / 40.0f ),		// 女の子１の整列位置への移動力
				new Vector2( -170.0f / 40.0f, -180.0f / 40.0f ),		// 女の子２の整列位置への移動力
				new Vector2( -250.0f / 40.0f, -160.0f / 40.0f ),		// 女の子３の整列位置への移動力
				new Vector2( -330.0f / 40.0f, -140.0f / 40.0f ),		// 女の子４の整列位置への移動力
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
				CharaTamago [i].GetComponent<SpriteRenderer> ().flipX = flag;

				posTamago [i].x = pos [i, 0];
				posTamago [i].y = pos [i, 1];

				cbTamagoChara [i].gotoAndPlay ("idle");

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
				for (int i = 0; i < 8; i++) {
					tamagoHitPosition [i] = 0;
					TamagochiIdouInit (i);
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

			if (appealTimeCounter >= 80) {
				float[] _posY = new float[12];

				for (int i = 0; i < 8; i++) {
					if ((posTamagoSpeedX [i] != 0.0f) || (posTamagoSpeedY [i] != 0.0f)) {
						TamagochiAnimeSet (i, "walk");

						if (posTamagoIdouXFlag [i]) {
							posTamago [i].x += posTamagoSpeedX [i];
							if (posTamago [i].x >= posTamagoTargetX [i]) {
								posTamago [i].x = posTamagoTargetX [i];
							}
							CharaTamago [i].GetComponent<SpriteRenderer> ().flipX = posTamagoIdouXFlag [i];
						} else {
							posTamago [i].x -= posTamagoSpeedX [i];
							if (posTamago [i].x <= posTamagoTargetX [i]) {
								posTamago [i].x = posTamagoTargetX [i];
							}
							CharaTamago [i].GetComponent<SpriteRenderer> ().flipX = posTamagoIdouXFlag [i];
						}

						if (posTamagoIdouYFlag [i]) {
							posTamago [i].y += posTamagoSpeedY [i];
							if (posTamago [i].y >= posTamagoTargetY [i]) {
								posTamago [i].y = posTamagoTargetY [i];
							}
						} else {
							posTamago [i].y -= posTamagoSpeedY [i];
							if (posTamago [i].y <= posTamagoTargetY [i]) {
								posTamago [i].y = posTamagoTargetY [i];
							}
						}

						if (posTamagoSpeedX [i] != 0.0f) {
							if (posTamago [i].x == posTamagoTargetX [i]) {
								if (posTamagoTargetType [i] < 4) {
									countTableChakusekiTime [i] = Random.Range (120, 180);
								} else {
									countTableChakusekiTime [i] = Random.Range (30, 90);
								}
								posTamago [i].x = posTamagoTargetX [i];
								posTamago [i].y = posTamagoTargetY [i];
								posTamagoSpeedX [i] = 0.0f;
								posTamagoSpeedY [i] = 0.0f;
							}
						} else {
							if (posTamago [i].y == posTamagoTargetY [i]) {
								if (posTamagoTargetType [i] < 4) {
									countTableChakusekiTime [i] = Random.Range (120, 180);
								} else {
									countTableChakusekiTime [i] = Random.Range (30, 90);
								}
								posTamago [i].x = posTamagoTargetX [i];
								posTamago [i].y = posTamagoTargetY [i];
								posTamagoSpeedX [i] = 0.0f;
								posTamagoSpeedY [i] = 0.0f;
							}
						}
						TamagochiTableHitcheck (i);
					} else {
						if (countTableChakusekiTime [i] != 0) {
							countTableChakusekiTime [i]--;
							TamagochiAnimeSet (i, "idle");
							if (i < 4) {
								CharaTamago [i].GetComponent<SpriteRenderer> ().flipX = false;
							} else {
								CharaTamago [i].GetComponent<SpriteRenderer> ().flipX = true;
							}
							TamagoPairCheck (i);
							TamagoCakeCheck (i);
						} else {
							TamagochiIdouInit (i);
						}
					}



					_posY [i] = posTamago [i].y;
				}

				_posY[8] = _tablePostionY[0] + 45.0f;
				_posY[9] = _tablePostionY[1] + 45.0f;
				_posY[10] = _tablePostionY[2] + 45.0f;
				_posY[11] = _tablePostionY[3] + 45.0f;

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
		private int[] tamagoHitPosition = new int[8];
		private void TamagochiTableHitcheck(int num){
			for(int i = 0;i < 4;i++){
				if (((posTamago[num].x - 40.0f) < _tablePostionX[i]) && (_tablePostionX[i] < (posTamago[num].x + 40.0f))) {
					if (((posTamago[num].y - 12.0f) < (_tablePostionY[i] + 45.0f)) && ((_tablePostionY[i] + 45.0f) < (posTamago[num].y + 12.0f))) {


//						if (posTamago [num].x < _tablePostionX [i]) {
//							posTamago [num].x = _tablePostionX [i] - 41.0f;
//						} else {
//							posTamago [num].x = _tablePostionX [i] + 41.0f;
//						}

						if (posTamago [num].y < (_tablePostionY[i] + 45.0f)) {
							posTamago [num].y = (_tablePostionY[i] + 45.0f) - 13.0f;
							tamagoHitPosition [num] = -1;
						} else {
							posTamago [num].y = (_tablePostionY[i] + 45.0f) + 13.0f;
							tamagoHitPosition [num] = 1;
						}



						posTamagoSpeedX [num] = 0.0f;
						posTamagoSpeedY [num] = 0.0f;

						countTableChakusekiTime [num] = Random.Range (20, 40);
					}
				}
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

			_tablePostionY[0] = EventAppeal.transform.Find ("table1").gameObject.transform.localPosition.y;
			_tablePostionY[1] = EventAppeal.transform.Find ("table2").gameObject.transform.localPosition.y;
			_tablePostionY[2] = EventAppeal.transform.Find ("table3").gameObject.transform.localPosition.y;
			_tablePostionY[3] = EventAppeal.transform.Find ("table4").gameObject.transform.localPosition.y;


			for (int i = 0; i < 4; i++) {
				posTamagoTargetTableMan [i].x = _tablePostionX [i] + 70.0f;
				posTamagoTargetTableMan [i].y = _tablePostionY [i] + 45.0f;
				posTamagoTargetTableWoman [i].x = _tablePostionX [i] - 70.0f;
				posTamagoTargetTableWoman [i].y = _tablePostionY [i] + 45.0f;
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
		private void TamagochiIdouInit(int num){
			float _randSpeed = Random.Range(100,120);
			int _randPoint;

tamagochiIdouInitLoop:
			_randPoint = Random.Range (0, 10);

			switch (_randPoint) {
			case	0:
			case	1:
			case	2:
			case	3:
				{
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
				{
					int _rand = Random.Range (0, posTamagoTargetCakeTable.Length);
					posTamagoTargetX [num] = posTamagoTargetCakeTable [_rand].x;
					posTamagoTargetY [num] = posTamagoTargetCakeTable [_rand].y;
					break;
				}
			default:
				{
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
			posTamagoTargetType [num] = _randPoint;

			float x1 = System.Math.Abs(posTamago [num].x - posTamagoTargetX[num]);
			if (x1 < 1.0f) {
				posTamagoTargetX [num] = posTamago [num].x;
			}
			float y1 = System.Math.Abs (posTamago [num].y - posTamagoTargetY [num]);
			if (y1 < 1.0f) {
				posTamagoTargetY [num] = posTamago [num].y;
			}

			switch (tamagoHitPosition [num]) {
			case	-1:
				{
					if (posTamagoTargetY [num] > posTamago [num].y) {
						goto tamagochiIdouInitLoop;
					}
					break;
				}
			case	1:
				{
					if (posTamagoTargetY [num] < posTamago [num].y) {
						goto tamagochiIdouInitLoop;
					}
					break;
				}
			}
			tamagoHitPosition [num] = 0;

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
							TamagochiAnimeSet (i, "glad1");
							TamagochiAnimeSet (num, "glad1");
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
							TamagochiAnimeSet (i, "glad1");
							TamagochiAnimeSet (num, "glad1");
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
					break;
				}
			case	tamagochiFukidashiType.HEART:
				{
					CharaTamagochi [num].transform.Find ("fukidashi/cake").gameObject.SetActive (false);	// 吹き出しケーキを消す
					CharaTamagochi [num].transform.Find ("fukidashi/heart").gameObject.SetActive (true);	// 吹き出しハートを表示
					CharaTamagochi [num].transform.Find ("fukidashi/message").gameObject.SetActive (false);	// 吹き出しコメントを消す
					break;
				}
			case	tamagochiFukidashiType.MESSAGE:
				{
					CharaTamagochi [num].transform.Find ("fukidashi/cake").gameObject.SetActive (false);	// 吹き出しケーキを消す
					CharaTamagochi [num].transform.Find ("fukidashi/heart").gameObject.SetActive (false);	// 吹き出しハートを消す
					CharaTamagochi [num].transform.Find ("fukidashi/message").gameObject.SetActive (true);	// 吹き出しコメントを表示
					break;
				}
			case	tamagochiFukidashiType.OFF:
				{
					CharaTamagochi [num].transform.Find ("fukidashi/cake").gameObject.SetActive (false);	// 吹き出しケーキを消す
					CharaTamagochi [num].transform.Find ("fukidashi/heart").gameObject.SetActive (false);	// 吹き出しハートを消す
					CharaTamagochi [num].transform.Find ("fukidashi/message").gameObject.SetActive (false);	// 吹き出しコメントを消す
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
				CharaTamago [i].GetComponent<SpriteRenderer> ().flipX = flag;

				posTamago [i].x = pos [i, 0];
				posTamago [i].y = pos [i, 1];

				cbTamagoChara [i].gotoAndPlay ("idle");
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
			pos.y += num;
			EventCurtain.transform.localPosition = pos;

			if (num > 0) {
				if (EventCurtain.transform.localPosition.y >= 1200.0f) {
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


			tamagoMes = mpdata.members [targetNumber].user.GetCharaAt (mpdata.members [targetNumber].index).cname;
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

					kokuhakuManMessage [0] = MesDisp.KokuhakuMesDispMan (mpdata.members [0].user.GetCharaAt (mpdata.members [0].index).wend);	// 告白宣言メッセージをここで決定
					kokuhakuManMessage [1] = MesDisp.KokuhakuMesDispMan (mpdata.members [1].user.GetCharaAt (mpdata.members [1].index).wend);
					kokuhakuManMessage [2] = MesDisp.KokuhakuMesDispMan (mpdata.members [2].user.GetCharaAt (mpdata.members [2].index).wend);
					kokuhakuManMessage [3] = MesDisp.KokuhakuMesDispMan (mpdata.members [3].user.GetCharaAt (mpdata.members [3].index).wend);
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount010:
				{
					if (kokuhakuManTable [kokuhakuManNumber] == 255) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount350;			// ライバル出現したので通常告白はなし
						break;
					}
						
					if (KokuhakuTimeIdou1 (kokuhakuManTable [kokuhakuManNumber])) {				// 男の子は一歩前に移動する
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount020;
						TamagochiPatanChenge (0, kokuhakuManTable [kokuhakuManNumber]);			// 告白者のアニメパターンを登録
						FukidashiMessageSetKokuhaku (kokuhakuManTable [kokuhakuManNumber], KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4);
						CharaTamagochi[kokuhakuManTable[kokuhakuManNumber]].transform.Find("fukidashi/comment").gameObject.SetActive(true);

						kokuhakuWaitTime = 120;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount020:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount030;
						CharaTamagochi[kokuhakuManTable[kokuhakuManNumber]].transform.Find("fukidashi/comment").gameObject.SetActive(false);

					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount030:
				{
					if (KokuhakuTimeIdou1 (KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4)) {		// 告白指定された女の子は一歩前に移動する
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount040;
						TamagochiPatanChenge (1, KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4);	// 告白対象者のアニメパターンを登録
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
						TamagochiPatanChenge (0, 1);											// 告白者のアニメパターンを登録
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
						TamagochiPatanChenge (0, 2);											// 告白者のアニメパターンを登録
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
						TamagochiPatanChenge (0, 3);											// 告白者のアニメパターンを登録
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
					if (KokuhakuTimeIdou2 (kokuhakuManTable [kokuhakuManNumber])) {				// 告白者をもう一歩前に
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount170;
						KokuhakuMessageDisp (kokuhakuManTable [kokuhakuManNumber], true);		// 吹き出しを表示
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount170:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						KokuhakuMessageDisp (kokuhakuManTable [kokuhakuManNumber], false);		// 吹き出しを非表示
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
						KokuhakuMessageDisp (1, true);											// 吹き出しを表示
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount200:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount210;
						KokuhakuMessageDisp (1, false);											// 吹き出しを非表示
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
						KokuhakuMessageDisp (2, true);											// 吹き出しを表示
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount230:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount240;
						KokuhakuMessageDisp (2, false);											// 吹き出しを非表示
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
						KokuhakuMessageDisp (3, true);											// 吹き出しを表示
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount260:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount270;
						KokuhakuMessageDisp (3, false);											// 吹き出しを非表示
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
						LoveParamCheck ();														// 告白判定

						FukidashiMessageKokuhakuReturn (KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]], loveParamFlag);		// 告白の返事を表示
						CharaTamagochi[KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].transform.Find("fukidashi/comment").gameObject.SetActive(true);


						heartSize = 0.0f;
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount290;
						kokuhakuWaitTime = 120;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount290:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						LoveResultDisp (true);													// 告白結果のハートなどを表示
						CharaTamagochi[KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].transform.Find("fukidashi/comment").gameObject.SetActive(false);
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount300;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount300:
				{
					if (LoveResultDispHeart ()) {												// 告白結果のハートを拡大表示
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount310;
						kokuhakuWaitTime = 120;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount310:
				{
					if (KokuhakuWaitTimeSubLoop ()) {
						LoveResultDisp (false);
						MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
						CharaTamago [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].GetComponent<SpriteRenderer> ().flipX = false;
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount320;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount320:
				{
					if (KokuhakuTimeIdou3 (KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4)) {	// 女の子は定位置に戻る
						CharaTamago [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].GetComponent<SpriteRenderer> ().flipX = true;

						if (loveParamFlag) {
							TamagochiPatanChenge (3, KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4);

							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount330;
							KokuhakuTimeIdou4Init (KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4);
							cbTamagoChara [loveParamManNumber].gotoAndPlay ("walk");
						} else {
							cbTamagoChara [KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]] + 4].gotoAndPlay ("idle");
							kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount340;
						}
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount330:
				{
					if (KokuhakuTimeIdou4 ()) {													// 告白成功したので女の子の前に男の子は移動する
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount340;
						TamagochiPatanChenge (3, loveParamManNumber);

						kokuhakuOkFlag = true;													// 告白成功者がいる
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount340:
				{
					if (KokuhakuTimeIdou5 ()) {													// 告白失敗者は定位置に戻る
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount350;
					}
					break;
				}

			case	statusKokuhakuCount.kokuhakuCount350:
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
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount360;
					} else {
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount010;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount360:
				{
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
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount370;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount370:
				{
					Vector3 pos = new Vector3 (1.0f, 1.0f, 1.0f);
					if (kokuhakuOkFlag) {														// 告白成功者がいる
						pos = EventLastResult.transform.Find ("yes/panel").gameObject.transform.localPosition;
					} else {																	// 告白成功者がいない
						pos = EventLastResult.transform.Find ("no/panel").gameObject.transform.localPosition;
					}
					pos.y += 15.0f;
					if (pos.y >= 2300.0f) {
						pos.y = 2300.0f;
						kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount380;
					}

					if (kokuhakuOkFlag) {														// 告白成功者がいる
						EventLastResult.transform.Find ("yes/panel").gameObject.transform.localPosition = pos;
					} else {																	// 告白成功者がいない
						EventLastResult.transform.Find ("no/panel").gameObject.transform.localPosition = pos;
					}
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount380:
				{
					if (kokuhakuOkFlag) {														// 告白成功者がいる
						EventLastResult.transform.Find ("yes").gameObject.SetActive (false);
					} else {																	// 告白成功者がいない
						EventLastResult.transform.Find ("no").gameObject.SetActive (false);
					}
					kokuhakuTimeLoopCount = statusKokuhakuCount.kokuhakuCount390;
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount390:
				{
					kokuhakuTimeEndFlag = true;													// 告白タイム終了
					break;
				}
			case	statusKokuhakuCount.kokuhakuCount400:
				{
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
		private void KokuhakuMessageDisp(int num,bool flag){
			CharaTamagochi [num].transform.Find ("fukidashi/comment/text").gameObject.GetComponent<Text> ().text = kokuhakuManMessage [num];
			CharaTamagochi [num].transform.Find ("fukidashi/comment").gameObject.SetActive (flag);	// 吹き出しコメントを表示・非表示
			if (flag) {
				TamagochiPatanChenge (0, num);														// 告白者のアニメパターンを登録
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDisp11);
				kokuhakuWaitTime = 120;
			} else {
				MesDisp.JikkyouMesDisp (Message.JikkyouMesTable.JikkyouMesDispOff);
			}
		}

		// num:たまごっちの番号（０〜７）
		private bool KokuhakuTimeIdou1(int num){
			float[,] posTable = new float[8,4] {
				{  200.0f,  120.0f, -2.0f, 0f },
				{  230.0f,  150.0f, -2.0f, 0f },
				{  260.0f,  180.0f, -2.0f, 0f },
				{  290.0f,  210.0f, -2.0f, 0f },
				{ -200.0f, -120.0f,  2.0f, 1f },
				{ -230.0f, -150.0f,  2.0f, 1f },
				{ -260.0f, -180.0f,  2.0f, 1f },
				{ -290.0f, -210.0f,  2.0f, 1f },
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
				{  120.0f,  40.0f, -2.0f, 0f },
				{  150.0f,  70.0f, -2.0f, 0f },
				{  180.0f, 100.0f, -2.0f, 0f },
				{  210.0f, 130.0f, -2.0f, 0f },
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
				{ -120.0f, -200.0f, -2.0f, 0f },
				{ -150.0f, -230.0f, -2.0f, 0f },
				{ -180.0f, -260.0f, -2.0f, 0f },
				{ -210.0f, -290.0f, -2.0f, 0f },
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
		// num:たまごっち（女の子）の番号（４〜７）
		private void KokuhakuTimeIdou4Init(int num){
			idou4GoalX = posTamago [num].x + 90.0f;												// 女の子の右隣を目標地点にする
			idou4GoalY = posTamago [num].y;

			idou4SpeedX = (idou4GoalX - posTamago [loveParamManNumber].x) / 60.0f;
			idou4SpeedY = (idou4GoalY - posTamago [loveParamManNumber].y) / 60.0f;

			CharaTamagochi [loveParamManNumber].GetComponent<Canvas> ().sortingOrder = CharaTamagochi [num].GetComponent<Canvas> ().sortingOrder;

			if ((playerNumber == num) || (playerNumber == loveParamManNumber)) {
				playerResultFlag = true;
				if (playerNumber == num) 
				{
					muser2 = mpdata.members[loveParamManNumber].user;
					mkind2 = mpdata.members[loveParamManNumber].index;
				}
				else
				{
					muser2 = mpdata.members[num].user;
					mkind2 = mpdata.members[num].index;
				}
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
				200.0f,
				230.0f,
				260.0f,
				290.0f,
			};
			bool retFlag = false;
			int retNum = 0;
			for (int i = 0; i < 4; i++) {
				if (loveParamManCryFlag [i]) {
					posTamago [i].x += 4.0f;
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
		private void TamagochiPatanChenge(int patanNum,int tamagoNum){
			switch (patanNum) {
			case	0:
				{	// 告白者のアニメパターン
					switch (Random.Range (0, 3)) {
					case	0:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("glad2");					// 喜び２
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
							cbTamagoChara [tamagoNum].gotoAndPlay ("glad1");					// 喜び１
							break;
						}
					case	1:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("glad2");					// 喜び２
							break;
						}
					case	2:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("glad3");					// 喜び３
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
							cbTamagoChara [tamagoNum].gotoAndPlay ("glad1");					// 喜び１
							break;
						}
					case	1:
						{
							cbTamagoChara [tamagoNum].gotoAndPlay ("glad2");					// 喜び２
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
		private void LoveParamCheck(){
			for (int i = 0; i < 4; i++) {
				loveParamManCryFlag [i] = false;
			}

			if(loveManWomanFix[kokuhakuManTable [kokuhakuManNumber],KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]] <= loveManWoman[kokuhakuManTable [kokuhakuManNumber],KokuhakuManToWomanTable [kokuhakuManTable [kokuhakuManNumber]]]){
				// 告白成功
				loveParamFlag = true;
				loveParamManNumber = kokuhakuManTable [kokuhakuManNumber];
				TamagochiPatanChenge (2, kokuhakuManTable [kokuhakuManNumber]);					// 告白成功時のアニメパターンを登録
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
						TamagochiPatanChenge (2, 1);											// 告白成功時のアニメパターンを登録
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
						TamagochiPatanChenge (2, 2);											// 告白成功時のアニメパターンを登録
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
						TamagochiPatanChenge (2, 3);											// 告白成功時のアニメパターンを登録
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
		private void LoveResultDisp(bool flag){
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



		private IEnumerator AppelTimeWait(){
			yield return new WaitForSeconds (APPEAL_LIMIT_TIME);
			buttonFlag = true;
		}

		// Yesボタンが押された時
		private void ButtneYesClick(){
			LoveManWomanNumberSet (10);		// 好感度を１０上げる
			buttonFlag = true;
			//btnYesNo = true;
		}
		// Noボタンが押された時
		private void ButtonNoClick(){
			LoveManWomanNumberSet (-10);	// 好感度を１０下げる
			buttonFlag = true;
			//btnYesNo = false;
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
