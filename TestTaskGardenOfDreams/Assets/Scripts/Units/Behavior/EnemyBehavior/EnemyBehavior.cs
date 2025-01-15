using UnityEngine;



public class EnemyBehavior : BaseBehavior //взял со своего проекта и немного переделал под текущие задачи
{
    [SerializeField] Rigidbody2D body;
    [SerializeField] Transform centre;
    [SerializeField] LayerMask maskToBypass;
    [SerializeField, Range(0, 15), Tooltip("Радиус в котором можно двигаться")] float walkingRadius;
    [SerializeField, Range(0, 15), Tooltip("Радиус в котором противник замечает игрока")] float agrRadius;
    [SerializeField, Range(0, 15), Tooltip("Радиус в котором противник уклоняется/бежить в сторону игрока")] float evadeRadius;
    [SerializeField, Range(0, 15), Tooltip("Радиус в котором противник атакует")] float attackRadius;
    [SerializeField, Range(0, 1), Tooltip("Точность при прибытии на заданную точку")] float targetPositionAccuracy;
    [SerializeField, Range(0, 10), Tooltip("Скорость")] float speed;
    [SerializeField, Range(0, 100), Tooltip("Шанс того, что юнит остановится и будет некоторое время стоять на месте")] float chillingChancePercent = 75;
    [SerializeField, Range(0, 5), Tooltip("Время остановки")] float chillingTime = 2;
    [SerializeField, Range(0, 5), Tooltip("Время после которого появляется возможность остановки")] float noChillingTime = 2;
    [SerializeField, Range(0, 5), Tooltip("Время между атаками")] float timeToAttack = 2f;
    [SerializeField] float attackDamage = 2f;
    [SerializeField] bool isMelee = false;
    private IDamageable _playerUnit;
    private Vector2 _targetPosition;
    private Vector2 _startPosition;
    private float _currChillTime = 100000;
    private float _currNoChillTime = 0;
    private float _meleeMulti = -1;
    private float _currTimeToAttack;
    private bool _isWalking = false;
    private bool _isChilling = false;
    private bool _isFlipped = false;

    private void Awake()
    {
        _startPosition = centre.position;
        if (isMelee)
        {
            _meleeMulti = 1;
        }
        if (player != null)
        {
            _playerUnit = player.GetComponent<IDamageable>();
        }
    }


    override protected void Attack()
    {
        if (player != null)
        {
            _currTimeToAttack += Time.fixedDeltaTime;
            if ((centre.position - player.transform.position).magnitude < attackRadius && timeToAttack <= _currTimeToAttack)
            {
                _playerUnit.TakeDamage(attackDamage);
                _currTimeToAttack = 0;
            }
            
        }
    }

    override protected void Chill()
    {
        if ((Random.value > chillingChancePercent / 100 || _isChilling) && _currNoChillTime > noChillingTime)
        {
            if (_currChillTime > chillingTime && _isChilling)
            {
                _isChilling = false;
                _currNoChillTime = chillingTime;
                _currChillTime = 0;
            }
            else
            {
                if (!_isChilling)
                {
                    _currNoChillTime = 0;
                    _isChilling = true;
                    Stop();
                }
                else
                {
                    _currChillTime += Time.fixedDeltaTime;
                }
            }
        }
        else if (!_isWalking)
        {
            var xTarget = Random.Range(-walkingRadius,
                walkingRadius);
            var yTarget = Random.Range(-walkingRadius,
                walkingRadius);
            _targetPosition = new Vector2(xTarget, yTarget) * 0.97f;

            _isWalking = true;
        }

        else if (_isWalking)
        {
            _currNoChillTime += Time.fixedDeltaTime;
            if (Vector2.Distance((Vector2)centre.position, _startPosition) > walkingRadius)
            {
                Move(-_startPosition + (Vector2)centre.position);
                _isWalking = false;
                _startPosition = centre.position;
            }
            else if (Vector2.Distance(_targetPosition, (Vector2)centre.position - _startPosition) < targetPositionAccuracy)
            {
                Stop();
                _isWalking = false;
            }
            else
            {
                Debug.Log(_targetPosition);

                Move(_targetPosition);
            }

            FindTheWay();
        }
        Flip();

    }

    override protected void MoveAndAttack()
    {
        Attack();
        if (player != null)
        {
            if (((Vector2)centre.position - (Vector2)player.transform.position).magnitude <= evadeRadius)
            {
                Evade();
            }
        }
        else if (player == null) { }
        else if (((Vector2)centre.position - (Vector2)player.transform.position).magnitude > agrRadius)
        {
            Chill();
        }
    }

    override protected void Evade()
    {

        if (player != null)
        {
            if (((Vector2)centre.position - (Vector2)player.transform.position).magnitude <= evadeRadius)
            {
                _isWalking = true;
                Move((Vector2)(player.transform.position - centre.position) * _meleeMulti);
            }
            else
            {
                _isWalking = false;
                _startPosition = transform.position;
                centre.position = _startPosition;
            }


        }

    }

    override protected void FindTheWay()
    {

        var obstacles = Physics2D.CircleCastAll((Vector2)transform.position, walkingRadius, _targetPosition, evadeRadius / 2, maskToBypass);
        foreach (var obstacle in obstacles)
        {

            if (obstacle.collider != null && (obstacle.collider.transform.position - transform.position).magnitude < walkingRadius * 0.5f)
            {
                Move((Vector2)(obstacle.collider.transform.position - transform.position) * 1.5f);
                _isWalking = false;
            }
            else if (!_isWalking && obstacle.collider == null)
            {
                _startPosition = transform.position;
            }
        }

    }



    private void Move(Vector2 target)
    {
        if (_isWalking)
        {
            body.velocity = target.normalized * speed;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_startPosition, walkingRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, agrRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, evadeRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centre.position, walkingRadius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(_targetPosition + _startPosition, 0.25f);
    }

    private void Flip()
    {
        if (player.transform.position.x < transform.position.x && !_isFlipped)
        {
            _isFlipped = true;
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        else if (_isFlipped && player.transform.position.x > transform.position.x)
        {
            _isFlipped = false;
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

    }

    private void Stop()
    {
        body.velocity = Vector2.zero;
    }
}

