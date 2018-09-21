using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

public class ItemBox : MonoBehaviour,IReceiver {

	[SerializeField] private Camera mainCamera;
	[SerializeField] private GameObject EventItemBoxMenu;			// プレゼントボックスメイン画面
	[SerializeField] private GameObject EventPresentMIX2;			// プレゼントボックスアイテム一覧画面MIX2用
	[SerializeField] private GameObject EventPresent;				// プレゼントボックスアイテム一覧画面
	[SerializeField] private GameObject EevntTushinOK;				// 通信成功画面
	[SerializeField] private GameObject EventTushinNot;				// 通信失敗画面
	[SerializeField] private GameObject EventPresentPoint;			// プレゼントボックスポイント表示画面
	[SerializeField] private GameObject EventTushinPoint;			// プレゼントボックスポイント転送量決定画面
	[SerializeField] private GameObject EventDialogKakunin;			// 確認画面
	[SerializeField] private GameObject EventDialogError;			// エラー画面

	[SerializeField] private Button ButtonPresent;					// メイン画面 アイテム一覧画面へのボタン
	[SerializeField] private Button ButtonPoint;					// メイン画面 ポイント表示画面へのボタン
	[SerializeField] private Button ButtonTojiru;					// メイン画面 とじるボタン

	[SerializeField] private Button ButtonPresentBackMIX2;			// アイテム一覧画面MIX2用 もどるボタン
	[SerializeField] private Button ButtonPresentBack;				// アイテム一覧画面　もどるボタン

	[SerializeField] private Button ButtonBackOK;					// 通信成功画面 もどるボタン
	[SerializeField] private Button ButtonBackNot;					// 通信失敗画面 もどるボタン

	[SerializeField] private Button ButtonPointSend;				// ポイント表示画面　おくるボタン
	[SerializeField] private Button ButtonPointBackMIX2;			// ポイント表示画面　MIX2用もどるボタン
	[SerializeField] private Button ButtonPointBack;				// ポイント表示画面　もどるボタン

	[SerializeField] private Button ButtonTushinBack;				// ポイント転送量決定画面　もどるボタン
	[SerializeField] private Button ButtonTushinSend;				// ポイント転送量決定画面　おくるボタン
	[SerializeField] private Button ButtonTushinSendAll;			// ポイント転送量決定画面　ぜんぶおくるボタン
	[SerializeField] private Button[] ButtonTushinUp;				// ポイント転送量決定画面　上ボタン（全４つ）
	[SerializeField] private Button[] ButtonTushinDown;				// ポイント転送量決定画面　下ボタン（全４つ）

	[SerializeField] private Button ButtonKakuninYes;				// 確認画面　はいボタン
	[SerializeField] private Button ButtonKakuninNo;				// 確認画面　いいえボタン
	[SerializeField] private Button ButtonErrorTojiru;				// エラー画面　とじるボタン

	[SerializeField] private Sprite[] SuujiBlack;					// 
	[SerializeField] private Sprite[] SuujiRed;						// 

	[SerializeField] private Sprite[] PresentImage;					//


	private object[]		mparam;
	private User muser1;//自分

	void Awake(){
		Debug.Log ("ItemBox Awake");
		mparam=null;
		muser1=null;
	}

	public void receive(params object[] parameter){
		Debug.Log ("ItemBox receive");
		mparam = parameter;
	}

