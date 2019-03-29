using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterFloor : MonoBehaviour {

	[SerializeField] private GameObject FloorSet = null;
	[SerializeField] private GameObject PlayerObject = null;
	[SerializeField] private int FloorLead = 10;
	[SerializeField] private GameObject[] obsticals;
	

	GameObject []floors;
	private float floorSize;
	private int closeIndex;
	private int farIndex;
	void Start () {

		floors = new GameObject[FloorLead];
		GameObject temp = Instantiate(FloorSet);
		//Get the size of the floor
		Transform box = temp.transform.Find("Middle_SpawnPoint");
		if (box != null)
			floorSize = box.GetComponent<BoxCollider>().bounds.size.z;
		
		//Create/SetUp First Floor
		temp.transform.position = new Vector3(0, 0, 0);
		temp.name = "Floor:0";
		floors[0] = temp;
		closeIndex = 0;
		farIndex = FloorLead - 1;

		for (int i = 1; i < FloorLead; i++)
		{
			temp = Instantiate(FloorSet);
			temp.transform.position = new Vector3(0, 0, floors[i - 1].transform.Find("SpawnPoint").transform.position.z);
			temp.name = "FloorSet:" + i;
			floors[i] = temp;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckFloorPass();
		
	}
	
	void CheckFloorPass()
	{
		BotController bot = PlayerObject.GetComponent<BotController>();
		if (bot != null)
		{
			if (bot.GetPassed())
			{
				MoveLastFloor();
				bot.SetPassed(false);
			}
		}
	}
	void MoveLastFloor()
	{
		float z = floors[farIndex].transform.Find("SpawnPoint").transform.position.z;
		floors[closeIndex].transform.position = new Vector3(0, 0, z);
		farIndex = closeIndex++;
		closeIndex = closeIndex > FloorLead - 1 ? 0 : closeIndex;
	}

}
