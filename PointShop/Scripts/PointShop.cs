using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

namespace Mix2App.PointShop
{
    public class PointShop : MonoBehaviour, IReceiver, IReadyable
    {
        [SerializeField] private GameObject ItemContainer = null;       // 
        [SerializeField] private GameObject ApplitchiChara = null;
        [SerializeField] private GameObject FukidashiChara = null;
        [SerializeField] private GameObject PointNumber = null;
        [SerializeField] private GameObject ButtonModoru = null;
        [SerializeField] private GameObject ButtonHai = null;
        [SerializeField] private GameObject ItemGetEvent = null;
        [SerializeField] private GameObject[] FukidashiMessage = null;
        [SerializeField] private Sprite[] ApplitchiImage = null;
        [SerializeField] private GameObject PrefabItem = null;



        private const int ITEM_MAX = 100;

        private object[] mparam;
        private int mMinigameID;                                        // ミニゲームID
        private GameObject[] prefabObj = new GameObject[ITEM_MAX];
        private int itemNumber;
        private bool buttonSelectFlag = true;
        private int pointData;                                          // コラボポイントの数
        private int itemCount;                                          // 所持アイテムの数

        private enum ApplitchiAnimeTable
        {
            NORMAL,                 // 普通
            GUIDE,                  // ガイド
            HAPPY,                  // 喜び
            SMILE,                  // 笑顔
            SURPRISE,               // 驚き
            CRY,                    // 泣き
            TROUBLE,                // 困る
        };
        private ApplitchiAnimeTable animeFlag = ApplitchiAnimeTable.NORMAL;
        private Coroutine retApplitchi = null;

        private enum MessageTypeTable
        {
            MESS1,                  // いらっしゃい
            MESS2,                  // 交換しますか？
            MESS3,                  // ポイントが足りない
            MESS4,                  // 持ち物がいっぱい
            MESS5,                  // 毎度あり
            MESS_OFF,
        };



        private struct PointShopData
        {
            //アイテム
            public ItemData items;

            //集めたポイント
            public int point;
        }
        private List<PointShopData> dataTable;



        void Awake()
        {
            mparam = null;
            mready = false;
            buttonSelectFlag = true;
            retApplitchi = null;
        }

        public void receive(params object[] parameter)
        {
            mparam = parameter;

            if (mparam == null)
            {
                mparam = new object[] {
                    3,                  // int ミニゲーム２のイベントID
                };
            }

            mMinigameID = (int)mparam[0];



            dataTable = new List<PointShopData>();

            PointShopData item = new PointShopData();
            item.items.code = "tg18_as16058_1";
            item.items.title = "テストアイテム";
            item.items.kind = 0;
            item.items.version = "tg18";
            item.point = 100;
            dataTable.Add(item);

            item = new PointShopData();
            item.items.code = "tg18_as16055_1";
            item.items.title = "テストアイテム";
            item.items.kind = 0;
            item.items.version = "tg18";
            item.point = 110;
            dataTable.Add(item);

            item = new PointShopData();
            item.items.code = "tg18_as16053_1";
            item.items.title = "テストアイテム";
            item.items.kind = 0;
            item.items.version = "tg18";
            item.point = 120;
            dataTable.Add(item);

            pointData = 1000;           // コラボポイントの数
            itemCount = 0;              // 所持アイテムの数

            StartCoroutine(InitMain());
        }

        private bool mready = false;
        public bool ready()
        {
            return mready;
        }

        void OnDestroy()
        {
            Debug.Log("Destroy");

            if(retApplitchi != null)
            {
                StopCoroutine(retApplitchi);
                retApplitchi = null;
            }
        }

        void Start()
        {
        }

        void Update()
        {
        }

        private IEnumerator InitMain()
        {
            SetupWdth(ItemContainer, 1.0f);

            // プレハブを登録する
            PrefabItemDataSet();

            // ボタンの制御関数を登録する
            ButtonHai.GetComponent<Button>().onClick.AddListener(ButtonClickHai);
            ButtonModoru.GetComponent<Button>().onClick.AddListener(ButtonClickModoru);
            ItemGetEvent.transform.Find("Button_blue_close").gameObject.GetComponent<Button>().onClick.AddListener(ButtonClickClose);

            ApplitchiAnime(ApplitchiAnimeTable.GUIDE);
            MessageJob(MessageTypeTable.MESS1);

            PointNumber.GetComponent<Text>().text = pointData.ToString();

            mready = true;

            yield return null;
        }

