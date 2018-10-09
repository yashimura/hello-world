using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mix2App.MiniGame1 {
	[CreateAssetMenu(fileName = "SeasonImg", menuName = "MiniGame1/SeasonImg", order = 1)]
	public class SeasonImg: ScriptableObject {
		[Tooltip("タイトル画像")]
		[SerializeField] private Sprite _ImgTitle;
		[Tooltip("背景画像")]
		[SerializeField] private Sprite _ImgBG;
		[Tooltip("紅葉")]
		[SerializeField] private Sprite _ImgMomiji;
		[Tooltip("雲")]
		[SerializeField] private Sprite _ImgKumo;
		[Tooltip("アイテム １０点、２０点、３０点、５０点、１００点、お邪魔")]
		[SerializeField] private Sprite[] _ImgItem;

		public Sprite ImgTitle {
			get {
				return _ImgTitle;
			}
		}

		public Sprite ImgBG{
			get{
				return _ImgBG;
			}
		}

		public Sprite ImgMomiji{
			get{
				return _ImgMomiji;
			}
		}

		public Sprite ImgKumo{
			get{
				return _ImgKumo;
			}
		}

		public Sprite[] ImgItem{
			get{
				return _ImgItem;
			}
		}


	}
}
