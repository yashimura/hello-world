using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.View;
using Mix2App.Lib.Events;

namespace Mix2App.Home
{
    public class HomeMain : MonoBehaviour, IReceiver, IReadyable
    {
        //[SerializeField] protected GameObject[] views;
        [SerializeField] protected GameObject[] meetsBtns=null;
        [SerializeField] protected GameEventHandler GEHandler = null;
        [SerializeField] protected GameObject CameraObj = null;
        [SerializeField] protected GameObject LineView = null;
        [SerializeField] protected GameObject BaseObj = null;


        private object[] mparam;
        private bool mready;
        private string mcnews;
        private string mcbbs;
        private string minfo;
        private string meinfo;
        private string mtoday;
        private int mstat;
        private HomeInfoData mhidata;

        private bool mTutorialFlag;
        private int mTutorialStepID;

        private readonly string[] MessageTable001 = new string[]
        {
            "「みーつパーク」を タップするぷり♪",
            "あなたの「たまごっちみーつ」の ナウたまを \nアプリにおでかけさせてみようぷり！\n「ナウたまをよぶ」ボタンを タップするぷり♪",
            "たまごっちみーつアプリのなかでもらった \nアイテムやごっちポイントは \n「プレゼント」のなかにあるぷり♪",
            "あそびかたは いつでもみることができるぷり！\nみたいときは 「あそびかた」ボタンを タップするぷり♪",
        };

        void Awake()
        {
            Debug.Log("Home.Awake");
            mready=false;
            //mlabel=null;
            //vid=0;
            //foreach (GameObject view in views)
            //{
            //view.SetActive(false);
            //}
            mstat = 0;
            GameEventHandler.OnRemoveSceneEvent += AchieveClearDelete;

            mTutorialFlag = false;
            mTutorialStepID = 0;
        }

        /// <summary>
        /// 画面遷移のパラメタ受け取り.
        /// 
        /// AwakeとStartの間でコールされます.
        /// 
        /// </summary>
        /// <see cref="IReceiver"/>
        public void receive(params object[] parameter)
        {
            Debug.Log("Home.receive");
            mparam = parameter;

            StartCoroutine(mstart());
        }

        public bool ready ()
        {
            return mready;
        }

        void OnDestroy()
        {
            GameEventHandler.OnRemoveSceneEvent -= AchieveClearDelete;
        }



        IEnumerator mstart()
        {
            Debug.Log("Home.Start");

/*
            if (mparam==null) {
                // 単体テストの時はreceiverが機能しないのでパラメタをでっちあげる
                mparam = new object[]{1};
            }
*/

            if(mparam != null)
            {
                if (mparam.Length >= 1 && mparam[0] is int)
                {
                    mTutorialFlag = true;
                    mTutorialStepID = (int)mparam[0];
                    if (mTutorialStepID == 0)
                    {
                        mTutorialFlag = false;
                    }
                }
            }

            mstat = 0;

            HomeInfoData? qdata=null;
            GameCall call = new GameCall(
                CallLabel.GET_HOME_INFO
            );
            call.AddListener((success, data) => 
                { 
                    qdata = (HomeInfoData)data;
                }
            );
            ManagerObject.instance.connect.send(call);

            while(qdata==null) yield return null;

            mhidata = (HomeInfoData)qdata;

            minfo = mhidata.infoHtml;
            meinfo = mhidata.eventInfoHtml;
            bool eflag = mhidata.eventflag;

            DateTime dt = System.DateTime.Now;
            mtoday = meinfo+"_"+dt.ToString("yyMMdd");

            //ローカル保存データから閲覧日を取得
            mcnews = ManagerObject.instance.app.checkedNews;
            mcbbs = ManagerObject.instance.app.checkedBbs;

            ManagerObject.PutLog("check info:"+mcnews+"="+minfo);
            ManagerObject.PutLog("check einfo:"+mcbbs+"="+mtoday);

            //掲示板強制表示はイベント期間中のみ
            if (!eflag) mtoday = mcbbs;

            //MIXユーザーの場合はタイマーチェック
           if (ManagerObject.instance.player.utype == UserType.MIX)
            {
                call = new GameCall(CallLabel.CHECK_MIX_TIMER);
                yield return ManagerObject.instance.connect.send(call);
            }

            //　ユーザータイプでみーつボタンの表示切替
            if (ManagerObject.instance.player.utype == UserType.MIX2)
            {
                meetsBtns[0].SetActive(false);
                meetsBtns[1].SetActive(true);
            }
            else
            {
                meetsBtns[0].SetActive(true);
                meetsBtns[1].SetActive(false);
            }

/*
            if (mhidata.tutorialFlag == 0)
            {
                // チュートリアル未閲覧なのでチュートリアル強制開催
                mTutorialFlag = true;
            }
*/

            //ログインからのホームの時は、強制表示チェック
            if (ManagerObject.instance.view.GetBackLabel()==null)
            {
                if (ManagerObject.instance.player.utype == UserType.LINE)
                {
                    mstat = 300;
                }
                else
                {
                    mstat = 100;
                }
            }
            else
            {
                if (mTutorialFlag)
                {
                    mstat = 400;
                }
            }

            ManagerObject.instance.sound.playBgm(1);
            mready = true;
        }

