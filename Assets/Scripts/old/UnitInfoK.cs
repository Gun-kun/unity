using UnityEngine;
using System.Collections;

//{}

//[RequireComponent(typeof(CircleCollider2D))]

public class UnitInfoK : MonoBehaviour {
	
	public int UnitID;
	public string UnitName;
	public int FriendFoe;		//1 allied 2 enemy 3 orb 4 pushable obstacle 5 solid obstacle
	
	public float Weight; 		//to push and get pushed
	public float Range;			//attack range
	public float Movement; 		//movement allowed
	public int Action; 			//cooldown-warmup
	
	public int Level;
	
	public int maxHp;
	public int curHp;
	
	public int Atk;
	public int Def;
	public int MAtk;
	public int MDef;
	
	//damage reductions
	public int RFire;
	public int RIce;
	public int RThunder;
	
	//status resist
	public int RStunned;
	public int RBlocked;
	public int RSilenced;
	
	//skills
	
	//buffs
	
	// Use this for initialization
	void Start () {
		curHp = maxHp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//get damage
	void TakeDamage (int damage) {
		//reduce damage per type
		curHp = curHp - damage;
		if(curHp == 0){
			//kill
		}
	}
	
	//get heal
	void TakeHeal (int heal) {
		curHp = curHp + heal;
		if(curHp > maxHp){
			curHp = maxHp;
		}
	}
	
	//attack
	void Attack (UnitInfoK unit) {
		//	unit.TakeDamage (10);
	}
	//draw attack range
	
	//show hp bar on mouse over
	void OnMouseOver (){
		//if not isDraggable
		//renderer.material.color -= new Color (0.1f, 0, 0) * Time.deltaTime;
	}
	
	void MessageHere (){
		//log received message from collision movement
		Debug.Log (""+gameObject.name+" received the message");
	}
	
	//	//=========================================================================================
	//	//collision checks
	//	//=========================================================================================
	//	//get in contact
	//	void OnCollisionEnter2D(Collision2D coll) {
	//		//check get self-other contact skills
	//	}
	//	
	//	//=========================================================================================
	//	//leave contact
	//	void OnCollisionStay2D(Collision2D coll) {
	//		//check keep self-other contact skills*
	//	}
	//	
	//	//=========================================================================================
	//	//stay in contact
	//	void OnCollisionExit2D(Collision2D coll) {
	//		//check leave self-other contact skills
	//	}
	//	//=========================================================================================
	//	//other gets in contact
	//	void OnTriggerEnter2D(Collision2D coll){
	//		//check get other-self contact skills
	//	}
	//	//=========================================================================================
	//	//other stay in contact
	//	void OnTriggerStay2D(Collision2D coll){
	//		//check keep other-self contact skills*
	//	}
	//	//=========================================================================================
	//	//other leave contact
	//	void OnTriggerExit2D(Collision2D coll){
	//		//check leave other-self cotact skills
	//	}
	
	//show attack range
}
