using UnityEngine;

public class CameraOnPlayer : MonoBehaviour
{
    [SerializeField] Transform player; // ������ �� ������ ������
    [SerializeField, Range(0, 0.150f)] float smoothSpeed = 0.125f; // �������� ����������� ��������
    private Vector3 _offset; // �������� ������ ������������ ������

    void LateUpdate()
    {
        if (player != null)
        {
            // ��������� ������� �������
            Vector3 targetPosition = player.position + _offset;

            // ������� �������� ������
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    private void Awake()
    {
        _offset = transform.position - player.position;
    }
}
