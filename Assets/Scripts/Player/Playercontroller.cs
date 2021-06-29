using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour,IDamageable
{

    private Rigidbody2D rb; //声明刚体组件
    private Animator anim;

    [Header("Move")]
    public float speed;
    public float jumpForce;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;

    [Header("States Check")]
    public bool isGround;
    public bool isJump;
    public bool canJump;

    [Header("Jump FX")]
    public GameObject jumpFX;
    public GameObject landFX;

    [Header("Attack")]
    public GameObject bomb;
    public float nextAttck = 0;
    public float attackRate;

    [Header("Player State")]
    public bool isDead = false;
    public float health;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();//获取刚体组件
        anim = GetComponent<Animator>();
        health = GameManager.instance.LoadHealth();
        UIManager.instance.UpdateHealth(health);
        GameManager.instance.IsPlayer(this);
    }

    void Update()//每帧1次 但根据设备 时间内执行次数不一样
    {
        anim.SetBool("isDead",isDead);
        if (isDead)
            return;
        CheckInput();//检测按键
    }

    private void FixedUpdate()//因为是物理上的移动，所以需要用Fixed。固定时间，50次/秒
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        PhysicsCheck();
        Movement();
        Jump();
    }

    void Movement() //玩家移动
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");// -1~1    raw不包含小数
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(horizontalInput, 1, 1);//根据移动方向进行翻转

        }
    }

    void Jump()//跳跃
    {
        if (canJump) 
        {
            isJump = true;
            jumpFX.SetActive(true);
            jumpFX.transform.position = transform.position+ new Vector3(0, -0.45f, 0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canJump = false;

        }
    }

    void CheckInput() 
    {
        if (Input.GetButtonDown("Jump") && isGround) //如果按下跳跃键，bool变为true
        {
            canJump = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();

        }
    
    }

    void PhysicsCheck() 
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGround)
        {
            rb.gravityScale = 1;
            isJump = false;

        }
        else {
            rb.gravityScale = 4;
        }
    
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    public void LandFX() 
    {
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0, -0.75f, 0);

    }

    public void Attack() {

        if (Time.time > nextAttck)
        {
            Instantiate(bomb,transform.position,bomb.transform.rotation);
            nextAttck = Time.time + attackRate;

        }
    }

    public void GetHit(float damgage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("PlayerHit"))
        {
            health -= damgage;
            if (health <= 0)
            {
                health = 0;
                isDead = true;

            }
            anim.SetTrigger("hit");

            UIManager.instance.UpdateHealth(health);

        }

    }
}
