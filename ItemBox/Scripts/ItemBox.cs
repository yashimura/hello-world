using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

namespace Mix2App.ItemBox
{
    public class ItemBox : MonoBehaviour, IReceiver, IReadyable
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject ItemContainer;
        [SerializeField] private GameObject EventItemBoxMenu;           // プレゼントボックスメイン画面
        [SerializeField] private GameObject EventPresent;               // プレゼントボックスアイテム一覧画面
        [SerializeField] private GameObject EventTushinOk;              // 通信成功画面
        [SerializeField] private GameObject EventTushinNot;             // 通信失敗画面
        [SerializeField] private GameObject EventPresentPoint;          // プレゼントボックスポイント表示画面
        [SerializeField] private GameObject EventTushinPoint;           // プレゼントボックスポイント転送量決定画面
        [SerializeField] private GameObject EventDialogKakunin;         // 確認画面
        [SerializeField] private GameObject EventDialogError;           // エラー画面
        [SerializeField] private GameObject EventTushinTime;            // 通信中画面
        [SerializeField] private GameObject ToyKakunin;                 // 通信前の玩具操作確認画面
        [SerializeField] private Button ButtonToyKakuninYes;            // 通信前の玩具操作確認画面　はいボタン

        [SerializeField] private Button ButtonPresent;                  // メイン画面 アイテム一覧画面へのボタン
        [SerializeField] private Button ButtonPoint;                    // メイン画面 ポイント表示画面へのボタン
        [SerializeField] private Button ButtonTojiru;                   // メイン画面 とじるボタン
        [SerializeField] private Button ButtonGPS;                      // メイン画面 GPSショートカットボタン

        [SerializeField] private Button ButtonPresentBack;              // アイテム一覧画面　もどるボタン

        [SerializeField] private Button ButtonBackOK;                   // 通信成功画面 もどるボタン
        [SerializeField] private Button ButtonBackNot;                  // 通信失敗画面 もどるボタン

        [SerializeField] private Button ButtonPointBack;                // ポイント表示画面　もどるボタン

        [SerializeField] private Button ButtonTushinBack;               // ポイント転送量決定画面　もどるボタン
        [SerializeField] private Button ButtonTushinSend;               // ポイント転送量決定画面　おくるボタン
        [SerializeField] private Button ButtonTushinSendAll;            // ポイント転送量決定画面　ぜんぶおくるボタン
        [SerializeField] private Button[] ButtonTushinUp;               // ポイント転送量決定画面　上ボタン（全４つ）
        [SerializeField] private Button[] ButtonTushinDown;             // ポイント転送量決定画面　下ボタン（全４つ）

        [SerializeField] private Button ButtonKakuninYes;               // 確認画面　はいボタン
        [SerializeField] private Button ButtonKakuninNo;                // 確認画面　いいえボタン
        [SerializeField] private Button ButtonErrorTojiru;              // エラー画面　とじるボタン

        [SerializeField] private Sprite[] SuujiBlack;                   // 
        [SerializeField] private Sprite[] SuujiRed;                     // 

        [SerializeField] private GameObject WakuPrefab;                 // 
        [SerializeField] private GameObject WakuMIX2Prefab;             // 



        private object[] mparam;
        private User muser1;//自分
        private PresentData mPresentData;
        private bool mready;
        private int gPointMax;
        private int gPointNow;
        private int itemCountNow;
        private int itemCountMax;
        private int itemNumberNow;      // 転送するアイテムの番号
        private GameObject[] prefabObj = new GameObject[100];
        private int sendMode;

        void Awake()
        {
            Debug.Log("ItemBox Awake");
            mparam = null;
            muser1 = null;
            mready=false;
        }

        public void receive(params object[] parameter)
        {
            Debug.Log("ItemBox receive");

            mstart();
        }

        public bool ready ()
        {
            return mready;
        }

