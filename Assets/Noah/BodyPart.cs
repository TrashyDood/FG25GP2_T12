using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BodyPart : MonoBehaviour
{
    [SerializeField]
    bool stackable;

    public Body Body { get; private set; }
    public Vector3Int Coords { get; private set; }
    public BoxCollider Collider { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }

    public void Initialize(Body body, Vector3Int coords)
    {
        Body = body;
        Coords = coords;

        transform.parent = body.transform;
    }

    private void OnDrawGizmos()
    {
        
    }
}