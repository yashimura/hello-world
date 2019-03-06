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
using Mix2App.UI.Elements;
using Mix2App.Lib;
using Mix2App.Lib.View;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;

namespace Mix2App.Profile {
    public class ProfileSetupWindow: UIWindow {

        [Header("Prefecture setup")]
        [Tooltip("Show current prefecture as image")]
        [SerializeField, Required] protected PrefectureElement PrefectureElementPrefab;

        [Tooltip("Window prefab to select current prefecture")]
        [SerializeField, Required] private PrefectureSelectWindow PrefectureSelectWindowPrefab = null;

        [SerializeField, Required] private Button SetupPrefectureButton = null;
    
        [Header("Avatar setup")]
        [SerializeField, Required] protected AvatarElement AvatarPrefab;

        [Header("Avatar sex setup")]
        [SerializeField, Required] private Button SetMaleSexButton = null;
        [SerializeField, Required] private Button SetFemaleSexButton = null;

        [SerializeField, Required] private GameObject MaleFrame = null;
        [SerializeField, Required] private GameObject FemaleFrame = null;

        [Header("Avatar element setup")]
        [Tooltip("Setup avatar's hair button")]
        [SerializeField, Required] private Button SetHairButton = null;

        [Tooltip("Setup avatar's face button")]
        [SerializeField, Required] private Button SetBodyButton = null;

        [Tooltip("Setup avatar's contour(body) button")]
        [SerializeField, Required] private Button SetFaceButton = null;

        [Tooltip("Setup avatar's upper body part button")]
        [SerializeField, Required] private Button SetTopsButton = null;

        [Tooltip("Setup avatar's lower body part button")]
        [SerializeField, Required] private Button SetBottomsButton = null;

        [Tooltip("Window prefab to select avatar's part")]
        [SerializeField, Required] private AvatarElementSelectWindow AvatarElementSelectWindowPrefab = null;

        protected User UserData;
        
        /// <summary>
        /// Show male frame (And hide female)
        /// </summary>
        private void SetMaleFrame() {
            SetFemaleSexButton.gameObject.SetActive(true);
            SetMaleSexButton.gameObject.SetActive(false);

            MaleFrame.SetActive(true);
            FemaleFrame.SetActive(false);
        }

        /// <summary>
        /// Show female frame (And hide male)
        /// </summary>
        private void SetFemaleFrame() {
            SetFemaleSexButton.gameObject.SetActive(false);
            SetMaleSexButton.gameObject.SetActive(true);

            MaleFrame.SetActive(false);
            FemaleFrame.SetActive(true);
        }

        /// <summary>
        /// Set avatars data. 
        /// </summary>
        /// <param name="ava"></param>
        private void SetupAvatar(TamaAvatar ava) {
            if (!this.gameObject.activeInHierarchy)
                this.gameObject.SetActive(true);
            AvatarPrefab.SetupAvatar(ava);
            if (ava.sex == 1)
                SetFemaleFrame();
            if (ava.sex == 2)
                SetMaleFrame();
        }

        /// <summary>
        /// Update avatars data. Also send changings to server.
        /// </summary>
        /// <param name="ava"></param>
        private void UpdateAvatar(TamaAvatar ava) {
            GameCall call = new GameCall(CallLabel.SET_AVATAR, ava);
            call.AddListener(mupdateavatar);
            ManagerObject.instance.connect.send(call);
        }

        void mupdateavatar(bool success,object data)
        {
            SetupAvatar(UserData.avatar);
        }

        /// <summary>
        /// Sets current prefecture.
        /// </summary>
        /// <param name="pref"></param>
        private void SetupPrefecture(int pref) {
            Debug.Log("SetupPrefecture:"+pref);
            // HOKKAIDO=1 SECRET=0
            int listindex = pref-1;
            if (listindex<0) listindex = 48;
            PrefectureElementPrefab.SetPrefecture(listindex);
        }

