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
	public class MessageEv99 : MonoBehaviour {
		[SerializeField] private GameObject EventJikkyou1;			// 実況の吹き出し１
		[SerializeField] private GameObject EventJikkyou2;			// 実況の吹き出し２
		[SerializeField] private GameObject EventJikkyouText;		// 実況の表示テキスト１
		[SerializeField] private GameObject EventJikkyouText2;		// 実況の表示テキスト２

		private Animator chAnim1;		// 実況のアニメーター
		private Animator chAnim2;		// 実況のアニメーター

		[SerializeField] private GameObject EventJikkyouAplichi1;	// アプリっちの表示スプライト１
		[SerializeField] private GameObject EventJikkyouAplichi2;	// アプリっちの表示スプライト２
		[SerializeField] private Sprite[]	EventJikkyouImage;		// アプリっちの表示スプライトリスト（普通 = 0、ガイド = 1、喜び = 2、笑顔 = 3、驚き = 4、泣き = 5、普通２ = 6、困る = 7）

		[SerializeField] private GameObject	EventSoudanText;		// アピールタイトル表示テキスト


		/// <summary>
		/// jikkyou_chにアタッチしてるAnimatorのparameter[status]の設定値
		/// Animatorはこのstatusを変更することでanimationを切り替えるようにしている
		/// </summary>
		private enum AnimId
		{
			Default,
			Guide,
			Happy,
			Komaru,
			Naku,
			Ororoku,
			Smile
		}

		// 開始時	
		private readonly string[] JikkyouMesType01 = new string[]{
			"レディース エ～ンド ジェントルマン！\nようこそ たまキュン♥パーティーへ！ しんこうは やべっちにまかしとき！",
			"たまキュン♥パーティーへ ようこそ！\nおかザルっちと たのしんでってや！",
			"さんかしゃ ぼしゅうちゅう・・・\nしばらくまっとってや！",
		};
		private JikkyouImageTable[] JikkyouImageType01 = new JikkyouImageTable[]{			// ガイド、ガイド、ガイド
			JikkyouImageTable.GUIDE,
			JikkyouImageTable.GUIDE,
			JikkyouImageTable.GUIDE,
		};
		// 男の子１人目入場時
		private readonly string[] JikkyouMesType02 = new string[]{
			"カモ～ン！\nまずは ステキなたまごっちを しょうかいするで！",
			"まずは このたまごっち！\nいいかんじじゃ ないすか！",
		};
		private JikkyouImageTable[] JikkyouImageType02 = new JikkyouImageTable[]{			// 喜び、喜び
			JikkyouImageTable.HAPPY,
			JikkyouImageTable.HAPPY,
		};
		// 男の子２～４人目入場時
		private readonly string[] JikkyouMesType03 = new string[]{
			"おつぎのかた ど～ぞ～！",
			"みんな おかザルっちに あいたいんかな？\nおかザルっち ドキドキしとるやろな～",
		};
		private JikkyouImageTable[] JikkyouImageType03 = new JikkyouImageTable[]{			// 普通、笑顔
			JikkyouImageTable.NORMAL,
			JikkyouImageTable.SMILE,
		};
		// 女の子１人目入場時
		private readonly string[] JikkyouMesType04 = new string[]{
			"おっ！ かわいいナウたまやね！\nいろんな ナウたまがいて びっくりするわ～",
			"ここからは かわいいナウたまのでばんじゃないすか！\nはりきっていこか～！",
		};
		private JikkyouImageTable[] JikkyouImageType04 = new JikkyouImageTable[]{			// 喜び、喜び
			JikkyouImageTable.HAPPY,
			JikkyouImageTable.HAPPY,
		};
		// 女の子２～４人目入場時
		private readonly string[] JikkyouMesType05 = new string[]{
			"たまごっちって ごっつ～　しんか したんやね！\nこせいてきでみんなキャラたってるな～！",
			"おかザルっちと けっこんできるのは だれか！？\nどんなことになるか たのしみやね～",
		};
		private JikkyouImageTable[] JikkyouImageType05 = new JikkyouImageTable[]{			// 笑顔、笑顔
			JikkyouImageTable.SMILE,
			JikkyouImageTable.SMILE,
		};
		// 全員入場
		private readonly string[] JikkyouMesType06 = new string[]{
			"このメンバーで たまキュン♥パーティーやるで！",
			"まずは そうだんタイムからはじめるで！",
		};
		private JikkyouImageTable[] JikkyouImageType06 = new JikkyouImageTable[]{			// 喜び、ガイド
			JikkyouImageTable.HAPPY,
			JikkyouImageTable.GUIDE,
		};
		// 相談待ち
		private readonly string[] JikkyouMesType07 = new string[]{
			"かんがえすぎは あかんよ！？",
			"だれに こくはくするか・・・おかザルっちでしょ！？",
		};
		private JikkyouImageTable[] JikkyouImageType07 = new JikkyouImageTable[]{			// 普通、普通
			JikkyouImageTable.NORMAL,
			JikkyouImageTable.NORMAL,
		};
		// アピールタイム開始時
		private readonly string[] JikkyouMesType08 = new string[]{
			"さぁ・・・めいいっぱい アピールしたってや！",
			"じゅんび できてますの？\nアピールタイムで もーれつにアピールせな！",
		};
		private JikkyouImageTable[] JikkyouImageType08 = new JikkyouImageTable[]{			// 喜び、笑顔
			JikkyouImageTable.HAPPY,
			JikkyouImageTable.SMILE,
		};
		// アピールタイム中
		private readonly string[] JikkyouMesType09 = new string[]{
			"おーっと！\nなかなか せめるやん！がんばりや～",
			"もっと おかザルっちの あいてしたげて！",
			"キャ～ おかザルっちさん ステキですよ！\nそうそう そこでもっと ぐぐっと！",
			"パーティーさんかのみなさ～ん！\nおかザルっちさんは にげませんよ～",
			"あかんて おかザルっちさん\nフラフラしすぎですやん・・・",
		};
		private JikkyouImageTable[] JikkyouImageType09 = new JikkyouImageTable[]{			// 驚く、笑顔、喜ぶ、ガイド、困る
			JikkyouImageTable.SURPRISE,
			JikkyouImageTable.SMILE,
			JikkyouImageTable.HAPPY,
			JikkyouImageTable.GUIDE,
			JikkyouImageTable.TROUBLE,
		};
		// 告白開始
		private readonly string[] JikkyouMesType10 = new string[]{
			"ついに こくはくタイムですよ\nないても わらっても これできまってまうよ～",
			"ドッキドキの こくはくタ～イム！\nおかザルっちさんと カップルになるのはだれ！？",
		};
		private JikkyouImageTable[] JikkyouImageType10 = new JikkyouImageTable[]{			// 喜び、喜び
			JikkyouImageTable.HAPPY,
			JikkyouImageTable.HAPPY,
		};
		// キャラ告白後
		private readonly string[] JikkyouMesType11 = new string[]{
			"いったぁ～！\nこれぞ うんめいの しゅんかん！",
			"いいね！そのストレートなかんじ きらいじゃないで！",
		};
		private JikkyouImageTable[] JikkyouImageType11 = new JikkyouImageTable[]{			// 笑顔、普通
			JikkyouImageTable.SMILE,
			JikkyouImageTable.NORMAL,
		};
		// ちょっとまった！後
		private readonly string[] JikkyouMesType12 = new string[]{
			"あかん！！ライバルの とうじょうですよ！！",
			"びっくりした！モテモテやないですかっ！？",
		};
		private JikkyouImageTable[] JikkyouImageType12 = new JikkyouImageTable[]{			// 驚き、驚き
			JikkyouImageTable.SURPRISE,
			JikkyouImageTable.SURPRISE,
		};
		// 告白成功後
		private readonly string[] JikkyouMesType13 = new string[]{
			"おめでとう！\nステキなカップルだと おもいますよ！",
			"なんと！\nいやぁ～ いいもん みせてもらいましたわ～",
		};
		private JikkyouImageTable[] JikkyouImageType13 = new JikkyouImageTable[]{			// 喜び、笑顔
			JikkyouImageTable.HAPPY,
			JikkyouImageTable.SMILE,
		};
		// 告白失敗後
		private readonly string[] JikkyouMesType14 = new string[]{
			"マジっすか！？",
			"あちゃ～！\nつぎ がんばりましょ！",
		};
		private JikkyouImageTable[] JikkyouImageType14 = new JikkyouImageTable[]{			// 驚き、泣き
			JikkyouImageTable.SURPRISE,
			JikkyouImageTable.CRY,
		};
		// お疲れ様
		private readonly string[] JikkyouMesType15 = new string[]{
			"おかザルっちと いっしょにまってるで～！",
			"またあおな！おかザルっちと なかよくしたってや～！",
		};
		private JikkyouImageTable[] JikkyouImageType15 = new JikkyouImageTable[]{			// 笑顔、笑顔
			JikkyouImageTable.SMILE,
			JikkyouImageTable.SMILE,
		};
			
		// 相談タイム用男の子みーつユーザー
		private readonly string[] SoudanTimeMesManType01 = new string[] {
			"このコと なかよくしたい（＋語尾）",
			"このコが きになる（＋語尾）",
			"このコを みているとドキドキする（＋語尾）",
		};
		// 相談タイム用女の子みーつユーザー
		private readonly string[] SoudanTimeMesWomanType01 = new string[] {
			"このコと なかよくしたい（＋語尾）",
			"このコが きになる（＋語尾）",
			"このコを みているとドキドキする（＋語尾）",
		};
		// 相談タイム用男の子みーつユーザー以外
		private readonly string[] SoudanTimeMesManType02 = new string[] {
			"このコのこと どうおもう？",
			"このコと デートにいきたいな",
			"このコと なかよくなりたいな",
		};
		// 相談タイム用女の子みーつユーザー以外
		private readonly string[] SoudanTimeMesWomanType02 = new string[] {
			"このコのこと どうおもう？",
			"このコと デートにいけたらいいな",
			"このコと なかよくなりたいな",
		};

		// 告白宣言メッセージ
		private readonly string[] KokuhakuAttackMesType01 = new string[] {
			"すきです！けっこんしたい（＋語尾）",
			"ひとめぼれ・・・けっこんしたい（＋語尾）",
			"もう あなたしかいない（＋語尾）",
			"あなたを しあわせにしたい（＋語尾）",
			"いっしょになってほしい（＋語尾）",
		};

		// 告白受諾メッセージ
		private readonly string[] KokuhakuReturnMesType01 = new string[]{
			"あなたと いっしょにいたい（＋語尾）",
			"むねのドキドキが とまらない（＋語尾）",
			"あなたと けっこんしたい（＋語尾）",
			"なかよくしてほしい（＋語尾）",
			"かんげきしちゃった（＋語尾）",
		};
		// 告白拒否メッセージ
		private readonly string[] KokuhakuReturnMesType02 = new string[]{
			"あきらめてほしい（＋語尾）",
			"ほかに すきなあいてがいる（＋語尾）",
			"すこし かんがえたい（＋語尾）",
			"おともだちならいい（＋語尾）",
			"あなたと けっこんできない（＋語尾）",
		};

		// コラボ告白宣言メッセージ
		private readonly string[] CoraboKokuhakuAttackMes = new string[] {
			"めっちゃすきやねん！けっこんしてほしいわ～",
		};

		// コラボ告白受諾メッセージ
		private readonly string[] CoraboKokuhakuReturnMes01 = new string[] {
			"めっちゃすきやねん！けっこんしてほしいわ～",
		};

		private enum JikkyouImageTable{
			NORMAL,					// 普通
			GUIDE,					// ガイド
			HAPPY,					// 喜び
			SMILE,					// 笑顔
			SURPRISE,				// 驚き
			CRY,					// 泣き
			TROUBLE,				// 困る
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
			chAnim1 = EventJikkyouAplichi1.GetComponent<Animator>();
			chAnim2 = EventJikkyouAplichi2.GetComponent<Animator>();
		}

		void Start(){
			jikkyouCharaAnimeNumber = 0;
			//countBaseTime1 = 0.0f;
			//countBaseTime2 = 0.0f;
			//countTime1 = 0;
			//countTime2 = 0;
			//countTime3 = 0;
			//SurpriseFlag = false;
		}

		void Destroy(){
			//SurpriseFlag = false;
		}



		private JikkyouImageTable jikkyouCharaAnimeNumber;
		//private bool SurpriseFlag;
		//private float countBaseTime1;
		//private float countBaseTime2;
//		private int countTime1;
		//private int countTime2;
		//private int countTime3;

		private Coroutine retImage,retSprite;

		void Update(){

			switch (jikkyouCharaAnimeNumber) {
			case	JikkyouImageTable.NORMAL:
				{	// 普通
					if (chAnim1.isActiveAndEnabled) chAnim1.SetInteger("status", (int)AnimId.Default);
					if (chAnim2.isActiveAndEnabled) chAnim2.SetInteger("status", (int)AnimId.Default);
					break;
				}
			case	JikkyouImageTable.GUIDE:
				{	// ガイド
					if (chAnim1.isActiveAndEnabled) chAnim1.SetInteger("status", (int)AnimId.Guide);
					if (chAnim2.isActiveAndEnabled) chAnim2.SetInteger("status", (int)AnimId.Guide);
					break;
				}
			case	JikkyouImageTable.HAPPY:
				{	// 喜び
					if (chAnim2.isActiveAndEnabled) chAnim2.SetInteger("status", (int)AnimId.Happy);
					break;
				}
			case	JikkyouImageTable.SMILE:
				{	// 笑顔
					if (chAnim2.isActiveAndEnabled) chAnim2.SetInteger("status", (int)AnimId.Smile);
					break;
				}
			case	JikkyouImageTable.SURPRISE:
				{	// 驚き
					if (chAnim2.isActiveAndEnabled) chAnim2.SetInteger("status", (int)AnimId.Ororoku);
					break;
				}
			case	JikkyouImageTable.CRY:
				{	// 泣き
					if (chAnim2.isActiveAndEnabled) chAnim2.SetInteger("status", (int)AnimId.Naku);
					break;
				}
			case	JikkyouImageTable.TROUBLE:
				{	// 困る
					if (chAnim2.isActiveAndEnabled) chAnim2.SetInteger("status", (int)AnimId.Komaru);
					break;
				}
			}
		}	

		private int JikkyouMesDisp09Flag = 0;
		public void JikkyouMesDisp(JikkyouMesTable flag){
			int randNum;

			EventJikkyou1.SetActive (true);
			EventJikkyou2.SetActive (true);

			switch (flag) {
			case	JikkyouMesTable.JikkyouMesDisp01:
				{	// 開始時	
					randNum = Random.Range (0, JikkyouMesType01.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType01 [randNum];

					jikkyouCharaAnimeNumber =  JikkyouImageType01 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp02:
				{	// 男の子１人目入場時
					randNum = Random.Range (0, JikkyouMesType02.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType02 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType02 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp03:
				{	// 男の子２～４人目入場時
					randNum = Random.Range (0, JikkyouMesType03.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType03 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType03 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp04:
				{	// 女の子１人目入場時
					randNum = Random.Range (0, JikkyouMesType04.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType04 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType04 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp05:
				{	// 女の子２～４人目入場時
					randNum = Random.Range (0, JikkyouMesType05.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType05 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType05 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp06:
				{	// 全員入場
					randNum = Random.Range (0, JikkyouMesType06.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType06 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType06 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp07:
				{	// 相談待ち
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
				{	// アピールタイム中
					while (true) {
						randNum = Random.Range (0, JikkyouMesType09.Length);
						if (randNum != JikkyouMesDisp09Flag) {
							JikkyouMesDisp09Flag = randNum;
							break;
						}
					}
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType09 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType09 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp10:
				{	// 告白開始
					randNum = Random.Range (0, JikkyouMesType10.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType10 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType10 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp11:
				{	// キャラ告白後
					randNum = Random.Range (0, JikkyouMesType11.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType11 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType11 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp12:
				{	// ちょっとまった！後
					randNum = Random.Range (0, JikkyouMesType12.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType12 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType12 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp13:
				{	// 告白成功後
					randNum = Random.Range (0, JikkyouMesType13.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType13 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType13 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp14:
				{	// 告白失敗後
					randNum = Random.Range (0, JikkyouMesType14.Length);
					EventJikkyouText.GetComponent<Text> ().text = JikkyouMesType14 [randNum];

					jikkyouCharaAnimeNumber = JikkyouImageType14 [randNum];
					break;
				}
			case	JikkyouMesTable.JikkyouMesDisp15:
				{	// お疲れ様
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
					jikkyouCharaAnimeNumber = JikkyouImageTable.NORMAL;
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
				{	// みーつユーザー女の子
					EventSoudanText.GetComponent<Text> ().text = SoudanTimeMesWomanType01 [Random.Range (0, SoudanTimeMesWomanType01.Length)].Replace("（＋語尾）",_gobi);
					break;
				}
			case	SoudanMesTable.SoudanMesDispMan2:
				{	// みーつユーザー以外男の子
					EventSoudanText.GetComponent<Text> ().text = SoudanTimeMesManType02 [Random.Range (0, SoudanTimeMesManType02.Length)];
					break;
				}
			case	SoudanMesTable.SoudanMesDispWoman2:
				{	// みーつユーザー以外女の子
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

		public string KokuhakuMesDispCorabo(){
			return CoraboKokuhakuAttackMes [Random.Range (0, CoraboKokuhakuAttackMes.Length)];
		}

		public string ReturnMesDisp(KokuhakuMesTable flag,string _gobi){
			string	retMes = "";

			switch (flag) {
			case	KokuhakuMesTable.KokuhakuMesDispOK:
				{	// 告白受諾メッセージ
					retMes = KokuhakuReturnMesType01 [Random.Range (0, KokuhakuReturnMesType01.Length)].Replace("（＋語尾）",_gobi);
					break;
				}
			case	KokuhakuMesTable.KokuhakuMesDispNo:
				{	// 告白拒否メッセージ
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

		public string ReturnMesDispCorabo(KokuhakuMesTable flag,string _gobi){
			string	retMes = "";

			switch (flag) {
			case	KokuhakuMesTable.KokuhakuMesDispOK:
				{	// 告白受諾メッセージ
					retMes = CoraboKokuhakuReturnMes01 [Random.Range (0, CoraboKokuhakuReturnMes01.Length)];
					break;
				}
			case	KokuhakuMesTable.KokuhakuMesDispNo:
				{	// 告白拒否メッセージ
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

		public void KokuhakuCurtainJikkyouOnOff(bool flag){
			EventJikkyouAplichi2.SetActive (flag);
		}

	}
}
