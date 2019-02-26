////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;
using UnityEngine.UI;
using Mix2App.UI;
using Mix2App.UI.Elements;
using Mix2App.Lib.Model;
using Mix2App.Lib.View;

namespace Mix2App.Profile.Elements {
    /// <summary>
    /// Element for family tree
    /// </summary>
    public class FamilyTreeElement: DataElement<FamilyTree> {

        [Tooltip("Title background image")]
        [SerializeField, Required] private Image BG;

        [Tooltip("Text to show generation number")]
        [SerializeField, Required] private Text GenerationText;
        
        [Header("Edges")]
        [Tooltip("Edge to next generation")]
        [SerializeField, Required] private GameObject TopEdge;

        [Tooltip("Edge to left character")]
        [SerializeField, Required] private GameObject LeftEdge;

        [Tooltip("Edge to right character")]
        [SerializeField, Required] private GameObject RightEdge;

        [Header("Characters")]
        [SerializeField, Required] private CharaBehaviour LeftCharacter;
        [SerializeField, Required] private CharaBehaviour MiddleCharacter;
        [SerializeField, Required] private CharaBehaviour RightCharacter;

        bool mflag;
        int mgene;

        /// <summary>
        /// Sets generation to this element
        /// </summary>
        /// <param name="num">generation number</param>
        /// <param name="now">if this one last, set true</param>
        public void SetGenerationNumber(int num,bool now) {
            if (now)
                GenerationText.text =  "ナウたま";
            else
                GenerationText.text = mgene + "だいめ";

            if (num % 2 == 0)
                BG.color = Color.white;
        }

        /// <summary>
        /// If this item - last generation, hide down edge.
        /// </summary>
        /// <param name="showed"></param>
        public void ShowTopEdge(bool showed = true) {
            TopEdge.SetActive(showed);
        }

        public bool GetMarriaged()
        {
            return mflag;
        }

        public override void SetData(FamilyTree data) {

            mgene = data.generation;
            mflag = data.marriage;

            RightEdge.SetActive(false);

            if (data.chara1 != null)
                MiddleCharacter.init(data.chara1);
            else
                MiddleCharacter.gameObject.SetActive(false);

            if (data.chara2 != null && data.twins)
                RightCharacter.init(data.chara2);
            else 
                RightCharacter.gameObject.SetActive(false);

            if (data.chara3 != null && data.marriage) {
                LeftCharacter.init(data.chara3);
                LeftEdge.SetActive(true);
            } 
            else
            {
                LeftCharacter.gameObject.SetActive(false);
                LeftEdge.SetActive(false);
            }
        }
    }
}
