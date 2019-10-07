using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

namespace Mix2App.TownEvent
{
    [System.Serializable]
    public struct TownEventSETable
    {
        public float time;
        public int number;
    }

    public class TownEventMain : MonoBehaviour, IReceiver, IReadyable
    {
        [SerializeField] private GameEventHandler GEHandler = null;
        [SerializeField] private GameObject[] CharaTamago = null;                   // たまごっち（プレイヤー）
        [SerializeField] private GameObject EventBase = null;
        [SerializeField] private GameObject EventScene = null;
        [SerializeField] private GameObject EventItem = null;
        [SerializeField] private GameObject EventItemWindow = null;
        [SerializeField] private GameObject CameraObj = null;

        [SerializeField] private GameObject TreasureChestClose = null;
        [SerializeField] private GameObject TreasureChestOpen = null;

        [Tooltip("シーン名（TownEventの次に入るもの）（TownEvent_XYZなら_XYZを入力）")]
        [SerializeField] private string TownEventName = null;

        [Tooltip("プレイヤーキャラの表示位置を登録する")]
        [SerializeField] private Vector3 CharaTamagoXYZ = new Vector3(0, 0, 0);

        [Tooltip("効果音の入力データ(Time:次のSEがなるまでの時間、Number:鳴らすSE")]
        [SerializeField] private TownEventSETable[] TownEventSEData = null;


        private object[] mparam;
        private User muser1;
        private CharaBehaviour[] cbCharaTamago = new CharaBehaviour[2];     // プレイヤー
        private RewardData mData;
        private bool TreasureFlag = false;



        void Awake()
        {
            muser1 = null;
            mready = false;
            TreasureFlag = false;
        }

        public void receive(params object[] parameter)
        {
            mparam = parameter;

            //単体動作テスト用
            //パラメタ詳細は設計書参照
            if (mparam == null)
            {
                mparam = new object[] {
                    new RewardData (),  // 報酬
                    4,                  // Depth値
                };

                mData = (RewardData)mparam[0];

                mData.kind = RewardKind.ITEM;
                mData.item = new ItemData();
                mData.item.code = "tg18_as16058_1";
                mData.item.title = "テストアイテム";
                mData.item.kind = 0;
                mData.item.version = "tg18";
            }
            else
            {
                mData = (RewardData)mparam[0];
            }

            if (mparam.Length == 1)
            {
                CameraObj.transform.GetComponent<Camera>().depth = 2;
            }
            else
            {
                CameraObj.transform.GetComponent<Camera>().depth = (int)mparam[1];
            }

            RewardBehaviour _rb = EventItemWindow.transform.Find("RewardView").gameObject.GetComponent<RewardBehaviour>();
            _rb.init(mData);                       // 報酬セット

            StartCoroutine(mStart());
        }

        private bool mready = false;
        public bool ready()
        {
            return mready;
        }

        void OnDestroy()
        {
        }

        void Start()
        {
        }

        IEnumerator mStart()
        {
            Debug.Log(" mStart");

            muser1 = ManagerObject.instance.player;     // たまごっち



            EventItemWindow.transform.Find("Button_blue_close").gameObject.GetComponent<Button>().onClick.AddListener(ButtonCloseClick);



            // たまごっちの表示位置の登録
            EventScene.transform.Find("Chara").transform.localPosition = CharaTamagoXYZ;



            // プレイヤーたまごっちを設定する
            cbCharaTamago[0] = CharaTamago[0].GetComponent<CharaBehaviour>();
            cbCharaTamago[1] = CharaTamago[1].GetComponent<CharaBehaviour>();

            yield return cbCharaTamago[0].init(muser1.chara1);
            if (muser1.chara2 != null)
            {
                yield return cbCharaTamago[1].init(muser1.chara2);
            }

            cbCharaTamago[0].gotoAndPlay(MotionLabel.IDLE);
            if (muser1.chara2 != null)
            {
                cbCharaTamago[1].gotoAndPlay(MotionLabel.IDLE);
            }
            else
            {
                CharaTamago[1].transform.localPosition = new Vector3(0, 5000, 0);
            }

            StartCoroutine(mainLoop());
        }

        void Update()
        {
            if (mready)
            {
                // 宝箱からたまごっちの動きを設定する
                TreasureTamagoAnimeMove(TreasureChestClose, false);
                TreasureTamagoAnimeMove(TreasureChestOpen, true);
            }
        }

        private void ButtonCloseClick()
        {
            ManagerObject.instance.sound.playSe(17);
            GEHandler.OnRemoveScene(SceneLabel.TOWN_EVENT + TownEventName);
        }

        private IEnumerator mainLoop()
        {
            Debug.Log("mainLoop Start");

            //39:18や21:9などの縦長スクリーンサイズに対応する
            //アスペクト比に応じて、画面のscaleを調整する。
            float kk1 = 1024f / 2048f;
            float kk2 = (float)Screen.height / (float)Screen.width;
            float kk3 = (kk2 < kk1) ? kk2 / kk1 : 1.0f;
            EventBase.transform.localScale = new Vector3(kk3, kk3, 1);
            EventScene.transform.localScale = new Vector3(kk3, kk3, 1);
            EventItemWindow.transform.localScale = new Vector3(kk3, kk3, 1);

            EventBase.SetActive(true);

            mready = true;
            StartCoroutine(mainSELoop());

            while (true)
            {
                if (EventBase.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    EventBase.GetComponent<Animator>().enabled = false;
                    break;
                }
                yield return null;
            }

            EventItem.transform.localPosition = new Vector3(0, 0, 0);
            ManagerObject.instance.sound.playSe(23);

            yield return null;
        }

        /// <summary>
        /// 宝箱のスプライトが表示状態かを調べてたまごっちの動きを設定する
        /// </summary>
        /// <param name="_objBase"></param>
        /// <param name="_flag"></param>
        private void TreasureTamagoAnimeMove(GameObject _objBase, bool _flag)
        {
            if (_objBase.activeSelf)
            {
                if (_flag)
                {
                    // たまごっちを喜ばして宝箱開封の効果音を鳴らす
                    if (cbCharaTamago[0].nowlabel != MotionLabel.GLAD1)
                    {
                        cbCharaTamago[0].gotoAndPlay(MotionLabel.GLAD1);

                        ManagerObject.instance.sound.playSe(18);
                    }
                    if (muser1.chara2 != null)
                    {
                        if (cbCharaTamago[1].nowlabel != MotionLabel.GLAD1)
                        {
                            cbCharaTamago[1].gotoAndPlay(MotionLabel.GLAD1);
                        }
                    }
                }
                else
                {
                    // 宝箱出現の効果音を鳴らす
                    if (!TreasureFlag)
                    {
                        TreasureFlag = true;
                        ManagerObject.instance.sound.playSe(22);
                    }
                }
            }
        }

        /// <summary>
        /// SEをTownEventSEDataの情報を元に鳴らす。
        /// </summary>
        /// <returns></returns>
        IEnumerator mainSELoop()
        {
            for(int i = 0; i < TownEventSEData.Length; i++)
            {
                yield return new WaitForSeconds(TownEventSEData[i].time);
                ManagerObject.instance.sound.playSe(TownEventSEData[i].number);
            }

            yield return null;
        }
    }
}
