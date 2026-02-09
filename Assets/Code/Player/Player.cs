using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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

    [Header("Events")]
    [SerializeField]
    UnityEvent onDeathEvent;

    XPManager xpManager;
    PlayerInput input;
    Body body;

    public XPManager XPManager => xpManager;
    public PlayerBuild Build => build;
    public PlayerInput Input => input;
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
        xpManager = GetComponent<XPManager>();

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

        if (Mathf.Round(newValue) < 1)
        {
            GameManager.Instance.StateManager.SetState(GameState.Dead);
            input.enabled = false;
            onDeathEvent.Invoke();
        }
    }
}
