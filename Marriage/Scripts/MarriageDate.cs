using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;
using Mix2App.UI;

namespace Mix2App.MarriageDate{
	public class MarriageDate : MonoBehaviour,IReceiver {
		[SerializeField] private GameObject EventDate = null;

		[SerializeField] private GameObject EventEnd = null;			// 原っぱ
		[SerializeField] private GameObject EventGarden = null;         // 庭園
        [SerializeField] private GameObject EventBeach = null;          // 海岸
        [SerializeField] private GameObject EventPark = null;			// 遊園地

		[SerializeField] private GameObject man_walk1 = null;			// 男の子、遊園地の歩き
		[SerializeField] private GameObject man_walk2 = null;			// 男の子、海岸の歩き
		[SerializeField] private GameObject man_sit = null;	    		// 男の子、庭園での座り
		[SerializeField] private GameObject man_happy = null;			// 男の子、原っぱでの喜び
		[SerializeField] private GameObject man_walk3 = null;			// 男の子、原っぱでの歩き

		[SerializeField] private GameObject woman_walk1 = null;	    	// 女の子、遊園地の歩き
		[SerializeField] private GameObject woman_walk2 = null;	    	// 女の子、海岸の歩き
		[SerializeField] private GameObject woman_sit = null;	    	// 女の子、庭園での座り
		[SerializeField] private GameObject woman_happy = null;			// 女の子、原っぱでの喜び
		[SerializeField] private GameObject woman_walk3 = null;			// 女の子、原っぱでの歩き

		[SerializeField] private GameObject man_text = null;			// 男の子、原っぱでのコメント
		[SerializeField] private GameObject woman_text = null;			// 女の子、原っぱでのコメント

		[SerializeField] private GameObject manChara1 = null;			// 男の子
		[SerializeField] private GameObject womanChara1 = null;			// 女の子

		[SerializeField] private GameObject EventWhite = null;

		[SerializeField] private Sprite[] StampImage = null;			// スタンプイメージ
	
		private readonly string[] manMessageTable = new string[]{		// 男の子のメッセージ
			"またいっしょに\nあそんでほしい（＋語尾）",
			"また あいたい（＋語尾）",
		};
		private readonly string[] womanMessageTable = new string[]{		// 女の子のメッセージ
			"これからも\nなかよくしたい（＋語尾）",
			"さそってくれて\nうれしかった（＋語尾）",
		};



		private object[]		mparam;

		private float 			panelWhiteA;

		private CharaBehaviour	cbMan1;								// 男の子キャラ
		private CharaBehaviour	cbWoman1;							// 女の子キャラ

		private bool startEndFlag = false;
		private int waitCount;

		private float manXposition = 0.0f;
		private float womanXposition = 0.0f;

		private bool whileParkFlag;
		private bool whileBeachFlag;
		private bool whileEndFlag;

		private statusJobCount	jobCount = statusJobCount.marriageJobCount000;
		private enum statusJobCount{
			marriageJobCount000,
			marriageJobCount010,
			marriageJobCount020,
			marriageJobCount030,
			marriageJobCount040,
			marriageJobCount050,
			marriageJobCount060,
			marriageJobCount070,
			marriageJobCount080,
			marriageJobCount090,
			marriageJobCount100,
			marriageJobCount110,
			marriageJobCount120,
			marriageJobCount130, 
			marriageJobCount140,
			marriageJobCount150,
		}

		private User muser1;//自分
		private User muser2;//相手
		//private int mkind;//結婚種類
		private int mkind1;//兄弟種類
		private int mkind2;//兄弟種類

		void Awake(){
			Debug.Log ("MarriageDate Awake");

			mparam=null;
			muser1=null;
			muser2=null;
			//mkind=0;
			mkind1=0;
			mkind2=0;
		}

		public void receive(params object[] parameter){
			Debug.Log ("MarriageDate receive");
			mparam = parameter;
		}

