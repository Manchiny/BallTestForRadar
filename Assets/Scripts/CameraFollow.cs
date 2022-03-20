using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;

    private Transform _target;
    private float _speed = 15f;

    private Vector3 _targetPosition;
    private Vector3 _smoothedPosition;

    private bool IsInited;
    private void FixedUpdate()
    {
        if (!IsInited)
            return;

        UpdateCameraPosition();
    }

    public void Init(Transform ball)
    {
        _target = ball;
        IsInited = true;
    }

    public void UpdateCameraPosition()
    {
        _targetPosition = new Vector3(0, _target.position.y, _target.position.z) + _offset;
        _smoothedPosition = Vector3.Lerp(transform.position, _targetPosition, _speed * Time.fixedDeltaTime);
        transform.position = _smoothedPosition;
    }
}


