using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	Sprite sprite = null;
	Vector2 walkDir = new Vector2(0.0f, 0.0f);
	
	const float _focusDist = 0.25f;
	const float walkSpeed = 10.0f;
	
	public GameObject _focus = null;
	public GameObject _textBox = null;
	public SpriteText _spriteText = null;
	
	public bool _interacting = false;
	
	public RoomEvents currentRoom = null;
	public GameObject _heldObject = null;
	
	public Interaction _currentInteraction;
	
	// Use this for initialization
	void Start () {
		sprite = GetComponent<Sprite>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_interacting == false) {
			normal();
		}
		else {
			interacting();	
		}
	}
		
	void normal() {
		// Movement
		Vector2 moveDir = new Vector2(0.0f, 0.0f);
		
		if (Input.GetKey(KeyCode.A)) {
			moveDir.x = -1.0f;
		} else if (Input.GetKey(KeyCode.D)) {
			moveDir.x = 1.0f;
		}
		else {
			moveDir.x = 0.0f;
		}
		
		if (Input.GetKey(KeyCode.W)) {
			moveDir.y = 1.0f;
		} else if (Input.GetKey(KeyCode.S)) {
			moveDir.y = -1.0f;
		}
		else {
			moveDir.y = 0.0f;	
		}
		
		bool moveHorizontal = Mathf.Abs(moveDir.x) > 0.0f;
		bool moveVertical = Mathf.Abs(moveDir.y) > 0.0f;
		
		if (moveHorizontal && moveVertical) {
			if (moveDir.y > 0.0f)
				sprite.DoAnim("Walk Up");
			else 
				sprite.DoAnim("Walk Down");
				
			gameObject.rigidbody.AddForce(moveDir.x * walkSpeed, 0.0f, moveDir.y * walkSpeed, ForceMode.Force);
			
			_focus.transform.position = transform.position + new Vector3(walkDir.x * _focusDist, transform.position.y, walkDir.y * _focusDist);
		}
		else {
			if (moveHorizontal) {
				if (moveDir.x > 0.0f)
					sprite.DoAnim("Walk Right");
				else 
					sprite.DoAnim("Walk Left");
				
				gameObject.rigidbody.AddForce(moveDir.x * walkSpeed, 0.0f, 0.0f, ForceMode.Force);
				
				_focus.transform.position = transform.position + new Vector3(walkDir.x* _focusDist, transform.position.y, walkDir.y* _focusDist);
				
			}
			
			if (moveVertical) {
				if (moveDir.y > 0.0f)
					sprite.DoAnim("Walk Up");
				else 
					sprite.DoAnim("Walk Down");
				
				gameObject.rigidbody.AddForce(0.0f, 0.0f, moveDir.y * walkSpeed, ForceMode.Force);
				
				_focus.transform.position = transform.position + new Vector3(walkDir.x * _focusDist, transform.position.y, walkDir.y * _focusDist);
			}
			
			if (moveVertical == false && moveHorizontal == false) {
				setFacing();
			}
		}
		
		walkDir = moveDir;
		
		//Interaction
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			SphereCollider collider = (SphereCollider) _focus.collider;
			Collider[] objs = Physics.OverlapSphere(_focus.transform.position, collider.radius, 1 << 8);
			if (objs.Length > 0) {
				GameObject obj = objs[0].gameObject;
				Interaction i = obj.GetComponent<Interaction>();
				Interactable ib = obj.GetComponent<Interactable>();
				Door d = obj.GetComponent<Door>();
				
				if (i != null && d == null) {
					if (ib != null)
						ib._interacting = true;
					setFacing();
					showInteraction(i, false);	
				}
			}
		}
	}
		
	void interacting() {
		if (_currentInteraction != null) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				if (_currentInteraction._currentInteraction < _currentInteraction._interactions.Length) {
					_currentInteraction._text.Text = _currentInteraction._interactions[_currentInteraction._currentInteraction++];
				}
				else {
					_currentInteraction._currentInteraction = 0;
					_currentInteraction._text.transform.parent.gameObject.SetActiveRecursively(false);
					
					if (_currentInteraction._callback)
						_currentInteraction._callback.finishText();
					
					if (_currentInteraction.collider) {
						_currentInteraction.collider.enabled = false;
						StartCoroutine(_currentInteraction.enableCollider());
					}
					
					finishInteraction();
				}
			}
		}
	}
	
	public void startInteraction() {
		_interacting = true;	
	}
	
	public void showInteraction(Interaction i, bool fromTrigger) {
		_interacting = true;
		_currentInteraction = i;
		i.startInteraction(this, _spriteText, fromTrigger);
	}
	
	public void finishInteraction() {
		_interacting = false;
		if (_currentInteraction) {
			_currentInteraction._currentInteraction = 0;
			_currentInteraction._interactions = null;
			_currentInteraction = null;
		}
	}
	
	public void setFacing(Vector2 face) {
		walkDir = face;
		setFacing();
	}
	
	public Vector2 getFacing() {
		return walkDir;	
	}
	
	public void setFacing() {
		if (walkDir.x > 0.0f) {
			sprite.DoAnim("Face Right");
			_focus.transform.position = transform.position + new Vector3(walkDir.x * _focusDist, transform.position.y, walkDir.y * _focusDist);
		}
		else if (walkDir.x < 0.0f) {
			sprite.DoAnim("Face Left");	
			_focus.transform.position = transform.position + new Vector3(walkDir.x * _focusDist, transform.position.y, walkDir.y * _focusDist);
		}
		else if (walkDir.y > 0.0f) {
			sprite.DoAnim("Face Up");	
			_focus.transform.position = transform.position + new Vector3(walkDir.x * _focusDist, transform.position.y, walkDir.y * _focusDist);
		}
		else if (walkDir.y < 0.0f) {
			sprite.DoAnim("Face Down");	
			_focus.transform.position = transform.position + new Vector3(walkDir.x * _focusDist, transform.position.y, walkDir.y * _focusDist);
		}	
	}
}
