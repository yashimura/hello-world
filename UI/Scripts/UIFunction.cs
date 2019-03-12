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
        }



    }
}
