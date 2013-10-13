using UnityEngine;
using System.Collections;

public class FirstFloorMain : RoomEvents
{
	const int MEET_JANITOR = 0;
	const int ENTER_GOLD = 1;
	
	public Interactable _janitor;
	
	public Transform _janitorFlee;
	
	public Door _basementDoor;

	public override void doEvent(int event_name, bool fromTrigger) {
		bool done = didEvent(event_name);
		if (event_name == MEET_JANITOR && done == false) {
			setEventFinished(event_name);
			StartCoroutine(meetJanitor(fromTrigger));
		}
		else if (event_name == ENTER_GOLD && done == false) {
			setEventFinished(event_name);
			StartCoroutine(enterGoldRoom(fromTrigger));
		}
	}
	
	IEnumerator enterGoldRoom(bool fromTrigger) {
		if (PlayerKeys.instance.hasKey("GoldKey")) {	
			
		} else {
			_red.startInteraction();
			
			textEvent( new string[] {"Ashley: Hmm.. it's locked..", "Ashley: ...", "Ashley:  Dad told me that the apartment building had an elaborate key exchange program..",
														"Ashley: If you lost your key, you could go to the apartment number one less than your own.", 
														"Each room has a safe that can only be opened with a passphrase..", "I need to start with apartment one.  I'm on my way, Kitty!" }, fromTrigger);
	
	
			while (showingText) {
				yield return null;
			}
			
			PlayerKeys.instance.addKey("CheckGoldRoom");
			PlayerKeys.instance.setLockedMessages(null, "GoldKey");
			_red.finishInteraction();
		}
	}
	
	IEnumerator meetJanitor(bool fromTrigger) {
		if (PlayerKeys.instance.hasKey("JanitorMeeting")) {
			//Ignore
		}
		else {
			_red.startInteraction();
			
			textEvent( new string[] {"Ashley: Ahhhh!", "Ashley: You!", "Ashley:  Why do you keep following me around??", 
												"Man: I.. I .. ", "Ashley:  My dad told me to stay away from you!", 
												"Man: I... I..", "Ashley: JUST LEAVE ME ALONE!!"}, fromTrigger);
			
			_red.setFacing(new Vector2(0.0f, -1.0f));
			PlayerKeys.instance.addKey("JanitorMeeting");
	
			while (showingText) {
				yield return null;
			}
			
			_janitor._state = Interactable.STATE.TO_POINT;
			_janitor.goToPoint = new Vector2(_janitorFlee.position.x, _janitorFlee.position.z);
			_red.startInteraction();
			
			while (Vector3.Distance(_janitor.transform.position, _janitorFlee.position) > 0.05f) {
				yield return null;
			}
			
			Destroy(_janitor.gameObject);
			//_janitor.transform.parent = _basementDoor._oppositeDoor._room.transform;
			
			textEvent( new string[] {"Ashley: That guy creeps me out.", "Ashley:  He brings me food.. and creeps around the apartment building like some sort of ..", 
												"Man: .. janitor!! ", "Ashley:  I wonder if he has a key to the roof..."}, fromTrigger);
	
			while (showingText) {
				yield return null;
			}
			
			string[] messages = new string[] {"Ashley: Mr. Janitor?  Can you hear me??", "Ashley: Do you have a key to the roof?  Someone left it open and my Kitty got out!", "...", "......  ",
				"Ashley: No answer.. ", "Ashley:  He's the only person I've seen in almost a month.. I shouldn't have been so mean.."};
			PlayerKeys.instance.setLockedMessages(messages, "Basement");
			
			string[] goldMessage = new string[] {"I should check Mrs. Gold's room for the roof key.."};
			PlayerKeys.instance.setLockedMessages(goldMessage, "CheckGoldRoom");
			
			_red.finishInteraction();
		}	
	}
}

