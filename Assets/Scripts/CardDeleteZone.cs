using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeleteZone : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
