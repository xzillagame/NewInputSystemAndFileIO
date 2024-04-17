using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    EnemyBaseState currentState = null;
    public EnemyBaseState CurrentState { get { return currentState; } }


    Enemy jeffEnemy = null;


    public void InitalizeStateMachine(Enemy jeff, EnemyBaseState startingState)
    {
        jeffEnemy = jeff;
        ChangeToNewState(startingState);
    }


    public void RunCurrentStateUpdate()
    {
        currentState.UpdateState();
    }


    public void ChangeToNewState(EnemyBaseState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState(jeffEnemy);
    }




}
