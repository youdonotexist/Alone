using UnityEngine;
using System.Collections;

public class InteractionTrigger : Interaction
{
	// Use this for initialization
	void Start ()
	{
		
	}
	
	public virtual void OnTriggerEnter(Collider c) {
		if (c.tag.Equals("Player")) {
			PlayerControl pc = c.GetComponent<PlayerControl>();
			pc.currentRoom.doEvent(_eventName, true);
			Debug.Log("Trigger entered");
		}
		
		Debug.Log("Trigger");
	}
}