        private void closehelp(int result)
        {
            Debug.Log("closehelp:"+result);
            if (result>0) ManagerObject.instance.app.checkedHelp |= 1;
            else ManagerObject.instance.app.checkedHelp &= ~1;

            Debug.Log("save helpf:"+ManagerObject.instance.app.checkedHelp);
            PlayerPrefs.SetInt("CheckedHelp",ManagerObject.instance.app.checkedHelp);
            PlayerPrefs.Save();

            if (mstat == 101) mstat++;
        }

        private void closebbs(int result)
        {
            Debug.Log("save bbsf:"+mtoday);
            mcbbs = mtoday;
            ManagerObject.instance.app.checkedBbs = mtoday;
            PlayerPrefs.SetString("CheckedBbs",mtoday);
            PlayerPrefs.Save();

            if (mstat == 121) mstat++;
        }

        private void closenews(int result)
        {
            Debug.Log("save newsf:"+minfo);
            mcnews = minfo;
            ManagerObject.instance.app.checkedNews = minfo;
            PlayerPrefs.SetString("CheckedNews",minfo);
            PlayerPrefs.Save();

            if (mstat == 111) mstat++;
        }

        public void clickbutton(string label)
        {
            ManagerObject.instance.sound.playSe(11);       
            if(!mready) return;                

            if (label=="Setting")
	    		ManagerObject.instance.view.change(label,true);
            else if (label == "Town" && ManagerObject.instance.player.chara1.IsTamgo && mTutorialFlag)
                ManagerObject.instance.view.change("GestGate",102);
            else if (label == "Town" && ManagerObject.instance.player.chara1.IsTamgo && !mTutorialFlag)
                ManagerObject.instance.view.change("GestGate");
            else if (label=="add_news")
	    		ManagerObject.instance.view.dialog("webview",new object[]{"news",minfo},closenews);
            else if (label=="add_bbs")
	    		ManagerObject.instance.view.dialog("webview",new object[]{"bbs",meinfo},closebbs);
            else if (label == "Town" && mTutorialFlag)
                ManagerObject.instance.view.change(label, 204);
            else if (label == "ToyLink" && mTutorialFlag)
                ManagerObject.instance.view.change(label, 203);
            else
                ManagerObject.instance.view.change(label);
		}


        void AchieveClearDelete(string label)
        {
            if (label == SceneLabel.ACHIEVE_CLEAR)
            {
                ManagerObject.instance.view.delete(SceneLabel.ACHIEVE_CLEAR);
            }
        }

        public void ButtonCloseClick()
        {
            ManagerObject.instance.sound.playSe(17);
            LineView.SetActive(false);

            if (mstat == 301) mstat++;
        }

