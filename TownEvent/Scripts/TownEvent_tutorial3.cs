using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

namespace Mix2App.TownEvent
{
    public class TownEvent_tutorial3 : MonoBehaviour, IReceiver, IReadyable
    {
        [SerializeField] private GameEventHandler GEHandler = null;
        [SerializeField] private GameObject CameraObj = null;
        [SerializeField] private GameObject MainObj = null;

        private object[] mparam;
        private User muser1;

        void Awake()
        {
            mparam = null;
            mready = false;

        }

        public void receive(params object[] parameter)
        {
            mparam = parameter;

            //単体動作テスト用
            if (mparam == null)
            {
                mparam = new object[] {
                    new RewardData (),  // 報酬
                    4,					// Depth値
				};
            }

            if (mparam.Length == 2)
            {
                CameraObj.transform.GetComponent<Camera>().depth = (int)mparam[1];
            }
            else
            {
                CameraObj.transform.GetComponent<Camera>().depth = 5;
            }

            StartCoroutine(mStart());
        }

        private bool mready = false;
        public bool ready()
        {
            return mready;
        }

        void OnDestroy()
        {
            Debug.Log("OnDestroy");
        }

        IEnumerator mStart()
        {
            Debug.Log("mStart");

            //39:18や21:9などの縦長スクリーンサイズに対応する
            //アスペクト比に応じて、画面のscaleを調整する。
            float kk1 = 1024f / 2048f;
            float kk2 = (float)Screen.height / (float)Screen.width;
            float kk3 = (kk2 < kk1) ? kk2 / kk1 : 1.0f;
            MainObj.transform.Find("tutorial").gameObject.transform.localScale = new Vector3(kk3, kk3, 1);


            MainObj.transform.Find("tutorial/bt_next").gameObject.GetComponent<Button>().onClick.AddListener(ButtonClick);

            mready = true;

            yield return null;
        }


        private void ButtonClick()
        {
            ManagerObject.instance.sound.playSe(17);
            Application.OpenURL("https://tamagotch.channel.or.jp/member/campaigns/summercampaign2019/");
            GEHandler.OnRemoveScene(SceneLabel.TOWN_EVENT + "_tutorial3");
        }

        void Start()
        {

        }

        void Update()
        {

        }



    }
}

