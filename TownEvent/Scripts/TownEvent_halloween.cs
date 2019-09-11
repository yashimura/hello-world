﻿using System.Collections;
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
    public class TownEvent_halloween : MonoBehaviour, IReceiver, IReadyable
    {
        [SerializeField] private GameEventHandler GEHandler = null;
        [SerializeField] private GameObject[] CharaTamago = null;                   // たまごっち（プレイヤー）
        [SerializeField] private GameObject EventBase = null;
        [SerializeField] private GameObject EventScene = null;
        [SerializeField] private GameObject EventItem = null;
        [SerializeField] private GameObject EventItemWindow = null;
        [SerializeField] private GameObject CameraObj = null;

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
                // ハロウィンっちの動きを設定する
                SpriteAnimeMove("Halloweentti", "Halloweentti");
                // ハロウィンっちのスプライトを設定する
                SpriteImageMove("Halloweentti/kitty_no", "Halloweentti/chara");

                // ふきだしのメッセージを設定する
                FukidashiTextSet();

                // 宝箱の動きを設定する
                TreasureAnimeMove("takara", false);
                TreasureAnimeMove("takara_op", true);

                // 煙の動きと色を設定する
                SpriteColorMove("kemuri1", "kemuri1");
                SpriteColorMove("kemuri_s1", "kemuri_s1");
                SpriteColorMove("kemuri_s2", "kemuri_s2");
                SpriteColorMove("kemuri_s3", "kemuri_s3");
                SpriteColorMove("kemuri_s4", "kemuri_s4");

            }
        }

        private void ButtonCloseClick()
        {
            ManagerObject.instance.sound.playSe(17);
            GEHandler.OnRemoveScene(SceneLabel.TOWN_EVENT + "_halloween");
        }

        private IEnumerator mainLoop()
        {
            Debug.Log("mainLoop Start");

            EventBase.SetActive(true);
            EventScene.transform.localPosition = new Vector3(0, 0, 0);

            //39:18や21:9などの縦長スクリーンサイズに対応する
            //アスペクト比に応じて、画面のscaleを調整する。
            float kk1 = 1024f / 2048f;
            float kk2 = (float)Screen.height / (float)Screen.width;
            float kk3 = (kk2 < kk1) ? kk2 / kk1 : 1.0f;
            EventScene.transform.localScale = new Vector3(kk3, kk3, 1);
            EventItemWindow.transform.localScale = new Vector3(kk3, kk3, 1);

            mready = true;
            StartCoroutine(mainSELoop());

            while (true)
            {
                if (EventBase.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    EventBase.SetActive(false);
                    break;
                }
                yield return null;
            }

            EventItem.transform.localPosition = new Vector3(0, 0, 0);
            ManagerObject.instance.sound.playSe(23);

            yield return null;
        }

        private void SpriteImageMove(string _name,string _name2)
        {
            GameObject _obj = EventScene.transform.Find(_name2).gameObject;
            GameObject _objBase = EventBase.transform.Find(_name).gameObject;

            _obj.gameObject.GetComponent<Image>().sprite = _objBase.gameObject.GetComponent<Image>().sprite;

        }

        private void SpriteAnimeMove(string _name, string _name2)
        {
            GameObject _obj = EventScene.transform.Find(_name2).gameObject;
            GameObject _objBase = EventBase.transform.Find(_name).gameObject;
            _obj.transform.localPosition = _objBase.transform.localPosition;
            _obj.transform.localRotation = _objBase.transform.localRotation;
            _obj.transform.localScale = _objBase.transform.localScale;
            _obj.SetActive(_objBase.activeSelf);
        }

        private void SpriteColorMove(string _name,string _name2)
        {
            GameObject _obj = EventScene.transform.Find(_name2).gameObject;
            GameObject _objBase = EventBase.transform.Find(_name).gameObject;
            _obj.gameObject.GetComponent<Image>().color = _objBase.gameObject.GetComponent<Image>().color;

            SpriteAnimeMove(_name, _name2);
        }

        private void FukidashiTextSet()
        {
            if (EventBase.transform.Find("fuki_kitty").gameObject.activeSelf)
            {
                string[] _name = new string[] { "fuki_kitty/Text1", "fuki_kitty/Text2", "fuki_kitty/Text3" };
                string _mes = "";
                TextAnchor _mes_ag = TextAnchor.MiddleCenter;

                EventScene.transform.Find("fuki").gameObject.SetActive(true);
                for (int i = 0; i < _name.Length; i++)
                {
                    if (EventBase.transform.Find(_name[i]).gameObject.activeSelf)
                    {
                        _mes = EventBase.transform.Find(_name[i]).gameObject.GetComponent<Text>().text;
                        _mes_ag = EventBase.transform.Find(_name[i]).gameObject.GetComponent<Text>().alignment;
                    }
                }

                EventScene.transform.Find("fuki/Text").gameObject.GetComponent<Text>().text = _mes;
                EventScene.transform.Find("fuki/Text").gameObject.GetComponent<Text>().alignment = _mes_ag;
            }
            else
            {
                EventScene.transform.Find("fuki").gameObject.SetActive(false);
            }
        }

        private void TreasureAnimeMove(string _name, bool _flag)
        {
            GameObject _objBase = EventBase.transform.Find(_name).gameObject;
            GameObject _obj = EventScene.transform.Find(_name).gameObject;
            if (_objBase.activeSelf)
            {
                _obj.transform.localPosition = _objBase.transform.localPosition;
                _obj.transform.localRotation = _objBase.transform.localRotation;
                _obj.transform.localScale = _objBase.transform.localScale;

                _obj.SetActive(true);

                if (_flag)
                {
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
                    if (!TreasureFlag)
                    {
                        TreasureFlag = true;
                        ManagerObject.instance.sound.playSe(22);
                    }
                }
            }
            else
            {
                _obj.SetActive(false);
            }
        }

        IEnumerator mainSELoop()
        {
            yield return new WaitForSeconds(3.0f);
            ManagerObject.instance.sound.playSe(24);
            yield return new WaitForSeconds(6.85f);
            ManagerObject.instance.sound.playSe(24);
        }
    }
}
