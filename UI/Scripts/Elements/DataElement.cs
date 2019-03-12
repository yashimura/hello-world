////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////

namespace Mix2App.UI.Elements  {
    /// <summary>
    /// Base class for data element
    /// </summary>
    /// <typeparam name="DataType"></typeparam>
    public abstract class DataElement<DataType>: UIElement {
        /// <summary>
        /// Setup data at this toggle
        /// </summary>
        /// <param name="data">Data to assign</param>
        public abstract void SetData(DataType data);
    }
}
