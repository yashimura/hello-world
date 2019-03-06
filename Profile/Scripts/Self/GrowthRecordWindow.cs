////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using Mix2App.UI;
using UnityEngine;
using Mix2App.Lib;
using Mix2App.Lib.View;
using Mix2App.Lib.Model;
using Mix2App.UI.Dialogs;
using Mix2App.Profile.Elements;

namespace Mix2App.Profile {
    public class GrowthRecordWindow:UIWindow {
        /// <summary>
        /// Loaded characters
        /// </summary>
        [SerializeField, Required] private CharaBehaviour[] Character1 = null;
        [SerializeField, Required] private CharaBehaviour[] Character2 = null;

        /// <summary>
        /// Frames list
        /// </summary>
        [SerializeField, Required] private GameObject[] Frames = null;

        public GrowthRecordWindow SetupCharacters(AlbumData[] albums) {
            if (albums.Length==0)
            {
                Frames[0].SetActive(true);
                for (int i = 1; i < 5; i++) {
                    Frames[i].SetActive(false);
                }

                Character1[0].init(ManagerObject.instance.player.chara1);
                Character2[0].gameObject.SetActive(ManagerObject.instance.player.chara2!=null);
                if (ManagerObject.instance.player.chara2 != null)
                    Character2[0].init(ManagerObject.instance.player.chara2);
            }
            else
            {
                for (int i = 0; i < 5; i++) {
                    if (albums.Length>i) {
                        Frames[i].SetActive(true);
                        Character1[i].init(albums[i].chara1);
                        Character2[i].gameObject.SetActive(albums[i].chara2!=null);
                        if (albums[i].chara2 != null)
                            Character2[i].init(albums[i].chara2);
                    } else {
                        Frames[i].SetActive(false);
                        Character1[i].gameObject.SetActive(false);
                        Character2[i].gameObject.SetActive(false);
                    }
                }
            }
            return this;
        }
    }
}
