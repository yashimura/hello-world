using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;



namespace Mix2App.Propose{
	public class Propose : MonoBehaviour,IReceiver {
		[SerializeField] private GameObject[] CharaTamago;					// たまごっち
		[SerializeField] private GameObject EventSky;						// 背景
		[SerializeField] private GameObject EventWait;						// メイン
		[SerializeField] private GameObject EventMiss;						// 失敗
//		[SerializeField] private GameObject EventFutago;					// 双子選択画面
//		[SerializeField] private GameObject EventKakunin;					// 確認画面



		private readonly string[] manMessageTable1 = new string[]{		// 男の子のメッセージ
			"すきです！けっこんしたい（＋語尾）",
			"ひとめぼれ・・・けっこんしたい（＋語尾）",
			"もう あなたしかいない（＋語尾）",
			"あなたを しあわせにしたい（＋語尾）",
			"いっしょになってほしい（＋語尾）",
		};
		private readonly string[] womanMessageTable1 = new string[]{	// 女の子のメッセージ
			"あなたと いっしょにいたい（＋語尾）",
			"あなたしかみえない（＋語尾）",
			"いっしょにいてほしい（＋語尾）",
			"おねがいっ！けっこんしたい（＋語尾）",
			"あなたへの スキがとまらない（＋語尾）",
		};
		private readonly string[] manMessageTable2 = new string[]{		// 男の子のメッセージ
			"あなたと けっこんしたい（＋語尾）",
			"もう あなたしかいない（＋語尾）",
			"すごくうれしい（＋語尾）",
			"しあわせになりたい（＋語尾）",
			"かんげきした（＋語尾）",
		};
		private readonly string[] womanMessageTable2 = new string[]{	// 女の子のメッセージ
			"いつも となりにいてほしい（＋語尾）",
			"むねのドキドキが とまらない（＋語尾）",
			"あなたとなら けっこんしたい（＋語尾）",
			"なかよくしてほしい（＋語尾）",
			"かんげきしちゃった（＋語尾）",
		};
		private readonly string[] manMessageTable3 = new string[]{		// 男の子のメッセージ
			"ごめんとしかいえない（＋語尾）",
			"あなたと けっこんできない（＋語尾）",
			"あきらめてほしい（＋語尾）",
			"ぼくには もったいない（＋語尾）",
			"ほかに すきなひとがいる（＋語尾）",
		};
		private readonly string[] womanMessageTable3 = new string[]{	// 女の子のメッセージ
			"あきらめてほしい（＋語尾）",
			"ほかに すきなあいてがいる（＋語尾）",
			"すこし かんがえたい（＋語尾）",
			"おともだちならいい（＋語尾）",
			"あなたと けっこんできない（＋語尾）",
		};



		private object[] mparam;											// 他のシーンからくるパラメータ
		private bool mProposeResult;										// プロポーズ結果
		private int mProposeType;											// プロポーズの種類
		private User mUser1;												// 自分
		private User mUser2;												// 相手
		private int mBrother1;												// 自分の兄弟種類
		private int mBrother2;												// 相手の兄弟種類

		private User userR, userL;
		private int brotherR, brotherL;

//		private CharaBehaviour[] cbCharaTamago = new CharaBehaviour[6];		// 
		private CharaBehaviour[] cbCharaTamago = new CharaBehaviour[2];		// 

		void Awake(){
			Debug.Log ("Propose Awake");
			mparam = null;
			mProposeResult = true;
			mProposeType = 0;
			mUser1 = null;
			mUser2 = null;
			mBrother1 = 0;
			mBrother2 = 0;
		}

		public void receive(params object[] parameter){
			Debug.Log ("Propose receive");
			mparam = parameter;

			//単体動作テスト用
			//パラメタ詳細は設計書参照
			if (mparam==null) {
				mparam = new object[] {
					true,														// プロポーズ結果　true=成功、false=失敗			bool
					1,															// プロポーズ種類　0=自分が受けた、1=自分がした	int
					new TestUser(1,UserKind.ANOTHER,UserType.MIX2,1,16,17,0,1),	// 自分										user
					1,															// 自分のキャラ種類、							int
					new TestUser(1,UserKind.ANOTHER,UserType.MIX2,1,23,24,0,1),	// プロポーズ相手、							user
					1,															// プロポーズキャラ種類							int
				};
			}

			mProposeResult = (bool)mparam [0];
			mProposeType = (int)mparam [1];
			mUser1 = (User)mparam [2];
			mBrother1 = (int)mparam [3];
			mUser2 = (User)mparam [4];
			mBrother2 = (int)mparam [5];

			StartCoroutine(mStart());
		}