        public void Update()
        {
            switch(mstat)
            {
                case 100:
                    if ((ManagerObject.instance.app.checkedHelp & 1) != 1)
                    {
                        mstat++;
                        ManagerObject.instance.view.dialog("webview", new object[] { "home" }, closehelp);
                    }
                    else
                    {
                        mstat+=2;
                    }
                    break;
                case 102:                    
                    mstat = 110;
                    break;

                case 110:
                    if (mcnews != minfo)
                    {
                        mstat++;
                        ManagerObject.instance.view.dialog("webview", new object[] { "news", minfo }, closenews);
                    }
                    else
                    {
                        mstat+=2;
                    }
                    break;
                case 112:
                    mstat = 120;
                    break;

                case 120:
                    if (mcbbs != mtoday)
                    {
                        mstat++;
                        ManagerObject.instance.view.dialog("webview", new object[] { "bbs", meinfo }, closebbs);
                    }
                    else
                    {
                        mstat+=2;
                    }
                    break;
                case 122:
                    mstat = 200;
                    break;

                case 200:
                    mstat++;
                    //達成アチーブがある場合は、アチーブ成功画面を呼び出す
                    if (mhidata.achieves != null && mhidata.achieves.Count != 0)
                    {
                        int CameraDepth = (int)(CameraObj.transform.GetComponent<Camera>().depth + 1);
                        ManagerObject.instance.view.add(SceneLabel.ACHIEVE_CLEAR,
                                mhidata.achieves,
                                CameraDepth);
                    }
                    break;

                case 201:
                    mstat = 400;
                    break;

                case 300:
                    mstat++;
                    LineView.SetActive(true);
                    //LineView.transform.Find("Button_blue_close").gameObject.GetComponent<Button>().onClick.AddListener(ButtonCloseClick);
                    break;
                case 302:
                    mstat = 100;
                    break;

                case 400:
                    mstat++;
                    if (mTutorialFlag)
                    {
                        if (mTutorialStepID != 222)
                        {
                            StartCoroutine(TutorialLoopStart());
                        }
                        else
                        {
                            StartCoroutine(TutorialMainLoop());
                        }
                    }
                    break;



                default:
                    break;
            }
        }


        public void TutorialStartButtonClick()
        {
            ManagerObject.instance.sound.playSe(13);

            StartCoroutine(TutorialLoopStart());
        }
        public void TutorialYesButtonClick()
        {
            ManagerObject.instance.sound.playSe(13);
            tutorialCounter = 1;
        }
        public void TutorialNoButtonClick()
        {
            ManagerObject.instance.sound.playSe(14);
            tutorialCounter = 2;
        }
        public void TutorialMIX2ButtonClick()
        {
            ManagerObject.instance.sound.playSe(13);
            tutorialCounter = 1;
        }
        public void TutorialGesutButtonClick()
        {
            ManagerObject.instance.sound.playSe(13);
            tutorialCounter = 2;
        }

        private int tutorialCounter;
        IEnumerator TutorialLoopStart()
        {
            if(ManagerObject.instance.player.utype == UserType.MIX2)
            {
                // 玩具連動ずみなので確認などをしないでタウンへ誘導
                mTutorialFlag = true;
                mTutorialStepID = 202;
                StartCoroutine(TutorialMainLoop());
            }
            else
            {
                tutorialCounter = 0;
                BaseObj.transform.Find("startcheck_1").gameObject.SetActive(true);
                while (true)
                {
                    if (tutorialCounter != 0)
                    {
                        break;
                    }
                    yield return null;
                }

                BaseObj.transform.Find("startcheck_1").gameObject.SetActive(false);

                if (tutorialCounter == 1)
                {
                    tutorialCounter = 0;
                    BaseObj.transform.Find("startcheck_2").gameObject.SetActive(true);
                    while (true)
                    {
                        if (tutorialCounter != 0)
                        {
                            break;
                        }
                        yield return null;
                    }

                    BaseObj.transform.Find("startcheck_2").gameObject.SetActive(false);

                    if (tutorialCounter == 1)
                    {
                        // 玩具連動していないので玩具連動へ誘導
                        mTutorialStepID = 201;
                    }
                    else
                    {
                        // ゲスト状態なのでタウンへ誘導
                        mTutorialStepID = 101;
                    }
                    mTutorialFlag = true;
                    StartCoroutine(TutorialMainLoop());
                }
            }

            yield return null;
        }

