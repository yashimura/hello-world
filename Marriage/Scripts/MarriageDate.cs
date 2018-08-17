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



		private object[]		mparam;

		private float 			panelWhiteA;

		private CharaBehaviour	cbMan1;								// 男の子キャラ
		private CharaBehaviour	cbWoman1;							// 女の子キャラ

		private Vector3 posMan1 = new Vector3(-17.0f,-1.5f,0.0f);
		private Vector3 posWoman1 = new Vector3 (-19.0f, -1.5f, 0.0f);

		private bool startEndFlag = false;
		private int waitCount;
		private bool waitFlag = false;

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

		void Awake(){
			Debug.Log ("MarriageDate Awake");
		}

		public void receive(params object[] parameter){
			Debug.Log ("MarriageDate receive");
			mparam = parameter;
		}

		IEnumerator Start () {
			Debug.Log ("MarriageDate start");

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
				EventDate.GetComponent<Transform> ().transform.localScale = new Vector3 (1.275f, 1.275f, 1.0f);
			}


			// 男の子と女の子のたまごっちをここで設定する
			cbMan1 = manChara1.GetComponent<CharaBehaviour> ();				// 男の子
			cbWoman1 = womanChara1.GetComponent<CharaBehaviour> ();			// 女の子
			yield return cbMan1.init (new TamaChara (16));
			yield return cbWoman1.init (new TamaChara (18));

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
							pos.x -= 20.0f;
							EventEnd.transform.Find ("serif_1").gameObject.transform.localPosition = pos;
						}
					} else {
						posMan1.y = (man_sit.transform.localPosition.y / 48.0f) + 0.5f;
						posWoman1.y = (man_sit.transform.localPosition.y / 48.0f) + 0.5f;
					}
					break;
				}
			case	statusJobCount.marriageJobCount050:
				{
					if ((manXposition == man_walk3.transform.localPosition.x) && (womanXposition == woman_walk3.transform.localPosition.x)) {
						posMan1.y = (EventEnd.transform.Find("bg1").gameObject.transform.localPosition.y / 47.0f) + 1.7f;
						posWoman1.y = (EventEnd.transform.Find("bg1").gameObject.transform.localPosition.y / 47.0f) + 1.7f;
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
						// たまタウンに移行する？
					}
					posMan1.x += 0.025f;
					posWoman1.x -= 0.025f;
					break;
				}
			case	statusJobCount.marriageJobCount070:
				{
					Debug.Log ("たまタウンへ・・・");
					jobCount = statusJobCount.marriageJobCount080;
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
			posMan1 = new Vector3 (-7.0f, -1.5f, 0.0f);
			posWoman1 = new Vector3 (-9.0f, -1.5f, 0.0f);
		}

		private readonly string[] manMessageTable = new string[]{
			"きょうはありがとう（＋ごび）",
			"またあそぼう（＋ごび）",
		};
		private readonly string[] womanMessageTable = new string[]{
			"これからもよろしく（＋ごび）",
			"たのしかった（＋ごび）",
		};

		private void EventEndMessageSet(){
			man_text.GetComponent<Text> ().text = manMessageTable [Random.Range (0, manMessageTable.Length)];
			woman_text.GetComponent<Text> ().text = womanMessageTable [Random.Range (0, womanMessageTable.Length)];
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
