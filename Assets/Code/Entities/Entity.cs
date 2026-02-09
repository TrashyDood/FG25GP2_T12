using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    [SerializeField]
    float maxHealth,
        currentHealth;

    [Header("Events")]
    [SerializeField]
    UnityEvent onDeathEvent;
    [SerializeField]
    UnityEvent<float, float, float> onHealthChangedEvent;

    public event Action<float, float, float> OnHealthChangedEvent;
    public event Action OnDeathEvent;

    public float MaxHealth => maxHealth;
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            OnHealthChanged(value);
            currentHealth = value;
        }
    }

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    protected virtual void OnHealthChanged(float newHealth)
    {
        onHealthChangedEvent.Invoke(newHealth, currentHealth, maxHealth);
        OnHealthChangedEvent?.Invoke(newHealth, currentHealth, maxHealth);

        if (Mathf.Round(newHealth) < 1)
            OnDeath();
    }

    protected virtual void OnDeath()
    {
        onDeathEvent.Invoke();
        OnDeathEvent?.Invoke();
    }
}
