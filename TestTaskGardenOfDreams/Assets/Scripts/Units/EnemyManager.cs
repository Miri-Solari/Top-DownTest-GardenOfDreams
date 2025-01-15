using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    private GameObject[] _enemies;
    private GameObject _player;
    private GameObject _closest;



    private void Awake()
    {
        instance = this;
        StartCoroutine(Starter());

    }

    IEnumerator Starter()
    {
        yield return new WaitForEndOfFrame();
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public GameObject GivePlayerClosestTarget()
    {
        if (_player != null && _enemies.Length > 0) 
        {
            _closest = _enemies[0];
            foreach (var target in _enemies)
            {
                if ((target.transform.position - _player.transform.position).magnitude < 
                    (_closest.transform.position - _player.transform.position).magnitude)
                {
                    _closest = target;
                }
            }
            return _closest;
        }
        return null;
    }
}
