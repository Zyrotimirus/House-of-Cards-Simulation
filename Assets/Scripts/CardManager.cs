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
    public bool stopPhysics = false;

	void Start () {
        cards = new List<Transform>();
        CreateCard(preparedCard, new Vector3(0,0,0));

	}
	
	void Update () {

        ModeChange();
        CameraFree();

        ActivateWind();

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            stopPhysics = !stopPhysics;
            StopPhysics(stopPhysics);
        }

    }

    public void ModeChange()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            creationMode = !creationMode;
        }
        if (creationMode) {
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

    public void CameraFree()
    {
        if (Input.GetMouseButton(1))
        {
            if (Input.GetKey(KeyCode.W))
            {
                Camera.main.transform.localPosition += new Vector3(0, 0, 0.05f);
            }
            if (Input.GetKey(KeyCode.S))
            {
                Camera.main.transform.localPosition += new Vector3(0, 0, -0.05f);
            }
            if (Input.GetKey(KeyCode.A))
            {
                Camera.main.transform.localPosition += new Vector3(-0.05f, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Camera.main.transform.localPosition += new Vector3(0.05f, 0, 0);
            }
            mousePos = Input.mousePosition;
            mousePos.z = 0.1f;
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);    

            Camera.main.transform.LookAt(objectPos);
        }
    }

    public void CreateCard(GameObject obj, Vector3 objectPosition)
    {
        Vector3 angle = new Vector3(0, 90, 90);
        if (obj.tag == "Card") {
            angle = new Vector3(cards[0].eulerAngles.x, cards[0].eulerAngles.y, cards[0].eulerAngles.z);
        }
        var clone = Instantiate(obj, objectPosition, Quaternion.Euler(angle), cardsParent.transform);
        
        if(clone.tag == "Card" && stopPhysics)
        {
            clone.GetComponent<Rigidbody>().isKinematic = stopPhysics;
        }
        cards.Add((Transform)clone.transform);
    }

    public void RotateCard()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            cards[0].Rotate(0, 2.0f, 0);
        } else if(Input.GetKey(KeyCode.E))
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

    public void ActivateWind()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            foreach(Transform card in cards)
            {
                if(card == null)
                {
                    return;
                }
                if(card.tag == "Card")
                {
                    card.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(0, 0.001f, 0.001f), transform.position, ForceMode.Impulse);
                }
            }
        }
    }

    public void StopPhysics(bool stop)
    {
        foreach(Transform card in cards)
        {
            if(card.tag == "Card")
            {
                card.GetComponent<Rigidbody>().isKinematic = stop;
            }
        }
    }

    private IEnumerator Pause(int p)
    {
        Time.timeScale = 0.1f;
        float pauseEndTime = Time.realtimeSinceStartup + 1;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1;
    }

    private IEnumerator Pause2(int p)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(p);
        Time.timeScale = 1;
    }

}
