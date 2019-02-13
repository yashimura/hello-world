////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;
using Mix2App.Lib.Events;
using Mix2App.Lib.Model;
using Mix2App.Lib;
using System.Collections.Generic;

namespace Mix2App.Propose {
    /// <summary>
    /// Core for propose scene.
    /// Propose scene must contain object of this type.
    /// </summary>
    public class ProposeSceneCore: MonoBehaviour, IReceiver {
        [SerializeField] private ActionManager Actions;
        [SerializeField] private UnityEngine.UI.Button BackButton;
        [SerializeField] private ManagerObject Manager;

        /// <summary>
        /// Recieve data from outer scene
        /// </summary>
        /// <param name="parameter"></param>
        public void receive(params object[] parameter) {

            // Setup back button
            BackButton.onClick.AddListener(Back_onClick);

            if (parameter.Length >= 6) {
                // true: success; 
                bool propose_result = (bool)parameter[0];
                
                // 0: make propose; 1: proposed
                int propose_type = (int)parameter[1];

                User self_user = (User)parameter[2];
                int self_character_type = (int)parameter[3];

                User other_user = (User)parameter[4];
                int other_character_type = (int)parameter[5];

                TamaChara left, right;

                if (propose_type == 1) {
                    right = self_user.GetCharaAt(self_character_type);
                    left = other_user.GetCharaAt(other_character_type);
                } else {
                    left = self_user.GetCharaAt(self_character_type);
                    right = other_user.GetCharaAt(other_character_type);
                }

                Actions.AnimationFinishedEvent.AddListener(()=> {
                    Debug.Log("Finished!");
                    if (propose_result) {
                        Manager.view.change("Marriage", propose_type, self_user, self_character_type, other_user, other_character_type);
                    } else {
                        //manager.view.change("MarriageDate", propose_type, self_user, self_character_type, other_user, other_character_type);
                        Manager.view.change("Town");
                    }
                });

                if (propose_result)
                    Actions.ForceSuccess(left, right);
                else
                    Actions.ForceFail(left, right);
            }

        }

        /// <summary>
        /// Back button
        /// </summary>
        private void Back_onClick() {
            Debug.Log("Pressed back button");
        }
               
        /// <summary>
        /// Hides UI elements.
        /// </summary>
        private void HideUI() {
            BackButton.enabled = false;
        }

        public RectTransform MainClientTransform;

#if false && UNITY_EDITOR // rapid test
        public bool ProposeResult;

        public void Start() {
            float t = tr.rect.width / tr.rect.height;
            Debug.Log(t);
            if (t > 2.0f) {
                t -= 2.0f;
                tr.anchorMin = new Vector2(tr.anchorMin.x, -t / 2.0f);
                tr.anchorMax = new Vector2(tr.anchorMax.x, 1.0f + t / 2.0f);
                Debug.Log("Rect change!");
            }

            List<int> backbodylist = new List<int> { 16, 17, 18, 21, 22, 23, 24, 27, 31, 32, 34, 35, 36, 37, 39, 40, 42, 43, 44 };

            User s_user = new User(0) {
                chara1 = new TamaChara(backbodylist[Random.Range(0, backbodylist.Count)])
            };

            User o_user = new User(0) {
                chara1 = new TamaChara(backbodylist[Random.Range(0, backbodylist.Count)])
            };

            receive(ProposeResult, 0, s_user, 0, o_user, 0);
        }

#else
        public void Start() {
            float t = MainClientTransform.rect.width / MainClientTransform.rect.height;
            if (t > 2.0f) {
                t -= 2.0f;
                MainClientTransform.anchorMin = new Vector2(MainClientTransform.anchorMin.x, -t / 2.0f);
                MainClientTransform.anchorMax = new Vector2(MainClientTransform.anchorMax.x, 1.0f + t / 2.0f);
            }
        }
#endif
    }
}
