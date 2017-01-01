using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Card : MonoBehaviour {

	const float MAXDELTA = 0.1f;
	const float MAXROT = 10f;

	public enum Cardcolor{red, green, yellow, blue, wild};

	public List<Sprite> card_sprite;
	public List<Sprite> card_hover_sprite;
	public List<Sprite> card_inactive_sprite;

	Cardcolor _color;
	bool _active;
	bool _hover;

	public Cardcolor color{ get{ return _color;} set {_color = value; UpdateSprite ();} }
	public bool active{ get { return _active; } set { _active = value; UpdateSprite ();} }
	public bool hover{ get { return _hover; } set { _hover = value; UpdateSprite ();} }
	public int number {
		get { return transform.GetChild (0).GetComponent<Number> ().number; }
		set { transform.GetChild (0).GetComponent<Number> ().number = value;}
	}

	Vector2 delta;
	Quaternion tilt;

	void Start () {
		delta = Random.insideUnitCircle * MAXDELTA;
		tilt = Quaternion.AngleAxis ((Random.value - .5f) * MAXROT, Vector3.forward);
		transform.position += (Vector3)delta;
		transform.rotation = tilt;
		UpdateSprite ();
	}

	void UpdateSprite (){
		if (active){
			if (hover){
				GetComponent<SpriteRenderer>().sprite = card_hover_sprite[(int)color];
			} else {
				GetComponent<SpriteRenderer>().sprite = card_sprite[(int)color];
			}
		} else {
			GetComponent<SpriteRenderer>().sprite = card_inactive_sprite[(int)color];
		}
	}
}
