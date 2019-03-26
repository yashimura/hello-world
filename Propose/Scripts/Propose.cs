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

namespace Mix2App.Propose{
	public class Propose : MonoBehaviour,IReceiver,IReadyable,IMessageHandler {
        [SerializeField] private GameEventHandler GEHandler = null;
        [SerializeField] private GameObject[] CharaTamago = null;					// たまごっち
		[SerializeField] private GameObject EventRoot = null;
		[SerializeField] private GameObject EventSky = null;						// 背景
		[SerializeField] private GameObject EventWait = null;						// メイン
		[SerializeField] private GameObject EventMiss = null;						// 失敗
        [SerializeField] private GameObject CameraObj = null;



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

		private CharaBehaviour[] cbCharaTamago = new CharaBehaviour[2];		// 

		private float[] xSpd = new float[2];								// 雲の移動スピード
		private float scrnOffX;

        private bool fProposeResult;
        private bool fProposeOff;


        void Awake(){
			Debug.Log ("Propose Awake");
			mparam = null;
			mProposeResult = false;
			mProposeType = 0;
			mUser1 = null;
			mUser2 = null;
			mBrother1 = 0;
			mBrother2 = 0;
			mready = false;
            fProposeResult = true;
            fProposeOff = false;

			xSpd [0] = Random.Range (0.5f, 1.0f);
			xSpd [1] = Random.Range (0.5f, 1.0f);
		}

		public void receive(params object[] parameter){
			Debug.Log ("Propose receive");
			mparam = parameter;

			//単体動作テスト用
			//パラメタ詳細は設計書参照
			if (mparam==null) {
				mparam = new object[] {
					0,														    // int  プロポーズ種類 0=自分が受けた、1=自分がした
                    1,                                                          // int  プロポーズ可否 0=失敗、1=成功（受けた時のみ使用）
					new TestUser(1,UserKind.ANOTHER,UserType.MIX2,1,16,17,0,1), // user 自分
					1,														    // int  自分のキャラ種類
					new TestUser(1,UserKind.ANOTHER,UserType.MIX2,1,23,24,0,1), // user プロポーズ相手
					1,														    // int  プロポーズキャラ種類
                    4,                                                          // int  カメラDepth値
				};
			}

			mProposeType = (int)mparam [0];
            if(mProposeType == 0)
            {
                if ((int)mparam[1] == 0)
                {
                    mProposeResult = false;
                }
                else
                {
                    mProposeResult = true;
                }
            }
            mUser1 = (User)mparam [2];
			mBrother1 = (int)mparam [3];
			mUser2 = (User)mparam [4];
			mBrother2 = (int)mparam [5];
            CameraObj.transform.GetComponent<Camera>().depth = (int)mparam[6];

            StartCoroutine(mStart());
		}

		void Start(){
			
		}
			
		private bool mready = false;
		public bool ready(){
			return mready;
		}

        public void OnReceiveMessage(string from, string message)
        {
            if(from == "Town")
            {
                if(message == "propose_ok")
                {
                    // 成功
                    mProposeResult = true;
                    fProposeResult = false;
                }
                if(message == "propose_ng")
                {
                    // 失敗
                    mProposeResult = false;
                    fProposeResult = false;
                }
            }

        }

        void Destroy(){
			Debug.Log ("Propose Destroy");
		}

		void OnDestroy(){
			Debug.Log ("Propose OnDestroy");
		}

		IEnumerator mStart(){
			// 描画エリアの横幅サイズの取得
			scrnOffX = EventRoot.gameObject.GetComponent<RectTransform> ().sizeDelta.x / 2;

			EventWait.transform.Find("Button_blue_modoru").gameObject.GetComponent<Button> ().onClick.AddListener (ButtonModoruClick);

			cbCharaTamago[0] = CharaTamago[0].GetComponent<CharaBehaviour> ();		// 
			cbCharaTamago[1] = CharaTamago[1].GetComponent<CharaBehaviour> ();		// 

			StartCoroutine(mainLoop());

			yield return null;
		}

		void Update () {
			// たまごっちのアニメを反映させる
            UIFunction.TamagochiImageMove(EventWait, CharaTamago[0], "tamago/charaR/");
            UIFunction.TamagochiImageMove(EventWait, CharaTamago[1], "tamago/charaL/");

			// 雲を移動する
			if (mready) {
				CloudIdouLoop (0);
				CloudIdouLoop (1);
			}
		}

		private void ButtonModoruClick(){
            if (!fProposeOff)
            {
                fProposeOff = true;
                ManagerObject.instance.sound.playSe(17);
                EventWait.transform.Find("Button_blue_modoru").gameObject.SetActive(false);
            }
        }