		void Start(){
			
		}
			
		void Destroy(){
			Debug.Log ("Propose Destroy");
		}

		void OnDestroy(){
			Debug.Log ("Propose OnDestroy");
		}

		IEnumerator mStart(){

			EventWait.transform.Find("Button_blue_modoru").gameObject.GetComponent<Button> ().onClick.AddListener (ButtonModoruClick);


			cbCharaTamago[0] = CharaTamago[0].GetComponent<CharaBehaviour> ();		// 
			cbCharaTamago[1] = CharaTamago[1].GetComponent<CharaBehaviour> ();		// 
/*
			cbCharaTamago[2] = CharaTamago[2].GetComponent<CharaBehaviour> ();		// 
			cbCharaTamago[3] = CharaTamago[3].GetComponent<CharaBehaviour> ();		// 
			cbCharaTamago[4] = CharaTamago[4].GetComponent<CharaBehaviour> ();		// 
			cbCharaTamago[5] = CharaTamago[5].GetComponent<CharaBehaviour> ();		// 


			yield return cbCharaTamago[2].init (mUser1.chara1);
			if (mUser1.chara2 != null) {
				yield return cbCharaTamago [3].init (mUser1.chara2);
			}
			yield return cbCharaTamago[4].init (mUser2.chara1);
			if (mUser2.chara2 != null) {
				yield return cbCharaTamago [5].init (mUser2.chara2);
			}
*/

			StartCoroutine(mainLoop());

			yield return null;
		}

		void Update () {
		

			TamagochiImageMove (EventWait, CharaTamago [0], "tamago/charaR/");
			TamagochiImageMove (EventWait, CharaTamago [1], "tamago/charaL/");
		}

		private void ButtonModoruClick(){
			ManagerObject.instance.sound.playSe (17);
			Debug.Log ("たまタウンへ・・・");
			ManagerObject.instance.view.change(SceneLabel.TOWN);
		}


		private IEnumerator mainLoop(){
/*
			if ((muser1.chara2 == null) && (muser2.chara2 == null)) {
				// シングル同士のプロポーズ
				EventKakunin.SetActive();
			} else {
				if (muser1.chara2 != null) {
					EventFutago.SetActive ();
				}
				if (muser2.chara2 != null) {
				}
			}
*/

			if (mProposeType == 0) {
				userR = mUser2;
				userL = mUser1;

				brotherR = mBrother2;
				brotherL = mBrother1;
			} else {
				userR = mUser1;
				userL = mUser2;

				brotherR = mBrother1;
				brotherL = mBrother2;
			}

			if (brotherR == 0) {
				yield return cbCharaTamago [0].init (userR.chara1);
			} else {
				yield return cbCharaTamago [0].init (userR.chara2);
			}

			if (brotherL == 0) {
				yield return cbCharaTamago [1].init (userL.chara1);
			} else {
				yield return cbCharaTamago [1].init (userL.chara2);
			}

			cbCharaTamago [0].gotoAndPlay (MotionLabel.IDLE);
			cbCharaTamago [1].gotoAndPlay (MotionLabel.IDLE);

			EventSky.SetActive (true);
			EventWait.SetActive (true);
			if (mProposeType == 1) {
				EventWait.transform.Find ("fukidashi_left").gameObject.SetActive (true);
				EventWait.transform.Find ("fukidashi_left/text").gameObject.GetComponent<Text> ().text = "・・・";
				yield return new WaitForSeconds (3.0f);
				EventWait.transform.Find ("fukidashi_left").gameObject.SetActive (false);
			}

			EventWait.transform.Find ("Button_blue_modoru").gameObject.SetActive (false);
			ManagerObject.instance.sound.playBgm (16);

			yield return StartCoroutine(TamagochiRightIdou (EventWait.transform.Find ("tamago/charaR").gameObject));


			if (mProposeResult == true) {
				// 成功
				yield return StartCoroutine(TamagochiProposeSuccess(EventWait.transform.Find("tamago/charaL").gameObject));

				Debug.Log ("結婚へ・・・");
				ManagerObject.instance.view.change(SceneLabel.MARRIAGE,mProposeType,mUser1,mBrother1,mUser2,mBrother2);
			} else {
				// 失敗
				yield return StartCoroutine(TamagochiProposeMiss());

				Debug.Log ("たまタウンへ・・・");
				ManagerObject.instance.view.change(SceneLabel.TOWN);
			}


			yield return null;
		}
			

