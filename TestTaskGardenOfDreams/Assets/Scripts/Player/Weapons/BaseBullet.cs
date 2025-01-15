using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public float LifeTime;
    public float BulletDamage;
    [SerializeField] bool isItScenes = false;
    private float _currentTime = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        _currentTime += Time.fixedDeltaTime;
        if (_currentTime > LifeTime && !isItScenes)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable? baseUnit = null;
        collision.gameObject.TryGetComponent<IDamageable>(out baseUnit);
        if (baseUnit == null)
        {
            baseUnit = collision.gameObject.GetComponentInChildren<BaseUnit>();
        }
        if (baseUnit == null)
        {
            baseUnit = collision.gameObject.GetComponentInParent<BaseUnit>();
        }
        if (baseUnit != null)
        {
            baseUnit.TakeDamage(BulletDamage);
            Destroy(gameObject);
        }
    }
}
