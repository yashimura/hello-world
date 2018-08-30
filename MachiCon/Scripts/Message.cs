using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

namespace Mix2App.MachiCon{
	public class Message : MonoBehaviour {
		[SerializeField] private GameObject EventJikkyou1;			// 実況の吹き出し１
		[SerializeField] private GameObject EventJikkyou2;			// 実況の吹き出し２
		[SerializeField] private GameObject EventJikkyouText;		// 実況の表示テキスト１
		[SerializeField] private GameObject EventJikkyouText2;		// 実況の表示テキスト２
		[SerializeField] private GameObject EventJikkyouAplichi1;	// アプリっちの表示スプライト１
		[SerializeField] private GameObject EventJikkyouAplichi2;	// アプリっちの表示スプライト２
		[SerializeField] private Sprite[]	EventJikkyouImage;		// アプリっちの表示スプライトリスト（普通 = 0、ガイド = 1、喜び = 2、笑顔 = 3、驚き = 4、泣き = 5、普通２ = 6）

		[SerializeField] private GameObject	EventSoudanText;		// アピールタイトル表示テキスト





		// 開始時	
		private readonly string[] JikkyouMesType01 = new string[]{
			"レディース エ～ンド ジェントルマン！\nようこそ たまキュン♥パーティへ！",
			"すてきな であいのばしょ・・・\nたまキュン♥パーティへ ようこそ！",
			"イエ～イ♪ はっぴ～か～い！\nたまキュン♥パーティはじまる よ！",
		};
		private int[] JikkyouImageType01 = new int[]{			// ガイド、ガイド、ガイド
			1,1,1,
		};
		// 男の子１人目入場時
		private readonly string[] JikkyouMesType02 = new string[]{
			"カムヒア～！\nまずは このダンディから にゅうじょうだ！",
			"さっそく\nステキなだんしを よんでみよう！",
		};
		private int[] JikkyouImageType02 = new int[]{			// 喜び、喜び
			2,2,
		};
		// 男の子２～４人目入場時
		private readonly string[] JikkyouMesType03 = new string[]{
			"おつぎは\nどんなイケメンボーイだい！？	",
			"う～ん なかなかのステキオーラ！\nつぎいってみよう！",
		};
		private int[] JikkyouImageType03 = new int[]{			// 普通、笑顔
			0,3,
		};
		// 女の子１人目入場時
		private readonly string[] JikkyouMesType04 = new string[]{
			"おつぎは\nかわいいプリンセスの とうじょう だ！",
			"おまたせ！\nガールズのみなさん でばんですよ！",
		};
		private int[] JikkyouImageType04 = new int[]{			// 喜び、喜び
			2,2,
		};
		// 女の子２～４人目入場時
		private readonly string[] JikkyouMesType05 = new string[]{
			"キラキラかがやいてまぶしいねっ！\nつぎいってみよう！",
			"きた～っ！\nかわゆい このこのにゅうじょう だ！",
		};
		private int[] JikkyouImageType05 = new int[]{			// 笑顔、笑顔
			3,3,
		};
		// 全員入場
		private readonly string[] JikkyouMesType06 = new string[]{
			"このメンバーで\nたまキュン♥パーティのスタートだ！",
			"まずは そうだんタイムスタート！",
		};
		private int[] JikkyouImageType06 = new int[]{			// 喜び、ガイド
			2,1,
		};
		// 相談待ち
		private readonly string[] JikkyouMesType07 = new string[]{
			"どんな そうだんを してるのかな？",
			"だれに こくはくするか・・・\nみんな なやむよね！",
		};
		private int[] JikkyouImageType07 = new int[]{			// 普通、普通
			0,0,
		};
		// アピールタイム開始時
		private readonly string[] JikkyouMesType08 = new string[]{
			"おまたせしました！\nアピールタ～イム！",
			"じゅんびはOK？\nアピールタイムのスタートだ！",
		};
		private int[] JikkyouImageType08 = new int[]{			// 喜び、笑顔
			2,3,
		};
		// アピールタイム中
		private readonly string[] JikkyouMesType09 = new string[]{
			"おーっと！\nきになる あのこに モーレツアピール！",
			"なごやかな ふんいきに\nなってきました！",
		};
		private int[] JikkyouImageType09 = new int[]{			// 驚き、笑顔
			4,3,
		};
		// 告白開始
		private readonly string[] JikkyouMesType10 = new string[]{
			"ゆうきを だして！\nこ～くは～くタ～イム！",
			"おまちかね！？\nドッキドキの こくはくタイム！",
		};
		private int[] JikkyouImageType10 = new int[]{			// 喜び、喜び
			2,2,
		};
		// キャラ告白後
		private readonly string[] JikkyouMesType11 = new string[]{
			"いったぁ～！\nこれぞ うんめいの しゅんかん！",
			"このストレートなおもいは\nかのじょにとどくのか！？",
		};
		private int[] JikkyouImageType11 = new int[]{			// 笑顔、普通
			3,0,
		};
		// ちょっとまった！後
		private readonly string[] JikkyouMesType12 = new string[]{
			"きたぁ～！！\nライバルの とうじょうだ！",
			"ここでくるのか！\nこいの ゆくえはいかに！",
		};
		private int[] JikkyouImageType12 = new int[]{			// 驚き、驚き
			4,4,
		};
		// 告白成功後
		private readonly string[] JikkyouMesType13 = new string[]{
			"おめでとう！\nステキなカップルの たんじょうです！",
			"なんと！\nまさかの こくはくOK これはよそうがい！",
		};
		private int[] JikkyouImageType13 = new int[]{			// 喜び、笑顔
			2,3,
		};
		// 告白失敗後
		private readonly string[] JikkyouMesType14 = new string[]{
			"あわわっ・・・\nこれは おどろき もものき さんしょのき！",
			"ざんねん！\nおたがい よくがんばり ました！",
		};
		private int[] JikkyouImageType14 = new int[]{			// 驚き、泣き
			4,5,
		};
		// お疲れ様
		private readonly string[] JikkyouMesType15 = new string[]{
			"またの おこしを おまちしてま～す！",
			"ではまた おあいしましょう！\nアプリっちでした！",
		};
		private int[] JikkyouImageType15 = new int[]{			// 笑顔、笑顔
			3,3,
		};
			
