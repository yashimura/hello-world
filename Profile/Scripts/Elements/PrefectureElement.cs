////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using System;
using UnityEngine;
using UnityEngine.UI;
using Mix2App.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mix2App.Profile.Elements {
    /// <summary>
    /// Element to select prefecture
    /// </summary>
    [Serializable] public class PrefectureElement {

        [Tooltip("SpriteList for prefecture places imge. Ended \"Secret\"")]
        [SerializeField, Required] public SpriteList PrefecturePlaceList = null;
        [SerializeField, Required] private Image PrefecturePlace = null;

        /// <summary>
        /// Current prefecture number.
        /// </summary>
        private int Prefecture = -1;

        /// <summary>
        /// Set current prefecture number.
        /// </summary>
        /// <param name="prefecture">Prefecture number. 0, or more than prefectures count - sets to secret</param>
        public void SetPrefecture(int prefecture) {
            Prefecture = prefecture;
            if (prefecture < 0 || prefecture >= PrefecturePlaceList.Count)
                PrefecturePlace.sprite = PrefecturePlaceList.Last;
            else
                PrefecturePlace.sprite = PrefecturePlaceList[prefecture];
        }

        /// <summary>
        /// Return current prefecture number.
        /// For secret - returns 0
        /// </summary>
        /// <returns></returns>
        public int GetPrefecture() {
            if (Prefecture < 0 || Prefecture >= PrefecturePlaceList.Count)
                return 0;
            else
                return Prefecture;
        }

#if UNITY_EDITOR && false
        [CustomPropertyDrawer(typeof(PrefectureElement))]
        public class PrefecturePropertyDrawer: PropertyDrawer {
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
                EditorGUI.ObjectField(PropertyRect, property.FindPropertyRelative("PrefecturePlace"), GUIContent.none);
            }
        }
#endif
    }
}
