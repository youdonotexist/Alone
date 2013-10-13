using UnityEngine;
using System.Collections;

public class BluesRoom : RoomEvents
{
	const int BOOKS = 0;
	const int FAUX_SHELF = 1;
	const int SAFE = 2;
	const int LIVE = 3;
	
	public UITextField _puzzle1;
	public UITextField _puzzle2;
	
	public GameObject _FauxShelf;
	
	public override void doEvent(int event_name, bool fromTrigger) {
		bool done = didEvent(event_name);
			
		if (event_name == BOOKS && done == false) {
			setEventFinished(event_name);
			StartCoroutine(bookshelf(fromTrigger));
		} 
		else if (event_name == FAUX_SHELF && done == false) {
			setEventFinished(event_name);
			StartCoroutine(fauxShelf(fromTrigger));
		}
		else if (event_name == SAFE && done == false) {
			setEventFinished(event_name);
			StartCoroutine(openSafe(fromTrigger));
		}
		else if (event_name == LIVE && done == false) {
			setEventFinished(event_name);
			StartCoroutine(live(fromTrigger));
		}
		else {
			_red.finishInteraction();	
		}
	}
	
	IEnumerator bookshelf(bool fromTrigger) {
		string[] messages = new string[] {"Ashley: Mr. Blue loved his books.. ", "Ashley:  I would walk by his apartment in the afternoon and see him in his study buried under a stack of dusty, hardcover books..", 
											"Ashley: When my parents weren't looking, he would slip me borrow a book on philosphy or science ..", ".. patting my on the head and saying 'Children's books are all rubbish'",
												"I didn't understand much of the books he gave me, but Mr. Blue always said..", "'Knowing starts with knowledge", ".. whatever that means." };
		textEvent(messages, fromTrigger);
		
		while (showingText) {
			yield return null;
		}
		
		_red.finishInteraction();
		restartEvent(BOOKS);
	}
	
	IEnumerator fauxShelf(bool fromTrigger) {
		string[] messages = new string[] {"Ashley: There's something weird with this shelf.."};
		textEvent(messages, fromTrigger);
		
		while (showingText) {
			yield return null;
		}
		
		messages = new string[] {"Ashley: Mr. Blue was tricky!  He hid his safe behind a fake bookshelf!"};
		textEvent(messages, fromTrigger);
		
		while (showingText) {
			yield return null;
		}	
		
		Vector3 safePos = _Safe.transform.position;
		safePos.y = _FauxShelf.transform.position.y;
		_Safe.transform.position = safePos;
		Destroy(_FauxShelf);
		_Safe.gameObject.active = true;
		
		_red.finishInteraction();
	}
	
	IEnumerator live(bool fromTrigger) {
		string[] messages = new string[] {"Ashley: Mr. Blue seemed super smart..", "So I never understood why he would kill animals for fun.", "I did a book report on the Native Americans..", "When they killed, they used every part of the animal.", 
											"And they felt really bad for having to kill the animal in the first place!", "I showed Mr. Blue my book report and he looked me right in the eye and said..", 
												"'To live is to kill.  Each moment we live, we are destroying and creating an infinite amount of life and death'", "'Like the ocean against the shore, we let live and we let die'"};
		textEvent(messages, fromTrigger);
		
		while (showingText) {
			yield return null;
		}
		
		_red.finishInteraction();
		restartEvent(LIVE);
	}
	
	IEnumerator openSafe(bool fromTrigger) {
		if (_Safe.OpenSafe(new string[] {_puzzle1.Text, _puzzle2.Text})) {
			_Safe.setOpenSprite();
			
			textEvent(new string[] {"*Picked Up " + _Safe.key_name + "!*"}, fromTrigger);
	
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

