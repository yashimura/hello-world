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
    public class TownEvent_tutorial2 : MonoBehaviour, IReceiver, IReadyable
    {
        [SerializeField] private GameEventHandler GEHandler = null;
        [SerializeField] private GameObject CameraObj = null;
        [SerializeField] private GameObject MainObj = null;
        [SerializeField] private Sprite[] ApplitchiImage = null;



        private object[] mparam;
        private User muser1;

        private enum ApplitchiAnimeTable
        {
            NORMAL,                 // 普通
            GUIDE,                  // ガイド
            HAPPY,                  // 喜び
            NORMAL2,                // 普通２
            DUMMY,
        };
        private ApplitchiAnimeTable[] animeFlag = new ApplitchiAnimeTable[6]{
            ApplitchiAnimeTable.DUMMY,ApplitchiAnimeTable.DUMMY,ApplitchiAnimeTable.DUMMY,
            ApplitchiAnimeTable.DUMMY,ApplitchiAnimeTable.DUMMY,ApplitchiAnimeTable.DUMMY,
        };
        private Coroutine[] retApplitchi = new Coroutine[6]{
            null,null,null,null,null,null,
        };
        private bool apritchiFLag = false;

        void Awake()
        {
            mparam = null;
            mready = false;

            for(int i = 0; i < animeFlag.Length; i++)
            {
                animeFlag[i] = ApplitchiAnimeTable.DUMMY;
                retApplitchi[i] = null;
            }
        }

        public void receive(params object[] parameter)
        {
            mparam = parameter;

            //単体動作テスト用
            if (mparam == null)
            {
                mparam = new object[] {
                    new RewardData (),  // 報酬
                    4,					// Depth値
				};
            }

            if (mparam.Length == 2)
            {
                CameraObj.transform.GetComponent<Camera>().depth = (int)mparam[1];
            }
            else
            {
                CameraObj.transform.GetComponent<Camera>().depth = 5;
            }

            StartCoroutine(mStart());
        }

        private bool mready = false;
        public bool ready()
        {
            return mready;
        }

        void OnDestroy()
        {
            Debug.Log("OnDestroy");

            for (int i = 0;i< animeFlag.Length; i++)
            {
                if (retApplitchi[i] != null)
                {
                    StopCoroutine(retApplitchi[i]);
                    retApplitchi[i] = null;
                }

            }
        }

        IEnumerator mStart()
        {
            Debug.Log("mStart");

            muser1 = ManagerObject.instance.player;     // たまごっち



            MainObj.transform.Find("tutorial2/bt_next").gameObject.GetComponent<Button>().onClick.AddListener(PageButtonClick);
            MainObj.transform.Find("tutorial2/bt_back").gameObject.GetComponent<Button>().onClick.AddListener(PageButtonClick);
            MainObj.transform.Find("tutorial2/bt_close").gameObject.GetComponent<Button>().onClick.AddListener(CloseButton2Click);
            MainObj.transform.Find("tutorial3/bt_next").gameObject.GetComponent<Button>().onClick.AddListener(PageButtonClick);
            MainObj.transform.Find("tutorial3/bt_back").gameObject.GetComponent<Button>().onClick.AddListener(PageButtonClick);
            MainObj.transform.Find("tutorial3/bt_close").gameObject.GetComponent<Button>().onClick.AddListener(CloseButton3Click);

            

            if(muser1.utype == UserType.MIX2)
            {
                MainObj.transform.Find("tutorial2").gameObject.SetActive(true);
            }
            else
            {
                MainObj.transform.Find("tutorial3").gameObject.SetActive(true);
            }



            mready = true;



            apritchiFLag = true;



            yield return null;
        }



        void Start()
        {

        }

        void Update()
        {
            if (apritchiFLag)
            {
                string[] apri_image_name = new string[] {
                    "tutorial2/fukidasi/apprich", "tutorial2/fukidasi/apprich_l", "tutorial2/fukidasi/apprich_r",
                    "tutorial3/fukidasi/apprich", "tutorial3/fukidasi/apprich_l", "tutorial3/fukidasi/apprich_r",
                };

                for (int i = 0; i < apri_image_name.Length; i++)
                {
                    GameObject _obj = MainObj.transform.Find(apri_image_name[i]).gameObject;

                    switch (MainObj.transform.Find(apri_image_name[i]).gameObject.GetComponent<Image>().sprite.name)
                    {
                        default:
                            {
                                ApplitchiAnime(i, ApplitchiAnimeTable.NORMAL2,_obj);
                                break;
                            }
                        case "applitchi_4":
                            {
                                ApplitchiAnime(i, ApplitchiAnimeTable.GUIDE,_obj);
                                break;
                            }
                        case "applitchi_5":
                            {
                                ApplitchiAnime(i, ApplitchiAnimeTable.NORMAL,_obj);
                                break;
                            }
                        case "applitchi_6":
                            {
                                ApplitchiAnime(i, ApplitchiAnimeTable.HAPPY,_obj);
                                break;
                            }
                    }
                }

                apritchiFLag = false;
            }
        }



        private void PageButtonClick()
        {
            apritchiFLag = true;
            ManagerObject.instance.sound.playSe(13);
        }

        private void CloseButton2Click()
        {
            ManagerObject.instance.sound.playSe(17);
            GEHandler.OnRemoveScene(SceneLabel.TOWN_EVENT + "_tutorial2");
        }
        private void CloseButton3Click()
        {
            ManagerObject.instance.sound.playSe(17);
            GEHandler.OnRemoveScene(SceneLabel.TOWN_EVENT + "_tutorial2");
        }



        // アプリッチのアニメを変更する
        private void ApplitchiAnime(int num,ApplitchiAnimeTable animeNumber,GameObject ApplitchiChara)
        {
            if (animeFlag[num] != animeNumber)
            {
                Vector3[] _posTable =
                {
                    new Vector3(0, -112, 0),
                    new Vector3(-214, -112, 0),
                    new Vector3(231, -112, 0),
                    new Vector3(0, -112, 0),
                    new Vector3(-214, -112, 0),
                    new Vector3(231, -112,0),
                };
                Vector3[] _sclTable =
                {
                    new Vector3(1, 1, 1),
                    new Vector3(-1, 1, 1),
                    new Vector3(1, 1, 1),
                    new Vector3(1, 1, 1),
                    new Vector3(-1, 1, 1),
                    new Vector3(1, 1, 1),
                };

                Vector3 _pos;
                Vector3 _scl;

                _pos = _posTable[num];
                _scl = _sclTable[num];



                ApplitchiChara.transform.localScale = _scl;
                ApplitchiChara.transform.localPosition = _pos;

                animeFlag[num] = animeNumber;

                if (retApplitchi[num] != null)
                {
                    StopCoroutine(retApplitchi[num]);
                    retApplitchi[num] = null;
                }

                switch (animeFlag[num])
                {
                    case ApplitchiAnimeTable.NORMAL:
                        {   // 普通
                            retApplitchi[num] = StartCoroutine(ApplitchiAnimeNORMAL(ApplitchiChara, _pos));
                            break;
                        }
                    case ApplitchiAnimeTable.GUIDE:
                        {   // ガイド
                            retApplitchi[num] = StartCoroutine(ApplitchiAnimeGUIDE(ApplitchiChara));
                            break;
                        }
                    case ApplitchiAnimeTable.NORMAL2:
                        {   // 普通２
                            retApplitchi[num] = StartCoroutine(ApplitchiAnimeNORMAL2(ApplitchiChara));
                            break;
                        }
                    case ApplitchiAnimeTable.HAPPY:
                        {   // 喜び
                            retApplitchi[num] = StartCoroutine(ApplitchiAnimeHAPPY(ApplitchiChara));
                            break;
                        }
                }
            }
        }

        // 普通
        private IEnumerator ApplitchiAnimeNORMAL(GameObject ApplitchiChara,Vector3 _pos)
        {
            ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[5];

            while (true)
            {
                ApplitchiChara.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                ApplitchiChara.transform.localPosition = new Vector3(_pos.x, _pos.y + 2f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(_pos.x, _pos.y - 2f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(_pos.x, _pos.y + 2f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(_pos.x, _pos.y - 2f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(_pos.x, _pos.y, 0.0f);
                yield return new WaitForSeconds(0.5f);

                ApplitchiChara.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                ApplitchiChara.transform.localPosition = new Vector3(_pos.x, _pos.y + 2f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(_pos.x, _pos.y - 2f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(_pos.x, _pos.y + 2f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(_pos.x, _pos.y - 2f, 0.0f);
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.transform.localPosition = new Vector3(_pos.x, _pos.y, 0.0f);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // ガイド
        private IEnumerator ApplitchiAnimeGUIDE(GameObject ApplitchiChara)
        {
            while (true)
            {
                ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[4];
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[5];
                yield return new WaitForSeconds(0.5f);
            }
        }

        // 普通２
        private IEnumerator ApplitchiAnimeNORMAL2(GameObject ApplitchiChara)
        {
            while (true)
            {
                ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[0];
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[1];
                yield return new WaitForSeconds(0.5f);
            }
        }

        // 喜び
        private IEnumerator ApplitchiAnimeHAPPY(GameObject ApplitchiChara)
        {
            while (true)
            {
                ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[5];
                yield return new WaitForSeconds(0.5f);
                ApplitchiChara.GetComponent<Image>().sprite = ApplitchiImage[6];
                yield return new WaitForSeconds(0.5f);
            }
        }


    }
}