        void mBleSendGift(bool success, object data)
        {
            Debug.LogFormat("ItemBox mBleSendGift:{0},{1}", success, data);

            EventTushinTime.SetActive(false);

            if (success)
            {
                for (int i = 0; i < itemCountNow; i++)
                {
                    Destroy(prefabObj[i]);
                }

                mPresentDataExpansion((PresentData)data);
                EventTushinOk.SetActive(true);
            }
            else
            {
                EventTushinNot.SetActive(true);
            }
        }

        void mstart()
        {
            muser1 = ManagerObject.instance.player;

            EventItemBoxMenu.SetActive(true);
            EventPresent.SetActive(true);
            ViewSwitch(EventPresent, false);

            // mPresentDataを展開して必要な画面に登録する
            mPresentDataExpansion(null);

            ButtonPresent.onClick.AddListener(ButtonPresentClick);
            ButtonPoint.onClick.AddListener(ButtonPointClick);
            ButtonTojiru.onClick.AddListener(ButtonTojiruClick);
            ButtonPresentBack.onClick.AddListener(ButtonPresentBackClick);
            ButtonBackOK.onClick.AddListener(ButtonBackOKClick);
            ButtonBackNot.onClick.AddListener(ButtonBackNotClick);
            ButtonPointBack.onClick.AddListener(ButtonPointBackClick);
            ButtonTushinBack.onClick.AddListener(ButtonTushinBackClick);
            ButtonTushinSend.onClick.AddListener(ButtonTushinSendClick);
            ButtonTushinSendAll.onClick.AddListener(ButtonTushinSendAllClick);
            ButtonKakuninYes.onClick.AddListener(ButtonKakuninYesClick);
            ButtonKakuninNo.onClick.AddListener(ButtonKakuninNoClick);
            ButtonErrorTojiru.onClick.AddListener(ButtonErrorTojiruClick);

            ButtonGPS.onClick.AddListener(ButtonGPSClick);
            ButtonToyKakuninYes.onClick.AddListener(ButtonToyKakuninYesClick);

            ButtonTushinUp[0].onClick.AddListener(ButtonTushinUp1000Click);
            ButtonTushinUp[1].onClick.AddListener(ButtonTushinUp100Click);
            ButtonTushinUp[2].onClick.AddListener(ButtonTushinUp10Click);
            ButtonTushinUp[3].onClick.AddListener(ButtonTushinUp1Click);
            ButtonTushinDown[0].onClick.AddListener(ButtonTushinDown1000Click);
            ButtonTushinDown[1].onClick.AddListener(ButtonTushinDown100Click);
            ButtonTushinDown[2].onClick.AddListener(ButtonTushinDown10Click);
            ButtonTushinDown[3].onClick.AddListener(ButtonTushinDown1Click);

        }

        // シーンパーツの表示のON/OFFの代わりにY座標移動で対応
        private void ViewSwitch(GameObject _obj, bool _flag)
        {
            Vector3 _pos = new Vector3(0.0f, 4.0f, 0.0f);

            if (!_flag)
            {
                _pos.y = 5000.0f;
            }
            _obj.transform.localPosition = _pos;
        }

