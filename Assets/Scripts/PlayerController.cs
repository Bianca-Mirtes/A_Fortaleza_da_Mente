using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    //PUBLIC
    public int id;
    public float cooldown;
    public string name;
    public string description;

    public void Activate()
    {
        if(cooldown == 0f)
        {
            ActivateSkill();
        }
    }

    private void ActivateSkill()
    {

    }
}

public class PlayerSkills
{
    //PUBLIC    


    //PRIVATE
    private List<Skill> unlockedSkills;

    public PlayerSkills() { 
        unlockedSkills = new List<Skill>();
    }

    public void SetSkills(List<Skill> type){
        unlockedSkills = type;
    }

    public void UnlockSkill(Skill type)
    {
        unlockedSkills.Add(type);
    }    

    public bool IsSkillUnlocked(Skill type)
    {
        return unlockedSkills.Contains(type);
    }

    public List<Skill> GetSkillTypes()
    {
        return unlockedSkills;
    }
}

public class PlayerController : MonoBehaviour   
{
    //PRIVATE
    private Animator animator; // Character Animator
    private float speed = 3f; // Character Speed
    private Vector2 movement; // Character Movement Direction
    private Rigidbody2D rb; // Character Rigidbody2D
    private Transform transform; // Character Transform





    //PUBLIC
    public PlayerSkills playerSkills { get; private set; } // Character Skills
    

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator =gameObject.GetComponent<Animator>();
        transform = gameObject.GetComponent<Transform>();

        playerSkills = new PlayerSkills();
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
        movement = new Vector2(moveX,  moveY).normalized *  speed;
        rb.velocity = movement;

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
    void FixedUpdate()
    {
        UpdateMovement();
    }
}
