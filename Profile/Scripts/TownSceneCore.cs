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

namespace Mix2App.Profile.Town
{
    public class TownSceneCore : MonoBehaviour, IReceiver
    {
        [SerializeField, Required] TownProfileWindow TownProfileWindowPrefab = null;
        [SerializeField] GameEventHandler handler = null;

        [SerializeField] GameObject[] proposeWindow = null;
        [SerializeField] GameObject cameraObj = null;

        [SerializeField] GameObject baseObj = null;

        User muser;
        int mbgmid;
        bool mproposeflag;

        bool mTutorialFlag;
        int mTutorialStepID;


        private readonly string[] MessageTable001 = new string[]
        {
            "いいねするときは\n「いいね」ボタンを タップするぷり！",
        };

        private readonly string[] MessageTable002 = new string[]
        {
            "きになるナウたまの プロフィールがめんを \nひらいたとき 「プロポーズ」ボタンがあれば\nプロポーズできるぷり♥",
            "プロポーズできるのは・・・\nあなたのナウたまが 「たまごっちみーつ」から\nこのアプリに おでかけしてきていること・・・",
            "あなたと あいてのナウたまが\n<color=red>フレンドき</color> であること\nあと <color=red>いせい</color> のときぷり！",
            "「プロポーズ」ボタンがあれば\nけっこんできると おもえばいいぷり♪",
        };

        private readonly string[] MessageTable003 = new string[]
        {
            "きになるナウたまの プロフィールがめんを \nひらいたとき 「プロポーズ」ボタンがあれば\nプロポーズできるぷり♥",
            "プロポーズできるのは・・・\nあなたのナウたまが 「たまごっちみーつ」から\nこのアプリに おでかけしてきていること・・・",
            "あなたと あいてのナウたまが\n<color=red>フレンドき</color> であること\nあと <color=red>いせい</color> のときぷり！",
            "「プロポーズ」ボタンがあれば\nけっこんできると おもえばいいぷり♪",
            "さっそく「プロポーズ」ボタンを タップするぷり！",
        };




        void Awake()
        {
            mTutorialFlag = false;
            mTutorialStepID = 0;
        }

        void OnDestroy()
        {
            tpwindow.ProposeCallBackDel();

            UIFunction.TutorialDataAllClear();
        }


        /// <summary>
        /// Receive data from scene controller
        /// </summary>
        /// <param name="parameter">
        /// </param>
        public void receive(params object[] parameter)
        {
            Debug.Log("receive:" + parameter);

            object[] mparam;
            if (parameter == null) mparam = new object[] { };
            else mparam = parameter;

            if (mparam.Length >= 1 && mparam[0] is User)
                muser = (User)mparam[0];
            else
                muser = ManagerObject.instance.player;

            if (mparam.Length >= 2 && mparam[1] is int)
                cameraObj.transform.GetComponent<Camera>().depth = (int)mparam[1];
            else
                cameraObj.transform.GetComponent<Camera>().depth = 2;

            if (mparam.Length >= 3 && mparam[2] is bool)
                mproposeflag = (bool)mparam[2];
            else
                mproposeflag = false;

            if (mparam.Length >= 4 && mparam[3] is int)
                mbgmid = (int)mparam[3];
            else
                mbgmid = 2;//春テーマ

            if (mparam.Length >= 5 && mparam[4] is int)
            {
                mTutorialFlag = true;
                mTutorialStepID = (int)mparam[4];
                if (mTutorialStepID == 0)
                {
                    mTutorialFlag = false;
                }
            }

            StartCoroutine(mstart());
        }

        private TownProfileWindow tpwindow;

        IEnumerator mstart()
        {

            AvatarElementSelectWindow.InitData();

            yield return null;

            if (mTutorialFlag)
            {
                proposeWindow[1].transform.Find("Button_blue_tojiru").gameObject.SetActive(false);
                proposeWindow[0].transform.Find("Button_blue_iie").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                proposeWindow[0].transform.Find("Button_blue_iie").GetComponent<Button>().enabled = false;
            }


            UIManager.GEHandlerSet(handler);
            UIManager.proposeWindowSet(proposeWindow);
            UIManager.cameraWindowSet(cameraObj);
            UIManager.proposeFlagSet(mproposeflag);

            UIFunction.TutorialDataAllSet(mTutorialFlag, mTutorialStepID);

            tpwindow = UIManager.ShowModal(TownProfileWindowPrefab);
            tpwindow.ProposeCallBackAdd();
            tpwindow.TownBgmId = mbgmid;
            tpwindow.SetupUserData(muser)
                    .AddCloseAction(() =>
                    {
                        // Setup back action
                        // Mix2App.Lib.ManagerObject.instance.view.back();

                        handler.OnRemoveScene(SceneLabel.PROFILE_TOWN);
                    });

            UIManager.GEHandlerSet(handler);

            if (mTutorialFlag)
            {
                // チュートリアル中

                switch (mTutorialStepID)
                {
                    case 110:   // ゲストルートいいねの仕方
                    case 211:   // みーつルートいいねの仕方
                        {
                            baseObj.transform.Find("tutorial/Window_up/main").transform.localPosition = new Vector3(150.0f, -150.0f, 0.0f);

                            StartCoroutine(TutorialIine());

                            break;
                        }
                    default:    // 113,214 プロポーズ
                        {
                            baseObj.transform.Find("tutorial/Window_up/main").transform.localPosition = new Vector3(830.0f, 80.0f, 0.0f);

                            StartCoroutine(TutorialPropose());

                            break;
                        }
                }
            }

        }

        public static void SceneClose()
        {
            UIManager.GEHandlerGet().OnRemoveScene(SceneLabel.PROFILE_TOWN);
        }

        void Update()
        {
            if (mTutorialFlag)
            {
                if(UIFunction.TutorialCountGet() == UIFunction.TUTORIAL_COUNTER.TutorialClear)
                {
                    TutorialMessageWindowDisp(false);
                }
            }
        }

        /// <summary>
        /// チュートリアルいいね
        /// </summary>
        /// <returns></returns>
        private IEnumerator TutorialIine()
        {
            yield return new WaitForSeconds(0.5f);
            TutorialMessageDataSet(MessageTable001[0]);
            TutorialMessageWindowDisp(true);
            yield return new WaitForSeconds(0.5f);
            UIFunction.TutorialCountSet(UIFunction.TUTORIAL_COUNTER.IineButtonTrueStart);         // いいねボタンを有効化

            yield return null;
        }

        /// <summary>
        /// チュートリアルプロポーズ
        /// </summary>
        /// <returns></returns>
        private IEnumerator TutorialPropose()
        {
            yield return new WaitForSeconds(0.5f);
            if (mTutorialStepID == 113)
            {
                // ゲストルート
                for (int i = 0; i < 4; i++)
                {
                    TutorialMessageDataSet(MessageTable002[i]);
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
            }
            else
            {
                // 玩具連動ルート
                for (int i = 0; i < 4; i++)
                {
                    TutorialMessageDataSet(MessageTable003[i]);
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

                if (mproposeflag)
                {
                    TutorialMessageDataSet(MessageTable003[4]);
                    TutorialMessageWindowDisp(true);
                    yield return new WaitForSeconds(0.5f);
                    UIFunction.TutorialCountSet(UIFunction.TUTORIAL_COUNTER.ProposeButtonTrueStart);         // プロポーズボタンを有効化
                    while (true)
                    {
                        yield return null;
                        if (UIFunction.TutorialCountGet() == UIFunction.TUTORIAL_COUNTER.ProposeEnd)
                        {
                            break;
                        }
                    }
                }
            }

            SceneClose();
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