using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator anim;
    public float startTime;
    public float waitTIme;
    public float bombForce;
    private Collider2D clld;
    private Rigidbody2D rg;

    [Header("Check")]
    public float radius;
    public LayerMask targetLayer;

    void Start()
    {
        anim = GetComponent<Animator>();
        clld = GetComponent<Collider2D>();
        rg = GetComponent<Rigidbody2D>();
        startTime = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > startTime + waitTIme && !anim.GetCurrentAnimatorStateInfo(0).IsName("bomb_off") )
        {

            anim.Play("bomb_explotion");
        
        }

        
    }

    public void Explotion() {
        clld.enabled = false;
        rg.gravityScale = 0;
        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position,radius,targetLayer);
        foreach (var item in aroundObjects){
            Vector3 pos = transform.position - item.transform.position;
            item.GetComponent<Rigidbody2D>().AddForce((-pos + Vector3.up) * bombForce,ForceMode2D.Impulse);
            if (item.CompareTag("Bomb") && item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
            {
                item.GetComponent<Bomb>().TurnOn();
            }
            if (item.CompareTag("Player") )
            {
                item.GetComponent<IDamageable>().GetHit(1);
            }

        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void DestroyThis() {

        Destroy(gameObject);
    
    }

    public void TurnOff() {

        anim.Play("bomb_off");
        gameObject.layer = LayerMask.NameToLayer("Npc");
    
    }

    public void TurnOn()
    {
        startTime = Time.time;
        anim.Play("Bomb_on");
        gameObject.layer = LayerMask.NameToLayer("Bomb");
    }

}
