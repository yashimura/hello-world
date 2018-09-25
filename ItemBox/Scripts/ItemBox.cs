using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

public class ItemBox : MonoBehaviour,IReceiver {

	[SerializeField] private Camera mainCamera;
	[SerializeField] private GameObject EventItemBoxMenu;			// プレゼントボックスメイン画面
	[SerializeField] private GameObject EventPresentMIX2;			// プレゼントボックスアイテム一覧画面MIX2用
	[SerializeField] private GameObject EventPresent;				// プレゼントボックスアイテム一覧画面
	[SerializeField] private GameObject EventTushinOk;				// 通信成功画面
	[SerializeField] private GameObject EventTushinNot;				// 通信失敗画面
	[SerializeField] private GameObject EventPresentPoint;			// プレゼントボックスポイント表示画面
	[SerializeField] private GameObject EventTushinPoint;			// プレゼントボックスポイント転送量決定画面
	[SerializeField] private GameObject EventDialogKakunin;			// 確認画面
	[SerializeField] private GameObject EventDialogError;			// エラー画面
	[SerializeField] private GameObject EventTushinTime;			// 通信中画面

	[SerializeField] private Button ButtonPresent;					// メイン画面 アイテム一覧画面へのボタン
	[SerializeField] private Button ButtonPoint;					// メイン画面 ポイント表示画面へのボタン
	[SerializeField] private Button ButtonTojiru;					// メイン画面 とじるボタン

	[SerializeField] private Button ButtonPresentBackMIX2;			// アイテム一覧画面MIX2用 もどるボタン
	[SerializeField] private Button ButtonPresentBack;				// アイテム一覧画面　もどるボタン

	[SerializeField] private Button ButtonBackOK;					// 通信成功画面 もどるボタン
	[SerializeField] private Button ButtonBackNot;					// 通信失敗画面 もどるボタン

	[SerializeField] private Button ButtonPointSend;				// ポイント表示画面　おくるボタン
	[SerializeField] private Button ButtonPointBackMIX2;			// ポイント表示画面　MIX2用もどるボタン
	[SerializeField] private Button ButtonPointBack;				// ポイント表示画面　もどるボタン

	[SerializeField] private Button ButtonTushinBack;				// ポイント転送量決定画面　もどるボタン
	[SerializeField] private Button ButtonTushinSend;				// ポイント転送量決定画面　おくるボタン
	[SerializeField] private Button ButtonTushinSendAll;			// ポイント転送量決定画面　ぜんぶおくるボタン
	[SerializeField] private Button[] ButtonTushinUp;				// ポイント転送量決定画面　上ボタン（全４つ）
	[SerializeField] private Button[] ButtonTushinDown;				// ポイント転送量決定画面　下ボタン（全４つ）

	[SerializeField] private Button ButtonKakuninYes;				// 確認画面　はいボタン
	[SerializeField] private Button ButtonKakuninNo;				// 確認画面　いいえボタン
	[SerializeField] private Button ButtonErrorTojiru;				// エラー画面　とじるボタン

	[SerializeField] private Sprite[] SuujiBlack;					// 
	[SerializeField] private Sprite[] SuujiRed;						// 

	[SerializeField] private Sprite[] PresentImage;					//

	[SerializeField] private GameObject WakuPrefab;					// 
	[SerializeField] private GameObject WakuMIX2Prefab;				// 


	private object[]		mparam;
	private User muser1;//自分
	private PresentData		mpdata;

	private int	gPointMax;
	private int	gPointNow;
	private int itemCountNow;
	private int itemCountMax;
	private int itemNumberNow;		// 転送するアイテムの番号
	private GameObject[]	prefabObj = new GameObject[100];


	private float[] itemBoxPosXTable = new float[4]{
		-470.0f,-160.0f,160.0f,470.0f
	};
	private float[] itemBoxPosYTable = new float[25]{
		  180.0f, -130.0f, -440.0f, -750.0f,-1060.0f,
		-1370.0f,-1680.0f,-1990.0f,-2300.0f,-2610.0f,
		-2920.0f,-3230.0f,-3540.0f,-3850.0f,-4160.0f,
		-4470.0f,-4780.0f,-5090.0f,-5400.0f,-5710.0f,
		-6020.0f,-6330.0f,-6640.0f,-6950.0f,-7260.0f,
	};





