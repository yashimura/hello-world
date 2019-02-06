using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mix2App.MachiCon{
	public class Cutin : MonoBehaviour {
		public GameObject controller;
		private Macicon_controller Macicon;

		private Animator cutin_animator;

		public Camera targetCamera;
		//private AnimatorStateInfo stateInfo;
		// Use this for initialization


		void Start () {
			//this.GetComponent<Canvas>().worldCamera = targetCamera;
			Macicon = controller.GetComponent<Macicon_controller> ();
			cutin_animator = this.GetComponent<Animator> ();	
			//cutin_animator.SetTrigger ("start");


		}

		// Update is called once per frame
		void Update () {

		}

		public void cutin(){
			cutin_animator.SetTrigger ("start");
		}

		public void cutinend(){
			Debug.Log ("cutinend");
			//Application.UnloadLevel("Cutin");
			//Resoures.UnloadUnusedAssets ();
			if (Macicon != null) {
				Macicon.cutinend ();
			}
		}
	}
}
