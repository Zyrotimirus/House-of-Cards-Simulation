using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public int id;
    public float minimumCardFrictionMoving;
    public float maximumCardFrictionMoving;

    private Rigidbody rb;
    private float oldVelo;
    private float curVeloDifference;

    Renderer m_Renderer;
	
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        
        m_Renderer = GetComponent<Renderer>();
        Object[] textures = Resources.LoadAll("Textures/card_textures", typeof(Texture2D));
        Texture2D texture = (Texture2D)textures[Random.Range(0, textures.Length)];
        m_Renderer.material.mainTexture = texture;
        oldVelo = rb.velocity.sqrMagnitude;
    }

    void update()
    {
        curVeloDifference = oldVelo - rb.velocity.sqrMagnitude;
        if (curVeloDifference <= 0.1f)
        {
            if (GetComponent<Collider>().material.dynamicFriction > minimumCardFrictionMoving)
            {
                GetComponent<Collider>().material.dynamicFriction -= 0.01f;
            }
        }
        else if (curVeloDifference >= 0.1f)
        {
            if (GetComponent<Collider>().material.dynamicFriction < maximumCardFrictionMoving)
            {
                GetComponent<Collider>().material.dynamicFriction += 0.01f;
            }
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        //rb.velocity = collision.rigidbody.velocity;
    }
}
