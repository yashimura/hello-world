////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using System;
using UnityEngine;
using UnityEditor;
using Mix2App.UI;
using Mix2App.Lib.View;
using Mix2App.Lib.Model;

namespace Mix2App.Profile.Elements {
    /// <summary>
    /// Wrapper over AvatarBehaviour
    /// </summary>
    [Serializable] public class AvatarElement {
        /// <summary>
        /// Current avatar. Assigned by editor.
        /// </summary>
        [SerializeField, Required] private AvatarBehaviour AvatarPrefab = null;
                
        public int Sex {
            get {
                return AvatarPrefab.sex;
            }
        }
        public int Hair {
            get {
                return AvatarPrefab.hair;
            }
        }
        public int Face {
            get {
                return AvatarPrefab.face;
            }
        }
        public int Body {
            get {
                return AvatarPrefab.body;
            }
        }
        public int Tops {
            get {
                return AvatarPrefab.tops;
            }
        }
        public int Bottoms {
            get {
                return AvatarPrefab.bottoms;
            }
        }

        public void SetupAvatar(TamaAvatar ava) {
            if (!AvatarPrefab.isActiveAndEnabled) {                
                AvatarPrefab.gameObject.SetActive(true);
            }

            if (ava == null) {
                Debug.LogError("TamaAvatar data is null!");
                return;
            }

            //Debug.Log(string.Format("body: {0}; face: {1}; hair{2}; tops: {3}; bottoms: {4}", ava.body, ava.face, ava.hair, ava.tops, ava.bottoms));

            AvatarPrefab.init(ava);
        }
        
        public void ChangeSex(int id) {
            AvatarPrefab.chSex(id);   
        }

        public void ChangeHair(int id) {
            AvatarPrefab.chHair(id);
        }

        public void ChangeBody(int id) {
            AvatarPrefab.chBody(id);
        }

        public void ChangeFace(int id) {
            AvatarPrefab.chFace(id);
        }

        public void ChangeTops(int id) {
            AvatarPrefab.chTops(id);
        }

        public void ChangeBottoms(int id) {
            AvatarPrefab.chBottoms(id);
        }

        public TamaAvatar GetAvatar() {
            return AvatarPrefab.generateData();
        }

        public bool EnabledBottoms(int index) {
            return AvatarPrefab.enabledBottomsAt(index);
        }

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(AvatarElement))]
        public class AvatarPropertyDrawer: PropertyDrawer {            
            public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
                // Setup property rects
                Rect LabelRect = new Rect(
                                rect.x,
                                rect.y,
                                EditorGUIUtility.labelWidth,
                                EditorGUIUtility.singleLineHeight);

                Rect PropertyRect = new Rect(
                            rect.x + LabelRect.width,
                            rect.y,
                            rect.width - LabelRect.width,
                            EditorGUIUtility.singleLineHeight);

                EditorGUI.PrefixLabel(LabelRect, label, GUIStyle.none);
                EditorGUI.ObjectField(PropertyRect, property.FindPropertyRelative("AvatarPrefab"), GUIContent.none);
            }
        }
#endif
    }
}                              
                               