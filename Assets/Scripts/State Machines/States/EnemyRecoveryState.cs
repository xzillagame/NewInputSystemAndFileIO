using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRecoveryState : EnemyBaseState
{

    float elapsedTime = 0f;


    public override void EnterState(Enemy jeffRef)
    {
        this.gameObject.SetActive(true);

        jeffEnemy = jeffRef;

        elapsedTime = 0f;
    }

    public override void UpdateState()
    {

        elapsedTime += Time.deltaTime;
        if(elapsedTime >= jeffEnemy.RecoveryTime)
        {
            jeffEnemy.JeffStateMachine.ChangeToNewState(jeffEnemy.JeffStates.chaseState);
        }


    }

    public override void ExitState()
    {
        jeffEnemy = null;
        elapsedTime = 0f;

        this.gameObject.SetActive(false);
    }

}
