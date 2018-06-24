using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCard : MonoBehaviour {

	
	void Start () {
		
	}
	
	
	void Update () {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.tag == "Card")
        {
            //gameObject.GetComponent<Material>().color = Color.red;
        }
    }
}