		// 相談タイム用男の子みーつユーザー
		private readonly string[] SoudanTimeMesManType01 = new string[] {
			"このコが すきかもしれない（＋語尾）",
			"このコに こくはくしたい（＋語尾）",
			"このコを みているとドキドキする（＋語尾）",
		};
		// 相談タイム用女の子みーつユーザー
		private readonly string[] SoudanTimeMesWomanType01 = new string[] {
			"このかた わたしのことがすきかも（＋語尾）",
			"こくはくされたら どうする（＋語尾）",
			"むねの たかまりが とまらない（＋語尾）",
		};
		// 相談タイム用男の子みーつユーザー以外
		private readonly string[] SoudanTimeMesManType02 = new string[] {
			"このコが きになりますか？",
			"このコに こくはくしたいですか？",
			"このコを みているとドキドキしますか",
		};
		// 相談タイム用女の子みーつユーザー以外
		private readonly string[] SoudanTimeMesWomanType02 = new string[] {
			"このコと なかよくしたいですか？",
			"このコに こくはくしてほしいですか？",
			"このコと カップルになれるといいですね",
		};

		// 告白宣言メッセージ
		private readonly string[] KokuhakuAttackMesType01 = new string[] {
			"すき（＋語尾） けっこんして（＋語尾）",
			"ひとめぼれ（＋語尾） けっこんして（＋語尾）",
			"もうあなたしかかんがえられない（＋語尾）",
			"ぜったいしあわせにする（＋語尾）",
			"いっしょになってほしい（＋語尾）",
		};

		// 告白受諾メッセージ
		private readonly string[] KokuhakuReturnMesType01 = new string[]{
			"ありがとう・・・（＋語尾）",
			"こちらこそおねがいします（＋語尾）",
			"わたしもだいすき（＋語尾）",
			"なかよくしてください（＋語尾）",
			"すこしかんげきしちゃった（＋語尾）",
		};
		// 告白拒否メッセージ
		private readonly string[] KokuhakuReturnMesType02 = new string[]{
			"ごめんなさい・・・（＋語尾）",
			"ほかにすきなひとがいるの（＋語尾）",
			"すこしかんがえさせてください（＋語尾）",
			"おともだちでいましょう（＋語尾）",
			"ほんとうにごめんなさい（＋語尾）",
		};