        private void PrefabItemDataSet()
        {
            for (int i = 0; i < dataTable.Count && i < ITEM_MAX; i++)
            {
                // プレハブを登録
                prefabObj[i] = (GameObject)Instantiate(PrefabItem);
                prefabObj[i].transform.SetParent(ItemContainer.transform, false);
                prefabObj[i].name = "Item" + i.ToString();

                // アイテムを表示
                ItemBehaviour ibItem;
                ibItem = prefabObj[i].transform.Find("Button_item/ItemView").gameObject.GetComponent<ItemBehaviour>();
                ibItem.init(dataTable[i].items);
                ibItem = prefabObj[i].transform.Find("Item_selected/ItemView").gameObject.GetComponent<ItemBehaviour>();
                ibItem.init(dataTable[i].items);

                // ポイントを表示
                prefabObj[i].transform.Find("Button_item/item_num").gameObject.GetComponent<Text>().text = dataTable[i].point.ToString();
                prefabObj[i].transform.Find("Item_selected/item_num").gameObject.GetComponent<Text>().text = dataTable[i].point.ToString();

                int ii = i + 0;
                // アイテム選択ボタンの有効化
                prefabObj[i].transform.Find("Button_item").gameObject.GetComponent<Button>().onClick.AddListener(() => ButtonClick(ii));
            }
        }

        private void ButtonClickHai()
        {
            if (dataTable[itemNumber].point > pointData)
            {
                // ポイント不足
                ManagerObject.instance.sound.playSe(16);

                if(Random.Range(0,2) == 0)
                {
                    ApplitchiAnime(ApplitchiAnimeTable.CRY);
                }
                else
                {
                    ApplitchiAnime(ApplitchiAnimeTable.TROUBLE);
                }

                MessageJob(MessageTypeTable.MESS3);
                ButtonHai.SetActive(false);
                return;
            }

            if (itemCount >= 99)
            {
                // アイテム所持MAX
                ManagerObject.instance.sound.playSe(16);

                if (Random.Range(0, 2) == 0)
                {
                    ApplitchiAnime(ApplitchiAnimeTable.SURPRISE);
                }
                else
                {
                    ApplitchiAnime(ApplitchiAnimeTable.TROUBLE);
                }

                MessageJob(MessageTypeTable.MESS4);
                ButtonHai.SetActive(false);
                return;
            }

            ManagerObject.instance.sound.playSe(13);

            ApplitchiAnime(ApplitchiAnimeTable.HAPPY);
            MessageJob(MessageTypeTable.MESS5);
            ButtonHai.SetActive(false);
            buttonSelectFlag = false;

            StartCoroutine(ShoppingPointJob());
        }

        private void ButtonClickModoru()
        {
            if (!buttonSelectFlag)
            {
                return;
            }

            ManagerObject.instance.sound.playSe(17);

            Debug.Log("MiniGame2に戻る・・・");
            ManagerObject.instance.view.change(SceneLabel.MINIGAME2, "Town", mMinigameID);
        }

        private void ButtonClickClose()
        {
            ManagerObject.instance.sound.playSe(17);
            ItemGetEvent.transform.localPosition = new Vector3(5000, 0, 0);

            ApplitchiAnime(ApplitchiAnimeTable.GUIDE);
            MessageJob(MessageTypeTable.MESS1);

            for (int i = 0; i < dataTable.Count && i < ITEM_MAX; i++)
            {
                prefabObj[i].transform.Find("Button_item").gameObject.SetActive(true);
                prefabObj[i].transform.Find("Item_selected").gameObject.SetActive(true);

            }
            buttonSelectFlag = true;
        }

