using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.System;
using Mix2App.Lib.Model;
using Mix2App.Lib.View;

namespace Mix2App.MachiCon{
	public class Message : MonoBehaviour {
		[SerializeField] private GameObject EventJikkyou;
		[SerializeField] private GameObject EventJikkyouText;

		[SerializeField] private GameObject	EventSoudanSprite;		// アピールタイトル表示スプライト
		[SerializeField] private Sprite 	EventSoudanSprite2;		// 表示オフ
		[SerializeField] private Sprite[]	EventMan1;				// みーつユーザー男の子のタイトル表示スプライトリスト
		[SerializeField] private Sprite[]	EventWoman1;			// みーつユーザー女の子のタイトル表示スプライトリスト
		[SerializeField] private Sprite[]	EventMan2;				// みーつユーザー以外男の子のタイトル表示スプライトリスト
		[SerializeField] private Sprite[]	EventWoman2;			// みーつユーザー以外女の子のタイトル表示スプライトリスト



		private readonly string[] JikkyouMesType01 = new string[]{
			"レディース エ～ンド ジェントルマン！ようこそ たまキュン♥パーティへ！",
			"すてきな であいのばしょ・・・ たまキュン♥パーティへ ようこそ！",
			"イエ～イ♪ はっぴ～か～い！ たまキュン♥パーティはじまる よ！",
		};
		private readonly string[] JikkyouMesType02 = new string[]{
			"カムヒア～！まずは このダンディから にゅうじょうだ！",
			"さっそく ステキなだんしを よんでみよう！",
		};
		private readonly string[] JikkyouMesType03 = new string[]{
			"おつぎは どんなイケメンボーイだい！？	",
			"う～ん なかなかのステキオーラ！つぎいってみよう！",
		};
		private readonly string[] JikkyouMesType04 = new string[]{
			"おつぎは かわいいプリンセスの とうじょう だ！",
			"おまたせ！ガールズのみなさん でばんですよ！",
		};
		private readonly string[] JikkyouMesType05 = new string[]{
			"キラキラかがやいてまぶしいねっ！つぎいってみよう！",
			"きた～っ！かわゆい このこのにゅうじょう だ！",
		};
		private readonly string[] JikkyouMesType06 = new string[]{
			"このメンバーで たまキュン♥パーティのスタートだ！",
			"まずは そうだんタイムスタート！",
		};
		private readonly string[] JikkyouMesType07 = new string[]{
			"どんな そうだんを してるのかな？",
			"だれに こくはくするか・・・みんな なやむよね！",
		};
		private readonly string[] JikkyouMesType08 = new string[]{
			"おまたせしました！アピールタ～イム！",
			"じゅんびはOK？アピールタイムのスタートだ！",
		};
		private readonly string[] JikkyouMesType09 = new string[]{
			"おーっと！きになる あのこに モーレツアピール！",
			"なごやかな ふんいきに なってきました！",
		};
		private readonly string[] JikkyouMesType10 = new string[]{
			"ゆうきを だして！こ～くは～くタ～イム！",
			"おまちかね！？ドッキドキの こくはくタイム！",
		};
		private readonly string[] JikkyouMesType11 = new string[]{
			"いったぁ～！これぞ うんめいの しゅんかん！",
			"このストレートなおもいは かのじょにとどくのか！？",
		};
		private readonly string[] JikkyouMesType12 = new string[]{
			"きたぁ～！！ライバルの とうじょうだ！",
			"ここでくるのか！こいの ゆくえはいかに！",
		};
		private readonly string[] JikkyouMesType13 = new string[]{
			"おめでとう！ステキなカップルの たんじょうです！",
			"なんと！まさかの こくはくOK これはよそうがい！",
		};
		private readonly string[] JikkyouMesType14 = new string[]{
			"あわわっ・・・これは おどろき もものき さんしょのき！",
			"ざんねん！おたがい よくがんばり ました！",
		};
		private readonly string[] JikkyouMesType15 = new string[]{
			"またの おこしを おまちしてま～す！",
			"ではまた おあいしましょう！アプリっちでした！",
		};
			
