using UnityEngine;
using System.Collections;

public class TurquoisRoom : RoomEvents
{
	const int BREATH = 0;
	const int LOVE = 1;
	const int SAFE = 2;
	
	public Sprite turquoise;
	
	public UITextField _puzzle1;
	public UITextField _puzzle2;
	
	public override void doEvent(int event_name, bool fromTrigger) {
		bool done = didEvent(event_name);
			
		if (event_name == BREATH && done == false) {
			setEventFinished(event_name);
			StartCoroutine(breath(fromTrigger));
		} 
		else if (event_name == LOVE && done == false) {
			setEventFinished(event_name);
			StartCoroutine(love(fromTrigger));
		}
		else if (event_name == SAFE && done == false) {
			setEventFinished(event_name);
			StartCoroutine(openSafe(fromTrigger));
		}
	}
	
	IEnumerator breath(bool fromTrigger) {
		bool isDead = PlayerKeys.instance.hasKey("DeadTurquoise");
		if (isDead == false) {
			string[] messages = new string[] {"Ashley: Oh, God!  He's still breathing!", "Ashley: Mr. Turquoise.. Mr. Turquoise.. can you hear me?", "Mr. Turquoise: ...", "Mr. Turquoise: ......", "Mr. Turquoise: *last breath*"};
			textEvent(messages, fromTrigger);
			
			while (showingText) {
				yield return null;
			}
			
			turquoise.DoAnim("Dead");
			PlayerKeys.instance.addKey("DeadTurquoise");
			
			messages = new string[] {"Ashley: Mr. Turquoise..."};
			textEvent(messages, fromTrigger);
			
			while (showingText) {
				yield return null;
			}
		}
		else {
			string[] messages = new string[] {"Ashley: He's.. gone.."};
			textEvent(messages, fromTrigger);
			
			while (showingText) {
				yield return null;
			}
		}
		
		_red.finishInteraction();
		restartEvent(BREATH);
	}
	
	IEnumerator love(bool fromTrigger) {
		string[] messages = new string[] {"Ashley: Mr. Turquoise and my father were once good frields. ", "Ashley: Two years ago, we lived in apartment unit 5, just next to Mr. Turquoise", "Ashley: Sometimes, when I did something bad, or when he was drunk..",
											".. my father would beat me and lock me in the service elevator in our apartment.", "Ashley: Mr. Turquoise would sneak over with the key and let me out.", 
												"Ashley: He would have me over for tea and tell me 'Love is not a fist.  Love is not a word thrown like a glass jar.", "Love is like a fire.  Your father needs to understand that.",
													"Mr. Turquoise..."};
											
		textEvent(messages, fromTrigger);
		
		while (showingText) {
			yield return null;
		}
		
		_red.finishInteraction();
		restartEvent(LOVE);
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

