using UnityEngine;

public class WeaponMouth : MonoBehaviour
{
    float shootCooldownTimer = 0;
    public float shootCooldown = 0.5f;
    public GameObject projectilePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shootCooldownTimer -= Time.deltaTime;
        if (shootCooldownTimer <= 0)
        {
            shootCooldownTimer = shootCooldown;

            GameObject projectileInst = Instantiate(projectilePrefab, transform.position, new Quaternion(0,0,0,0));
            projectileInst.GetComponent<ProjectileMovement>().projectileDirection = transform.right;
        }
    }
}
