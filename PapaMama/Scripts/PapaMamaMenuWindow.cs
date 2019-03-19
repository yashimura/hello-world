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
    public class PapaMamaMenuWindow: UIWindow {
        [SerializeField, Required] private Button GPSButton = null;
        [SerializeField, Required] private Button MeetSettingsButton = null;

        [Header("Settings")]
        [SerializeField, Required] private Button SettingsButton = null;
        [SerializeField, Required] private PapaMamaSettingsWindow PapaMamaSettingsWindowPrefab = null;

        public override void Setup() {
            base.Setup();

            ManagerObject.instance.sound.playBgm(1);

            AddButtonTapListner(GPSButton, () => {
                ManagerObject.instance.view.change(SceneLabel.DISCOVER);
            });
            AddButtonTapListner(SettingsButton, () => {
                UIManager.ShowModal(PapaMamaSettingsWindowPrefab);
            });

            AddButtonTapListner(MeetSettingsButton, () =>
             {
                 ManagerObject.instance.view.change(SceneLabel.MINIGAME2, "PapaMama", 2);
             });
        }
    }
}
