using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Range(0f, 2.5f)]
    [SerializeField]
    float followSmoothing;

    Transform followTarget;
    Vector3 followOffset;

    private void Start()
    {
        followTarget = GameManager.Instance.Player.transform;
        followOffset = transform.position - followTarget.position;
    }

    private void FixedUpdate()
    {
        if (followTarget != null)
            transform.position = Vector3.Lerp(transform.position,
                followTarget.position + followOffset,
                (1 / followSmoothing) * Time.fixedDeltaTime);
    }
}
