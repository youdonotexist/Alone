using UnityEngine;
using System.Collections;

public class Door : InteractionTrigger
{
	public RoomEvents _room;
	public GameObject _exitPoint;
	public Door _oppositeDoor;
	public string _lockName = null;
	
	public bool FlipH = false;
	public bool FlipV = false;
	
	//Interaction _interaction = null;
	
	// Use this for initialization
	void Start ()
	{
		//_interaction = GetComponent<Interaction>();
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public override void OnTriggerEnter(Collider other) {
		GameObject trig = other.gameObject;
		if (trig.tag.Equals("Player")) {
			if (_oppositeDoor != null) {
				PlayerControl pc = trig.GetComponent<PlayerControl>();
				if (_lockName == null || _lockName.Length == 0) {
					Vector3 newPos = _oppositeDoor._exitPoint.transform.position;
					trig.transform.position = new Vector3(newPos.x, trig.transform.position.y, newPos.z);
					
					Vector2 face = pc.getFacing();
					if (FlipH || FlipV) {
						if (FlipH)
							face.x *= -1.0f;
						if (FlipV)
							face.y *=1.0f;
						
						pc.setFacing(face);
					}
					
					_oppositeDoor._room.gameObject.SetActiveRecursively(true);
					_room.gameObject.SetActiveRecursively(false);
					pc.currentRoom = _oppositeDoor._room;
				}
				else {
					bool hasKey = PlayerKeys.instance.hasKey(_lockName);
					if (hasKey) {
						Vector3 newPos = _oppositeDoor._exitPoint.transform.position;
						trig.transform.position = new Vector3(newPos.x, trig.transform.position.y, newPos.z);
					
						_oppositeDoor._room.gameObject.SetActiveRecursively(true);
						_room.gameObject.SetActiveRecursively(false);	
						pc.currentRoom = _oppositeDoor._room;
					}
					else {
						GameObject go = trig.gameObject;
						PlayerControl control = go.GetComponent<PlayerControl>();
						if (_eventName != -1) {
							control.startInteraction();
							control.setFacing();
							control.currentRoom.doEvent(_eventName, true);
						}
						else if (_lockName != null) {
							string[] s = PlayerKeys.instance.lockedMessage(_lockName);
							_interactions = s;
							_currentInteraction = 0;
							control.setFacing();
							control.showInteraction(this, true);
						}
					}
				}
			}
		}
	}
}

