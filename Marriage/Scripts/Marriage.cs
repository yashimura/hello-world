using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

namespace Mix2App.Marriage{
	public class Marriage : MonoBehaviour,IReceiver {
		[SerializeField] private GameObject CameraObj;
		[SerializeField] private GameObject CameraObjMarriage;
		[SerializeField] private GameObject CameraObjWait;

		[SerializeField] private GameObject EventMarriage;
		[SerializeField] private GameObject	EventWait;

		[SerializeField] private GameObject	EventFinale;			// 結婚式
		[SerializeField] private GameObject	EventGarden;			// 庭園
		[SerializeField] private GameObject EventBeach;				// 海岸
		[SerializeField] private GameObject EventPark;				// 遊園地

		[SerializeField] private GameObject	man_walk1;				// 男の子、遊園地の歩き
		[SerializeField] private GameObject	man_walk2;				// 男の子、海岸の歩き
		[SerializeField] private GameObject man_sit;				// 男の子、庭園での座り
		[SerializeField] private GameObject	man_happy;				// 男の子、結婚式での喜び

		[SerializeField] private GameObject woman_walk1;			// 女の子、遊園地の歩き
		[SerializeField] private GameObject woman_walk2;			// 女の子、海岸の歩き
		[SerializeField] private GameObject woman_sit;				// 女の子、庭園での座り
		[SerializeField] private GameObject woman_happy;			// 女の子、結婚式での喜び

		[SerializeField] private GameObject omedetouPosition;
		[SerializeField] private GameObject man_happy2;				// 男の子、結婚式の双子の子
		[SerializeField] private GameObject woman_happy2;			// 女の子、結婚式の双子の子
		[SerializeField] private GameObject[] petChara;				


		[SerializeField] private GameObject manChara1;				// 男の子
		[SerializeField] private GameObject womanChara1;			// 女の子
		[SerializeField] private GameObject manChara2;				// 男の子の双子
		[SerializeField] private GameObject womanChara2;			// 女の子の双子

		[SerializeField] private GameObject petChara1;				// 男の子のペット
		[SerializeField] private GameObject petChara2;				// 女の子のペット


		[SerializeField] private GameObject PanelWhite;


		private object[]		mparam;

		private float 			panelWhiteA;

		private CharaBehaviour	cbMan1;								// 男の子キャラ
		private CharaBehaviour	cbMan2;								// 男の子の双子キャラ
		private CharaBehaviour	cbWoman1;							// 女の子キャラ
		private CharaBehaviour	cbWoman2;							// 女の子の双子キャラ
		private PetBehaviour	pbPet1;								// ペット１
		private PetBehaviour	pbPet2;								// ペット２

		private Vector3 posMan1 = new Vector3(-7.0f,-1.5f,0.0f);
		private Vector3 posWoman1 = new Vector3 (-9.0f, -1.5f, 0.0f);
		private Vector3 posMan2 = new Vector3 (1.2f, 50.0f, 0.0f);
		private Vector3 posWoman2 = new Vector3 (-1.2f, 50.0f, 0.0f);
		private Vector3 posOmedetou = new Vector3 (0.0f, 0.0f, 0.0f);
		private Vector3 posPet1 = new Vector3 (0.0f, 0.0f, 0.0f);
		private Vector3 posPet2 = new Vector3 (0.0f, 0.0f, 0.0f);

		private bool startEndFlag = false;
		private int waitCount;
		private bool waitFlag = false;
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
			Debug.Log ("Marriage Awake");
		}

		public void receive(params object[] parameter){
			Debug.Log ("Marriage receive");
			mparam = parameter;
		}

		IEnumerator Start () {
			Debug.Log ("Marriage start");

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
			man_happy2.SetActive (false);
			woman_happy2.SetActive (false);
			for (int i = 0; i < 4; i++) {
				petChara [i].SetActive (false);
			}


			// 表示位置初期化
			posInit ();


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
				EventMarriage.GetComponent<Transform> ().transform.localScale = new Vector3 (1.275f, 1.275f, 1.0f);
				EventWait.GetComponent<Transform> ().transform.localScale = new Vector3 (1.275f, 1.275f, 1.0f);
			}



			// 男の子と女の子のたまごっちをここで設定する

			cbMan1 = manChara1.GetComponent<CharaBehaviour> ();				// 男の子
			cbMan2 = manChara2.GetComponent<CharaBehaviour> ();				// 男の子の双子
			cbWoman1 = womanChara1.GetComponent<CharaBehaviour> ();			// 女の子
			cbWoman2 = womanChara2.GetComponent<CharaBehaviour> ();			// 女の子の双子
			yield return cbMan1.init (new TamaChara (16));
			yield return cbMan2.init (new TamaChara (17));
			yield return cbWoman1.init (new TamaChara (18));
			yield return cbWoman2.init (new TamaChara (19));

			// ペットをここで設定する

			pbPet1 = petChara1.GetComponent<PetBehaviour> ();				// 男の子のペット
			pbPet2 = petChara2.GetComponent<PetBehaviour> ();				// 女の子のペット
			yield return pbPet1.init (new TamaPet (700));
			yield return pbPet2.init (new TamaPet (701));

			// 男の子に双子がいなければ、cbMan2.SetActive(false);
			// 女の子に双子がいなければ、cbWoman2.SetActive(false);
			// 男の子にペットがいなければ、pbPet1.SetActive(false);
			// 女の子にペットがいなければ、pbPet2.SeteActive(false);