	void Awake(){
		Debug.Log ("ItemBox Awake");
		mparam=null;
		muser1=null;
	}

	public void receive(params object[] parameter){
		Debug.Log ("ItemBox receive");
		mparam = parameter;
	}

	void Start() {
		Debug.Log ("ItemBox start");

		//ルーム情報取得、メンバーマッチング処理
		//パラメタは設計書参照
		GameCall call = new GameCall(CallLabel.GET_PRESENTS);
		call.AddListener(mGetPresents);
		ManagerObject.instance.connect.send(call);
	}
	void mGetPresents(bool success,object data)
	{
		// dataの内容は設計書参照
		mpdata = (PresentData)data;
		StartCoroutine(mstart());
	}
	void mBleSendGift(bool success,object data)
	{
		Debug.LogFormat("ItemBox mBleSendGift:{0},{1}",success,data);

		EventTushinTime.SetActive (false);

		if (success) {
			for (int i = 0; i < itemCountNow; i++) {
				Destroy (prefabObj [i]);
			}

			mpdata = (PresentData)data;
			mpdataExpansion ();
			EventTushinOk.SetActive (true);
		} else {
			EventTushinNot.SetActive (true);
		}
	}

	IEnumerator mstart(){
		//単体動作テスト用
		//パラメタ詳細は設計書参照
		if (mparam==null) {
			mparam = new object[] {
				ManagerObject.instance.player,
			};
		}
		muser1 = (User)mparam[0];		// たまごっち



		muser1.utype = UserType.MIX2;



		// mpdataを展開して必要な画面に登録する
		mpdataExpansion ();



		ButtonPresent.onClick.AddListener (ButtonPresentClick);
		ButtonPoint.onClick.AddListener (ButtonPointClick);
		ButtonTojiru.onClick.AddListener (ButtonTojiruClick);
		ButtonPresentBackMIX2.onClick.AddListener (ButtonPresentBackMIX2Click);
		ButtonPresentBack.onClick.AddListener (ButtonPresentBackClick);
		ButtonBackOK.onClick.AddListener (ButtonBackOKClick);
		ButtonBackNot.onClick.AddListener (ButtonBackNotClick);
		ButtonPointSend.onClick.AddListener (ButtonPointSendClick);
		ButtonPointBackMIX2.onClick.AddListener (ButtonPointBackMIX2Click);
		ButtonPointBack.onClick.AddListener (ButtonPointBackClick);
		ButtonTushinBack.onClick.AddListener (ButtonTushinBackClick);
		ButtonTushinSend.onClick.AddListener (ButtonTushinSendClick);
		ButtonTushinSendAll.onClick.AddListener (ButtonTushinSendAllClick);
		ButtonKakuninYes.onClick.AddListener (ButtonKakuninYesClick);
		ButtonKakuninNo.onClick.AddListener (ButtonKakuninNoClick);
		ButtonErrorTojiru.onClick.AddListener (ButtonErrorTojiruClick);

		ButtonTushinUp [0].onClick.AddListener (ButtonTushinUp1000Click);
		ButtonTushinUp [1].onClick.AddListener (ButtonTushinUp100Click);
		ButtonTushinUp [2].onClick.AddListener (ButtonTushinUp10Click);
		ButtonTushinUp [3].onClick.AddListener (ButtonTushinUp1Click);
		ButtonTushinDown [0].onClick.AddListener (ButtonTushinDown1000Click);
		ButtonTushinDown [1].onClick.AddListener (ButtonTushinDown100Click);
		ButtonTushinDown [2].onClick.AddListener (ButtonTushinDown10Click);
		ButtonTushinDown [3].onClick.AddListener (ButtonTushinDown1Click);



		swipIdouFlag = false;



		ItemBoxScrollFlag = true;
		StartCoroutine ("ItemBoxScroll");


	
		yield return null;


	}

	void Destroy(){
		ItemBoxScrollFlag = false;
		Debug.Log ("ItemBox Destroy");
	}


	private bool swipIdouFlag;
	private float StartPos;
	private float EndPos;

