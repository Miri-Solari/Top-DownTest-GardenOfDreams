using UnityEngine;

public enum EnemyState { Chill, Attack, Move, MoveAndAttack, Evade, FindTheWay }

public class BaseBehavior : MonoBehaviour
{
    public EnemyState State;
    [SerializeField] protected GameObject player;

    private void FixedUpdate()
    {
        switch (State)
        {
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Chill:
                Chill();
                break;
            case EnemyState.MoveAndAttack:
                MoveAndAttack();
                break;
            case EnemyState.Evade:
                Evade();
                break;
            case EnemyState.FindTheWay:
                FindTheWay();
                break;
        }
    }

    protected virtual void Attack() { }
    protected virtual void Chill() { }
    protected virtual void MoveAndAttack() { }
    protected virtual void Evade() { }
    protected virtual void FindTheWay() { }
}

