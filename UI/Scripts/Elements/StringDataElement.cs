////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;
using UnityEngine.UI;

namespace Mix2App.UI.Elements {
    /// <summary>
    /// Base class for element with settable image
    /// </summary>
    /// <typeparam name="ElementType"></typeparam>
    public class StringDataElement<ElementType>: ControlDataElement<ElementType, string>
        where ElementType : UnityEngine.EventSystems.UIBehaviour {
        [SerializeField, Required] protected Text StringDataComponent;

        public override void SetData(string data) {
            StringDataComponent.text = data;
        }
    }
}
