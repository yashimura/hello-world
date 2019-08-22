using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;

using Mix2App.Lib;
using Mix2App.Lib.Model;
using Mix2App.Lib.View;
using Mix2App.Lib.Events;

namespace Mix2App.Setting
{
    public class View2 : MonoBehaviour
    {
		[SerializeField] Button[] btnOns = null;
		[SerializeField] Button[] btnOffs = null;

		[SerializeField] Button[] sevolbtns = null;
		[SerializeField] Button[] bgmvolbtns = null;

		[SerializeField] Image[] onpus1 = null;
		[SerializeField] Image[] onpus2 = null;

        Coroutine mcol;
        bool soundFlag;
        int seVolume;
        int bgmVolume;
        
        Color oncolor = new Color(1f,1f,1f,1f);
        Color offcolor = new Color(1f,1f,1f,0f);
        Color offhigholor = new Color(1f,1f,1f,0.4f);

        void Awake()
        {
            mcol=null;
        }

        void OnEnable()
        {           
            soundFlag = ManagerObject.instance.app.enabledSound;
            seVolume = ManagerObject.instance.app.seVolume;
            bgmVolume = ManagerObject.instance.app.bgmVolume;
            updateView();
        }

        void OnDisable()
        {           
            StopAllCoroutines();
        }

        void updateView()
        {
            int i;

            //サウンドオンオフの表示
            if (soundFlag)
            {
                btnOns[0].gameObject.SetActive(false);
                btnOns[1].gameObject.SetActive(true);
                btnOffs[0].gameObject.SetActive(true);
                btnOffs[1].gameObject.SetActive(false);
            }
            else 
            {
                btnOns[0].gameObject.SetActive(true);
                btnOns[1].gameObject.SetActive(false);
                btnOffs[0].gameObject.SetActive(false);
                btnOffs[1].gameObject.SetActive(true);
            }

            for (i=0;i<5;i++)
            {
                int id = i+1;
                
                //ボリュームゲージの表示
                var colors1 = sevolbtns[i].colors;
                colors1.normalColor = (seVolume>=id) ? oncolor : offcolor;
                colors1.highlightedColor = (seVolume>=id) ? oncolor : offhigholor;
                sevolbtns[i].colors = colors1;

                var colors2 = bgmvolbtns[i].colors;
                colors2.normalColor = (bgmVolume>=id) ? oncolor : offcolor;
                colors2.highlightedColor = (bgmVolume>=id) ? oncolor : offhigholor;
                bgmvolbtns[i].colors = colors2;
    
                //音符の表示
                if (id==seVolume) onpus1[i].enabled=true;
                else onpus1[i].enabled=false;

                if (id==bgmVolume) onpus2[i].enabled=true;
                else onpus2[i].enabled=false;
            }
        }
        
        IEnumerator updateSound()
        {
            updateView();
            GameCall call = new GameCall(CallLabel.SET_SOUND_VOLUME,soundFlag,seVolume,bgmVolume); 
            yield return ManagerObject.instance.connect.send(call);
            ManagerObject.instance.sound.playSe(11);
        }

        public void clickOnOff(string label)
        {            
            if (mcol!=null)return;
            soundFlag = (label=="on");
            StartCoroutine(updateSound());
        }

        public void clickSeVolume(int level)
        {
            if (mcol!=null)return;
            if (seVolume == level) return;
            seVolume = level;
            StartCoroutine(updateSound());
        }

        public void clickBgmVolume(int level)
        {
            if (mcol!=null)return;
            if (bgmVolume == level) return;
            bgmVolume = level;
            StartCoroutine(updateSound());
        }

    }
}