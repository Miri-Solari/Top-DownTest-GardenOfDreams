using UnityEngine;

public class BaseUnit : MonoBehaviour, IDamageable, IMortal
{
    public float HP { get => _currHP;}

    [SerializeField] private protected float _currHP = 10;

    public void TakeDamage(float dmg) //получаем урон
    {
        _currHP -= dmg;
        if (_currHP <= 0)
        {
            Death();
        }
    }

    public void Death() // помираем
    {
        Destroy(gameObject);
    }
}
