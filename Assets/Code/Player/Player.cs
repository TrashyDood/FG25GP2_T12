using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Body))]
public class Player : MonoBehaviour
{
    private float currentHealth;
    private float maxHealth = 100;

    [SerializeField]
    FillbarUI healthBar;

    public Body Body { get; private set; }

    public float Health
    {
        get => currentHealth;
        set => SetHealth(value);
    }

    private void Awake()
    {
        Body = GetComponent<Body>();
    }

    private void Start()
    {
        Health = maxHealth;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            currentHealth -= 20;
            SetHealth(currentHealth);
        }
    }
    public void SetHealth(float newValue)
    {
        currentHealth = newValue;
        healthBar.FillAmount = currentHealth / maxHealth;
    }
}
