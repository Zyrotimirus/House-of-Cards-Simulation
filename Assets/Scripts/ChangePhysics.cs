using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePhysics : MonoBehaviour {

    public void ValueChanger(float newValue)
    {
        Physics.gravity = new Vector3(0, newValue, 0);
    }
}
