using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Shooting Behaviours/Snake Bite")]
public class SnakeBite : ShootingBehaviour
{
    public float maxRange = 15f;
    public float attackSpeed;
    private float attackTimer;
    [SerializeField]
    float forwardOffset;
    [SerializeField]
    float capsuleRadius;
    [SerializeField]
    LayerMask enemyLayer;

    public override void Shoot(WeaponInstance weapon, Transform firePoint)
    {
        Vector3 start = firePoint.transform.position + firePoint.forward * forwardOffset;
        Vector3 end = start * maxRange;

        Collider[] enemyColliders = Physics.OverlapCapsule(start, end, capsuleRadius, enemyLayer);

        foreach (Collider enemyHit in enemyColliders)
        {
            if (enemyHit.TryGetComponent(out Enemy enemy))
            {
                //Deal damage
                //play snake animation (lunging or something)
                Debug.Log("Something is in my OverlapCapsule");
            }
        }
    }
}
