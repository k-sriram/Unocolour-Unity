using UnityEngine;
using System.Collections.Generic;

public class CardStack : MonoBehaviour {

	LinkedList<GameObject> cardList = new LinkedList<GameObject>();

	public GameObject cardObject;

	bool _active = false;
	bool _hover = false;
	bool _stagger = false;
	bool _numbered = false;

	public bool active{ get{ return _active; } set{_active = value; if (number>0) LastCard ().active = value;} }
	public bool hover{ get{ return _hover; } set{_hover = value; if (number>0) LastCard ().hover = value;} }
	public bool stagger{ get{ return _stagger; } set{_stagger = value; if (number > 0) LastCard ().stagger = value;} }
	public bool numbered {
		get{ return _numbered; }
		set {
			_numbered = value;
			if (number > 0) {
				if (value) {
					LastCard ().number = number;
				} else {
					LastCard ().number = 0;
				}
			}
		}
	}
	public int number{get{return cardList.Count;}}

	public void ReceiveCard(GameObject receivedCard)
	{
		cardList.AddLast (receivedCard);
		Card card = receivedCard.GetComponent<Card> ();
		if (numbered) {
			card.number = number;
		} else {
			card.number = 0;
		}
		card.position = transform.position + new Vector3(0f,0f,-0.1f) * (number - 1);
		card.stagger = stagger;
		card.active = active;
		card.hover = hover;
	}


	public GameObject SendCard()
	{
		GameObject cardToSend = cardList.Last.Value;
		cardList.RemoveLast ();
		Card card = cardList.Last.Value.GetComponent<Card> ();
		if (numbered) {
			card.number = number;
		} else {
			card.number = 0;
		}
		card.active = active;
		card.hover = hover;
		return cardToSend;
	}

	public Card.Cardcolor GetTopCardColor()
	{
		return LastCard ().color;
	}

	public void shuffle()
	{
		int n = number;
		LinkedListNode<GameObject> node = cardList.First;
		for (int i = 0; i <= n - 2; i++) {
			int j = Random.Range (0, n-i);
			LinkedListNode<GameObject> other = ElementAfter (node, j);
			GameObject tempswap = node.Value;
			node.Value = other.Value;
			other.Value = tempswap;
			node = node.Next;
		}
		node = cardList.First;
		for (int i = 0; i < n; i++) {
			node.Value.GetComponent<Card> ().position = transform.position + new Vector3 (0f, 0f, -0.1f) * i;
			node.Value.GetComponent<Card> ().number = i + 1;
			node = node.Next;
		}
	}

	LinkedListNode<GameObject> ElementAfter(LinkedListNode<GameObject> node,int index)
	{
		if (index == 0)
			return node;
		else
			return ElementAfter (node.Next, index - 1);
	
	}

	
	Card LastCard()
	{
		return cardList.Last.Value.GetComponent<Card>();
	}
		


	// Use this for initialization
	void Start () {
		numbered = true;
		stagger = true;
		GameObject foo = Instantiate (cardObject) as GameObject;
		ReceiveCard (foo);
		foo = Instantiate (cardObject) as GameObject;
		ReceiveCard (foo);
		shuffle ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
