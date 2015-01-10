using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(CircleCollider2D))]

public class DragnDrop3 : MonoBehaviour {
	
	private CircleCollider2D other;	
	public float speed=10f;				//mouse follow speed
	public float knockback=0.02f;		//knockback on collision
	public float MovementAllowed;		//max movement allowed in a turn for the unit on turn

	private GameObject otherguy;
	private Vector3 othePos;			//position of the other collider of a collision
	private int otheFriend;				//type of collider: 1 allied 2 enemy 3 orb 4 pushable obstacle 5 solid obstacle
	private int otherWeight;			//weight of the collider
	private float pushdistance;
	private float limx = 19f;			//battlefield border x
	private float limy = 10f;			//battlefield border y
	private Vector3 curScreenPoint;		//coordinates of mouse on screen
	private Vector3 curPosition;		//next position to move towards
	private Vector3 moveDirection;		//knockback direction
	private Vector3 screenPoint;		//z
	private Vector3 offset;				//starting position
	private float step;					//distance to get to mouse from current position
	private bool isDrag;				//flag is draggable
	private bool isTouch;				//flag is colliding with another collider
	
	void Start () {
		//check if this is the unit that can be moved on this turn
		//to do

		//initialize movement allowed
		MovementAllowed = gameObject.GetComponent<AlliedInfo>().Speed;
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

				//check borders of battlefield
				if(curPosition.x > limx){
					curPosition.x = limx;
				}else if(curPosition.x < -limx){
					curPosition.x = -limx;
				}
				if(curPosition.y > limy){
					curPosition.y = limy;
				}else if(curPosition.y < -limy){
					curPosition.y = -limy;
				}
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
			//apply knockback
			moveDirection = othePos - transform.position;
			moveDirection.Normalize ();
			transform.Translate(moveDirection*(-knockback));

			//apply push on allies
			if (otheFriend==1){
				pushdistance=1+((gameObject.GetComponent<UnitInfo>().Weight)/otherWeight);
				otherguy.transform.Translate(moveDirection*pushdistance);
				MovementAllowed-=pushdistance;

				//check pushed position valid
				//to do
			}

		}
		else
		{
			step = Vector3.Distance(transform.position,curPosition);
			//check last step of movement allowed
			if (MovementAllowed > 0 && step > 0)
			{
				MovementAllowed-=speed*Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, curPosition, speed*Time.deltaTime);
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
	void OnTriggerEnter2D( Collider2D other )
	{
		//stops the drag input
		isDrag = false;
		isTouch = true;
		othePos = other.transform.position;
		otherguy = other.gameObject;
		otheFriend= other.gameObject.GetComponent<UnitInfo> ().FriendFoe;
		otherWeight= otherguy.GetComponent<UnitInfo>().Weight;
		
		//if allied or pushable obstacle set flags for push
		//to do
		
		//check target skill activation
		//to do
		other.gameObject.SendMessage ("MessageHere"); //sample
		
		//check user skill activation
		//to do
	}

	void OnTriggerStay2D( Collider2D other )
	{
		//stops the drag input
		isDrag = false;
		isTouch = true;
		
		//check target skill activation
		//to do
		other.gameObject.SendMessage ("MessageHere"); //sample
		
		//check user skill activation
		//to do
	}

	void OnTriggerExit2D( Collider2D other )
	{
		//keeps the drag input stopped
		//isDrag = true;
		isTouch = false;
		
		//check target skill activation
		//to do
		other.gameObject.SendMessage ("MessageHere"); //sample
		
		//check user skill activation
		//to do
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