	private itemBoxIdouTable	itemBoxIdouFlag = itemBoxIdouTable.Null;
	private enum itemBoxIdouTable{
		Null,
		Down010,
		Down020,
		Down030,
		Up010,
		Up020,
		Up030,
	};
	void Update(){
		if (swipIdouFlag) {
			if (Input.GetMouseButtonDown (0)) {
				StartPos = Input.mousePosition.y;
			}
			if (Input.GetMouseButtonUp (0)) {
				if ((StartPos != 0) && (itemBoxIdouFlag == itemBoxIdouTable.Null)) {
					EndPos = Input.mousePosition.y;
					if (StartPos > EndPos) {
//						Debug.Log ("上から下移動" + StartPos.ToString () + "から" + EndPos.ToString ());
						itemBoxIdouFlag = itemBoxIdouTable.Down010;
					} else if (StartPos < EndPos) {
//						Debug.Log ("下から上移動" + StartPos.ToString () + "から" + EndPos.ToString ());
						itemBoxIdouFlag = itemBoxIdouTable.Up010;
					}
				}
				StartPos = 0;
				EndPos = 0;
			}
		} else {
			StartPos = 0;
			EndPos = 0;
			itemBoxIdouFlag = 0;
		}
	}

//310
	private bool ItemBoxScrollFlag;
	private float NextYPos;
	private float[] NextYPosTable = new float[25]{
		   0.0f,   0.0f, 310.0f, 620.0f, 930.0f,
		1240.0f,1550.0f,1860.0f,2170.0f,2480.0f,
		2790.0f,3100.0f,3410.0f,3720.0f,4030.0f,
		4340.0f,4650.0f,4960.0f,5270.0f,5580.0f,
		5890.0f,6200.0f,6510.0f,6820.0f,7130.0f,
	};

	private IEnumerator ItemBoxScroll(){
		Vector3 _Pos;

		while (ItemBoxScrollFlag) {
			switch (itemBoxIdouFlag) {
			case	itemBoxIdouTable.Down010:
				{
					NextYPos = EventPresent.transform.Find ("mask/panel").transform.localPosition.y - 310.0f;
					if (NextYPos < 0.0f) {
						itemBoxIdouFlag = itemBoxIdouTable.Null;
					} else {
						itemBoxIdouFlag = itemBoxIdouTable.Down020;
					}
					break;
				}
			case	itemBoxIdouTable.Down020:
				{
					_Pos = EventPresent.transform.Find ("mask/panel").transform.localPosition;
					_Pos.y = _Pos.y - (31.0f * (60.0f * Time.deltaTime));
					if (_Pos.y <= NextYPos) {
						_Pos.y = NextYPos;
						itemBoxIdouFlag = itemBoxIdouTable.Null;
					}
					EventPresent.transform.Find ("mask/panel").transform.localPosition = _Pos;
					EventPresentMIX2.transform.Find ("mask/panel").transform.localPosition = _Pos;
					break;
				}
			case	itemBoxIdouTable.Down030:
				{
					break;
				}



			case	itemBoxIdouTable.Up010:
				{
					NextYPos = EventPresent.transform.Find ("mask/panel").transform.localPosition.y + 310.0f;
					int _cnt = itemCountNow;
					if (_cnt != 0) {
						_cnt--;
					}
					if (NextYPos > NextYPosTable [_cnt >> 2]) {
						itemBoxIdouFlag = itemBoxIdouTable.Null;
					} else {
						itemBoxIdouFlag = itemBoxIdouTable.Up020;
					}
					break;
				}
			case	itemBoxIdouTable.Up020:
				{
					_Pos = EventPresent.transform.Find ("mask/panel").transform.localPosition;
					_Pos.y = _Pos.y + (31.0f * (60.0f * Time.deltaTime));
					if (_Pos.y >= NextYPos) {
						_Pos.y = NextYPos;
						itemBoxIdouFlag = itemBoxIdouTable.Null;
					}
					EventPresent.transform.Find ("mask/panel").transform.localPosition = _Pos;
					EventPresentMIX2.transform.Find ("mask/panel").transform.localPosition = _Pos;
					break;
				}
			case	itemBoxIdouTable.Up030:
				{
					break;
				}
			}
			yield return null;
		}
	}

	// アイテム一覧画面のアイテム毎にボタン化しボタンが押されたら確認画面を開く
	private void ButtonWakuClick(int num){
		if (itemBoxIdouFlag == itemBoxIdouTable.Null) {
			Debug.Log ("Button " + num.ToString ());

			EventDialogKakunin.SetActive (true);
			EventDialogKakunin.transform.Find ("1").gameObject.SetActive (true);
			EventDialogKakunin.transform.Find ("1/Image").gameObject.GetComponent<Image> ().sprite = prefabObj [num].transform.Find ("Image").gameObject.GetComponent<Image> ().sprite;
			EventDialogKakunin.transform.Find ("1/Text").gameObject.GetComponent<Text> ().text = prefabObj [num].transform.Find ("Text").gameObject.GetComponent<Text> ().text;

			itemNumberNow = num;
		}
	}





