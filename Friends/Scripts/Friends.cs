using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

public class Friends : MonoBehaviour,IReceiver {
	[SerializeField] private GameObject TamagoBase;

	[SerializeField] private GameObject EventMenu;			// 初期選択画面
	[SerializeField] private GameObject EventNoteApp;		// 友達手帳アプリの友達
	[SerializeField] private GameObject EventNoteMIX2;		// 友達手帳MIX2の友達
	[SerializeField] private GameObject EventNoteRenraku;	// 友達手帳の連絡帳
	[SerializeField] private GameObject EventSearch;		// 友達検索画面
	[SerializeField] private GameObject EventResult;		// 結果画面
	[SerializeField] private GameObject EventKakunin;		// 確認画面

	[SerializeField] private Button BtnMenuTecho;			// 友達手帳へのボタン
	[SerializeField] private Button BtnMenuSearch;			// 友達検索へのボタン
	[SerializeField] private Button BtnMenuTojiru;			// 終了ボタン

	[SerializeField] private Button BtnNoteAppApp;			// 友達手帳アプリの友達へのボタン（未使用）
	[SerializeField] private Button BtnNoteAppMIX2;			// 友達手帳MIX2の友達へのボタン
	[SerializeField] private Button BtnNoteAppRenraku;		// 友達手帳の連絡帳へのボタン
	[SerializeField] private Button BtnNoteAppBack;			// 初期選択画面へのボタン
		
	[SerializeField] private Button BtnNoteMIX2App;			// 友達手帳アプリの友達へのボタン
	[SerializeField] private Button BtnNoteMIX2MIX2;		// 友達手帳MIX2の友達へのボタン（未使用）
	[SerializeField] private Button BtnNoteMIX2Renraku;		// 友達手帳の連絡帳へのボタン
	[SerializeField] private Button BtnNoteMIX2Back;		// 初期選択画面へのボタン

	[SerializeField] private Button BtnNoteRenrakuApp;		// 友達手帳アプリの友達へのボタン
	[SerializeField] private Button BtnNoteRenrakuMIX2;		// 友達手帳MIX2の友達へのボタン
	[SerializeField] private Button BtnNoteRenrakuRenraku;	// 友達手帳の連絡帳へのボタン（未使用）
	[SerializeField] private Button BtnNoteRenrakuBack;		// 初期選択画面へのボタン

	[SerializeField] private Button BtnSearchInit;			// はじめからボタン
	[SerializeField] private Button BtnSearchSearch;		// 探すボタン
	[SerializeField] private Button BtnSearchBack;			// 初期選択画面へのボタン

	[SerializeField] private Button BtnResultBack;			// もどるボタン

	[SerializeField] private Button BtnKakuninBack;			// もどるボタン
	[SerializeField] private Button BtnKakuninTojiru;		// とじるボタン
	[SerializeField] private Button BtnKakuninYes;			// はいボタン
	[SerializeField] private Button BtnKakuninNo;			// いいえボタン

	[SerializeField] private Sprite[] ImgNumber;			// ００から２０まで

	[SerializeField] private GameObject PrefabApp;
	[SerializeField] private GameObject PrefabMIX2;
	[SerializeField] private GameObject	PrefabRenraku;
	[SerializeField] private GameObject PrefabSearch;
	[SerializeField] private GameObject PrefabChara;

	private object[]		mparam;
	private User muser1;//自分
	private FriendData		mFriendData;
	private List<User> 		mFriendSearchData;


	private GameObject[]	prefabObjApp = new GameObject[10];					// アプリの友達
	private GameObject[]	prefabObjMIX2 = new GameObject[20];					// 玩具の友達
	private GameObject[]	prefabObjRenraku = new GameObject[50];				// 友達申請（最大値は不明）
	private GameObject[]	prefabObjSearch = new GameObject[50];				// 友達検索結果（最大値は不明）

	private CharaBehaviour[] cbApp = new CharaBehaviour[10];
	private CharaBehaviour[] cbMIX2 = new CharaBehaviour[20];
	private CharaBehaviour[] cbRenraku = new CharaBehaviour[50];
	private CharaBehaviour[] cbSearch = new CharaBehaviour[50];

	private GameObject[] prefabCharaApp = new GameObject[10];
	private GameObject[] prefabCharaMIX2 = new GameObject[20];
	private GameObject[] prefabCharaRenraku = new GameObject[50];
	private GameObject[] prefabCharaSearch = new GameObject[50];

	void Awake(){
		Debug.Log ("Friends Awake");
		mparam=null;
		muser1=null;
	}

	public void receive(params object[] parameter){
		Debug.Log ("Friends receive");
		mparam = parameter;
	}

	void Start() {
		Debug.Log ("Friends start");

		//ルーム情報取得、メンバーマッチング処理
		//パラメタは設計書参照
		GameCall call = new GameCall (CallLabel.GET_FRIEND_INFO);
		call.AddListener (mGetFriendInfo);
		ManagerObject.instance.connect.send (call);
	}

