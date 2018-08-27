﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

namespace Mix2App.MarriageDate{
	public class MarriageDate : MonoBehaviour,IReceiver {
		public ManagerObject manager;//ライブラリ

		[SerializeField] private GameObject EventDate;

		[SerializeField] private GameObject	EventEnd;				// 原っぱ
		[SerializeField] private GameObject	EventGarden;			// 庭園
		[SerializeField] private GameObject EventBeach;				// 海岸
		[SerializeField] private GameObject EventPark;				// 遊園地

		[SerializeField] private GameObject	man_walk1;				// 男の子、遊園地の歩き
		[SerializeField] private GameObject	man_walk2;				// 男の子、海岸の歩き
		[SerializeField] private GameObject man_sit;				// 男の子、庭園での座り
		[SerializeField] private GameObject	man_happy;				// 男の子、原っぱでの喜び
		[SerializeField] private GameObject	man_walk3;				// 男の子、原っぱでの歩き

		[SerializeField] private GameObject woman_walk1;			// 女の子、遊園地の歩き
		[SerializeField] private GameObject woman_walk2;			// 女の子、海岸の歩き
		[SerializeField] private GameObject woman_sit;				// 女の子、庭園での座り
		[SerializeField] private GameObject woman_happy;			// 女の子、原っぱでの喜び
		[SerializeField] private GameObject woman_walk3;			// 女の子、原っぱでの歩き

		[SerializeField] private GameObject man_text;				// 男の子、原っぱでのコメント
		[SerializeField] private GameObject woman_text;				// 女の子、原っぱでのコメント

		[SerializeField] private GameObject manChara1;				// 男の子
		[SerializeField] private GameObject womanChara1;			// 女の子


	
		private readonly string[] manMessageTable = new string[]{
			"きょうはありがとう（＋語尾）",
			"またあそぼう（＋語尾）",
		};
		private readonly string[] womanMessageTable = new string[]{
			"これからもよろしく（＋語尾）",
			"たのしかった（＋語尾）",
		};



		private object[]		mparam;

		private float 			panelWhiteA;

		private CharaBehaviour	cbMan1;								// 男の子キャラ
		private CharaBehaviour	cbWoman1;							// 女の子キャラ

		private Vector3 posMan1 = new Vector3(-17.0f,-1.5f,0.0f);
		private Vector3 posWoman1 = new Vector3 (-19.0f, -1.5f, 0.0f);

		private bool startEndFlag = false;
		private int waitCount;
		private bool screenModeFlag = false;

		private float manXposition = 0.0f;
		private float womanXposition = 0.0f;



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
		}

		private User muser1;//自分
		private User muser2;//相手
//		private int mkind;//結婚種類
//		private int mkind1;//兄弟種類
//		private int mkind2;//兄弟種類

		void Awake(){
			Debug.Log ("MarriageDate Awake");

			mparam=null;
			muser1=null;
			muser2=null;
//			mkind=0;
//			mkind1=0;
//			mkind2=0;
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
					manager.player,
					0,
					new TestUser(1,UserKind.ANOTHER,UserType.MIX2,23,0,0,1),
					0
				};
			}

//			mkind = (int)mparam[0];
			muser1 = (User)mparam[1];		// 右のたまごっち
//			mkind1 = (int)mparam[2];
			muser2 = (User)mparam[3];		// 左のたまごっち
