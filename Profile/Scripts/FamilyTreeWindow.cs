////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Mix2App.UI.Dialogs;
using Mix2App.Profile.Elements;
using Mix2App.Lib.Model;

namespace Mix2App.Profile {
    /// <summary>
    /// Represent a familytree window
    /// </summary>
    public class FamilyTreeWindow:ElementListDialog<FamilyTreeElement, FamilyTree> {
        public int listCount=0;
        public ScrollRect scrollRect;
        protected override void ElementAdded(FamilyTreeElement element, int element_number) {
            
            base.ElementAdded(element, element_number);

            //if (Elements.Count>0)
                //Elements[Elements.Count - 1].SetGenerationNumber(element_number, false);

            // Start generation from 1
            element.SetGenerationNumber(element_number+1,Elements.Count+1==listCount);

            if (element_number > 0)
            {
                FamilyTreeElement prevelement = Elements[element_number-1];
                element.ShowTopEdge(prevelement.GetMarriaged());
            }
            else
                element.ShowTopEdge(false);

            if (Elements.Count+1 >= listCount)
            {
                StartCoroutine(putlogend());
            }

        }

        IEnumerator putlogend()
        {
            yield return new WaitForSeconds(0.2f);
            scrollRect.verticalNormalizedPosition = 0;
        }
    }
}