	void mGetFriendInfo(bool success,object data){
		mFriendData = (FriendData)data;
		StartCoroutine(mstart());
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



		for (int i = 0; i < prefabCharaApp.Length; i++) {
			prefabCharaApp [i] = (GameObject)Instantiate (PrefabChara);
			prefabCharaApp [i].transform.SetParent (TamagoBase.transform.Find ("App").transform, false);
			prefabCharaApp [i].name = "CharaApp" + i.ToString ();
		}
		for (int i = 0; i < prefabCharaMIX2.Length; i++) {
			prefabCharaMIX2 [i] = (GameObject)Instantiate (PrefabChara);
			prefabCharaMIX2 [i].transform.SetParent (TamagoBase.transform.Find ("MIX2").transform, false);
			prefabCharaMIX2 [i].name = "CharaMIX" + i.ToString ();
		}
		for (int i = 0; i < prefabCharaRenraku.Length; i++) {
			prefabCharaRenraku [i] = (GameObject)Instantiate (PrefabChara);
			prefabCharaRenraku [i].transform.SetParent (TamagoBase.transform.Find ("Renraku").transform, false);
			prefabCharaRenraku [i].name = "CharaRenraku" + i.ToString ();
		}
		for (int i = 0; i < prefabCharaSearch.Length; i++) {
			prefabCharaSearch [i] = (GameObject)Instantiate (PrefabChara);
			prefabCharaSearch [i].transform.SetParent (TamagoBase.transform.Find ("Search").transform, false);
			prefabCharaSearch [i].name = "CharaSearch" + i.ToString ();
		}


	
		// 友達手帳のデータを登録する
		NoteDataSet ();



		BtnMenuTecho.onClick.AddListener(BtnMenuTechoClick);					// 友達手帳へのボタン
		BtnMenuSearch.onClick.AddListener(BtnMenuSearchClick);					// 友達検索へのボタン
		BtnMenuTojiru.onClick.AddListener(BtnMenuTojiruClick);					// 終了ボタン

		BtnNoteAppApp.onClick.AddListener(BtnNoteAppAppClick);					// 友達手帳アプリの友達へのボタン（未使用）
		BtnNoteAppMIX2.onClick.AddListener(BtnNoteAppMIX2Click);				// 友達手帳MIX2の友達へのボタン
		BtnNoteAppRenraku.onClick.AddListener(BtnNoteAppRenrakuClick);			// 友達手帳の連絡帳へのボタン
		BtnNoteAppBack.onClick.AddListener(BtnNoteAppBackClick);				// 初期選択画面へのボタン

		BtnNoteMIX2App.onClick.AddListener(BtnNoteMIX2AppClick);				// 友達手帳アプリの友達へのボタン
		BtnNoteMIX2MIX2.onClick.AddListener(BtnNoteMIX2MIX2Click);				// 友達手帳MIX2の友達へのボタン（未使用）
		BtnNoteMIX2Renraku.onClick.AddListener(BtnNoteMIX2RenrakuClick);		// 友達手帳の連絡帳へのボタン
		BtnNoteMIX2Back.onClick.AddListener(BtnNoteMIX2BackClick);				// 初期選択画面へのボタン

		BtnNoteRenrakuApp.onClick.AddListener(BtnNoteRenrakuAppClick);			// 友達手帳アプリの友達へのボタン
		BtnNoteRenrakuMIX2.onClick.AddListener(BtnNoteRenrakuMIX2Click);		// 友達手帳MIX2の友達へのボタン
		BtnNoteRenrakuRenraku.onClick.AddListener(BtnNoteRenrakuRenrakuClick);	// 友達手帳の連絡帳へのボタン（未使用）
		BtnNoteRenrakuBack.onClick.AddListener(BtnNoteRenrakuBackClick);		// 初期選択画面へのボタン

		BtnSearchInit.onClick.AddListener(BtnSearchInitClick);					// はじめからボタン
		BtnSearchSearch.onClick.AddListener(BtnSearchSearchClick);				// 探すボタン
		BtnSearchBack.onClick.AddListener(BtnSearchBackClick);					// 初期選択画面へのボタン

		BtnResultBack.onClick.AddListener(BtnResultBackClick);					// もどるボタン

		BtnKakuninBack.onClick.AddListener(BtnKakuninBackClick);				// もどるボタン
		BtnKakuninTojiru.onClick.AddListener(BtnKakuninTojiruClick);			// とじるボタン
		BtnKakuninYes.onClick.AddListener(BtnKakuninYesClick);					// はいボタン
		BtnKakuninNo.onClick.AddListener(BtnKakuninNoClick);					// いいえボタン





		FriendListScrollFlag = true;
		StartCoroutine ("FriendAppListScroll");





		yield return null;
	}

	void Destroy(){
		FriendListScrollFlag = false;
		Debug.Log ("Friends Destroy");
	}


	// サーチ結果
	void mSearchFriend(bool success,object data)
	{
		Debug.LogFormat("Friends mSearchFriend:{0},{1}",success,data);

		if (success) {
			mFriendSearchData = (List<User>)data;
			if (mFriendSearchData.Count != 0) {
				SearchDataSet ();
			} else {
				// サーチは成功したが０件なので確認画面を表示
				EventKakunin.SetActive (true);
				EventKakunin.transform.Find ("Button_blue_tojiru").gameObject.SetActive (true);
				EventKakunin.transform.Find ("Text (2)").gameObject.SetActive (true);
			}
		} else {
			// サーチに失敗したので確認画面を表示
			EventKakunin.SetActive (true);
			EventKakunin.transform.Find ("Button_blue_tojiru").gameObject.SetActive (true);
			EventKakunin.transform.Find ("Text (2)").gameObject.SetActive (true);
		}
	}
		


	private swipIdouPage swipIdouFlag = swipIdouPage.Null;
	private enum swipIdouPage{
		Null,
		App,
		MIX2,
		Renraku,
		Search,
	};
	private float StartPos;
	private float EndPos;

	private friendListTable	friendListIdouFlag = friendListTable.Null;
	private enum friendListTable{
		Null,
		Down010,
		Down020,
		Down030,
		Up010,
		Up020,
		Up030,
	};
	void Update () {
		if (swipIdouFlag != swipIdouPage.Null) {
			if (Input.GetMouseButtonDown (0)) {
				StartPos = Input.mousePosition.y;
			}
			if (Input.GetMouseButtonUp (0)) {
				if ((StartPos != 0) && (friendListIdouFlag == friendListTable.Null)) {
					EndPos = Input.mousePosition.y;
					if (StartPos > EndPos) {
						friendListIdouFlag = friendListTable.Down010;
					} else if (StartPos < EndPos) {
						friendListIdouFlag = friendListTable.Up010;
					}
				}
				StartPos = 0;
				EndPos = 0;
			}
		} else {
			StartPos = 0;
			EndPos = 0;
			friendListIdouFlag = 0;
		}


		if (mFriendData.appfriends != null) {
			for (int i = 0; i < mFriendData.appfriends.Count; i++) {
				TamagochiImageMove (prefabObjApp [i], prefabCharaApp [i], "chara_waku/chara");
			}
		}
		if (mFriendData.toyfriends != null) {
			for (int i = 0; i < mFriendData.toyfriends.Count; i++) {
				TamagochiImageMove (prefabObjMIX2 [i], prefabCharaMIX2 [i], "chara_waku/chara");
			}
		}
		if (mFriendData.applys != null) {
			for (int i = 0; i < mFriendData.applys.Count; i++) {
				TamagochiImageMove (prefabObjRenraku [i], prefabCharaRenraku [i], "chara_waku/chara");
			}
		}
		if (mFriendSearchData != null) {
			for (int i = 0; i < mFriendSearchData.Count; i++) {
				TamagochiImageMove (prefabObjSearch [i], prefabCharaSearch [i], "chara_waku/chara");
			}
		}

	}



