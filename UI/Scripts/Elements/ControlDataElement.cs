////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;

namespace Mix2App.UI.Elements {
    /// <summary>
    /// Represent UI Element with assignable data field
    /// </summary>
    /// <typeparam name="ElementType"></typeparam>
    /// <typeparam name="DataType"></typeparam>
    public abstract class ControlDataElement<ElementType, DataType>: DataElement<DataType> 
        where ElementType: UnityEngine.EventSystems.UIBehaviour {
        [SerializeField, Required] private ElementType _ControlComponent = null;

        /// <summary>
        /// Represent control component in this data element
        /// </summary>
        public ElementType ControlComponent {
            get {
                return _ControlComponent;
            }
        }
    }
}