		IEnumerator Start () {
			Debug.Log ("MarriageDate start");

			//単体動作テスト用
			//パラメタ詳細は設計書参照
			if (mparam==null) {
				mparam = new object[] {
					0,
					ManagerObject.instance.player,
					0,
					new TestUser(1,UserKind.ANOTHER,UserType.MIX2,1,23,0,0,1),
					0
				};
			}

			//mkind = (int)mparam[0];
			muser1 = (User)mparam[1];		// 右のたまごっち
			mkind1 = (int)mparam[2];
			muser2 = (User)mparam[3];		// 左のたまごっち
			mkind2 = (int)mparam[4];




//			muser1.utype = UserType.MIX;
//			muser2.utype = UserType.MIX;




			jobCount = statusJobCount.marriageJobCount000;
			startEndFlag = false;
			waitCount = 1;

			man_walk1.SetActive (false);
			woman_walk1.SetActive (false);
			man_walk2.SetActive (false);
			woman_walk2.SetActive (false);
			man_sit.SetActive (false);
			woman_sit.SetActive (false);
			man_happy.SetActive (false);
			woman_happy.SetActive (false);
			man_walk3.SetActive (false);
			woman_walk3.SetActive (false);

			EventPark.SetActive (true);
			EventBeach.SetActive (false);
			EventGarden.SetActive (false);
			EventEnd.SetActive (false);



			float use_screen_x = Screen.currentResolution.width;
			float use_screen_y = Screen.currentResolution.height;
			#if UNITY_EDITOR
			use_screen_x = Screen.width;
			use_screen_y = Screen.height;
			#endif

			float num;
			if (use_screen_x >= use_screen_y) {
				num = use_screen_x / use_screen_y;
			} else {
				num = use_screen_y / use_screen_x;
			}

			if ((num > 1.33f) && (num < 1.34f)) {
				EventDate.GetComponent<Transform> ().transform.localScale = new Vector3 (1.4f, 1.4f, 1.0f);
			} else {
				EventDate.GetComponent<Transform> ().transform.localScale = new Vector3 (1.45f, 1.45f, 1.0f);
			}

			// 男の子と女の子のたまごっちをここで設定する
			cbMan1 = manChara1.GetComponent<CharaBehaviour> ();				// 男の子
			cbWoman1 = womanChara1.GetComponent<CharaBehaviour> ();			// 女の子
			if (mkind1 == 0) {
				yield return cbMan1.init (muser1.chara1);
			} else {
				yield return cbMan1.init (muser1.chara2);
			}
			if (mkind2 == 0) {
				yield return cbWoman1.init (muser2.chara1);
			} else {
				yield return cbWoman1.init (muser2.chara2);
			}

			startEndFlag = true;
		}

		void Destroy()
		{
			Debug.Log("MarriageDate Destroy");
		}

