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
        CreateCard(preparedCard);

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
            CreateCard(card);
        }
    }

    public void EditMode()
    {
        
    }

    public void CreateCard(GameObject obj)
    {
        float angle = 0;
        if (obj.tag == "Card") {
            angle = cards[0].eulerAngles.x;
        }
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        var clone = Instantiate(obj, objectPos, Quaternion.Euler(angle, 90, 90), cardsParent.transform);
        cards.Add((Transform)clone.transform);
    }

    public void RotateCard()
    {
        //To avoid rotation reset glitch
        cards[0].eulerAngles = new Vector3(cards[0].eulerAngles.x, 90, -90);

        if(Input.GetKey(KeyCode.A))
        {
            cards[0].eulerAngles += new Vector3(2.0f, 0, 0);
        } else if(Input.GetKey(KeyCode.D))
        {
            cards[0].eulerAngles += new Vector3(-2.0f, 0, 0);
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
