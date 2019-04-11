using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mix2App.MiniGame2
{
    [CreateAssetMenu(fileName = "MiniGame2EventImg", menuName = "MiniGame2/MiniGame2EventImg", order = 1)]
    public class MiniGame2EventImg : ScriptableObject
    {
        [Tooltip("背景画像")]
        [SerializeField] private Sprite _ImgBG = null;
        [Tooltip("タイトル画像")]
        [SerializeField] private Sprite _ImgTitle = null;
        [Tooltip("タイトルバナー画像")]
        [SerializeField] private Sprite _ImgBanner = null;

        public Sprite ImgBG
        {
            get
            {
                return _ImgBG;
            }
        }
        public Sprite ImgTitle
        {
            get
            {
                return _ImgTitle;
            }
        }
        public Sprite ImgBanner
        {
            get
            {
                return _ImgBanner;
            }
        }



    }
}
