using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mix2App.PointShop
{
    [CreateAssetMenu(fileName = "PointShopEventImg", menuName = "PointShop/PointShopEventImg", order = 1)]
    public class PointShopEventImg : ScriptableObject
    {
        [Tooltip("ベース画像")]
        [SerializeField] private Sprite _ImgBase = null;

        public Sprite ImgBase
        {
            get
            {
                return _ImgBase;
            }
        }



    }
}
