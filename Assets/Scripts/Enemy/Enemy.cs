using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private GameObject alarmSign;
    [Header("State")]
    EnemyBaseState currentState;
    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();

    [Header("Move")]
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;

    [Header("Attack")]
    public List<Transform> attackList = new List<Transform>();
    private float nextAttck = 0;
    public float attackRate;
    public float attackRange,skillRange;


    [Header("Anim")]
    public Animator anim;
    public int anim_state;

    [Header("Enemy State")]
    public bool isDead = false;
    public float health;
    public bool isBoth;

    public virtual void Init() {

        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject;
    }
    private void Awake()
    {
        Init();
    }

    void Start()
    {
        GameManager.instance.AddEnemy(this);
        TransitionToState(patrolState);
        if (isBoth) {
            UIManager.instance.SetBossHealth(health);
        }
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isDead", isDead);
        if (isBoth)
        {
            UIManager.instance.UpdateBossHealth(health);

        }
        if (isDead)
        {
            GameManager.instance.EnemyDead(this);
            return;

        }
        currentState.OnUpdate(this);
        anim.SetInteger("state", anim_state);

    }


    public void TransitionToState(EnemyBaseState state) {

        currentState = state;
        currentState.EnterState(this);
    
    }

    public void MoveToTatget() 
    {
    
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed*Time.deltaTime);
        FlipDirection();
    }

    public void FlipDirection() 
    {
        if (transform.position.x - targetPoint.position.x > 0)
        {

            transform.rotation = Quaternion.Euler(0,180f,0f);

        }
        else {
            transform.rotation = Quaternion.Euler(0f, 0, 0);
        }
    }

    public void SwitchPoint()
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;

        }
        else {

            targetPoint = pointB;
        }
    
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!attackList.Contains(collision.transform) && !GameManager.instance.gameOver)
            attackList.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attackList.Remove(collision.transform);
    }

    public virtual void Attack()
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAttck)
            {
                anim.SetTrigger("attack");
                nextAttck = Time.time + attackRate;
            }
        }

    }

    public virtual void SkillAction()
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextAttck)
            {
                anim.SetTrigger("skill");
                nextAttck = Time.time + attackRate;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.gameOver)
        {
            StartCoroutine(OnAlarm());
        }
    }

    IEnumerator OnAlarm() 
    {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);
    }
}