		void Update () {
			switch (jobCount) {
			case	statusJobCount.marriageJobCount000:
				{
					if (startEndFlag) {
						waitCount--;
						if (waitCount == 0) {
							jobCount = statusJobCount.marriageJobCount010;
							cbMan1.gotoAndPlay (MotionLabel.WALK);
							cbWoman1.gotoAndPlay (MotionLabel.WALK);

							ManagerObject.instance.sound.playBgm (19);
						}
					}
					break;
				}
			case	statusJobCount.marriageJobCount010:
				{
					MarriageJobTypeOpening ();
//					EventPark.SetActive (true);
					jobCount = statusJobCount.marriageJobCount020;
					whileParkFlag = true;
					StartCoroutine ("EventParkCharaIdou");

					Debug.Log ("遊園地");

					// 右向きに散歩
					cbMan1.gotoAndPlay (MotionLabel.WALK);
					cbWoman1.gotoAndPlay (MotionLabel.WALK);

					break;
				}
			case	statusJobCount.marriageJobCount020:
				{
					if (EventPark.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.marriageJobCount030;
						panelWhiteA = 0.0f;
					}
					break;
				}
			case	statusJobCount.marriageJobCount030:
				{
					if (WhiteOut ()) {
						EventPark.SetActive (false);
						EventBeach.SetActive (true);
						whileParkFlag = false;
						jobCount = statusJobCount.marriageJobCount040;
						whileBeachFlag = true;
						StartCoroutine ("EventBeachCharaIdou");

						Debug.Log ("海岸");

						// 左向きに散歩
						cbMan1.gotoAndPlay (MotionLabel.WALK);
						cbWoman1.gotoAndPlay (MotionLabel.WALK);
					}
					break;
				}
			case	statusJobCount.marriageJobCount040:
				{
					if (WhiteIn ()) {
						jobCount = statusJobCount.marriageJobCount050;
					}
					break;
				}
			case	statusJobCount.marriageJobCount050:
				{
					if (EventBeach.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0) {
						panelWhiteA = 0.0f;
						jobCount = statusJobCount.marriageJobCount060;
					}
					break;
				}
			case	statusJobCount.marriageJobCount060:
				{
					if (WhiteOut ()) {
						EventBeach.SetActive (false);
						EventGarden.SetActive (true);
						jobCount = statusJobCount.marriageJobCount070;
						whileBeachFlag = false;

						Debug.Log ("庭園");

						// ベンチに向き合って座る
						cbMan1.gotoAndPlay (MotionLabel.SIT2);
						cbWoman1.gotoAndPlay (MotionLabel.SIT2);
					}
					break;
				}
			case	statusJobCount.marriageJobCount070:
				{
					if (WhiteIn ()) {
						jobCount = statusJobCount.marriageJobCount080;
					}
					break;
				}
			case	statusJobCount.marriageJobCount080:
				{
					if (EventGarden.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						panelWhiteA = 0.0f;
						jobCount = statusJobCount.marriageJobCount090;
					}
					break;
				}
			case	statusJobCount.marriageJobCount090:
				{
					if (WhiteOut ()) {
						EventGarden.SetActive (false);
						EventEnd.SetActive (true);
						jobCount = statusJobCount.marriageJobCount100;

						Debug.Log ("原っぱ");

						TamagochiAnimeRandomChange (cbMan1);
						TamagochiAnimeRandomChange (cbWoman1);

						manXposition = man_walk3.transform.localPosition.x;
						womanXposition = woman_walk3.transform.localPosition.x;

						EventEndMessageSet ();

						{
							Vector3 pos = EventEnd.transform.Find ("serif_1").gameObject.transform.localPosition;
							pos.x -= 23.0f;
							EventEnd.transform.Find ("serif_1").gameObject.transform.localPosition = pos;
						}

						whileEndFlag = true;
						StartCoroutine ("EventEndCharaIdou");
						StartCoroutine ("EventEndFukidashiCheck");
					}
					break;
				}
			case	statusJobCount.marriageJobCount100:
				{
					if (WhiteIn ()) {
						jobCount = statusJobCount.marriageJobCount110;
					}
					break;
				}
			case	statusJobCount.marriageJobCount110:
				{
					if((manXposition != man_walk3.transform.localPosition.x) || (womanXposition != woman_walk3.transform.localPosition.x)){
						jobCount = statusJobCount.marriageJobCount120;
					}
					break;
				}
			case	statusJobCount.marriageJobCount120:
				{
					if (EventEnd.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.marriageJobCount130;
						panelWhiteA = 0.0f;
					}
					break;
				}
			case	statusJobCount.marriageJobCount130:
				{
					if (WhiteOut ()) {
						jobCount = statusJobCount.marriageJobCount140;
						whileEndFlag = false;
					}
					break;
				}
			case	statusJobCount.marriageJobCount140:
				{
					Debug.Log ("たまタウンへ・・・");
					jobCount = statusJobCount.marriageJobCount150;
					ManagerObject.instance.view.change (SceneLabel.TOWN);
					break;
				}
			case	statusJobCount.marriageJobCount150:
				{
					break;
				}
			}



			UIFunction.TamagochiImageMove (EventPark, manChara1, "Chara/manChara/");
			UIFunction.TamagochiImageMove (EventPark, womanChara1, "Chara/womanChara/");

			UIFunction.TamagochiImageMove (EventBeach, manChara1, "Chara/manChara/");
			UIFunction.TamagochiImageMove (EventBeach, womanChara1, "Chara/womanChara/");

			UIFunction.TamagochiImageMove (EventGarden, manChara1, "bg1/manChara/");
			UIFunction.TamagochiImageMove (EventGarden, womanChara1, "bg1/womanChara/");

			UIFunction.TamagochiImageMove (EventEnd, manChara1, "bg1/manChara/");
			UIFunction.TamagochiImageMove (EventEnd, womanChara1, "bg1/womanChara/");
		}
			
		private void MarriageJobTypeOpening(){
			EventDate.SetActive (true);
		}

		private void EventEndMessageSet(){
			string _gobiMan, _gobiWoman;

			if (mkind1 == 0) {
				_gobiMan = muser1.chara1.wend;
			} else {
				_gobiMan = muser1.chara2.wend;
			}

			if (mkind2 == 0) {
				_gobiWoman = muser2.chara1.wend;
			} else {
				_gobiWoman = muser2.chara2.wend;
			}

			man_text.GetComponent<Text> ().text = manMessageTable [Random.Range (0, manMessageTable.Length)].Replace ("（＋語尾）", _gobiMan);
			woman_text.GetComponent<Text> ().text = womanMessageTable [Random.Range (0, womanMessageTable.Length)].Replace ("（＋語尾）", _gobiWoman);

			EventEnd.transform.Find("bg1/manChara/fukidashi/stamp").gameObject.GetComponent<Image>().sprite = StampImage[Random.Range(0,StampImage.Length)];
			EventEnd.transform.Find("bg1/womanChara/fukidashi/stamp").gameObject.GetComponent<Image>().sprite = StampImage[Random.Range(0,StampImage.Length)];

//			if (muser1.utype != UserType.MIX2) {
				EventEnd.transform.Find ("serif_1").gameObject.SetActive (false);
//			}

//			if (muser2.utype != UserType.MIX2) {
				EventEnd.transform.Find ("serif 2").gameObject.SetActive (false);
//			}
			EventEnd.transform.Find ("bg1/manChara/fukidashi").gameObject.SetActive (false);
			EventEnd.transform.Find ("bg1/womanChara/fukidashi").gameObject.SetActive (false);
		}

