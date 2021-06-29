using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy,IDamageable
{
    public void GetHit(float damgage)
    {
        health -= damgage;
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
        anim.SetTrigger("hit");
    }

    public override void Init()
    {
        base.Init();
    }

    public void SetOff() {

        targetPoint.GetComponent<Bomb>().TurnOff();
    
    }

}
