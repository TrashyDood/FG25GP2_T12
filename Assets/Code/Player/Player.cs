using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;

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

    [SerializeField]
    UnityEvent onDeathEvent;

    XPManager xpManager;
    PlayerInput input;
    Body body;

    [SerializeField] WeaponLibrary weaponLibrary;
    [SerializeField] string startingWeaponId = "Eye_Weapon";
    [SerializeField] private BodyPart startingWeaponObject;

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

        CreateNewBuild();
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

    public void CreateNewBuild()
    {
        build = new PlayerBuild();

        build.stats = new StatCollection();
        build.stats.SetBaseStat(StatType.Speed, 10f);

        build.weapons = new List<WeaponInstance>();

        AddStartingWeapon();
    }

    public void AddStartingWeapon()
    {
        if (weaponLibrary == null) return;

        WeaponDefinition weaponDef = weaponLibrary.GetWeapon(startingWeaponId);
        if (weaponDef == null) return;

        // Use the central part as the weapon object
        BodyPart centralWeaponPart = Body.CentralPart;

        WeaponInstance instance = new WeaponInstance
        {
            definition = weaponDef,
            level = 1,
            currentStats = weaponDef.baseStats.Clone(),
            cooldownTimer = 0f,
            weaponObject = centralWeaponPart // assign the existing central cube
        };

        build.weapons.Add(instance);

        Debug.Log($"Starting weapon '{weaponDef.weaponName}' added to build. FirePoint: {instance.FirePoint.position}");
    }


}