		private void TamagochiAnimeRandomChange(CharaBehaviour cb){
			
			switch (Random.Range (0, 3)) {
			case	0:
				{
					cb.gotoAndPlay (MotionLabel.GLAD1);				// 喜び１
					break;
				}
			case	1:
				{
					cb.gotoAndPlay (MotionLabel.GLAD2);				// 喜び２
					break;
				}
			case	2:
				{
					cb.gotoAndPlay (MotionLabel.GLAD3);				// 喜び３
					break;
				}
			}
		}

		private bool WhiteOut(){
			panelWhiteA += (4.0f * (60 * Time.deltaTime));
			if(panelWhiteA >= 255.0f){
				panelWhiteA = 255.0f;
			}
			EventWhite.GetComponent<Image> ().color = new Color (1.0f, 1.0f, 1.0f, panelWhiteA / 255.0f);
			if (panelWhiteA == 255.0f) {
				return true;
			}
			return false;
		}

		private bool WhiteIn(){
			panelWhiteA -= (4.0f * (60 * Time.deltaTime));
			if(panelWhiteA <= 0.0f){
				panelWhiteA = 0.0f;
			}
			EventWhite.GetComponent<Image> ().color = new Color (1.0f, 1.0f, 1.0f, panelWhiteA / 255.0f);
			if (panelWhiteA == 0.0f) {
				return true;
			}
			return false;
		}

		private IEnumerator EventParkCharaIdou(){
			while (whileParkFlag) {
				Vector3 _pos = EventPark.transform.Find ("Chara").gameObject.transform.localPosition;
				_pos.x += (0.075f * (60 * Time.deltaTime));
				EventPark.transform.Find ("Chara").gameObject.transform.localPosition = _pos;
				yield return null;
			}
		}

		private IEnumerator EventBeachCharaIdou(){
			while (whileBeachFlag) {
				Vector3 _pos = EventBeach.transform.Find ("Chara").gameObject.transform.localPosition;
				_pos.x -= (0.6f * (60 * Time.deltaTime));
				EventBeach.transform.Find ("Chara").gameObject.transform.localPosition = _pos;
				yield return null;
			}
		}

		private IEnumerator EventEndCharaIdou(){
			Vector3 _posMan = EventEnd.transform.Find ("bg1/manChara").gameObject.transform.localPosition;
			Vector3 _posWoman = EventEnd.transform.Find("bg1/womanChara").gameObject.transform.localPosition;

			while (true) {
				if ((manXposition == man_walk3.transform.localPosition.x) && (womanXposition == woman_walk3.transform.localPosition.x)) {
					yield return null;
				} else {
					break;
				}
			}

			yield return new WaitForSeconds (0.5f);

			EventEnd.transform.Find ("bg1/manChara").gameObject.transform.localScale = new Vector3 (-1.0f, 1.0f, 0);
			EventEnd.transform.Find ("bg1/womanChara").gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 0);
			cbMan1.gotoAndPlay (MotionLabel.WALK);
			cbWoman1.gotoAndPlay (MotionLabel.WALK);

			while (whileEndFlag) {
				_posMan.x += (0.8f * (60 * Time.deltaTime));
				_posWoman.x -= (0.8f * (60 * Time.deltaTime));

				EventEnd.transform.Find ("bg1/manChara").gameObject.transform.localPosition = _posMan;
				EventEnd.transform.Find ("bg1/womanChara").gameObject.transform.localPosition = _posWoman;
			
				yield return null;
			}
		}
		private IEnumerator EventEndFukidashiCheck(){
			while (whileEndFlag) {
//				if (muser1.utype != UserType.MIX2) {
					if (man_text.GetComponent<Text> ().enabled == true) {
						EventEnd.transform.Find ("bg1/manChara/fukidashi").gameObject.SetActive (true);
					} else {
						EventEnd.transform.Find ("bg1/manChara/fukidashi").gameObject.SetActive (false);
					}
//				}

//				if (muser2.utype != UserType.MIX2) {
					if (woman_text.GetComponent<Text> ().enabled == true) {
						EventEnd.transform.Find ("bg1/womanChara/fukidashi").gameObject.SetActive (true);
					} else {
						EventEnd.transform.Find ("bg1/womanChara/fukidashi").gameObject.SetActive (false);
					}
//				}

				yield return null;
			}
		}



	}
}
