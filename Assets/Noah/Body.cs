using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Body : MonoBehaviour
{
    [SerializeField]
    Vector3Int size = Vector3Int.one;
    [SerializeField]
    LayerMask bodypartLayer;
    [SerializeField]
    BodyPart defaultPart;

    BodyPart[,,] body;
    List<BodyPart> parts = new();

    public BodyPart this[Vector3Int coords] =>
        body[coords.y, coords.x, coords.z];

    private void Awake()
    {
        body = new BodyPart[size.y - 1, size.x - 1, size.z - 1];
        AddPart(new(3, 0, 3));
    }

    public BodyPart AddPart(Vector3Int coords, BodyPart prefab = null)
    {
        BodyPart part = Instantiate(prefab ?? defaultPart, ToWorldPos(coords), transform.rotation, transform);

        part.Initialize(this, coords);

        return part;
    }

    public Vector3Int GetCursorCoords(Camera camera) => default;
    public Vector3Int ToCoords(Vector3 pos) => default;
    public Vector3 ToWorldPos(Vector3Int coords) => default;

    public bool IsValid(Vector3Int coords) =>
        coords.x < 0 || coords.x >= size.x || coords.x < 0 || coords.x >= size.x || coords.z < 0 || coords.z >= size.z;

    public void EnterEditMode(Action returnCallback, BodyPart part = null) =>
        StartCoroutine(EditModeCR(returnCallback, part));

    IEnumerator EditModeCR(Action returnCallback, BodyPart part)
    {
        part ??= defaultPart;

        while (!Input.GetMouseButtonDown(0))
        {

            yield return null;
        }

        returnCallback();
    }
}
