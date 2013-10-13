using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerKeys : MonoBehaviour
{
	private static volatile PlayerKeys _instance;
	
	public static PlayerKeys instance {
        get {
            if (_instance == null) {
                _instance =  FindObjectOfType(typeof (PlayerKeys)) as PlayerKeys;
            }
            
            if (_instance == null) {
                GameObject obj = new GameObject("PlayerKeys");
                _instance = obj.AddComponent(typeof (PlayerKeys)) as PlayerKeys;
            }
            
            return _instance;
        }
    }
	
	Dictionary<string, bool> _locks = new Dictionary<string, bool>();
	Dictionary<string, string[]> _lockMessages = new Dictionary<string, string[]>();
	
	public string[] debugkeys = new string[] {};
	
	// Use this for initialization
	void Start ()
	{
		_lockMessages["FeedCat"] = new string[] {"I should feed Kitty before I go.."};
		_lockMessages["MainToBlueDoor"] = new string[] {"I can't open this door.."};
		
		_lockMessages["Roof"] = new string[] {"KITTY!", "Someone opened the door.. and it locked when Kitty went through.", "Mom told me to never go outside.. but.. Kitty..", "I bet Mrs. Gold has a roof key in her room..", ".. where did she live?  I think it was the first floor."};
	}

	// Update is called once per frame
	void Update ()
	{
		foreach (string key in debugkeys) {
			_locks[key] = true;	
		}
	}
	
	public bool hasKey(string lockName) {
		bool has = false;
		_locks.TryGetValue(lockName, out has);
		return has;
	}
	
	public string[] lockedMessage(string lockName) {
		string[] m = new string[] {};
		_lockMessages.TryGetValue(lockName, out m);
		if (m == null) 
			m = new string[] {"You need a key to open this door."};
		return m;
	}
	
	public void addKey(string lockName) {
		_locks[lockName] = true;	
	}
	
	public void setLockedMessages(string[] messages, string keyname) {
		_lockMessages[keyname] = messages;
	}
}

