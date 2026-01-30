using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Body : MonoBehaviour
{
    [SerializeField]
    Vector3 slotScale;
    [SerializeField]
    Vector3Int size;
    [SerializeField]
    UnityEvent onPartAdded, onPartRemoved;

    Dictionary<Vector3Int, BodyPart> parts = new();

    public Vector3 SlotSize => new Vector3(slotScale.x * transform.localScale.x, slotScale.y * transform.localScale.y, slotScale.z * transform.localScale.z);

    public Matrix4x4 GetMatrix(Vector3Int coord) =>
         Matrix4x4.TRS(GetWorldPos(coord), Quaternion.identity, SlotSize * 0.25f);

    public Vector3 GetWorldPos(Vector3Int coord) =>
    new Vector3(coord.x * slotScale.x * transform.localScale.x,
            coord.y * slotScale.y * transform.localScale.y,
            coord.z * slotScale.z * transform.localScale.z) +
        transform.position -
        new Vector3(size.x * 0.5f, 0, size.z * 0.5f);

    public Vector3Int GetGridPos(Vector3 pos)
    {
        // Remove the position offset
        Vector3 localPos = pos - transform.position + new Vector3(size.x * 0.5f, 0, size.z * 0.5f);

        // Divide by slot scale and transform scale to get grid coordinates
        int x = Mathf.RoundToInt(localPos.x / (slotScale.x * transform.localScale.x));
        int y = Mathf.RoundToInt(localPos.y / (slotScale.y * transform.localScale.y));
        int z = Mathf.RoundToInt(localPos.z / (slotScale.z * transform.localScale.z));

        return new Vector3Int(x, y, z);
    }

    public BodyPart RemovePart(Vector3 pos)
    {
        Vector3Int coord = GetGridPos(pos);

        if (!parts.TryGetValue(coord, out BodyPart part))
            return null;



        return part;
    }

    public bool IsValid(Vector3Int coord) =>
        coord.x == Mathf.Clamp(coord.x, 0, size.x) &&
        coord.y == Mathf.Clamp(coord.y, 0, size.y) &&
        coord.z == Mathf.Clamp(coord.z, 0, size.z);
}
