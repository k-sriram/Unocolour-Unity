using UnityEngine;
using System.Collections.Generic;

public class CardStack : MonoBehaviour
{

    readonly static Vector3 CardThickness = new Vector3(0f, 0f, -0.02f);

    LinkedList<GameObject> cardList = new LinkedList<GameObject>();

    public GameObject cardObject;

    bool _active = false;
    bool _stagger = false;
    bool _numbered = false;

    public bool active { get { return _active; } set { _active = value; if (number > 0) LastCard.active = value; } }
    public bool stagger
    {
        get { return _stagger; }
        set
        {
            _stagger = value;
            if (number > 0)
            {
                foreach (var i in cardList)
                {
                    i.GetComponent<Card>().stagger = value;
                }
            }
        }
    }
    public bool numbered
    {
        get { return _numbered; }
        set
        {
            _numbered = value;
            if (number > 0)
            {
                if (value)
                {
                    LastCard.number = number;
                }
                else
                {
                    LastCard.number = 0;
                }
            }
        }
    }
    public int number { get { return cardList.Count; } }
    public CardColor topCardColor
    {
        get
        {
            if (number > 0)
                return LastCard.color;
            else
                return CardColor.none;
        }
    }

    public void Hover()
    {
        if (number > 0)
        {
            LastCard.hover = true;
        }
    }

    public void Click(bool value = true)
    {
        if (number > 0)
        {
            LastCard.click = value;
        }
    }

    public void SetProperties(bool activeVal, bool staggerVal, bool numberedVal)
    {
        active = activeVal;
        stagger = staggerVal;
        numbered = numberedVal;
    }

    public void ReceiveCard(GameObject receivedCard)
    {
        if (number > 0)
        {
            LastCard.active = false;
            LastCard.click = false;
            LastCard.number = 0;
        }

        cardList.AddLast(receivedCard);
        Card card = receivedCard.GetComponent<Card>();
        if (numbered)
        {
            card.number = number;
        }
        else
        {
            card.number = 0;
        }
        card.position = transform.position + CardThickness * (number - 1);
        card.stagger = stagger;
        card.active = active;
        card.click = false;
    }


    public GameObject SendCard()
    {
        GameObject cardToSend = cardList.Last.Value;
        cardList.RemoveLast();
        if (number > 0)
        {
            Card card = LastCard;
            if (numbered)
            {
                card.number = number;
            }
            else
            {
                card.number = 0;
            }
            card.active = active;
        }
        return cardToSend;
    }

    public void Shuffle()
    {
        int n = number;
        LinkedListNode<GameObject> node = cardList.First;
        for (int i = 0; i <= n - 2; i++)
        {
            int j = Random.Range(0, n - i);
            LinkedListNode<GameObject> other = ElementAfter(node, j);
            GameObject tempswap = node.Value;
            node.Value = other.Value;
            other.Value = tempswap;
            node = node.Next;
        }
        node = cardList.First;
        for (int i = 0; i < n; i++)
        {
            node.Value.GetComponent<Card>().position = transform.position + CardThickness * i;
            if (numbered)
            {
                node.Value.GetComponent<Card>().number = i + 1;
            }
            node = node.Next;
        }
    }


    // Private functions
    LinkedListNode<GameObject> ElementAfter(LinkedListNode<GameObject> node, int index)
    {
        if (index == 0)
            return node;
        else
            return ElementAfter(node.Next, index - 1);

    }


    Card LastCard
    {
        get 
        {
            return cardList.Last.Value.GetComponent<Card>();
        }
    }



    // Use this for initialization
    //	void Start () {
    //		numbered = true;
    //		stagger = true;
    //		GameObject foo = Instantiate (cardObject) as GameObject;
    //		ReceiveCard (foo);
    //		foo = Instantiate (cardObject) as GameObject;
    //		ReceiveCard (foo);
    //		shuffle ();
    //	}

    // Update is called once per frame
    void Update()
    {

    }
}
