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
        [SerializeField, Required] TownProfileWindow TownProfileWindowPrefab;
        [SerializeField] GameEventHandler handler;

		[SerializeField] GameObject[] proposeWindow;
		[SerializeField] GameObject cameraObj;



        object[] mparam;

        void Awake()
        {
            mparam = null;
        }

        /// <summary>
        /// Receive data from scene controller
        /// </summary>
        /// <param name="parameter">
        /// </param>
        public void receive(params object[] parameter) {
            Debug.Log("receive:"+parameter);

            mparam = parameter;

            if (mparam==null)
                mparam = new object[] { ManagerObject.instance.player };
            
			if (mparam.Length == 1) {
				cameraObj.transform.GetComponent<Camera> ().depth = 1;
			} else {
				cameraObj.transform.GetComponent<Camera> ().depth = (int)mparam [1];
			}


            StartCoroutine(mstart());
        }

        IEnumerator mstart()
        {

            AvatarElementSelectWindow.InitData();

            yield return null;



			UIManager.proposeWindowSet (proposeWindow);



            User user = (User)mparam[0];
            UIManager.ShowModal(TownProfileWindowPrefab)
				.SetupUserData(user)
                .AddCloseAction(() => {
                    // Setup back action
                    // Mix2App.Lib.ManagerObject.instance.view.back();

                    handler.OnRemoveScene(SceneLabel.PROFILE_TOWN);
            });
        }
    }
}