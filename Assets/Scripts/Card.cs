using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class Card : MonoBehaviour {

	const float MAXDELTA = 0.1f;
	const float MAXROT = 10f;
	const float epsilon = 0.1f;

	class HoverState:Object
	{
		enum State {normal,hover,posthover};
		State state;

		static Dictionary<State,State> UpdateTransition = new Dictionary<State,State>()
		{{State.hover,State.posthover}, {State.posthover,State.normal}, {State.normal,State.normal}};

		static Dictionary<State,bool> QueryFunction = new Dictionary<State,bool>()
		{{State.hover,true}, {State.posthover,true}, {State.normal,false}};

		public HoverState(){
			state = State.normal;
		}

		public void Set (bool command)
		{
			if (command) {
				state = State.hover;
			} else {
				state = State.normal;
			}
		}
		public void Update ()
		{
			state = UpdateTransition [state];
		}
		public bool Query()
		{
			return QueryFunction [state];
		}
	}

	public List<Sprite> card_sprite;
	public List<Sprite> card_hover_sprite;
	public List<Sprite> card_inactive_sprite;

	HoverState _hover = new HoverState();
	bool _stagger;
	Vector3 _position;
	Vector3 stagger_delta;
	Quaternion stagger_rot;

	public float speed;
	public CardColor color;
	public bool active;
	public bool hover{ get { return _hover.Query(); } set { _hover.Set(value);} }
	public int number {
		get { return GetComponentInChildren<Number>().number; }
		set { GetComponentInChildren<Number> ().number = value;}
	}

	public bool stagger { get { return _stagger; } set { _stagger = value; } }

	public Vector3 position { get { return _position; } set { _position = value; } }


	void InitializeStagger()
	{
		stagger_delta = Random.insideUnitCircle * MAXDELTA;
		stagger_rot = Quaternion.AngleAxis ((Random.value - .5f) * MAXROT, Vector3.forward);
	}
		
	void Start () {
		InitializeStagger ();
	}

	void Update()
	{
		_hover.Update ();

		UpdateSprite ();
		UpdatePosition ();


	}

	void UpdatePosition()
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
	void UpdateSprite ()
	{
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
