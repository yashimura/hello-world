using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

namespace Mix2App.Friends
{
    public class Friends : MonoBehaviour, IReceiver, IReadyable
    {
        [SerializeField] private GameObject AppFriendContainer = null;
        [SerializeField] private GameObject ToyFriendContainer = null;
        [SerializeField] private GameObject ApplyContainer = null;
        [SerializeField] private GameObject SearchContainer = null;
        [SerializeField] private GameObject EventMenu = null;          // 初期選択画面
        [SerializeField] private GameObject EventNoteApp = null;       // 友達手帳アプリの友達
        [SerializeField] private GameObject EventNoteMIX2 = null;      // 友達手帳MIX2の友達
        [SerializeField] private GameObject EventNoteRenraku = null;   // 友達手帳の連絡帳
        [SerializeField] private GameObject EventSearch = null;        // 友達検索画面
        [SerializeField] private GameObject EventResult = null;        // 結果画面
        [SerializeField] private GameObject EventKakunin = null;       // 確認画面

        [SerializeField] private Button BtnMenuTecho = null;           // 友達手帳へのボタン
        [SerializeField] private Button BtnMenuSearch = null;          // 友達検索へのボタン
        [SerializeField] private Button BtnMenuTojiru = null;          // 終了ボタン

        [SerializeField] private Button BtnNoteAppApp = null;          // 友達手帳アプリの友達へのボタン（未使用）
        [SerializeField] private Button BtnNoteAppMIX2 = null;         // 友達手帳MIX2の友達へのボタン
        [SerializeField] private Button BtnNoteAppRenraku = null;      // 友達手帳の連絡帳へのボタン
        [SerializeField] private Button BtnNoteAppBack = null;         // 初期選択画面へのボタン

        [SerializeField] private Button BtnNoteMIX2App = null;         // 友達手帳アプリの友達へのボタン
        [SerializeField] private Button BtnNoteMIX2MIX2 = null;        // 友達手帳MIX2の友達へのボタン（未使用）
        [SerializeField] private Button BtnNoteMIX2Renraku = null;     // 友達手帳の連絡帳へのボタン
        [SerializeField] private Button BtnNoteMIX2Back = null;        // 初期選択画面へのボタン

        [SerializeField] private Button BtnNoteRenrakuApp = null;      // 友達手帳アプリの友達へのボタン
        [SerializeField] private Button BtnNoteRenrakuMIX2 = null;     // 友達手帳MIX2の友達へのボタン
        [SerializeField] private Button BtnNoteRenrakuRenraku = null;  // 友達手帳の連絡帳へのボタン（未使用）
        [SerializeField] private Button BtnNoteRenrakuBack = null;     // 初期選択画面へのボタン

        [SerializeField] private Button BtnSearchInit = null;          // はじめからボタン
        [SerializeField] private Button BtnSearchSearch = null;        // 探すボタン
        [SerializeField] private Button BtnSearchBack = null;          // 初期選択画面へのボタン

        [SerializeField] private Button BtnResultBack = null;          // もどるボタン

        [SerializeField] private Button BtnKakuninBack = null;         // もどるボタン
        [SerializeField] private Button BtnKakuninTojiru = null;       // とじるボタン
        [SerializeField] private Button BtnKakuninYes = null;          // はいボタン
        [SerializeField] private Button BtnKakuninNo = null;           // いいえボタン

        [SerializeField] private Sprite[] ImgNumber = null;            // ００から２０まで
        [SerializeField] private Sprite[] ImgHeart = null;

        [SerializeField] private GameObject PrefabApp = null;
        [SerializeField] private GameObject PrefabMIX2 = null;
        [SerializeField] private GameObject PrefabRenraku = null;
        [SerializeField] private GameObject PrefabSearch = null;

        private FriendData mFriendData;
        private List<User> mFriendSearchData;



        private const int APPFRIENDS_MAX = 10;                                              // アプリの友達の最大数
        private const int TOYFRIENDS_MAX = 20;                                              // 玩具の友達の最大数
        private const int RENRAKU_MAX = 50;                                                 // 友達申請の最大数
        private const int SEARCH_MAX = 50;                                                  // 友達検索結果の最大数



        private GameObject[] prefabObjApp = new GameObject[APPFRIENDS_MAX];             // アプリの友達
        private GameObject[] prefabObjMIX2 = new GameObject[TOYFRIENDS_MAX];            // 玩具の友達
        private GameObject[] prefabObjRenraku = new GameObject[RENRAKU_MAX];            // 友達申請（５０以上申請があったとしても５０件までしか表示しない）
        private GameObject[] prefabObjSearch = new GameObject[SEARCH_MAX];              // 友達検索結果（５０以上検索結果があったとしても５０件までしか表示しない）

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

        private bool minited;
        private InputField msearchin;

        void Awake()
        {
            Debug.Log("Friends Awake");
            minited = false;
            // パパママモードで友達検索を許可しているかどうか
            BtnMenuSearch.interactable = ManagerObject.instance.app.enabledViewSearchFriend;

            msearchin = EventSearch.transform.Find("InputCode").gameObject.GetComponent<InputField>();

            appfriendsCount = 0;
            toyfriendsCount = 0;
            applysCount = 0;
        }

        void OnEnable()
        {
            msearchin.onValidateInput += ValidateInput;
        }

        void OnDisable()
        {
            msearchin.onValidateInput -= ValidateInput;
        }

        public char ValidateInput(string text, int charIndex, char addedChar)
        {
            if (char.IsSurrogate(addedChar))
            {
                // サロゲートペアの場合には削除
                addedChar = '\0';
            }
            return addedChar;
        }

        public void receive(params object[] parameter)
        {
            Debug.Log("Friends receive");
        }

        public bool ready()
        {
            return minited;
        }

