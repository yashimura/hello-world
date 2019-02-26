////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;
using UnityEngine.UI;
using Mix2App.Lib;
using Mix2App.UI;
using Mix2App.Profile.Elements;
using Mix2App.Lib.View;
using Mix2App.Lib.Model;
using Mix2App.Lib.Net;
using Mix2App.Lib.Events;

namespace Mix2App.Profile {
    public class ProfileWindow: UIWindow {
        protected User UserData;
                
        [Header("FamilyTree setup")]
        [Tooltip("Window prefab to select current prefecture")]
        [SerializeField, Required] private FamilyTreeWindow FamilyTreeWindowPrefab;
        [SerializeField, Required] private FamilyTreeExceptionWindow FamilyTreeExceptionWindowPrefab;
        [SerializeField, Required] private Button ShowFamilyTreeButton;

        [Header("User info")]
        [SerializeField, Required] protected UserInfoElement UserInfoElementPrefab;

        public virtual ProfileWindow SetupUserData(User user_data) {
            UserData = user_data;
            UserInfoElementPrefab.SetupUserData(user_data);
            return this;
        }
        
        /// <summary>
        /// "Show familytree" action
        /// </summary>
        private void ShowFamilyTreeWindow() {

            GameCall call = new GameCall(CallLabel.GET_FAMILY_TREE,UserData);
            call.AddListener(mgetftree);
            ManagerObject.instance.connect.send(call);
        }

        void mgetftree(bool success,object data)
        {        
            if (UserData.ftrees.Count > 0) {
                FamilyTree[] r = UserData.ftrees.ToArray();                
                FamilyTreeWindowPrefab.listCount = r.Length;
                UIManager.ShowModal(FamilyTreeWindowPrefab, false).AddElements(r);
            } 
            else 
            {
                //UIManager.ShowModal(FamilyTreeWindowPrefab).AddElements(new FamilyTree() { chara1 = UserData.chara1 });
                UIManager.ShowModal(FamilyTreeExceptionWindowPrefab, false).SetupCharacter(UserData.chara1);
            }
        }

        public override void Setup() {
            base.Setup();

            AddButtonTapListner(ShowFamilyTreeButton, ShowFamilyTreeWindow);
        }
    }
}
