using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour {

    public GameObject card;
    public GameObject preparedCard;
    public GameObject cardsParent;
    public Transform selectedCard;

    [Tooltip("The space between rows in card width percentage")]
    public float spacePercentage;
    public float cameraSpeed = 2f;
    public int floors;
    public int pillars;
    public int globalCardID = 0;
    public string cardTagName = "Card";
    public bool creationMode = true;
    public bool lockedMode = true;
    public bool stopPhysics = false;

    private List<Transform> cards;
    private Vector3 mousePos;
    private float zAxis;
    private float cameraYaw = 0;
    private float cameraPitch = 0;
    private float spawnHeight = 0;

    void Start () {
        cards = new List<Transform>();
        CreateCard(preparedCard, new Vector3(0,0,0), true, 45);

        zAxis = GameObject.Find("Table").transform.position.z;
        spawnHeight = 6.52f;
        //spawnHeight = GameObject.Find("Table").transform.position.y + card.transform.position.y;
        Debug.Log("Spawnheight = " + spawnHeight);
    }
	
	void Update () {
        ModeChange();
        CameraFree();
        MoveSpawningRow();
        DeleteAll();

        CheckMouseWheel();

        ActivateWind();

        cards.RemoveAll(card => card == null);
    }

    public void ModeChange()
    {
        if (creationMode) {
            selectedCard = null;
            CreationMode();
        } else {
            EditMode();
        }
        cards[0].gameObject.SetActive(creationMode);
    }

    public void CreationMode()
    {
        if (lockedMode)
        {
            cards[0].position = new Vector3(CalculateMousePosition(spawnHeight).x, CalculateMousePosition(spawnHeight).y, zAxis);
        }
        else cards[0].position = new Vector3(CalculateMousePosition(spawnHeight).x, CalculateMousePosition(spawnHeight).y, CalculateMousePosition(spawnHeight).z);
        RotateCard();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (lockedMode)
            {
                CreateCard(card, new Vector3(CalculateMousePosition(spawnHeight).x, CalculateMousePosition(spawnHeight).y, zAxis), false, 0);
            }
            else
            {
                CreateCard(card, new Vector3(CalculateMousePosition(spawnHeight).x, CalculateMousePosition(spawnHeight).y, CalculateMousePosition(spawnHeight).z), false, 0);
            }
        }
    }

    public void EditMode()
    {
        SelectCard();
        MoveCard();
    }

    public void SelectCard()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.transform.tag == cardTagName)
                {
                    Debug.Log("Card ID: " + hitInfo.transform.GetComponent<Card>().id);
                    selectedCard = hitInfo.transform;
                }
            }
        }
    }

    public void MoveCard()
    {
        if (Input.GetKey(KeyCode.Space) && selectedCard != null)
        {
            selectedCard.position = new Vector3(CalculateMousePosition(spawnHeight).x, CalculateMousePosition(spawnHeight).y, zAxis);
        }
    }

    public Vector3 CalculateMousePosition(float positionY)
    {
        mousePos = Input.mousePosition;
        
        mousePos.z = zAxis * -1f;
        

        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        
        if (objectPos.y <= positionY)
        {
            objectPos.y = positionY;
        }

        return objectPos;
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
        if (obj.tag == cardTagName && !autoCreation) {
            angle = new Vector3(cards[0].eulerAngles.x, cards[0].eulerAngles.y, cards[0].eulerAngles.z);
        }
        if(autoCreation)
        {
            angle = new Vector3(autoAngle, 90, 90);
        }
        var clone = Instantiate(obj, objectPosition, Quaternion.Euler(angle), cardsParent.transform);

        if(clone.tag == cardTagName)
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
        if(!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Q))
        {
            cards[0].Rotate(new Vector3(0, 1, 0), 1.6f);
        }
        else if(!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.E))
        {
            cards[0].Rotate(new Vector3(0, 1, 0), -1.6f);
        }
        /*else if(Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift))
        {
            if (Mathf.Approximately(cards[0].transform.eulerAngles.y % 15, 0))
            {
                cards[0].Rotate(new Vector3(0, 1, 0), 15);
            }
            else
            {
                cards[0].Rotate(new Vector3(0, 1, 0), cards[0].transform.eulerAngles.y % 15);
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E))
        {
            if (cards[0].transform.eulerAngles.y % 15 != 0)
            {
                cards[0].Rotate(new Vector3(0, 1, 0), cards[0].transform.eulerAngles.y % 15 * -1.0f);
            }
            else
            {
                cards[0].Rotate(new Vector3(0, 1, 0), -15);
            }
        }*/
    }

    public void ActivateWind()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            foreach (Transform card in cards)
            {
                if(card != null && card.tag == cardTagName)
                {
                    card.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(0, 0.01f, 0.01f), transform.position, ForceMode.Impulse);
                }
            }
        }
    }

    public void StopPhysics(bool stop)
    {
        foreach(Transform card in cards)
        {
            if (card != null && card.tag == cardTagName)
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

    public void CreateHouseAutomatically()
    {
        int pillars = 0;
        int floors = 0;
        InputField pillarInput = GameObject.Find("pillars").GetComponent<InputField>();
        InputField floorInput = GameObject.Find("floors").GetComponent<InputField>();

        try
        {
            pillars = int.Parse(pillarInput.text);
        }
        catch{
            pillarInput.text = "Wrong value";
        }
        try
        {
            floors = int.Parse(floorInput.text);
        }
        catch
        {
            floorInput.text = "Wrong value";
        }

        if (floors > pillars)
        {
            floorInput.text = "Choose less floors";
            return;
        }
        
        for(int i = 0; i < floors; i++)
        {
            StartCoroutine(CreateCardRow(pillars - i, ( i * 0.33333f ) + ( - pillars / 2.66666f ), i * GetxSize(10.0f), zAxis + 6.0f, i*2));
        }
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
        return card.GetComponent<Renderer>().bounds.size.x * Mathf.Cos(angle * Mathf.Deg2Rad);
    }

    public void CheckMouseWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            cards[0].Rotate(new Vector3(0, 0, 1), 15);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            cards[0].Rotate(new Vector3(0, 0, 1), -15);
        }
    }

    public void DeleteAll()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach(Transform card in cards)
            {
                if (card.tag == cardTagName)
                {
                    Destroy(card.gameObject);
                }
            }
        }
    }
}
    
