using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    Transform _followTarget;
    [Range(0f, 2.5f)]
    [SerializeField]
    float _followSmoothing;

    Vector3 _followOffset;

    private void Awake()
    {
        _followOffset =  transform.position - _followTarget.position;
    }

    private void FixedUpdate()
    {
        if (_followTarget != null)
            transform.position = Vector3.Lerp(transform.position, _followTarget.position + _followOffset, (1 / _followSmoothing) * Time.fixedDeltaTime);
    }
}
