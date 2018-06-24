using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ToggleScript : MonoBehaviour {

    
    public string toggleText = null;
    public Color toggleColor = new Vector4(1, 0, 0, 1);
    public KeyCode triggerKey;
    public UnityEvent functionToCall;

    private Color startColor;
    private string startText;
    private bool isActive = false;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void Awake()
    {
        if (functionToCall == null)
            functionToCall = new UnityEvent();
    }

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            TaskOnClick();
        }
    }

    void TaskOnClick()
    {
        if (isActive)
        {
            isActive = false;
            GetComponentInChildren<Text>().text = startText;
            GetComponent<Image>().color = startColor;
            functionToCall.Invoke();
        }
        else
        {
            isActive = true;
            startText = GetComponentInChildren<Text>().text;
            startColor = GetComponent<Image>().color;
            GetComponentInChildren<Text>().text = toggleText;
            GetComponent<Image>().color = toggleColor;
            functionToCall.Invoke();
        }
    }
}