        private void ButtonClick(int num)
        {
            if (!buttonSelectFlag)
            {
                return;
            }

            ManagerObject.instance.sound.playSe(12);

            itemNumber = num;

            for (int i = 0;i < dataTable.Count && i < ITEM_MAX; i++)
            {
                prefabObj[i].transform.Find("Button_item").gameObject.SetActive(true);
                prefabObj[i].transform.Find("Item_selected").gameObject.SetActive(true);

                if(num == i)
                {
                    prefabObj[i].transform.Find("Button_item").gameObject.SetActive(false);
                }
            }

            ApplitchiAnime(ApplitchiAnimeTable.SMILE);
            MessageJob(MessageTypeTable.MESS2);
            ButtonHai.SetActive(true);
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



        // アイテムを購入したのでポイントを減らす処理
        private IEnumerator ShoppingPointJob()
        {
            int _subPoint = dataTable[itemNumber].point;

            while (true)
            {
                ManagerObject.instance.sound.playSe(6);
                _subPoint--;
                pointData--;

                PointNumber.GetComponent<Text>().text = pointData.ToString();

                yield return new WaitForSeconds(0.01f);

                if(_subPoint == 0)
                {
                    yield return new WaitForSeconds(0.5f);

                    // 結果画面を表示する
                    ItemBehaviour ibItem;
                    ibItem = ItemGetEvent.transform.Find("ItemView").gameObject.GetComponent<ItemBehaviour>();
                    ibItem.init(dataTable[itemNumber].items);

                    ItemGetEvent.transform.localPosition = new Vector3(0, 0, 0);

                    ManagerObject.instance.sound.playSe(15);



                    // ここでアイテム購入情報などを送信する？      購入したアイテム、購入金額・・・



                    itemCount++;                // 所持アイテム数を増やす

                    break;
                }
            }

            yield return null;
        }



        private void MessageJob(MessageTypeTable num)
        {
            FukidashiChara.SetActive(true);
            for(int i = 0;i < FukidashiMessage.Length; i++)
            {
                // メッセージを一度全部消す
                FukidashiMessage[i].SetActive(false);
            }

            switch (num)
            {
                case MessageTypeTable.MESS1:
                    {   // いらっしゃい
                        FukidashiMessage[0].SetActive(true);
                        break;
                    }
                case MessageTypeTable.MESS2:
                    {   // 交換しますか？
                        FukidashiMessage[1].SetActive(true);
                        break;
                    }
                case MessageTypeTable.MESS3:
                    {   // ポイントが足りない
                        FukidashiMessage[2].SetActive(true);
                        break;
                    }
                case MessageTypeTable.MESS4:
                    {   // 持ち物がいっぱい
                        FukidashiMessage[3].SetActive(true);
                        break;
                    }
                case MessageTypeTable.MESS5:
                    {   // 毎度あり
                        FukidashiMessage[4].SetActive(true);
                        break;
                    }
                case MessageTypeTable.MESS_OFF:
                    {   // メッセージオフ
                        FukidashiChara.SetActive(false);
                        break;
                    }
            }
        }



        // アプリッチのアニメを変更する
        private void ApplitchiAnime(ApplitchiAnimeTable num)
        {
            if (animeFlag != num)
            {
                ApplitchiChara.transform.localPosition = new Vector3(517.0f, -153.0f, 0.0f);
                ApplitchiChara.transform.localScale = new Vector3(5.0f, 5.0f, 1.0f);
                ApplitchiChara.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

                animeFlag = num;

                if (retApplitchi != null)
                {
                    StopCoroutine(retApplitchi);
                    retApplitchi = null;
                }

                switch (animeFlag)
                {
                    case ApplitchiAnimeTable.NORMAL:
                        {   // 普通
                            retApplitchi = StartCoroutine(ApplitchiAnimeNORMAL());
                            break;
                        }
                    case ApplitchiAnimeTable.GUIDE:
                        {   // ガイド
                            retApplitchi = StartCoroutine(ApplitchiAnimeGUIDE());
                            break;
                        }
                    case ApplitchiAnimeTable.SMILE:
                        {   // 笑顔
                            retApplitchi = StartCoroutine(ApplitchiAnimeSMILE());
                            break;
                        }
                    case ApplitchiAnimeTable.CRY:
                        {   // 泣く
                            retApplitchi = StartCoroutine(ApplitchiAnimeCRY());
                            break;
                        }
                    case ApplitchiAnimeTable.TROUBLE:
                        {   // 困る
                            retApplitchi = StartCoroutine(ApplitchiAnimeTROUBLE());
                            break;
                        }
                    case ApplitchiAnimeTable.SURPRISE:
                        {   // 驚く
                            retApplitchi = StartCoroutine(ApplitchiAnimeSURPRISE());
                            break;
                        }
                    case ApplitchiAnimeTable.HAPPY:
                        {   // 喜び
                            retApplitchi = StartCoroutine(ApplitchiAnimeHAPPY());
                            break;
                        }
                }
            }
        }



        // 普通
        private IEnumerator ApplitchiAnimeNORMAL()
        {
            ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[5];

            while (true)
            {
                ApplitchiChara.transform.localScale = new Vector3(5.0f, 5.0f, 1.0f);
                ApplitchiChara.transform.localPosition = new Vector3(517.0f, -153.0f + 1f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(517.0f, -153.0f - 1f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(517.0f, -153.0f + 1f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(517.0f, -153.0f - 1f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(517.0f, -153.0f, 0.0f);
                yield return new WaitForSeconds(0.5f);

                ApplitchiChara.transform.localScale = new Vector3(-5.0f, 5.0f, 1.0f);
                ApplitchiChara.transform.localPosition = new Vector3(517.0f, -153.0f + 1f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(517.0f, -153.0f - 1f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(517.0f, -153.0f + 1f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(517.0f, -153.0f - 1f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(517.0f, -153.0f, 0.0f);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // ガイド
        private IEnumerator ApplitchiAnimeGUIDE()
        {
            while (true)
            {
                ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[4];
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[5];
                yield return new WaitForSeconds(0.5f);
            }
        }

        // 笑顔
        private IEnumerator ApplitchiAnimeSMILE()
        {
            while (true)
            {
                ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[2];
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[3];
                yield return new WaitForSeconds(0.5f);
            }
        }

        // 泣く
        private IEnumerator ApplitchiAnimeCRY()
        {
            ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[9];
            while (true)
            {
                ApplitchiChara.transform.eulerAngles = new Vector3(0.0f, 0.0f, 1.0f);
                yield return new WaitForSeconds(0.2f);
                ApplitchiChara.transform.eulerAngles = new Vector3(0.0f, 0.0f, -1.0f);
                yield return new WaitForSeconds(0.2f);
            }
        }

        // 困る
        private IEnumerator ApplitchiAnimeTROUBLE()
        {
            ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[7];
            while (true)
            {
                ApplitchiChara.transform.localScale = new Vector3(5.0f, 5.0f, 1.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localScale = new Vector3(-5.0f, 5.0f, 1.0f);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // 驚き
        private IEnumerator ApplitchiAnimeSURPRISE()
        {
            ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[8];

            Vector3 _pos = new Vector3(517.0f, -153.0f, 0.0f);

            yield return new WaitForSeconds(0.1f);

            _pos.y += 30.0f;
            while (true)
            {
                ApplitchiChara.transform.localPosition = Vector3.MoveTowards(ApplitchiChara.transform.localPosition, _pos, 200.0f * Time.deltaTime);
                if (ApplitchiChara.transform.localPosition.y == _pos.y)
                {
                    break;
                }
                yield return null;
            }
            _pos.y += 5.0f;
            while (true)
            {
                ApplitchiChara.transform.localPosition = Vector3.MoveTowards(ApplitchiChara.transform.localPosition, _pos, 25.0f * Time.deltaTime);
                if (ApplitchiChara.transform.localPosition.y == _pos.y)
                {
                    break;
                }
                yield return null;
            }
            _pos.y -= 5.0f;
            while (true)
            {
                ApplitchiChara.transform.localPosition = Vector3.MoveTowards(ApplitchiChara.transform.localPosition, _pos, 25.0f * Time.deltaTime);
                if (ApplitchiChara.transform.localPosition.y == _pos.y)
                {
                    break;
                }
                yield return null;
            }
            _pos.y -= 30.0f;
            while (true)
            {
                ApplitchiChara.transform.localPosition = Vector3.MoveTowards(ApplitchiChara.transform.localPosition, _pos, 200.0f * Time.deltaTime);
                if (ApplitchiChara.transform.localPosition.y == _pos.y)
                {
                    break;
                }
                yield return null;
            }

            for (int i = 0; i < 2; i++)
            {
                _pos.y += 5.0f;
                while (true)
                {
                    ApplitchiChara.transform.localPosition = Vector3.MoveTowards(ApplitchiChara.transform.localPosition, _pos, 50.0f * Time.deltaTime);
                    if (ApplitchiChara.transform.localPosition.y == _pos.y)
                    {
                        break;
                    }
                    yield return null;
                }
                _pos.y -= 5.0f;
                while (true)
                {
                    ApplitchiChara.transform.localPosition = Vector3.MoveTowards(ApplitchiChara.transform.localPosition, _pos, 50.0f * Time.deltaTime);
                    if (ApplitchiChara.transform.localPosition.y == _pos.y)
                    {
                        break;
                    }
                    yield return null;
                }
            }

            while (true)
            {
                yield return null;
            }
        }

        // 喜び
        private IEnumerator ApplitchiAnimeHAPPY()
        {
            while (true)
            {
                ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[5];
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[6];
                yield return new WaitForSeconds(0.5f);
            }
        }



    }
}