        void Start()
        {
            Debug.Log("Friends start");
            EventMenu.SetActive(true);
            EventNoteApp.SetActive(true);
            EventNoteMIX2.SetActive(true);
            EventNoteRenraku.SetActive(true);
            EventSearch.SetActive(true);

            FriendSetActive(EventMenu, false);
            FriendSetActive(EventNoteApp, false);
            FriendSetActive(EventNoteMIX2, false);
            FriendSetActive(EventNoteRenraku, false);
            FriendSetActive(EventSearch, false);

            BtnMenuTecho.onClick.AddListener(BtnMenuTechoClick);                    // 友達手帳へのボタン
            BtnMenuSearch.onClick.AddListener(BtnMenuSearchClick);                  // 友達検索へのボタン
            BtnMenuTojiru.onClick.AddListener(BtnMenuTojiruClick);                  // 終了ボタン

            BtnNoteAppApp.onClick.AddListener(BtnNoteAppAppClick);                  // 友達手帳アプリの友達へのボタン（未使用）
            BtnNoteAppMIX2.onClick.AddListener(BtnNoteAppMIX2Click);                // 友達手帳MIX2の友達へのボタン
            BtnNoteAppRenraku.onClick.AddListener(BtnNoteAppRenrakuClick);          // 友達手帳の連絡帳へのボタン
            BtnNoteAppBack.onClick.AddListener(BtnNoteAppBackClick);                // 初期選択画面へのボタン

            BtnNoteMIX2App.onClick.AddListener(BtnNoteMIX2AppClick);                // 友達手帳アプリの友達へのボタン
            BtnNoteMIX2MIX2.onClick.AddListener(BtnNoteMIX2MIX2Click);              // 友達手帳MIX2の友達へのボタン（未使用）
            BtnNoteMIX2Renraku.onClick.AddListener(BtnNoteMIX2RenrakuClick);        // 友達手帳の連絡帳へのボタン
            BtnNoteMIX2Back.onClick.AddListener(BtnNoteMIX2BackClick);              // 初期選択画面へのボタン

            BtnNoteRenrakuApp.onClick.AddListener(BtnNoteRenrakuAppClick);          // 友達手帳アプリの友達へのボタン
            BtnNoteRenrakuMIX2.onClick.AddListener(BtnNoteRenrakuMIX2Click);        // 友達手帳MIX2の友達へのボタン
            BtnNoteRenrakuRenraku.onClick.AddListener(BtnNoteRenrakuRenrakuClick);  // 友達手帳の連絡帳へのボタン（未使用）
            BtnNoteRenrakuBack.onClick.AddListener(BtnNoteRenrakuBackClick);        // 初期選択画面へのボタン

            BtnSearchInit.onClick.AddListener(BtnSearchInitClick);                  // はじめからボタン
            BtnSearchSearch.onClick.AddListener(BtnSearchSearchClick);              // 探すボタン
            BtnSearchBack.onClick.AddListener(BtnSearchBackClick);                  // 初期選択画面へのボタン

            BtnResultBack.onClick.AddListener(BtnResultBackClick);                  // もどるボタン

            BtnKakuninBack.onClick.AddListener(BtnKakuninBackClick);                // もどるボタン
            BtnKakuninTojiru.onClick.AddListener(BtnKakuninTojiruClick);            // とじるボタン
            BtnKakuninYes.onClick.AddListener(BtnKakuninYesClick);                  // はいボタン
            BtnKakuninNo.onClick.AddListener(BtnKakuninNoClick);                    // いいえボタン


            EventMenuOpen();


            minited = true;
        }

        // サーチ結果
        void mSearchFriend(bool success, object data)
        {
            Debug.LogFormat("Friends mSearchFriend:{0},{1}", success, data);

            if (success)
            {
                mFriendSearchData = (List<User>)data;
                if (mFriendSearchData.Count > 0)
                {
                    // 検索結果があるのでデータをパネルにセット
                    SearchDataSet();
                }
                else
                {
                    // サーチは成功したが０件なので確認画面を表示
                    EventKakunin.SetActive(true);
                    EventKakunin.transform.Find("Button_blue_tojiru").gameObject.SetActive(true);
                    EventKakunin.transform.Find("Text (2)").gameObject.SetActive(true);
                }
            }
            else
            {
                // サーチに失敗したので確認画面を表示
                EventKakunin.SetActive(true);
                EventKakunin.transform.Find("Button_blue_tojiru").gameObject.SetActive(true);
                EventKakunin.transform.Find("Text (2)").gameObject.SetActive(true);
            }

        }

        void Update()
        {
            if (minited)
            {
                if (applysCount != 0)
                {
                    EventMenu.transform.Find("renraku_ari").gameObject.SetActive(true);
                    EventNoteApp.transform.Find("daishi/tab3/heart").gameObject.GetComponent<Image>().sprite = ImgHeart[0];
                    EventNoteMIX2.transform.Find("daishi/tab3/heart").gameObject.GetComponent<Image>().sprite = ImgHeart[0];
                    EventNoteRenraku.transform.Find("daishi/tab3/heart").gameObject.GetComponent<Image>().sprite = ImgHeart[0];
                }
                else
                {
                    EventMenu.transform.Find("renraku_ari").gameObject.SetActive(false);
                    EventNoteApp.transform.Find("daishi/tab3/heart").gameObject.GetComponent<Image>().sprite = ImgHeart[1];
                    EventNoteMIX2.transform.Find("daishi/tab3/heart").gameObject.GetComponent<Image>().sprite = ImgHeart[1];
                    EventNoteRenraku.transform.Find("daishi/tab3/heart").gameObject.GetComponent<Image>().sprite = ImgHeart[1];
                }
            }
        }

        private bool FriendSearchBtnFlag;
        // 探すボタンの表示非表示を監視
        private IEnumerator FriendSearchBtn()
        {
            while (FriendSearchBtnFlag)
            {

                string _data = msearchin.text;
                bool _roFlag = msearchin.readOnly;
                bool _flag;

                if ((_data == "") || (_data.Length > 9) || _roFlag)
                {
                    _flag = false;
                }
                else
                {
                    if (_data.Length > 3)
                    {
                        _flag = true;
                    }
                    else
                    {
                        _flag = false;
                    }
                }

                if (_flag)
                {
                    BtnSearchSearch.interactable = true;
                }
                else
                {
                    BtnSearchSearch.interactable = false;
                }

                yield return null;
            }
        }

