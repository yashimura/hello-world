////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using System;
using System.Collections;
using UnityEngine;
using Mix2App.Lib.View;
using UnityEngine.Events;
using UnityEngine.UI;
using Mix2App.Lib.Model;
using Mix2App.Lib;

namespace Mix2App.Propose {
    [Serializable] public class ActionManager:MonoBehaviour {
        [Header("Characters")]
        [SerializeField] private CharaBehaviour CharaLeft;
        [SerializeField] private CharaBehaviour CharaRight;

        [Header("Dialogs balloons")]
        [SerializeField] private GameObject LeftDialogBalloon;
        [SerializeField] private GameObject RightDialogBalloon;
        [SerializeField] private Text LeftDialogBalloonText;
        [SerializeField] private Text RightDialogBalloonText;

        [Header("Character heart")]
        [SerializeField] private Transform HeartGroup;
        [SerializeField] private Transform HeartGroupDestination;
        [SerializeField] private Transform HeartRotationCenter;

        [SerializeField] private GameObject FullHeart;

        [SerializeField] private GameObject LeftHalfHeart;
        [SerializeField] private GameObject RightHalfHeart;

        [Header("Front Hearts")]
        [SerializeField] private GameObject SuccessHearts;
        [SerializeField] private GameObject FailHearts;

        private Vector3 SuccessHeartsDestination;
        private Vector3 FailHeartsDestination;

        private TamaChara LeftCharaData;
        private TamaChara RightCharaData;

        /// <summary>
        /// Setup left(recieve side) and right(propose side) characters
        /// </summary>
        /// <param name="left">recieve side</param>
        /// <param name="right">propose side</param>
        /// <returns></returns>
        private IEnumerator Setup(TamaChara left, TamaChara right) {
            LeftCharaData = left;
            RightCharaData = right;

            RectTransform rt = SuccessHearts.GetComponent<RectTransform>();
            SuccessHeartsDestination = rt.position;
            rt.anchoredPosition = new Vector3(rt.position.x, rt.position.y - rt.rect.width* 1.6692914f, rt.position.z);

            rt = FailHearts.GetComponent<RectTransform>();
            FailHeartsDestination = rt.position;
            rt.anchoredPosition = new Vector3(rt.position.x, rt.position.y + rt.rect.width * 1.6692914f, rt.position.z);


            CharaLeft.init(left);
            CharaRight.init(right);
            while (!CharaLeft.ready || !CharaRight.ready)
                yield return null;
        }

        /// <summary>
        /// Starts coroutine to move transform from current point to destination for time
        /// </summary>
        /// <param name="transform">moved transform</param>
        /// <param name="destination">destination point</param>
        /// <param name="time">required time</param>
        /// <returns></returns>
        private IEnumerator MoveToPoint(Transform transform, Vector3 destination, float time) {
            ManagerObject.instance.sound.playSe(1);

            float speed = Vector3.Distance(transform.position, destination) / time;

            while (true) {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, destination, step);
                if (Vector3.Distance(transform.position, destination) <= step)
                    break;
                yield return null;
            }
        }

        /// <summary>
        /// Ratio for meet point.
        /// Calculated like:
        /// ([Left character]...[MeetPoint_Left: 1/WalkRatio]...[MeetPoint_Right: 1-1/WalkRatio]...[Right character])
        /// </summary>
        private const float MeetRatio = 3.0f;
        private Vector3 MeetPoint_Right {
            get {
                return (CharaLeft.transform.position + CharaRight.transform.position * (MeetRatio - 1.0f)) / MeetRatio;
            }
        }
        private Vector3 MeetPoint_Left {
            get {
                return ((MeetRatio - 1.0f) * CharaLeft.transform.position + CharaRight.transform.position) / 3.0f;
            }
        }

        // Setup timings
        private const float ActionDelay = 0.5f;
        private const float StartDelay = 2.0f;
        private const float WalkTime = 2.0f;
        private const float SuccessWalkTime = 3.0f;
        private const float DialogBalloonTime = 3.0f;
        private const float ThinkingTime = 0.70f;
        private const float WaitingHearthTime = 3.00f;
        private const float HappyFrameDelay = 0.30f;
        private const float CryFrameDelay = 0.40f;
        private const float HeartsShowTime = 1.00f;

        private void ShowLeftText(string str) {
            LeftDialogBalloon.SetActive(true);
            LeftDialogBalloonText.text = str;
        }

        private void HideLeftText() {
            LeftDialogBalloon.SetActive(false);
        }

        private void ShowRightText(string str) {
            RightDialogBalloon.SetActive(true);
            RightDialogBalloonText.text = str;
        }

        private void HideRightText() {
            RightDialogBalloon.SetActive(false);
        }

