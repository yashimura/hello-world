using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

namespace Mix2App.Achieve
{
    public class Achieve : MonoBehaviour, IReceiver, IReadyable
    {
        [SerializeField] private GameObject Container = null;
        [SerializeField] private GameObject PrefabList = null;
        [SerializeField] private GameObject EventRoot = null;


        private const int LIST_MAX = 100;

        private List<AchieveData> mData;
        private GameObject[] prefabObj = new GameObject[LIST_MAX];

        void Awake()
        {
        }

        public void receive(params object[] parameter)
        {
            GameCall call = new GameCall(CallLabel.GET_ACHIEVES);
            call.AddListener(mGetAchieves);
            ManagerObject.instance.connect.send(call);

        }
        void mGetAchieves(bool success, object data)
        {
            Debug.Log(success + "/" + data);
            if (success)
            {
                mData = (List<AchieveData>)data;

                StartCoroutine(mStart());
            }
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

        IEnumerator mStart()
        {
            EventRoot.transform.Find("Achieve_list/Button_blue_tojiru").gameObject.GetComponent<Button>().onClick.AddListener(ButtonTojiruClick);

            PrefabSet();

            mready = true;

            yield return null;
        }

        private void PrefabSet()
        {
            for (int i = 0; i < mData.Count && i < LIST_MAX; i++)
            {
                // プレハブを登録
                prefabObj[i] = (GameObject)Instantiate(PrefabList);
                prefabObj[i].transform.SetParent(Container.transform, false);
                prefabObj[i].name = "AchieveList" + i.ToString();

                // 背景の色を偶数、奇数で変更する
                prefabObj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f * (i & 1));

                // アチーブメントのタイトルと回数を登録
                prefabObj[i].transform.Find("Title").gameObject.GetComponent<Text>().text = mData[i].atitle;
                prefabObj[i].transform.Find("Times").gameObject.GetComponent<Text>().text = "あと" + mData[i].count.ToString() + "かい";
            }
        }


        void Update()
        {
        }

            private void ButtonTojiruClick()
        {
            ManagerObject.instance.sound.playSe(17);
            ManagerObject.instance.view.change(SceneLabel.HOME);
        }


    }
}
