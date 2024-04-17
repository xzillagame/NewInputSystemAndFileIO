using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]
public class EnemyWanderState : EnemyBaseState
{   
    [SerializeField] float wanderTimeInterval = 5f;
    private float interalWaitingTime = 0f;

    private SphereCollider chaseSphereDetectionArea;


    public override void EnterState(Enemy jeffRef)
    {
        this.gameObject.SetActive(true);

        jeffEnemy = jeffRef;
        interalWaitingTime = 0f;

        chaseSphereDetectionArea = GetComponent<SphereCollider>();
        chaseSphereDetectionArea.radius = jeffEnemy.PlayerSightRange;
    }

    public override void UpdateState()
    {
      

        //Pick a new spot to wander too after a certain time has passed
        interalWaitingTime += Time.deltaTime;
        if(interalWaitingTime > wanderTimeInterval)
        {
            jeffEnemy.NavMeshAgent.SetDestination(GetRandomPointForNavMovement());
            interalWaitingTime = 0f;
        }


    }

    public override void ExitState()
    {
        

        jeffEnemy = null;
        interalWaitingTime = 0f;


        this.gameObject.SetActive(false);
    }


    private Vector3 GetRandomPointForNavMovement()
    {
        Vector3 offset = new Vector3(Random.Range(-jeffEnemy.WanderRange, jeffEnemy.WanderRange),
                                    0f,
                                    Random.Range(-jeffEnemy.WanderRange, jeffEnemy.WanderRange));

        NavMeshHit hit;

        bool isValidPoint = NavMesh.SamplePosition(jeffEnemy.StartingLocation + offset, out hit, 1, NavMesh.AllAreas);

        if(isValidPoint == true)
        {
            return hit.position;
        }

        return Vector3.zero;
    }




    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<FPSController>() != null)
        {
            jeffEnemy.JeffStateMachine.ChangeToNewState(jeffEnemy.JeffStates.chaseState);
        }
    }

}
