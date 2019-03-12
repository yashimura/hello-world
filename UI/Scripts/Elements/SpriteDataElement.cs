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
    public class SpriteDataElement<ElementType>: ControlDataElement<ElementType, Sprite>
        where ElementType : UnityEngine.EventSystems.UIBehaviour {
        [SerializeField, Required] protected Image ImageDataComponent;

        public override void SetData(Sprite data) {
            ImageDataComponent.sprite = data;
        }
    }
}
