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

        void Awake()
        {
        }

        public void receive(params object[] parameter)
        {
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



    }
}
