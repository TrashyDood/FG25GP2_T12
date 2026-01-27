using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VoxelBodyTesting : MonoBehaviour
{
    [SerializeField]
    Material material;
    [SerializeField]
    Mesh mesh;
    [SerializeField]
    Vector3Int size;

    BodyCube[] bodyCubes;
    List<BodyCube> activeCubes = new();
    List<Matrix4x4> activeMatrices = new();

    RenderParams renderParams;

    void Start()
    {
        bodyCubes = new BodyCube[size.x * size.y * size.z];
        renderParams = new RenderParams(material)
        {
            worldBounds = new Bounds(Vector3.zero, Vector3.one * 1000),
            shadowCastingMode = ShadowCastingMode.On
        };

        AddCube(0, 0, 0);
        AddCube(1, 0, 0);
    }

    private void Update()
    {
        UpdateMatrices();
        RenderBody();
    }

    void RenderBody()
    {
        // DrawMeshInstanced has a limit of 1023 instances per call
        for (int i = 0; i < activeCubes.Count; i += 1023)
            Graphics.RenderMeshInstanced(renderParams, mesh, 0, activeMatrices, Mathf.Min(1023, activeCubes.Count - i));
    }

    void UpdateMatrices()
    {
        for (int i = 0; i < activeMatrices.Count; ++i)
            activeMatrices[i] = Matrix4x4.TRS(activeCubes[i].coord + transform.position, Quaternion.identity, transform.localScale);
    }

    public void AddCube(Vector3Int coords) => AddCube(coords.x, coords.y, coords.z);

    public void AddCube(int x, int y, int z)
    {
        int index = GetIndex(x, y, z);

        if (bodyCubes[index] != null)
            return;

        activeMatrices.Add(new());
        activeCubes.Add(bodyCubes[index] = new()
        {
            coord = new(x, y, z),
            index = index
        });
    }

    public void RemoveCube(int x, int y, int z)
    {
        int index = GetIndex(x, y, z);

        if (bodyCubes[index] == null)
            return;

        activeMatrices.RemoveAt(index);
        activeCubes.RemoveAt(index);
    }

    int GetIndex(int x, int y, int z) =>
        x + y * size.x + z * size.x * size.y;
}

[Serializable]
public class Body
{

}

public class BodyCube
{
    public Vector3Int coord;
    public int index;

    public BodyCube()
    {

    }
}