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
            if (value > 99)
                _number = 99;
            if (number > 0) {
				GetComponent<SpriteRenderer> ().sprite = numberSprite [number];
				GetComponent<SpriteRenderer> ().enabled = true;
			} else if (number == 0) {
				GetComponent<SpriteRenderer> ().enabled = false;
			}
		}
	}
}