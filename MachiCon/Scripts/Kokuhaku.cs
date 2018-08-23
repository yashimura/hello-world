using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mix2App.MachiCon{
	public class Kokuhaku : MonoBehaviour {
		public GameObject controller;
		private Macicon_controller Macicon;

		//private Animator kokuhaku_animator;
		//private AnimatorStateInfo stateInfo;
		// Use this for initialization
		void Start () {
			Macicon = controller.GetComponent<Macicon_controller>();


			//kokuhaku_animator = this.GetComponent<Animator> ();
			//stateInfo = kokuhaku_animator.GetCurrentAnimatorStateInfo(0);
			gameObject.SetActive (false);
		}

		// Update is called once per frame
		void Update () {

		}
		public void kokuhaku_start(){
			gameObject.SetActive (true);
		}

		public void kokuhaku_end(){
			if (Macicon != null) {
				Macicon.kokuhaku ();
			}
		}
	}
}