        IEnumerator TutorialMainLoop()
        {
            ButtonClickValid(false);
            BaseObj.transform.Find("tutorial").gameObject.SetActive(true);
            BaseObj.transform.Find("tutorial/Window_up").gameObject.SetActive(false);
            BaseObj.transform.Find("tutorial/Window_down").gameObject.SetActive(false);

            switch (mTutorialStepID)
            {
                case 101:
                case 202:
                    {
                        BaseObj.transform.Find("tutorial/Window_down").gameObject.SetActive(true);
                        BaseObj.transform.Find("tutorial/Window_down/aplich_set/fukidasi/Text").GetComponent<Text>().text = MessageTable001[0];
                        BaseObj.transform.Find("main/panel1/main1_btn").GetComponent<Button>().enabled = true;

                        Vector3 _pos = BaseObj.transform.Find("main/panel1/main1_btn").position;
                        _pos.y = _pos.y + 5.3f;
                        BaseObj.transform.Find("tutorial/Window_down/main").position = _pos;
                        break;
                    }
                case 201:
                    {
                        BaseObj.transform.Find("tutorial/Window_down").gameObject.SetActive(true);
                        BaseObj.transform.Find("tutorial/Window_down/aplich_set/fukidasi/Text").GetComponent<Text>().text = MessageTable001[1];
                        BaseObj.transform.Find("main/panel2/menu_bg/menu1_btn").GetComponent<Button>().enabled = true;     // ナウたまをよぶ
                        BaseObj.transform.Find("main/panel2/menu_bg/menu1_2_btn").GetComponent<Button>().enabled = true;   // みーつにかえる

                        Vector3 _pos = BaseObj.transform.Find("main/panel2/menu_bg/menu1_btn").position;
                        _pos.y = _pos.y + 3.3f;
                        BaseObj.transform.Find("tutorial/Window_down/main").position = _pos;
                        break;
                    }
                default:        // 119,222
                    {
                        Vector3 _pos;

                        BaseObj.transform.Find("tutorial/Window_up").gameObject.SetActive(true);
                        _pos = BaseObj.transform.Find("main/panel1/main5_btn").position;
                        _pos.y = _pos.y + 4.3f;
                        BaseObj.transform.Find("tutorial/Window_up/main").position = _pos;
                        BaseObj.transform.Find("tutorial/Window_up/aplich_set/fukidasi/Text").GetComponent<Text>().text = MessageTable001[2];
                        yield return new WaitForSeconds(0.1f);

                        while (true)
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                break;
                            }
                            yield return null;
                        }

                        _pos = BaseObj.transform.Find("main/panel2/tutorial_btn").position;
                        _pos.y = _pos.y + 3.8f;
                        BaseObj.transform.Find("tutorial/Window_up/main").position = _pos;
                        BaseObj.transform.Find("tutorial/Window_up/aplich_set/fukidasi/Text").GetComponent<Text>().text = MessageTable001[3];
                        yield return new WaitForSeconds(0.1f);

                        while (true)
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                break;
                            }
                            yield return null;
                        }

                        ButtonClickValid(true);
                        BaseObj.transform.Find("tutorial").gameObject.SetActive(false);
                        mTutorialFlag = false;
                        mTutorialStepID = 0;



                        // チュートリアル閲覧済フラグを立てる
                        // mTutorialStepID が 119 の時はゲストルート閲覧済にする
                        // mTutorialStepID が 222 の時はみーつルート閲覧済にする




                        ManagerObject.instance.view.dialog("webview", new object[] { "home" }, null);

                        break;
                    }
            }


            yield return null;
        }

        private void ButtonClickValid(bool _flag)
        {
            BaseObj.transform.Find("main/panel1/main1_btn").GetComponent<Button>().enabled = _flag;             // みーつパーク
            BaseObj.transform.Find("main/panel1/main2_btn").GetComponent<Button>().enabled = _flag;             // パパママモード
            BaseObj.transform.Find("main/panel1/main3_btn").GetComponent<Button>().enabled = _flag;             // チャレンジ
            BaseObj.transform.Find("main/panel1/main4_btn").GetComponent<Button>().enabled = _flag;             // ともだち
            BaseObj.transform.Find("main/panel1/main5_btn").GetComponent<Button>().enabled = _flag;             // プレゼント

            BaseObj.transform.Find("main/panel2/setting_btn").GetComponent<Button>().enabled = _flag;           // せってい
            BaseObj.transform.Find("main/panel2/tutorial_btn").GetComponent<Button>().enabled = _flag;          // あそびかた
            BaseObj.transform.Find("main/panel2/menu_bg/menu1_btn").GetComponent<Button>().enabled = _flag;     // ナウたまをよぶ
            BaseObj.transform.Find("main/panel2/menu_bg/menu1_2_btn").GetComponent<Button>().enabled = _flag;   // みーつにかえる
            BaseObj.transform.Find("main/panel2/menu_bg/menu2_btn").GetComponent<Button>().enabled = _flag;     // おしらせ
            BaseObj.transform.Find("main/panel2/menu_bg/menu3_btn").GetComponent<Button>().enabled = _flag;     // けいじばん

        }


    }
}