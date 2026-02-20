using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]   
    float acceleration = 50,
            deceleration = 80;
    [SerializeField]
    PlayerInput playerInput;
    [SerializeField]
    Player player;

    [Header("Events")]
    [SerializeField]
    UnityEvent onMoveEvent;
    [SerializeField]
    UnityEvent onStillEvent;

    bool isMoving;
    Rigidbody rigidBody;
    Vector2 input;
    Vector3 currentVelocity,
            targetVelocity;

    public event Action OnMoveEvent,
        OnStillEvent;

    public Vector2 InputVector
    {
        get => input;
        set
        {
            input = value;
            targetVelocity = new Vector3(input.x, 0, input.y) * Speed;
        }
    }

    public float Speed => player.Build?.stats.GetStat(StatType.Speed) ?? 10f;

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
        player = GetComponent<Player>();
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        if (!isMoving && currentVelocity.sqrMagnitude > 0)
        {
            isMoving = true;
            onMoveEvent.Invoke();
            OnMoveEvent?.Invoke();
        }
        else if (isMoving && currentVelocity.sqrMagnitude == 0)
        {
            isMoving = false;
            onStillEvent.Invoke();
            OnStillEvent?.Invoke();
        }

        currentVelocity = Vector3.MoveTowards(currentVelocity,
            targetVelocity,
            ((currentVelocity.sqrMagnitude > targetVelocity.sqrMagnitude) ? deceleration : acceleration) * Time.fixedDeltaTime);

        rigidBody.Move(transform.position + currentVelocity * Time.fixedDeltaTime, Quaternion.identity);
    }
}
