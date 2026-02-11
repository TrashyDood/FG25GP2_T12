using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour
{
    [SerializeField]
    float maxHealth,
        currentHealth,
        damageCooldown;

    [Header("Events")]
    [SerializeField]
    UnityEvent onDeathEvent;
    [SerializeField]
    UnityEvent<float, float> onDamageEvent,
        onHealedEvent;

    float damageTimer;
    Rigidbody rigidbody;

    public event Action OnDeathEvent;
    public event Action<float, float> OnDamagedEvent,
        OnHealedEvent;

    public float MaxHealth => maxHealth;
    Rigidbody Rigidbody => rigidbody;

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
        rigidbody = GetComponent<Rigidbody>();

        CurrentHealth = maxHealth; // Initialize health
    }

    protected virtual void FixedUpdate()
    {
        if (damageTimer > 0)
            damageTimer -= Time.fixedDeltaTime;
    }

    protected virtual bool OnHealthChanged(float newHealth)
    {
        if (newHealth < currentHealth && damageTimer <= 0) // I-Frames
        {
            OnDamaged(newHealth);
            return true;
        }
        else if (newHealth > currentHealth)
        {
            OnHealed(newHealth);
            return true;
        }

        return false;
    }

    protected virtual void OnDamaged(float newHealth)
    {
        onDamageEvent.Invoke(newHealth, maxHealth);
        OnDamagedEvent?.Invoke(newHealth, maxHealth);
        damageTimer = damageCooldown;

        if (Mathf.Round(newHealth) < 1)
            OnDeath();
    }

    protected virtual void OnHealed(float newHealth)
    {
        onHealedEvent.Invoke(newHealth, maxHealth);
        OnHealedEvent?.Invoke(newHealth, maxHealth);
    }

    protected virtual void OnDeath()
    {
        onDeathEvent.Invoke();
        OnDeathEvent?.Invoke();
    }
}
