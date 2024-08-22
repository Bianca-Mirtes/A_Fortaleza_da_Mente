using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    //public GameObject pointA, pointB;
    public float velocity;
    public bool isVertical;
    public int damage;

    private bool isTurning;    
    private Animator animator;
    private Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {        
        if(isTurning)
        {                        
            velocity *= -1;
            isTurning = false;
        }
        if (isVertical)
            movement = new Vector2(0, velocity);
        else
            movement = new Vector2(velocity, 0);
        animator.SetFloat("velocity", velocity);
        GetComponent<Rigidbody2D>().velocity = movement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TurningPoint")
        {
            isTurning = true;            
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().DamagePlayer(damage);
        }
    }        
}
