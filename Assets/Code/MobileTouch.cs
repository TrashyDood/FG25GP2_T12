using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class MobileTouch : MonoBehaviour
{
    [Header("Joystick")]
    [SerializeField]
    Vector2 joyStickSize = new Vector2(300, 300);
    [SerializeField]
    FloatingJoystick joyStick;

    Finger movementFinger;
    Vector2 movementAmount;

    [Header("Player movement")]
    [SerializeField]
    float acceleration = 50;
    [SerializeField]
    float deceleration = 80;
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

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }
    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }
    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
            Vector3 knobPosition;
            float maxMovement = joyStickSize.x / 2;
            ETouch.Touch currentTouch = movedFinger.currentTouch;

            if (Vector2.Distance(
                    currentTouch.screenPosition, 
                    joyStick.rectTransform.anchoredPosition
                    ) > maxMovement)
            {
                knobPosition = (
                    currentTouch.screenPosition - joyStick.rectTransform.anchoredPosition
                    ).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - joyStick.rectTransform.anchoredPosition;
            }

            joyStick.knob.anchoredPosition = knobPosition;
            movementAmount = knobPosition / maxMovement;
        }
    }
    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == movementFinger)
        {
            movementFinger = null;
            joyStick.knob.anchoredPosition = Vector2.zero;
            joyStick.gameObject.SetActive(false);
            movementAmount = Vector2.zero;
        }
    }
    private void HandleFingerDown(Finger touchedFinger)
    {
        if (movementFinger == null && GameManager.Instance.StateManager.CurrentIndex == 0)
        {
            movementFinger = touchedFinger;
            movementAmount = Vector2.zero;
            joyStick.gameObject.SetActive(true);
            joyStick.rectTransform.sizeDelta = joyStickSize;
            joyStick.rectTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        if (startPosition.x < joyStickSize.x / 2)
        {
            startPosition.x = joyStickSize.x / 2;
        }

        if (startPosition.y < joyStickSize.y / 2)
        {
            startPosition.y = joyStickSize.y / 2;
        }
        else if (startPosition.y > Screen.height - joyStickSize.y / 2)
        {
            startPosition.y = Screen.height - joyStickSize.y / 2;
        }

        return startPosition;
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

        if (movementAmount.sqrMagnitude > 0.001f)
        {
            Vector3 scaledMovement = 10f * Time.fixedDeltaTime * new Vector3(
                movementAmount.x,
                0,
                movementAmount.y
            );

            rigidBody.MovePosition(rigidBody.position + scaledMovement);
        }

        // change the 10f in scaledMovement with a float player movespeed :) (pray that is works man, otherwise im ending it all)
        /*    
        currentVelocity = Vector3.MoveTowards(currentVelocity,
            targetVelocity,
            ((currentVelocity.sqrMagnitude > targetVelocity.sqrMagnitude) ? deceleration : acceleration) * Time.fixedDeltaTime);
        rigidBody.Move(transform.position + currentVelocity * Time.fixedDeltaTime, Quaternion.identity);
        */
    }
    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        targetVelocity = new Vector3(input.x, 0, input.y) * Speed;
    }
}
