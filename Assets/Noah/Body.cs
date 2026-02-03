using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Body : MonoBehaviour
{
    [SerializeField]
    Vector3Int size;
    [SerializeField]
    LayerMask partMask;
    [SerializeField]
    BodyPart defaultPart;

    BodyPart[,,] body;
    List<BodyPart> parts;

    public BodyPart this[Vector3Int coords] =>
        body[coords.y, coords.x, coords.z];

    private void Awake()
    {
        
    }

    public void AddPart(Vector3Int coords, BodyPart part)
    {

    }

    public Vector3 ToWorldPos(Vector3Int coords) => default;
    public Vector3Int ToCoords(Vector3 pos) => default;

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
