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
	public class TownEvent_easter : MonoBehaviour,IReceiver,IReadyable {
		[SerializeField] private GameEventHandler GEHandler;
		[SerializeField] private GameObject[] CharaTamago;					// たまごっち（プレイヤー）
		[SerializeField] private GameObject CharaTamagoNpc;					// たまごっち（カラフルっち）
		[SerializeField] private GameObject EventBase;
		[SerializeField] private GameObject EventScene;
		[SerializeField] private GameObject EventItem;
		[SerializeField] private GameObject CameraObj;		



		private object[]		mparam;
		private User muser1;
		private CharaBehaviour[] cbCharaTamago = new CharaBehaviour[2];		// プレイヤー
		private CharaBehaviour cbCharaTamagoNpc;							// カラフルっち
		private RewardData		mData;



		private readonly string[] MessageTable1 = new string[]{				// メッセージ
			"ハッピーイースター！",
			"あなたとカラフルなまいにちを\nすごしたい",
			"カラフルラビっちですカラ～！",
			"よいしょっと・・・",
			"プレゼントなんだカラ～♪",
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
			if (mparam==null) {
				mparam = new object[] {
					1,					// 報酬ID
					4,					// Depth値
				};
			}
			if (mparam.Length == 1) {
				CameraObj.transform.GetComponent<Camera> ().depth = 2;
			} else {
				CameraObj.transform.GetComponent<Camera> ().depth = (int)mparam [1];
			}

			GameCall call = new GameCall (CallLabel.GET_EVENT_REWARD,mparam[0]);
			call.AddListener (mGetEventReward);
			ManagerObject.instance.connect.send (call);

			StartCoroutine (mStart ());
		}
		void mGetEventReward(bool success,object data){
			Debug.Log(success + "/" + data);
			if (success) {
				mData = (RewardData)data;

				RewardBehaviour _rb = EventItem.transform.Find("RewardView").gameObject.GetComponent<RewardBehaviour>();
				_rb.init (mData);
			}
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

			// カラフルっちたまごっちを設定する
			cbCharaTamagoNpc = CharaTamagoNpc.GetComponent<CharaBehaviour> ();

			yield return cbCharaTamagoNpc.init (new TamaChara(183));

			cbCharaTamagoNpc.gotoAndPlay (MotionLabel.IDLE);



			StartCoroutine (mainLoop ());
		}

		void Update(){
			if (mready) {
				// カラフルっちのキャラの動きを設定する
				TamagochiAnimeMove ("color_no", CharaTamagoNpc, cbCharaTamagoNpc, MotionLabel.IDLE);
				TamagochiAnimeMove ("color_yo", CharaTamagoNpc, cbCharaTamagoNpc, MotionLabel.GLAD2);
				TamagochiAnimeMove ("color_cl", CharaTamagoNpc, cbCharaTamagoNpc, MotionLabel.IDLE);
				TamagochiAnimeMove ("color_wo", CharaTamagoNpc, cbCharaTamagoNpc, MotionLabel.SHY2);
				TamagochiAnimeMove ("color_bi", CharaTamagoNpc, cbCharaTamagoNpc, MotionLabel.SHOCK);

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
			yield return new WaitForSeconds (3.0f);

			ManagerObject.instance.sound.playSe (24);
			yield return new WaitForSeconds (0.34f);
			ManagerObject.instance.sound.playSe (24);
			yield return new WaitForSeconds (0.34f);
			ManagerObject.instance.sound.playSe (24);
			yield return new WaitForSeconds (0.34f);
			ManagerObject.instance.sound.playSe (24);

			yield return new WaitForSeconds (6.2f);

			ManagerObject.instance.sound.playSe (24);
			yield return new WaitForSeconds (0.3f);
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
			if(EventBase.transform.Find("fuki").gameObject.activeSelf){
				string[] _name = new string[]{"fuki/Text1","fuki/Text2","fuki/Text3","fuki/Text4","fuki/Text5"};
				string _mes = "";

				EventScene.transform.Find("fuki").gameObject.SetActive(true);
				for(int i = 0;i < _name.Length;i++){
					if(EventBase.transform.Find(_name[i]).gameObject.activeSelf){
						_mes = MessageTable1[i];
					}
				}

				EventScene.transform.Find("fuki/Text").gameObject.GetComponent<Text>().text = _mes;
			}
			else{
				EventScene.transform.Find("fuki").gameObject.SetActive(false);
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
					if (!TreasureFlag) {
						TreasureFlag = true;
						ManagerObject.instance.sound.playSe (26);
					}
				}
			} else {
				_obj.SetActive (false);
			}
		}




			


	}
}
