////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Mix2App.UI {
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class Required: System.Attribute {}

    /// <summary>
    /// Base class for custom UI elements
    /// </summary>
    public class UIElement: MonoBehaviour { }
}
