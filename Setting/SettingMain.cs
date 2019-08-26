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

namespace Mix2App.Setting
{
    public class SettingMain : MonoBehaviour, IReceiver, IWebViewEventHandler
    {
		public GameEventHandler handler;

        private object[] mparam;
        private int state;
        private int vid;
        [SerializeField] GameObject[] views = null;
        [SerializeField] GameObject panel2 = null;

        [SerializeField] GameObject baseObj = null;

        private bool mTutorialFlag;
        private int mTutorialStepID;

        private readonly string[] MessageTable001 = new string[]
        {
            "「プロフィール」ボタンを タップするぷり",
            "つぎは いいねの しかたぷり\nみーつパークに もどるぷり！",
        };


        void Awake()
        {
            state=0;
            vid=0;
            foreach (GameObject view in views)
            {
                view.SetActive(false);
            }

            mTutorialFlag = false;
            mTutorialStepID = 0;

        }
        
        public void receive(params object[] parameter)
        {
            mparam = parameter;
        }

        void Start()
        {
            if (mparam==null) {
                mparam = new object[]{true};
            }
			
            if (mparam.Length>= 1 && mparam[0] is bool)
            {
                bool allflag = (bool)mparam[0];
                panel2.SetActive(allflag);
                if(mparam.Length >= 2 && mparam[1] is int)
                {
                    mTutorialFlag = true;
                    mTutorialStepID = (int)mparam[1];
                }
            }
            else
            {
                panel2.SetActive(false);
                if (mparam.Length >= 1 && mparam[0] is int)
                {
                    mTutorialFlag = true;
                    mTutorialStepID = (int)mparam[0];
                }
            }

            if(mTutorialStepID == 0)
            {
                mTutorialFlag = false;
            }

            changeview(1);

            if (mTutorialFlag)
            {
                StartCoroutine(TutorialMain());
            }
        }

        public void clickClose()
        {
            ManagerObject.instance.sound.playSe(17);
            ManagerObject.instance.view.back();
        }

        public void OnClickBack()
        {
            ManagerObject.instance.sound.playSe(17);
            removebnidview();
        }

        public void clickWebBack()
        {
            ManagerObject.instance.sound.playSe(17);
            if (vid==2) removehelpview();
            else if (vid==3) removebnidview();
        }

        public void clickBack()
        {
            ManagerObject.instance.sound.playSe(17);
            if(state<=1) return;

            switch (state)
            {
                case 2:
                changeview(1);
                break;
            }
        }

        public void clickbutton(string label)
        {
            ManagerObject.instance.sound.playSe(11);      
            if(state==0||vid>0) return;

			switch (label)
            {
                case "profile":
                    if (!mTutorialFlag)
                    {
                        GameCall call = new GameCall(CallLabel.MOVE_PROFILE);
                        ManagerObject.instance.connect.send(call);
                    }
                    else
                    {
                        ManagerObject.instance.view.change(SceneLabel.PROFILE, ManagerObject.instance.player, mTutorialStepID + 1);
                    }
                    break;
                case "sound":
				changeview(2);
                break;
                case "help":
				addhelpview(1);
                break;
                case "links":
				addhelpview(2);
                break;
                case "contacts":
				addhelpview(3);
                break;
                case "terms":
				addhelpview(4);
                break;
                case "bnid":
				StartCoroutine(addbnidview(1));
                break;
                case "mvapp":
				StartCoroutine(addbnidview(2));
                break;
                default:
                changeview(1);
                break;
            }
		}

        IEnumerator addbnidview(int hid)
        {
            if (vid>0) yield break;

            string pass="";
            GameCall call = new GameCall(CallLabel.GET_BNID_PASS);
            call.AddListener((success,data) => {
                pass = (string)data;
            });
            yield return ManagerObject.instance.connect.send(call);

            vid=3;
            views[3].SetActive(true);
            views[3].GetComponent<BnIdView>().init(this.gameObject,hid,pass);
        }

        void removebnidview()
        {
            vid=0;
            views[3].SetActive(false);
        }

        void addhelpview(int hid)
        {
            vid=2;
            views[2].SetActive(true);
            views[2].GetComponent<HelpView>().init(hid);
        }

        void removehelpview()
        {
            vid=0;
            views[2].SetActive(false);
        }

        void changeview(int vid,params object[] options)
        {
            state=vid;
            foreach (GameObject view in views)
            {
                view.SetActive(false);
            }
            views[vid-1].SetActive(true);
        }

        void addview(int vid)
        {
            views[vid-1].SetActive(true);
            views[vid-1].transform.SetAsLastSibling();
        }

        IEnumerator TutorialMain()
        {
            baseObj.transform.Find("1_window_setting/LayoutPanel1/LayoutPanel2/bt_plof").GetComponent<Button>().enabled = false;
            baseObj.transform.Find("1_window_setting/LayoutPanel1/LayoutPanel2/bt_sound").GetComponent<Button>().enabled = false;
            baseObj.transform.Find("1_window_setting/LayoutPanel1/LayoutPanel2/bt_help").GetComponent<Button>().enabled = false;
            baseObj.transform.Find("1_window_setting/LayoutPanel1/LayoutPanel2/bt_toiawase").GetComponent<Button>().enabled = false;

            baseObj.transform.Find("1_window_setting/LayoutPanel1/LayoutPanel3/bt_data").GetComponent<Button>().enabled = false;
            baseObj.transform.Find("1_window_setting/LayoutPanel1/LayoutPanel3/bt_ID").GetComponent<Button>().enabled = false;
            baseObj.transform.Find("1_window_setting/LayoutPanel1/LayoutPanel3/bt_line").GetComponent<Button>().enabled = false;
            baseObj.transform.Find("1_window_setting/LayoutPanel1/LayoutPanel3/bt_link").GetComponent<Button>().enabled = false;
            baseObj.transform.Find("1_window_setting/LayoutPanel1/LayoutPanel3/bt_kiyaku").GetComponent<Button>().enabled = false;

            baseObj.transform.Find("1_window_setting/bt_close").GetComponent<Button>().enabled = false;

            if((mTutorialStepID == 106) || (mTutorialStepID == 207))
            {
                baseObj.transform.Find("1_window_setting/LayoutPanel1/LayoutPanel2/bt_plof").GetComponent<Button>().enabled = true;
                baseObj.transform.Find("tutorial").gameObject.SetActive(true);
                baseObj.transform.Find("tutorial/Window_up").gameObject.SetActive(false);
                baseObj.transform.Find("tutorial/Window_down").gameObject.SetActive(true);
                baseObj.transform.Find("tutorial/Window_down/aplich_set/fukidasi/Text").GetComponent<Text>().text = MessageTable001[0];
            }
            else
            {
                baseObj.transform.Find("1_window_setting/bt_close").GetComponent<Button>().enabled = true;
                baseObj.transform.Find("tutorial").gameObject.SetActive(true);
                baseObj.transform.Find("tutorial/Window_up").gameObject.SetActive(true);
                baseObj.transform.Find("tutorial/Window_down").gameObject.SetActive(false);
                baseObj.transform.Find("tutorial/Window_up/aplich_set/fukidasi/Text").GetComponent<Text>().text = MessageTable001[1];
            }


            yield return null;
        }


    }
}