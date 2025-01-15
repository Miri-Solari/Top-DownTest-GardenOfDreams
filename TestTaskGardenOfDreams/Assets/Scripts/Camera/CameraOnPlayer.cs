using UnityEngine;

public class CameraOnPlayer : MonoBehaviour
{
    [SerializeField] Transform player; // Ссылка на объект игрока
    [SerializeField, Range(0, 0.150f)] float smoothSpeed = 0.125f; // Скорость сглаживания движения
    private Vector3 _offset; // Смещение камеры относительно игрока

    void LateUpdate()
    {
        if (player != null)
        {
            // Вычисляем целевую позицию
            Vector3 targetPosition = player.position + _offset;

            // Плавное движение камеры
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    private void Awake()
    {
        _offset = transform.position - player.position;
    }
}