	private void ButtonPresentClick(){
		if (muser1.utype == UserType.MIX2) {
			EventPresentMIX2.SetActive (true);
		} else {
			EventPresent.SetActive (true);
		}
		swipIdouFlag = true;

		gPointNow = 0;
		itemNumberNow = -1;
	}
	private void ButtonPointClick(){
		
		EventPresentPoint.SetActive (true);
		if (muser1.utype == UserType.MIX2) {
			ButtonPointSend.gameObject.SetActive (true);
			ButtonPointBackMIX2.gameObject.SetActive (true);
			ButtonPointBack.gameObject.SetActive (false);
		} else {
			ButtonPointSend.gameObject.SetActive (false);
			ButtonPointBackMIX2.gameObject.SetActive (false);
			ButtonPointBack.gameObject.SetActive (true);
		}

		gPointNow = 0;
		itemNumberNow = -1;
	}
	private void ButtonTojiruClick(){
		Debug.Log ("たまタウンへ・・・");
		ManagerObject.instance.view.change("Town");
	}
	private void ButtonPresentBackMIX2Click(){
		EventPresentMIX2.SetActive (false);
	}
	private void ButtonPresentBackClick(){
		EventPresent.SetActive (false);
		swipIdouFlag = false;
	}
	private void ButtonBackOKClick(){
		EventTushinOk.SetActive (false);
	}
	private void ButtonBackNotClick(){
		EventTushinNot.SetActive (false);
	}
	private void ButtonPointSendClick(){
		gPointNow = 0;
		pointSendSet ();
		EventTushinPoint.SetActive (true);
	}
	private void ButtonPointBackMIX2Click(){
		EventPresentPoint.SetActive (false);
	}
	private void ButtonPointBackClick(){
		EventPresentPoint.SetActive (false);
	}
	private void ButtonTushinBackClick(){
		EventTushinPoint.SetActive (false);
	}
	private void ButtonTushinSendClick(){
		pointKakuninSet ();
		EventDialogKakunin.SetActive (true);
		EventDialogKakunin.transform.Find ("3").gameObject.SetActive (true);
	}
	private void ButtonTushinSendAllClick(){
		gPointNow = gPointMax;
		if (gPointNow >= 10000) {
			gPointNow = 9999;
		}
		pointSendSet ();
		ButtonTushinSendClick ();
	}



	private void ButtonKakuninYesClick(){
		GameCall call;

		if (itemNumberNow != -1) {
			call = new GameCall (CallLabel.BLE_SEND_GIFT, mpdata.items [itemNumberNow].iid, 0);
		} else {
			call = new GameCall (CallLabel.BLE_SEND_GIFT, 0, gPointNow);
		}
		call.AddListener(mBleSendGift);
		ManagerObject.instance.connect.send(call);

		ButtonKakuninNoClick ();
		EventTushinTime.SetActive (true);
	}
	private void ButtonKakuninNoClick(){
		EventDialogKakunin.transform.Find ("1").gameObject.SetActive (false);
		EventDialogKakunin.transform.Find ("2").gameObject.SetActive (false);
		EventDialogKakunin.transform.Find ("3").gameObject.SetActive (false);
		EventDialogKakunin.transform.Find ("4").gameObject.SetActive (false);
		EventDialogKakunin.SetActive (false);
	}
	private void ButtonErrorTojiruClick(){
	}
		
	private void ButtonTushinUp1000Click(){
		pointNowAdd (1000);
	}
	private void ButtonTushinUp100Click(){
		pointNowAdd (100);
	}
	private void ButtonTushinUp10Click(){
		pointNowAdd (10);
	}
	private void ButtonTushinUp1Click(){
		pointNowAdd (1);
	}
	private void ButtonTushinDown1000Click(){
		pointNowSub (1000);
	}
	private void ButtonTushinDown100Click(){
		pointNowSub (100);
	}
	private void ButtonTushinDown10Click(){
		pointNowSub (10);
	}
	private void ButtonTushinDown1Click(){
		pointNowSub (1);
	}

