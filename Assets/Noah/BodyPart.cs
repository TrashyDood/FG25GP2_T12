using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class BodyPart : MonoBehaviour
{
    [SerializeField]
    UnityEvent onInitialized;
    [SerializeField]
    Direction attachTo,
        attachFrom;
    [SerializeField]
    bool canTakeDamage = true;
    [SerializeField]
    Vector3Int size = Vector3Int.one;

    public AudioSource audioSource { get; private set; }

    BodyPart[] neighbours = new BodyPart[6];
    BodyPartData data;

    public Direction AttachTo => attachTo;
    public Direction AttachFrom => attachFrom;
    public Body Body { get; private set; }
    public Vector3Int Coords { get; private set; }
    public Vector3Int Size { get; private set; }
    public BoxCollider Collider { get; private set; }
    public WeaponDefinition Weapon { get; private set; }

    protected virtual void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
    }
    
    public void Initialize(Body body, WeaponDefinition weapon, Vector3Int coords, Direction direction)
    {
        Body = body;
        Coords = coords;
        transform.rotation = GetRotation(direction);
        onInitialized.Invoke();
    }

    void OnDataChanged(BodyPartData newData)
    {
        Body = newData.body;
        Coords = newData.coords;
        transform.rotation = GetRotation(newData.direction);
        onInitialized.Invoke();
    }

    public virtual void OnRemoved() { }

    public virtual void TakeDamage(float damage)
    {
        if (!canTakeDamage)
            return;

        Body.Player.Health -= damage;
    }

    static Quaternion GetRotation(Direction orientation)
    {
        return orientation switch
        {
            Direction.Forward => Quaternion.Euler(0, 0, 0),
            Direction.Right => Quaternion.Euler(0, 90, 0),
            Direction.Back => Quaternion.Euler(0, 180, 0),
            Direction.Left => Quaternion.Euler(0, -90, 0),
            Direction.Up => Quaternion.Euler(-90, 0, 0),
            Direction.Down => Quaternion.Euler(90, 0, 0),
            _ => Quaternion.identity
        };
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, size);
    }
#endif
}

public class BodyPartData
{
    public Body body;
    public Vector3Int coords;
    public Direction direction;
    public WeaponDefinition weapon;
}