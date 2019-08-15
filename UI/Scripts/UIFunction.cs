using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Utils;

namespace Mix2App.UI
{
    public class UIFunction : MonoBehaviour
    {
        public static void TamagochiImageMove(GameObject toObj, GameObject fromObj, string toStr)
        {
            for (int i = 0; i < fromObj.transform.Find("Layers").transform.childCount; i++)
            {
                toObj.transform.Find(toStr + "CharaImg/Layers/" + fromObj.transform.Find("Layers").transform.GetChild(i).name).gameObject.transform.SetSiblingIndex(i);
            }

            toObj.transform.Find(toStr + "CharaImg").gameObject.GetComponent<Image>().enabled = false;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer0").gameObject.GetComponent<Image>().enabled = fromObj.transform.Find("Layers/Layer0").gameObject.GetComponent<Image>().enabled;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer1").gameObject.GetComponent<Image>().enabled = fromObj.transform.Find("Layers/Layer1").gameObject.GetComponent<Image>().enabled;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer2").gameObject.GetComponent<Image>().enabled = fromObj.transform.Find("Layers/Layer2").gameObject.GetComponent<Image>().enabled;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer3").gameObject.GetComponent<Image>().enabled = fromObj.transform.Find("Layers/Layer3").gameObject.GetComponent<Image>().enabled;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer4").gameObject.GetComponent<Image>().enabled = fromObj.transform.Find("Layers/Layer4").gameObject.GetComponent<Image>().enabled;

