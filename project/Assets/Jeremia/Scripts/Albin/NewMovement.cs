using UnityEngine;

public class NewMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float accel = 10f;
    [SerializeField] private float decel = 10f;
    public float playerHealth;

    private Vector3 currentVel;
    private Vector3 targetVel;

    void Awake()
    {
        _rb ??= GetComponent<Rigidbody>();
    }

    void Update()
    {
        float x = 0f;
        float z = 0f;

        /*if (Keyboard.current.wKey.isPressed) z += 1;
        if (Keyboard.current.sKey.isPressed) z -= 1;
        if (Keyboard.current.dKey.isPressed) x += 1;
        if (Keyboard.current.aKey.isPressed) x -= 1;
        */

        x += Input.GetAxis("Horizontal");
        z += Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(x, 0, z).normalized;
        targetVel = dir * speed;
    }

    void FixedUpdate()
    {
        float rate = currentVel.sqrMagnitude > targetVel.sqrMagnitude ? decel : accel;
        currentVel = Vector3.MoveTowards(currentVel, targetVel, rate * Time.fixedDeltaTime);

        _rb.MovePosition(_rb.position + currentVel * Time.fixedDeltaTime);

        if (currentVel.sqrMagnitude > 0.001f)
            _rb.MoveRotation(Quaternion.LookRotation(currentVel));
    }
}