        /// <summary>
        /// Update current prefecture.
        /// Also send changings to server.
        /// </summary>
        /// <param name="pref"></param>
        private void UpdatePrefecture(int pref) {
            Debug.Log("UpdatePrefecture:"+pref);
            // HOKKAIDO=1 SECRET=0
            int listindex = pref+1;
            if (listindex>48) listindex = 0;
            GameCall call = new GameCall(CallLabel.SET_BPLACE, listindex);
            call.AddListener(mupdatebplace);
            ManagerObject.instance.connect.send(call);
        }

        void mupdatebplace(bool success,object data)
        {
            SetupPrefecture(UserData.bplace);
        }
        
        /// <summary>
        /// Get Current avatar.
        /// </summary>
        /// <returns></returns>
        public TamaAvatar GetAvatar() {
            return AvatarPrefab.GetAvatar();
        }

        /// <summary>
        /// Get current prefecture
        /// </summary>
        /// <returns></returns>
        public int GetPrefecture() {
            return PrefectureElementPrefab.GetPrefecture();
        }

        public ProfileSetupWindow SetupUserData(User user_data) {
            UserData = user_data;


            AddButtonTapListner(SetupPrefectureButton, () => {
                PrefectureSelectWindow pw = (PrefectureSelectWindow)UIManager.ShowModal(PrefectureSelectWindowPrefab, false)
                    .AddSelectAction(UpdatePrefecture)
                    .AddElements(PrefectureElementPrefab.PrefecturePlaceList.Sprites);

                pw.SetCurrentElement(PrefectureElementPrefab.GetPrefecture());
            });

            AddButtonTapListner(SetHairButton, () => {
                UIManager.ShowModal(AvatarElementSelectWindowPrefab, false)
                    .SetupAvatar(AvatarPrefab.GetAvatar(), AvatarElementSelectWindow.PartType.Hair)
                    .AddAvatarSetupAction(UpdateAvatar);
            });


            AddButtonTapListner(SetBodyButton, () => {
                UIManager.ShowModal(AvatarElementSelectWindowPrefab, false)
                    .SetupAvatar(AvatarPrefab.GetAvatar(), AvatarElementSelectWindow.PartType.Body)
                    .AddAvatarSetupAction(UpdateAvatar);
            });


            AddButtonTapListner(SetFaceButton, () => {
                UIManager.ShowModal(AvatarElementSelectWindowPrefab, false)
                    .SetupAvatar(AvatarPrefab.GetAvatar(), AvatarElementSelectWindow.PartType.Face)
                    .AddAvatarSetupAction(UpdateAvatar);
            });


            AddButtonTapListner(SetTopsButton, () => {
                UIManager.ShowModal(AvatarElementSelectWindowPrefab, false)
                    .SetupAvatar(AvatarPrefab.GetAvatar(), AvatarElementSelectWindow.PartType.ClothesTop)
                    .AddAvatarSetupAction(UpdateAvatar);
            });


            AddButtonTapListner(SetBottomsButton, () => {
                UIManager.ShowModal(AvatarElementSelectWindowPrefab, false)
                    .SetupAvatar(AvatarPrefab.GetAvatar(), AvatarElementSelectWindow.PartType.ClothesBottom)
                    .AddAvatarSetupAction(UpdateAvatar);
            });


            SetFemaleSexButton.gameObject.SetActive(true);
            SetMaleSexButton.gameObject.SetActive(false);

            MaleFrame.SetActive(true);
            FemaleFrame.SetActive(false);

            AddButtonSEListner(SetMaleSexButton, SE_Choice, () => {
                SetFemaleSexButton.gameObject.SetActive(true);
                SetMaleSexButton.gameObject.SetActive(false);

                MaleFrame.SetActive(true);
                FemaleFrame.SetActive(false);

                AvatarPrefab.ChangeSex(2);
            });

            AddButtonSEListner(SetFemaleSexButton, SE_Choice, () => {
                SetFemaleSexButton.gameObject.SetActive(false);
                SetMaleSexButton.gameObject.SetActive(true);

                MaleFrame.SetActive(false);
                FemaleFrame.SetActive(true);
                AvatarPrefab.ChangeSex(1);
            });

            SetupAvatar(UserData.avatar);
            SetupPrefecture(UserData.bplace);

            return this;
        }
    }
}