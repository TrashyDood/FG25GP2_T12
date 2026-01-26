using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    Rigidbody _rigidBody;
    [SerializeField]
    float _speed,
          _acceleration,
          _deceleration;

    Vector2 _input;
    Vector3 _currentVelocity,
            _targetVelocity;

    private void Awake()
    {
        _rigidBody ??= GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _currentVelocity = Vector3.MoveTowards(_currentVelocity,
            _targetVelocity,
            ((_currentVelocity.sqrMagnitude > _targetVelocity.sqrMagnitude) ? _deceleration : _acceleration) * Time.fixedDeltaTime);

        _rigidBody.Move(transform.position + _currentVelocity * Time.fixedDeltaTime, Quaternion.LookRotation(_currentVelocity + transform.position));
    }

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public float Acceleration
    {
        get => _acceleration;
        set => _acceleration = value;
    }

    public float Deceleration
    {
        get => _deceleration;
        set => _deceleration = value;
    }

    public void OnMove(InputValue value)
    {
        Debug.Log("OnMove");
        _input = value.Get<Vector2>();
        _targetVelocity = new Vector3(_input.x, 0, _input.y) * _speed;
    }
}
