using UnityEngine;
using System.Collections;

//{}

public class CollisionK : MonoBehaviour {
	
	public bool isSelected;				//character selected to be moved
	
	public float followspeed=50f;		//drag speed
	public float knockback=0.02f;		//knockback speed
	public float pushspeed=0.1f;		//push speed
	
	private float limx = 18f;			//battlefield border x
	private float limy = 9f;			//battlefield border y
	private Vector3 curPosition;		//next position to move towards
	private Vector3 moveDirection;		//knockback direction
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		curPosition = transform.position;
		
		//check borders of battlefield
		if(curPosition.x > limx){
			curPosition.x = limx;
			transform.position = Vector3.MoveTowards(transform.position, curPosition, followspeed*Time.deltaTime);
		}
		else if(curPosition.x < -limx)
		{
			curPosition.x = -limx;
			transform.position = Vector3.MoveTowards(transform.position, curPosition, followspeed*Time.deltaTime);
		}
		if(curPosition.y > limy){
			curPosition.y = limy;
			transform.position = Vector3.MoveTowards(transform.position, curPosition, followspeed*Time.deltaTime);
		}
		else if(curPosition.y < -limy)
		{
			curPosition.y = -limy;
			transform.position = Vector3.MoveTowards(transform.position, curPosition, followspeed*Time.deltaTime);
		}
	}
	
	//=========================================================================================
	//passive collision check
	//=========================================================================================
	void OnTriggerEnter2D( Collider2D other )
	{	
		SendMessage ("MessageEnter", other.gameObject);
		if(isSelected)
		{
			//no knockback nor push
		}
		else
		{
			if (other.GetComponent<UnitInfoK>().FriendFoe==gameObject.GetComponent<UnitInfoK>().FriendFoe)
			{
				//pushed by allied
				moveDirection = transform.position - other.transform.position;
				moveDirection.Normalize ();
				transform.Translate(moveDirection*(pushspeed));
			}
			else
			{
				//knockback on enemy
				moveDirection = other.transform.position - transform.position;
				moveDirection.Normalize ();
				other.transform.Translate(moveDirection*(knockback));
			}
			//pushable obstacle
			//to do
		}
		
		//check target skill activation
		//to do
		
		//check user skill activation
		//to do
	}
	
	void OnTriggerStay2D( Collider2D other )
	{
		SendMessage ("MessageStay", other.gameObject);
		if(isSelected)
		{
			//no knockback nor push
		}
		else
		{
			if (other.GetComponent<UnitInfoK>().FriendFoe==gameObject.GetComponent<UnitInfoK>().FriendFoe)
			{
				//pushed by allied
				moveDirection = transform.position - other.transform.position;
				moveDirection.Normalize ();
				transform.Translate(moveDirection*(pushspeed));
			}
			else
			{
				//knockback on enemy
				moveDirection = other.transform.position - transform.position;
				moveDirection.Normalize ();
				other.transform.Translate(moveDirection*(knockback));
				//pushable obstacle
				//to do
			}
		}
		
		//check target skill activation
		//to do
		
		//check user skill activation
		//to do
	}
	
	void OnTriggerExit2D( Collider2D other )
	{
		SendMessage ("MessageExit", other.gameObject);
		
		//check target skill activation
		//to do
		
		//check user skill activation
		//to do
	}
	
}

