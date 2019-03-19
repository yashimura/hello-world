////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Mix2App.UI;
using Mix2App.UI.Elements;
using Mix2App.Lib;
using Mix2App.Lib.View;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;

namespace Mix2App.PapaMama {
    public class PapaMamaSettingsWindow: UIWindow {
        [SerializeField, Required] private Toggle[] Group0 = null;
        [SerializeField, Required] private Toggle[] Group1 = null;
        [SerializeField, Required] private Toggle[] Group2 = null;

        private class ToggleGroup {
            public int Current = 0;
            public ToggleGroup(Toggle[] group, int current, UnityAction<bool> callback) {
                foreach (var item in group)
                    item.isOn = false;
                    
                Current = current;
                group[Current].isOn = true;

                for (int i = 0; i < group.Length; i++) {
                    int l = i;
                    group[l].onValueChanged.AddListener((state) => {
                        //Debug.Log(state+","+Current+","+l);
                        if (state) {
                            if (Current != l) {
                                // change first
                                Current = l;
                                // off non-current
                                for (int j = 0; j < group.Length; j++) {
                                    if (l!=j) group[j].isOn = false;
                                }
                                // save when current value changes 
                                callback(state);
                            }
                            PlaySE(SE_Choice);
                        }
                        else if (Current == l)
                        {
                            // current cant be turned off
                            group[Current].isOn = true;
                        }
                    });
                }


            }
        }

        void savesettings(bool state)
        {
            GameCall call = new GameCall(CallLabel.SET_PAPA_MAMA, Groups[0].Current,Groups[1].Current,Groups[2].Current);
            ManagerObject.instance.connect.send(call);
        }

        ToggleGroup[] Groups;

        public override void Setup() {
            base.Setup();

            // default value setting
            int g1 = ManagerObject.instance.app.enabledSearchTarget ? 1: 0;
            int g2 = ManagerObject.instance.app.enabledViewSearchFriend ? 1: 0;
            int g3 = ManagerObject.instance.app.enabledStampArea;

            Groups = new ToggleGroup[3];
            Groups[0] = new ToggleGroup(Group0,g1,savesettings);
            Groups[1] = new ToggleGroup(Group1,g2,savesettings);
            Groups[2] = new ToggleGroup(Group2,g3,savesettings);
            //AddCloseAction(() => {
                //GameCall call = new GameCall(CallLabel.SET_PAPA_MAMA, Groups[0].Current,Groups[1].Current,Groups[2].Current);
                //ManagerObject.instance.connect.send(call);
            //});
        }
    }
}
