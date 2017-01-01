using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Number : MonoBehaviour {

	public List<Sprite> numberSprite;
	int _number;

	public int number{ 
		get{ return _number; }
		set{ _number = value; GetComponent<SpriteRenderer> ().sprite = numberSprite [_number - 1];}
	}
}
