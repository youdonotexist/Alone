using UnityEngine;
using System.Collections;

public class BasementRoom : RoomEvents
{
	const int SAFE = 0;
	
	public Sprite fakesafe = null;
	
	public override void doEvent(int event_name, bool fromTrigger) {
		bool done = didEvent(event_name);
			
		if (event_name == SAFE && done == false) {
			setEventFinished(event_name);
			StartCoroutine(openSafe(fromTrigger));
		} 
	}
	
	IEnumerator openSafe(bool fromTrigger) {
		string[] messages = new string[] {"Ashley: Not another safe.."};
		textEvent(messages, fromTrigger);
		
		while (showingText) {
			yield return null;
		}
		
		fakesafe.DoAnim("SafeOpen");
		
		messages = new string[] {"Ashley: It's unlocked!", "*Picked Up RoofKey!*", "Finally!  Kitty here I come!"};
		textEvent(messages, fromTrigger);
		
		while (showingText) {
			yield return null;
		}
		
		PlayerKeys.instance.addKey("Roof");
		PlayerKeys.instance.addKey("Basement");
		
		_red.finishInteraction();
	}
}
