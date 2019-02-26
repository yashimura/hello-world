////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mix2App.UI;
using Mix2App.UI.Dialogs;
using Mix2App.UI.Elements;
using UnityEngine.Events;
using UnityEngine.U2D;
using Mix2App.Lib;
using Mix2App.Lib.View;
using Mix2App.Lib.Events;
using Mix2App.Profile.Elements;
using Mix2App.Profile.Events;
using Mix2App.Lib.Model;

namespace Mix2App.Profile {
    /// <summary>
    /// This window used to select avatar part with avatar preview.
    /// </summary>
    public class AvatarElementSelectWindow: SelectElementWindow<SpriteToggle, Sprite> {
        [Tooltip("Avatar preview")]
        [SerializeField, Required] protected AvatarElement AvatarPrefab;

        private PartType CurrentPartType;

        protected override void ElementSelected(int element_number) {
            base.ElementSelected(element_number);
            switch (CurrentPartType) {
                case PartType.Body:
                    AvatarPrefab.ChangeBody(GetelementNumber(AvatarPrefab.Sex, ID_BODY, element_number));
                    break;
                case PartType.Hair:
                    AvatarPrefab.ChangeHair(GetelementNumber(AvatarPrefab.Sex, ID_HAIR, element_number));
                    break;
                case PartType.Face:
                    AvatarPrefab.ChangeFace(GetelementNumber(AvatarPrefab.Sex, ID_FACE, element_number));
                    break;
                case PartType.ClothesTop:
                    AvatarPrefab.ChangeTops(GetelementNumber(AvatarPrefab.Sex, ID_TOPS, element_number));
                    break;                    
                case PartType.ClothesBottom:
                    AvatarPrefab.ChangeBottoms(GetelementNumber(AvatarPrefab.Sex, ID_BOTTOMS, element_number));
                    break;
                default:
                    break;
            }
        }

        protected override void ElementAdded(SpriteToggle element, int element_number) {
            base.ElementAdded(element, element_number);

            if (CurrentPartType == PartType.ClothesBottom
                && !AvatarPrefab.EnabledBottoms(element_number))
                element.UIElementEnabled = false;
        }

        public enum PartType {
            Body,
            Hair,
            Face,
            ClothesTop,
            ClothesBottom
        }
        
        //[SerializeField, Required] private SpriteAtlas ThumbinalSprites;
        
        const int ID_BODY = 1;
        const int ID_FACE = 2;
        const int ID_HAIR = 3;
        const int ID_TOPS = 4;
        const int ID_BOTTOMS = 5;

        private int GetelementNumber(int sex, int id, int num) {
            if (idlist != null)
                return idlist[sex-1][id-1][num];
            Debug.LogError("Error while load avatar info. (Call with GET_AVATAR_INFO)");
            return num+1;            
        }

        /// <summary>
        /// Return avatar part sprites.
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private IEnumerable<Sprite> GetElements(int sex, int id) {
            int i = 1;
            while (true) {
                string key = string.Format("{0}{1}{2:0#}", sex, id, i++);
                if (thumbnails.ContainsKey(key))
                {
                    Sprite sp = thumbnails[key];
                    yield return sp;
                }
                else
                    break;
            }
        }

        /// <summary>
        /// Setup avatar data
        /// </summary>
        /// <param name="data">Avatar data (temporary)</param>
        /// <param name="set">settings</param>
        /// <param name="c_part_type">setted part type</param>
        /// <returns></returns>
        public AvatarElementSelectWindow SetupAvatar(TamaAvatar ava, PartType c_part_type) {
            CurrentPartType = c_part_type;

            AvatarPrefab.SetupAvatar(ava);            
            switch (CurrentPartType) {
                case PartType.Body:
                    this.AddElements(GetElements(ava.sex, ID_BODY));
                    SetCurrentElement(ava.body - 1);
                    break;
                case PartType.Hair:
                    this.AddElements(GetElements(ava.sex, ID_HAIR));
                    SetCurrentElement(ava.hair - 1);
                    break;
                case PartType.Face:
                    this.AddElements(GetElements(ava.sex, ID_FACE));
                    SetCurrentElement(ava.face - 1);
                    break;
                case PartType.ClothesTop:
                    this.AddElements(GetElements(ava.sex, ID_TOPS));
                    SetCurrentElement(ava.tops - 1);
                    break;
                case PartType.ClothesBottom:
                    this.AddElements(GetElements(ava.sex, ID_BOTTOMS));
                    SetCurrentElement(ava.bottoms - 1);
                    break;
                default:
                    break;
            }

            while (Elements.Count < 3) {
                this.AddElements(new Sprite[] { null });

                Transform tr = Elements[Elements.Count - 1].transform;
                DisableComponents(tr, typeof(Image));
                DisableComponents(tr, typeof(Toggle));
            }

            return this;
        }

        private static void DisableComponents(Transform tr, Type tp) {
            foreach (Transform item in tr) {
                foreach (Behaviour img in item.GetComponents(tp))
                    img.enabled = false;
                DisableComponents(item, tp);
            }
        }
        
        private AvatarSetupEvent AvatarSetupEvent = new AvatarSetupEvent();

        /// <summary>
        /// Action invoked when user apply avatar changings.
        /// </summary>
        /// <param name="action"></param>
        /// <returns>self</returns>
        public AvatarElementSelectWindow AddAvatarSetupAction(UnityAction<TamaAvatar> action) {
            AvatarSetupEvent.AddListener(action);            
            return this;
        }

        /// <summary>
        /// Avatar Thumbnail SpriteAtlas:
        /// </summary>
        private static Dictionary<string,Sprite> thumbnails;
        
        /// <summary>
        /// Same for Avatar parts ID
        /// </summary>
        private static List<List<List<int>>> idlist;

        /// <summary>
        /// Use this to load avatar data to static variables.
        /// </summary>
        public static void InitData() {
            GameCall call = new GameCall(CallLabel.GET_AVATAR_INFO);
            call.AddListener((bool success, object data) => {
                if (success) {
                    AvatarInfoData dt = (AvatarInfoData)data;
                    thumbnails = dt.thumbs;
                    idlist = dt.idlist;
                }
            });
            ManagerObject.instance.connect.send(call);
        }

        public override void Setup() {
            base.Setup();
            AddSelectAction((index) => {
                AvatarSetupEvent.Invoke(AvatarPrefab.GetAvatar());
            });
        }
    }
}
