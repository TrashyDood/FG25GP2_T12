using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float currentHealth;
    private float maxHealth = 100;

    [SerializeField] Slider healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        healthBar.value = currentHealth;
    }
}
