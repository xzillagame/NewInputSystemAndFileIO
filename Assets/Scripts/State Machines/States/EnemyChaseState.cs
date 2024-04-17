using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{

    [SerializeField] LayerMask detectionLayer;

    FPSController player;


    public override void EnterState(Enemy jeffRef)
    {

        this.gameObject.SetActive(true);


        jeffEnemy = jeffRef;



        Collider[] colliders = Physics.OverlapSphere(transform.position, jeffEnemy.PlayerSightRange, detectionLayer);



        if (colliders.Length != 0)
        {
            //Assume Player is first index
            player = colliders[0].GetComponent<FPSController>();
        }


    }

    public override void UpdateState()
    {

        jeffEnemy.NavMeshAgent.SetDestination(player.transform.position);



        float distanceFromSelfToTarget = Vector3.Distance(jeffEnemy.transform.position, player.transform.position);


        if(distanceFromSelfToTarget >= jeffEnemy.PlayerSightRange)
        {
            jeffEnemy.JeffStateMachine.ChangeToNewState(jeffEnemy.JeffStates.wanderState);
        }
        else if (distanceFromSelfToTarget <= jeffEnemy.PlayerArrackRange)
        {
            jeffEnemy.JeffStateMachine.ChangeToNewState(jeffEnemy.JeffStates.attackState);
        }


    }

    public override void ExitState()
    {
        jeffEnemy = null;


        this.gameObject.SetActive(false);
    }



}
