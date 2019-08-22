using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using Mix2App.Lib;
using Mix2App.Lib.View;
using Mix2App.Lib.Model;

namespace Mix2App.Setting
{
    public class HelpView : AbstractWebViewer
    {
        [SerializeField] GameObject[] titles = null;
        // Use this for initialization
        void OnEnable()
        {
            foreach(GameObject title in titles)
            {
                title.SetActive(false);    
            }
            if (webViewObject!=null) webViewObject.SetVisibility(false);       
        }

        public void init(int hid)
        {
            titles[hid-1].SetActive(true);
            string url="";
            string parameter=null;
            bool webkitflag = false;
            switch (hid)
            {
                case 1://help
                url = ManagerObject.instance.serverUrl1 + "page/help.html";
                break;
                case 2://link
                url = ManagerObject.instance.serverUrl1 + "page/links.html";
                webkitflag=true;
                break;
                case 3://asks
                url = ManagerObject.instance.serverUrl1 + "page/contactus.html";
                parameter = "id=" + ManagerObject.instance.app.customerNo;

                break;
                case 4://kiyaku
                url = ManagerObject.instance.serverUrl1 + "page/terms.html";
                break;
                case 5://bnid link
                url = ManagerObject.instance.serverUrl2 + "api/bnidjoin.php";
                parameter = "login=" + ManagerObject.instance.app.loginCode;
                break;
                case 6://hikkoshi
                url = ManagerObject.instance.serverUrl2 + "api/bnidmov.php";
                parameter = "login=" + ManagerObject.instance.app.loginCode;
                break;
            }
            base.view(url,webkitflag,parameter);
        }

		protected override void setmargin()
		{
            //18:9でのwebrootのwidth,height,posY
            float vw = 1152f;//wakuは1180
			float vh = 662f;//wakuは690
			float vy = 25f;
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

        void OnDisable()
        {
            base.hide();
        }

    }
}