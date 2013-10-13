using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour
{
	public GameObject followObject = null;
	// Use this for initialization
	void Start ()
	{
		
	}

	// Update is called once per frame
	void Update ()
	{
		Vector3 followPos = followObject.transform.position;
		Vector3 camPos = transform.position;
		camPos = new Vector3(followPos.x, camPos.y, followPos.z);
		transform.position = camPos;
		
	}
}

