using UnityEngine;
using System.Collections.Generic;

public class CardStack : MonoBehaviour {

	public GameObject cardObject;

	int _size;

	bool _active = false;
	bool _hover = false;

	public bool active{ get{ return _active; } set{_active = value; if (size>0) LastCard ().active = value;} }
	public bool hover{ get{ return _hover; } set{_hover = value; if (size>0) LastCard ().hover = value;} }

	public int size { get { return _size; } }

	public void AddCard(Card.Cardcolor color, int number = 0)
	{
		if (number == 0) {
			number = size + 1;
		}

		if (size > 0) {
			LastCard ().active = false;
			LastCard ().hover = false;
		}

		GameObject newCardObject;
		Card newCard;
		newCardObject = Instantiate (cardObject, transform.position + new Vector3 (0.0f, 0.0f, -0.2f * size), Quaternion.identity,transform) as GameObject;
		newCard = newCardObject.GetComponent<Card> ();
		newCard.color = color;
		newCard.number = number;
		newCard.active = active;
		newCard.hover = hover;
		_size++;
	}
		
	public void RemoveCard(int count=1)
	{
		for (int i = 0; i < count; i++) {
			Destroy (LastCard().gameObject);
			_size--;
		}
		active = active;
		hover = hover;

	}
	
	Card LastCard()
	{
		return transform.GetChild (transform.childCount - 1).GetComponent<Card>();
	}
		


	// Use this for initialization
	void Start () {
		AddCard (Card.Cardcolor.green);
		AddCard (Card.Cardcolor.red);
		RemoveCard ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
