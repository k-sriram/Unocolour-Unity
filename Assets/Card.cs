using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Card : MonoBehaviour {

	const float MAXDELTA = 0.1f;
	const float MAXROT = 10f;
	const float epsilon = 0.1f;

	public enum Cardcolor{red, green, yellow, blue, wild};

	public List<Sprite> card_sprite;
	public List<Sprite> card_hover_sprite;
	public List<Sprite> card_inactive_sprite;

	Cardcolor _color;
	bool _active;
	bool _hover;
	bool _stagger;
	bool stagger_uninitialized = true;
	Vector3 _position;
	Vector3 stagger_delta;
	Quaternion stagger_rot;

	public float speed;
	public Cardcolor color{ get{ return _color;} set {_color = value; UpdateSprite ();} }
	public bool active{ get { return _active; } set { _active = value; UpdateSprite ();} }
	public bool hover{ get { return _hover; } set { _hover = value; UpdateSprite ();} }
	public int number {
		get { return GetComponentInChildren<Number>().number; }
		set { GetComponentInChildren<Number> ().number = value;}
	}

	public bool stagger { get { return _stagger; }
		set { 
			_stagger = value;
			if (value && stagger_uninitialized) {
				InitializeStagger ();
			}
		}
	}

	public Vector3 position { get { return _position; } set { _position = value; } }

	void InitializeStagger()
	{
		stagger_delta = Random.insideUnitCircle * MAXDELTA;
		stagger_rot = Quaternion.AngleAxis ((Random.value - .5f) * MAXROT, Vector3.forward);
		stagger_uninitialized = false;
	}

	void Start () {
		UpdateSprite ();
	}

	void Update()
	{
		Vector3 target_position;
		if (stagger) {
			transform.rotation = stagger_rot;
			target_position = position + stagger_delta;
		} else {
			transform.rotation = Quaternion.identity;
			target_position = position;
		}
		transform.position = Vector3.MoveTowards (transform.position, target_position, speed * Time.deltaTime);
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
