using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomEvents : MonoBehaviour
{
	protected Dictionary<int, bool> _events = new Dictionary<int, bool>();
	protected bool showingText = false;
	
	public Safe _Safe;
	public PlayerControl _red;
	
	protected Interaction _interaction;
	// Use this for initialization
	public virtual void Start ()
	{
		_interaction = GetComponent<Interaction>();
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public virtual void doEvent(int event_name, bool fromTrigger) {
	
	}
	
	public void setEventFinished(int event_name) {
		Debug.Log("Event Finished: " + this.GetType().ToString() + " " + event_name.ToString());
		_events[event_name] = true;	
	}
	
	public void restartEvent(int event_name) {
		Debug.Log("Event Restarted: " + this.GetType().ToString() + " " + event_name.ToString());
		_events[event_name] = false;	
	}
	
	public bool didEvent(int event_name) {
		bool done = false;
		_events.TryGetValue(event_name, out done);
		return done;
	}
	
	public void startText() {
		showingText = true;	
	}
	
	public void finishText() {
		showingText = false;
	}
	
	public void TextDelegate(IKeyFocusable del) {
		del.Commit();
	}
	
	public void textEvent(string[] messages, bool fromTrigger) {
		if (_red != null) {
			_interaction._interactions = messages;
			_interaction._currentInteraction = 0;
			_interaction._callback = this;

			_red.showInteraction(_interaction, fromTrigger);

			startText();	
		}
	}
	
	public void CleanupEvent() {
		_interaction._interactions = null;
		_interaction._currentInteraction = 0;	
	}
}

