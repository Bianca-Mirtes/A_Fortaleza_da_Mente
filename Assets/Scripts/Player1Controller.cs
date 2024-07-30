using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    //PUBLIC
    public int id;
    public float cooldown;
    public float time;
    public string name;
    public string description;
    public Skill(int id, float cool, string name, string desc)
    {
        this.id = id;
        cooldown = cool;
        this.name = name;
        description = desc;
    }   

    public void Activate()
    {
        if(time == 0f)
        {
            ActivateSkill();
        }
    }

    protected void ActivateSkill()
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

public class Player1Controller : MonoBehaviour   
{
    //PRIVATE
    private Animator animator; // Character Animator
    private float speed = 3f; // Character Speed
    private Vector2 movement; // Character Movement Direction
    private Rigidbody2D rb; // Character Rigidbody2D
    private Transform transform; // Character Transform
    private string email { get; set; }
    private string password { get; set; }
    private int aux = -1;


    NetworkController nm;
    // Start is called before the first frame update
    void Awake()
    {
        nm = NetworkController.Instance;
    }

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
        2 : Upwards, 
        3 : Rightside
         */

        float moveX = Input.GetAxisRaw("Horizontal"); // X input movement
        float moveY = Input.GetAxisRaw("Vertical"); // Y input movement
        movement = new Vector2(moveX,  moveY).normalized *  speed;
        rb.velocity = movement;        

        

        // Animations prioritize the X movement

        // Moving Rightside
        if (moveX > 0)
        {
            
            animator.SetInteger("isWalking", 3);
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
            if (animator.GetInteger("isWalking") != -1)
            {
                aux = animator.GetInteger("isWalking");
            }
            animator.SetInteger("isIdle", aux);
            animator.SetInteger("isWalking", -1);
        }      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMovement();
    }
}
