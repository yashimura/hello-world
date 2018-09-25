using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

public class Friends : MonoBehaviour,IReceiver {



	private object[]		mparam;
	private User muser1;//自分
	private FriendData		mFriendData;


	void Awake(){
		Debug.Log ("Friends Awake");
		mparam=null;
		muser1=null;
	}

	public void receive(params object[] parameter){
		Debug.Log ("Friends receive");
		mparam = parameter;
	}

	void Start() {
		Debug.Log ("Friends start");

		//ルーム情報取得、メンバーマッチング処理
		//パラメタは設計書参照
		GameCall call = new GameCall(CallLabel.GET_FRIEND_INFO);
		call.AddListener(mGetFriendInfo);
		ManagerObject.instance.connect.send(call);
	}

	void mGetFriendInfo(bool success,object data){
		mFriendData = (FriendData)data;
		StartCoroutine(mstart());
	}

	IEnumerator mstart(){
		//単体動作テスト用
		//パラメタ詳細は設計書参照
		if (mparam==null) {
			mparam = new object[] {
				ManagerObject.instance.player,
			};
		}
		muser1 = (User)mparam[0];		// たまごっち





		yield return null;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
