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

namespace Mix2App.Profile.Town {
    public class TownSceneCore: MonoBehaviour, IReceiver {
        [SerializeField, Required] TownProfileWindow TownProfileWindowPrefab = null;
        [SerializeField] GameEventHandler handler = null;

		[SerializeField] GameObject[] proposeWindow = null;
		[SerializeField] GameObject cameraObj = null;


        User muser;
        int mbgmid;
        bool mproposeflag;

        void Awake()
        {

        }

        void OnDestroy()
        {
            tpwindow.ProposeCallBackDel();
        }


        /// <summary>
        /// Receive data from scene controller
        /// </summary>
        /// <param name="parameter">
        /// </param>
        public void receive(params object[] parameter) {
            Debug.Log("receive:"+parameter);

            object[] mparam;
            if (parameter == null) mparam = new object[] { };
            else mparam = parameter;

            if (mparam.Length >= 1 && mparam[0] is User)
                muser = (User)mparam[0];
            else
                muser = ManagerObject.instance.player;

            if (mparam.Length >= 2 && mparam[1] is int)
                cameraObj.transform.GetComponent<Camera>().depth = (int)mparam[1];
            else
                cameraObj.transform.GetComponent<Camera>().depth = 2;

            if (mparam.Length >= 3 && mparam[2] is bool)
                mproposeflag = (bool)mparam[2];
            else
                mproposeflag = false;

            if (mparam.Length >= 4 && mparam[3] is int)
                mbgmid = (int)mparam[3];
            else
                mbgmid = 2;//春テーマ

            StartCoroutine(mstart());
        }

        private TownProfileWindow tpwindow;

        IEnumerator mstart()
        {

            AvatarElementSelectWindow.InitData();

            yield return null;



            UIManager.GEHandlerSet(handler);
            UIManager.proposeWindowSet(proposeWindow);
            UIManager.cameraWindowSet(cameraObj);
            UIManager.proposeFlagSet(mproposeflag);


            tpwindow = UIManager.ShowModal(TownProfileWindowPrefab);
            tpwindow.ProposeCallBackAdd();
            tpwindow.TownBgmId = mbgmid;
            tpwindow.SetupUserData(muser)
                    .AddCloseAction(() => {
                    // Setup back action
                    // Mix2App.Lib.ManagerObject.instance.view.back();

                    handler.OnRemoveScene(SceneLabel.PROFILE_TOWN);
            });

            UIManager.GEHandlerSet(handler);
        }

        public static void SceneClose()
        {
            UIManager.GEHandlerGet().OnRemoveScene(SceneLabel.PROFILE_TOWN);
        }
    }
}