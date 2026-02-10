using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class Entity : MonoBehaviour
{
    [SerializeField]
    float maxHealth,
        currentHealth,
        damageCooldown;

    float damageTimer;

    [Header("Events")]
    [SerializeField]
    UnityEvent onDeathEvent;
    [SerializeField]
    UnityEvent<float, float> onHealthChangedEvent;

    public event Action<float, float> OnHealthChangedEvent;
    public event Action OnDeathEvent;

    public float MaxHealth => maxHealth;
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            if (OnHealthChanged(value))
                currentHealth = value;
        }
    }

    protected virtual void Awake()
    {
        CurrentHealth = maxHealth; // Initialize health
    }

    protected virtual void FixedUpdate()
    {
        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
    }

    protected virtual bool OnHealthChanged(float newHealth)
    {
        if (newHealth < currentHealth && damageTimer > 0) // I-Frames
            return false;

        onHealthChangedEvent.Invoke(newHealth, maxHealth);
        OnHealthChangedEvent?.Invoke(newHealth, maxHealth);

        if (Mathf.Round(newHealth) < 1)
            OnDeath();

        return true;
    }

    protected virtual void OnDeath()
    {
        onDeathEvent.Invoke();
        OnDeathEvent?.Invoke();
    }
}
