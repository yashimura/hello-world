using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mix2App.Lib;
using Mix2App.Lib.View;

namespace Mix2App.MiniGame1{
	public class HelpWebView : MonoBehaviour {
		public string Url;
		WebViewObject webViewObject;

		// Use this for initialization
		void Start () {
		}

		void OnEnable(){
#if !UNITY_EDITOR
			webViewObject = (new GameObject ("WebViewObject")).AddComponent<WebViewObject> ();
			webViewObject.Init ();

			float vw = 1152f;
			float vh = 662f;
			float kx = Screen.width / 2048f;
			float ky = Screen.height / 1024f;
			float mX = (Screen.width - (vw * ky)) / 2f ;
			float mY = (Screen.height - (vh * ky)) / 2f ;
			float oY = mY / 8f;

			webViewObject.SetMargins((int)mX,(int)(mY-oY),(int)mX,(int)(mY+oY));
			webViewObject.LoadURL(Url);
			webViewObject.SetVisibility(true);
#endif
		}

		void OnDisable(){
#if !UNITY_EDITOR
			webViewObject.SetVisibility (false);
			Destroy(webViewObject);
#endif
		}

		// Update is called once per frame
		void Update () {
		
		}
	}
}
