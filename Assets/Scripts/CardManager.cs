using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {

    public GameObject card;
    public GameObject preparedCard;
    public GameObject cardsParent;
    private List<Transform> cards;
    private Vector3 mousePos;

    public bool creationMode = true;

	void Start () {
        cards = new List<Transform>();
        CreateCard(preparedCard, new Vector3(0,0,0));

	}
	
	void Update () {
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            creationMode = !creationMode;
        }
        if(creationMode) {
            CreationMode();
        } else {
            EditMode(); 
        }
        cards[0].gameObject.SetActive(creationMode);
    }

    public void CreationMode()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 5.0f;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        cards[0].position = new Vector3(objectPos.x, objectPos.y, objectPos.z);

        RotateCard();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateCard(card, objectPos);
        }
    }

    public void EditMode()
    {
        
    }

    public void CreateCard(GameObject obj, Vector3 objectPosition)
    {
        float angle = 0;
        if (obj.tag == "Card") {
            angle = cards[0].eulerAngles.x;
        }
        var clone = Instantiate(obj, objectPosition, Quaternion.Euler(angle, 90, 90), cardsParent.transform);
        cards.Add((Transform)clone.transform);
    }

    public void RotateCard()
    {
        if(Input.GetKey(KeyCode.A))
        {
            cards[0].Rotate(0, 2.0f, 0);
        } else if(Input.GetKey(KeyCode.D))
        {
            cards[0].Rotate(0, -2.0f, 0);
            
        }
    }

    public void SelectCard()
    {

    }

    public void MoveCard()
    {

    }

    public void DeleteCard(GameObject card)
    {
        Destroy(card);
    }

}
