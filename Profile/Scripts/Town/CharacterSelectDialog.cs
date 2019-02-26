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
using Mix2App.UI;
using Mix2App.UI.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Model;

namespace Mix2App.Profile.Town {
    public class CharacterSelectDialog: UIWindow {
        [Header("Left choice")]
        [SerializeField] private CharaBehaviour LeftChoiceCharacter;
        [SerializeField] private Button LeftChoiceButton;
        [SerializeField] private Text LeftCharacterNameText;

        [Header("Right choice")]
        [SerializeField] private Button RightChoiceButton;
        [SerializeField] private CharaBehaviour RightChoiceCharacter;
        [SerializeField] private Text RightCharacterNameText;

        /// <summary>
        /// Action will invoked when user make choice
        /// value:
        /// 0 - Left choice
        /// 1 - Right choice
        /// </summary>
        /// <param name="action"></param>
        /// <returns>self</returns>
        public CharacterSelectDialog AddChoiceAction(UnityAction<int> action) {
            ChoiceEvent.AddListener(action);
            return this;
        }

        public CharacterSelectDialog SetCharacters(TamaChara leftChara, TamaChara rightChara) {
            LeftChoiceCharacter.init(leftChara);
            RightChoiceCharacter.init(rightChara);

            LeftCharacterNameText.text = leftChara.cname;
            RightCharacterNameText.text = rightChara.cname;
            return this;
        }

        private UnityIntEvent ChoiceEvent = new UnityIntEvent();

        public override void Setup() {
            base.Setup();

            AddButtonTapListner(LeftChoiceButton, () => {
                ChoiceEvent.Invoke(0);
            });

            AddButtonTapListner(RightChoiceButton, () => {
                ChoiceEvent.Invoke(1);
            });
        }
    }
}
