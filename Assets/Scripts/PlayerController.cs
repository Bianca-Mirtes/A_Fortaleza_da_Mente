using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour   
{
    //PRIVATE
    [SerializeField] private Animator animator; // Character Animator
    private float speed; // Character Speed
    private Vector2 movement; // Character Movement Direction
    private Rigidbody2D rb;



    //PUBLIC

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
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

        rb.SetRotation(0f);

        // Animations prioritize the X movement

        // Moving Upwards
        if(moveY > 0)
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

        // Moving Rightside
        if (moveX > 0)
        {
            rb.SetRotation(0f);
            animator.SetInteger("isWalking", 3);
            animator.SetInteger("isIdle", -1);


        }

        // Moving Leftside
        else if (moveX < 0)
        {
            animator.SetInteger("isWalking", 1);
            animator.SetInteger("isIdle", -1);
        }

        // Idle
        else
        {
            animator.SetInteger("isWalking", -1);
            if(animator.GetInteger("isWalking") != -1)
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