        // 初期選択画面を開く時にデータをロードするようにした
        private void EventMenuOpen()
        {
            // フレンド情報を取得
            GameCall call = new GameCall(CallLabel.GET_FRIEND_INFO);
            call.AddListener(mGetFriendInfo);
            ManagerObject.instance.connect.send(call);
        }

        void mGetFriendInfo(bool success, object data)
        {
            mFriendData = (FriendData)data;

            // 友達手帳のデータを登録する
            NoteDataSet();
            FriendSetActive(EventMenu, true);
        }



        // 友達手帳へのボタン
        private void BtnMenuTechoClick()
        {
            ManagerObject.instance.sound.playSe(11);
            FriendNoteChange(friendTabTable.NoteApp);
        }

        // 友達検索へのボタン
        private void BtnMenuSearchClick()
        {
            ManagerObject.instance.sound.playSe(11);

            // 探すボタンの監視
            FriendSearchBtnFlag = true;
            StartCoroutine("FriendSearchBtn");

            eventSearchInputFieldSet("");
            FriendSetActive(EventSearch, true);

            // 検索結果を無しにする
            mFriendSearchData = null;

            SearchDataSet();
        }
        // 終了ボタン
        private void BtnMenuTojiruClick()
        {
            ManagerObject.instance.sound.playSe(17);

            ManagerObject.instance.view.change(SceneLabel.HOME);
        }

        // 友達手帳アプリの友達へのボタン（未使用）
        private void BtnNoteAppAppClick()
        {
        }
        // 友達手帳MIX2の友達へのボタン
        private void BtnNoteAppMIX2Click()
        {
            ManagerObject.instance.sound.playSe(11);

            FriendNoteChange(friendTabTable.NoteMIX2);
        }
        // 友達手帳の連絡帳へのボタン
        private void BtnNoteAppRenrakuClick()
        {
            ManagerObject.instance.sound.playSe(11);

            FriendNoteChange(friendTabTable.NoteRenraku);
        }
        // 初期選択画面へのボタン
        private void BtnNoteAppBackClick()
        {
            ManagerObject.instance.sound.playSe(17);

            FriendNoteChange(friendTabTable.NoteBack);
        }

        // 友達手帳アプリの友達へのボタン
        private void BtnNoteMIX2AppClick()
        {
            ManagerObject.instance.sound.playSe(11);

            FriendNoteChange(friendTabTable.NoteApp);
        }
        // 友達手帳MIX2の友達へのボタン（未使用）
        private void BtnNoteMIX2MIX2Click()
        {
        }
        // 友達手帳の連絡帳へのボタン
        private void BtnNoteMIX2RenrakuClick()
        {
            ManagerObject.instance.sound.playSe(11);

            FriendNoteChange(friendTabTable.NoteRenraku);
        }
        // 初期選択画面へのボタン
        private void BtnNoteMIX2BackClick()
        {
            ManagerObject.instance.sound.playSe(17);

            FriendNoteChange(friendTabTable.NoteBack);
        }

        // 友達手帳アプリの友達へのボタン
        private void BtnNoteRenrakuAppClick()
        {
            ManagerObject.instance.sound.playSe(11);

            FriendNoteChange(friendTabTable.NoteApp);
        }
        // 友達手帳MIX2の友達へのボタン
        private void BtnNoteRenrakuMIX2Click()
        {
            ManagerObject.instance.sound.playSe(11);

            FriendNoteChange(friendTabTable.NoteMIX2);
        }
        // 友達手帳の連絡帳へのボタン（未使用）
        private void BtnNoteRenrakuRenrakuClick()
        {
        }
        // 初期選択画面へのボタン
        private void BtnNoteRenrakuBackClick()
        {
            ManagerObject.instance.sound.playSe(17);

            FriendNoteChange(friendTabTable.NoteBack);
        }

        // はじめからボタン
        private void BtnSearchInitClick()
        {
            ManagerObject.instance.sound.playSe(11);

            eventSearchInputFieldSet("");
        }
        private void eventSearchInputFieldSet(string _data)
        {
            if (_data != null)
                msearchin.text = _data;
        }
        // 探すボタン
        private void BtnSearchSearchClick()
        {
            ManagerObject.instance.sound.playSe(11);

            string _data = msearchin.text;

            if ((_data == "") || (_data.Length > 9))
            {
                // 未入力の時などにエラー表示（探すボタンが４文字以上入力しないと表示ないから必要ないかも）
                EventKakunin.SetActive(true);
                EventKakunin.transform.Find("Button_blue_tojiru").gameObject.SetActive(true);
                EventKakunin.transform.Find("Text (2)").gameObject.SetActive(true);
            }
            else
            {
                // フレンドのID検索
                GameCall call = new GameCall(CallLabel.SEARCH_FRIEND, _data);
                call.AddListener(mSearchFriend);
                ManagerObject.instance.connect.send(call);
            }
        }
        // 初期選択画面へのボタン
        private void BtnSearchBackClick()
        {
            ManagerObject.instance.sound.playSe(17);

            FriendSetActive(EventSearch, false);
            EventMenuOpen();

            FriendSearchBtnFlag = false;
        }

        // もどるボタン
        private void BtnResultBackClick()
        {
            ManagerObject.instance.sound.playSe(17);

            // 結果画面で表示される可能性のあるパーツを全て消す。
            EventResult.transform.Find("0_touroku zumi").gameObject.SetActive(false);
            EventResult.transform.Find("1_shinsei shimashita").gameObject.SetActive(false);
            EventResult.transform.Find("2_tomodachini narimashita").gameObject.SetActive(false);
            EventResult.transform.Find("3_touroku dekimasen").gameObject.SetActive(false);
            EventResult.transform.Find("Text_Arial").gameObject.SetActive(false);
            EventResult.transform.Find("Text 0").gameObject.SetActive(false);
            EventResult.transform.Find("Text 1").gameObject.SetActive(false);
            EventResult.transform.Find("Text 2").gameObject.SetActive(false);
            EventResult.transform.Find("Text 3-1").gameObject.SetActive(false);
            EventResult.transform.Find("Text 3-2").gameObject.SetActive(false);
            EventResult.transform.Find("Text 4").gameObject.SetActive(false);

            EventResult.SetActive(false);
        }