		private readonly string[] KokuhakuReturnMesType01 = new string[]{
			"ありがとう・・・（＋語尾）",
			"こちらこそおねがいします（＋語尾）",
			"わたしもだいすき（＋語尾）",
			"なかよくしてください（＋語尾）",
			"すこしかんげきしちゃった（＋語尾）",
		};
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
			
		}

		void Destroy(){
			
		}

		void Update(){

		}	

		public void JikkyouMesDisp(JikkyouMesTable flag){
			switch (flag) {
			case	JikkyouMesTable.JikkyouMesDisp01:
				{
					// 開始時	
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType01 [Random.Range (0, JikkyouMesType01.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp02:
				{
					// 男の子１人目入場時
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType02 [Random.Range (0, JikkyouMesType02.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp03:
				{
					// 男の子２～４人目入場時
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType03 [Random.Range (0, JikkyouMesType03.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp04:
				{
					// 女の子１人目入場時
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType04 [Random.Range (0, JikkyouMesType04.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp05:
				{
					// 女の子２～４人目入場時
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType05 [Random.Range (0, JikkyouMesType05.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp06:
				{
					// 全員入場
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType06 [Random.Range (0, JikkyouMesType06.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp07:
				{
					// 相談待ち
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType07 [Random.Range (0, JikkyouMesType07.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp08:
				{
					// アピールタイム開始時
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType08 [Random.Range (0, JikkyouMesType08.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp09:
				{
					// アピールタイム中
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType09 [Random.Range (0, JikkyouMesType09.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp10:
				{
					// 告白開始
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType10 [Random.Range (0, JikkyouMesType10.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp11:
				{
					// キャラ告白後
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType11 [Random.Range (0, JikkyouMesType11.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp12:
				{
					// ちょっとまった！後
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType12 [Random.Range (0, JikkyouMesType12.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp13:
				{
					// 告白成功後
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType13 [Random.Range (0, JikkyouMesType13.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp14:
				{
					// 告白失敗後
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType14 [Random.Range (0, JikkyouMesType14.Length)];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp15:
				{
					// お疲れ様
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType15 [Random.Range (0, JikkyouMesType15.Length)];
					break;
				}
			default:
				{
					EventJikkyouText.GetComponent<Text> ().text = "";
					break;
				}
			}
		}

		public void SoudanMesDisp(SoudanMesTable flag){
			switch (flag) {
			case	SoudanMesTable.SoudanMesDispMan1:
				{
					// みーつユーザー男の子
					EventSoudanSprite.GetComponent<SpriteRenderer> ().sprite = EventMan1 [Random.Range (0, EventMan1.Length)];
					break;
				}
			case	SoudanMesTable.SoudanMesDispWoman1:
				{
					// みーつユーザー女の子
					EventSoudanSprite.GetComponent<SpriteRenderer> ().sprite = EventWoman1 [Random.Range (0, EventWoman1.Length)];
					break;
				}
			case	SoudanMesTable.SoudanMesDispMan2:
				{
					// みーつユーザー以外男の子
					EventSoudanSprite.GetComponent<SpriteRenderer> ().sprite = EventMan2 [Random.Range (0, EventMan2.Length)];
					break;
				}
			case	SoudanMesTable.SoudanMesDispWoman2:
				{
					// みーつユーザー以外女の子
					EventSoudanSprite.GetComponent<SpriteRenderer> ().sprite = EventWoman2 [Random.Range (0, EventWoman2.Length)];
					break;
				}
			default:
				{
					EventSoudanSprite.GetComponent<SpriteRenderer> ().sprite = EventSoudanSprite2;
					break;
				}
			}
		}

		public string KokuhakuMesDisp(KokuhakuMesTable flag){
			string	retMes = "";

			switch (flag) {
			case	KokuhakuMesTable.KokuhakuMesDispOK:
				{
					retMes = KokuhakuReturnMesType01 [Random.Range (0, KokuhakuReturnMesType01.Length)];
					break;
				}
			case	KokuhakuMesTable.KokuhakuMesDispNo:
				{
					retMes = KokuhakuReturnMesType02 [Random.Range (0, KokuhakuReturnMesType02.Length)];
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