	private void TamagochiImageMove(GameObject toObj,GameObject fromObj,string toStr){
		for (int i = 0; i < fromObj.transform.Find ("Layers").transform.childCount; i++) {
			toObj.transform.Find (toStr + "/CharaImg/Layers/" + fromObj.transform.Find ("Layers").transform.GetChild (i).name).gameObject.transform.SetSiblingIndex (i);
		}

		toObj.transform.Find (toStr + "/CharaImg").gameObject.GetComponent<Image> ().enabled = false;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer0").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer0").gameObject.GetComponent<Image> ().enabled;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer1").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer1").gameObject.GetComponent<Image> ().enabled;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer2").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer2").gameObject.GetComponent<Image> ().enabled;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer3").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer3").gameObject.GetComponent<Image> ().enabled;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer4").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer4").gameObject.GetComponent<Image> ().enabled;

		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer0").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer0").gameObject.GetComponent<Image> ().sprite;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer1").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer1").gameObject.GetComponent<Image> ().sprite;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer2").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer2").gameObject.GetComponent<Image> ().sprite;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer3").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer3").gameObject.GetComponent<Image> ().sprite;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer4").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer4").gameObject.GetComponent<Image> ().sprite;

		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer0").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer0").gameObject.transform.localPosition;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer1").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer1").gameObject.transform.localPosition;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer2").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer2").gameObject.transform.localPosition;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer3").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer3").gameObject.transform.localPosition;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer4").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer4").gameObject.transform.localPosition;

		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer0").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer0").gameObject.transform.localScale;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer1").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer1").gameObject.transform.localScale;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer2").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer2").gameObject.transform.localScale;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer3").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer3").gameObject.transform.localScale;
		toObj.transform.Find (toStr + "/CharaImg/Layers/Layer4").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer4").gameObject.transform.localScale;
	}








	private bool FriendListScrollFlag;
	private float _NextPos;
	float[] NextYPosTableApp = new float[5]{
		0.0f,  350.0f, 700.0f,1050.0f,1400.0f,
	};
	private float[] NextYPosTableMIX2 = new float[50]{
		    0.0f,    0.0f,  240.0f,  480.0f,  720.0f,
		  960.0f, 1200.0f, 1440.0f, 1680.0f, 1920.0f,
		 2160.0f, 2400.0f, 2640.0f, 2880.0f, 3120.0f,
		 3360.0f, 3600.0f, 3840.0f, 4080.0f, 4320.0f,
		 4560.0f, 4800.0f, 5040.0f, 5280.0f, 5520.0f,
		 5760.0f, 6000.0f, 6240.0f, 6480.0f, 6720.0f,
		 6960.0f, 7200.0f, 7440.0f, 7680.0f, 7920.0f,
		 8160.0f, 8400.0f, 8640.0f, 8880.0f, 9120.0f,
		 9360.0f, 9600.0f, 9840.0f,10080.0f,10320.0f,
		10560.0f,10800.0f,11040.0f,11280.0f,11520.0f,
	};

	private IEnumerator FriendAppListScroll(){
		Vector3 _Pos = new Vector3(0.0f,0.0f,0.0f);

		while (FriendListScrollFlag) {
			switch (friendListIdouFlag) {
			case	friendListTable.Down010:
				{
					switch (swipIdouFlag) {
					case	swipIdouPage.App:
						{
							_NextPos = EventNoteApp.transform.Find ("daishi/mask/Panel").transform.localPosition.y - 350.0f;
							break;
						}
					case	swipIdouPage.MIX2:
						{
							_NextPos = EventNoteMIX2.transform.Find ("daishi/mask/Panel").transform.localPosition.y - 240.0f;
							break;
						}
					case	swipIdouPage.Renraku:
						{
							_NextPos = EventNoteRenraku.transform.Find ("daishi/mask/Panel").transform.localPosition.y - 240.0f;
							break;
						}
					case	swipIdouPage.Search:
						{
							_NextPos = EventSearch.transform.Find ("mask/Panel").transform.localPosition.y - 240.0f;
							break;
						}
					}
					if (_NextPos < 0.0f) {
						friendListIdouFlag = friendListTable.Null;
					} else {
						friendListIdouFlag = friendListTable.Down020;
					}
					break;
				}
			case	friendListTable.Down020:
				{
					switch(swipIdouFlag){
					case	swipIdouPage.App:
						{
							_Pos = EventNoteApp.transform.Find ("daishi/mask/Panel").transform.localPosition;
							_Pos.y = _Pos.y - (35.0f * (60.0f * Time.deltaTime));
							break;
						}
					case	swipIdouPage.MIX2:
						{
							_Pos = EventNoteMIX2.transform.Find ("daishi/mask/Panel").transform.localPosition;
							_Pos.y = _Pos.y - (24.0f * (60.0f * Time.deltaTime));
							break;
						}
					case	swipIdouPage.Renraku:
						{
							_Pos = EventNoteRenraku.transform.Find ("daishi/mask/Panel").transform.localPosition;
							_Pos.y = _Pos.y - (24.0f * (60.0f * Time.deltaTime));
							break;
						}
					case	swipIdouPage.Search:
						{
							_Pos = EventSearch.transform.Find ("mask/Panel").transform.localPosition;
							_Pos.y = _Pos.y - (24.0f * (60.0f * Time.deltaTime));
							break;
						}
					}
					if (_Pos.y <= _NextPos) {
						_Pos.y = _NextPos;
						friendListIdouFlag = friendListTable.Null;
					}
					switch(swipIdouFlag){
					case	swipIdouPage.App:
						{
							EventNoteApp.transform.Find ("daishi/mask/Panel").transform.localPosition = _Pos;
							break;
						}
					case	swipIdouPage.MIX2:
						{
							EventNoteMIX2.transform.Find("daishi/mask/Panel").transform.localPosition = _Pos;
							break;
						}
					case	swipIdouPage.Renraku:
						{
							EventNoteRenraku.transform.Find("daishi/mask/Panel").transform.localPosition = _Pos;
							break;
						}
					case	swipIdouPage.Search:
						{
							EventSearch.transform.Find("mask/Panel").transform.localPosition = _Pos;
							break;
						}
					}
					break;
				}
			case	friendListTable.Down030:
				{
					break;
				}



			case	friendListTable.Up010:
				{
					float _NextStopPos = 0.0f;

					switch(swipIdouFlag){
					case	swipIdouPage.App:
						{
							_NextPos = EventNoteApp.transform.Find ("daishi/mask/Panel").transform.localPosition.y + 350.0f;
							int _cnt = mFriendData.appfriends.Count;
							if (_cnt != 0) {
								_cnt--;
							}
							_NextStopPos = NextYPosTableApp [_cnt >> 1];
							break;
						}
					case	swipIdouPage.MIX2:
						{
							_NextPos = EventNoteMIX2.transform.Find("daishi/mask/Panel").transform.localPosition.y + 240.0f;
							int _cnt = mFriendData.toyfriends.Count;
							if(_cnt != 0){
								_cnt--;
							}
							_NextStopPos = NextYPosTableMIX2[_cnt >> 1];
							break;
						}
					case	swipIdouPage.Renraku:
						{
							_NextPos = EventNoteRenraku.transform.Find("daishi/mask/Panel").transform.localPosition.y + 240.0f;
							int _cnt = mFriendData.applys.Count;
							if (_cnt != 0) {
								_cnt--;
							}
							_NextStopPos = NextYPosTableMIX2[_cnt];
							break;
						}
					case	swipIdouPage.Search:
						{
							_NextPos = EventSearch.transform.Find("mask/Panel").transform.localPosition.y + 240.0f;
							int _cnt = SearchNumber;
							if (_cnt != 0) {
								_cnt--;
							}
							_NextStopPos = NextYPosTableMIX2[_cnt];
							break;
						}
					}

					if (_NextPos > _NextStopPos) {
						friendListIdouFlag = friendListTable.Null;
					} else {
						friendListIdouFlag = friendListTable.Up020;
					}
					break;
				}
			case	friendListTable.Up020:
				{
					switch (swipIdouFlag) {
					case	swipIdouPage.App:
						{
							_Pos = EventNoteApp.transform.Find ("daishi/mask/Panel").transform.localPosition;
							_Pos.y = _Pos.y + (35.0f * (60.0f * Time.deltaTime));
							break;
						}
					case	swipIdouPage.MIX2:
						{
							_Pos = EventNoteMIX2.transform.Find ("daishi/mask/Panel").transform.localPosition;
							_Pos.y = _Pos.y + (24.0f * (60.0f * Time.deltaTime));
							break;
						}
					case	swipIdouPage.Renraku:
						{
							_Pos = EventNoteRenraku.transform.Find ("daishi/mask/Panel").transform.localPosition;
							_Pos.y = _Pos.y + (24.0f * (60.0f * Time.deltaTime));
							break;
						}
					case	swipIdouPage.Search:
						{
							_Pos = EventSearch.transform.Find ("mask/Panel").transform.localPosition;
							_Pos.y = _Pos.y + (24.0f * (60.0f * Time.deltaTime));
							break;
						}
					}
					if (_Pos.y >= _NextPos) {
						_Pos.y = _NextPos;
						friendListIdouFlag = friendListTable.Null;
					}
					switch (swipIdouFlag) {
					case	swipIdouPage.App:
						{
							EventNoteApp.transform.Find ("daishi/mask/Panel").transform.localPosition = _Pos;
							break;
						}
					case	swipIdouPage.MIX2:
						{
							EventNoteMIX2.transform.Find ("daishi/mask/Panel").transform.localPosition = _Pos;
							break;
						}
					case	swipIdouPage.Renraku:
						{
							EventNoteRenraku.transform.Find ("daishi/mask/Panel").transform.localPosition = _Pos;
							break;
						}
					case	swipIdouPage.Search:
						{
							EventSearch.transform.Find ("mask/Panel").transform.localPosition = _Pos;
							break;
						}
					}
					break;
				}
			case	friendListTable.Up030:
				{
					break;
				}
			}
			yield return null;
		}
	}

	// 友達手帳へのボタン
	private void BtnMenuTechoClick(){
		FriendNoteChange (friendTabTable.NoteApp);
	}
	// 友達検索へのボタン
	private void BtnMenuSearchClick(){
		EventSearch.SetActive (true);
		SearchDataSet ();
		swipIdouFlag = swipIdouPage.Search;
		friendListIdouFlag = friendListTable.Null;
	}
	// 終了ボタン
	private void BtnMenuTojiruClick(){
		Debug.Log ("たまタウンへ・・・");
		ManagerObject.instance.view.change(SceneLabel.TOWN);
	}

	// 友達手帳アプリの友達へのボタン（未使用）
	private void BtnNoteAppAppClick(){
	}
	// 友達手帳MIX2の友達へのボタン
	private void BtnNoteAppMIX2Click(){
		FriendNoteChange (friendTabTable.NoteMIX2);
	}
	// 友達手帳の連絡帳へのボタン
	private void BtnNoteAppRenrakuClick(){
		FriendNoteChange (friendTabTable.NoteRenraku);
	}
	// 初期選択画面へのボタン
	private void BtnNoteAppBackClick(){
		FriendNoteChange (friendTabTable.NoteBack);
	}

	// 友達手帳アプリの友達へのボタン
	private void BtnNoteMIX2AppClick(){
		FriendNoteChange (friendTabTable.NoteApp);
	}
	// 友達手帳MIX2の友達へのボタン（未使用）
	private void BtnNoteMIX2MIX2Click(){
	}
	// 友達手帳の連絡帳へのボタン
	private void BtnNoteMIX2RenrakuClick(){
		FriendNoteChange (friendTabTable.NoteRenraku);
	}
	// 初期選択画面へのボタン
	private void BtnNoteMIX2BackClick(){
		FriendNoteChange (friendTabTable.NoteBack);
	}

	// 友達手帳アプリの友達へのボタン
	private void BtnNoteRenrakuAppClick(){
		FriendNoteChange (friendTabTable.NoteApp);
	}
	// 友達手帳MIX2の友達へのボタン
	private void BtnNoteRenrakuMIX2Click(){
		FriendNoteChange (friendTabTable.NoteMIX2);
	}
	// 友達手帳の連絡帳へのボタン（未使用）
	private void BtnNoteRenrakuRenrakuClick(){
	}
	// 初期選択画面へのボタン
	private void BtnNoteRenrakuBackClick(){
		FriendNoteChange (friendTabTable.NoteBack);
	}

	// はじめからボタン
	private void BtnSearchInitClick(){
		EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().characterLimit = 20;
		EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().contentType = InputField.ContentType.Standard;
		EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().text = "みーつID を いれてね";
		EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().characterLimit = 9;
		EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().contentType = InputField.ContentType.Alphanumeric;
	}
	// 探すボタン
	private void BtnSearchSearchClick(){
		string _data = EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().text;

		if ((_data == "") || (_data.Length > 9)) {
			EventKakunin.SetActive (true);
			EventKakunin.transform.Find ("Button_blue_tojiru").gameObject.SetActive (true);
			EventKakunin.transform.Find ("Text (2)").gameObject.SetActive (true);
		} else {
			// フレンドのID検索
			GameCall call = new GameCall (CallLabel.SEARCH_FRIEND, _data);
			call.AddListener (mSearchFriend);
			ManagerObject.instance.connect.send (call);
		}
	}
	// 初期選択画面へのボタン
	private void BtnSearchBackClick(){
		EventSearch.SetActive (false);
		swipIdouFlag = swipIdouPage.Null;
		friendListIdouFlag = friendListTable.Null;
	}

	// もどるボタン
	private void BtnResultBackClick(){
		EventResult.transform.Find ("0_touroku zumi").gameObject.SetActive (false);
		EventResult.transform.Find ("1_shinsei shimashita").gameObject.SetActive (false);
		EventResult.transform.Find ("2_tomodachini narimashita").gameObject.SetActive (false);
		EventResult.transform.Find ("3_touroku dekimasen").gameObject.SetActive (false);
		EventResult.transform.Find ("Text_Arial").gameObject.SetActive (false);
		EventResult.transform.Find ("Text 0").gameObject.SetActive (false);
		EventResult.transform.Find ("Text 1").gameObject.SetActive (false);
		EventResult.transform.Find ("Text 2").gameObject.SetActive (false);
		EventResult.transform.Find ("Text 3-1").gameObject.SetActive (false);
		EventResult.transform.Find ("Text 3-2").gameObject.SetActive (false);
		EventResult.transform.Find ("Text 4").gameObject.SetActive (false);

		EventResult.SetActive (false);
	}

	// もどるボタン
	private void BtnKakuninBackClick(){
	}
	// とじるボタン
	private void BtnKakuninTojiruClick(){
		KakuninModeOff ();
		mFriendSearchData = null;
		SearchDataSet ();
	}
	// はいボタン
	private void BtnKakuninYesClick(){
		KakuninModeOff ();

		switch (kakuninYesNoMode) {
		case	0:
			{	// フレンド申請
				int _id = int.Parse (prefabObjSearch [ReqUserNumber].transform.Find ("ID").gameObject.GetComponent<Text> ().text);

				GameCall call = new GameCall (CallLabel.APPLY_FRIEND, _id);
				call.AddListener (mApplyFriend);
				ManagerObject.instance.connect.send (call);
				break;
			}
		case	1:
			{	// フレンドを削除する
				int _id = mFriendData.appfriends[SakujoNumber].id;

				GameCall call = new GameCall (CallLabel.DELETE_FRIEND, _id);
				call.AddListener (mDeleteFriend);
				ManagerObject.instance.connect.send (call);
				break;
			}
		case	2:
		case	3:
			{	// フレンド申請に対する返答
				int _id = mFriendData.applys [FriendYNNumber].user.id;
				bool _flag;
				if (kakuninYesNoMode == 2) {
					_flag = true;				// 承認
				} else {
					_flag = false;				// 却下
				}
				GameCall call = new GameCall (CallLabel.REPLY_FRIEND, _id, _flag);
				call.AddListener (mReplyFriend);
				ManagerObject.instance.connect.send (call);
				break;
			}
		}
	}
	// いいえボタン
	private void BtnKakuninNoClick(){
		KakuninModeOff ();
	}



	private void KakuninModeOff(){
		EventKakunin.transform.Find ("Button_blue_modoru").gameObject.SetActive (false);
		EventKakunin.transform.Find ("Button_blue_tojiru").gameObject.SetActive (false);
		EventKakunin.transform.Find ("Text_Arial").gameObject.SetActive (false);
		EventKakunin.transform.Find ("Text (1)").gameObject.SetActive (false);
		EventKakunin.transform.Find ("Text (2)").gameObject.SetActive (false);
		EventKakunin.transform.Find ("Text (3)").gameObject.SetActive (false);
		EventKakunin.transform.Find ("Button_red_hai").gameObject.SetActive (false);
		EventKakunin.transform.Find ("Button_blue_iie").gameObject.SetActive (false);

		EventKakunin.SetActive (false);
	}




	private enum friendTabTable{
		NoteApp,
		NoteMIX2,
		NoteRenraku,
		NoteBack,
	};

	private void FriendNoteChange(friendTabTable type){
		Vector3 _Pos = new Vector3(0.0f,0.0f,0.0f);

		switch (type) {
		case	friendTabTable.NoteApp:
			{
				EventNoteApp.SetActive (true);
				EventNoteMIX2.SetActive (false);
				EventNoteRenraku.SetActive (false);
				swipIdouFlag = swipIdouPage.App;
				break;
			}
		case	friendTabTable.NoteMIX2:
			{
				EventNoteApp.SetActive (false);
				EventNoteMIX2.SetActive (true);
				EventNoteRenraku.SetActive (false);
				swipIdouFlag = swipIdouPage.MIX2;
				break;
			}
		case	friendTabTable.NoteRenraku:
			{
				EventNoteApp.SetActive (false);
				EventNoteMIX2.SetActive (false);
				EventNoteRenraku.SetActive (true);
				swipIdouFlag = swipIdouPage.Renraku;
				break;
			}
		case	friendTabTable.NoteBack:
			{
				EventNoteApp.SetActive (false);
				EventNoteMIX2.SetActive (false);
				EventNoteRenraku.SetActive (false);
				swipIdouFlag = swipIdouPage.Null;
				break;
			}
		}
		EventNoteApp.transform.Find ("daishi/mask/Panel").transform.localPosition = _Pos;
		EventNoteMIX2.transform.Find ("daishi/mask/Panel").transform.localPosition = _Pos;
		EventNoteRenraku.transform.Find ("daishi/mask/Panel").transform.localPosition = _Pos;

		friendListIdouFlag = friendListTable.Null;
	}



	private float[] prefabObjAppXPosTable = new float[2]{
		-310.0f,310.0f,
	};
	private float[] prefabObjMIX2XPosTable = new float[2]{
		-310.0f,310.0f,
	};

	// 友達手帳のデータを登録する
	private void NoteDataSet(){
		Vector3 _Pos = new Vector3 (0.0f, 0.0f, 0.0f);
		string mesRet = "\n";
		string mesData;

		// スクロールリセット
		EventNoteApp.transform.Find ("daishi/mask/Panel").transform.localPosition = _Pos;
		EventNoteMIX2.transform.Find ("daishi/mask/Panel").transform.localPosition = _Pos;
		EventNoteRenraku.transform.Find ("daishi/mask/Panel").transform.localPosition = _Pos;

		// アプリのともだちの現状人数
		EventNoteApp.transform.Find ("daishi/tab1/05").gameObject.GetComponent<Image> ().sprite = ImgNumber [mFriendData.appfriends.Count];
		EventNoteMIX2.transform.Find ("daishi/tab1/05").gameObject.GetComponent<Image> ().sprite = ImgNumber [mFriendData.appfriends.Count];
		EventNoteRenraku.transform.Find ("daishi/tab1/05").gameObject.GetComponent<Image> ().sprite = ImgNumber [mFriendData.appfriends.Count];

		// 玩具のともだちの現状人数
		EventNoteApp.transform.Find ("daishi/tab2/05").gameObject.GetComponent<Image> ().sprite = ImgNumber [mFriendData.toyfriends.Count];
		EventNoteMIX2.transform.Find ("daishi/tab2/05").gameObject.GetComponent<Image> ().sprite = ImgNumber [mFriendData.toyfriends.Count];
		EventNoteRenraku.transform.Find ("daishi/tab2/05").gameObject.GetComponent<Image> ().sprite = ImgNumber [mFriendData.toyfriends.Count];

		// アプリのともだちパネルセット
		for (int i = 0; i < mFriendData.appfriends.Count; i++) {
			prefabObjApp [i] = (GameObject)Instantiate (PrefabApp);
			prefabObjApp [i].transform.SetParent (EventNoteApp.transform.Find ("daishi/mask/Panel").transform, false);
			prefabObjApp [i].name = "friendApp" + i.ToString ();

			int ii = i + 0;
			// 削除ボタンを有効化
			prefabObjApp [i].transform.Find("Button_sakujo").gameObject.GetComponent<Button> ().onClick.AddListener (() => BtnSakujoReq (ii));

			_Pos.x = prefabObjAppXPosTable [i & 1];
			_Pos.y = 100.0f - (350.0f * (i >> 1));
			_Pos.z = 0.0f;
			prefabObjApp [i].transform.localPosition = _Pos;

			prefabObjApp [i].transform.Find ("name_daishi/Text").gameObject.GetComponent<Text> ().text = mFriendData.appfriends [i].nickname;

			mesData = mFriendData.appfriends [i].chara1.cname;
			if (mFriendData.appfriends [i].chara2 != null) {
				mesData = mesData + mesRet + mFriendData.appfriends [i].chara2.cname;
			}
			prefabObjApp [i].transform.Find ("name_daishi/Text (2)").gameObject.GetComponent<Text> ().text = mesData;

			StartCoroutine (cbAppDataSet (i));
		}
		// 玩具のともだちパネルセット
		for (int i = 0; i < mFriendData.toyfriends.Count; i++) {
			prefabObjMIX2 [i] = (GameObject)Instantiate (PrefabMIX2);
			prefabObjMIX2 [i].transform.SetParent (EventNoteMIX2.transform.Find ("daishi/mask/Panel").transform, false);
			prefabObjMIX2 [i].name = "friendMIX2" + i.ToString ();

			_Pos.x = prefabObjMIX2XPosTable [i & 1];
			_Pos.y = 150.0f - (240.0f * (i >> 1));
			_Pos.z = 0.0f;
			prefabObjMIX2 [i].transform.localPosition = _Pos;

			prefabObjMIX2 [i].transform.Find ("name_daishi/Text").gameObject.GetComponent<Text> ().text = mFriendData.toyfriends [i].nickname;
			mesData = mFriendData.toyfriends [i].chara1.cname;
			if (mFriendData.toyfriends [i].chara2 != null) {
				mesData = mesData + mesRet + mFriendData.toyfriends [i].chara2.cname;
			}
			prefabObjMIX2 [i].transform.Find ("name_daishi/Text (2)").gameObject.GetComponent<Text> ().text = mesData;

			StartCoroutine (cbMIX2DataSet (i));
		}
		// 連絡のともだちパネルセット
		for (int i = 0; i < mFriendData.applys.Count; i++) {
			prefabObjRenraku [i] = (GameObject)Instantiate (PrefabRenraku);
			prefabObjRenraku [i].transform.SetParent (EventNoteRenraku.transform.Find ("daishi/mask/Panel").transform, false);
			prefabObjRenraku [i].name = "friendRenraku" + i.ToString ();

			int ii = i + 0;
			// ともだちになるボタンを有効化
			prefabObjRenraku [i].transform.Find("Button_red").gameObject.GetComponent<Button> ().onClick.AddListener (() => BtnFriendOKReq (ii));
			// ともだちにならないボタンを有効化
			prefabObjRenraku [i].transform.Find ("Button_blue").gameObject.GetComponent<Button> ().onClick.AddListener (() => BtnFriendNOReq (ii));

			_Pos.x = 0.0f;
			_Pos.y = 150.0f - (240.0f * i);
			_Pos.z = 0.0f;
			prefabObjRenraku [i].transform.localPosition = _Pos;

			prefabObjRenraku [i].transform.Find ("name_daishi/Text").gameObject.GetComponent<Text> ().text = mFriendData.applys [i].user.nickname;
			mesData = mFriendData.applys [i].user.chara1.cname;
			if (mFriendData.applys [i].user.chara2 != null) {
				mesData = mesData + mesRet + mFriendData.applys [i].user.chara2.cname;
			}
			prefabObjRenraku [i].transform.Find ("name_daishi/Text (2)").gameObject.GetComponent<Text> ().text = mesData;

			StartCoroutine (cbRenrakuDataSet (i));
		}
	}
	private IEnumerator cbAppDataSet(int num){
		cbApp [num] = prefabCharaApp [num].GetComponent<CharaBehaviour> ();
		yield return cbApp[num].init (mFriendData.appfriends[num].chara1);
		cbApp [num].gotoAndPlay (MotionLabel.IDLE);
	}
	private IEnumerator cbMIX2DataSet(int num){
		cbMIX2 [num] = prefabCharaMIX2 [num].GetComponent<CharaBehaviour> ();
		yield return cbMIX2 [num].init (mFriendData.toyfriends [num].chara1);
		cbMIX2 [num].gotoAndPlay (MotionLabel.IDLE);
	}
	private IEnumerator cbRenrakuDataSet (int num){
		cbRenraku [num] = prefabCharaRenraku [num].GetComponent<CharaBehaviour> ();
		yield return cbRenraku [num].init (mFriendData.applys [num].user.chara1);
		cbRenraku [num].gotoAndPlay (MotionLabel.IDLE);
	}

	private int FriendYNNumber;
	// 連絡のともだちになるボタン
	private void BtnFriendOKReq(int num){
		FriendYNNumber = num;
		string _mes;

		EventKakunin.SetActive (true);
		EventKakunin.transform.Find ("Text_Arial").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Text (1)").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_red_hai").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_blue_iie").gameObject.SetActive (true);
		kakuninYesNoMode = 2;

		EventKakunin.transform.Find ("Text_Arial").gameObject.GetComponent<Text> ().text = mFriendData.appfriends [num].nickname;
		if (mFriendData.appfriends [num].chara2 != null) {
			_mes = MsgDataTable_1;
			_mes = _mes + mFriendData.appfriends [num].chara1.cname;
			_mes = _mes + MsgDataTable_2;
			_mes = _mes + mFriendData.appfriends [num].chara2.cname;
			_mes = _mes + MsgDataTable_6;
		} else {
			_mes = MsgDataTable_1;
			_mes = _mes + mFriendData.appfriends [num].chara1.cname;
			_mes = _mes + MsgDataTable_6;
		}
		EventKakunin.transform.Find ("Text (1)").gameObject.GetComponent<Text> ().text = _mes;
	}
	// 連絡のともだちにならないボタン
	private void BtnFriendNOReq(int num){
		FriendYNNumber = num;
		string _mes;

		EventKakunin.SetActive (true);
		EventKakunin.transform.Find ("Text_Arial").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Text (1)").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_red_hai").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_blue_iie").gameObject.SetActive (true);
		kakuninYesNoMode = 3;

		EventKakunin.transform.Find ("Text_Arial").gameObject.GetComponent<Text> ().text = mFriendData.appfriends [num].nickname;
		if (mFriendData.appfriends [num].chara2 != null) {
			_mes = MsgDataTable_1;
			_mes = _mes + mFriendData.appfriends [num].chara1.cname;
			_mes = _mes + MsgDataTable_2;
			_mes = _mes + mFriendData.appfriends [num].chara2.cname;
			_mes = _mes + MsgDataTable_7;
		} else {
			_mes = MsgDataTable_1;
			_mes = _mes + mFriendData.appfriends [num].chara1.cname;
			_mes = _mes + MsgDataTable_7;
		}
		EventKakunin.transform.Find ("Text (1)").gameObject.GetComponent<Text> ().text = _mes;
	}

	private int SakujoNumber;
	// アプリのともだち削除ボタン
	private void  BtnSakujoReq(int num){
		string _mes;
		SakujoNumber = num;

		EventKakunin.SetActive (true);
		EventKakunin.transform.Find ("Text_Arial").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Text (1)").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_red_hai").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_blue_iie").gameObject.SetActive (true);
		kakuninYesNoMode = 1;

		EventKakunin.transform.Find ("Text_Arial").gameObject.GetComponent<Text> ().text = mFriendData.appfriends [num].nickname;
		if (mFriendData.appfriends [num].chara2 != null) {
			_mes = MsgDataTable_1;
			_mes = _mes + mFriendData.appfriends [num].chara1.cname;
			_mes = _mes + MsgDataTable_2;
			_mes = _mes + mFriendData.appfriends [num].chara2.cname;
			_mes = _mes + MsgDataTable_5;
		} else {
			_mes = MsgDataTable_1;
			_mes = _mes + mFriendData.appfriends [num].chara1.cname;
			_mes = _mes + MsgDataTable_5;
		}
		EventKakunin.transform.Find ("Text (1)").gameObject.GetComponent<Text> ().text = _mes;
	}



	private int SearchNumber = 0;
	// ともだち検索データセット
	private void SearchDataSet(){
		SearchDataClr ();

		if (mFriendSearchData == null) {
			SearchNumber = 0;
		} else {
			Vector3 _Pos;
			string mesRet = "\n";
			string mesData;

			// ともだち検索パネルセット
			for (int i = 0; i < mFriendSearchData.Count; i++) {
				prefabObjSearch [i] = (GameObject)Instantiate (PrefabSearch);
				prefabObjSearch [i].transform.SetParent (EventSearch.transform.Find ("mask/Panel").transform, false);
				prefabObjSearch [i].name = "friendSearch" + i.ToString ();

				int ii = i + 0;
				// ともだちになりたいボタンを有効化
				prefabObjSearch [i].transform.Find("Button_red").gameObject.GetComponent<Button> ().onClick.AddListener (() => BtnSearchReq (ii));

				_Pos.x = 0.0f;
				_Pos.y = 150.0f - (240.0f * i);
				_Pos.z = 0.0f;
				prefabObjSearch [i].transform.localPosition = _Pos;

				prefabObjSearch [i].transform.Find ("ID").gameObject.GetComponent<Text> ().text = mFriendSearchData [i].id.ToString ();
				prefabObjSearch [i].transform.Find ("name_daishi/Text").gameObject.GetComponent<Text> ().text = mFriendSearchData [i].nickname;

				mesData = mFriendSearchData [i].chara1.cname;
				if (mFriendSearchData[i].chara2 != null) {
					mesData = mesData + mesRet + mFriendSearchData[i].chara2.cname;
				}
				prefabObjSearch [i].transform.Find ("name_daishi/Text (2)").gameObject.GetComponent<Text> ().text = mesData;

				StartCoroutine (cbSearchDataSet (i));
			}
			SearchNumber = mFriendSearchData.Count;
		}
	}
	private IEnumerator cbSearchDataSet (int num){
		cbSearch [num] = prefabCharaSearch [num].GetComponent<CharaBehaviour> ();
		yield return cbSearch [num].init (mFriendSearchData[num].chara1);
		cbSearch [num].gotoAndPlay (MotionLabel.IDLE);
	}

	private void SearchDataClr(){
		Vector3 _Pos = new Vector3 (0.0f, 0.0f, 0.0f);

		for (int i = 0; i < SearchNumber; i++) {
			Destroy (prefabObjSearch [i]);
		}
		SearchNumber = 0;
		EventSearch.transform.Find ("mask/Panel").transform.localPosition = _Pos;
	}



	private readonly string MsgDataTable_1 = "の\n「";
	private readonly string MsgDataTable_2 = "」と「";
	private readonly string MsgDataTable_3 = "」と\nともだちに なりますか？";
	private readonly string MsgDataTable_4 = "」に\nれんらくしました";
	private readonly string MsgDataTable_5 = "」と\nおわかれしますか？";
	private readonly string MsgDataTable_6 = "」と\nともだちに なりますか？";
	private readonly string MsgDataTable_7 = "」と\nともだちに なりません";
		
	private int kakuninYesNoMode;


	private int ReqUserNumber;
	// 友達申請ボタンが押された時
	private void BtnSearchReq(int num){
		ReqUserNumber = num;

		string _mes;

		EventKakunin.SetActive (true);
		EventKakunin.transform.Find ("Text_Arial").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Text (1)").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_red_hai").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_blue_iie").gameObject.SetActive (true);
		kakuninYesNoMode = 0;

		EventKakunin.transform.Find ("Text_Arial").gameObject.GetComponent<Text> ().text = mFriendSearchData [num].nickname;
		if (mFriendSearchData [num].chara2 != null) {
			_mes = MsgDataTable_1;
			_mes = _mes + mFriendSearchData [num].chara1.cname;
			_mes = _mes + MsgDataTable_2;
			_mes = _mes + mFriendSearchData [num].chara2.cname;
			_mes = _mes + MsgDataTable_3;
		} else {
			_mes = MsgDataTable_1;
			_mes = _mes + mFriendSearchData [num].chara1.cname;
			_mes = _mes + MsgDataTable_3;
		}
		EventKakunin.transform.Find ("Text (1)").gameObject.GetComponent<Text> ().text = _mes;

	}
	// 申請結果
	private void mApplyFriend(bool success,object data)
	{
		Debug.LogFormat("Friends mApplyFriend:{0},{1}",success,data);

		if (success) {
			string _mes;
			// 成功
			EventResult.SetActive(true);
			EventResult.transform.Find ("1_shinsei shimashita").gameObject.SetActive (true);
			EventResult.transform.Find ("Text_Arial").gameObject.SetActive (true);
			EventResult.transform.Find ("Text 1").gameObject.SetActive (true);

			EventResult.transform.Find ("Text_Arial").gameObject.GetComponent<Text> ().text = mFriendSearchData [ReqUserNumber].nickname;
			if (mFriendSearchData [ReqUserNumber].chara2 != null) {
				_mes = MsgDataTable_1;
				_mes = _mes + mFriendSearchData [ReqUserNumber].chara1.cname;
				_mes = _mes + MsgDataTable_2;
				_mes = _mes + mFriendSearchData [ReqUserNumber].chara2.cname;
				_mes = _mes + MsgDataTable_4;
			} else {
				_mes = MsgDataTable_1;
				_mes = _mes + mFriendSearchData [ReqUserNumber].chara1.cname;
				_mes = _mes + MsgDataTable_4;
			}
			EventResult.transform.Find ("Text 1").gameObject.GetComponent<Text> ().text = _mes;
		} else {
			// 失敗
			int retFlag = (int)data;

			switch (retFlag) {
			case	1:
				{
					Debug.Log ("自分の枠が満杯");
					break;
				}
			case	2:
				{
					Debug.Log ("相手の枠が満杯");
					break;
				}
			case	3:
				{
					Debug.Log ("既に申請済みの相手");
					break;
				}
			}
		}
	}


	private void mDeleteFriend(bool success,object data)
	{
		Debug.LogFormat ("Friends mDeleteFriend:{0},{1}", success, data);

		if (success) {
			// 成功
			for (int i = 0; i < mFriendData.appfriends.Count; i++) {
				Destroy (prefabObjApp [i]);
			}
			for (int i = 0; i < mFriendData.toyfriends.Count; i++) {
				Destroy (prefabObjMIX2 [i]);
			}
			for (int i = 0; i < mFriendData.applys.Count; i++) {
				Destroy (prefabObjRenraku [i]);
			}

			// 友達手帳のデータを登録する
			mFriendData = (FriendData)data;
			NoteDataSet ();
		}
	}

	private void mReplyFriend(bool success,object data)
	{
		Debug.LogFormat ("Friends mReplyFriend:{0},{1}", success, data);

		if (success) {
			// 成功
			for (int i = 0; i < mFriendData.appfriends.Count; i++) {
				Destroy (prefabObjApp [i]);
			}
			for (int i = 0; i < mFriendData.toyfriends.Count; i++) {
				Destroy (prefabObjMIX2 [i]);
			}
			for (int i = 0; i < mFriendData.applys.Count; i++) {
				Destroy (prefabObjRenraku [i]);
			}

			// 友達手帳のデータを登録する
			mFriendData = (FriendData)data;
			NoteDataSet ();
		} else {
			// 失敗
			int retFlag = (int)data;

			switch (retFlag) {
			case	1:
				{
					Debug.Log ("自分の枠が満杯");
					break;
				}
			case	2:
				{
					Debug.Log ("相手の枠が満杯");
					break;
				}
			case	3:
				{
					Debug.Log ("既に申請済みの相手");
					break;
				}
			}
		}
	}
}