			startEndFlag = true;
		}

		void Destroy()
		{
			Debug.Log("Marriage Destroy");
		}

		void Update () {
			switch (jobCount) {
			case	statusJobCount.marriageJobCount000:
				{
					if (startEndFlag) {
						waitCount--;
						if (waitCount == 0) {
							StartCoroutine ("Wait60");

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
					EventFinale.GetComponent<Transform> ().transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
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
						EventFinale.GetComponent<Transform> ().transform.localScale = new Vector3 (0.8574491f, 0.8574491f, 0.0f);
						jobCount = statusJobCount.marriageJobCount050;

						Debug.Log ("結婚式");

						// 結婚式場で整列
						cbMan1.gotoAndPlay ("idle");
						cbWoman1.gotoAndPlay ("idle");
						cbMan2.gotoAndPlay ("idle");
						cbWoman2.gotoAndPlay ("idle");
						pbPet1.gotoAndPlay ("idle");
						pbPet2.gotoAndPlay ("idle");
						cbMan1.GetComponent<SpriteRenderer> ().flipX = false;
						cbWoman1.GetComponent<SpriteRenderer> ().flipX = true;
						cbMan2.GetComponent<SpriteRenderer> ().flipX = false;
						cbWoman2.GetComponent<SpriteRenderer> ().flipX = true;
						pbPet1.GetComponent<SpriteRenderer> ().flipX = false;
						pbPet2.GetComponent<SpriteRenderer> ().flipX = true;

						posOmedetou = omedetouPosition.transform.localPosition;
					} else {
						posMan1.y = (man_sit.transform.localPosition.y / 48.0f) + 0.5f;
						posWoman1.y = (man_sit.transform.localPosition.y / 48.0f) + 0.5f;
					}
					break;
				}
			case	statusJobCount.marriageJobCount050:
				{
					if (EventFinale.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						jobCount = statusJobCount.marriageJobCount060;

						posMan1.y = 500.0f;					// 男の子を画面外に
						posMan2.y = 500.0f;					// 女の子を画面外に
						posWoman1.y = 500.0f;				// 男の子の双子を画面外に
						posWoman2.y = 500.0f;				// 女の子の双子を画面外に

						posPet1.y = 500.0f;					// ペット１を画面外に
						posPet2.y = 500.0f;					// ペット２を画面外に
					} else {
						posMan1.y = (man_happy.transform.localPosition.y / 47.0f) + 1.2f;
						posWoman1.y = (woman_happy.transform.localPosition.y / 47.0f) + 1.2f;

						posMan2.y = (man_happy.transform.localPosition.y / 47.0f) + 4.25f;
						posWoman2.y = (woman_happy.transform.localPosition.y / 47.0f) + 4.25f;

						posPet1.y = (man_happy.transform.localPosition.y / 47.0f) + 3.35f;
						posPet2.y = (woman_happy.transform.localPosition.y / 47.0f) + 3.35f;

						if (posOmedetou.y != omedetouPosition.transform.localPosition.y) {
							posMan2.y += ((omedetouPosition.transform.localPosition.y - posOmedetou.y) / 50.0f);
							posWoman2.y += ((omedetouPosition.transform.localPosition.y - posOmedetou.y) / 50.0f);

							posPet1.y += ((omedetouPosition.transform.localPosition.y - posOmedetou.y) / 80.0f);
							posPet2.y += ((omedetouPosition.transform.localPosition.y - posOmedetou.y) / 80.0f);
						}
					}
					break;
				}
			case	statusJobCount.marriageJobCount060:
				{
					MarriageJobTypeEnding ();
					jobCount = statusJobCount.marriageJobCount070;
					break;
				}
			case	statusJobCount.marriageJobCount070:
				{
					// ホワイトイン
					panelWhiteA -= 5.0f;
					PanelWhite.GetComponent<Image> ().color = new Color (1.0f, 1.0f, 1.0f, panelWhiteA / 255.0f);
					if (panelWhiteA == 0) {
						jobCount = statusJobCount.marriageJobCount080;
					}
					break;
				}
			case	statusJobCount.marriageJobCount080:
				{
					if (waitFlag) {
						waitCount = 1;
						jobCount = statusJobCount.marriageJobCount000;
					} else {
//						if (たまごっちとの通信終了) {
//							Debug.Log("たまタウンへ・・・");
//							jobCount = statusJobCount.marriageJobCount090;
//						}
					}

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
			manChara2.transform.localPosition = posMan2;
			womanChara1.transform.localPosition = posWoman1;
			womanChara2.transform.localPosition = posWoman2;

			petChara1.transform.localPosition = posPet1;
			petChara2.transform.localPosition = posPet2;
		}


		private void MarriageJobTypeOpening(){
			EventMarriage.SetActive (true);
			EventWait.SetActive (false);
			CameraObj.SetActive (false);
			CameraObjMarriage.SetActive (true);
			PanelWhite.SetActive (false);
		}


		private void MarriageJobTypeEnding(){
			EventWait.SetActive (true);
			EventMarriage.SetActive (false);
			CameraObjMarriage.SetActive (false);
			CameraObjWait.SetActive (true);
			PanelWhite.SetActive (true);
			panelWhiteA = 255.0f;
			PanelWhite.GetComponent<Image> ().color = new Color (1.0f, 1.0f, 1.0f, panelWhiteA / 255.0f);
		}

		private void posInit(){
			posMan1 = new Vector3 (-7.0f, -1.5f, 0.0f);
			posWoman1 = new Vector3 (-9.0f, -1.5f, 0.0f);
			posMan2 = new Vector3 (1.2f, 50.0f, 0.0f);
			posWoman2 = new Vector3 (-1.2f, 50.0f, 0.0f);

			posPet1 = new Vector3 (1.1f, 50.0f, 0.0f);
			posPet2 = new Vector3 (-1.1f, 50.0f, 0.0f);
		}


		private IEnumerator Wait60(){
			waitFlag = false;
			yield return new WaitForSeconds (60.0f);
			waitFlag = true;
		}
	}
}
