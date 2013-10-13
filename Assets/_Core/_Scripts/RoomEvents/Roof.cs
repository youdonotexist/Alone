using UnityEngine;
using System.Collections;

public class Roof : RoomEvents
{
	const int END = 0;
	
	public GameObject walkTo;
	public Cat _Cat;
	public Interactable _Janitor;
	
	public GameObject _Sky;
	public GameObject _SkyWhite;
	public GameObject _MountKitty;
	public GameObject _JanAppear;
	
	public GameObject _DeadRed;
	
	public override void doEvent(int event_name, bool fromTrigger) {
		bool done = didEvent(event_name);
			
		if (event_name == END && done == false) {
			setEventFinished(event_name);
			StartCoroutine(end(fromTrigger));
		} 
	}
	
	IEnumerator end(bool fromTrigger) {
		_Cat.setFacing(new Vector2(1.0f, 0.0f));
		_red.setFacing(new Vector2(-1.0f, 0.0f));
		_SkyWhite.active = false;
		
		string[] messages = new string[] {"Ashley: Kitty?", "Kitty:  Muuuurrrough...", "Kitty: ...", "Kitty: ......", "Kitty:  MEEERRROROOOWWWW!!", "Ashley:  Kitty, it's me!"};
		textEvent(messages, fromTrigger);
		
		while (showingText) {
			yield return null;
		}	
		
		_Cat._state = Interactable.STATE.TO_POINT;
		_Cat.walkSpeed = 2.0f;
		_Cat.goToPoint = new Vector2 (_Cat.transform.position.x + 20.0f, _Cat.transform.position.y);
		//147 0, 0, 147
		float time = 0.0f;
		
		_red.startInteraction();
		
		string name = "_MainColor";
		
		while (time < 5.0f) {
			if (time > 0.5f) {
				_Sky.active = false;
				_SkyWhite.active = true;
			}
			
			time += Time.deltaTime;
			
			yield return null;	
		}
		
		_DeadRed.active = true;
		_red.gameObject.active = false;
		_Sky.active = true;
		_SkyWhite.active = false;
		
		_Janitor.gameObject.active = true;
		_Janitor.transform.position = _JanAppear.transform.position;
		_Cat.gameObject.active = false;
		
		time = 0.0f;
		while (time < 2.0f) {
			time += Time.deltaTime;
			
			yield return null;	
		}	
		
		messages = new string[] {"Man: I.. I.. am alone.. END"};
		textEvent(messages, fromTrigger);
		
		time = 0.0f;
		while (time < 2.0f) {
			time += Time.deltaTime;
			
			yield return null;	
		}	
		
		
	}
}

