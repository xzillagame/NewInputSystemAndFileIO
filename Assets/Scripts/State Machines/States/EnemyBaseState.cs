using UnityEngine;

public abstract class EnemyBaseState : MonoBehaviour
{

    protected Enemy jeffEnemy;

    public abstract void EnterState(Enemy jeffRef);
    public abstract void UpdateState();
    public abstract void ExitState();


}
