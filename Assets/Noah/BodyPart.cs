using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BodyPart : MonoBehaviour
{
    [SerializeField]
    bool stackable;
    [SerializeField]
    Vector3Int size = Vector3Int.one;

    public Body Body { get; private set; }
    public Vector3Int Coords { get; private set; }
    public Vector3Int Size { get; private set; }
    public BoxCollider Collider { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }

    public void Initialize(Body body, Vector3Int coords)
    {
        Body = body;
        Coords = coords;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, size);
    }
}