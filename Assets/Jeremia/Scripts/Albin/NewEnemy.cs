using UnityEngine;

public class NewEnemy : MonoBehaviour
{
    [SerializeField] GameObject expGlobe;

    public NewEnemyMovement parentMovement;
    public float enemyHealth = 100f;   // starting value, adjust as needed
    public float enemyDamage = 1f;


    private void Start()
    {
        parentMovement = GetComponentInParent<NewEnemyMovement>();
    }
    
    private void Update()
    {
        if (enemyHealth <= 0f)
        {
            OnDeath();
        }
    }

    private void OnTriggerStay(Collider other)
{
    if (other.CompareTag("Player"))
    {
        // Tell movement to stop
        if (parentMovement != null)
            parentMovement.stop = true;

        // Damage player
        PlayerMeters ph = other.GetComponent<PlayerMeters>();
        if (ph != null)
        {
            ph.playerHealth -= enemyDamage * Time.deltaTime;
        }
    }

    if (other.CompareTag("PDamage"))
    {
        // Tell movement to stop
        if (parentMovement != null)
            parentMovement.stop = true;
        
        // Damage player
        NewPDamage pd = other.GetComponent<NewPDamage>();
        if (pd != null)
            {
                enemyHealth -= pd.damagePerFrame * Time.deltaTime;
                if (pd.destroyOnCollision) {Destroy(pd.gameObject);}
            }
    }
}

private void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Player"))
    {
        if (parentMovement != null)
            parentMovement.stop = false;
    }
}

    private void OnDeath()
    {
        Instantiate(expGlobe, transform.position, Quaternion.identity);
        Destroy(transform.parent.gameObject);
    }
}