        private class PhraseList {
            private string[] WomanPhrases;
            private string[] ManPhrases;

            public string GetPhrase(TamaChara ch) {
                if (ch.sex == 1) 
                    return string.Format(WomanPhrases[UnityEngine.Random.Range(0, WomanPhrases.Length)], "");
                return string.Format(ManPhrases[UnityEngine.Random.Range(0, ManPhrases.Length)], "");
            }

            public PhraseList(string[] manPhrases, string[] womanPhrases) {
                ManPhrases = manPhrases;
                WomanPhrases = womanPhrases;
            }
        }

        #region propose phrases
        private PhraseList ProposePhraseList = new PhraseList(
            // Mans
            new string[] {
                "すきです！けっこんしたい{0}",
                "ひとめぼれ・・・けっこんしたい{0}",
                "もう あなたしかいない{0}",
                "しあわせにすると やくそくする{0}",
                "いっしょになってほしい{0}"
            }, 

            // Womans
            new string[] {
                "あなたと いっしょにいたい{0}",
                "あなたしかみえない{0}",
                "いっしょにいてほしい{0}",
                "おねがいっ！けっこんしたい{0}",
                "わたしと しあわせになる{0}"
            }
        );

        private void ShowProposeText() {
            ShowRightText(ProposePhraseList.GetPhrase(RightCharaData));
        }
        #endregion

        #region propose accept phrases
        private PhraseList ProposeAcceptPhraseList = new PhraseList(
            // Mans
            new string[] {
                "ぼくも けっこんしたい{0}",
                "ぼくも あなたしかいない{0}",
                "すごくうれしい{0}",
                "しあわせになる{0}",
                "かんげきした{0}"
            },

            // Womans
            new string[] {
                "わたしも いっしょにいたい{0}",
                "むねのドキドキが とまらない{0}",
                "わたしも けっこんしたい{0}",
                "なかよくしてほしい{0}",
                "かんげきしちゃった{0}"
            });

        private void ShowProposeAcceptText() {
            ShowLeftText(ProposeAcceptPhraseList.GetPhrase(LeftCharaData));
        }
        #endregion

        #region propose refuse phrases
        private PhraseList ProposeRefusePhraseList = new PhraseList(
            // Mans
            new string[] {
                "ごめんとしかいえない{0}",
                "あなたと けっこんできない{0}",
                "あきらめてほしい{0}",
                "ぼくには もったいない{0}",
                "ほかに すきなひとがいる{0}"
            },

            // Womans
            new string[] {
                "あきらめてほしい{0}",
                "ほかに すきなひとがいる{0}",
                "すこし かんがえたい{0}",
                "おともだちならいい{0}",
                "あなたと けっこんできない{0}"
            });

        private void ShowProposeRefuseAcceptText() {
            ShowLeftText(ProposeRefusePhraseList.GetPhrase(LeftCharaData));
        }
        #endregion

        /// <summary>
        /// Starts propose animation
        /// </summary>
        /// <returns></returns>
        private IEnumerator ProposeAnimation() {
            ManagerObject.instance.sound.playBgm(16);

            // Right character walk
            CharaRight.gotoAndPlay("walk");
            yield return StartCoroutine(MoveToPoint(CharaRight.transform, MeetPoint_Right, WalkTime));
            
            yield return new WaitForSeconds(ActionDelay);

            // Show propose dialog
            ShowProposeText();
            CharaRight.gotoAndPlay("idle");
            yield return new WaitForSeconds(DialogBalloonTime);
            HideRightText();
        }

        /// <summary>
        /// Force character to happy animation
        /// </summary>
        /// <param name="chara"></param>
        /// <param name="repeatcount"></param>
        /// <returns></returns>
        private IEnumerator HappyAnimation(CharaBehaviour chara, int repeatcount) {
            int i = 0;
            while (i < repeatcount) {
                chara.gotoAndPlay("glad1");
                yield return new WaitForSeconds(HappyFrameDelay);
                chara.gotoAndPlay("glad2");
                yield return new WaitForSeconds(HappyFrameDelay);
                chara.gotoAndPlay("glad3");
                yield return new WaitForSeconds(HappyFrameDelay);
                i++;
            }
        }

