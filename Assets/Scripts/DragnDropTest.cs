using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(CircleCollider2D))]

public class DragnDropTest : MonoBehaviour {
	
	//public CircleCollider2D collider;
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
		else
		{
			//end turn
		}
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