	private void pointNowAdd(int _point){
		gPointNow += _point;
		if (gPointNow > gPointMax) {
			gPointNow = gPointMax;
		}
		if (gPointNow >= 10000) {
			gPointNow = 9999;
		}

		pointSendSet ();
	}
	private void pointNowSub(int _point){
		gPointNow -= _point;
		if (gPointNow <= 0) {
			gPointNow = 0;
		}
		pointSendSet ();
	}



	// PresentDataを展開し、画面に必要なものを抽出する
	private void mpdataExpansion(){
		gPointMax = mpdata.gpt;
		gPointNow = 0;

		itemCountMax = mpdata.items.Count;
		if (itemCountMax > 100) {
			itemCountNow = 100;
		} else {
			itemCountNow = itemCountMax;
		}



		PresentItemBoxSet ();				// アイテム一覧画面にプレハブを登録しボタンを登録する

		pointSet (EventPresentPoint);		// プレゼントボックスポイント表示画面（集めたポイント）を登録する
		pointSet (EventTushinPoint);		// プレゼントボックスポイント転送量決定画面（集めたポイント）を登録する
		pointSendSet ();					// プレゼントボックスポイント転送量決定画面（送るポイント）を登録する
	}



	// アイテム一覧画面
	private void PresentItemBoxSet (){
		Vector3 _Pos;
		GameObject _Obj;

		if (muser1.utype == UserType.MIX2) {
			_Obj = EventPresentMIX2;
		} else {
			_Obj = EventPresent;
		}

		for (int i = 0; i < itemCountNow; i++) {
			if (muser1.utype == UserType.MIX2) {
				prefabObj [i] = (GameObject)Instantiate (WakuMIX2Prefab);
			} else {
				prefabObj [i] = (GameObject)Instantiate (WakuPrefab);
			}
			prefabObj [i].transform.SetParent (_Obj.transform.Find ("mask/panel").transform, false);
			prefabObj [i].name = "Waku" + i.ToString ();
			if (muser1.utype == UserType.MIX2) {
				int ii = i + 0;
				prefabObj [i].GetComponent<Button> ().onClick.AddListener (() => ButtonWakuClick (ii));
			}

			_Pos.x = itemBoxPosXTable [i & 3];
			_Pos.y = itemBoxPosYTable [i >> 2];
			_Pos.z = 0.0f;
			prefabObj [i].transform.localPosition = _Pos;

			prefabObj [i].transform.Find ("Image").gameObject.GetComponent<Image> ().sprite = PresentImage [mpdata.items [i].iid];
			prefabObj [i].transform.Find ("Text").gameObject.GetComponent<Text> ().text = mpdata.items [i].title;
		}

		if (itemCountMax > 100) {
			_Obj.transform.Find ("item_kazu/100").transform.GetComponent<Image> ().sprite = SuujiRed [itemCountMax / 100];
			_Obj.transform.Find ("item_kazu/10").transform.GetComponent<Image> ().sprite = SuujiRed [(itemCountMax % 100) / 10];
			_Obj.transform.Find ("item_kazu/1").transform.GetComponent<Image> ().sprite = SuujiRed [itemCountMax % 10];

			_Obj.transform.Find ("item_kazu/100").gameObject.SetActive (true);
			_Obj.transform.Find ("item_kazu/10").gameObject.SetActive (true);
			_Obj.transform.Find ("item_kazu/1").gameObject.SetActive (true);
		} else {
			bool _flag2 = false;

			if (itemCountMax == 100) {
				_Obj.transform.Find ("item_kazu/100").transform.GetComponent<Image> ().sprite = SuujiBlack [1];
				_Obj.transform.Find ("item_kazu/100").gameObject.SetActive (true);
				_flag2 = true;
			} else {
				_Obj.transform.Find ("item_kazu/100").gameObject.SetActive (false);
			}
			if ((((itemCountMax % 100) / 10) != 0) || (_flag2)) {
				_Obj.transform.Find ("item_kazu/10").transform.GetComponent<Image> ().sprite = SuujiBlack [(itemCountMax % 100) / 10];
				_Obj.transform.Find ("item_kazu/10").gameObject.SetActive (true);
			} else {
				_Obj.transform.Find ("item_kazu/10").gameObject.SetActive (false);
			}
			_Obj.transform.Find ("item_kazu/1").transform.GetComponent<Image> ().sprite = SuujiBlack [itemCountMax % 10];
		}
	}

