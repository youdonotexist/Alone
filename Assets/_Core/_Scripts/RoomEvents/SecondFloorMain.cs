using UnityEngine;
using System.Collections;

public class SecondFloorMain : RoomEvents {
	public static int CAT_FLEE_1 = 0;
	public static int SET_FLEE_POS = 1;
	
	public Cat _Cat;
	public Interactable _Mouse;
	public FollowCam _Camera;
	
	public GameObject[] CatPath1;
	
	public Transform _mouseStart1;
	public Transform _catStart1;
	
	public Door _roofExit;
	
	// Use this for initialization
	public override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void doEvent(int event_name, bool fromTrigger) {
		bool done = didEvent(event_name);
		if (event_name == CAT_FLEE_1 && done == false) {
			setEventFinished(event_name);
			StartCoroutine(startFleeScene1());
		}
		else if (event_name == SET_FLEE_POS) {
			Vector3 cPos = _catStart1.position;
			Vector3 mPos = _mouseStart1.position;
			
			_Cat.transform.position = new Vector3(cPos.x, _Cat.transform.position.y, cPos.z);
			_Mouse.transform.position = new Vector3(mPos.x, _Mouse.transform.position.y, mPos.z);
		}
	}
	
	IEnumerator startFleeScene1() {
		Debug.Log("Starting flee scene1");
		
		_Mouse._state = Interactable.STATE.FOLLOW_PATH;
		_Cat._state = Interactable.STATE.FOLLOW_PATH;
		
		_Mouse._interacting = false;
		_Cat._interacting = false;
		
		_Mouse.FollowPath(CatPath1);
		_Cat.FollowPath(CatPath1);
		
		_Camera.followObject = _Cat.gameObject;
		_red._interacting = false;
		
		Vector3 nPos = CatPath1[CatPath1.Length-1].transform.position;
		Vector3 nodePos = new Vector3(nPos.x, _Cat.transform.position.y, nPos.z);
		
		while (Vector3.Distance(_Cat.transform.position, nodePos) > 0.05f) {
			yield return null;
		}
		
		Sprite s = _roofExit.gameObject.GetComponent<Sprite>();
		s.DoAnim("Closed");
		
		_Camera.followObject = _red.gameObject;
		
		_Cat.gameObject.active = false;
		_Mouse.gameObject.active = false;
		
		Destroy(_Cat.gameObject);
		Destroy(_Mouse.gameObject);
		//_Cat.transform.parent = _roofExit._room.gameObject.transform;
		//_Mouse.transform.parent = _roofExit._room.gameObject.transform;
	}
}