        private IEnumerator mainLoop(){
			if (mProposeType == 0) {			// 自分がプロポーズを受けた
				userR = mUser2;
				userL = mUser1;

				brotherR = mBrother2;
				brotherL = mBrother1;
			} else {							// 自分がプロポーズをした
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

			mready = true;

			EventSky.SetActive (true);
			EventWait.SetActive (true);
			if (mProposeType == 1) {
                fProposeResult = true;
                GameCall call = new GameCall(CallLabel.SEND_PROPOSE, mUser1, mBrother1, mUser2, mBrother2);
                call.AddListener(SendPropose);
                ManagerObject.instance.connect.send(call);

                // 自分がプロポーズしたのでプロポーズ待機状態を表示する
                EventWait.transform.Find ("fukidashi_left").gameObject.SetActive (true);
				EventWait.transform.Find ("fukidashi_left/text").gameObject.GetComponent<Text> ().text = "・・・";
                yield return WaitTimer(30);
                EventWait.transform.Find ("fukidashi_left").gameObject.SetActive (false);
			}

            if (!fProposeOff)
            {
                // プロポーズ待機状態が終了したので音楽スタートand戻るボタンを消す
                EventWait.transform.Find("Button_blue_modoru").gameObject.SetActive(false);
                ManagerObject.instance.sound.playBgm(16);

                // 右に配置されているたまごっちを移動させる
                yield return StartCoroutine(TamagochiRightIdou(EventWait.transform.Find("tamago/charaR").gameObject));

                if (mProposeResult == true)
                {
                    // 告白が成功したので左に配置されているたまごっちが演出をする（少し前に移動する）
                    yield return StartCoroutine(TamagochiProposeSuccess(EventWait.transform.Find("tamago/charaL").gameObject));

                    Debug.Log("結婚へ・・・");
                    ManagerObject.instance.view.change(SceneLabel.MARRIAGE, mProposeType, mUser1, mBrother1, mUser2, mBrother2);
                }
                else
                {
                    // 告白が失敗したので左に配置されているたまごっちが演出をする
                    yield return StartCoroutine(TamagochiProposeMiss());

                    GEHandler.OnRemoveScene(SceneLabel.PROPOSE);
                }
            }
            else
            {
                // プロポーズ中止されたので終了
                GEHandler.OnRemoveScene(SceneLabel.PROPOSE);
            }
            yield return null;
		}

        void SendPropose(bool success, object data)
        {

        }


        private IEnumerator WaitTimer(int _time)
        {
            float _waitSecTime1 = 0.0f;
            int _waitSecTime2 = _time;

            while (true)
            {
                if (!fProposeResult || fProposeOff)
                {
                    break;
                }

                _waitSecTime1 += 1.0f * Time.deltaTime;
                if (_waitSecTime1 >= 1.0f)
                {
                    _waitSecTime1 -= 1.0f;
                    if (_waitSecTime2 == 0)
                    {
                        break;
                    }
                    else
                    {
                        _waitSecTime2--;
                    }
                }

                yield return null;
            }

            yield return null;
        }



        // 右のたまごっちの告白演出
        private IEnumerator TamagochiRightIdou(GameObject _obj){
			Vector3 _pos = new Vector3 (100, -50, 0);					// 右のたまごっちの移動先

			cbCharaTamago [0].gotoAndPlay (MotionLabel.WALK);
			StartCoroutine (TamagochiWalkSEPlay (cbCharaTamago [0]));

			while (true) {												// 移動処理
				_obj.transform.localPosition = Vector3.MoveTowards (_obj.transform.localPosition, _pos, (40.0f * Time.deltaTime));
				if (_obj.transform.localPosition.x == _pos.x) {
					break;
				}
				yield return null;
			}

			cbCharaTamago [0].gotoAndPlay (MotionLabel.SHY1);

			// 告白メッセージを表示
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

		// 左のたまごっちの告白OK演出
		private IEnumerator TamagochiProposeSuccess(GameObject _obj){
			Vector3 _pos = new Vector3 (-100, -50, 0);					// 左のたまごっちの移動先

			cbCharaTamago [1].gotoAndPlay (MotionLabel.WALK);
			StartCoroutine (TamagochiWalkSEPlay (cbCharaTamago [1]));

			while (true) {												// 移動処理
				_obj.transform.localPosition = Vector3.MoveTowards (_obj.transform.localPosition, _pos, (40.0f * Time.deltaTime));
				if (_obj.transform.localPosition.x == _pos.x) {
					break;
				}
				yield return null;
			}

			// 告白メッセージを消す
			EventWait.transform.Find ("fukidashi_right").gameObject.SetActive (false);

			if (Random.Range (0, 2) == 0) {
				cbCharaTamago [1].gotoAndPlay (MotionLabel.SHY2);
			} else {
				cbCharaTamago [1].gotoAndPlay (MotionLabel.SHY3);
			}
			cbCharaTamago [0].gotoAndPlay (MotionLabel.SHOCK);

			ManagerObject.instance.sound.playJingle (17);

			// 告白OKメッセージを表示
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

			// 告白OKメッセージを消す
			EventWait.transform.Find ("fukidashi_left2").gameObject.SetActive (false);

			// たまごっちを喜び状態にする
			TamagochiRandomGlad (cbCharaTamago [0]);
			TamagochiRandomGlad (cbCharaTamago [1]);

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

		// 左のたまごっちの告白No演出
		private IEnumerator TamagochiProposeMiss(){
			// 告白メッセージを消す
			EventWait.transform.Find ("fukidashi_right").gameObject.SetActive (false);

			// 告白Noメッセージを表示
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

			// ハートの割れる演出
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


		private void TamagochiRandomGlad(CharaBehaviour _cb){
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

		private void CloudIdouLoop (int _num){
			string[] _name = new string[]{ "cloud", "cloud2" };

			GameObject _obj = EventSky.transform.Find (_name[_num]).gameObject;
			Vector3 _pos = _obj.transform.localPosition;
			if (_pos.x >= scrnOffX + 150.0f) {
				_pos.x = -(scrnOffX + 150.0f);
				xSpd[_num] = Random.Range (0.5f, 1.0f);
			}
			_pos.x += (xSpd[_num] * (60 * Time.deltaTime));
			_obj.transform.localPosition = _pos;
		}

	}
}
