using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public int id;
    private Rigidbody rb;

    Renderer m_Renderer;
	
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        
        m_Renderer = GetComponent<Renderer>();
        Object[] textures = Resources.LoadAll("Textures/card_textures", typeof(Texture2D));
        Texture2D texture = (Texture2D)textures[Random.Range(0, textures.Length)];
        m_Renderer.material.mainTexture = texture;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //rb.velocity = collision.rigidbody.velocity;
    }
}
