////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using System.Collections;
using UnityEngine;
using Mix2App.Lib;
using Mix2App.Lib.Events;
using Mix2App.UI;
using Mix2App.Lib.Model;

namespace Mix2App.Profile {
    /// <summary>
    /// Profile scene core
    /// </summary>
    public class SceneCore: MonoBehaviour, IReceiver {
        [SerializeField, Required] SelfProfileWindow SelfProfileWindowPrefab;

        object[] mparam;        
        private void CloseAction() {
            ManagerObject.instance.view.back();
        }

        private User GetSampleUser() {
            return new TestUser(901, UserKind.ANOTHER, UserType.MIX2, 0, 16, 0, 1, 1);
        }

        void Awake()
        {
            mparam=null;
        }

        /// <summary>
        /// Receive data from scene controller
        /// </summary>
        /// <param name="parameter">
        /// </param>
        public void receive(params object[] parameter) {
            Debug.Log("receive");

            if (mparam != null)
                mparam = parameter;
            else
                mparam = new object[] { ManagerObject.instance.player };
        }

        IEnumerator Start()
        {
            while (mparam==null)
                yield return null;

            AvatarElementSelectWindow.InitData();

            yield return null;

            User user = (User)mparam[0];
            if (user != null)
                UIManager.ShowModal(SelfProfileWindowPrefab)
                    .SetupUserData(user)
                    .AddCloseAction(CloseAction);
            else {
                int user_id = (int)mparam[0];
                GameCall call = new GameCall(CallLabel.GET_USER, user_id);
                call.AddListener((bool success, object data) => {
                    user = (User)data;
                    UIManager.ShowModal(SelfProfileWindowPrefab)
                            .SetupUserData(user)
                            .AddCloseAction(CloseAction);
                });
                ManagerObject.instance.connect.send(call);
            }
        }
    }
}
