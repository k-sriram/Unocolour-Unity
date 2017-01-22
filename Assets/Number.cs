using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Number : MonoBehaviour {

	public List<Sprite> numberSprite;
	int _number;

	public int number{ 
		get{ return _number; }
		set{
			_number = value;
			if (value > 0) {
				GetComponent<SpriteRenderer> ().sprite = numberSprite [_number - 1];
				GetComponent<SpriteRenderer> ().enabled = true;
			} else if (value == 0) {
				GetComponent<SpriteRenderer> ().enabled = false;
			}
		}
	}
}
