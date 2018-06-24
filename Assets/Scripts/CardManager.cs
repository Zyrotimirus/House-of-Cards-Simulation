using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {

    public GameObject card;
    public GameObject preparedCard;
    public GameObject cardsParent;
    [Tooltip("The percentage of")]
    public float spacePercentage;
    public float cameraSpeed = 2f;

    private List<Transform> cards;
    private Vector3 mousePos;
    private float zAxis = 0;
    private float cameraYaw = 0;
    private float cameraPitch = 0;

    public int globalCardID = 0;

    public bool creationMode = true;
    public bool lockedMode = true;
    public bool stopPhysics = false;
    

    void Start () {
        cards = new List<Transform>();
        CreateCard(preparedCard, new Vector3(0,0,0));

        zAxis = GameObject.Find("Table").transform.position.z;
    }
	
	void Update () {

        ModeChange();
        CameraFree();
        MoveSpawningRow();

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
        if (lockedMode)
        {
            objectPos.z = zAxis;
        }

        if (objectPos.y <= 6.5f)
        {
            objectPos.y = 6.5f;
        }
        cards[0].position = new Vector3(objectPos.x, objectPos.y, zAxis);
        
        RotateCard();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateCard(card, new Vector3(objectPos.x, objectPos.y, zAxis));
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
                Camera.main.transform.localPosition += Camera.main.transform.forward / 10;
            }
            if (Input.GetKey(KeyCode.S))
            {
                Camera.main.transform.localPosition -= Camera.main.transform.forward / 10;
            }

            if (Input.GetKey(KeyCode.A))
            {
                Camera.main.transform.localPosition -= Camera.main.transform.right / 10;
            }
            if (Input.GetKey(KeyCode.D))
            {
                Camera.main.transform.localPosition += Camera.main.transform.right / 10;
            }

            cameraPitch -= cameraSpeed * Input.GetAxis("Mouse Y");
            cameraYaw += cameraSpeed * Input.GetAxis("Mouse X");

            Camera.main.transform.localEulerAngles = new Vector3(cameraPitch, cameraYaw, 0.0f);
        }
    }

    public void MoveSpawningRow()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            zAxis += (preparedCard.GetComponent<Renderer>().bounds.size.y * (1 + spacePercentage / 100));
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            zAxis -= (preparedCard.GetComponent<Renderer>().bounds.size.y * (1 + spacePercentage / 100));
        }
    }

    public void CreateCard(GameObject obj, Vector3 objectPosition)
    {
        Vector3 angle = new Vector3(0, 90, 90);
        if (obj.tag == "Card") {
            angle = new Vector3(cards[0].eulerAngles.x, cards[0].eulerAngles.y, cards[0].eulerAngles.z);
        }
        var clone = Instantiate(obj, objectPosition, Quaternion.Euler(angle), cardsParent.transform);

        if(clone.tag == "Card")
        {
            clone.GetComponent<Card>().id = globalCardID;
            if(stopPhysics)
            {
                clone.GetComponent<Rigidbody>().isKinematic = stopPhysics;
            }
        }
        cards.Add((Transform)clone.transform);
        globalCardID++;
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
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            cards[0].Rotate(0, cards[0].localEulerAngles.y * -1, 0);
        }
    }

    public void SelectCard()
    {

    }

    public void MoveCard()
    {

    }

    public void ActivateWind()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            foreach(Transform card in cards)
            {
                if(card == null)
                {
                    cards.Remove(card);  
                }
                else if(card.tag == "Card")
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
            if (card == null)
            {
                cards.Remove(card);
            }
            else if (card.tag == "Card")
            {
                card.GetComponent<Rigidbody>().isKinematic = stop;
            }
        }
    }
}
