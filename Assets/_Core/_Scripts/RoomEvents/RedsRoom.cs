using UnityEngine;
using System.Collections;

public class RedsRoom : RoomEvents
{
	public int CAT_FLEE = 0;
	public int PICKUP_TEDDY = 1;
	public int ENTER_BEDROOM = 2;
	public int CROSS = 3;
	public int BED = 4;
	public int SAFE = 5;
	public int FEED = 6;
	
	public Cat _Cat;
	public Interactable _Mouse;
	public GameObject _Teddy;
	//public GameObject _Cross;
	//public GameObject _Bed;
	public GameObject _FearTrigger;
	
	public UITextField _puzzle1;
	public UITextField _puzzle2;
	
	public GameObject[] CatPath;
	
	public Door _exit;
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start();
		_events[CAT_FLEE] = false;
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public override void doEvent(int event_name, bool fromTrigger) {
		bool done = didEvent(event_name);
			
		if (event_name == CAT_FLEE && done == false) {
			setEventFinished(event_name);
			StartCoroutine(startFleeScene(fromTrigger));
		}
		
		if (event_name == PICKUP_TEDDY && done == false) {
			setEventFinished(event_name);
			StartCoroutine(pickupTeddy(fromTrigger));
		}
		else if (event_name == ENTER_BEDROOM && done == false) {
			setEventFinished(event_name);
			StartCoroutine(enterBedroom(fromTrigger));
		}
		else if (event_name == CROSS && done == false) {
			setEventFinished(event_name);
			StartCoroutine(cross(fromTrigger));
		}
		else if (event_name == BED && done == false) {
			setEventFinished(event_name);
			StartCoroutine(deadParents(fromTrigger));
		}
		else if (event_name == SAFE && done == false) {
			setEventFinished(event_name);
			StartCoroutine(safeOpened(fromTrigger));
		}
		else if (event_name == FEED && done == false) { 
			setEventFinished(event_name);
			StartCoroutine(feedAlert(fromTrigger));
		}
	}
	
	IEnumerator feedAlert(bool fromTrigger) {
		textEvent(new string[] { "I should feed Kitty before I go.."}, fromTrigger);
		
		while (showingText) {
			yield return null;
		}
		_red.finishInteraction();
		restartEvent(FEED);
	}
	
	IEnumerator safeOpened(bool fromTrigger) {
		if (_Safe.OpenSafe(new string[] {_puzzle1.Text, _puzzle2.Text})) {
			_Safe.setOpenSprite();
			
			textEvent(new string[] {"*Picked Up " + _Safe.key_name + "!*", "Ashley: *sniff*"}, fromTrigger);
	
			while (showingText) {
				yield return null;
			}	
			
			_red.finishInteraction();
		}
		else {
			textEvent( new string[] {"Ashley:  The safe is locked.  I need to figure out the passphrase.."}, fromTrigger);
	
			while (showingText) {
				yield return null;
			}	
			
			_red.finishInteraction();	
			restartEvent(SAFE);
		}
	}
	
	IEnumerator deadParents(bool fromTrigger) {
		textEvent( new string[] {"Ashley: M-mom..?  D-dad...?", "...", "......", "..........", "Ashley: *sniff*"}, fromTrigger );

		while (showingText) {
			yield return null;
		}	
		
		_red.finishInteraction();
	}
	
	IEnumerator pickupTeddy(bool fromTrigger) {
		if (PlayerKeys.instance.hasKey("JanitorMeeting")) {
			PlayerKeys.instance.addKey("ParentBedroom");
			
			textEvent(new string[] {"*Picked Up Teddy Bear*", "Ashley:  I feel less scared carrying you around"}, fromTrigger);
	
			while (showingText) {
				yield return null;
			}
			
			_Teddy.active = false;
			Destroy(_Teddy.gameObject);
			
			Destroy(_FearTrigger.gameObject);
			_red.finishInteraction();
		}
		else {
			Debug.Log("Picking up teddy..");
			textEvent(new string[] {"Teddy.. will mom and dad ever wake up?"}, fromTrigger);
	
			while (showingText) {
				yield return null;
			}
			
			Debug.Log("Finished teddy");
			_red.finishInteraction();
			restartEvent(PICKUP_TEDDY);
		}
	}
	
	IEnumerator enterBedroom(bool fromTrigger) {
		if (PlayerKeys.instance.hasKey("ParentBedroom")) {
				
		}
		else {
			textEvent(new string[] {"Ashley:  I'm.. too scared to go in there.."}, fromTrigger);
			
			while (showingText) {
				yield return null;
			}
			_red.finishInteraction();
			restartEvent(ENTER_BEDROOM);
		}
	}
	
	IEnumerator cross(bool fromTrigger) {
		if (PlayerKeys.instance.hasKey("JanitorMeeting")) {
			textEvent( new string[] {"Ashley:  Dad said the end times are near.. and that we should start going to church..", "Ashley: If not for him stocking up on canned beans.. Kitty and I would be...."}, fromTrigger);
			
			while (showingText) {
				yield return null;
			}
			
			_red.finishInteraction();
		}
		else {
			textEvent(new string[] {"Ashley:  Dad said the end times are near.. and that we should start going to church.."}, fromTrigger);
	
			while (showingText) {
				yield return null;
			}
			
			_red.finishInteraction();
			restartEvent(CROSS);
		}
	}
	
	IEnumerator startFleeScene(bool fromTrigger) {
		textEvent(new string[] {"Ashley:  Here, Kitty..  I'm sorry there isn't more food.."}, fromTrigger);
		
		while (showingText) {
			yield return null;
		}
		
		PlayerKeys.instance.addKey("FeedCat");
		_red.startInteraction();
		
		_Cat.setFacing(new Vector2(-1.0f, 0.0f));
		_Cat._state = Interactable.STATE.STOP;
		
		float t = 0.0f;
		while (t < 2.0f) {
			t+=Time.deltaTime;
			yield return null; 
		}
		
		Debug.Log("Starting flee scene");
		
		/////////////////
		// Mouse Flee
		////////////////
		_Mouse._interacting = false;
		_Mouse._state = Interactable.STATE.FOLLOW_PATH;
		
		_Mouse.FollowPath(CatPath);
		
		while (Vector3.Distance(_Mouse.transform.position, CatPath[CatPath.Length-1].transform.position) > 0.05f) {
			yield return null;
		}
		
		_Mouse.gameObject.active = false;
		_Mouse.transform.parent = _exit._oppositeDoor.transform;
		
		/////////////////
		// Red Yell
		////////////////
		
		textEvent(new string[] {"Kitty, Stop!!"}, fromTrigger);
		
		while (showingText) {
			yield return null;
		}
		
		/////////////////
		// Cat Flee
		////////////////
		
		_Cat._interacting = false;
		_Cat._state = Cat.STATE.FOLLOW_PATH;
		_Cat.FollowPath(CatPath);
		_red.startInteraction();
		
		while (Vector3.Distance(_Cat.transform.position, CatPath[CatPath.Length-1].transform.position) > 0.05f) {
			yield return null;
		}
		
		_Cat.gameObject.active = false;
		_Cat.transform.parent = _exit._oppositeDoor.transform;
		_red.finishInteraction();
		
		_Cat._state = Interactable.STATE.STOP;
		_Mouse._state = Interactable.STATE.STOP;
		_exit._oppositeDoor._room.doEvent(SecondFloorMain.SET_FLEE_POS, fromTrigger);
	}
}

