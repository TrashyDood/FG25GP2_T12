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

        build = new PlayerBuild();
        build.stats = new StatCollection();
        build.stats.SetBaseStat(StatType.Speed, 10f);
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        GameManager.Instance.StateManager.SetState(GameState.Dead);
        input.enabled = false;
    }
}
