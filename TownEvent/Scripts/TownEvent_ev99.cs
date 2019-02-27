using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

namespace Mix2App.TownEvent{
	public class TownEvent_ev99 : MonoBehaviour,IReceiver,IReadyable {
		[SerializeField] private GameEventHandler GEHandler;
		[SerializeField] private GameObject[] CharaTamago;					// たまごっち（プレイヤー）
		[SerializeField] private GameObject[] CharaTamagoNpc;				// たまごっち（ナイナイ）
		[SerializeField] private GameObject EventBase;
		[SerializeField] private GameObject EventScene;
		[SerializeField] private GameObject EventItem;
		[SerializeField] private GameObject CameraObj;		



		private object[]		mparam;
		private User muser1;
		private CharaBehaviour[] cbCharaTamago = new CharaBehaviour[2];		// プレイヤー
		private CharaBehaviour[] cbCharaTamagoNpc = new CharaBehaviour[2];	// ナイナイ
		private RewardData		mData;



		private readonly string[] MessageTable1 = new string[]{	// オカムラのメッセージ
			"まっとったでぇ！",
			"いま いおうと おもったのに！",
			"それはさておき！",
			"いいもんもってきたで！",
			"ほんのきもちや とっときや",
		};
		private readonly string[] MessageTable2 = new string[]{	// ヤベのメッセージ
			"うちの あいかたが\nおせわになりました。",
			"そんな こうふんせんでも・・・",
			"さておかれたっ！",
			"さすが おかザルっち！",
		};



		void Awake(){
			muser1 = null;
			mready = false;
			TreasureFlag = false;
		}

		public void receive(params object[] parameter){
			mparam = parameter;

			//単体動作テスト用
			//パラメタ詳細は設計書参照
			if (mparam == null) {
				mparam = new object[] {
					new RewardData (),	// 報酬
					4,					// Depth値
				};

				mData = (RewardData)mparam [0];

				mData.kind = RewardKind.ITEM;
				mData.item = new ItemData ();
				mData.item.code = "tg18_as16058_1";
				mData.item.title = "テストアイテム";
				mData.item.kind = 0;
				mData.item.version = "tg18";
			} else {
				mData = (RewardData)mparam [0];
			}

			if (mparam.Length == 1) {
				CameraObj.transform.GetComponent<Camera> ().depth = 2;
			} else {
				CameraObj.transform.GetComponent<Camera> ().depth = (int)mparam [1];
			}

			RewardBehaviour _rb = EventItem.transform.Find("RewardView").gameObject.GetComponent<RewardBehaviour>();
			_rb.init (mData);						// 報酬セット

			StartCoroutine (mStart ());
		}

		private bool mready = false;
		public bool ready(){
			return mready;
		}

		void OnDestroy(){
		}

		void Start(){
		}		
	
		IEnumerator mStart(){
			Debug.Log (" mStart");

			muser1 = ManagerObject.instance.player;		// たまごっち



			EventItem.transform.Find("Button_blue_close").gameObject.GetComponent<Button> ().onClick.AddListener (ButtonCloseClick);



			// プレイヤーたまごっちを設定する
			cbCharaTamago [0] = CharaTamago [0].GetComponent<CharaBehaviour> ();
			cbCharaTamago [1] = CharaTamago [1].GetComponent<CharaBehaviour> ();

			yield return cbCharaTamago [0].init (muser1.chara1);
			if (muser1.chara2 != null) {
				yield return cbCharaTamago [1].init (muser1.chara2);
			}

			cbCharaTamago [0].gotoAndPlay (MotionLabel.IDLE);
			if (muser1.chara2 != null) {
				cbCharaTamago [1].gotoAndPlay (MotionLabel.IDLE);
			} else {
				CharaTamago [1].transform.localPosition = new Vector3 (0, 5000, 0);
			}

			// ナイナイたまごっちを設定する
			cbCharaTamagoNpc [0] = CharaTamagoNpc [0].GetComponent<CharaBehaviour> ();
			cbCharaTamagoNpc [1] = CharaTamagoNpc [1].GetComponent<CharaBehaviour> ();

			yield return cbCharaTamagoNpc [0].init (new TamaChara(53));
			yield return cbCharaTamagoNpc [1].init (new TamaChara(182));

			cbCharaTamagoNpc [0].gotoAndPlay (MotionLabel.IDLE);
			cbCharaTamagoNpc [1].gotoAndPlay (MotionLabel.IDLE);



			StartCoroutine (mainLoop ());
		}

		void Update(){
			if (mready) {
				// ナイナイのキャラの動きを設定する
				TamagochiAnimeMove ("oka", CharaTamagoNpc [0], cbCharaTamagoNpc [0], MotionLabel.IDLE);
				TamagochiAnimeMove ("oka_sa", CharaTamagoNpc [0], cbCharaTamagoNpc [0], MotionLabel.ANGER);
				TamagochiAnimeMove ("oka_hp", CharaTamagoNpc [0], cbCharaTamagoNpc [0], MotionLabel.GLAD1);
				TamagochiAnimeMove ("yab", CharaTamagoNpc [1], cbCharaTamagoNpc [1], MotionLabel.IDLE);
				TamagochiAnimeMove ("yab_sa", CharaTamagoNpc [1], cbCharaTamagoNpc [1], MotionLabel.ANGER);
				TamagochiAnimeMove ("yab_ku", CharaTamagoNpc [1], cbCharaTamagoNpc [1], MotionLabel.GLAD1);

				// ふきだしのメッセージを設定する
				FukidashiTextSet ();

				// 宝箱の動きを設定する
				TreasureAnimeMove ("takara",false);
				TreasureAnimeMove ("takara_op",true);
			}
		}

