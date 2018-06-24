using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    private Rigidbody rb;

    Renderer m_Renderer;
	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        
        m_Renderer = GetComponent<Renderer>();
        Object[] textures = Resources.LoadAll("Textures/card_textures", typeof(Texture2D));
        Texture2D texture = (Texture2D)textures[Random.Range(0, textures.Length)];
        m_Renderer.material.mainTexture = texture;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = collision.rigidbody.velocity;
    }


}
