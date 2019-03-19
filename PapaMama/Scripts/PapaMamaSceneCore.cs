////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;
using UnityEngine.UI;
using Mix2App.UI;
using Mix2App.UI.Elements;
using Mix2App.Lib;
using Mix2App.Lib.View;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;

namespace Mix2App.PapaMama {
    public class PapaMamaSceneCore: MonoBehaviour {
        [SerializeField, Required] private PapaMamaMenuWindow PapaMamaMenuWindowPrefab = null;
//        [SerializeField, Required] private PapaMamaSettingsWindow PapaMamaSettingsWindowPrefab;

        void Start() {
            UIManager.ShowModal(PapaMamaMenuWindowPrefab).AddCloseAction(()=> {
            //UIManager.ShowModal(PapaMamaSettingsWindowPrefab).AddCloseAction(()=> {
                ManagerObject.instance.view.change(SceneLabel.HOME);
            });
        }

    }
}