		public enum JikkyouMesTable{
			JikkyouMesDisp01,		// 開始時
			JikkyouMesDisp02,		// 男の子１人目入場時
			JikkyouMesDisp03,		// 男の子２～４人目入場時
			JikkyouMesDisp04,		// 女の子１人目入場時
			JikkyouMesDisp05,		// 女の子２～４人目入場時
			JikkyouMesDisp06,		// 全員入場
			JikkyouMesDisp07,		// 相談待ち
			JikkyouMesDisp08,		// アピールタイム開始時
			JikkyouMesDisp09,		// アピールタイム中
			JikkyouMesDisp10,		// 告白開始
			JikkyouMesDisp11,		// キャラ告白後
			JikkyouMesDisp12,		// ちょっとまった！後
			JikkyouMesDisp13,		// 告白成功後
			JikkyouMesDisp14,		// 告白失敗後
			JikkyouMesDisp15,		// お疲れ様
			JikkyouMesDispOff,		// 表示オフ
		};

		public enum SoudanMesTable{
			SoudanMesDispMan1,		// みーつユーザー男の子
			SoudanMesDispWoman1,	// みーつユーザー女の子
			SoudanMesDispMan2,		// みーつユーザー以外の男の子
			SoudanMesDispWoman2,	// みーつユーザー以外の女の子
			SoudanMesDispOff,		// 表示オフ
		};

		public enum KokuhakuMesTable{
			KokuhakuMesDispOK,		// 告白OK
			KokuhakuMesDispNo,		// 告白No
			KokuhakuMesDispOff,		// 表示オフ
		};

		void Awake(){
			
		}

		void Start(){
			jikkyouCharaAnimeNumber = 0;
			countTime1 = 0.0f;
			countTime2 = 0;
		}

		void Destroy(){
			
		}



		private int jikkyouCharaAnimeNumber;
		private float countTime1;
		private int countTime2;
		void Update(){

			countTime1 += 1.0f * Time.deltaTime;
			if (countTime1 >= 0.5f) {
				countTime1 -= 0.5f;
				countTime2++;
			}

			switch (jikkyouCharaAnimeNumber) {
			case	0:
				{
					if ((countTime2 & 1) != 0) {
						EventJikkyouAplichi1.GetComponent<SpriteRenderer> ().sprite = EventJikkyouImage [0];
						EventJikkyouAplichi2.GetComponent<Image> ().sprite = EventJikkyouImage [0];
					} else {
						EventJikkyouAplichi1.GetComponent<SpriteRenderer> ().sprite = EventJikkyouImage [6];
						EventJikkyouAplichi2.GetComponent<Image> ().sprite = EventJikkyouImage [6];
					}

					break;
				}
			case	1:
				{
					EventJikkyouAplichi1.GetComponent<SpriteRenderer> ().sprite = EventJikkyouImage [1];
					EventJikkyouAplichi2.GetComponent<Image> ().sprite = EventJikkyouImage [1];
					break;
				}
			case	2:
				{
					EventJikkyouAplichi1.GetComponent<SpriteRenderer> ().sprite = EventJikkyouImage [2];
					EventJikkyouAplichi2.GetComponent<Image> ().sprite = EventJikkyouImage [2];
					break;
				}
			case	3:
				{
					EventJikkyouAplichi1.GetComponent<SpriteRenderer> ().sprite = EventJikkyouImage [3];
					EventJikkyouAplichi2.GetComponent<Image> ().sprite = EventJikkyouImage [3];
					break;
				}
			case	4:
				{
					EventJikkyouAplichi1.GetComponent<SpriteRenderer> ().sprite = EventJikkyouImage [4];
					EventJikkyouAplichi2.GetComponent<Image> ().sprite = EventJikkyouImage [4];
					break;
				}
			case	5:
				{
					EventJikkyouAplichi1.GetComponent<SpriteRenderer> ().sprite = EventJikkyouImage [5];
					EventJikkyouAplichi2.GetComponent<Image> ().sprite = EventJikkyouImage [5];
					break;
				}
			}
		}	

