using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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

public class PlayerController : MonoBehaviour   
{
    //PUBLIC
    public int maxHealth;
    

    //PRIVATE
    private Animator animator; // Character Animator
    private float speed = 3f; // Character Speed
    private Vector2 movement; // Character Movement Direction
    private Rigidbody2D rb; // Character Rigidbody2D
    private Transform transform; // Character Transform
    private string id;
    private Image healthBar;
    private string email { get; set; }
    private string password { get; set; }
    private bool isOpen = false;
    private int aux = -1;
    private int health;
    


    NetworkController nm;
    // Start is called before the first frame update
    void Awake()
    {
        nm = NetworkController.Instance;
    }

    public void UpdateID(string newId)
    {
        id = newId;
    }

    //PUBLIC
    public PlayerSkills playerSkills { get; private set; } // Character Skills
    

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator =gameObject.GetComponent<Animator>();
        transform = gameObject.GetComponent<Transform>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();

        playerSkills = new PlayerSkills();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            GameObject inventario;
            if (gameObject.name == "Elizabeth")
            {
                inventario = GameObject.FindGameObjectWithTag("InventarioE");
            }
            else
            {
                inventario = GameObject.FindGameObjectWithTag("InventarioA");
            }
            if (!isOpen)
            {
                inventario.GetComponent<FadeController>().FadeIn();
                isOpen = !isOpen;
            }
            else
            {
                inventario.GetComponent<FadeController>().FadeOut();
                isOpen = !isOpen;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject talk = GameObject.Find("Talk1");
            DialogueController.instance.StartConversation(talk.GetComponent<DialogueContainer>());
        }
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "EstanteElizabeth" || collision.gameObject.name == "EstantePais")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                collision.gameObject.SetActive(false);
                GameObject.Find("SecretPass").GetComponent<TilemapRenderer>().sortingOrder = 1;
                GameObject.Find("FalseWall").GetComponent<TilemapRenderer>().sortingOrder = 5;
            }
        }
    }

    public void DamagePlayer(int damage)
    {
        health -= damage;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        if(health <= 0)
        {
            animator.SetBool("isAlive", false);
            health = 0;
        }
        healthBar.fillAmount = ((float)health) / ((float)maxHealth);
    }
}
