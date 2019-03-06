////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;
using Mix2App.UI;
using Mix2App.UI.Elements;
using UnityEngine.UI;
using Mix2App.Lib.Model;
using Mix2App.Lib.View;

namespace Mix2App.Profile.Elements {
    /// <summary>
    /// Represent button with user information
    /// </summary>
    public class UserDataButton: ControlDataElement<Button, ProfileBookData> {
        [Tooltip("Text to display user name")]
        [SerializeField, Required] private Text UserNameCaption = null;
        [SerializeField, Required] private CharaBehaviour Character = null;

        /// <summary>
        /// Gets current user data
        /// </summary>
        public ProfileBookData pbookData { get; private set; }

        public override void SetData(ProfileBookData data) {
            pbookData = data;
            UserNameCaption.text = data.user.nickname;
            Character.init(data.user.chara1);
        }
    }
}
