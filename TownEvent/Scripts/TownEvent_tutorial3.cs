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

            MainObj.transform.Find("tutorial/bt_close").gameObject.GetComponent<Button>().onClick.AddListener(ButtonClick);

            mready = true;

            yield return null;
        }


        private void ButtonClick()
        {
            ManagerObject.instance.sound.playSe(17);
//            Application.OpenURL("https://google.co.jp/");
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

