using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;

using Mix2App.Lib;
using Mix2App.Lib.View;
using Mix2App.Lib.Events;
using Mix2App.Lib.Model;

namespace Mix2App.Setting
{
    public class BnIdView : AbstractWebViewer
    {
        GameObject mTarget;
        [SerializeField] GameObject BackButton = null;
        [SerializeField] GameObject[] titles = null;
        // Use this for initialization
        void OnEnable()
        {
            foreach(GameObject title in titles)
            {
                title.SetActive(false);    
            }
            if (webViewObject!=null) webViewObject.SetVisibility(false);  
#if !UNITY_EDITOR  
            BackButton.SetActive(false);   
#endif
        }

        public void init(GameObject eventTarget,int hid,string pass)
        {
            titles[hid-1].SetActive(true);
            string url="";
            string parameter=null;
            mTarget = eventTarget;
            switch (hid)
            {
                case 1://bnid link
                url = ManagerObject.instance.serverUrl2 + "api/bnidjoin.php";
                parameter = "login=" + pass;
                break;
                case 2://hikkoshi
                url = ManagerObject.instance.serverUrl2 + "api/bnidmov.php";
                parameter = "login=" + pass;
                break;
            }
            base.view(url,false,parameter);
        }

		protected override void mload_comp(string message)
		{
            Debug.Log("mload_comp:"+message);
			webViewObject.SetVisibility(true);
            if (message.IndexOf("bandainamcoid.com")>=0)
                BackButton.SetActive(true);
            else 
                BackButton.SetActive(false);
		}

        protected override void mload_start(string message)
		{
			BackButton.SetActive(false);
		}

		protected override void setmargin()
		{
            //18:9でのwebrootのwidth,height,posY
            float vw = 1216f;
			float vh = 766f;
			float vy = 70f;
            //canvas scalerのreference resolutionと実画面サイズで比率を求める
			//float kx = Screen.width / 2048f;
			float ky = Screen.height / 1024f;
            //実画面サイズとwebviewrootサイズから差分を計算して上下左右のマージンを求める
			float mX = (Screen.width - (vw * ky)) / 2f ;
			float mY = (Screen.height - (vh * ky)) / 2f ;
            //offset値も比率計算
			float oY = (vy * ky);
            //マージンを設定
			webViewObject.SetMargins((int)mX,(int)(mY-oY),(int)mX,(int)(mY+oY));
		}

        protected override void getmessage(string message)
        {
            if (message == "returnhome")
                ManagerObject.instance.view.change(SceneLabel.HOME);
            else if (message == "returntitle")
                ManagerObject.instance.view.reboot();
            else if (message.IndexOf("http")==0||message.IndexOf("mailto")==0)
				Application.OpenURL(message);
            else if (message == "returnsetting")
            {
                ExecuteEvents.Execute<IWebViewEventHandler>(
                    target: mTarget,
                    eventData: null,
                    functor: (reciever,eventData)=>reciever.OnClickBack()
                );
            }
        }

        void OnDisable()
        {
            base.hide();
        }

    }
}