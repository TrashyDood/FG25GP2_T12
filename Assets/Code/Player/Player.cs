using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Body))]
public class Player : MonoBehaviour
{
    [SerializeField]
    float currentHealth,
        maxHealth = 100;

    [SerializeField]
    FillbarUI healthBar;

    public Body Body { get; private set; }
    public float MaxHealth => maxHealth;
    public float Health
    {
        get => currentHealth;
        set
        {
            OnHealthChanged(value);
            currentHealth = value;
        }
    }

    private void Awake()
    {
        Body = GetComponent<Body>();
    }

    private void Start()
    {
        Health = maxHealth;
    }

    void OnHealthChanged(float newValue)
    {
        currentHealth = newValue;
        healthBar.FillAmount = currentHealth / maxHealth;

        if (Mathf.Round(newValue) < 1)
        {
            GameManager.Instance.StateManager.SetState(GameState.Dead);
        }
    }
}
