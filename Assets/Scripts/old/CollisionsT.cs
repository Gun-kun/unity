using UnityEngine;
using System.Collections;

//{}

public class CollisionsT : MonoBehaviour {
	
	public bool isSelected;				//character selected to be moved
	public float dragspeed=50f;			//drag speed
	public float followspeed=40f;		//follow speed
	public float knockback=0.02f;		//knockback speed

	private bool isFollow;				//character is following someone
	private float limx = 18f;			//battlefield border x
	private float limy = 9f;			//battlefield border y
	private Vector3 nextPosition;		//next position to move towards
	private Vector3 moveDirection;		//knockback direction
	private GameObject leader;			//allied to follow

	void Start (){}

	void Update () 
	{
		nextPosition = transform.position;

		//follows the leader
		if(isFollow)
		{
			moveDirection = leader.transform.position - nextPosition;
			if (moveDirection.magnitude>leader.GetComponent<UnitInfoT>().Range)
			{
				moveDirection.Normalize ();
				transform.Translate(moveDirection*followspeed*Time.deltaTime);
			}
		}

		//check borders of battlefield
		if(nextPosition.x > limx){
			nextPosition.x = limx;
			transform.position = Vector3.MoveTowards(transform.position, nextPosition, dragspeed*Time.deltaTime);
		}
		else if(nextPosition.x < -limx)
		{
			nextPosition.x = -limx;
			transform.position = Vector3.MoveTowards(transform.position, nextPosition, dragspeed*Time.deltaTime);
		}
		if(nextPosition.y > limy){
			nextPosition.y = limy;
			transform.position = Vector3.MoveTowards(transform.position, nextPosition, dragspeed*Time.deltaTime);
		}
		else if(nextPosition.y < -limy)
		{
			nextPosition.y = -limy;
			transform.position = Vector3.MoveTowards(transform.position, nextPosition, dragspeed*Time.deltaTime);
		}
	}

	//=========================================================================================
	//passive collision check
	//=========================================================================================
	void OnTriggerEnter2D( Collider2D other )
	{	
		SendMessageUpwards ("MessageEnter", other.gameObject);
		if(isSelected)
		{
			//no knockback nor push
		}
		else
		{
			if (other.GetComponent<UnitInfoT>().FriendFoe==gameObject.GetComponent<UnitInfoT>().FriendFoe && other.GetComponent<CollisionsT>().isSelected==true)
			{
				//follows leader
				isFollow = true;
				//leaderPosition = other.transform.position;
				leader=other.gameObject;
				//knockback on self
				moveDirection = transform.position - other.transform.position;
				moveDirection.Normalize ();
				transform.Translate(moveDirection*(knockback));
			}
//			else if(other.GetComponent<UnitInfoT>().FriendFoe==gameObject.GetComponent<UnitInfoT>().FriendFoe && other.GetComponent<CollisionsT>().isSelected==false)
//			{
//				//knockback on self
//				moveDirection = transform.position - other.transform.position;
//				moveDirection.Normalize ();
//				transform.Translate(moveDirection*(knockback));
//			}
			else
			{
				//knockback on other
				moveDirection = other.transform.position - transform.position;
				moveDirection.Normalize ();
				other.transform.Translate(moveDirection*(knockback));
			}
		}
	}
	
	void OnTriggerStay2D( Collider2D other )
	{
		SendMessageUpwards ("MessageStay", other.gameObject);
		if(isSelected)
		{
			//no knockback nor push
		}
		else
		{
//			if (other.GetComponent<UnitInfoT>().FriendFoe==gameObject.GetComponent<UnitInfoT>().FriendFoe)
//			{
//				//knockback on self
//				moveDirection = transform.position - other.transform.position;
//				moveDirection.Normalize ();
//				transform.Translate(moveDirection*(knockback));
//			}
//			else
//			{
				//knockback on other
				moveDirection = other.transform.position - transform.position;
				moveDirection.Normalize ();
				other.transform.Translate(moveDirection*(knockback));
//			}
		}
	}
	
	void OnTriggerExit2D( Collider2D other )
	{
		SendMessageUpwards ("MessageExit", other.gameObject);
	}
	//=========================================================================================

	void StopFollowing()
	{
		isFollow = false;
	}
}