	private void pointSet (GameObject obj){
		bool _flag = false;

		if ((gPointMax / 10000) > 0) {
			obj.transform.Find ("waku/10000").transform.GetComponent<Image> ().sprite = SuujiBlack [gPointMax / 10000];
			obj.transform.Find ("waku/10000").gameObject.SetActive (true);
			_flag = true;
		} else {
			obj.transform.Find ("waku/10000").gameObject.SetActive (false);
		}

		if ((((gPointMax % 10000) / 1000) > 0) || (_flag)) {
			obj.transform.Find ("waku/1000").transform.GetComponent<Image> ().sprite = SuujiBlack [(gPointMax % 10000) / 1000];
			obj.transform.Find ("waku/1000").gameObject.SetActive (true);
			_flag = true;
		} else {
			obj.transform.Find ("waku/1000").gameObject.SetActive (false);
		}

		if ((((gPointMax % 1000) / 100) > 0) || (_flag)) {
			obj.transform.Find ("waku/100").transform.GetComponent<Image> ().sprite = SuujiBlack [(gPointMax % 1000) / 100];
			obj.transform.Find ("waku/100").gameObject.SetActive (true);
			_flag = true;
		} else {
			obj.transform.Find ("waku/100").gameObject.SetActive (false);
		}

		if ((((gPointMax % 100) / 10) > 0) || (_flag)) {
			obj.transform.Find ("waku/10").transform.GetComponent<Image> ().sprite = SuujiBlack [(gPointMax % 100) / 10];
			obj.transform.Find ("waku/10").gameObject.SetActive (true);
			_flag = true;
		} else {
			obj.transform.Find ("waku/10").gameObject.SetActive (false);
		}

		obj.transform.Find ("waku/1").transform.GetComponent<Image> ().sprite = SuujiBlack [gPointMax % 10];
		obj.transform.Find ("waku/1").gameObject.SetActive (true);
	}

	private void pointSendSet (){
		EventTushinPoint.transform.Find ("waku/01000").transform.GetComponent<Image> ().sprite = SuujiBlack [(gPointNow % 10000) / 1000];
		EventTushinPoint.transform.Find ("waku/00100").transform.GetComponent<Image> ().sprite = SuujiBlack [(gPointNow % 1000) / 100];
		EventTushinPoint.transform.Find ("waku/00010").transform.GetComponent<Image> ().sprite = SuujiBlack [(gPointNow % 100) / 10];
		EventTushinPoint.transform.Find ("waku/00001").transform.GetComponent<Image> ().sprite = SuujiBlack [(gPointNow % 10)];
	}
		
	private void pointKakuninSet(){
		bool _flag = false;

		if ((gPointNow / 1000) > 0) {
			EventDialogKakunin.transform.Find ("3/1000").transform.GetComponent<Image> ().sprite = SuujiBlack [gPointNow / 1000];
			EventDialogKakunin.transform.Find ("3/1000").gameObject.SetActive (true);
			_flag = true;
		} else {
			EventDialogKakunin.transform.Find ("3/1000").gameObject.SetActive (false);
		}

		if ((((gPointNow % 1000) / 100) > 0) || (_flag)) {
			EventDialogKakunin.transform.Find ("3/100").transform.GetComponent<Image> ().sprite = SuujiBlack [(gPointNow % 1000) / 100];
			EventDialogKakunin.transform.Find ("3/100").gameObject.SetActive (true);
			_flag = true;
		} else {
			EventDialogKakunin.transform.Find ("3/100").gameObject.SetActive (false);
		}

		if ((((gPointNow % 100) / 10) > 0) || (_flag)) {
			EventDialogKakunin.transform.Find ("3/10").transform.GetComponent<Image> ().sprite = SuujiBlack [(gPointNow % 100) / 10];
			EventDialogKakunin.transform.Find ("3/10").gameObject.SetActive (true);
			_flag = true;
		} else {
			EventDialogKakunin.transform.Find ("3/10").gameObject.SetActive (false);
		}

		EventDialogKakunin.transform.Find ("3/1").transform.GetComponent<Image> ().sprite = SuujiBlack [gPointNow % 10];
		EventDialogKakunin.transform.Find ("3/1").gameObject.SetActive (true);
	}
}
