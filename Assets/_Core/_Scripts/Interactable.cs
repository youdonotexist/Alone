using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
	public bool _interacting = false;
	
	protected Sprite sprite;
	
	Vector2 wanderPoint = Vector2.zero;
	Vector2 wanderDir = new Vector2(0.0f, 0.0f);
	
	float _lastWander = 0.0f;
	Vector2 walkDir = Vector3.zero;
	public float walkSpeed = 3.0f;
	
	public Vector2 goToPoint;
	
	public enum STATE {
		WANDER,
		TO_POINT,
		STOP,
		FOOD_PACE,
		FOLLOW_PATH
	};
	
	GameObject[] followPath = null;
	int currentPathNode = -1;
	
	public STATE _state;
	
	void Start ()
	{
		//_state = STATE.FOOD_PACE;
		sprite = GetComponent<Sprite>();
	}

	// Update is called once per frame
	public void Update ()
	{
		if (_interacting == false) {
			updateState();
			updateMovement();
		}
	}
	
	public void updateState() {
		if (_state == STATE.WANDER) {
			wanderPoint = pickPoint();
			_state = STATE.TO_POINT;
		}
		
		if (_state == STATE.TO_POINT) {
			Vector3 catPos = transform.position;
			Vector3 nodePos = new Vector3(goToPoint.x, catPos.y, goToPoint.y);
			
			float d = Vector3.Distance(	nodePos, catPos );
			if (d > 0.05f) {
				
				Vector3 diff = nodePos - catPos;
				Vector3 norm = diff.normalized;
				wanderDir = new Vector2(norm.x, norm.z) * 5.0f;
			}
		}
		
		if (_state == STATE.FOOD_PACE) {
			if (_lastWander > 0.5f) {
				if (wanderDir.y > 0.0f)
					wanderDir = new Vector2(0.0f, -1.0f);
				else 
					wanderDir = new Vector2(0.0f, 1.0f);
				_lastWander = 0.0f;
			}
			else {
				_lastWander += Time.deltaTime;
			}
		}
		
		if (_state == Cat.STATE.STOP) {
			wanderDir = Vector2.zero;
		}
		
		if (_state == Cat.STATE.FOLLOW_PATH) {
			//If we're not on the path, yet
			if (currentPathNode == -1) {
				if (followPath != null && followPath.Length > 0) {
					currentPathNode = 0;
				}
			}
			else if (currentPathNode < followPath.Length) {
				Vector3 nPos = followPath[currentPathNode].transform.position;
				Vector3 catPos = transform.position;
				Vector3 nodePos = new Vector3(nPos.x, catPos.y, nPos.z);
				
				float d = Vector3.Distance(	nodePos, catPos );
				if (d > 0.05f) {
					
					Vector3 diff = nodePos - catPos;
					Vector3 norm = diff.normalized;
					wanderDir = new Vector2(norm.x, norm.z) * 5.0f;
					
				}
				else {
					currentPathNode++;
					if (currentPathNode < followPath.Length) {
						nPos = followPath[currentPathNode].transform.position;
						nodePos = new Vector3(nPos.x, catPos.y, nPos.z);
				
						float d2 = Vector3.Distance( nodePos, transform.position );
						if (d2 > 0.05f) {
							Vector3 diff = nodePos - catPos;
							Vector3 norm = diff.normalized;
							wanderDir = new Vector2(norm.x, norm.z) * 5.0f;
						}	
					}
				}
				
				//Debug.Log("Current Node: " + currentPathNode.ToString());
			}
		}
	}
	
	public void FollowPath(GameObject[] path) {
		followPath = path;
		currentPathNode = -1;
	}
	
	public void updateMovement() {
		Vector2 moveDir = wanderDir;
		
		//Debug.Log("Wander Dir: " + wanderDir.ToString());
		
		bool moveHorizontal = Mathf.Abs(moveDir.x) > 0.0f;
		bool moveVertical = Mathf.Abs(moveDir.y) > 0.0f;
		
		if (moveHorizontal && moveVertical) {
			bool upLarger = Mathf.Abs(moveDir.y) >= Mathf.Abs(moveDir.x);
			
			if (upLarger) {
				if (moveDir.y > 0.0f) {
					sprite.DoAnim("Walk Up");
				}
				else { 
					sprite.DoAnim("Walk Down");
				}
			}
			else {
				if (moveDir.x > 0.0f) {
					sprite.DoAnim("Walk Right");
				}
				else { 
					sprite.DoAnim("Walk Left");
				}
			}
				
			gameObject.rigidbody.AddForce(moveDir.x * walkSpeed, 0.0f, moveDir.y * walkSpeed, ForceMode.Force);
		}
		else {
			if (moveHorizontal) {
				if (moveDir.x > 0.0f)
					sprite.DoAnim("Walk Right");
				else 
					sprite.DoAnim("Walk Left");
				
				gameObject.rigidbody.AddForce(moveDir.x * walkSpeed, 0.0f, 0.0f, ForceMode.Force);
			}
			
			if (moveVertical) {
				if (moveDir.y > 0.0f)
					sprite.DoAnim("Walk Up");
				else 
					sprite.DoAnim("Walk Down");
				
				gameObject.rigidbody.AddForce(0.0f, 0.0f, moveDir.y * walkSpeed, ForceMode.Force);
			}
			
			if (moveVertical == false && moveHorizontal == false) {
				setFacing();
			}
		}
		
		walkDir = moveDir;
	}
	
	Vector2 pickPoint() {
		float r1 = (Random.Range(0, 2) - 1);
		float r2 = (Random.Range(0, 2) - 1);
		float up = 0.0f;
		float left = 0.0f;
		
		if (r1 > 0) {
			up = 1.0f;	
		}
		else if (r1 < 0) {
			up = -1.0f;	
		}
		else {
			up = 0.0f;	
		}
		
		if (r2 > 0) {
			left = 1.0f;	
		}
		else if (r2 < 0) {
			left = -1.0f;	
		}
		else {
			left = 0.0f;	
		}
		
		return new Vector2(up, left).normalized;
	}
	
	public void setFacing(Vector2 face) {
		walkDir = face;
		setFacing();
	}
	
	public void setFacing() {
		if (walkDir.x > 0.0f) {
			sprite.DoAnim("Face Right");
		}
		else if (walkDir.x < 0.0f) {
			sprite.DoAnim("Face Left");	
		}
		else if (walkDir.y > 0.0f) {
			sprite.DoAnim("Face Up");	
		}
		else if (walkDir.y < 0.0f) {
			sprite.DoAnim("Face Down");
		}	
	}
}