		private IEnumerator TamagochiRightIdou(GameObject _obj){
			Vector3 _pos = new Vector3 (100, -50, 0);

			cbCharaTamago [0].gotoAndPlay (MotionLabel.WALK);

			StartCoroutine (TamagochiWalkSEPlay (cbCharaTamago [0]));

			while (true) {
				_obj.transform.localPosition = Vector3.MoveTowards (_obj.transform.localPosition, _pos, (40.0f * Time.deltaTime));
				if (_obj.transform.localPosition.x == _pos.x) {
					break;
				}
				yield return null;
			}

			cbCharaTamago [0].gotoAndPlay (MotionLabel.SHY1);

			string _gobi;
			if (brotherR == 0) {
				_gobi = userR.chara1.wend;
			} else {
				_gobi = userR.chara2.wend;
			}

			if (userR.chara1.sex == 0) {
				EventWait.transform.Find ("fukidashi_right/text").gameObject.GetComponent<Text> ().text = manMessageTable1 [Random.Range (0, manMessageTable1.Length)].Replace ("（＋語尾）", _gobi);
			} else {
				EventWait.transform.Find ("fukidashi_right/text").gameObject.GetComponent<Text> ().text = womanMessageTable1 [Random.Range (0, womanMessageTable1.Length)].Replace ("（＋語尾）", _gobi);
			}
			EventWait.transform.Find ("fukidashi_right").gameObject.SetActive (true);

			yield return new WaitForSeconds (3.0f);
		}

		private IEnumerator TamagochiProposeSuccess(GameObject _obj){
			Vector3 _pos = new Vector3 (-100, -50, 0);

			cbCharaTamago [1].gotoAndPlay (MotionLabel.WALK);

			StartCoroutine (TamagochiWalkSEPlay (cbCharaTamago [1]));

			while (true) {
				_obj.transform.localPosition = Vector3.MoveTowards (_obj.transform.localPosition, _pos, (40.0f * Time.deltaTime));
				if (_obj.transform.localPosition.x == _pos.x) {
					break;
				}
				yield return null;
			}

			EventWait.transform.Find ("fukidashi_right").gameObject.SetActive (false);

			if (Random.Range (0, 2) == 0) {
				cbCharaTamago [1].gotoAndPlay (MotionLabel.SHY2);
			} else {
				cbCharaTamago [1].gotoAndPlay (MotionLabel.SHY3);
			}
			cbCharaTamago [0].gotoAndPlay (MotionLabel.SHOCK);

			ManagerObject.instance.sound.playJingle (17);

			string _gobi;
			if (brotherL == 0) {
				_gobi = userL.chara1.wend;
			} else {
				_gobi = userL.chara2.wend;
			}

			if (userL.chara1.sex == 0) {
				EventWait.transform.Find ("fukidashi_left2/text").gameObject.GetComponent<Text> ().text = manMessageTable2 [Random.Range (0, manMessageTable2.Length)].Replace ("（＋語尾）", _gobi);
			} else {
				EventWait.transform.Find ("fukidashi_left2/text").gameObject.GetComponent<Text> ().text = womanMessageTable2 [Random.Range (0, womanMessageTable2.Length)].Replace ("（＋語尾）", _gobi);
			}
			EventWait.transform.Find ("fukidashi_left2").gameObject.SetActive (true);

			yield return new WaitForSeconds (2.0f);

			EventWait.transform.Find ("fukidashi_left2").gameObject.SetActive (false);

			TamagochiRandomGlay (cbCharaTamago [0]);
			TamagochiRandomGlay (cbCharaTamago [1]);

			yield return new WaitForSeconds (3.0f);

			yield return StartCoroutine(ProposeHeartSuccess());
		}
		private IEnumerator ProposeHeartSuccess(){
			GameObject _obj = EventWait.transform.Find ("heartloder").gameObject;
			Vector3 _pos = _obj.transform.localPosition;

			while (true) {
				_pos.y += 100;
				_obj.transform.localPosition = _pos;
				if (_pos.y >= 1000) {
					break;
				}
				yield return null;
			}
		}

