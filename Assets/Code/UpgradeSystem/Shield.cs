using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{
    [Header("Shield Stats")]
    public int maxHP = 100;
    public int currentHP;
    public float regenDelay = 5f;
    public float regenRate = 10f;
    public float blockAngle = 90f; 


    private bool isRegenerating = false;
    private Transform playerTransform;

    private void Awake()
    {
        currentHP = maxHP;
        playerTransform = transform.parent;
    }

    private void Update()
    {
        if (isRegenerating)
        {
            Regenerate();
        }
    }

    public void TakeDamage(int dmg, Vector3 hitDirection)
    {
        if (!IsBlocking(hitDirection))
            return;

        currentHP -= dmg;
        if (currentHP <= 0)
        {
            currentHP = 0;
            DeactivateShield();
            StartCoroutine(StartRegen());
        }
    }

    private bool IsBlocking(Vector3 hitDirection)
    {
        Vector3 forward = playerTransform.forward;
        float angle = Vector3.Angle(forward, hitDirection);
        return angle <= blockAngle / 2f;
    }

    private void DeactivateShield()
    {
        gameObject.SetActive(false);
        isRegenerating = true;
    }

    private IEnumerator StartRegen()
    {
        yield return new WaitForSeconds(regenDelay);
        isRegenerating = false;
        gameObject.SetActive(true);
        currentHP = 0;
    }

    private void Regenerate()
    {
        currentHP += Mathf.CeilToInt(regenRate * Time.deltaTime);
        if (currentHP >= maxHP)
        {
            currentHP = maxHP;
            isRegenerating = false;
        }
    }
}
