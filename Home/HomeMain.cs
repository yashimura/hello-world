using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        private object[] mparam;
        private bool mready;
        private string mcnews;
        private string mcbbs;
        private string minfo;
        private string meinfo;
        private string mtoday;
        private int mstat;
        private HomeInfoData mhidata;

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

            if (mparam==null) {
                // 単体テストの時はreceiverが機能しないのでパラメタをでっちあげる
                mparam = new object[]{1};
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
            else if (label=="Town"&& ManagerObject.instance.player.chara1.IsTamgo)
	    		ManagerObject.instance.view.change("GestGate");
            else if (label=="add_news")
	    		ManagerObject.instance.view.dialog("webview",new object[]{"news",minfo},closenews);
            else if (label=="add_bbs")
	    		ManagerObject.instance.view.dialog("webview",new object[]{"bbs",meinfo},closebbs);
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

                case 300:
                    mstat++;
                    LineView.SetActive(true);
                    //LineView.transform.Find("Button_blue_close").gameObject.GetComponent<Button>().onClick.AddListener(ButtonCloseClick);
                    break;
                case 302:
                    mstat = 100;
                    break;

                default:
                    break;
            }
        }

    }
}