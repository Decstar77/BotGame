using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLoorSet : MonoBehaviour {

	[SerializeField] private GameObject[] obsticals;

	

	void Start () {
		SpawnObsticals();
	}	
	
	void Update () {
		
	}

	void SpawnObsticals()
	{
		if (obsticals.Length != 3)
		{
			return;
		}
		float r = Random.Range(0, 100);
		if (r < 30)
		{
			GameObject temp = Instantiate(obsticals[0]);
			Vector3 pos = transform.Find("Right_SpawnPoint").position;
			temp.transform.parent = transform;
			temp.transform.position = pos;
		}
		else if ( r < 50)
		{
			GameObject temp = Instantiate(obsticals[1]);
			Vector3 pos = transform.Find("Left_SpawnPoint").position;
			temp.transform.parent = transform;
			temp.transform.position = pos;
		}
		else if (r < 70)
		{
			GameObject temp = Instantiate(obsticals[2]);
			temp.transform.parent = transform;
			Vector3 pos = transform.Find("Middle_SpawnPoint").position;
			temp.transform.position = pos;
		}
		else
		{

		}
	}

	

}
