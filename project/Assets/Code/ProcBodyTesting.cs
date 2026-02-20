using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProcBodyTesting : MonoBehaviour
{
    [SerializeField]
    AnimationCurve massProfile;
    [SerializeField]
    ProcBody body;

    [Header("Visuals")]
    [SerializeField]
    Mesh mesh;
    [SerializeField]
    Material material;

    private void Start()
    {
        body.MassProfile = (i) => massProfile.Evaluate(i);
    }

    private void Update()
    {
        body.Evaluate(transform.position);
        RenderBody();
    }

    void RenderBody()
    {
        Matrix4x4[] matrices = new Matrix4x4[body.segments.Length];

        for (int i = 0; i < matrices.Length; i++)
            matrices[i] = body.segments[i].Matrix;

        Graphics.DrawMeshInstanced(mesh, 0, material, matrices);
    }
}

[Serializable]
public class ProcBody
{
    [Header("Info")]
    public int centerIndex;
    public Segment[] segments;

    Func<int, float> massProfile;

    public Segment Center => segments[centerIndex];

    public Func<int, float> MassProfile
    {
        get => massProfile;
        set
        {
            massProfile = value;
        }
    }

    public void Evaluate(Vector3 newPosition)
    {
        if (segments[0].Data.position != newPosition)
            segments[0].Data = new(newPosition, newPosition - segments[0].Data.position);

        for (int i = 1; i < segments.Length; i++)
            segments[i].Evaluate(segments[i - 1]);
    }
}

[Serializable]
public class Segment
{
    [SerializeField]
    SegmentData data;
    [SerializeField]
    float distance = 1,
        maxAngle = 90,
        radius = 1,
        smoothing = 0;

    public Matrix4x4 Matrix => Matrix4x4.TRS(data.position, Quaternion.LookRotation(data.position + data.direction), Vector3.one * radius);
    public SegmentData Data { get => data; set => data = value; }

    public virtual void Evaluate(Segment parent)
    {
        data = data.ConstrainChain(parent.data, true, distance, true, maxAngle);
    }
}

[Serializable]
public struct SegmentData
{
    public Vector3 position,
        direction;
    public float angle;

    public SegmentData(Vector3 position = new(), Vector3 direction = new(), float angle = 0)
    {
        this.position = position;
        this.direction = direction;
        this.angle = angle;
    }

    public SegmentData ConstrainChain(SegmentData parent, bool constrainDistance = true, float distance = 1, bool constrainAngle = true, float maxAngle = 90)
    {
        Vector3 newPosition = position;
        Vector3 newDirection = position - parent.position;
        float newAngle = angle;

        // Calculate new direction if parent is far away.
        if (newDirection.sqrMagnitude < 0.0001f)
            newDirection = direction;

        // Constrain angle
        if (constrainAngle && maxAngle > 0.001f && maxAngle < 179.999f)
        {
            newAngle = Vector3.Angle(parent.direction, newDirection);
            if (newAngle > maxAngle)
                newDirection = Vector3.Slerp(parent.direction, newDirection, maxAngle / newAngle);
        }

        // Constrain distance
        if (constrainDistance)
        {
            newDirection.Normalize();
            newPosition = parent.position + newDirection * distance;
        }

        return new(newPosition, newDirection, newAngle);
    }
}