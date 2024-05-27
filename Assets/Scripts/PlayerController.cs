using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour   
{
    //PRIVATE
    private Animator animator; // Character Animator
    private float speed; // Character Speed
    private Vector2 movement; // Character Movement Direction
    private Rigidbody2D rb;
    private Transform transform;



    //PUBLIC

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator =gameObject.GetComponent<Animator>();
        transform = gameObject.GetComponent<Transform>();
    }

    void UpdateMovement()
    {        
        /*
         Animator Directions = { 
        0 : Downwards,
        1 : Leftside,
        2 : Upwards        
         */

        float moveX = Input.GetAxisRaw("Horizontal"); // X input movement
        float moveY = Input.GetAxisRaw("Vertical"); // Y input movement
        movement = new Vector2(moveX,  moveY).normalized * Time.deltaTime * speed;
        //transform.position = movement;

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        // Animations prioritize the X movement

        // Moving Rightside
        if (moveX > 0)
        {
            transform.rotation = Quaternion.Euler(0f,180f,0f);
            animator.SetInteger("isWalking", 1);
            animator.SetInteger("isIdle", -1);


        }

        // Moving Leftside
        else if (moveX < 0)
        {
            animator.SetInteger("isWalking", 1);
            animator.SetInteger("isIdle", -1);
        }

        // Moving Upwards
        else if (moveY > 0)
        {
            animator.SetInteger("isWalking", 2);
            animator.SetInteger("isIdle", -1);
        }

        // Moving Downwards
        else if (moveY < 0)
        {
            animator.SetInteger("isWalking", 0);
            animator.SetInteger("isIdle", -1);
        }       

        // Idle
        else
        {
            animator.SetInteger("isWalking", -1);
            if (animator.GetInteger("isWalking") != -1)
                animator.SetInteger("isIdle", animator.GetInteger("isWalking"));
            else
                animator.SetInteger("isIdle", 0);
        }


        

        
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }
}
