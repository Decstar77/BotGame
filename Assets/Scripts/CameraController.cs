using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	
	[SerializeField] private Transform follow;
	[SerializeField] private Vector3 offset;
	void Start () {
		
	}	
	
	void Update () {
		if (follow != null)
		{
			Vector3 pos = follow.position;
			transform.position = pos - offset;
		}
	}
}