		private IEnumerator TamagochiProposeMiss(){
			EventWait.transform.Find ("fukidashi_right").gameObject.SetActive (false);

			string _gobi;
			if (brotherL == 0) {
				_gobi = userL.chara1.wend;
			} else {
				_gobi = userL.chara2.wend;
			}

			if (userL.chara1.sex == 0) {
				EventWait.transform.Find ("fukidashi_left/text").gameObject.GetComponent<Text> ().text = manMessageTable3 [Random.Range (0, manMessageTable3.Length)].Replace ("（＋語尾）", _gobi);
			} else {
				EventWait.transform.Find ("fukidashi_left/text").gameObject.GetComponent<Text> ().text = womanMessageTable3 [Random.Range (0, womanMessageTable3.Length)].Replace ("（＋語尾）", _gobi);
			}
			EventWait.transform.Find ("fukidashi_left").gameObject.SetActive (true);

			ManagerObject.instance.sound.playJingle (18);

			cbCharaTamago [0].gotoAndPlay (MotionLabel.SHOCK);
			yield return new WaitForSeconds (0.5f);

			cbCharaTamago [0].gotoAndPlay (MotionLabel.CRY);
			yield return new WaitForSeconds (1.0f);

			EventMiss.SetActive (true);

			while (true) {
				if (EventMiss.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
					break;
				}
				yield return null;
			}
			EventMiss.SetActive (false);

			yield return new WaitForSeconds (2.0f);

			yield return StartCoroutine(ProposeHeartMiss());

		}
		private IEnumerator ProposeHeartMiss(){
			GameObject _obj = EventWait.transform.Find ("heartloder_b").gameObject;
			Vector3 _pos = _obj.transform.localPosition;

			while (true) {
				_pos.y -= 100;
				_obj.transform.localPosition = _pos;
				if (_pos.y <= -1000) {
					break;
				}
				yield return null;
			}
		}

		private IEnumerator TamagochiWalkSEPlay(CharaBehaviour _cb){
			while (true) {
				if (_cb.nowlabel != MotionLabel.WALK) {
					break;
				}
				ManagerObject.instance.sound.playSe (1);
				yield return new WaitForSeconds (0.5f);
			}
		}


		private void TamagochiRandomGlay(CharaBehaviour _cb){
			switch (Random.Range (0, 3)) {
			case	0:
				{
					_cb.gotoAndPlay (MotionLabel.GLAD1);
					break;
				}
			case	1:
				{
					_cb.gotoAndPlay (MotionLabel.GLAD2);
					break;
				}
			case	2:
				{
					_cb.gotoAndPlay (MotionLabel.GLAD3);
					break;
				}
			}
		}



		private void TamagochiImageMove(GameObject toObj,GameObject fromObj,string toStr){
			for (int i = 0; i < fromObj.transform.Find ("Layers").transform.childCount; i++) {
				toObj.transform.Find (toStr + "CharaImg/Layers/" + fromObj.transform.Find ("Layers").transform.GetChild (i).name).gameObject.transform.SetSiblingIndex (i);
			}

			toObj.transform.Find (toStr + "CharaImg").gameObject.GetComponent<Image> ().enabled = false;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer0").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer0").gameObject.GetComponent<Image> ().enabled;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer1").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer1").gameObject.GetComponent<Image> ().enabled;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer2").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer2").gameObject.GetComponent<Image> ().enabled;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer3").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer3").gameObject.GetComponent<Image> ().enabled;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer4").gameObject.GetComponent<Image> ().enabled = fromObj.transform.Find ("Layers/Layer4").gameObject.GetComponent<Image> ().enabled;

			toObj.transform.Find (toStr + "CharaImg/Layers/Layer0").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer0").gameObject.GetComponent<Image> ().sprite;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer1").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer1").gameObject.GetComponent<Image> ().sprite;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer2").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer2").gameObject.GetComponent<Image> ().sprite;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer3").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer3").gameObject.GetComponent<Image> ().sprite;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer4").gameObject.GetComponent<Image> ().sprite = fromObj.transform.Find ("Layers/Layer4").gameObject.GetComponent<Image> ().sprite;

			toObj.transform.Find (toStr + "CharaImg/Layers/Layer0").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer0").gameObject.transform.localPosition;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer1").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer1").gameObject.transform.localPosition;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer2").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer2").gameObject.transform.localPosition;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer3").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer3").gameObject.transform.localPosition;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer4").gameObject.transform.localPosition = fromObj.transform.Find ("Layers/Layer4").gameObject.transform.localPosition;

			toObj.transform.Find (toStr + "CharaImg/Layers/Layer0").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer0").gameObject.transform.localScale;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer1").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer1").gameObject.transform.localScale;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer2").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer2").gameObject.transform.localScale;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer3").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer3").gameObject.transform.localScale;
			toObj.transform.Find (toStr + "CharaImg/Layers/Layer4").gameObject.transform.localScale = fromObj.transform.Find ("Layers/Layer4").gameObject.transform.localScale;
		}

	}
}
