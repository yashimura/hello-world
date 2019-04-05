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


        private List<AchieveData> mData;
        private object[] mparam;                                            // 他のシーンからくるパラメータ
        private bool loopFlag = true;
        private bool endFlag = false;



        void Awake()
        {
            mparam = null;

            loopFlag = true;
            endFlag = false;
        }

        public void receive(params object[] parameter)
        {
            mparam = parameter;

            if (mparam == null)
            {
                mparam = new object[] {
                    new List<AchieveData>(),
                    4,                                                          // int  カメラDepth値
                };


            }
            mData = (List<AchieveData>)mparam[0];

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
            EventRoot.transform.Find("clear_dialog/base/Button_blue_tojiru").gameObject.GetComponent<Button>().onClick.AddListener(ButtonTojiruClick);

            DispOff();

            yield return null;

            mready = true;

            loopFlag = true;

            for(int i = 0;i < mData.Count; i++)
            {
                RewardBehaviour _rb = EventRoot.transform.Find("clear_dialog/base/RewardView").gameObject.GetComponent<RewardBehaviour>();
                _rb.init(mData[i].reward);                                             // 報酬セット

                DispOn();

                while (loopFlag)
                {
                    yield return null;
                }
                DispOff();
                loopFlag = true;
                yield return new WaitForSeconds(0.1f);
            }

            endFlag = true;
            yield return null;
        }

        void Update()
        {
            if (endFlag)
            {
                GEHandler.OnRemoveScene(SceneLabel.ACHIEVE_CLEAR);
                endFlag = false;
            }
        }

        private void ButtonTojiruClick()
        {
            ManagerObject.instance.sound.playSe(17);
            loopFlag = false;
        }


        private void DispOff()
        {
            EventRoot.transform.Find("clear_dialog/base").gameObject.transform.localPosition = new Vector3(0, 5000, 0);
        }

        private void DispOn()
        {
            EventRoot.transform.Find("clear_dialog/base").gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }


    }
}