//			mkind2 = (int)mparam[4];

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
				EventDate.GetComponent<Transform> ().transform.localScale = new Vector3 (1.35f, 1.35f, 1.0f);
				screenModeFlag = true;
			} else {
				EventDate.GetComponent<Transform> ().transform.localScale = new Vector3 (1.45f, 1.45f, 1.0f);
				screenModeFlag = false;
			}


			// 男の子と女の子のたまごっちをここで設定する
			cbMan1 = manChara1.GetComponent<CharaBehaviour> ();				// 男の子
			cbWoman1 = womanChara1.GetComponent<CharaBehaviour> ();			// 女の子
			yield return cbMan1.init (muser1.chara1);
			yield return cbWoman1.init (muser2.chara1);

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
							cbMan1.gotoAndPlay ("walk");
							cbWoman1.gotoAndPlay ("walk");

						}
					}
					break;
				}
			case	statusJobCount.marriageJobCount010:
				{
					MarriageJobTypeOpening ();
					EventPark.GetComponent<Transform> ().transform.localScale = new Vector3 (3.897114f, 3.897114f, 3.897114f);
					EventBeach.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
					EventGarden.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
					EventEnd.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
					jobCount = statusJobCount.marriageJobCount020;

					Debug.Log ("遊園地");

					// 右向きに散歩
					cbMan1.gotoAndPlay ("walk");
					cbWoman1.gotoAndPlay ("walk");
					cbMan1.GetComponent<SpriteRenderer> ().flipX = true;
					cbWoman1.GetComponent<SpriteRenderer> ().flipX = true;

					posInit ();
					break;
				}
			case	statusJobCount.marriageJobCount020:
				{
					if (EventPark.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						EventPark.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventBeach.GetComponent<Transform> ().transform.localScale = new Vector3 (0.8574491f, 0.8574491f, 0.0f);
						jobCount = statusJobCount.marriageJobCount030;

						Debug.Log ("海岸");

						// 左向きに散歩
						cbMan1.gotoAndPlay ("walk");
						cbWoman1.gotoAndPlay ("walk");
						cbMan1.GetComponent<SpriteRenderer> ().flipX = false;
						cbWoman1.GetComponent<SpriteRenderer> ().flipX = false;
					} else {
						posMan1.x = (man_walk1.transform.localPosition.x / 10.0f) - 0.75f;
						posWoman1.x = (woman_walk1.transform.localPosition.x / 10.0f) - 0.75f;
					}
					break;
				}
			case	statusJobCount.marriageJobCount030:
				{
					if (EventBeach.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0) {
						EventBeach.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventGarden.GetComponent<Transform> ().transform.localScale = new Vector3 (0.8574491f, 0.8574491f, 0.0f);
						jobCount = statusJobCount.marriageJobCount040;

						Debug.Log ("庭園");

						// ベンチに向き合って座る
						posMan1.x = 1.0f;
						posWoman1.x = -1.0f;
						cbMan1.gotoAndPlay ("sit");
						cbWoman1.gotoAndPlay ("sit");
						cbMan1.GetComponent<SpriteRenderer> ().flipX = false;
						cbWoman1.GetComponent<SpriteRenderer> ().flipX = true;
					} else {
						posMan1.x = (man_walk2.transform.localPosition.x / 66.0f) + 1.8f;
						posWoman1.x = (woman_walk2.transform.localPosition.x / 66.0f) + 1.3f;
					}
					break;
				}
			case	statusJobCount.marriageJobCount040:
				{
					if (EventGarden.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						EventGarden.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
						EventEnd.GetComponent<Transform> ().transform.localScale = new Vector3 (1.0f, 1.0f, 0.0f);
						jobCount = statusJobCount.marriageJobCount050;

						Debug.Log ("原っぱ");

						TamagochiAnimeRandomChenge (cbMan1);
						TamagochiAnimeRandomChenge (cbWoman1);
						cbMan1.GetComponent<SpriteRenderer> ().flipX = false;
						cbWoman1.GetComponent<SpriteRenderer> ().flipX = true;

						manXposition = man_walk3.transform.localPosition.x;
						womanXposition = woman_walk3.transform.localPosition.x;

						EventEndMessageSet ();

						{
							Vector3 pos = EventEnd.transform.Find ("serif_1").gameObject.transform.localPosition;
							pos.x -= 20.0f - 5.0f;
							EventEnd.transform.Find ("serif_1").gameObject.transform.localPosition = pos;

							pos = EventEnd.transform.Find ("serif 2").gameObject.transform.localPosition;
							pos.x = -5.0f;
							EventEnd.transform.Find ("serif 2").gameObject.transform.localPosition = pos;
						}
					} else {
						if (screenModeFlag) {
							posMan1.y = (man_sit.transform.localPosition.y / 47.0f) + 0.5f - 1.0f;
							posWoman1.y = (man_sit.transform.localPosition.y / 47.0f) + 0.5f - 1.0f;
						} else {
							posMan1.y = (man_sit.transform.localPosition.y / 43.7f) + 0.5f - 1.0f;
							posWoman1.y = (man_sit.transform.localPosition.y / 43.7f) + 0.5f - 1.0f;
						}
					}
					break;
				}
			case	statusJobCount.marriageJobCount050:
				{
					if ((manXposition == man_walk3.transform.localPosition.x) && (womanXposition == woman_walk3.transform.localPosition.x)) {
						posMan1.y = (EventEnd.transform.Find("bg1").gameObject.transform.localPosition.y / 47.0f) + 1.7f - 1.0f;
						posWoman1.y = (EventEnd.transform.Find("bg1").gameObject.transform.localPosition.y / 47.0f) + 1.7f - 1.0f;
						posMan1.x = 2.0f;
						posWoman1.x = -2.0f;
					} else {
						jobCount = statusJobCount.marriageJobCount060;
						cbMan1.GetComponent<SpriteRenderer> ().flipX = true;
						cbWoman1.GetComponent<SpriteRenderer> ().flipX = false;
						cbMan1.gotoAndPlay ("walk");
						cbWoman1.gotoAndPlay ("walk");
					}
					break;
				}
			case	statusJobCount.marriageJobCount060:
				{
					if (EventEnd.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.marriageJobCount070;

						posMan1.y = 500.0f;					// 男の子を画面外に
						posWoman1.y = 500.0f;				// 男の子の双子を画面外に
					}
					posMan1.x += 0.025f;
					posWoman1.x -= 0.025f;
					break;
				}
			case	statusJobCount.marriageJobCount070:
				{
					Debug.Log ("たまタウンへ・・・");
					jobCount = statusJobCount.marriageJobCount080;
					manager.view.change("Town");
					break;
				}
			case	statusJobCount.marriageJobCount080:
				{
					break;
				}
			case	statusJobCount.marriageJobCount090:
				{
					break;
				}
			case	statusJobCount.marriageJobCount100:
				{
					break;
				}
			}

			manChara1.transform.localPosition = posMan1;
			womanChara1.transform.localPosition = posWoman1;
		}


		private void MarriageJobTypeOpening(){
			EventDate.SetActive (true);
		}


		private void posInit(){
			posMan1 = new Vector3 (-7.0f, -1.5f - 1.0f, 0.0f);
			posWoman1 = new Vector3 (-9.0f, -1.5f - 1.0f, 0.0f);
		}

		private void EventEndMessageSet(){
			man_text.GetComponent<Text> ().text = manMessageTable [Random.Range (0, manMessageTable.Length)].Replace ("（＋語尾）", muser1.chara1.wend);
			woman_text.GetComponent<Text> ().text = womanMessageTable [Random.Range (0, womanMessageTable.Length)].Replace ("（＋語尾）", muser2.chara1.wend);
		}

		private void TamagochiAnimeRandomChenge(CharaBehaviour cb){
			
			switch (Random.Range (0, 3)) {
			case	0:
				{
					cb.gotoAndPlay ("glad1");						// 喜び１
					break;
				}
			case	1:
				{
					cb.gotoAndPlay ("glad2");						// 喜び２
					break;
				}
			case	2:
				{
					cb.gotoAndPlay ("glad3");						// 喜び３
					break;
				}
			}
		}

	}
}
