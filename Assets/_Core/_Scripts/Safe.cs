using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Safe : Interaction
{
	public string key_name;
	Dictionary<string, bool> _words = new Dictionary<string, bool>();
	
	public string[] words = new string[] {};
	
	public void Start() {
		foreach (string word in words) {
			_words.Add(word.ToLower(), true);	
		}
	}
	
	public bool OpenSafe(string[] in_words) {
		
		if (in_words.Length != _words.Count)
			return false;
		
		foreach (string s in in_words) {
			bool val = false;
			_words.TryGetValue(s.ToLower(), out val);
			if (val == false) {
				return false;
			}
		}
		
		return true;
	}
	
	public void setOpenSprite() {
		Sprite s = GetComponent<Sprite>();
		s.DoAnim("SafeOpen");
		if (key_name != null && key_name.Length > 0)
			PlayerKeys.instance.addKey(key_name);
	}
}