        /// <summary>
        /// Propose success animation.        
        /// </summary>
        /// <param name="act">action, when animation finished</param>
        private IEnumerator ProposeSuccessAnimation() {
            // Setup points
            Vector3 left_point = CharaLeft.transform.position;
            Vector3 right_point = CharaRight.transform.position;
            
            Vector3 DestinationLeftPoint = (3.0f*left_point + right_point * (3.0f - 2.0f)) / 3.0f;

            // Idle animation
            CharaLeft.gotoAndPlay("idle");
            CharaRight.gotoAndPlay("idle");

            yield return new WaitForSeconds(StartDelay);

            // Propose
            yield return StartCoroutine(ProposeAnimation());

            // Wait a bit aka characters think
            yield return new WaitForSeconds(ThinkingTime);

            // Left character move
            CharaLeft.gotoAndPlay("walk");
            yield return StartCoroutine(MoveToPoint(CharaLeft.transform, DestinationLeftPoint, SuccessWalkTime));


            // Show arigatou dialog
            ShowProposeAcceptText();
            CharaLeft.gotoAndPlay("idle");
            yield return new WaitForSeconds(DialogBalloonTime);
            HideLeftText();
            
            // Wait a bit aka characters think
            yield return new WaitForSeconds(WaitingHearthTime);

            ManagerObject.instance.sound.playSe(17);

            // Happy!
            HappyAnimation(CharaLeft, 2);
            HappyAnimation(CharaRight, 2);
            
            // Move hearths
            StartCoroutine(MoveToPoint(SuccessHearts.transform, SuccessHeartsDestination, HeartsShowTime));

            // They still happy XD
            HappyAnimation(CharaLeft, 20);
            HappyAnimation(CharaRight, 20);
        }
        
        /// <summary>
        /// Force character to cry
        /// </summary>
        /// <param name="chara"></param>
        /// <param name="repeatcount"></param>
        /// <returns></returns>
        private IEnumerator CryAnimation(CharaBehaviour chara, int repeatcount) {
            int i = 0;
            while (i < repeatcount) {
                chara.gotoAndPlay("cry");
                yield return new WaitForSeconds(2*CryFrameDelay);
                chara.gotoAndPlay("shock");
                yield return new WaitForSeconds(CryFrameDelay);
                i++;
            }
        }

        /// <summary>
        /// Propose fail animation
        /// </summary>
        /// <param name="act">action, when animation finished</param>
        private IEnumerator ProposeFailedAnimation() {
            // Idle animation
            CharaLeft.gotoAndPlay("idle");
            CharaRight.gotoAndPlay("idle");

            yield return new WaitForSeconds(StartDelay);

            // Propose
            yield return StartCoroutine(ProposeAnimation());

            // Wait a bit aka characters think
            yield return new WaitForSeconds(ThinkingTime);

            // Show refuse dialog
            ShowProposeRefuseAcceptText();
            yield return new WaitForSeconds(DialogBalloonTime);
            HideLeftText();

            // Wait a bit aka characters think
            yield return new WaitForSeconds(ThinkingTime);
            
            ManagerObject.instance.sound.playSe(18);

            // Force sad animation
            StartCoroutine(CryAnimation(CharaRight, 50));

            // Move in character heart
            FullHeart.SetActive(true);
            yield return StartCoroutine(MoveToPoint(HeartGroup, HeartGroupDestination.position, 0.50f));

            // Broke heart animation
            FullHeart.SetActive(false);
            LeftHalfHeart.SetActive(true);
            RightHalfHeart.SetActive(true);
            int i = 0;
            while (i < 20) {
                LeftHalfHeart.transform.RotateAround(HeartRotationCenter.position, Vector3.forward, 3.0f);
                RightHalfHeart.transform.RotateAround(HeartRotationCenter.position, Vector3.forward, -3.0f);
                i++;
                yield return null;
            }

            // Wait before heart moves in
            yield return new WaitForSeconds(1.50f);

            // Move in hearths
            StartCoroutine(MoveToPoint(FailHearts.transform, FailHeartsDestination, HeartsShowTime));
        }

        /// <summary>
        /// Starts waiting animation.
        /// </summary>
        public void ProposeWaitingAnimation() {
            CharaLeft.gotoAndPlay("idle");
            CharaRight.gotoAndPlay("idle");
        }

        public UnityEvent AnimationFinishedEvent = new UnityEvent();

        private IEnumerator SetupAndAction(TamaChara left, TamaChara right, IEnumerator action) {
            yield return StartCoroutine(Setup(left, right));
            yield return StartCoroutine(action);
            AnimationFinishedEvent.Invoke();
        }

        /// <summary>
        /// Init characters and force success animation
        /// </summary>
        /// <param name="left">recieve side</param>
        /// <param name="right">propose side</param>
        public void ForceSuccess(TamaChara left, TamaChara right) {
            StartCoroutine(SetupAndAction(left, right, ProposeSuccessAnimation()));
        }

        /// <summary>
        /// Init characters and force fail animation
        /// </summary>
        /// <param name="left">recieve side</param>
        /// <param name="right">propose side</param>
        public void ForceFail(TamaChara left, TamaChara right) {
            StartCoroutine(SetupAndAction(left, right, ProposeFailedAnimation()));
        }
    }
}