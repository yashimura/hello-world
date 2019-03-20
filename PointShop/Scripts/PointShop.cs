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
        [SerializeField] private GameObject[] FukidashiMessage = null;  // 0：いらっしゃい、1：交換しますか、2：ポイントが足りない、3：持ち物がいっぱい、4：毎度あり
        [SerializeField] private Sprite[] ApplitchiImage = null;        // アプリっちの表示スプライトリスト
        [SerializeField] private GameObject PrefabItem = null;


        private const int ITEM_MAX = 100;

        private object[] mparam;
        private GameObject[] prefabObj = new GameObject[ITEM_MAX];



        private List<ItemData> items;


        void Awake()
        {
            mparam = null;
            mready = false;
        }

        public void receive(params object[] parameter)
        {
            mparam = parameter;

            if (mparam == null)
            {
            }

            items = new List<ItemData>();

            ItemData item = new ItemData();
            item.code = "tg18_as16058_1";
            item.title = "テストアイテム";
            item.kind = 0;
            item.version = "tg18";
            items.Add(item);

            item = new ItemData();
            item.code = "tg18_as16055_1";
            item.title = "テストアイテム";
            item.kind = 0;
            item.version = "tg18";
            items.Add(item);

            item = new ItemData();
            item.code = "tg18_as16053_1";
            item.title = "テストアイテム";
            item.kind = 0;
            item.version = "tg18";
            items.Add(item);

            StartCoroutine(prefabItemSet());


        }

        private bool mready = false;
        public bool ready()
        {
            return mready;
        }

        void OnDestroy()
        {
        }

        void Start()
        {
        }

        void Update()
        {
        }

        private IEnumerator prefabItemSet()
        {

            ItemDataSet();

            yield return null;
        }

        private void ItemDataSet()
        {
            for (int i = 0; i < items.Count && i < ITEM_MAX; i++)
            {
                // プレハブを登録
                prefabObj[i] = (GameObject)Instantiate(PrefabItem);
                prefabObj[i].transform.SetParent(ItemContainer.transform, false);
                prefabObj[i].name = "Item" + i.ToString();

                // アイテムを表示
                ItemBehaviour ibItem;

                ibItem = prefabObj[i].transform.Find("Button_item/ItemView").gameObject.GetComponent<ItemBehaviour>();
                ibItem.init(items[i]);

                ibItem = prefabObj[i].transform.Find("Item_selected/ItemView").gameObject.GetComponent<ItemBehaviour>();
                ibItem.init(items[i]);

            }

        }





    }
}