	IEnumerator Start(){
		Debug.Log ("ItemBox Start");

		ButtonPresent.onClick.AddListener (ButtonPresentClick);
		ButtonPoint.onClick.AddListener (ButtonPointClick);
		ButtonTojiru.onClick.AddListener (ButtonTojiruClick);
		ButtonPresentBackMIX2.onClick.AddListener (ButtonPresentBackMIX2Click);
		ButtonPresentBack.onClick.AddListener (ButtonPresentBackClick);
		ButtonBackOK.onClick.AddListener (ButtonBackOKClick);
		ButtonBackNot.onClick.AddListener (ButtonBackNotClick);
		ButtonPointSend.onClick.AddListener (ButtonPointSendClick);
		ButtonPointBackMIX2.onClick.AddListener (ButtonPointBackMIX2Click);
		ButtonPointBack.onClick.AddListener (ButtonPointBackClick);
		ButtonTushinBack.onClick.AddListener (ButtonTushinBackClick);
		ButtonTushinSend.onClick.AddListener (ButtonTushinSendClick);
		ButtonTushinSendAll.onClick.AddListener (ButtonTushinSendAllClick);
		ButtonKakuninYes.onClick.AddListener (ButtonKakuninYesClick);
		ButtonKakuninNo.onClick.AddListener (ButtonKakuninNoClick);
		ButtonErrorTojiru.onClick.AddListener (ButtonErrorTojiruClick);

		ButtonTushinUp [0].onClick.AddListener (ButtonTushinUp1000Click);
		ButtonTushinUp [1].onClick.AddListener (ButtonTushinUp100Click);
		ButtonTushinUp [2].onClick.AddListener (ButtonTushinUp10Click);
		ButtonTushinUp [3].onClick.AddListener (ButtonTushinUp1Click);
		ButtonTushinDown [0].onClick.AddListener (ButtonTushinDown1000Click);
		ButtonTushinDown [1].onClick.AddListener (ButtonTushinDown100Click);
		ButtonTushinDown [2].onClick.AddListener (ButtonTushinDown10Click);
		ButtonTushinDown [3].onClick.AddListener (ButtonTushinDown1Click);



		swipIdouFlag = false;

		//単体動作テスト用
		//パラメタ詳細は設計書参照
		if (mparam==null) {
			mparam = new object[] {
				ManagerObject.instance.player,
			};
		}
		muser1 = (User)mparam[0];		// たまごっち
//		muser1.utype = UserType.MIX2;

	
		yield return null;


	}

	void Destroy(){
		Debug.Log ("ItemBox Destroy");
	}


	private bool swipIdouFlag;
	private float StartPos;
	private float EndPos;

	void Update(){
		if (swipIdouFlag) {
			if (Input.GetMouseButtonDown (0)) {
				StartPos = Input.mousePosition.y;
			}
			if (Input.GetMouseButtonUp (0)) {
				if (StartPos != 0) {
					EndPos = Input.mousePosition.y;
					if (StartPos > EndPos) {
						Debug.Log ("上から下移動" + StartPos.ToString () + "から" + EndPos.ToString ());
					} else if (StartPos < EndPos) {
						Debug.Log ("下から上移動" + StartPos.ToString () + "から" + EndPos.ToString ());
					}
				}
				StartPos = 0;
				EndPos = 0;
			}
		} else {
			StartPos = 0;
			EndPos = 0;
		}
	}



	private void ButtonPresentClick(){
		if (muser1.utype == UserType.MIX2) {
			EventPresentMIX2.SetActive (true);
		} else {
			EventPresent.SetActive (true);
			swipIdouFlag = true;
		}
	}
	private void ButtonPointClick(){
		
		EventPresentPoint.SetActive (true);
		if (muser1.utype == UserType.MIX2) {
			ButtonPointSend.gameObject.SetActive (true);
			ButtonPointBackMIX2.gameObject.SetActive (true);
			ButtonPointBack.gameObject.SetActive (false);
		} else {
			ButtonPointSend.gameObject.SetActive (false);
			ButtonPointBackMIX2.gameObject.SetActive (false);
			ButtonPointBack.gameObject.SetActive (true);
		}
	}
	private void ButtonTojiruClick(){
		Debug.Log ("たまタウンへ・・・");
		ManagerObject.instance.view.change("Town");
	}
	private void ButtonPresentBackMIX2Click(){
		EventPresentMIX2.SetActive (false);
	}
	private void ButtonPresentBackClick(){
		EventPresent.SetActive (false);
		swipIdouFlag = false;
	}
	private void ButtonBackOKClick(){
	}
	private void ButtonBackNotClick(){
	}
	private void ButtonPointSendClick(){
		EventTushinPoint.SetActive (true);
	}
	private void ButtonPointBackMIX2Click(){
		EventPresentPoint.SetActive (false);
	}
	private void ButtonPointBackClick(){
		EventPresentPoint.SetActive (false);
	}
	private void ButtonTushinBackClick(){
		EventTushinPoint.SetActive (false);
	}
	private void ButtonTushinSendClick(){
	}
	private void ButtonTushinSendAllClick(){
	}
	private void ButtonKakuninYesClick(){
	}
	private void ButtonKakuninNoClick(){
	}
	private void ButtonErrorTojiruClick(){
	}
		
	private void ButtonTushinUp1000Click(){
	}
	private void ButtonTushinUp100Click(){
	}
	private void ButtonTushinUp10Click(){
	}
	private void ButtonTushinUp1Click(){
	}
	private void ButtonTushinDown1000Click(){
	}
	private void ButtonTushinDown100Click(){
	}
	private void ButtonTushinDown10Click(){
	}
	private void ButtonTushinDown1Click(){
	}



}
