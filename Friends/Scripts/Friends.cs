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

	private object[]		mparam;
	private User muser1;//自分
	private FriendData		mFriendData;
	private List<User> 		mFriendSearchData;



	private const int APPFRIENDS_MAX = 10;												// アプリの友達の最大数
	private const int TOYFRIENDS_MAX = 20;												// 玩具の友達の最大数
	private const int RENRAKU_MAX = 50;													// 友達申請の最大数
	private const int SEARCH_MAX = 50;													// 友達検索結果の最大数



	private GameObject[]	prefabObjApp = new GameObject[APPFRIENDS_MAX];				// アプリの友達
	private GameObject[]	prefabObjMIX2 = new GameObject[TOYFRIENDS_MAX];				// 玩具の友達
	private GameObject[]	prefabObjRenraku = new GameObject[RENRAKU_MAX];				// 友達申請（５０以上申請があったとしても５０件までしか表示しない）
	private GameObject[]	prefabObjSearch = new GameObject[SEARCH_MAX];				// 友達検索結果（５０以上検索結果があったとしても５０件までしか表示しない）

	private CharaBehaviour[] cbApp = new CharaBehaviour[APPFRIENDS_MAX];
	private CharaBehaviour[] cbMIX2 = new CharaBehaviour[TOYFRIENDS_MAX];
	private CharaBehaviour[] cbRenraku = new CharaBehaviour[RENRAKU_MAX];
	private CharaBehaviour[] cbSearch = new CharaBehaviour[SEARCH_MAX];



	private readonly string MsgDataTable_1 = "の\n「";
	private readonly string MsgDataTable_2 = "」と「";
	private readonly string MsgDataTable_3 = "」と\nともだちに なりますか？";
	private readonly string MsgDataTable_4 = "」に\nれんらくしました";
	private readonly string MsgDataTable_5 = "」と\nおわかれしますか？";
	private readonly string MsgDataTable_6 = "」と\nともだちに なりますか？";
	private readonly string MsgDataTable_7 = "」と\nともだちに なりません";
	private readonly string MsgDataTable_8 = "」の\nともだちが いっぱいで とうろくできません";





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

		// フレンド情報を取得
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
		muser1 = ManagerObject.instance.player;

		EventNoteApp.SetActive (true);
		EventNoteMIX2.SetActive (true);
		EventNoteRenraku.SetActive (true);
		EventSearch.SetActive (true);

		FriendSetActive (EventNoteApp, false);
		FriendSetActive (EventNoteMIX2, false);
		FriendSetActive (EventNoteRenraku, false);
		FriendSetActive (EventSearch, false);



		// パパママモードで友達検索を許可しているかどうか
		if (ManagerObject.instance.app.enabledViewSearchFriend) {
			// 友達検索OK
			EventMenu.transform.Find("Button_kensaku_gray").gameObject.SetActive(false);
		} else {
			// 友達検索NO
			EventMenu.transform.Find("Button_kensaku_gray").gameObject.SetActive(true);
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



		// アイテムパネルのスクロール監視
		FriendListScrollFlag = true;
		StartCoroutine ("FriendListScroll");



		yield return null;
	}

	void Destroy(){
		FriendListScrollFlag = false;
		Debug.Log ("Friends Destroy");
	}

	void OnDestroy(){
		FriendListScrollFlag = false;
		Debug.Log ("Friends OnDestroy");
	}

	// サーチ結果
	void mSearchFriend(bool success,object data)
	{
		Debug.LogFormat("Friends mSearchFriend:{0},{1}",success,data);

		if (success) {
			mFriendSearchData = (List<User>)data;
			if (mFriendSearchData.Count != 0) {
				// 検索結果があるのでデータをパネルにセット
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



	private bool FriendSearchBtnFlag;
	// 探すボタンの表示非表示を監視
	private IEnumerator FriendSearchBtn(){
		while (FriendSearchBtnFlag) {
			string _data = EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().text;
			bool _roFlag = EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().readOnly;
			bool _flag;

			if ((_data == "") || (_data.Length > 9) || _roFlag) {
				_flag = false;
			} else {
				if (_data.Length > 3) {
					_flag = true;
				} else {
					_flag = false;
				}
			}

			if (_flag) {
				BtnSearchSearch.gameObject.SetActive (true);
			} else {
				BtnSearchSearch.gameObject.SetActive (false);
			}

			yield return null;
		}
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
	// アイテムパネルのスクロール監視
	private IEnumerator FriendListScroll(){
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
							int _cnt = appfriendsCount;
							if (_cnt != 0) {
								_cnt--;
							}
							_NextStopPos = NextYPosTableApp [_cnt >> 1];
							break;
						}
					case	swipIdouPage.MIX2:
						{
							_NextPos = EventNoteMIX2.transform.Find("daishi/mask/Panel").transform.localPosition.y + 240.0f;
							int _cnt = toyfriendsCount;
							if(_cnt != 0){
								_cnt--;
							}
							_NextStopPos = NextYPosTableMIX2[_cnt >> 1];
							break;
						}
					case	swipIdouPage.Renraku:
						{
							_NextPos = EventNoteRenraku.transform.Find("daishi/mask/Panel").transform.localPosition.y + 240.0f;
							int _cnt = applysCount;
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
		ManagerObject.instance.sound.playSe (11);

		FriendNoteChange (friendTabTable.NoteApp);
	}
	// 友達検索へのボタン
	private void BtnMenuSearchClick(){
		ManagerObject.instance.sound.playSe (11);

		// 探すボタンの監視
		FriendSearchBtnFlag = true;
		StartCoroutine ("FriendSearchBtn");

		eventSearchInputFieldSet("みーつID を いれてね",false);
		FriendSetActive (EventSearch, true);

		// 検索結果を無しにする
		mFriendSearchData = null;

		SearchDataSet ();
		swipIdouFlag = swipIdouPage.Search;
		friendListIdouFlag = friendListTable.Null;
	}
	// 終了ボタン
	private void BtnMenuTojiruClick(){
		ManagerObject.instance.sound.playSe (17);

		ManagerObject.instance.view.change(SceneLabel.HOME);
	}

	// 友達手帳アプリの友達へのボタン（未使用）
	private void BtnNoteAppAppClick(){
	}
	// 友達手帳MIX2の友達へのボタン
	private void BtnNoteAppMIX2Click(){
		ManagerObject.instance.sound.playSe (11);

		FriendNoteChange (friendTabTable.NoteMIX2);
	}
	// 友達手帳の連絡帳へのボタン
	private void BtnNoteAppRenrakuClick(){
		ManagerObject.instance.sound.playSe (11);

		FriendNoteChange (friendTabTable.NoteRenraku);
	}
	// 初期選択画面へのボタン
	private void BtnNoteAppBackClick(){
		ManagerObject.instance.sound.playSe (17);

		FriendNoteChange (friendTabTable.NoteBack);
	}

	// 友達手帳アプリの友達へのボタン
	private void BtnNoteMIX2AppClick(){
		ManagerObject.instance.sound.playSe (11);

		FriendNoteChange (friendTabTable.NoteApp);
	}
	// 友達手帳MIX2の友達へのボタン（未使用）
	private void BtnNoteMIX2MIX2Click(){
	}
	// 友達手帳の連絡帳へのボタン
	private void BtnNoteMIX2RenrakuClick(){
		ManagerObject.instance.sound.playSe (11);

		FriendNoteChange (friendTabTable.NoteRenraku);
	}
	// 初期選択画面へのボタン
	private void BtnNoteMIX2BackClick(){
		ManagerObject.instance.sound.playSe (17);

		FriendNoteChange (friendTabTable.NoteBack);
	}

	// 友達手帳アプリの友達へのボタン
	private void BtnNoteRenrakuAppClick(){
		ManagerObject.instance.sound.playSe (11);

		FriendNoteChange (friendTabTable.NoteApp);
	}
	// 友達手帳MIX2の友達へのボタン
	private void BtnNoteRenrakuMIX2Click(){
		ManagerObject.instance.sound.playSe (11);

		FriendNoteChange (friendTabTable.NoteMIX2);
	}
	// 友達手帳の連絡帳へのボタン（未使用）
	private void BtnNoteRenrakuRenrakuClick(){
	}
	// 初期選択画面へのボタン
	private void BtnNoteRenrakuBackClick(){
		ManagerObject.instance.sound.playSe (17);

		FriendNoteChange (friendTabTable.NoteBack);
	}

	// はじめからボタン
	private void BtnSearchInitClick(){
		ManagerObject.instance.sound.playSe (11);

		eventSearchInputFieldSet("みーつID を いれてね",false);
	}
	private void eventSearchInputFieldSet(string _data,bool _flag){
		EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().characterLimit = 20;
		EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().contentType = InputField.ContentType.Standard;
		EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().text = _data;
		EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().characterLimit = 9;
		EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().contentType = InputField.ContentType.Alphanumeric;
		EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().readOnly = _flag;
		if (_flag) {
			EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<Text> ().color = new Color (128.0f / 255.0f, 128.0f / 255.0f, 128.0f / 255.0f, 1.0f);
		} else {
			EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<Text> ().color = new Color (50.0f / 255.0f, 50.0f / 255.0f, 50.0f / 255.0f, 1.0f);
		}
	}
	// 探すボタン
	private void BtnSearchSearchClick(){
		ManagerObject.instance.sound.playSe (11);

		string _data = EventSearch.transform.Find ("Image/Text").gameObject.GetComponent<InputField> ().text;

		if ((_data == "") || (_data.Length > 9)) {
			// 未入力の時などにエラー表示（探すボタンが４文字以上入力しないと表示ないから必要ないかも）
			EventKakunin.SetActive (true);
			EventKakunin.transform.Find ("Button_blue_tojiru").gameObject.SetActive (true);
			EventKakunin.transform.Find ("Text (2)").gameObject.SetActive (true);
		} else {
			// フレンドのID検索
			GameCall call = new GameCall (CallLabel.SEARCH_FRIEND, _data);
			call.AddListener (mSearchFriend);
			ManagerObject.instance.connect.send (call);

			eventSearchInputFieldSet (_data,true);
		}
	}
	// 初期選択画面へのボタン
	private void BtnSearchBackClick(){
		ManagerObject.instance.sound.playSe (17);

		FriendSetActive (EventSearch, false);
		swipIdouFlag = swipIdouPage.Null;
		friendListIdouFlag = friendListTable.Null;

		FriendSearchBtnFlag = false;
	}

	// もどるボタン
	private void BtnResultBackClick(){
		ManagerObject.instance.sound.playSe (17);

		// 結果画面で表示される可能性のあるパーツを全て消す。
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
		ManagerObject.instance.sound.playSe (17);

		KakuninModeOff ();
		mFriendSearchData = null;
		SearchDataSet ();
	}
	// はいボタン
	private void BtnKakuninYesClick(){
		ManagerObject.instance.sound.playSe (13);

		KakuninModeOff ();

		switch (YesNoModeFlag) {
		case	YesNoModeTable.APPLY_FRIEND:
			{	// フレンド申請
//				int _id = int.Parse (prefabObjSearch [ReqUserNumber].transform.Find ("IDbase/ID").gameObject.GetComponent<Text> ().text);
				GameCall call = new GameCall (CallLabel.APPLY_FRIEND, mFriendSearchData [ReqUserNumber].code);
				call.AddListener (mApplyFriend);
				ManagerObject.instance.connect.send (call);
				break;
			}
		case	YesNoModeTable.DELETE_FRIEND:
			{	// フレンドを削除する
				int _id = mFriendData.appfriends[SakujoNumber].id;

				GameCall call = new GameCall (CallLabel.DELETE_FRIEND, _id);
				call.AddListener (mDeleteFriend);
				ManagerObject.instance.connect.send (call);
				break;
			}
		case	YesNoModeTable.REPLY_FRIEND_YES:
		case	YesNoModeTable.REPLY_FRIEND_NO:
			{	// フレンド申請に対する返答
				int _id = mFriendData.applys [FriendYNNumber].user.id;
				bool _flag;
				if (YesNoModeFlag == YesNoModeTable.REPLY_FRIEND_YES) {
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
		ManagerObject.instance.sound.playSe (14);

		KakuninModeOff ();
	}



	// 確認画面で表示される可能性のあるパーツを全て消す
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

	// シーンパーツの表示のON/OFFの代わりにY座標移動で対応
	private void FriendSetActive(GameObject _obj,bool _flag){
		Vector3 _pos = new Vector3 (0.0f, 0.5f, 0.0f);

		if (!_flag) {
			_pos.y = 5000.0f;
		}
		_obj.transform.localPosition = _pos;
	}



	private enum friendTabTable{
		NoteApp,
		NoteMIX2,
		NoteRenraku,
		NoteBack,
	};
	// 友達手帳のタブ変更
	private void FriendNoteChange(friendTabTable type){
		Vector3 _Pos = new Vector3(0.0f,0.0f,0.0f);

		switch (type) {
		case	friendTabTable.NoteApp:
			{	// アプリの友達が選択された時
				FriendSetActive (EventNoteApp, true);
				FriendSetActive (EventNoteMIX2, false);
				FriendSetActive (EventNoteRenraku, false);
				swipIdouFlag = swipIdouPage.App;
				break;
			}
		case	friendTabTable.NoteMIX2:
			{	// みーつの友達が選択された時
				FriendSetActive (EventNoteApp, false);
				FriendSetActive (EventNoteMIX2, true);
				FriendSetActive (EventNoteRenraku, false);
				swipIdouFlag = swipIdouPage.MIX2;
				break;
			}
		case	friendTabTable.NoteRenraku:
			{	// 連絡が選択された時
				FriendSetActive (EventNoteApp, false);
				FriendSetActive (EventNoteMIX2, false);
				FriendSetActive (EventNoteRenraku, true);
				swipIdouFlag = swipIdouPage.Renraku;
				break;
			}
		case	friendTabTable.NoteBack:
			{	// もどるが選択された時
				FriendSetActive (EventNoteApp, false);
				FriendSetActive (EventNoteMIX2, false);
				FriendSetActive (EventNoteRenraku, false);
				swipIdouFlag = swipIdouPage.Null;
				break;
			}
		}
		// スクロールパネルの表示位置をリセット
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

	private int appfriendsCount = 0;
	private int toyfriendsCount = 0;
	private int applysCount = 0;
	// 友達手帳のデータを登録する
	private void NoteDataSet(){
		Vector3 _Pos = new Vector3 (0.0f, 0.0f, 0.0f);
		string mesRet = "\n";
		string mesData;

		// スクロールパネルの表示位置をリセット
		EventNoteApp.transform.Find ("daishi/mask/Panel").transform.localPosition = _Pos;
		EventNoteMIX2.transform.Find ("daishi/mask/Panel").transform.localPosition = _Pos;
		EventNoteRenraku.transform.Find ("daishi/mask/Panel").transform.localPosition = _Pos;


		appfriendsCount = mFriendData.appfriends.Count;
		if (appfriendsCount > APPFRIENDS_MAX) {
			appfriendsCount = APPFRIENDS_MAX;
		}
		toyfriendsCount = mFriendData.toyfriends.Count;
		if (toyfriendsCount > TOYFRIENDS_MAX) {
			toyfriendsCount = TOYFRIENDS_MAX;
		}
		applysCount = mFriendData.applys.Count;
		if (applysCount > RENRAKU_MAX) {
			applysCount = RENRAKU_MAX;
		}

		// アプリのともだちの現状人数
		EventNoteApp.transform.Find ("daishi/tab1/05").gameObject.GetComponent<Image> ().sprite = ImgNumber [appfriendsCount];
		EventNoteMIX2.transform.Find ("daishi/tab1/05").gameObject.GetComponent<Image> ().sprite = ImgNumber [appfriendsCount];
		EventNoteRenraku.transform.Find ("daishi/tab1/05").gameObject.GetComponent<Image> ().sprite = ImgNumber [appfriendsCount];

		// 玩具のともだちの現状人数
		EventNoteApp.transform.Find ("daishi/tab2/05").gameObject.GetComponent<Image> ().sprite = ImgNumber [toyfriendsCount];
		EventNoteMIX2.transform.Find ("daishi/tab2/05").gameObject.GetComponent<Image> ().sprite = ImgNumber [toyfriendsCount];
		EventNoteRenraku.transform.Find ("daishi/tab2/05").gameObject.GetComponent<Image> ().sprite = ImgNumber [toyfriendsCount];

		// アプリのともだちパネルセット
		for (int i = 0; i < appfriendsCount; i++) {
			// プレハブを登録
			prefabObjApp [i] = (GameObject)Instantiate (PrefabApp);
			prefabObjApp [i].transform.SetParent (EventNoteApp.transform.Find ("daishi/mask/Panel").transform, false);
			prefabObjApp [i].name = "friendApp" + i.ToString ();

			int ii = i + 0;
			// 削除ボタンを有効化
			prefabObjApp [i].transform.Find("Button_sakujo").gameObject.GetComponent<Button> ().onClick.AddListener (() => BtnSakujoReq (ii));

			// プロフィール画面へのボタンを有効化
			prefabObjApp [i].gameObject.GetComponent<Button> ().onClick.AddListener (() => BtnUserEventNoteApp (ii));

			// 表示位置を登録
			_Pos.x = prefabObjAppXPosTable [i & 1];
			_Pos.y = 100.0f - (350.0f * (i >> 1));
			_Pos.z = 0.0f;
			prefabObjApp [i].transform.localPosition = _Pos;

			// ユーザー名を登録
			prefabObjApp [i].transform.Find ("name_daishi/Text").gameObject.GetComponent<Text> ().text = mFriendData.appfriends [i].nickname;
			// たまごっちの名前を登録
			mesData = mFriendData.appfriends [i].chara1.cname;
			if (mFriendData.appfriends [i].chara2 != null) {
				mesData = mesData + mesRet + mFriendData.appfriends [i].chara2.cname;
			}
			prefabObjApp [i].transform.Find ("name_daishi/Text (2)").gameObject.GetComponent<Text> ().text = mesData;

			// たまごっちキャラを登録
			StartCoroutine (cbAppDataSet (i));
		}
		// 玩具のともだちパネルセット
		for (int i = 0; i < toyfriendsCount; i++) {
			// プレハブを登録
			prefabObjMIX2 [i] = (GameObject)Instantiate (PrefabMIX2);
			prefabObjMIX2 [i].transform.SetParent (EventNoteMIX2.transform.Find ("daishi/mask/Panel").transform, false);
			prefabObjMIX2 [i].name = "friendMIX2" + i.ToString ();

			int ii = i + 0;
			// プロフィール画面へのボタンを有効化
			prefabObjMIX2 [i].GetComponent<Button> ().onClick.AddListener (() => BtnUserEventNoteMIX2 (ii));
	
			// 表示位置を登録
			_Pos.x = prefabObjMIX2XPosTable [i & 1];
			_Pos.y = 150.0f - (240.0f * (i >> 1));
			_Pos.z = 0.0f;
			prefabObjMIX2 [i].transform.localPosition = _Pos;

			// ユーザー名を登録
			prefabObjMIX2 [i].transform.Find ("name_daishi/Text").gameObject.GetComponent<Text> ().text = mFriendData.toyfriends [i].nickname;
			// たまごっちの名前を登録
			mesData = mFriendData.toyfriends [i].chara1.cname;
			if (mFriendData.toyfriends [i].chara2 != null) {
				mesData = mesData + mesRet + mFriendData.toyfriends [i].chara2.cname;
			}
			prefabObjMIX2 [i].transform.Find ("name_daishi/Text (2)").gameObject.GetComponent<Text> ().text = mesData;

			// たまごっちキャラを登録
			StartCoroutine (cbMIX2DataSet (i));
		}
		// 連絡のともだちパネルセット
		for (int i = 0; i < applysCount; i++) {
			// プレハブを登録
			prefabObjRenraku [i] = (GameObject)Instantiate (PrefabRenraku);
			prefabObjRenraku [i].transform.SetParent (EventNoteRenraku.transform.Find ("daishi/mask/Panel").transform, false);
			prefabObjRenraku [i].name = "friendRenraku" + i.ToString ();

			int ii = i + 0;
			// ともだちになるボタンを有効化
			prefabObjRenraku [i].transform.Find("Button_red").gameObject.GetComponent<Button> ().onClick.AddListener (() => BtnFriendOKReq (ii));
			// ともだちにならないボタンを有効化
			prefabObjRenraku [i].transform.Find ("Button_blue").gameObject.GetComponent<Button> ().onClick.AddListener (() => BtnFriendNOReq (ii));

			// プロフィール画面へのボタンを有効化
			prefabObjRenraku [i].GetComponent<Button> ().onClick.AddListener (() => BtnUserEventNoteRenraku (ii));

			// 表示位置を登録
			_Pos.x = 0.0f;
			_Pos.y = 150.0f - (240.0f * i);
			_Pos.z = 0.0f;
			prefabObjRenraku [i].transform.localPosition = _Pos;

			// ユーザー名を登録
			prefabObjRenraku [i].transform.Find ("name_daishi/Text").gameObject.GetComponent<Text> ().text = mFriendData.applys [i].user.nickname;
			// たまごっちの名前を登録
			mesData = mFriendData.applys [i].user.chara1.cname;
			if (mFriendData.applys [i].user.chara2 != null) {
				mesData = mesData + mesRet + mFriendData.applys [i].user.chara2.cname;
			}
			prefabObjRenraku [i].transform.Find ("name_daishi/Text (2)").gameObject.GetComponent<Text> ().text = mesData;

			// たまごっちキャラを登録
			StartCoroutine (cbRenrakuDataSet (i));
		}
	}
	private IEnumerator cbAppDataSet(int num){
		cbApp [num] = prefabObjApp [num].transform.Find("chara_waku/chara/CharaImg").gameObject.GetComponent<CharaBehaviour>();
		yield return cbApp[num].init (mFriendData.appfriends[num].chara1);
		cbApp [num].gotoAndPlay (MotionLabel.IDLE);
	}
	private IEnumerator cbMIX2DataSet(int num){
		cbMIX2 [num] = prefabObjMIX2 [num].transform.Find("chara_waku/chara/CharaImg").gameObject.GetComponent<CharaBehaviour>();
		yield return cbMIX2 [num].init (mFriendData.toyfriends [num].chara1);
		cbMIX2 [num].gotoAndPlay (MotionLabel.IDLE);
	}
	private IEnumerator cbRenrakuDataSet (int num){
		cbRenraku [num] = prefabObjRenraku [num].transform.Find("chara_waku/chara/CharaImg").gameObject.GetComponent<CharaBehaviour>();
		yield return cbRenraku [num].init (mFriendData.applys [num].user.chara1);
		cbRenraku [num].gotoAndPlay (MotionLabel.IDLE);
	}

	private int FriendYNNumber;
	// 連絡のともだちになるボタン
	private void BtnFriendOKReq(int num){
		FriendYNNumber = num;
		string _mes;

		ManagerObject.instance.sound.playSe (11);

		EventKakunin.SetActive (true);
		EventKakunin.transform.Find ("Text_Arial").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Text (1)").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_red_hai").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_blue_iie").gameObject.SetActive (true);
		YesNoModeFlag = YesNoModeTable.REPLY_FRIEND_YES;

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

		ManagerObject.instance.sound.playSe (11);

		EventKakunin.SetActive (true);
		EventKakunin.transform.Find ("Text_Arial").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Text (1)").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_red_hai").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_blue_iie").gameObject.SetActive (true);
		YesNoModeFlag = YesNoModeTable.REPLY_FRIEND_NO;

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

		ManagerObject.instance.sound.playSe (11);

		EventKakunin.SetActive (true);
		EventKakunin.transform.Find ("Text_Arial").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Text (1)").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_red_hai").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_blue_iie").gameObject.SetActive (true);
		YesNoModeFlag = YesNoModeTable.DELETE_FRIEND;

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
			int _SearchNumber = mFriendSearchData.Count;

			if (_SearchNumber > SEARCH_MAX) {
				_SearchNumber = SEARCH_MAX;
			}

			// ともだち検索パネルセット
			for (int i = 0; i < _SearchNumber; i++) {
				// プレハブを登録
				prefabObjSearch [i] = (GameObject)Instantiate (PrefabSearch);
				prefabObjSearch [i].transform.SetParent (EventSearch.transform.Find ("mask/Panel").transform, false);
				prefabObjSearch [i].name = "friendSearch" + i.ToString ();

				int ii = i + 0;
				// ともだちになりたいボタンを有効化
				prefabObjSearch [i].transform.Find("Button_red").gameObject.GetComponent<Button> ().onClick.AddListener (() => BtnSearchReq (ii));

				// プロフィール画面へのボタンを有効化
				prefabObjSearch [i].GetComponent<Button> ().onClick.AddListener (() => BtnUserEventSearch (ii));

				// 表示位置を登録
				_Pos.x = 0.0f;
				_Pos.y = 150.0f - (240.0f * i);
				_Pos.z = 0.0f;
				prefabObjSearch [i].transform.localPosition = _Pos;

				// みーつIDを登録
				prefabObjSearch [i].transform.Find ("IDbase/ID").gameObject.GetComponent<Text> ().text = mFriendSearchData [i].code;
				// ユーザー名を登録
				prefabObjSearch [i].transform.Find ("name_daishi/Text").gameObject.GetComponent<Text> ().text = mFriendSearchData [i].nickname;
				// たまごっちの名前を登録
				mesData = mFriendSearchData [i].chara1.cname;
				if (mFriendSearchData[i].chara2 != null) {
					mesData = mesData + mesRet + mFriendSearchData[i].chara2.cname;
				}
				prefabObjSearch [i].transform.Find ("name_daishi/Text (2)").gameObject.GetComponent<Text> ().text = mesData;

				// たまごっちキャラを登録
				StartCoroutine (cbSearchDataSet (i));
			}
			SearchNumber = _SearchNumber;

			EventSearch.transform.Find ("mask/Panel").transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
		}
	}
	private IEnumerator cbSearchDataSet (int num){
		cbSearch [num] = prefabObjSearch [num].transform.Find ("chara_waku/chara/CharaImg").gameObject.GetComponent<CharaBehaviour> ();
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



	private YesNoModeTable YesNoModeFlag = YesNoModeTable.NULL;
	private enum YesNoModeTable{
		NULL,
		APPLY_FRIEND,
		DELETE_FRIEND,
		REPLY_FRIEND_YES,
		REPLY_FRIEND_NO,
	};
	private int ReqUserNumber;
	// 友達申請ボタンが押された時
	private void BtnSearchReq(int num){
		ReqUserNumber = num;

		string _mes;

		ManagerObject.instance.sound.playSe (11);

		EventKakunin.SetActive (true);
		EventKakunin.transform.Find ("Text_Arial").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Text (1)").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_red_hai").gameObject.SetActive (true);
		EventKakunin.transform.Find ("Button_blue_iie").gameObject.SetActive (true);
		YesNoModeFlag = YesNoModeTable.APPLY_FRIEND;

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
		string _mes;

		if (success) {
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

			// 申請した相手をリストから削除する
			mFriendSearchData.Remove (mFriendSearchData [ReqUserNumber]);
			SearchDataSet ();
		} else {
			// 失敗
			int retFlag = (int)data;

			switch (retFlag) {
			case	1:
				{	// 自分の枠が満杯
					FriendReqErrorDisp(mFriendSearchData[ReqUserNumber],1);
					break;
				}
			case	2:
				{	// 相手の枠が満杯
					FriendReqErrorDisp(mFriendSearchData[ReqUserNumber],2);
					break;
				}
			case	3:
				{	// 既に申請済みの相手
					FriendReqErrorDisp(mFriendSearchData[ReqUserNumber],3);
					// 登録されていたのでリストから削除する
					mFriendSearchData.Remove (mFriendSearchData [ReqUserNumber]);
					SearchDataSet ();
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
			for (int i = 0; i < appfriendsCount; i++) {
				Destroy (prefabObjApp [i]);
			}
			for (int i = 0; i < toyfriendsCount; i++) {
				Destroy (prefabObjMIX2 [i]);
			}
			for (int i = 0; i < applysCount; i++) {
				Destroy (prefabObjRenraku [i]);
			}

			// 友達手帳のデータを登録する
			mFriendData = (FriendData)data;
			NoteDataSet ();
		}
	}

	// フレンド申請に対する返答
	private void mReplyFriend(bool success,object data)
	{
		Debug.LogFormat ("Friends mReplyFriend:{0},{1}", success, data);

		if (success) {
			// 成功
			for (int i = 0; i < appfriendsCount; i++) {
				Destroy (prefabObjApp [i]);
			}
			for (int i = 0; i < toyfriendsCount; i++) {
				Destroy (prefabObjMIX2 [i]);
			}
			for (int i = 0; i < applysCount; i++) {
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
				{	// 自分の枠が満杯
					FriendReqErrorDisp(mFriendData.applys[FriendYNNumber].user,1);
					break;
				}
			case	2:
				{	// 相手の枠が満杯
					FriendReqErrorDisp(mFriendData.applys[FriendYNNumber].user,2);
					break;
				}
			case	3:
				{	// 既に登録済みの相手
					FriendReqErrorDisp(mFriendData.applys[FriendYNNumber].user,3);

					for (int i = 0; i < appfriendsCount; i++) {
						Destroy (prefabObjApp [i]);
					}
					for (int i = 0; i < toyfriendsCount; i++) {
						Destroy (prefabObjMIX2 [i]);
					}
					for (int i = 0; i < applysCount; i++) {
						Destroy (prefabObjRenraku [i]);
					}

					mFriendData.applys.Remove (mFriendData.applys[FriendYNNumber]);
					NoteDataSet ();

					break;
				}
			}
		}
	}

	private void FriendReqErrorDisp(User _user,int _mode){
		string _mes;

		switch (_mode) {
		case	1:
			{	// 自分の枠が満杯
				EventResult.SetActive (true);
				EventResult.transform.Find ("3_touroku dekimasen").gameObject.SetActive (true);
				EventResult.transform.Find ("Text 3-1").gameObject.SetActive (true);
				break;
			}
		case	2:
			{	// 相手の枠が満杯
				EventResult.SetActive (true);
				EventResult.transform.Find ("3_touroku dekimasen").gameObject.SetActive (true);
				EventResult.transform.Find ("Text_Arial").gameObject.SetActive (true);
				EventResult.transform.Find ("Text 3-2").gameObject.SetActive (true);

				EventResult.transform.Find ("Text_Arial").gameObject.GetComponent<Text> ().text = _user.nickname;

				if (_user.chara2 != null) {
					_mes = MsgDataTable_1;
					_mes = _mes + _user.chara1.cname;
					_mes = _mes + MsgDataTable_2;
					_mes = _mes + _user.chara2.cname;
					_mes = _mes + MsgDataTable_8;
				} else {
					_mes = MsgDataTable_1;
					_mes = _mes + _user.chara1.cname;
					_mes = _mes + MsgDataTable_8;
				}
				EventResult.transform.Find ("Text 3-2").gameObject.GetComponent<Text> ().text = _mes;
				break;
			}
		case	3:
			{	// 既に申請済みの相手
				EventResult.SetActive (true);
				EventResult.transform.Find ("0_touroku zumi").gameObject.SetActive (true);
				EventResult.transform.Find ("Text 0").gameObject.SetActive (true);
				break;
			}
		}
	}

	// キャラとユーザー情報を押したらプロフィール画面へ
	private void BtnUserEventNoteApp(int num){
		EventProfileSend (mFriendData.appfriends [num]);
	}
	// キャラとユーザー情報を押したらプロフィール画面へ
	private void BtnUserEventNoteMIX2(int num){
		EventProfileSend (mFriendData.toyfriends [num]);
	}
	// キャラとユーザー情報を押したらプロフィール画面へ
	private void BtnUserEventNoteRenraku(int num){
		EventProfileSend (mFriendData.applys [num].user);
	}
	// キャラとユーザー情報を押したらプロフィール画面へ
	private void BtnUserEventSearch(int num){
		EventProfileSend (mFriendSearchData [num]);
	}


	private void EventProfileSend(User _user){
		ManagerObject.instance.sound.playSe (11);

		Debug.Log ("プロフィール画面へ・・・" + _user.nickname);
		ManagerObject.instance.view.add (SceneLabel.PROFILE_TOWN, _user);
//		ManagerObject.instance.view.change(SceneLabel.PROFILE,_user);
	}


}
