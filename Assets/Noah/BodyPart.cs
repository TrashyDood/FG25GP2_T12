using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BodyPart : MonoBehaviour
{
    [SerializeField]
    Direction attachTo,
        attachFrom;
    [SerializeField]
    Vector3Int size = Vector3Int.one;

    BodyPart[] neighbours = new BodyPart[6];

    public Direction AttachTo => attachTo;
    public Direction AttachFrom => attachFrom;
    public Body Body { get; private set; }
    public Vector3Int Coords { get; private set; }
    public Vector3Int Size { get; private set; }
    public BoxCollider Collider { get; private set; }

    protected virtual void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }

    public virtual void Initialize(Body body, Vector3Int coords, Direction orientation)
    {
        Body = body;
        Coords = coords;
        transform.rotation = GetRotation(orientation);
    }

    public virtual void OnRemoved() { }

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, size);
    }
}