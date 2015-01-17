using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(CircleCollider2D))]

public class DragnDropK : MonoBehaviour {
	
	public float followspeed=50f;		//mouse follow speed
	public float limx = 18f;			//battlefield border x
	public float limy = 9f;				//battlefield border y
	
	private Vector3 curPosition;		//next position to move towards
	private Vector3 curScreenPoint;		//coordinates of mouse on screen
	private Vector3 screenPoint;		//z
	private Vector3 offset;				//starting position
	private Vector3 moveDirection;		//knockback direction
	
	private GameObject otherguy;
	private float MovementAllowed;		//max movement allowed in a turn for the unit on turn
	private float step;					//distance to get to mouse from current position
	private bool isDrag;				//flag is draggable
	private bool isTouch;				//flag is colliding with another collider
	
	void Start () {
		//initialize movement allowed
		MovementAllowed = gameObject.GetComponent<UnitInfoK>().Movement;
	}
	
	void Update () {
		
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
		else
		{
			//don't update position to move towards
		}
	}
	
	void LateUpdate(){
		//check obstacles
		if(isTouch)
		{
			
		}
		else
		{
			step = Vector3.Distance(transform.position,curPosition);
			//check last step of movement allowed
			if (MovementAllowed > 0 && step > 0)
			{
				MovementAllowed-=followspeed*Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, curPosition, followspeed*Time.deltaTime);
			}
			//apply knockback due to movement allowed exhaustion
			else if (MovementAllowed < 0)
			{
				moveDirection = curPosition-transform.position;
				moveDirection.Normalize ();
				transform.Translate(moveDirection*MovementAllowed);
				MovementAllowed=0;
				isDrag=false;
			}
			else
			{
				//end movement phase
			}
		}
	}
	
	//=========================================================================================
	//collision check
	//=========================================================================================
	void MessageEnter(GameObject other)
	{
		otherguy = other.gameObject;
		if (other.GetComponent<UnitInfoK>().FriendFoe==gameObject.GetComponent<UnitInfoK>().FriendFoe)
		{
			//keeps the drag input
			isDrag = true;
			isTouch = true;
			//push movement cost
			MovementAllowed-=((otherguy.GetComponent<UnitInfoK>().Weight)/(gameObject.GetComponent<UnitInfoK>().Weight));
		}
		else
		{
			//stops the drag input
			isDrag = false;
			isTouch = true;
			//knockback movement cost
			MovementAllowed-=1;
		}
	}
	
	void MessageStay(GameObject other)
	{
		otherguy = other.gameObject;
		if (other.GetComponent<UnitInfoK>().FriendFoe==gameObject.GetComponent<UnitInfoK>().FriendFoe)
		{
			//keeps the drag input
			isDrag = true;
			isTouch = true;
			//push movement cost
			MovementAllowed-=((otherguy.GetComponent<UnitInfoK>().Weight)/(gameObject.GetComponent<UnitInfoK>().Weight));
		}
		else
		{
			//stops the drag input
			isDrag = false;
			isTouch = true;
			//knockback movement cost
			MovementAllowed-=1;
		}
	}
	
	void MessageExit(GameObject other)
	{
		otherguy = other.gameObject;
		//keeps the drag input stopped
		isTouch = false;
	}
	
	
	//=========================================================================================
	//drag and drop
	//=========================================================================================
	void OnMouseDown() {
		isDrag = true;
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	void OnMouseUp() {
		isDrag = false;
	}
	
	//=========================================================================================
	//show movement allowed bar
	//=========================================================================================
	void OnGUI(){
		//!! to be changed into a bar with gauge
		//to do
		GUI.Label(new Rect(0,0,300,50),"movement allowed "+MovementAllowed);
	}
	
	//=========================================================================================
}