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
using Mix2App.Profile.Elements;
using Mix2App.Lib.View;
using Mix2App.Lib.Model;

namespace Mix2App.Profile {
    public class FamilyTreeExceptionWindow: UIWindow {
        [SerializeField, Required] private CharaBehaviour Chara;
        public void SetupCharacter(TamaChara ch) {
            if (ch != null) {
                Chara.init(ch);
            }
        }
    }
}