        private void SetupWdth(GameObject container, float GridRatio)
        {
            GridLayoutGroup gr = container.GetComponent<GridLayoutGroup>();
            if (gr != null)
                if (gr.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
                {
                    float width = container.GetComponent<RectTransform>().rect.size.x;
                    width = (width - gr.spacing.x * (gr.constraintCount + 1)) / (float)(gr.constraintCount);
                    Debug.Log(GridRatio);
                    gr.cellSize = new Vector2(width, width * GridRatio);
                    Debug.Log("cellsize=" + gr.cellSize);
                }
        }

        // アイテム一覧画面のアイテム毎にボタン化しボタンが押されたら確認画面を開く
        private void ButtonWakuClick(int num)
        {
            ManagerObject.instance.sound.playSe(11);

            EventDialogKakunin.SetActive(true);
            EventDialogKakunin.transform.Find("waku/1").gameObject.SetActive(true);

            ItemBehaviour ibItem = EventDialogKakunin.transform.Find("waku/1/ItemView").gameObject.GetComponent<ItemBehaviour>();
            ibItem.init(mPresentData.items[num]);

            itemNumberNow = num;

            // アイテム送信予定済み
            sendMode |= 1;

        }

        // 集めたアイテムをみるボタンが押された時
        private void ButtonPresentClick()
        {
            ManagerObject.instance.sound.playSe(11);

            //アイテム送信は、玩具の「プレゼント」からの通信なので、
            //おでかけしてると選択できないのでMIX2制限は外す
            ViewSwitch(EventPresent, true);

            gPointNow = 0;
            itemNumberNow = -1;

            sendMode = 0;
        }
        // 集めたごっちポイントをみるボタンが押された時
        private void ButtonPointClick()
        {
            ManagerObject.instance.sound.playSe(11);

            EventPresentPoint.SetActive(true);

            //送信はアイテムとセットなのでポイント主体の送信はできないようにする
            ButtonPointBack.gameObject.SetActive(true);

            gPointNow = 0;
            itemNumberNow = -1;

            sendMode = 0;
        }
        // ルートメニューでとじるが押された時
        private void ButtonTojiruClick()
        {
            ManagerObject.instance.sound.playSe(17);

            ManagerObject.instance.view.change(SceneLabel.HOME);
        }
        // アイテム一覧でもどるが押された時
        private void ButtonPresentBackClick()
        {
            ManagerObject.instance.sound.playSe(17);

            ViewSwitch(EventPresent, false);
        }
        // 通信成功画面のもどるボタン
        private void ButtonBackOKClick()
        {
            ManagerObject.instance.sound.playSe(17);

            EventTushinOk.SetActive(false);
        }
        // 通信失敗画面のもどるボタン
        private void ButtonBackNotClick()
        {
            ManagerObject.instance.sound.playSe(17);

            EventTushinNot.SetActive(false);
        }
        // ごっちポイント表示画面のもどるボタン（MIX2以外用）
        private void ButtonPointBackClick()
        {
            ManagerObject.instance.sound.playSe(17);

            EventPresentPoint.SetActive(false);
        }
        // ごっちポイント送信画面のもどるボタン
        private void ButtonTushinBackClick()
        {
            ManagerObject.instance.sound.playSe(17);

            EventTushinPoint.SetActive(false);
            EventPresentPoint.SetActive(false);

            ViewSwitch(EventPresent, true);
            sendMode = 0;
            gPointNow = 0;

            _btnTushinCheckFlag = false;
        }
        // ごっちポイント送信画面の送るボタン
        private void ButtonTushinSendClick()
        {
            ManagerObject.instance.sound.playSe(11);

            pointKakuninSet();
            EventDialogKakunin.SetActive(true);
            EventDialogKakunin.transform.Find("waku/3").gameObject.SetActive(true);

            // ごっちポイント送信予定済み
            sendMode |= 2;
        }
        // ごっちポイント送信画面の全部送るボタン
        private void ButtonTushinSendAllClick()
        {
            ManagerObject.instance.sound.playSe(11);

            gPointNow = gPointMax;
            if (gPointNow >= 10000)
            {
                gPointNow = 9999;
            }
            pointSendSet();
            ButtonTushinSendClick();
        }

        private void ButtonGPSClick()
        {
            ManagerObject.instance.sound.playSe(11);
            ManagerObject.instance.view.change(SceneLabel.DISCOVER);
        }

        private void ButtonToyKakuninYesClick()
        {
            ManagerObject.instance.sound.playSe(13);
            PresentSendJob2();
        }

        // 確認画面のはいボタン
        private void ButtonKakuninYesClick()
        {
            ManagerObject.instance.sound.playSe(13);

            switch (sendMode)
            {
                case 1:
                    {   // アイテム送信の確認のはいが押された時（まだ、ごっちポイントは決定していない時）
                        kakuninMenuOFF();
                        sendMode |= 16;
                        if (gPointMax>0)
                        {
                            EventDialogKakunin.transform.Find("waku/2").gameObject.SetActive(true);
                            return;
                        }
                        else
                        {
                            break;
                        }
                    }
                case 2:
                    {   // ごっちポイント送信の確認のはいが押された時（まだ、アイテムは決定していない時）
                        kakuninMenuOFF();
                        EventDialogKakunin.transform.Find("waku/4").gameObject.SetActive(true);
                        sendMode |= 16;
                        return;
                    }
                case 17:
                    {   // ごっちポイントも送るかの確認画面（ごっちポイント送信画面を開く）
                        kakuninMenuOFF();
                        EventDialogKakunin.SetActive(false);

                        ViewSwitch(EventPresent, false);

                        gPointNow = 0;
                        pointSendSet();
                        EventTushinPoint.SetActive(true);

                        _btnTushinCheckFlag = true;
                        StartCoroutine("BtnTushinCheck");
                        return;
                    }
                case 18:
                    {   // アイテムも送るかの確認画面（アイテム一覧画面を開く）
                        kakuninMenuOFF();
                        EventDialogKakunin.SetActive(false);

                        EventPresentPoint.SetActive(false);
                        EventTushinPoint.SetActive(false);
                        _btnTushinCheckFlag = false;

                        ViewSwitch(EventPresent, true);
                        itemNumberNow = -1;
                        return;
                    }
            }

            // 玩具へアイテムやごっちポイントを送る
            PresentSendJob();

        }
        // 確認画面のいいえボタン
        private void ButtonKakuninNoClick()
        {
            ManagerObject.instance.sound.playSe(14);

            switch (sendMode)
            {
                case 17:// アイテムを送信する
                case 18:// ごっちポイントを送信する
                    PresentSendJob();
                    return;
            }

            kakuninMenuOFF();
            EventDialogKakunin.SetActive(false);
        }
        private void ButtonErrorTojiruClick()
        {
            EventDialogError.SetActive(false);
            EventDialogError.transform.Find("1").gameObject.SetActive(false);
            EventDialogError.transform.Find("2").gameObject.SetActive(false);
            EventDialogError.transform.Find("3").gameObject.SetActive(false);
        }

        private void kakuninMenuOFF()
        {
            EventDialogKakunin.transform.Find("waku/1").gameObject.SetActive(false);
            EventDialogKakunin.transform.Find("waku/2").gameObject.SetActive(false);
            EventDialogKakunin.transform.Find("waku/3").gameObject.SetActive(false);
            EventDialogKakunin.transform.Find("waku/4").gameObject.SetActive(false);
        }

        // 玩具へアイテムやごっちポイントを送る
        private void PresentSendJob()
        {
            if (ManagerObject.instance.app.toys.Count > 0)
            {
                ToyKakunin.SetActive(true);
            }
            else
            {
                EventDialogError.SetActive(true);
                EventDialogError.transform.Find("3").gameObject.SetActive(true);
                clearmenu();
            }
        }

        private void PresentSendJob2()
        {
            GameCall call;
            //		Debug.Log (itemNumberNow.ToString () + "/" + gPointNow.ToString () + " " + sendMode.ToString());

            if (itemNumberNow != -1)
            {
                call = new GameCall(CallLabel.BLE_SEND_GIFT, gPointNow, mPresentData.items[itemNumberNow]);
            }
            else
            {
                call = new GameCall(CallLabel.BLE_SEND_GIFT, gPointNow);
            }
            call.AddListener(mBleSendGift);
            ManagerObject.instance.connect.send(call);

            EventTushinTime.SetActive(true);
            clearmenu();
        }

        void clearmenu()
        {
            kakuninMenuOFF();
            EventDialogKakunin.SetActive(false);

            ToyKakunin.SetActive(false);

            ViewSwitch(EventPresent, false);
            EventPresentPoint.SetActive(false);
            EventTushinPoint.SetActive(false);
            _btnTushinCheckFlag = false;
        }


        // ごっちポイント送信画面の１０００ポイントアップボタン
        private void ButtonTushinUp1000Click()
        {
            ManagerObject.instance.sound.playSe(11);

            pointNowAdd(1000);
        }
        // ごっちポイント送信画面の１００ポイントアップボタン
        private void ButtonTushinUp100Click()
        {
            ManagerObject.instance.sound.playSe(11);

            pointNowAdd(100);
        }
        // ごっちポイント送信画面の１０ポイントアップボタン
        private void ButtonTushinUp10Click()
        {
            ManagerObject.instance.sound.playSe(11);

            pointNowAdd(10);
        }
        // ごっちポイント送信画面の１ポイントアップボタン
        private void ButtonTushinUp1Click()
        {
            ManagerObject.instance.sound.playSe(11);

            pointNowAdd(1);
        }
        // ごっちポイント送信画面の１０００ポイントダウンボタン
        private void ButtonTushinDown1000Click()
        {
            ManagerObject.instance.sound.playSe(11);

            pointNowSub(1000);
        }
        // ごっちポイント送信画面の１００ポイントダウンボタン
        private void ButtonTushinDown100Click()
        {
            ManagerObject.instance.sound.playSe(11);

            pointNowSub(100);
        }
        // ごっちポイント送信画面の１０ポイントダウンボタン
        private void ButtonTushinDown10Click()
        {
            ManagerObject.instance.sound.playSe(11);

            pointNowSub(10);
        }
        // ごっちポイント送信画面の１ポイントダウンボタン
        private void ButtonTushinDown1Click()
        {
            ManagerObject.instance.sound.playSe(11);

            pointNowSub(1);
        }

        private void pointNowAdd(int _point)
        {
            gPointNow += _point;
            if (gPointNow > gPointMax)
            {
                gPointNow = gPointMax;
            }
            if (gPointNow >= 10000)
            {
                gPointNow = 9999;
            }
            pointSendSet();
        }
        private void pointNowSub(int _point)
        {
            gPointNow -= _point;
            if (gPointNow <= 0)
            {
                gPointNow = 0;
            }
            pointSendSet();
        }

        private bool _btnTushinCheckFlag;
        private IEnumerator BtnTushinCheck()
        {
            while (_btnTushinCheckFlag)
            {
                ButtonTushinSend.gameObject.SetActive(true);
                ButtonTushinSendAll.gameObject.SetActive(true);

                if (gPointMax == 0)
                {
                    // 所有ごっちポイントが０の時は送るボタン系は全てオフ
                    ButtonTushinSend.gameObject.SetActive(false);
                    ButtonTushinSendAll.gameObject.SetActive(false);
                }
                else
                {
                    if (gPointNow == 0)
                    {
                        // 送信ごっちポイントが０の時は送るボタンはオフ
                        ButtonTushinSend.gameObject.SetActive(false);
                    }
                }

                yield return null;
            }
        }

        // PresentDataを展開し、画面に必要なものを抽出する
        private void mPresentDataExpansion(object data)
        {
            if (data == null)
            {
                // プレゼントデータを取得
                GameCall call = new GameCall(CallLabel.GET_PRESENTS);
                call.AddListener(mGetPresents);
                ManagerObject.instance.connect.send(call);
            }
            else
            {
                mGetPresents(true, data);
            }
        }

        private void mGetPresents(bool success, object data)
        {
            // dataの内容は設計書参照
            mPresentData = (PresentData)data;

            gPointMax = mPresentData.gpt;
            gPointNow = 0;

            itemCountMax = mPresentData.items.Count;
            if (itemCountMax > 100)
            {
                itemCountNow = 100;
            }
            else
            {
                itemCountNow = itemCountMax;
            }

            SetupWdth(ItemContainer, 1.0f);

            PresentItemBoxSet();                // アイテム一覧画面にプレハブとボタンを登録する

            pointSet(EventPresentPoint);        // プレゼントボックスポイント表示画面（集めたポイント）を登録する
            pointSet(EventTushinPoint);     // プレゼントボックスポイント転送量決定画面（集めたポイント）を登録する
            pointSendSet();                 // プレゼントボックスポイント転送量決定画面（送るポイント）を登録する

            mready = true;
        }

        // アイテム一覧画面
        private void PresentItemBoxSet()
        {
            GameObject _Obj = EventPresent;

            for (int i = 0; i < itemCountNow; i++)
            {
                //if (muser1.utype == UserType.MIX2) {
                prefabObj[i] = (GameObject)Instantiate(WakuMIX2Prefab);
                //} else {
                //	prefabObj [i] = (GameObject)Instantiate (WakuPrefab);
                //}
                prefabObj[i].transform.SetParent(ItemContainer.transform, false);
                prefabObj[i].name = "Waku" + i.ToString();
                //if (muser1.utype == UserType.MIX2) {
                int ii = i + 0;
                // アイテム選択ボタンの有効化
                prefabObj[i].GetComponent<Button>().onClick.AddListener(() => ButtonWakuClick(ii));
                //}

                ItemBehaviour ibItem = prefabObj[i].transform.Find("ItemView").gameObject.GetComponent<ItemBehaviour>();
                ibItem.init(mPresentData.items[i]);
            }

            if (itemCountMax > 100)
            {
                _Obj.transform.Find("item_kazu/100").transform.GetComponent<Image>().sprite = SuujiRed[itemCountMax / 100];
                _Obj.transform.Find("item_kazu/10").transform.GetComponent<Image>().sprite = SuujiRed[(itemCountMax % 100) / 10];
                _Obj.transform.Find("item_kazu/1").transform.GetComponent<Image>().sprite = SuujiRed[itemCountMax % 10];

                _Obj.transform.Find("item_kazu/100").gameObject.SetActive(true);
                _Obj.transform.Find("item_kazu/10").gameObject.SetActive(true);
                _Obj.transform.Find("item_kazu/1").gameObject.SetActive(true);
            }
            else
            {
                bool _flag2 = false;

                if (itemCountMax == 100)
                {
                    _Obj.transform.Find("item_kazu/100").transform.GetComponent<Image>().sprite = SuujiBlack[1];
                    _Obj.transform.Find("item_kazu/100").gameObject.SetActive(true);
                    _flag2 = true;
                }
                else
                {
                    _Obj.transform.Find("item_kazu/100").gameObject.SetActive(false);
                }
                if ((((itemCountMax % 100) / 10) != 0) || (_flag2))
                {
                    _Obj.transform.Find("item_kazu/10").transform.GetComponent<Image>().sprite = SuujiBlack[(itemCountMax % 100) / 10];
                    _Obj.transform.Find("item_kazu/10").gameObject.SetActive(true);
                }
                else
                {
                    _Obj.transform.Find("item_kazu/10").gameObject.SetActive(false);
                }
                _Obj.transform.Find("item_kazu/1").transform.GetComponent<Image>().sprite = SuujiBlack[itemCountMax % 10];
            }
        }



        // ごっちポイント表示画面と送信画面の所持ごっちポイント表示
        private void pointSet(GameObject obj)
        {
            GotchiBehaviour gbPoint = obj.transform.Find("waku/GotchiView").gameObject.GetComponent<GotchiBehaviour>();
            gbPoint.init(gPointMax);
        }

        // ごっち送信画面の送るごっちポイント表示
        private void pointSendSet()
        {
            EventTushinPoint.transform.Find("waku/01000").transform.GetComponent<Image>().sprite = SuujiBlack[(gPointNow % 10000) / 1000];
            EventTushinPoint.transform.Find("waku/00100").transform.GetComponent<Image>().sprite = SuujiBlack[(gPointNow % 1000) / 100];
            EventTushinPoint.transform.Find("waku/00010").transform.GetComponent<Image>().sprite = SuujiBlack[(gPointNow % 100) / 10];
            EventTushinPoint.transform.Find("waku/00001").transform.GetComponent<Image>().sprite = SuujiBlack[(gPointNow % 10)];
        }

        // 確認画面のごっちポイント表示
        private void pointKakuninSet()
        {
            GotchiBehaviour gbPoint = EventDialogKakunin.transform.Find("waku/3/GotchiView").gameObject.GetComponent<GotchiBehaviour>();
            gbPoint.init(gPointNow);
        }
    }
}