		private void ButtonCloseClick(){
			ManagerObject.instance.sound.playSe (17);
			GEHandler.OnRemoveScene (SceneLabel.TOWN_EVENT);
		}


		private IEnumerator mainLoop(){
			Debug.Log ("mainLoop Start");

			EventBase.SetActive (true);
			EventScene.transform.localPosition = new Vector3 (0, 0, 0);
			mready = true;

			StartCoroutine (mainSELoop ());

			while (true) {
				if (EventBase.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
					EventBase.SetActive (false);
					break;
				}
				yield return null;
			}

			EventItem.transform.localPosition = new Vector3 (0, 0, 0);
			ManagerObject.instance.sound.playSe (23);

			yield return null;
		}

		IEnumerator mainSELoop(){
			yield return new WaitForSeconds (2.2f);
			ManagerObject.instance.sound.playSe (24);
			yield return new WaitForSeconds (0.27f);
			ManagerObject.instance.sound.playSe (24);
			yield return new WaitForSeconds (0.27f);
			ManagerObject.instance.sound.playSe (24);
			yield return new WaitForSeconds (4.5f);

			ManagerObject.instance.sound.playSe (33);

			yield return new WaitForSeconds (13.1f);

			ManagerObject.instance.sound.playSe (24);
			yield return new WaitForSeconds (0.75f);
			ManagerObject.instance.sound.playSe (24);
			yield return new WaitForSeconds (0.37f);
			ManagerObject.instance.sound.playSe (24);
			yield return new WaitForSeconds (0.37f);
			ManagerObject.instance.sound.playSe (24);
		}

		private void TamagochiAnimeMove(string _name,GameObject _obj,CharaBehaviour _cb,string _ml){
			GameObject _objBase;

			_objBase = EventBase.transform.Find (_name).gameObject;
			if (_objBase.activeSelf) {
				_obj.transform.localPosition = _objBase.transform.localPosition;
				_obj.transform.localRotation = _objBase.transform.localRotation;
				_obj.transform.localScale = _objBase.transform.localScale;

				if (_cb.nowlabel != _ml) {
					_cb.gotoAndPlay (_ml);
				}
			}
		}

		private void FukidashiTextSet(){
			if(EventBase.transform.Find("fuki＿oka").gameObject.activeSelf){
				string[] _name = new string[]{"fuki＿oka/Text1","fuki＿oka/Text2","fuki＿oka/Text3","fuki＿oka/Text4","fuki＿oka/Text5"};
				string _mes = "";

				EventScene.transform.Find("fukiL1").gameObject.SetActive(true);
				for(int i = 0;i < _name.Length;i++){
					if(EventBase.transform.Find(_name[i]).gameObject.activeSelf){
						_mes = MessageTable1[i];
					}
				}

				EventScene.transform.Find("fukiL1/Text").gameObject.GetComponent<Text>().text = _mes;
			}
			else{
				EventScene.transform.Find("fukiL1").gameObject.SetActive(false);
			}

			if(EventBase.transform.Find("fuki_yab").gameObject.activeSelf){
				string[] _name = new string[]{"fuki_yab/Text1","fuki_yab/Text2","fuki_yab/Text3","fuki_yab/Text4"};
				string _mes = "";

				EventScene.transform.Find("fukiL2").gameObject.SetActive(true);
				for(int i = 0;i < _name.Length;i++){
					if(EventBase.transform.Find(_name[i]).gameObject.activeSelf){
						_mes = MessageTable2[i];
					}
				}

				EventScene.transform.Find("fukiL2/Text").gameObject.GetComponent<Text>().text = _mes;
			}
			else{
				EventScene.transform.Find("fukiL2").gameObject.SetActive(false);
			}
		}



		private bool TreasureFlag = false;
		private void TreasureAnimeMove (string _name,bool _flag){
			GameObject _objBase = EventBase.transform.Find (_name).gameObject;
			GameObject _obj = EventScene.transform.Find (_name).gameObject;
			if (_objBase.activeSelf) {
				_obj.transform.localPosition = _objBase.transform.localPosition;
				_obj.transform.localRotation = _objBase.transform.localRotation;
				_obj.transform.localScale = _objBase.transform.localScale;

				_obj.SetActive (true);

				if (_flag) {
					if (cbCharaTamago [0].nowlabel != MotionLabel.GLAD1) {
						cbCharaTamago [0].gotoAndPlay (MotionLabel.GLAD1);
						ManagerObject.instance.sound.playSe (30);
					}
					if (muser1.chara2 != null) {
						if (cbCharaTamago [1].nowlabel != MotionLabel.GLAD1) {
							cbCharaTamago [1].gotoAndPlay (MotionLabel.GLAD1);
						}
					}
				} else {
					if (_obj.transform.localPosition.y <= -250.0f) {
						if (!TreasureFlag) {
							TreasureFlag = true;
							ManagerObject.instance.sound.playSe (26);
						}
					}
				}
			} else {
				_obj.SetActive (false);
			}
		}






	}
}
