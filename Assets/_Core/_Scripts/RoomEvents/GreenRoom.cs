using UnityEngine;
using System.Collections;

public class GreenRoom : RoomEvents
{
	const int PORTRAIT = 0;
	const int VANITY = 1;
	const int SAFE = 2;
	
	public UITextField _puzzle1;
	public UITextField _puzzle2;
	
	public override void doEvent(int event_name, bool fromTrigger) {
		bool done = didEvent(event_name);
		if (event_name == PORTRAIT && done == false) {
			setEventFinished(event_name);
			StartCoroutine(viewPictures(fromTrigger));
		}
		else if (event_name == VANITY && done == false) {
			setEventFinished(event_name);
			StartCoroutine(viewVanity(fromTrigger));
		}
		else if (event_name == SAFE && done == false) {
			setEventFinished(event_name);
			StartCoroutine(openSafe(fromTrigger));
		}
		
		Debug.Log("Doing something..");
	}
	
	IEnumerator viewPictures(bool fromTrigger) {
		string[] messages = new string[] {"Mr. and Mrs. Green were married for 50 years..", "He outlived her by 10 years.. I'll never forget how much sadness I saw in his eyes..", 
											"And then he would look up at her picture and, just for just a moment..", "..close his eyes and smile.", "He told me it was the memory of kissing her lips."};
		textEvent(messages, fromTrigger);
		
		while (showingText) {
			yield return null;
		}	
		
		_red.finishInteraction();
		restartEvent(PORTRAIT);									
	}
	
	IEnumerator viewVanity(bool fromTrigger) {
		textEvent( new string[] {"Mr. Green would always tell me to 'never lose the moment'", "I never understood what that meant..", "He used to let me try on the makeup his wife left behind",
											"He would say.. 'Gracie would spend forever getting ready..", ".. but after she was done, waiting forever was always worth it." }, fromTrigger);
		
		while (showingText) {
			yield return null;
		}	
		
		_red.finishInteraction();
		restartEvent(VANITY);		
	}
	
	IEnumerator openSafe(bool fromTrigger) {
		if (_Safe.OpenSafe(new string[] {_puzzle1.Text, _puzzle2.Text})) {
			_Safe.setOpenSprite();
			
			textEvent( new string[] {"*Picked Up " + _Safe.key_name + "!*"} , fromTrigger);
	
			while (showingText) {
				yield return null;
			}	
			
			PlayerKeys.instance.addKey(_Safe.key_name);
			_red.finishInteraction();
		}
		else {
			textEvent( new string[] {"Ashley:  The safe is locked.  I need to figure out the passphrase.."} , fromTrigger);
			
			while (showingText) {
				yield return null;
			}	
			
			_red.finishInteraction();	
			restartEvent(SAFE);
		}
	}
}

