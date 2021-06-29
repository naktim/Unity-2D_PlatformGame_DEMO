using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.anim_state = 0;
        enemy.SwitchPoint();
    }

    public override void OnUpdate(Enemy enemy)
    {
        if (!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            enemy.anim_state = 1;
            enemy.MoveToTatget();
        }

        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.01f)
        {
            enemy.SwitchPoint();
            enemy.TransitionToState(enemy.patrolState);
        }

        if (enemy.attackList.Count >0 )
        {
            enemy.TransitionToState(enemy.attackState);
        }
    }
}
