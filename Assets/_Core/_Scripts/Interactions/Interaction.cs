using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour
{
	public string[] _interactions = null;
	public int _currentInteraction = 0;
	
	public int _eventName = -1;
	
	PlayerControl _player = null;
	public SpriteText _text = null;
	
	bool _current = false;
	
	
	
	public RoomEvents _callback = null;
	// Use this for initialization
	void Start ()
	{
		
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		/*if (_current) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				if (_currentInteraction < _interactions.Length) {
					_text.Text = _interactions[_currentInteraction++];
				}
				else {
					_currentInteraction = 0;
					_current = false;
					_text.transform.parent.gameObject.SetActiveRecursively(false);
					_player.finishInteraction();
					
					if (_callback)
						_callback.finishText();
					
					if (collider) {
						collider.enabled = false;
						StartCoroutine(enableCollider());
					}
				}
			}
		}*/
	}

	public void startInteraction(PlayerControl control, SpriteText box, bool fromTrigger) {
		_player = control;
		_text = box;
		
		if (_interactions != null && _interactions.Length > 0 ) {
			_text.transform.parent.gameObject.SetActiveRecursively(true);
			_text.Text = _interactions[_currentInteraction++];
		}
		else if (_eventName != -1) {
			RoomEvents events = control.currentRoom;
			if (events.didEvent(_eventName) == false)
				events.doEvent(_eventName, fromTrigger);
			else 
				control.finishInteraction();
		}
		else {
			control.finishInteraction();	
		}
	}
	
	public IEnumerator enableCollider() {
		yield return new WaitForSeconds(1.0f);
		collider.enabled = true;
		Interactable ib = GetComponent<Interactable>();
		if (ib)
			ib._interacting = false;
	}
}

