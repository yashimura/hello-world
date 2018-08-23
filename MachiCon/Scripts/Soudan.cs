using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Mix2App.MachiCon{
	public class Soudan : MonoBehaviour {
		public GameObject controller;
		private Macicon_controller Macicon;
		private Animator soudan_animator;
		private AnimatorStateInfo stateInfo;

		public GameObject ch;
		private Animator pl_animator;
		public GameObject f_tenten;
		// Use this for initialization
		void Start () {
			Macicon = controller.GetComponent<Macicon_controller>();
			soudan_animator = this.GetComponent<Animator> ();
			pl_animator = ch.GetComponent<Animator> ();
			stateInfo = soudan_animator.GetCurrentAnimatorStateInfo(0);
			gameObject.SetActive (false);
			pl_animator.SetTrigger ("def");
		}

		// Update is called once per frame
		void Update () {

		}

		public void Phasestart(){
			gameObject.SetActive (true);
			soudan_animator.Play(stateInfo.fullPathHash, 0, 0.0f);  //初期位置に戻す
		}

		public void push_no(){
			pl_animator.SetTrigger ("cry");
			soudan_animator.SetTrigger ("waite");
		}
		public void push_yes(){
			pl_animator.SetTrigger ("fun");
			soudan_animator.SetTrigger ("waite");
		}

		public void waite_end(){
			Debug.Log ("waite");
			soudan_animator.SetTrigger ("w_end");
		}

		public void push_end(){
			soudan_animator.SetTrigger ("w_end");
		}
		public void soudan_end(){
			gameObject.SetActive (false);
			if (Macicon != null) {
				Macicon.soudan_end ();
			}
		}

		void fukidasi(string txt){
			Transform parparent = ch.transform;
			Vector3 fukidasipos = ch.transform.position;
			Vector3 parpos_pl = new Vector3 (fukidasipos.x, fukidasipos.y+25 , fukidasipos.z);

			GameObject obj = Instantiate (f_tenten, parpos_pl, Quaternion.identity, parparent)as GameObject;
			//obj.AddComponent<Fukidasiscript> ();

			//Fukidasiscript fobj = obj.GetComponent<Fukidasiscript> ();

			//fobj.spritset (0); 

			Destroy (obj, 1f);
		}
	}
}
