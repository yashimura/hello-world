////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using System.Collections;
using System.Collections.Generic;
using Mix2App.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Mix2App.UI.Dialogs {
    public class ElementListDialog<ElementType, DataType>: UIWindow
        where ElementType: DataElement<DataType>  {
        [SerializeField, Required] private ElementType ElementPrefab = null;
        [SerializeField, Required] private Transform Container = null;

        protected List<ElementType> Elements = new List<ElementType>();

        /// <summary>
        /// Called before element added to Elements list
        /// </summary>
        /// <param name="element"></param>
        /// <param name="element_number"></param>
        protected virtual void ElementAdded(ElementType element, int element_number) { }

        /// <summary>
        /// Add elements to list.
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public ElementListDialog<ElementType, DataType> AddElements(params DataType[] elements) {
            return AddElements((IEnumerable<DataType>)elements);
        }

        public ElementListDialog<ElementType, DataType> AddElements(IEnumerable<DataType> elements) {
            foreach (var item in elements) {
                GameObject go = GameObject.Instantiate(ElementPrefab.gameObject, Container);
                ElementType et = go.GetComponent<ElementType>();
                et.SetData(item);
                ElementAdded(et, Elements.Count);
                Elements.Add(et);
            }
            return this;
        }

        [Tooltip("If GridLayoutGroup.constraint == FixedColumnCount\nYou can choose size ratio for elements.")]
        [SerializeField] private float GridRatio = 1.0f;

        private IEnumerator SetupWdth() {
            yield return null;
            GridLayoutGroup gr = Container.GetComponent<GridLayoutGroup>();
            if (gr != null)
                if (gr.constraint == GridLayoutGroup.Constraint.FixedColumnCount) {
                    float width = Container.GetComponent<RectTransform>().rect.size.x;
                    width = (width - gr.spacing.x * (gr.constraintCount + 1)) / (float)(gr.constraintCount);
                    gr.cellSize = new Vector2(width, width * GridRatio);
                }
        }

        public override void Setup() {
            base.Setup();

            StartCoroutine(SetupWdth());
        }
    }
}
