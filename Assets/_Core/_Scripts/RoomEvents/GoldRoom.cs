using UnityEngine;
using System.Collections;

public class GoldRoom : RoomEvents
{
	const int FOOL = 0;
	const int ALONE = 1;
	const int ELEVATOR = 2;
	const int SAFE = 3;
	
	public UITextField _puzzle1;
	public UITextField _puzzle2;
	
	public Door _serviceDoor;
	
	public override void doEvent(int event_name, bool fromTrigger) {
		bool done = didEvent(event_name);
			
		if (event_name == FOOL && done == false) {
			setEventFinished(event_name);
			StartCoroutine(fool(fromTrigger));
		} 
		else if (event_name == ALONE && done == false) {
			setEventFinished(event_name);
			StartCoroutine(alone(fromTrigger));
		}
		else if (event_name == SAFE && done == false) {
			setEventFinished(event_name);
			StartCoroutine(safe(fromTrigger));
		}
		else if (event_name == ELEVATOR && done == false) {
			setEventFinished(event_name);
			StartCoroutine(elevator(fromTrigger));
		}
	}
	
	IEnumerator fool(bool fromTrigger) {
		string[] messages = new string[] {"Ashley: Ms. Gold was.. not my favorite..", "Ashley: When Mr. White died, Ms. Gold inherited the apartment complex.  She was our landlord", 
											"I would have to bring the payment down to her on the 5th of every month", "And every 5th, she would be sitting at this very table..", 
												"She would look at me right in the eye..", "And say 'The wise make proverbs.. and the fools repeat them'", "I asked my father what Ms. Gold meant..", 
													".. and he told me 'She is calling you foolish, girl.'", "I never liked Ms. Gold." };
		textEvent(messages, fromTrigger);
		
		while (showingText) {
			yield return null;
		}
		
		_red.finishInteraction();
		restartEvent(FOOL);
	}
	
	IEnumerator alone(bool fromTrigger) {
		string[] messages = new string[] {"Ashley: One month.. when I came to drop off the the rent for my father..", ".. I could hear Ms. Gold quitely sobbing on her twin bed", 
											"It.. was so sad to see someone so alone with no one in the world..", "..but maybe no one likes her because she's mean.."};
		textEvent(messages, fromTrigger);
		
		while (showingText) {
			yield return null;
		}
		
		_red.finishInteraction();
		restartEvent(ALONE);
	}
	
	IEnumerator elevator(bool fromTrigger) {
		bool hasElevatorKey = PlayerKeys.instance.hasKey("ElevatorKey");
		if (hasElevatorKey == false) {
			string[] messages = new string[] {"Ashley: Yuck.. this is the service elevator my father used to lock me in.", "Ashley:  It seems to be locked.. good riddance.."};	
			textEvent(messages, fromTrigger);
		
			while (showingText) {
				yield return null;
			}
		}
		else {
			string[] messages = new string[] {"Ashley: Hmm.. I don't think this goes to the roof.. but it does go to the basement..", "Maybe I can find that janitor and get the roof key from him.."};	
			textEvent(messages, fromTrigger);
		
			while (showingText) {
				yield return null;
			}
			
			
		}
		
		
		_red.finishInteraction();
		restartEvent(ELEVATOR);
	}
	
	IEnumerator safe(bool fromTrigger) {
		if (_Safe.OpenSafe(new string[] {_puzzle1.Text, _puzzle2.Text})) {
			_Safe.setOpenSprite();
			
			textEvent( new string[] {"*Picked Up " + _Safe.key_name + "!*"}, fromTrigger);
	
			while (showingText) {
				yield return null;
			}	
			
			PlayerKeys.instance.addKey(_Safe.key_name);
			_red.finishInteraction();
		}
		else {
			textEvent( new string[] {"Ashley:  The safe is locked.  I need to figure out the passphrase.."}, fromTrigger );
			
			while (showingText) {
				yield return null;
			}	
			
			_red.finishInteraction();	
			restartEvent(SAFE);
		}
	}
	
}

