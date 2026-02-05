using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Body), typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [SerializeField]
    float currentHealth,
        maxHealth = 100;
    [SerializeField]
    FillbarUI healthBar;
    [SerializeField]
    PlayerBuild build;

    PlayerInput input;
    Body body;

    public Body Body => body;
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
        body = GetComponent<Body>();
        input = GetComponent<PlayerInput>();

        build = new PlayerBuild();
        build.stats = new StatCollection();
        build.stats.SetBaseStat(StatType.Speed, 10f);
    }

    private void Start()
    {
        Health = maxHealth;
    }

    void OnHealthChanged(float newValue)
    {
        healthBar.FillAmount = newValue / maxHealth;

        Debug.Log(healthBar.FillAmount);

        if (Mathf.Round(newValue) < 1)
        {
            GameManager.Instance.StateManager.SetState(GameState.Dead);
            
        }
    }
}
