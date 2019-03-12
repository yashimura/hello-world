////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;

namespace Mix2App.UI {
    /// <summary>
    /// I think, its a temporary solution.
    /// Will be replaced by global sprite library
    /// </summary>    
    [CreateAssetMenu(fileName = "SpriteList", menuName = "Sprites/List", order = 1)]
    public class SpriteList: ScriptableObject {
        public Sprite[] Sprites;
        
        public Sprite Last {
            get {
                return Sprites[Sprites.Length - 1];
            }
        }

        public Sprite this[int num] {
            get {
                return Sprites[num];
            }
        }
        
        public Sprite GetSprite(int num) {
            return this[num];
        }

        public int Count {
            get {
                return Sprites.Length;
            }
        }
    }
}
