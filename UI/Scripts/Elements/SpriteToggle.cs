﻿////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
namespace Mix2App.UI.Elements {
    /// <summary>
    /// Simple class for toggle with assignable sprite.
    /// Used to be assigned to UI elemnt as a component
    /// </summary>
    [UnityEngine.AddComponentMenu("UI/DataElements/SpriteToggle")]
    public class SpriteToggle: SpriteDataElement<UnityEngine.UI.Toggle> {
        public bool UIElementEnabled {
            set {
                ControlComponent.interactable = value;
                if (value)
                    ImageDataComponent.color = UnityEngine.Color.white;
                else
                    ImageDataComponent.color = ControlComponent.colors.disabledColor;
            }
        }
    }
}
