using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mix2App.MiniGame1 {
    [CreateAssetMenu(fileName = "GameItem", menuName = "MiniGame1/GameItem", order = 1)]
    public class GameItem: ScriptableObject {
        [Tooltip("Added score for this item. Use negtive value to make it bomb")]
        [SerializeField] private int _Score;
        public int Score {
            get {
                return _Score;
            }
        }

        [SerializeField] private Sprite _ItemImage;
        public Sprite ItemImage {
            get {
                return _ItemImage;
            }
        }
    }
}
