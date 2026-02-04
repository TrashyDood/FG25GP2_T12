using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class MobileTouch : MonoBehaviour
{
    [SerializeField]
    Vector2 joyStickSize = new Vector2(300, 300);
    [SerializeField]
    Image joyStick;

    Finger movementFinger;
    Vector2 movementAmount;

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
            Vector2 joyStickPosition;
            float maxMovement = joyStickSize.x / 2;
            ETouch.Touch currentTouch = movedFinger.currentTouch;

            if (Vector2.Distance(currentTouch.screenPosition, joyStick.rectTransform.anchoredPosition) < maxMovement)
            {
                joyStickPosition = (currentTouch.screenPosition - joyStick.rectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                joyStickPosition = currentTouch.screenPosition - joyStick.rectTransform.anchoredPosition;
            }

            movementAmount = joyStickPosition / maxMovement;
        }
    }
    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == movementFinger)
        {
            movementFinger = null;
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
}
