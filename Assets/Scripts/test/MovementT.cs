using UnityEngine;
using System.Collections;
using System.Collections.Generic;		//for list
using System.Linq;						//for list
//{}

public class MovementT : MonoBehaviour {

	public bool isSelected;				//character selected to be moved
	public float dragspeed=50f;			//mouse follow speed
	public float followspeed=40f;		//follow speed
	public float knockback=0.02f;		//knockback speed
	public float limx = 18f;			//battlefield border x
	public float limy = 9f;				//battlefield border y
	
	//drag n drop
	private Vector3 curPosition;		//next position to move towards
	private Vector3 curScreenPoint;		//coordinates of mouse on screen
	private Vector3 screenPoint;		//z
	private Vector3 offset;				//starting position
	private Vector3 moveDirection;		//knockback direction
	
	//movement limitations
	private bool isDrag;				//flag is draggable
	private bool isTouch;				//flag is colliding with another collider
	private float MovementAllowed;		//max movement allowed in a turn for the unit on turn
	private float step;					//distance to get to mouse from current position
	
	//follow system
	private List<GameObject> followers;	//all allies following
	private float FollowCost;			//movement cost per step when followers tag along
	private float Leadership;			//leader movement cost
	
	private bool isFollow;				//character is following someone
	private GameObject leader;			//allied to follow


	//=========================================================================================
	//common
	//=========================================================================================



	//=========================================================================================
	//initialize
	void Start ()
	{
		if (isSelected)
		{//initialize movement allowed
			MovementAllowed = gameObject.GetComponent<UnitInfoT>().Movement;
			
			//initialize follow cost
			Leadership = gameObject.GetComponent<UnitInfoT> ().Leadership;
			FollowCost = Leadership;
			
			//initialize followers
			followers = new List<GameObject>();
		}
	}
	//=========================================================================================


	//=========================================================================================
	//movement and follow
	void Update ()
	{	
		if (isSelected)
		//get move destination
		{
			//check amount of movement allowed left for the unit
			if (MovementAllowed > 0)
			{
				//check mouse dragging position
				if(isDrag)
				{
					//update position to move towards
					curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
					curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
				}
				else
				{
					//doesn't move
					curPosition=transform.position;
				}
			}
			//don't update position to move towards
		}
		else if(isFollow)
		//follows the leader
		{
			moveDirection = leader.transform.position - transform.position;
			if (moveDirection.magnitude>leader.GetComponent<UnitInfoT>().Range)
			{
				moveDirection.Normalize ();
				transform.Translate(moveDirection*followspeed*Time.deltaTime);
			}
		}
		//check borders of battlefield
		CheckBorders(transform.position);
	}
	//=========================================================================================


	//=========================================================================================
	//stop all followers
	void StopFollowers()
	{
		FollowCost = Leadership;
		//send message to all followers
		while(followers.Count>0)
		{
			followers.Last().SendMessage ("StopFollowing");
			followers.Remove (followers.Last());
		}
	}
	//stop following on received message
	void StopFollowing()
	{
		isFollow = false;
	}
	//=========================================================================================


	//=========================================================================================
	//show movement allowed bar
	void OnGUI()
	{
		if (isSelected)
		{
			GUI.Label(new Rect(0,0,300,50),"movement allowed " +MovementAllowed);
			GUI.Label (new Rect(0,10,300,50),"movement cost " + FollowCost/Leadership);
		}
	}
	//=========================================================================================


	//=========================================================================================
	//check borders of battlefield
	void CheckBorders(Vector3 nextPosition)
	{
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


	//=========================================================================================
	//passive collision check
	void OnTriggerEnter2D( Collider2D other )
	{	
		Enter(other.gameObject);
		if(isSelected)
		{
			//no knockback nor push
		}
		else
		{
			if (other.GetComponent<UnitInfoT>().FriendFoe==gameObject.GetComponent<UnitInfoT>().FriendFoe && other.GetComponent<MovementT>().isSelected==true)
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
		Stay(other.gameObject);
		if(isSelected)
		{
			//no knockback nor push
		}
		else
		{
			//knockback on other
			moveDirection = other.transform.position - transform.position;
			moveDirection.Normalize ();
			other.transform.Translate(moveDirection*(knockback));
		}
	}
	
	void OnTriggerExit2D( Collider2D other )
	{
		Exit(other.gameObject);
	}
	//=========================================================================================


	
	//=========================================================================================
	//isSelected only
	//=========================================================================================



	//=========================================================================================
	//perform movement
	void LateUpdate()
	{
		if(isSelected)
		{
			//check obstacles
			if(isTouch)
			{
				//doesn't move, other's knockback will move it out of touch
			}
			else
			{
				step = Vector3.Distance(transform.position,curPosition);
				
				//check last step of movement allowed
				if (MovementAllowed > 0 && step > 0)
				{
					//reduce movement allowed and moves
					MovementAllowed-=(FollowCost/Leadership);//*Time.deltaTime;
					transform.position = Vector3.MoveTowards(transform.position, curPosition, dragspeed*Time.deltaTime);
				}
				else if (MovementAllowed < 0)
				{
					//apply knockback due to movement exhaustion
					moveDirection = curPosition-transform.position;
					moveDirection.Normalize ();
					transform.Translate(moveDirection*MovementAllowed);
					MovementAllowed=0;
					isDrag=false;
					
					//stop all followers
					StopFollowers();
				}
				else
				{
					//end movement phase
				}
			}
		}
	}
	//=========================================================================================


	//=========================================================================================
	//collision check from collisions class
	void Enter(GameObject other)
	{
		if (isSelected)
		{
			//if the other is an allied
			if (other.GetComponent<UnitInfoT>().FriendFoe==gameObject.GetComponent<UnitInfoT>().FriendFoe)
			{
				//add allied to followers
				if (followers.Contains(other.gameObject)==false)
				{
					followers.Add(other.gameObject);
					//update follow cost
					FollowCost +=2*(other.gameObject.GetComponent<UnitInfoT>().Leadership);
				}
				//keeps the drag input, ignores contact with allied
			}
			//if the other is an enemy
			else
			{
				//stops the drag input
				isDrag = false;
				isTouch = true;
				StopFollowers();
				//knockback movement cost
				MovementAllowed-=1;
			}
		}
	}
	
	void Stay(GameObject other)
	{
		if (isSelected)
		{
			//if the other is an allied
			if (other.GetComponent<UnitInfoT>().FriendFoe!=gameObject.GetComponent<UnitInfoT>().FriendFoe)
			{
				//stops the drag input
				isDrag = false;
				isTouch = true;
				StopFollowers();
				//knockback movement cost
				MovementAllowed-=1;
			}
		}
	}
	
	void Exit(GameObject other)
	{
		if (isSelected)
		{
			//keeps the drag input stopped
			isTouch = false;
		}
	}
	//=========================================================================================


	//=========================================================================================
	//drag and drop
	void OnMouseDown()
	{
		if(isSelected)
		{
			isDrag = true;
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		}
	}

	void OnMouseUp()
	{
		if(isSelected)
		{
			isDrag = false;
			//update follow cost
			if (followers.Count>0)
			{
				FollowCost -= 2*(followers.Last().GetComponent<UnitInfoT> ().Leadership);
				//stop last follower
				followers.Last().SendMessage ("StopFollowing");
				followers.Remove (followers.Last());
			}
		}
	}
	//=========================================================================================
}