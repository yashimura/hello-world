////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mix2App.Lib;
using Mix2App.Lib.Events;
using Mix2App.UI;
using Mix2App.Lib.Model;

namespace Mix2App.Profile {
    /// <summary>
    /// Profile scene core
    /// </summary>
    public class SceneCore: MonoBehaviour, IReceiver {
        [SerializeField, Required] SelfProfileWindow SelfProfileWindowPrefab = null;

        [SerializeField] GameObject baseObj = null;

        object[] mparam;

        bool mTutorialFlag;
        int mTutorialRoute;
        int mTutorialStep;


        private readonly string[] MessageTable001 = new string[]
        {
            "あなたの ナウたまの いいねは\nここで かくにんできるぷり♪",
            "いままで あつめた\nいいねのかずは こっちぷり！",
            "つぎは いいねの しかたぷり\nみーつパークに もどるぷり！",
        };



        private void CloseAction() {
            ManagerObject.instance.view.back();
        }

        private User GetSampleUser() {
            return new TestUser(901, UserKind.ANOTHER, UserType.MIX2, 0, 16, 0, 1, 1);
        }

        void Awake()
        {
            mparam=null;
        }

        /// <summary>
        /// Receive data from scene controller
        /// </summary>
        /// <param name="parameter">
        /// </param>
        public void receive(params object[] parameter) {
            Debug.Log("receive");

            if (mparam != null)
                mparam = parameter;
            else
                mparam = new object[] { ManagerObject.instance.player };
        }

        void OnDestroy()
        {
            UIFunction.TutorialDataAllClear();
        }

        IEnumerator Start()
        {
            while (mparam==null)
                yield return null;

            if (mparam.Length >= 3 && mparam[1] is int && mparam[2] is int)
            {
                mTutorialFlag = true;
                mTutorialRoute = (int)mparam[1];
                mTutorialStep = (int)mparam[2];
            }
            else
            {
                mTutorialFlag = false;
                mTutorialRoute = 0;
                mTutorialStep = 0;
            }
            UIFunction.TutorialDataAllSet(mTutorialFlag, mTutorialRoute, mTutorialStep);

            AvatarElementSelectWindow.InitData();

            yield return null;

            User user = (User)mparam[0];
            if (user != null)
                UIManager.ShowModal(SelfProfileWindowPrefab)
                    .SetupUserData(user)
                    .AddCloseAction(CloseAction);
            else {
                int user_id = (int)mparam[0];
                GameCall call = new GameCall(CallLabel.GET_USER, user_id);
                call.AddListener((bool success, object data) => {
                    user = (User)data;
                    UIManager.ShowModal(SelfProfileWindowPrefab)
                            .SetupUserData(user)
                            .AddCloseAction(CloseAction);
                });
                ManagerObject.instance.connect.send(call);
            }

            if (mTutorialFlag)
            {
                StartCoroutine(TutorialIineSetumei());
            }

        }

        IEnumerator TutorialIineSetumei()
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 2; i++)
            {
                TutorialMessageDataSet(MessageTable001[i]);
                TutorialMessageWindowDisp(true);
                yield return new WaitForSeconds(0.1f);
                while (true)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        break;
                    }
                    yield return null;
                }
            }
            TutorialMessageDataSet(MessageTable001[2]);
            TutorialMessageWindowDisp(true);
            yield return new WaitForSeconds(0.1f);

            UIFunction.TutorialCountSet(UIFunction.TUTORIAL_COUNTER.ButtonTrueStart);         // 戻るボタンを有効化

            yield return null;
        }



        private void TutorialMessageWindowDisp(bool flag)
        {
            if (flag)
            {
                baseObj.transform.Find("tutorial").gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            }
            else
            {
                baseObj.transform.Find("tutorial").gameObject.transform.localPosition = new Vector3(5000.0f, 0.0f, 0.0f);
            }
        }

        private void TutorialMessageDataSet(string _mes)
        {
            baseObj.transform.Find("tutorial/Window_up/aplich_set/fukidasi/Text").GetComponent<Text>().text = _mes;
        }

    }
}