        // もどるボタン
        private void BtnKakuninBackClick()
        {
        }
        // とじるボタン
        private void BtnKakuninTojiruClick()
        {
            ManagerObject.instance.sound.playSe(17);

            KakuninModeOff();
            mFriendSearchData = null;
            SearchDataSet();
        }
        // はいボタン
        private void BtnKakuninYesClick()
        {
            ManagerObject.instance.sound.playSe(13);

            KakuninModeOff();

            switch (YesNoModeFlag)
            {
                case YesNoModeTable.APPLY_FRIEND:
                    {   // フレンド申請
                        //				int _id = int.Parse (prefabObjSearch [ReqUserNumber].transform.Find ("IDbase/ID").gameObject.GetComponent<Text> ().text);
                        GameCall call = new GameCall(CallLabel.APPLY_FRIEND, mFriendSearchData[ReqUserNumber]);
                        call.AddListener(mApplyFriend);
                        ManagerObject.instance.connect.send(call);
                        break;
                    }
                case YesNoModeTable.DELETE_FRIEND:
                    {   // フレンドを削除する
                        GameCall call = new GameCall(CallLabel.DELETE_FRIEND, mFriendData.appfriends[SakujoNumber]);
                        call.AddListener(mDeleteFriend);
                        ManagerObject.instance.connect.send(call);
                        break;
                    }
                case YesNoModeTable.REPLY_FRIEND_YES:
                case YesNoModeTable.REPLY_FRIEND_NO:
                    {   // フレンド申請に対する返答
                        bool _flag;
                        if (YesNoModeFlag == YesNoModeTable.REPLY_FRIEND_YES)
                        {
                            _flag = true;               // 承認
                        }
                        else
                        {
                            _flag = false;              // 却下
                        }
                        GameCall call = new GameCall(CallLabel.REPLY_FRIEND, mFriendData.applys[FriendYNNumber], _flag);
                        call.AddListener(mReplyFriend);
                        ManagerObject.instance.connect.send(call);
                        break;
                    }
            }
        }
        // いいえボタン
        private void BtnKakuninNoClick()
        {
            ManagerObject.instance.sound.playSe(14);

            KakuninModeOff();
        }



        // 確認画面で表示される可能性のあるパーツを全て消す
        private void KakuninModeOff()
        {
            EventKakunin.transform.Find("Button_blue_modoru").gameObject.SetActive(false);
            EventKakunin.transform.Find("Button_blue_tojiru").gameObject.SetActive(false);
            EventKakunin.transform.Find("Text_Arial").gameObject.SetActive(false);
            EventKakunin.transform.Find("Text (1)").gameObject.SetActive(false);
            EventKakunin.transform.Find("Text (2)").gameObject.SetActive(false);
            EventKakunin.transform.Find("Text (3)").gameObject.SetActive(false);
            EventKakunin.transform.Find("Button_red_hai").gameObject.SetActive(false);
            EventKakunin.transform.Find("Button_blue_iie").gameObject.SetActive(false);

            EventKakunin.SetActive(false);
        }

        // シーンパーツの表示のON/OFFの代わりにY座標移動で対応
        private void FriendSetActive(GameObject _obj, bool _flag)
        {
            Vector3 _pos = new Vector3(0.0f, 0.5f, 0.0f);

            if (!_flag)
            {
                _pos.y = 5000.0f;
            }
            _obj.transform.localPosition = _pos;
        }



        private enum friendTabTable
        {
            NoteApp,
            NoteMIX2,
            NoteRenraku,
            NoteBack,
        };
        // 友達手帳のタブ変更
        private void FriendNoteChange(friendTabTable type)
        {
            switch (type)
            {
                case friendTabTable.NoteApp:
                    {   // アプリの友達が選択された時
                        FriendSetActive(EventNoteApp, true);
                        FriendSetActive(EventNoteMIX2, false);
                        FriendSetActive(EventNoteRenraku, false);
                        break;
                    }
                case friendTabTable.NoteMIX2:
                    {   // みーつの友達が選択された時
                        FriendSetActive(EventNoteApp, false);
                        FriendSetActive(EventNoteMIX2, true);
                        FriendSetActive(EventNoteRenraku, false);
                        break;
                    }
                case friendTabTable.NoteRenraku:
                    {   // 連絡が選択された時
                        FriendSetActive(EventNoteApp, false);
                        FriendSetActive(EventNoteMIX2, false);
                        FriendSetActive(EventNoteRenraku, true);
                        break;
                    }
                case friendTabTable.NoteBack:
                    {   // もどるが選択された時
                        FriendSetActive(EventNoteApp, false);
                        FriendSetActive(EventNoteMIX2, false);
                        FriendSetActive(EventNoteRenraku, false);
                        EventMenuOpen();
                        break;
                    }
            }

        }

