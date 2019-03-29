using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour {


	[SerializeField] AnimationCurve TransitionCurve;
	[SerializeField] float gravity;
	struct lanes
	{
		public float left;
		public float middle;
		public float right;
	}
	private bool pass = false;
	private bool isRagDoll = false;
	private int currentLane;
	private float currentLanePosition;
	private int nextLane;

	private lanes lanePositions;
	private CharacterController characterController;
	private GameObject ragDoll;

	private Vector3 moveDirection;
	private Vector2 startClick;
	private Vector2 endClick;

	void Start () {
		characterController = GetComponent<CharacterController>();

		currentLane = 1;
		currentLanePosition = 0;
		nextLane = -1;
		TurnRagDollOn(transform);
		lanePositions.left = -4;
		lanePositions.right = 4;
		lanePositions.middle = 0;
	}
	

	void Update () {
		float a = Input.GetKeyDown(KeyCode.A) == true ? 1 : 0;
		float b = Input.GetKeyDown(KeyCode.D) == true ? 1 : 0;
		Vector2 direction = GetSwipe();
		if (direction.x < 0 && nextLane == -1)
		{
			if (currentLane > 0)
			{
				nextLane = currentLane - 1;
			}
		}
		else if (direction.x > 0 && nextLane == -1)
		{
			if (currentLane < 2)
			{
				nextLane = currentLane + 1;
			}
		}
		else if (direction.y > 0)
		{
			moveDirection.y = 8 + gravity * Time.deltaTime;
		}
		else if (direction.y < 0)
		{

		}
		ChangeLane();
		GetSwipe();
		moveDirection.y -= gravity * Time.deltaTime;
		characterController.Move(moveDirection * Time.deltaTime);
		characterController.Move(new Vector3(0, 0, 5) * Time.deltaTime);
	}
	private Vector2 GetSwipe()
	{
		Vector3 mouse;
		if (Input.GetMouseButtonDown(0))
		{
			mouse = Input.mousePosition;
			mouse = Camera.main.ScreenToViewportPoint(mouse);
			startClick = mouse;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			mouse = Input.mousePosition;
			mouse = Camera.main.ScreenToViewportPoint(mouse);
			endClick = mouse;
			Vector2 res = endClick - startClick;
			if (res.magnitude < 0.1)
				return Vector2.zero;
			if (res.y / (res.x + 0.01) < 0.5) //Swiping horizontal
				return Vector2.right * Mathf.Sign(res.x);
			else
				return Vector2.up * Mathf.Sign(res.y);
		}		
		return Vector2.zero;
	}
	private void Jump()
	{

	}
	private void ChangeLane()
	{
		if (nextLane != -1)
		{
			float x;
			float nVal;
			switch (nextLane)
			{
				case 0:
					x = transform.position.x;
					nVal = Utilities.Norimalize(x, currentLanePosition, lanePositions.left);
					float u = TransitionCurve.Evaluate(nVal) * 2;
					x = Mathf.Lerp(x, lanePositions.left, 0.3f);
					
					transform.position = new Vector3(x, transform.position.y, transform.position.z);
					if (Mathf.Abs(x-lanePositions.left) < 0.05f)
					{
						currentLanePosition = lanePositions.left;
						currentLane = nextLane;
						nextLane = -1;
					}
					break;
				case 1:
					x = transform.position.x;
					x = Mathf.Lerp(x, lanePositions.middle, 0.3f);
					transform.position = new Vector3(x, transform.position.y, transform.position.z);
					if (Mathf.Abs(x - lanePositions.middle) < 0.05f)
					{
						currentLanePosition = lanePositions.middle;
						currentLane = nextLane;
						nextLane = -1;
					}
					break;
				case 2:				
					x = transform.position.x;
					x = Mathf.Lerp(x, lanePositions.right, 0.3f);
					transform.position = new Vector3(x, transform.position.y, transform.position.z);
					if (Mathf.Abs(x - lanePositions.right) < 0.05f)
					{
						currentLanePosition = lanePositions.right;
						currentLane = nextLane;
						nextLane = -1;
					}
					break;
			}			
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		StartCoroutine(Move(2));
	}
	private void TurnRagDollOn(Transform tran)
	{
		if (!isRagDoll)
		{
			characterController.enabled = false;
			GetComponent<Rigidbody>().isKinematic = false; ;
			GetComponent<Animator>().enabled = false;
			GetComponent<BotController>().enabled = false;
			isRagDoll = true;
		}

		int cCount = tran.childCount;
		for (int i = 0; i < cCount; i++)
		{
			Transform child = tran.GetChild(i);
			if (child.name == "bot1_rig_v01:eff" || child.name == "bot1_rig_v01:eff5" || child.name == "bot1_rig_v01:eff10")
			{
				print(child.name);
				return;
			}
			Rigidbody ri = child.gameObject.GetComponent<Rigidbody>();
			if (ri != null)	
			{
				CapsuleCollider col =  child.GetComponent<CapsuleCollider>();
				if (col == null)
				{
					BoxCollider col2 = child.GetComponent<BoxCollider>();
					col2.enabled = true;
					col2.isTrigger = false;
				}
				else
				{
					col.enabled = true;
					col.isTrigger = false;
				}
				ri.isKinematic = false;
				TurnRagDollOn(child);
				print(child.name);
			}
			else	
				TurnRagDollOn(child);			
		}
		
	}
	IEnumerator Move(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		pass = true;
	}
	public void SetPassed(bool a)
	{
		pass = a;
	}
	public bool GetPassed()
	{
		return pass;
	}
}
