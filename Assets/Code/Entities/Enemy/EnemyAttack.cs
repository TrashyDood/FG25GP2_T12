using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private float radiusLength;
    [SerializeField]
    private LayerMask playerLayer;
    private void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radiusLength, playerLayer);
        if (hits.Length > 0)
        {
            Player player = hits[0].GetComponent<Player>();
            if (player != null)
            {
                Debug.Log("I hit the player");
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusLength);
    }
}
