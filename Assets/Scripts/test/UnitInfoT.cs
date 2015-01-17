using UnityEngine;
using System.Collections;

//{}

//[RequireComponent(typeof(CircleCollider2D))]

public class UnitInfoT : MonoBehaviour {
	
	public int UnitID;
	public string UnitName;
	public int FriendFoe;		//1 allied 2 enemy 3 orb 4 pushable obstacle 5 solid obstacle

	public float Leadership; 	//to follow and be followed
	public float Range;			//attack range
	public float Initiative; 	//cooldown-warmup
	public float Movement; 		//movement allowed
	
	public int Level;

	public int maxHp;
	public int curHp;
	
	public int Atk;
	public int Def;
	public int MAtk;
	public int MDef;
	
	//damage reductions
	public int RPhys;
	public int RFire;
	public int RIce;
	public int RThunder;
	public int RHeal;
	
	//status resist
	public int RStunned;
	public int RBlocked;
	public int RSilenced;
	
	//skills
	
	//buffs
	
	void Start ()
	{
		curHp = maxHp;
	}

	void Update () {	
	}
	
	void MessageHere (){
		//log received message from collision movement
		Debug.Log (""+gameObject.name+" received the message");
	}
}
