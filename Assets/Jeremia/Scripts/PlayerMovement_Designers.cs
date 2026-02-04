using UnityEngine;

public class PlayerMovement_Designers : MonoBehaviour
{
    public float baseSpeed = 0;
    public float legMassMult;
    Vector3 moveDirection = Vector3.zero;

    Rigidbody rb;
    BodyPlacing bodyPlacing;
    PlayerMeters playerMeters;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bodyPlacing = GetComponent<BodyPlacing>();
        playerMeters = GetComponent<PlayerMeters>();
    }

    void Update()
    {
        moveDirection = Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));
        rb.linearVelocity = moveDirection * (baseSpeed * (bodyPlacing.legCount*legMassMult-(legMassMult-1))/(bodyPlacing.massCount));
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Damage player
            playerMeters.playerHealth -= other.gameObject.GetComponent<NewEnemy>().enemyDamage * Time.deltaTime;
        }
        else if (other.CompareTag("XP"))
        {
            playerMeters.playerXP += 10;
            Destroy(other.gameObject);
        }
    }
}
