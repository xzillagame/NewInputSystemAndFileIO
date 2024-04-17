using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{

    [SerializeField] float lungeForce = 10f;

    [SerializeField] float attackTime = 0.5f;
    float elapsedTime = 0f;

    public override void EnterState(Enemy jeffRef)
    {

        this.gameObject.SetActive(true);

        jeffEnemy = jeffRef;


        //Apply Lunge

        jeffEnemy.Rigidbody.isKinematic = false;
        jeffEnemy.NavMeshAgent.enabled = false;

        jeffEnemy.Rigidbody.AddForce(jeffEnemy.transform.forward * lungeForce, ForceMode.Impulse);


        Debug.Log("BOOOOOOOOOOOOOOOO!!!!!!!!!!!");

    }

    public override void UpdateState()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > attackTime)
        {
            elapsedTime = 0f;
            jeffEnemy.JeffStateMachine.ChangeToNewState(jeffEnemy.JeffStates.recoveryState);
            return;
        }
    }

    public override void ExitState()
    {
        jeffEnemy.Rigidbody.isKinematic = true;
        jeffEnemy.NavMeshAgent.enabled = true;

        jeffEnemy = null;

        this.gameObject.SetActive(false);
    }

}
