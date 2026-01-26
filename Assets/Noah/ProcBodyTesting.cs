using System;
using System.Collections;
using System.Collections.Generic;
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
            matrices[i] = body.Matrix(body.segments[i]);

        Graphics.DrawMeshInstanced(mesh, 0, material, matrices);
    }
}

[Serializable]
public class ProcBody
{
    [Header("Info")]
    public int centerIndex;
    public SegmentInfo[] segments;

    [Header("Config")]
    public bool constrainDistance = true;
    public bool constrainAngle = true;

    Func<int, float> massProfile;

    public SegmentInfo Center => segments[centerIndex];

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
        if (segments[0].position != newPosition)
        {
            segments[0].position = newPosition;
            segments[0].direction = segments[0].position - newPosition;
        }

        for (int i = centerIndex + 1; i < segments.Length; i++)
            segments[i] = segments[i].Constrain(segments[i - 1]);
    }

    public Matrix4x4 Matrix(SegmentInfo segment) =>
        Matrix4x4.TRS(segment.position, Quaternion.LookRotation(segment.position + segment.direction), Vector3.one);

    [Serializable]
    public struct SegmentInfo
    {
        public Vector3 position;
        public Vector3 direction;
        public float angle;

        public SegmentInfo(Vector3 position = new(), Vector3 direction = new(), float angle = 0)
        {
            this.position = position;
            this.direction = direction;
            this.angle = angle;
        }

        public SegmentInfo Constrain(in SegmentInfo parent, in bool constrainDistance = true, in float distance = 1, in bool constrainAngle = true, in float maxAngle = 90)
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
}