        private void SetupWdth(GameObject container, float GridRatio)
        {
            GridLayoutGroup gr = container.GetComponent<GridLayoutGroup>();
            if (gr != null)
            {
                if (gr.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
                {
                    float width = container.GetComponent<RectTransform>().rect.size.x;
                    width = (width - gr.spacing.x * (gr.constraintCount + 1)) / (float)(gr.constraintCount);
                    gr.cellSize = new Vector2(width, width * GridRatio);
                }
            }
        }

        private int appfriendsCount = 0;
        private int toyfriendsCount = 0;
        private int applysCount = 0;
        // 友達手帳のデータを登録する
        private void NoteDataSet()
        {
            string mesRet = "\n";
            string mesData;

            //containerのwidthからセルサイズを決定する
            SetupWdth(AppFriendContainer, 335f / 590f);
            SetupWdth(ToyFriendContainer, 204f / 540f);
            //SetupWdth(ApplyContainer,1f);

            foreach ( Transform n in AppFriendContainer.transform )
            {
                GameObject.Destroy(n.gameObject);
            }

            foreach ( Transform n in ToyFriendContainer.transform )
            {
                GameObject.Destroy(n.gameObject);
            }

            foreach ( Transform n in ApplyContainer.transform )
            {
                GameObject.Destroy(n.gameObject);
            }

            appfriendsCount = mFriendData.appfriends.Count;
            if (appfriendsCount > APPFRIENDS_MAX)
            {
                appfriendsCount = APPFRIENDS_MAX;
            }
            toyfriendsCount = mFriendData.toyfriends.Count;
            if (toyfriendsCount > TOYFRIENDS_MAX)
            {
                toyfriendsCount = TOYFRIENDS_MAX;
            }
            applysCount = mFriendData.applys.Count;
            if (applysCount > RENRAKU_MAX)
            {
                applysCount = RENRAKU_MAX;
            }

            // アプリのともだちの現状人数
            EventNoteApp.transform.Find("daishi/tab1/05").gameObject.GetComponent<Image>().sprite = ImgNumber[appfriendsCount];
            EventNoteMIX2.transform.Find("daishi/tab1/05").gameObject.GetComponent<Image>().sprite = ImgNumber[appfriendsCount];
            EventNoteRenraku.transform.Find("daishi/tab1/05").gameObject.GetComponent<Image>().sprite = ImgNumber[appfriendsCount];

            // 玩具のともだちの現状人数
            EventNoteApp.transform.Find("daishi/tab2/05").gameObject.GetComponent<Image>().sprite = ImgNumber[toyfriendsCount];
            EventNoteMIX2.transform.Find("daishi/tab2/05").gameObject.GetComponent<Image>().sprite = ImgNumber[toyfriendsCount];
            EventNoteRenraku.transform.Find("daishi/tab2/05").gameObject.GetComponent<Image>().sprite = ImgNumber[toyfriendsCount];

            // アプリのともだちパネルセット
            for (int i = 0; i < appfriendsCount; i++)
            {
                // プレハブを登録
                prefabObjApp[i] = (GameObject)Instantiate(PrefabApp);
                prefabObjApp[i].transform.SetParent(AppFriendContainer.transform, false);
                prefabObjApp[i].name = "friendApp" + i.ToString();

                int ii = i + 0;
                // 削除ボタンを有効化
                prefabObjApp[i].transform.Find("Button_sakujo").gameObject.GetComponent<Button>().onClick.AddListener(() => BtnSakujoReq(ii));

                // プロフィール画面へのボタンを有効化
                //prefabObjApp [i].gameObject.GetComponent<Button> ().onClick.AddListener (() => BtnUserEventNoteApp (ii));

                // ユーザー名を登録
                prefabObjApp[i].transform.Find("name_daishi/Text").gameObject.GetComponent<Text>().text = userNicknameRetInsert(userNicknameChange(mFriendData.appfriends[i].nickname));
                // たまごっちの名前を登録
                mesData = mFriendData.appfriends[i].chara1.cname;
                if (mFriendData.appfriends[i].chara2 != null)
                {
                    mesData = mesData + mesRet + mFriendData.appfriends[i].chara2.cname;
                }
                prefabObjApp[i].transform.Find("name_daishi/Text (2)").gameObject.GetComponent<Text>().text = mesData;

                // たまごっちキャラを登録
                StartCoroutine(cbAppDataSet(i));
            }
            // 玩具のともだちパネルセット
            for (int i = 0; i < toyfriendsCount; i++)
            {
                // プレハブを登録
                prefabObjMIX2[i] = (GameObject)Instantiate(PrefabMIX2);
                prefabObjMIX2[i].transform.SetParent(ToyFriendContainer.transform, false);
                prefabObjMIX2[i].name = "friendMIX2" + i.ToString();

                // プロフィール画面へのボタンを有効化
                //int ii = i + 0;
                //prefabObjMIX2 [i].GetComponent<Button> ().onClick.AddListener (() => BtnUserEventNoteMIX2 (ii));

                // ユーザー名を登録
                prefabObjMIX2[i].transform.Find("name_daishi/Text").gameObject.GetComponent<Text>().text = userNicknameRetInsert(userNicknameChange(mFriendData.toyfriends[i].nickname));
                // たまごっちの名前を登録
                mesData = mFriendData.toyfriends[i].chara1.cname;
                if (mFriendData.toyfriends[i].chara2 != null)
                {
                    mesData = mesData + mesRet + mFriendData.toyfriends[i].chara2.cname;
                }
                prefabObjMIX2[i].transform.Find("name_daishi/Text (2)").gameObject.GetComponent<Text>().text = mesData;

                // たまごっちキャラを登録
                StartCoroutine(cbMIX2DataSet(i));
            }
            // 連絡のともだちパネルセット
            for (int i = 0; i < applysCount; i++)
            {
                // プレハブを登録
                prefabObjRenraku[i] = (GameObject)Instantiate(PrefabRenraku);
                prefabObjRenraku[i].transform.SetParent(ApplyContainer.transform, false);
                prefabObjRenraku[i].name = "friendRenraku" + i.ToString();

                int ii = i + 0;
                // ともだちになるボタンを有効化
                prefabObjRenraku[i].transform.Find("Button_red").gameObject.GetComponent<Button>().onClick.AddListener(() => BtnFriendOKReq(ii));
                // ともだちにならないボタンを有効化
                prefabObjRenraku[i].transform.Find("Button_blue").gameObject.GetComponent<Button>().onClick.AddListener(() => BtnFriendNOReq(ii));

                // プロフィール画面へのボタンを有効化
                //prefabObjRenraku [i].GetComponent<Button> ().onClick.AddListener (() => BtnUserEventNoteRenraku (ii));

                // ユーザー名を登録
                prefabObjRenraku[i].transform.Find("name_daishi/Text").gameObject.GetComponent<Text>().text = userNicknameRetInsert(userNicknameChange(mFriendData.applys[i].user.nickname));
                // たまごっちの名前を登録
                mesData = mFriendData.applys[i].user.chara1.cname;
                if (mFriendData.applys[i].user.chara2 != null)
                {
                    mesData = mesData + mesRet + mFriendData.applys[i].user.chara2.cname;
                }
                prefabObjRenraku[i].transform.Find("name_daishi/Text (2)").gameObject.GetComponent<Text>().text = mesData;

                // たまごっちキャラを登録
                StartCoroutine(cbRenrakuDataSet(i));
            }
        }
        private IEnumerator cbAppDataSet(int num)
        {
            cbApp[num] = prefabObjApp[num].transform.Find("chara_waku/chara/CharaImg").gameObject.GetComponent<CharaBehaviour>();
            yield return cbApp[num].init(mFriendData.appfriends[num].chara1);
            cbApp[num].gotoAndPlay(MotionLabel.IDLE);
        }
        private IEnumerator cbMIX2DataSet(int num)
        {
            cbMIX2[num] = prefabObjMIX2[num].transform.Find("chara_waku/chara/CharaImg").gameObject.GetComponent<CharaBehaviour>();
            yield return cbMIX2[num].init(mFriendData.toyfriends[num].chara1);
            cbMIX2[num].gotoAndPlay(MotionLabel.IDLE);
        }
        private IEnumerator cbRenrakuDataSet(int num)
        {
            cbRenraku[num] = prefabObjRenraku[num].transform.Find("chara_waku/chara/CharaImg").gameObject.GetComponent<CharaBehaviour>();
            yield return cbRenraku[num].init(mFriendData.applys[num].user.chara1);
            cbRenraku[num].gotoAndPlay(MotionLabel.IDLE);
        }

        private int FriendYNNumber;
        // 連絡のともだちになるボタン
        private void BtnFriendOKReq(int num)
        {
            FriendYNNumber = num;
            string _mes;

            ManagerObject.instance.sound.playSe(11);

            EventKakunin.SetActive(true);
            EventKakunin.transform.Find("Text_Arial").gameObject.SetActive(true);
            EventKakunin.transform.Find("Text (1)").gameObject.SetActive(true);
            EventKakunin.transform.Find("Button_red_hai").gameObject.SetActive(true);
            EventKakunin.transform.Find("Button_blue_iie").gameObject.SetActive(true);
            YesNoModeFlag = YesNoModeTable.REPLY_FRIEND_YES;

            EventKakunin.transform.Find("Text_Arial").gameObject.GetComponent<Text>().text = userNicknameChange(mFriendData.applys[num].user.nickname);
            if (mFriendData.applys[num].user.chara2 != null)
            {
                _mes = MsgDataTable_1;
                _mes = _mes + mFriendData.applys[num].user.chara1.cname;
                _mes = _mes + MsgDataTable_2;
                _mes = _mes + mFriendData.applys[num].user.chara2.cname;
                _mes = _mes + MsgDataTable_6;
            }
            else
            {
                _mes = MsgDataTable_1;
                _mes = _mes + mFriendData.applys[num].user.chara1.cname;
                _mes = _mes + MsgDataTable_6;
            }
            EventKakunin.transform.Find("Text (1)").gameObject.GetComponent<Text>().text = _mes;
        }
        // 連絡のともだちにならないボタン
        private void BtnFriendNOReq(int num)
        {
            FriendYNNumber = num;
            string _mes;

            ManagerObject.instance.sound.playSe(11);

            EventKakunin.SetActive(true);
            EventKakunin.transform.Find("Text_Arial").gameObject.SetActive(true);
            EventKakunin.transform.Find("Text (1)").gameObject.SetActive(true);
            EventKakunin.transform.Find("Button_red_hai").gameObject.SetActive(true);
            EventKakunin.transform.Find("Button_blue_iie").gameObject.SetActive(true);
            YesNoModeFlag = YesNoModeTable.REPLY_FRIEND_NO;

            EventKakunin.transform.Find("Text_Arial").gameObject.GetComponent<Text>().text = userNicknameChange(mFriendData.applys[num].user.nickname);
            if (mFriendData.applys[num].user.chara2 != null)
            {
                _mes = MsgDataTable_1;
                _mes = _mes + mFriendData.applys[num].user.chara1.cname;
                _mes = _mes + MsgDataTable_2;
                _mes = _mes + mFriendData.applys[num].user.chara2.cname;
                _mes = _mes + MsgDataTable_7;
            }
            else
            {
                _mes = MsgDataTable_1;
                _mes = _mes + mFriendData.applys[num].user.chara1.cname;
                _mes = _mes + MsgDataTable_7;
            }
            EventKakunin.transform.Find("Text (1)").gameObject.GetComponent<Text>().text = _mes;
        }

        private int SakujoNumber;
        // アプリのともだち削除ボタン
        private void BtnSakujoReq(int num)
        {
            string _mes;
            SakujoNumber = num;

            ManagerObject.instance.sound.playSe(11);

            EventKakunin.SetActive(true);
            EventKakunin.transform.Find("Text_Arial").gameObject.SetActive(true);
            EventKakunin.transform.Find("Text (1)").gameObject.SetActive(true);
            EventKakunin.transform.Find("Button_red_hai").gameObject.SetActive(true);
            EventKakunin.transform.Find("Button_blue_iie").gameObject.SetActive(true);
            YesNoModeFlag = YesNoModeTable.DELETE_FRIEND;

            EventKakunin.transform.Find("Text_Arial").gameObject.GetComponent<Text>().text = userNicknameChange(mFriendData.appfriends[num].nickname);
            if (mFriendData.appfriends[num].chara2 != null)
            {
                _mes = MsgDataTable_1;
                _mes = _mes + mFriendData.appfriends[num].chara1.cname;
                _mes = _mes + MsgDataTable_2;
                _mes = _mes + mFriendData.appfriends[num].chara2.cname;
                _mes = _mes + MsgDataTable_5;
            }
            else
            {
                _mes = MsgDataTable_1;
                _mes = _mes + mFriendData.appfriends[num].chara1.cname;
                _mes = _mes + MsgDataTable_5;
            }
            EventKakunin.transform.Find("Text (1)").gameObject.GetComponent<Text>().text = _mes;
        }



        private int SearchNumber = 0;
        // ともだち検索データセット
        private void SearchDataSet()
        {
            SearchDataClr();

            if (mFriendSearchData == null)
            {
                SearchNumber = 0;
            }
            else
            {
                Vector3 _Pos;
                string mesRet = "\n";
                string mesData;
                int _SearchNumber = mFriendSearchData.Count;

                if (_SearchNumber > SEARCH_MAX)
                {
                    _SearchNumber = SEARCH_MAX;
                }

                // ともだち検索パネルセット
                for (int i = 0; i < _SearchNumber; i++)
                {
                    // プレハブを登録
                    prefabObjSearch[i] = (GameObject)Instantiate(PrefabSearch);
                    prefabObjSearch[i].transform.SetParent(SearchContainer.transform, false);
                    prefabObjSearch[i].name = "friendSearch" + i.ToString();

                    int ii = i + 0;
                    // ともだちになりたいボタンを有効化
                    prefabObjSearch[i].transform.Find("Button_red").gameObject.GetComponent<Button>().onClick.AddListener(() => BtnSearchReq(ii));

                    // プロフィール画面へのボタンを有効化
                    //prefabObjSearch [i].GetComponent<Button> ().onClick.AddListener (() => BtnUserEventSearch (ii));

                    // 表示位置を登録
                    _Pos.x = 0.0f;
                    _Pos.y = 150.0f - (240.0f * i);
                    _Pos.z = 0.0f;
                    prefabObjSearch[i].transform.localPosition = _Pos;

                    // みーつIDを登録
                    prefabObjSearch[i].transform.Find("IDbase/ID").gameObject.GetComponent<Text>().text = mFriendSearchData[i].code;
                    // ユーザー名を登録
                    prefabObjSearch[i].transform.Find("name_daishi/Text").gameObject.GetComponent<Text>().text = userNicknameRetInsert(userNicknameChange(mFriendSearchData[i].nickname));
                    // たまごっちの名前を登録
                    mesData = mFriendSearchData[i].chara1.cname;
                    if (mFriendSearchData[i].chara2 != null)
                    {
                        mesData = mesData + mesRet + mFriendSearchData[i].chara2.cname;
                    }
                    prefabObjSearch[i].transform.Find("name_daishi/Text (2)").gameObject.GetComponent<Text>().text = mesData;

                    // たまごっちキャラを登録
                    StartCoroutine(cbSearchDataSet(i));
                }
                SearchNumber = _SearchNumber;

                //EventSearch.transform.Find ("mask/Panel").transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
            }
        }
        private IEnumerator cbSearchDataSet(int num)
        {
            cbSearch[num] = prefabObjSearch[num].transform.Find("chara_waku/chara/CharaImg").gameObject.GetComponent<CharaBehaviour>();
            yield return cbSearch[num].init(mFriendSearchData[num].chara1);
            cbSearch[num].gotoAndPlay(MotionLabel.IDLE);
        }

        private void SearchDataClr()
        {
            //Vector3 _Pos = new Vector3(0.0f, 0.0f, 0.0f);

            for (int i = 0; i < SearchNumber; i++)
            {
                Destroy(prefabObjSearch[i]);
            }
            SearchNumber = 0;
            //EventSearch.transform.Find ("mask/Panel").transform.localPosition = _Pos;
        }



        private YesNoModeTable YesNoModeFlag = YesNoModeTable.NULL;
        private enum YesNoModeTable
        {
            NULL,
            APPLY_FRIEND,
            DELETE_FRIEND,
            REPLY_FRIEND_YES,
            REPLY_FRIEND_NO,
        };
        private int ReqUserNumber;
        // 友達申請ボタンが押された時
        private void BtnSearchReq(int num)
        {
            ReqUserNumber = num;

            string _mes;

            ManagerObject.instance.sound.playSe(11);

            EventKakunin.SetActive(true);
            EventKakunin.transform.Find("Text_Arial").gameObject.SetActive(true);
            EventKakunin.transform.Find("Text (1)").gameObject.SetActive(true);
            EventKakunin.transform.Find("Button_red_hai").gameObject.SetActive(true);
            EventKakunin.transform.Find("Button_blue_iie").gameObject.SetActive(true);
            YesNoModeFlag = YesNoModeTable.APPLY_FRIEND;

            EventKakunin.transform.Find("Text_Arial").gameObject.GetComponent<Text>().text = userNicknameChange(mFriendSearchData[num].nickname);
            if (mFriendSearchData[num].chara2 != null)
            {
                _mes = MsgDataTable_1;
                _mes = _mes + mFriendSearchData[num].chara1.cname;
                _mes = _mes + MsgDataTable_2;
                _mes = _mes + mFriendSearchData[num].chara2.cname;
                _mes = _mes + MsgDataTable_3;
            }
            else
            {
                _mes = MsgDataTable_1;
                _mes = _mes + mFriendSearchData[num].chara1.cname;
                _mes = _mes + MsgDataTable_3;
            }
            EventKakunin.transform.Find("Text (1)").gameObject.GetComponent<Text>().text = _mes;
        }
        // 申請結果
        private void mApplyFriend(bool success, object data)
        {
            Debug.LogFormat("Friends mApplyFriend:{0},{1}", success, data);
            string _mes;

            if (success)
            {
                // 成功
                EventResult.SetActive(true);
                EventResult.transform.Find("1_shinsei shimashita").gameObject.SetActive(true);
                EventResult.transform.Find("Text_Arial").gameObject.SetActive(true);
                EventResult.transform.Find("Text 1").gameObject.SetActive(true);

                EventResult.transform.Find("Text_Arial").gameObject.GetComponent<Text>().text = userNicknameChange(mFriendSearchData[ReqUserNumber].nickname);
                if (mFriendSearchData[ReqUserNumber].chara2 != null)
                {
                    _mes = MsgDataTable_1;
                    _mes = _mes + mFriendSearchData[ReqUserNumber].chara1.cname;
                    _mes = _mes + MsgDataTable_2;
                    _mes = _mes + mFriendSearchData[ReqUserNumber].chara2.cname;
                    _mes = _mes + MsgDataTable_4;
                }
                else
                {
                    _mes = MsgDataTable_1;
                    _mes = _mes + mFriendSearchData[ReqUserNumber].chara1.cname;
                    _mes = _mes + MsgDataTable_4;
                }
                EventResult.transform.Find("Text 1").gameObject.GetComponent<Text>().text = _mes;

                // 申請した相手をリストから削除する
                mFriendSearchData.Remove(mFriendSearchData[ReqUserNumber]);
                SearchDataSet();
            }
            else
            {
                // 失敗
                int retFlag = (int)data;

                switch (retFlag)
                {
                    case 1:
                        {   // 自分の枠が満杯
                            FriendReqErrorDisp(mFriendSearchData[ReqUserNumber], 1);
                            break;
                        }
                    case 2:
                        {   // 相手の枠が満杯
                            FriendReqErrorDisp(mFriendSearchData[ReqUserNumber], 2);
                            break;
                        }
                    case 3:
                        {   // 既に申請済みの相手
                            FriendReqErrorDisp(mFriendSearchData[ReqUserNumber], 3);
                            // 登録されていたのでリストから削除する
                            mFriendSearchData.Remove(mFriendSearchData[ReqUserNumber]);
                            SearchDataSet();
                            break;
                        }
                }
            }
        }


        private void mDeleteFriend(bool success, object data)
        {
            Debug.LogFormat("Friends mDeleteFriend:{0},{1}", success, data);

            if (success)
            {
                // 成功
                for (int i = 0; i < appfriendsCount; i++)
                {
                    Destroy(prefabObjApp[i]);
                }
                for (int i = 0; i < toyfriendsCount; i++)
                {
                    Destroy(prefabObjMIX2[i]);
                }
                for (int i = 0; i < applysCount; i++)
                {
                    Destroy(prefabObjRenraku[i]);
                }

                // 友達手帳のデータを登録する
                mFriendData = (FriendData)data;
                NoteDataSet();
            }
        }

        // フレンド申請に対する返答
        private void mReplyFriend(bool success, object data)
        {
            Debug.LogFormat("Friends mReplyFriend:{0},{1}", success, data);

            if (success)
            {
                // 成功
                for (int i = 0; i < appfriendsCount; i++)
                {
                    Destroy(prefabObjApp[i]);
                }
                for (int i = 0; i < toyfriendsCount; i++)
                {
                    Destroy(prefabObjMIX2[i]);
                }
                for (int i = 0; i < applysCount; i++)
                {
                    Destroy(prefabObjRenraku[i]);
                }

                // 友達手帳のデータを登録する
                mFriendData = (FriendData)data;
                NoteDataSet();
            }
            else
            {
                // 失敗
                int retFlag = (int)data;

                switch (retFlag)
                {
                    case 1:
                        {   // 自分の枠が満杯
                            FriendReqErrorDisp(mFriendData.applys[FriendYNNumber].user, 1);
                            break;
                        }
                    case 2:
                        {   // 相手の枠が満杯
                            FriendReqErrorDisp(mFriendData.applys[FriendYNNumber].user, 2);
                            break;
                        }
                    case 3:
                        {   // 既に登録済みの相手
                            FriendReqErrorDisp(mFriendData.applys[FriendYNNumber].user, 3);

                            for (int i = 0; i < appfriendsCount; i++)
                            {
                                Destroy(prefabObjApp[i]);
                            }
                            for (int i = 0; i < toyfriendsCount; i++)
                            {
                                Destroy(prefabObjMIX2[i]);
                            }
                            for (int i = 0; i < applysCount; i++)
                            {
                                Destroy(prefabObjRenraku[i]);
                            }

                            mFriendData.applys.Remove(mFriendData.applys[FriendYNNumber]);
                            NoteDataSet();

                            break;
                        }
                }
            }
        }

        private void FriendReqErrorDisp(User _user, int _mode)
        {
            string _mes;

            switch (_mode)
            {
                case 1:
                    {   // 自分の枠が満杯
                        EventResult.SetActive(true);
                        EventResult.transform.Find("3_touroku dekimasen").gameObject.SetActive(true);
                        EventResult.transform.Find("Text 3-1").gameObject.SetActive(true);
                        break;
                    }
                case 2:
                    {   // 相手の枠が満杯
                        EventResult.SetActive(true);
                        EventResult.transform.Find("3_touroku dekimasen").gameObject.SetActive(true);
                        EventResult.transform.Find("Text_Arial").gameObject.SetActive(true);
                        EventResult.transform.Find("Text 3-2").gameObject.SetActive(true);

                        EventResult.transform.Find("Text_Arial").gameObject.GetComponent<Text>().text = userNicknameChange(_user.nickname);

                        if (_user.chara2 != null)
                        {
                            _mes = MsgDataTable_1;
                            _mes = _mes + _user.chara1.cname;
                            _mes = _mes + MsgDataTable_2;
                            _mes = _mes + _user.chara2.cname;
                            _mes = _mes + MsgDataTable_8;
                        }
                        else
                        {
                            _mes = MsgDataTable_1;
                            _mes = _mes + _user.chara1.cname;
                            _mes = _mes + MsgDataTable_8;
                        }
                        EventResult.transform.Find("Text 3-2").gameObject.GetComponent<Text>().text = _mes;
                        break;
                    }
                case 3:
                    {   // 既に申請済みの相手
                        EventResult.SetActive(true);
                        EventResult.transform.Find("0_touroku zumi").gameObject.SetActive(true);
                        EventResult.transform.Find("Text 0").gameObject.SetActive(true);
                        break;
                    }
            }
        }

        private string userNicknameChange(string baseStr)
        {
            string retStr = baseStr;

            if (baseStr != null)
            {
                if (baseStr.Length > 20)
                {
                    retStr = baseStr.Remove(18, baseStr.Length - 18);
                    retStr = retStr + "……";
                }
            }
            else
            {
                retStr = "";
            }

            return retStr;
        }
        private string userNicknameRetInsert(string baseStr)
        {
            string retStr = baseStr;

            if (baseStr != null)
            {
                if (baseStr.Length > 10)
                {
                    retStr = baseStr.Insert(10, "\n");
                }
            }
            else
            {
                retStr = "";
            }

            return retStr;
        }

        /* フレンド画面ではプロフィール詳細表示はないです
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
        */

    }
}