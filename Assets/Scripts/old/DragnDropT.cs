using UnityEngine;
using System.Collections;
using System.Collections.Generic;		//for list
using System.Linq;						//for list
//{}

public class DragnDropT : MonoBehaviour {
	
	public float dragspeed=50f;			//mouse follow speed
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

	void Start ()
	{
		//initialize movement allowed
		MovementAllowed = gameObject.GetComponent<UnitInfoT>().Movement;

		//initialize follow cost
		Leadership = gameObject.GetComponent<UnitInfoT> ().Leadership;
		FollowCost = Leadership;

		//initialize followers
		followers = new List<GameObject>();
	}
	//=========================================================================================
	//get move destination
	void Update ()
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
	//=========================================================================================
	//perform movement
	void LateUpdate()
	{
		//check obstacles
		//if(isTouch=false)
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
				//MovementAllowed-=(FollowCost/Leadership)*dragspeed*Time.deltaTime;
				MovementAllowed-=(FollowCost/Leadership)*Time.deltaTime;
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
				StopFollow();
			}
			else
			{
				//end movement phase
			}
		}
	}
	//=========================================================================================
	//collision check from collisions class
	void MessageEnter(GameObject other)
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
			StopFollow();
			//knockback movement cost
			MovementAllowed-=1;
		}
	}
	
	void MessageStay(GameObject other)
	{
		//if the other is an allied
		if (other.GetComponent<UnitInfoT>().FriendFoe!=gameObject.GetComponent<UnitInfoT>().FriendFoe)
		{
			//stops the drag input
			isDrag = false;
			isTouch = true;
			StopFollow();
			//knockback movement cost
			MovementAllowed-=1;
		}
	}
	
	void MessageExit(GameObject other)
	{
		//keeps the drag input stopped
		isTouch = false;
	}
	//=========================================================================================
	//drag and drop
	void OnMouseDown()
	{
		isDrag = true;
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	void OnMouseUp()
	{
		isDrag = false;
		//update follow cost
		FollowCost -= 2*(followers.Last().GetComponent<UnitInfoT> ().Leadership);
		//stop last follower
		followers.Last().SendMessage ("StopFollowing");
		followers.Remove (followers.Last());
	}
	//=========================================================================================
	//stop all followers
	void StopFollow()
	{
		while(followers.Count>0)
		{
			FollowCost = Leadership;
			followers.Last().SendMessage ("StopFollowing");
			followers.Remove (followers.Last());
		}
	}
	//=========================================================================================
	//show movement allowed bar
	void OnGUI()
	{
		GUI.Label(new Rect(0,0,300,50),"movement allowed "+MovementAllowed);
		GUI.Label (new Rect(0,10,300,50),"movement cost " + FollowCost/Leadership);
	}
	//=========================================================================================
}