            toObj.transform.Find(toStr + "CharaImg/Layers/Layer0").gameObject.GetComponent<Image>().sprite = fromObj.transform.Find("Layers/Layer0").gameObject.GetComponent<Image>().sprite;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer1").gameObject.GetComponent<Image>().sprite = fromObj.transform.Find("Layers/Layer1").gameObject.GetComponent<Image>().sprite;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer2").gameObject.GetComponent<Image>().sprite = fromObj.transform.Find("Layers/Layer2").gameObject.GetComponent<Image>().sprite;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer3").gameObject.GetComponent<Image>().sprite = fromObj.transform.Find("Layers/Layer3").gameObject.GetComponent<Image>().sprite;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer4").gameObject.GetComponent<Image>().sprite = fromObj.transform.Find("Layers/Layer4").gameObject.GetComponent<Image>().sprite;

            toObj.transform.Find(toStr + "CharaImg/Layers/Layer0").gameObject.transform.localPosition = fromObj.transform.Find("Layers/Layer0").gameObject.transform.localPosition;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer1").gameObject.transform.localPosition = fromObj.transform.Find("Layers/Layer1").gameObject.transform.localPosition;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer2").gameObject.transform.localPosition = fromObj.transform.Find("Layers/Layer2").gameObject.transform.localPosition;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer3").gameObject.transform.localPosition = fromObj.transform.Find("Layers/Layer3").gameObject.transform.localPosition;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer4").gameObject.transform.localPosition = fromObj.transform.Find("Layers/Layer4").gameObject.transform.localPosition;

            toObj.transform.Find(toStr + "CharaImg/Layers/Layer0").gameObject.transform.localScale = fromObj.transform.Find("Layers/Layer0").gameObject.transform.localScale;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer1").gameObject.transform.localScale = fromObj.transform.Find("Layers/Layer1").gameObject.transform.localScale;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer2").gameObject.transform.localScale = fromObj.transform.Find("Layers/Layer2").gameObject.transform.localScale;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer3").gameObject.transform.localScale = fromObj.transform.Find("Layers/Layer3").gameObject.transform.localScale;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer4").gameObject.transform.localScale = fromObj.transform.Find("Layers/Layer4").gameObject.transform.localScale;

            toObj.transform.Find(toStr + "CharaImg/Layers/Layer0").gameObject.transform.localRotation = fromObj.transform.Find("Layers/Layer0").gameObject.transform.localRotation;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer1").gameObject.transform.localRotation = fromObj.transform.Find("Layers/Layer1").gameObject.transform.localRotation;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer2").gameObject.transform.localRotation = fromObj.transform.Find("Layers/Layer2").gameObject.transform.localRotation;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer3").gameObject.transform.localRotation = fromObj.transform.Find("Layers/Layer3").gameObject.transform.localRotation;
            toObj.transform.Find(toStr + "CharaImg/Layers/Layer4").gameObject.transform.localRotation = fromObj.transform.Find("Layers/Layer4").gameObject.transform.localRotation;

        }

        public static void TamagochiPetImageMove(GameObject toObj, GameObject fromObj, string toStr)
        {
            for (int i = 0; i < fromObj.transform.Find("Layers").transform.childCount; i++)
            {
                toObj.transform.Find(toStr + "PetImg/Layers/" + fromObj.transform.Find("Layers").transform.GetChild(i).name).gameObject.transform.SetSiblingIndex(i);
            }

            toObj.transform.Find(toStr + "PetImg").gameObject.GetComponent<Image>().enabled = false;
            toObj.transform.Find(toStr + "PetImg/Layers/Layer").gameObject.GetComponent<Image>().enabled = fromObj.transform.Find("Layers/Layer").gameObject.GetComponent<Image>().enabled;
            toObj.transform.Find(toStr + "PetImg/Layers/Layer (1)").gameObject.GetComponent<Image>().enabled = fromObj.transform.Find("Layers/Layer (1)").gameObject.GetComponent<Image>().enabled;

            toObj.transform.Find(toStr + "PetImg/Layers/Layer").gameObject.GetComponent<Image>().sprite = fromObj.transform.Find("Layers/Layer").gameObject.GetComponent<Image>().sprite;
            toObj.transform.Find(toStr + "PetImg/Layers/Layer (1)").gameObject.GetComponent<Image>().sprite = fromObj.transform.Find("Layers/Layer (1)").gameObject.GetComponent<Image>().sprite;

            toObj.transform.Find(toStr + "PetImg/Layers/Layer").gameObject.transform.localPosition = fromObj.transform.Find("Layers/Layer").gameObject.transform.localPosition;
            toObj.transform.Find(toStr + "PetImg/Layers/Layer (1)").gameObject.transform.localPosition = fromObj.transform.Find("Layers/Layer (1)").gameObject.transform.localPosition;

            toObj.transform.Find(toStr + "PetImg/Layers/Layer").gameObject.transform.localScale = fromObj.transform.Find("Layers/Layer").gameObject.transform.localScale;
            toObj.transform.Find(toStr + "PetImg/Layers/Layer (1)").gameObject.transform.localScale = fromObj.transform.Find("Layers/Layer (1)").gameObject.transform.localScale;

            toObj.transform.Find(toStr + "PetImg/Layers/Layer").gameObject.transform.localRotation = fromObj.transform.Find("Layers/Layer").gameObject.transform.localRotation;
            toObj.transform.Find(toStr + "PetImg/Layers/Layer (1)").gameObject.transform.localRotation = fromObj.transform.Find("Layers/Layer (1)").gameObject.transform.localRotation;
        }

        /// <summary>
        /// 文字列が２０文字以上の場合１９文字目と２０文字目を……に変更して２０文字にする。
        /// </summary>
        /// <param name="baseStr"></param>
        /// <returns></returns>
        public static string UserNicknameChange(string baseStr)
        {
            string retStr = baseStr;

            if (baseStr != null)
            {
                if (baseStr.Length > 20)
                {
                    retStr = baseStr.Remove(18, baseStr.Length - 18);
                    retStr = retStr + "……";
                }
            }
            else
            {
                retStr = "";
            }

            return retStr;
        }
        /// <summary>
        /// 文字列が１３文字以上なら１０文字で改行する。
        /// </summary>
        /// <param name="baseStr"></param>
        /// <returns></returns>
        public static string UserNicknameRetInsert(string baseStr)
        {
            string retStr = baseStr;

            if (baseStr != null)
            {
                if (baseStr.Length > 12)
                {
                    retStr = baseStr.Insert(10, "\n");
                }
            }
            else
            {
                retStr = "";
            }

            return retStr;
        }

        /// <summary>
        /// ボタンの制御モードの変更
        /// </summary>
        /// <param name="_btn"></param>
        /// <param name="_flag"></param>
        public static void ButtonClickModeChenage(Button _btn,bool _flag)
        {
            // ボタン制御を変更
            _btn.enabled = _flag;
        }    


        private static bool _tutorialFlag;
        private static int _tutorialRoute;
        private static int _tutorialStep;
        private static TUTORIAL_COUNTER _tutorialCount;

        public static void TutorialDataAllClear()
        {
            _tutorialFlag = false;
            _tutorialRoute = 0;
            _tutorialStep = 0;
            _tutorialCount = 0;
        }
        public static void TutorialDataAllSet(bool _flag,int _route,int _step)
        {
            _tutorialFlag = _flag;
            _tutorialRoute = _route;
            _tutorialStep = _step;
            _tutorialCount = TUTORIAL_COUNTER.Normal;
        }
        public static bool TutorialFlagGet()
        {
            return _tutorialFlag;
        }
        public static int TutorialRouteGet()
        {
            return _tutorialRoute;
        }
        public static int TutorialStepGet()
        {
            return _tutorialStep;
        }

        public enum TUTORIAL_COUNTER
        {
            Normal,
            ButtonTrueStart,
            ButtonTrueEnd,
            TutorialClear,
            ProposeEnd,
        };

        public static void TutorialCountSet(TUTORIAL_COUNTER _cnt)
        {
            _tutorialCount = _cnt;
        }
        public static TUTORIAL_COUNTER TutorialCountGet()
        {
            return _tutorialCount;
        }


    }
}
