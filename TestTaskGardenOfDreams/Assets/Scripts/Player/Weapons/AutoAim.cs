using UnityEngine;

public class AutoAim : MonoBehaviour
{
    [SerializeField, Range(1, 10)] float aimRadius = 3f;
    [SerializeField, Range(0, 1)] float rotationSpeed = 0.25f;
    private GameObject _target;
    private Quaternion _pureRotation;

    private void FixedUpdate()
    {
        _target = EnemyManager.instance.GivePlayerClosestTarget();
        if (_target != null)
        {
            if ((_target.transform.position - transform.position).magnitude < aimRadius)
            {
                _pureRotation = Quaternion.LookRotation(_target.transform.position - transform.position, transform.up);
                _pureRotation.eulerAngles = new Vector3(0, 0, _pureRotation.eulerAngles.z);
                Debug.Log(Quaternion.Lerp(transform.rotation, _pureRotation, rotationSpeed).eulerAngles);
                transform.rotation = _pureRotation;

            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aimRadius);
    }
}
