using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    float speed = 10,
          acceleration = 50,
          deceleration = 80;
    [SerializeField]
    PlayerInput playerInput;

    Rigidbody rigidBody;
    Vector2 input;
    Vector3 currentVelocity,
            targetVelocity;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public float Acceleration
    {
        get => acceleration;
        set => acceleration = value;
    }

    public float Deceleration
    {
        get => deceleration;
        set => deceleration = value;
    }

    private void Awake()
    {
        rigidBody ??= GetComponent<Rigidbody>();
        playerInput ??= GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        currentVelocity = Vector3.MoveTowards(currentVelocity,
            targetVelocity,
            ((currentVelocity.sqrMagnitude > targetVelocity.sqrMagnitude) ? deceleration : acceleration) * Time.fixedDeltaTime);

        rigidBody.Move(transform.position + currentVelocity * Time.fixedDeltaTime, Quaternion.LookRotation(currentVelocity + transform.position));
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        targetVelocity = new Vector3(input.x, 0, input.y) * speed;
    }
}
