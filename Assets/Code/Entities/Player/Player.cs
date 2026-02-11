using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Body), typeof(PlayerInput))]
public class Player : Entity
{
    [SerializeField]
    PlayerBuild build;

    XPManager xpManager;
    PlayerInput input;
    Body body;
    PlayerMovement playerMovement;

    [SerializeField]
    WeaponLibrary weaponLibrary;

    [SerializeField]
    string startingWeaponId = "Eye_Weapon";

    public PlayerMovement PlayerMovement => playerMovement;
    public XPManager XPManager => xpManager;
    public PlayerBuild Build => build;
    public PlayerInput Input => input;
    public Body Body => body;

    protected override void Awake()
    {
        base.Awake();

        body = GetComponent<Body>();
        input = GetComponent<PlayerInput>();
        xpManager = GetComponent<XPManager>();
        playerMovement = GetComponent<PlayerMovement>();

        build = new PlayerBuild();
        build.stats = new StatCollection();
        build.stats.SetBaseStat(StatType.Speed, 10f);

        build.AddWeaponById(startingWeaponId, weaponLibrary);

        // Grab the weapon instance that was just added
        WeaponInstance instance = build.weapons.Find(
            w => w.definition.weaponId == startingWeaponId
        );

        if (instance != null && body?.CentralPart != null)
        {
            instance.weaponObject = body.CentralPart;
            Debug.Log("[Player] Default weapon bound to CentralPart");
        }
        else
        {
            Debug.LogWarning("[Player] Failed to bind default weapon");
        }
    }

    protected override void OnDamaged(float newHealth)
    {
        base.OnDamaged(newHealth);

        GameManager.Instance.PlayerCamera.Shake();
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        GameManager.Instance.StateManager.SetState(GameState.Dead);
        input.enabled = false;
    }
}
