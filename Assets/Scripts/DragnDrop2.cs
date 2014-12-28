using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]

public class DragnDrop2 : MonoBehaviour {
	
	public CircleCollider2D collider;
	public float knockback=0.02f;
	public float teleport=0.02f;

	private Vector3 curScreenPoint;
	private Vector3 curPosition;
	private Vector3 telPosition;
	private Vector3 prevPosition;
	private Vector3 otherPosition;
	private Vector3 moveDirection;
	private Vector3 screenPoint;
	private Vector3 offset;
	private bool isDraggable;
	private bool isTouching;
	
	// Use this for initialization
	void Start () {
		isDraggable = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(isTouching)
		{
			moveDirection = otherPosition - transform.position;
			moveDirection.Normalize ();
			transform.position -= moveDirection * knockback;
		}
	}

	void OnTriggerEnter2D( Collider2D other )
	{
		isDraggable = false;
		isTouching = true;
		otherPosition = other.transform.position;
	}
	
	void OnTriggerExit2D( Collider2D other )
	{
		isDraggable = true;
		isTouching = false;
	}

	void OnMouseDown() {
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	
	void OnMouseDrag()
	{
		if(isDraggable)
		{
			curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

			if(Vector3.Distance(curPosition,prevPosition) > teleport)
			{
				telPosition = curPosition - prevPosition;
				telPosition.Normalize();
				curPosition = prevPosition + telPosition*teleport;
				transform.position = curPosition;
				prevPosition=curPosition;
			}else
			{
				transform.position = curPosition;
				prevPosition=curPosition;
			}
		}
	}
}