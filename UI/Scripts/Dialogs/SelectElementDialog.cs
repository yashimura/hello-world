////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Mix2App.UI.Elements;
using Mix2App.UI.Events;

namespace Mix2App.UI.Dialogs {
    /// <summary>
    /// Base class for simple dialog to select one Element.
    /// </summary>
    /// <typeparam name="ToggleType"></typeparam>
    /// <typeparam name="DataType"></typeparam>    
    public class SelectElementWindow<ToggleType, DataType>: ElementListDialog<ToggleType, DataType>
        where ToggleType:ControlDataElement<Toggle, DataType> {        
        [SerializeField, Required] private Button AcceptButton = null;

        protected override void ElementAdded(ToggleType element, int element_number) {
            base.ElementAdded(element, element_number);

            element.ControlComponent.isOn = false;
            element.ControlComponent.onValueChanged.AddListener((state) => {
                if (state) {
                    if (CurrentSelectedElement != element_number) {
                        if (CurrentSelectedElement != -1)
                            Elements[CurrentSelectedElement].ControlComponent.isOn = false;
                        CurrentSelectedElement = element_number;
                        ElementSelected(element_number);
                    }
                    PlaySE(SE_Choice);
                }
            });
        }

        private UnityIntEvent SelectedEvent = new UnityIntEvent();

        private int StartSelectedElement = -1;
        private int CurrentSelectedElement = -1;        

        /// <summary>
        /// Setup started prefecture (prefecture with this number will have click effect)
        /// </summary>
        /// <param name="pref"></param>
        /// <returns>self</returns>
        public SelectElementWindow<ToggleType, DataType> SetCurrentElement(int elem) {
            StartSelectedElement = elem;
            if (elem>=0 && elem< Elements.Count)
                Elements[elem].ControlComponent.isOn = true;
            return this;
        }

        protected virtual void ElementSelected(int element_number) { }
                
        /// <summary>
        /// Call this action when prefecture changed.
        /// </summary>
        /// <param name="action"></param>
        public SelectElementWindow<ToggleType, DataType> AddSelectAction(UnityAction<int> action) {
            SelectedEvent.AddListener(action);
            return this;
        }

        /// <summary>
        /// Setup action to accept button
        /// </summary>
        public void Accept() {
            if (StartSelectedElement != CurrentSelectedElement)
                SelectedEvent.Invoke(CurrentSelectedElement);
            Close();
        }
        
        public override void Setup() {
            base.Setup();
                        
            AddButtonSEListner(AcceptButton, SE_OK, Accept);
        }
    }
}
