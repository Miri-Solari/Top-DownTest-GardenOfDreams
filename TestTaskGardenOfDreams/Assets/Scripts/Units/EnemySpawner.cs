using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;


    private void Awake()
    {
        Instance = this;
    }

    public void RandomSpawn(int number)
    {
        for (int x = 0; x < number; x++)
        {

        }
    }
}
