using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Mix2App.Lib;
using Mix2App.Lib.Events;
using Mix2App.Lib.View;
using Mix2App.Lib.Model;

namespace Mix2App.MachiCon
{
    public class Talk1Controller : MonoBehaviour
    {
        [SerializeField] private GameObject evtarget;

        public void PrintEvent(string s)
        {
            Debug.Log("PrintEvent: " + s + " called at: " + Time.time);
            if (s == "step")
            {
                ManagerObject.instance.sound.playSe(1);
            }
            else if (s == "win")
            {
                ManagerObject.instance.sound.playSe(34);
            }
            else if (s == "jump")
            {
                ManagerObject.instance.sound.playSe(24);
            }
            else if (s == "hit")
            {
                ManagerObject.instance.sound.playSe(24);
            }
            else if (s == "end")
            {
                StopAllCoroutines();
                ExecuteEvents.Execute<IMachiConEventHandler>(
                    target: evtarget,
                    eventData: null,
                    functor: (reciever,eventData)=>reciever.endMovie()
                );
            }
        }

        // Use this for initialization
        void Awake()
        {
            StopAllCoroutines();
        }

    }
}