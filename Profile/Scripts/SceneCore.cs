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
using System.Collections.Generic;

namespace Mix2App.Profile {
    /// <summary>
    /// Profile scene core
    /// </summary>
    public class SceneCore: MonoBehaviour, IReceiver {
        [SerializeField, Required] SelfProfileWindow SelfProfileWindowPrefab = null;

        [SerializeField] GameObject baseObj = null;

        object[] mparam;

        bool mTutorialFlag;
        int mTutorialStepID;


        private readonly string[] MessageTable001 = new string[]
        {
            "あなたの ナウたまの いいねは\nここで かくにんできるぷり♪",
            "いままで あつめた\nいいねのかずは こっちぷり！",
            "つぎは いいねの しかたぷり\nみーつパークに もどるぷり！",
        };


        private Vector3[] YajirusiPosTable = new Vector3[]
        {
            new Vector3(-80.0f,-180.0f,0.0f),
            new Vector3(470.0f,-180.0f,0.0f),
            new Vector3(830.0f,-200.0f,0.0f),
        };



        private void CloseAction() {
            if (!mTutorialFlag)
            {
                ManagerObject.instance.view.back();
            }
            else
            {
                ManagerObject.instance.view.change(SceneLabel.SETTING, mTutorialStepID + 1);
            }
        }

        private User GetSampleUser() {
            return new TestUser(901, UserKind.ANOTHER, UserType.MIX2, 0, 16, 0, 1, 1);
        }

        void Awake()
        {
            mparam=null;
            mTutorialFlag = false;
            mTutorialStepID = 0;

            GameEventHandler.OnRemoveSceneEvent += AchieveClearDelete;
        }

        /// <summary>
        /// Receive data from scene controller
        /// </summary>
        /// <param name="parameter">
        /// </param>
        public void receive(params object[] parameter) {
            Debug.Log("receive");

            if (parameter != null)
                mparam = parameter;
            else
                mparam = new object[] { ManagerObject.instance.player };
        }

        void OnDestroy()
        {
            UIFunction.TutorialDataAllClear();

            GameEventHandler.OnRemoveSceneEvent -= AchieveClearDelete;
        }

        void AchieveClearDelete(string label)
        {
            if (label == SceneLabel.ACHIEVE_CLEAR)
            {
                ManagerObject.instance.view.delete(SceneLabel.ACHIEVE_CLEAR);
            }
        }

        IEnumerator Start()
        {
            while (mparam==null)
                yield return null;

            if (mparam.Length >= 2 && mparam[1] is int)
            {
                mTutorialFlag = true;
                mTutorialStepID = (int)mparam[1];
                if (mTutorialStepID == 0)
                {
                    mTutorialFlag = false;
                }
            }

            UIFunction.TutorialDataAllSet(mTutorialFlag, mTutorialStepID);

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

        void Update()
        {
        }


        IEnumerator TutorialIineSetumei()
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 2; i++)
            {
                TutorialMessageDataSet(MessageTable001[i]);
                baseObj.transform.Find("tutorial/Window_up/main").transform.localPosition = YajirusiPosTable[i];
                TutorialMessageWindowDisp(true);
                yield return new WaitForSeconds(0.5f);
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
            baseObj.transform.Find("tutorial/Window_up/main").transform.localPosition = YajirusiPosTable[2];
            TutorialMessageWindowDisp(true);
            yield return new WaitForSeconds(0.1f);

            UIFunction.TutorialCountSet(UIFunction.TUTORIAL_COUNTER.ProposeButtonTrueStart);         // 戻るボタンを有効化

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
