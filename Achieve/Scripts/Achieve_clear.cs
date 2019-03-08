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
    public class Achieve_clear : MonoBehaviour, IReceiver, IReadyable
    {
        [SerializeField] private GameEventHandler GEHandler = null;
        [SerializeField] private GameObject CameraObj = null;
        [SerializeField] private GameObject EventRoot = null;


        private AchieveData mData;
        private object[] mparam;                                            // 他のシーンからくるパラメータ


        void Awake()
        {
            mparam = null;
        }

        public void receive(params object[] parameter)
        {
            mparam = parameter;

            if (mparam == null)
            {
                mparam = new object[] {
                    new AchieveData(),
                    4,                                                          // int  カメラDepth値
                };

            }
            mData = (AchieveData)mparam[0];

            CameraObj.transform.GetComponent<Camera>().depth = (int)mparam[1];

            StartCoroutine(mStart());
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
            RewardBehaviour _rb = EventRoot.transform.Find("clear_dialog/RewardView").gameObject.GetComponent<RewardBehaviour>();
            _rb.init(mData.reward);                                             // 報酬セット

            EventRoot.transform.Find("clear_dialog/Button_blue_tojiru").gameObject.GetComponent<Button>().onClick.AddListener(ButtonTojiruClick);

            mready = true;

            yield return null;
        }

        void Update()
        {
        }

        private void ButtonTojiruClick()
        {
            ManagerObject.instance.sound.playSe(17);
            GEHandler.OnRemoveScene(SceneLabel.ACHIEVE_CLEAR);
        }



    }
}
