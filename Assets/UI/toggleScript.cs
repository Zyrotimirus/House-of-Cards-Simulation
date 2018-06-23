using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr : MonoBehaviour {

    public Text myText = null;
    private bool toggle = false;

public void ButtonToggle()
    {
        if (toggle)
        {
            toggle = false;
            //swap texture to OFF
        }
        else
        {
            toggle = true;

        }
    }
}
