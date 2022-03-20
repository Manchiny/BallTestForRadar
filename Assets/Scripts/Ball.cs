using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const string GAME_ITEMS_TAG = "GameItem";
    private const string BORDERS_TAG = "Border";
    private const int INPUT_CHECKER_LAYER = 10;
    private const float SPEED_UP_TIME = 2f;

    [SerializeField] private float _startSpeed = 7f;

    private Rigidbody _rigidbody;
    private Camera _camera;

    private float _currentSpeed;
    public float CurrentSpeed
    {
        get => _currentSpeed;
        set
        {
            _currentSpeed = value;
            OnCurrenSpeedChanged?.Invoke(_currentSpeed);
        }
    }

    private float _deltaX = 0f;

    public Action<GameItem> OnGameItemContact;
    public Action OnBorderContact;
    public Action<float> OnCurrenSpeedChanged;

    private void Start()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        CurrentSpeed = _startSpeed;

        _rigidbody.velocity = transform.forward * CurrentSpeed;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) == true)
        {
            GetDeltaXFromRay();
        }
        else
        {
            _deltaX = 0;
        }
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        Vector3 target = Vector3.forward;
        target.x = _deltaX;
        _rigidbody.velocity = target * CurrentSpeed;
    }

    private void GetDeltaXFromRay()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100f, 1 << INPUT_CHECKER_LAYER);

        float tempTargetX = hit.point.x;

        _deltaX = tempTargetX - transform.position.x;
    }

    private void OnTriggerEnter(Collider other)
    {      
        if (other.gameObject.tag == GAME_ITEMS_TAG)
        {
            GameItem item = other.gameObject.GetComponent<GameItem>();
            OnGameItemContact?.Invoke(item);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == BORDERS_TAG)
        {          
            OnBorderContact?.Invoke();
        }
        else if (collision.gameObject.tag == GAME_ITEMS_TAG)
        {
            GameItem item = collision.gameObject.GetComponent<GameItem>();
            OnGameItemContact?.Invoke(item);
        }
    }

    public void SpeedUp(float multiplier)
    {
        float targetSpeed = _startSpeed * multiplier;
        StopAllCoroutines();
        StartCoroutine (IncreaseSpeed(CurrentSpeed, targetSpeed));
    }

    private IEnumerator IncreaseSpeed(float speed, float targetSpeed)
    {
        float acceleration = (targetSpeed - speed) / SPEED_UP_TIME;
        while(CurrentSpeed < targetSpeed)
        {
            CurrentSpeed += acceleration * Time.deltaTime;
            yield return null;
        }

        if(CurrentSpeed >= targetSpeed)
        {
            CurrentSpeed = targetSpeed;
            yield break;
        }
    }

    public void StopBall()
    {
        StopAllCoroutines();

        _currentSpeed = 0;
    }

    public void Restart(Vector3 spawnPoint)
    {
        transform.position = spawnPoint;

        _deltaX = 0;
        _currentSpeed = _startSpeed;    
    }
}
