using UnityEngine;
using System.Collections;

//backup script
[RequireComponent(typeof(CircleCollider2D))]

public class DragnDrop : MonoBehaviour {
	
	public CircleCollider2D collider;
	public float speed=10f;
	public float knockback=0.02f;
	
	private float limx = 19f;		//battlefield border x
	private float limy = 10f;		//battlefield border y
	private Vector3 curScreenPoint;
	private Vector3 curPosition;
	private Vector3 otherPosition;
	private Vector3 moveDirection;
	private Vector3 screenPoint;
	private Vector3 offset;
	private float step;
	private bool isDraggable;		//static
	private bool isTouching;		//static
	private float MovementAllowed;
	
	void Start () {
		MovementAllowed = gameObject.GetComponent<AlliedInfo>().Speed;
	}
	
	void Update () {
		
		if (MovementAllowed > 0)
		{
			if(isDraggable)
			{
				curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
				curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
				
				//check borders
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
				curPosition=transform.position;
			}
			
			if(isTouching)
			{
				moveDirection = otherPosition - transform.position;
				moveDirection.Normalize ();
				//curPosition=moveDirection*(-knockback);
				//transform.position = Vector3.MoveTowards(transform.position, curPosition, speed*Time.deltaTime);
				transform.Translate(moveDirection*(-knockback));
			}
			else
			{
				step = (transform.position-curPosition).magnitude;
				if (MovementAllowed > step)
				{
					MovementAllowed-=step;
					transform.position = Vector3.MoveTowards(transform.position, curPosition, speed*Time.deltaTime);
				}
				else
				{
					moveDirection = transform.position-curPosition;
					moveDirection.Normalize ();
					curPosition=moveDirection*MovementAllowed;
					transform.position = Vector3.MoveTowards(transform.position, curPosition, speed*Time.deltaTime);
					MovementAllowed=0f;
				}
			}
		}
		else
		{
			//end turn
		}
	}
	
	void OnTriggerEnter2D( Collider2D other )
	{
		isDraggable = false;
		isTouching = true;
		otherPosition = other.transform.position;
		
		//if allied or pushable obstacle do push
		//to do
		
		//send message to touched object unitinfo script to check if it can cast on touched skills
		other.gameObject.SendMessage ("MessageHere");
		//or using a similar command MovementAllowed = gameObject.GetComponent<AlliedInfo>().Speed;
		
		//send message to touching object unitinfo script to check if it can cast on touching skills
		gameObject.SendMessage ("MessageHere");
	}
	
	void OnTriggerExit2D( Collider2D other )
	{
		isDraggable = true;
		isTouching = false;
		
		//send message to touched object unitinfo script to check if it can cast on exit skills
		other.gameObject.SendMessage ("MessageHere");
		
		//send message to touching object unitinfo script to check if it can cast on exiting skills
		gameObject.SendMessage ("MessageHere");
	}
	
	void OnMouseDown() {
		isDraggable = true;
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	
	void OnMouseUp() {
		isDraggable = false;
	}
	
	//show movement bar
	void OnGUI(){
		GUI.Label(new Rect(0,0,300,50),"movement allowed "+MovementAllowed);
	}
}