		public void JikkyouMesDisp(JikkyouMesTable flag){
			int randNum;

			EventJikkyou1.SetActive (true);
			EventJikkyou2.SetActive (true);

			switch (flag) {
			case	JikkyouMesTable.JikkyouMesDisp01:
				{
					// 開始時	
					randNum = Random.Range (0, JikkyouMesType01.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType01 [randNum];

					jikkyouCharaAnimeNumber =  JikkyouImageType01 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp02:
				{
					// 男の子１人目入場時
					randNum = Random.Range (0, JikkyouMesType02.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType02 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType02 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp03:
				{
					// 男の子２～４人目入場時
					randNum = Random.Range (0, JikkyouMesType03.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType03 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType03 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp04:
				{
					// 女の子１人目入場時
					randNum = Random.Range (0, JikkyouMesType04.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType04 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType04 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp05:
				{
					// 女の子２～４人目入場時
					randNum = Random.Range (0, JikkyouMesType05.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType05 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType05 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp06:
				{
					// 全員入場
					randNum = Random.Range (0, JikkyouMesType06.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType06 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType06 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp07:
				{
					// 相談待ち
					randNum = Random.Range (0, JikkyouMesType07.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType07 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType07 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp08:
				{
					// アピールタイム開始時
					randNum = Random.Range (0, JikkyouMesType08.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType08 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType08 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp09:
				{
					// アピールタイム中
					randNum = Random.Range (0, JikkyouMesType09.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType09 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType09 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp10:
				{
					// 告白開始
					randNum = Random.Range (0, JikkyouMesType10.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType10 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType10 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp11:
				{
					// キャラ告白後
					randNum = Random.Range (0, JikkyouMesType11.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType11 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType11 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp12:
				{
					// ちょっとまった！後
					randNum = Random.Range (0, JikkyouMesType12.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType12 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType12 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp13:
				{
					// 告白成功後
					randNum = Random.Range (0, JikkyouMesType13.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType13 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType13 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp14:
				{
					// 告白失敗後
					randNum = Random.Range (0, JikkyouMesType14.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType14 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType14 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp15:
				{
					// お疲れ様
					randNum = Random.Range (0, JikkyouMesType15.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType15 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType15 [randNum];
					break;
				}
			default:
				{
					EventJikkyouText.GetComponent<Text> ().text = "";
					EventJikkyou1.SetActive (false);
					EventJikkyou2.SetActive (false);
					jikkyouCharaAnimeNumber = 0;
					break;
				}
			}
			EventJikkyouText2.GetComponent<Text> ().text = EventJikkyouText.GetComponent<Text> ().text;
		}

		// 相談タイム用
		public void SoudanMesDisp(SoudanMesTable flag,string _gobi = ""){
			bool _flag = true;

			switch (flag) {
			case	SoudanMesTable.SoudanMesDispMan1:
				{
					// みーつユーザー男の子
					EventSoudanText.GetComponent<Text> ().text = SoudanTimeMesManType01 [Random.Range (0, SoudanTimeMesManType01.Length)].Replace("（＋語尾）",_gobi);
					break;
				}
			case	SoudanMesTable.SoudanMesDispWoman1:
				{
					// みーつユーザー女の子
					EventSoudanText.GetComponent<Text> ().text = SoudanTimeMesWomanType01 [Random.Range (0, SoudanTimeMesWomanType01.Length)].Replace("（＋語尾）",_gobi);
					break;
				}
			case	SoudanMesTable.SoudanMesDispMan2:
				{
					// みーつユーザー以外男の子
					EventSoudanText.GetComponent<Text> ().text = SoudanTimeMesManType02 [Random.Range (0, SoudanTimeMesManType02.Length)];
					break;
				}
			case	SoudanMesTable.SoudanMesDispWoman2:
				{
					// みーつユーザー以外女の子
					EventSoudanText.GetComponent<Text> ().text = SoudanTimeMesWomanType02 [Random.Range (0, SoudanTimeMesWomanType02.Length)];
					break;
				}
			default:
				{
					EventSoudanText.GetComponent<Text> ().text = "";
					_flag = false;
					break;
				}
			}

			EventSoudanText.SetActive (_flag);
		}


		public string KokuhakuMesDispMan(string _gobi){
			return KokuhakuAttackMesType01 [Random.Range (0, KokuhakuAttackMesType01.Length)].Replace("（＋語尾）",_gobi);
		}

		public string KokuhakuMesDisp(KokuhakuMesTable flag,string _gobi){
			string	retMes = "";

			switch (flag) {
			case	KokuhakuMesTable.KokuhakuMesDispOK:
				{
					// 告白受諾メッセージ
					retMes = KokuhakuReturnMesType01 [Random.Range (0, KokuhakuReturnMesType01.Length)].Replace("（＋語尾）",_gobi);
					break;
				}
			case	KokuhakuMesTable.KokuhakuMesDispNo:
				{
					// 告白拒否メッセージ
					retMes = KokuhakuReturnMesType02 [Random.Range (0, KokuhakuReturnMesType02.Length)].Replace("（＋語尾）",_gobi);
					break;
				}
			default:
				{
					retMes = "";
					break;
				}
			}
			return retMes;
		}

	}
}
