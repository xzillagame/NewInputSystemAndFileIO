using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    Vector3 origin;

    public NavMeshAgent NavMeshAgent { get; private set; }



    //State Machines Assignemnt Variables

    [Header("State Machine")]
    [SerializeField] StateMachine enemyStateMachine;
    public StateMachine JeffStateMachine { get { return enemyStateMachine; } }

    [Header("States")]
    #region Jeff States
    [SerializeField] private EnemyStates enemyStates;
    public EnemyStates JeffStates { get { return enemyStates; } }
    [Serializable]
    public struct EnemyStates
    {
        public EnemyWanderState wanderState;
        public EnemyChaseState chaseState;
        public EnemyAttackState attackState;
        public EnemyRecoveryState recoveryState;
    }
    #endregion



    [Header("Enemy State Ranges")]
    [SerializeField] float wanderRange;
    public float WanderRange { get { return wanderRange; } }

    [SerializeField] float playerSightRange;
    public float PlayerSightRange { get { return playerSightRange; } }

    [SerializeField] float playerAttackRange;
    public float PlayerArrackRange { get { return playerAttackRange; } }

    [Header("Recovery Time")]
    [SerializeField] float recoveryTime;
    public float RecoveryTime { get { return recoveryTime; } }





    Vector3 startingLocataion;
    public Vector3 StartingLocation { get { return startingLocataion; } }


    float currentStateElapsed = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        origin = transform.position;

        startingLocataion = transform.position;

        //Initalizing State Machine
        enemyStateMachine.InitalizeStateMachine(this, enemyStates.wanderState);


        //Set NavMesh
        NavMeshAgent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {


        enemyStateMachine.RunCurrentStateUpdate();


        if(Input.GetKeyDown(KeyCode.H))
        {

            if(enemyStateMachine.CurrentState is EnemyWanderState)
            {
                enemyStateMachine.ChangeToNewState(enemyStates.chaseState);
            }
            else if(enemyStateMachine.CurrentState is EnemyChaseState)
            {
                enemyStateMachine.ChangeToNewState(enemyStates.wanderState);
            }

        }



    }

    public void ApplyKnockback(Vector3 knockback)
    {
        GetComponent<Rigidbody>().AddForce(knockback, ForceMode.Impulse);
    }

    public void Respawn()
    {
        transform.position = origin;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRange);


        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerSightRange);


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerAttackRange);
    }







}
