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
    public int floors;
    public int pillars;

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
        CreateCard(preparedCard, new Vector3(0,0,0), false, 0);

        zAxis = GameObject.Find("Table").transform.position.z;

        CreateHouseAutomatically(pillars, floors);
        Debug.Log("Card anglesize whatever = " + GetxSize(10.0f));
        Debug.Log("card.GetComponent<Renderer>().bounds.size.x = " + card.GetComponent<Renderer>().bounds.size.x);
        Debug.Log("Mathf.Cos(10.0f) = " + Mathf.Acos(10.0f));
    }
	
	void Update () {
        ModeChange();
        CameraFree();
        MoveSpawningRow();

        ActivateWind();
        cards.RemoveAll(card => card == null);
    }

    public void ModeChange()
    {
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
            CreateCard(card, new Vector3(objectPos.x, objectPos.y, zAxis), false, 0);
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
        if (lockedMode)
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
    }

    public void CreateCard(GameObject obj, Vector3 objectPosition, bool autoCreation, float autoAngle)
    {
        Vector3 angle = new Vector3(0, 90, 90);
        if (obj.tag == "Card" && !autoCreation) {
            angle = new Vector3(cards[0].eulerAngles.x, cards[0].eulerAngles.y, cards[0].eulerAngles.z);
        }
        if(autoCreation)
        {
            angle = new Vector3(autoAngle, 90, 90);
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
            //Debug.Log("Cards length davor = " + cards.Count);
            
            //Debug.Log("Cards length danach = " + cards.Count);
            foreach (Transform card in cards)
            {
                if(card != null && card.tag == "Card")
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

    public void FlipPhysic()
    {
        stopPhysics = !stopPhysics;
        StopPhysics(stopPhysics);
    }

    public void FlipCreationMode()
    {
        creationMode = !creationMode;
        ModeChange();
    }

    public void FlipLockedMode()
    {
        lockedMode = !lockedMode;
    }

    public void CreateHouseAutomatically(int pillars, int floors)
    {
        if (floors > pillars)
        {
            return;
        }
        
        for(int i = 0; i < floors; i++)
        {
            StartCoroutine(CreateCardRow(pillars - i, ( i * 0.33333f ) + ( - pillars / 2.66666f ), i * 0.8f, 0, i*2));
        }


        /*StartCoroutine(CreateCardRow(12, -3, 0, 0, 0));

        StartCoroutine(CreateCardRow(11, -2.6665f, 0.9f, 0, 2));

        StartCoroutine(CreateCardRow(10, -2.3335f, 1.8f, 0, 4));

        StartCoroutine(CreateCardRow(9, -2f, 2.7f, 0, 6));

        StartCoroutine(CreateCardRow(8, -1.6665f, 3.6f, 0, 8));

        StartCoroutine(CreateCardRow(7, -1.3335f, 4.5f, 0, 10));*/
    }

    IEnumerator CreateCardRow(int triangularPillars, float startPosition, float floor, float axis, int time)
    {
        yield return new WaitForSeconds(time);

        bool booleanSwitch = true;
        for (int i = 1; i <= triangularPillars * 2; i++)
        {
            if (booleanSwitch)
            {
                CreateCard(card, new Vector3(startPosition + i * 0.35f, 6.6f + floor, -6.0f + axis), true, 10);
            }
            else
            {
                CreateCard(card, new Vector3(startPosition + i * 0.35f, 6.6f + floor, -6.0f + axis), true, -10);
            }
            booleanSwitch = !booleanSwitch;
        }

        yield return new WaitForSeconds(1);
        for(int i = 1; i < triangularPillars; i++)
        {
           CreateCard(card, new Vector3(startPosition + (i * 0.863f - ((i - 1) * 0.16f)), 7.0f + floor, -6.0f + axis), true, 90);
        }
    }

    public float GetxSize(float angle)
    {
        return card.GetComponent<Renderer>().bounds.size.x * Mathf.Cos(angle);
    }
}
    
