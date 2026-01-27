using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class VoxelBodyTesting : MonoBehaviour
{
    [SerializeField]
    Material cubeMaterial,
        slotMaterial;
    [SerializeField]
    Mesh cubeMesh,
        slotMesh;
    [SerializeField]
    Vector3Int size;
    [Header("Placement")]
    [SerializeField]
    float maxDistance;
    [SerializeField]
    Camera camera;

    BodyCube[] bodyCubes;
    List<BodyCube> activeCubes = new();
    List<Matrix4x4> cubeMatrices = new(),
        slotMatrices = new();
    [SerializeField]
    List<Vector3Int> slots;

    RenderParams renderParams;

    void Start()
    {
        bodyCubes = new BodyCube[size.x * size.y * size.z];
        renderParams = new RenderParams(cubeMaterial)
        {
            worldBounds = new Bounds(Vector3.zero, Vector3.one * 1000),
            shadowCastingMode = ShadowCastingMode.On
        };

        AddCube(0, 0, 0);
        AddCube(1, 0, 0);
    }

    private void Update()
    {
        for (int i = 0; i < cubeMatrices.Count; ++i)
            cubeMatrices[i] = GetMatrix(activeCubes[i].coord + transform.position, transform.localScale);

        Render();
    }

    void Render()
    {
        // Render cubes
        for (int i = 0; i < cubeMatrices.Count; i += 1023)
            Graphics.RenderMeshInstanced(renderParams, cubeMesh, 0, cubeMatrices, Mathf.Min(1023, activeCubes.Count - i), i);

        // Render slots
        for (int i = 0; i < slotMatrices.Count; ++i)
            Graphics.RenderMeshInstanced(renderParams, slotMesh, 0, slotMatrices, Mathf.Min(1023, activeCubes.Count - i), i);
    }

    public void AddCube(Vector3Int coords) => AddCube(coords.x, coords.y, coords.z);

    public void AddCube(int x, int y, int z)
    {
        if (!IsValid(x, y, z)) return;

        int index = GetIndex(x, y, z);
        if (bodyCubes[index] != null)
            return;

        cubeMatrices.Add(new());
        activeCubes.Add(bodyCubes[index] = new()
        {
            coord = new(x, y, z),
        });

        //CalcSlots();
    }

    public void RemoveCube(int x, int y, int z)
    {
        int gridIndex = GetIndex(x, y, z);
        if (bodyCubes[gridIndex] == null)
            return;

        int listIndex = activeCubes.IndexOf(bodyCubes[gridIndex]);

        activeCubes.RemoveAt(listIndex);
        cubeMatrices.RemoveAt(listIndex);
        bodyCubes[gridIndex] = null;
        CalcSlots();
    }

    public BodyCube GetCube(int x, int y, int z)
    {
        int index = GetIndex(x, y, z);
        return IsValid(x, y, z) ? bodyCubes[index] : null;
    }

    int GetIndex(int x, int y, int z) =>
        x + y * size.x + z * size.x * size.y;

    bool IsValid(int x, int y, int z) =>
        x >= 0 && x < size.x && y >= 0 && y < size.y && z >= 0 && z < size.z;

    public void CalcSlots()
    {
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0),
            new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1)
        };

        slots.Clear();
        slotMatrices.Clear();

        foreach (var cube in activeCubes)
        {
            foreach (var dir in directions)
            {
                Vector3Int coord = cube.coord + dir;

                if (!visited.Contains(coord) &&
                    IsValid(coord.x, coord.y, coord.z) &&
                    GetCube(coord.x, coord.y, coord.z) == null)
                {
                    cubeMatrices.Add(GetMatrix(coord + transform.position, Vector3.one * 0.25f));
                    slots.Add(coord);
                    visited.Add(coord);
                }
            }
        }
    }

    Matrix4x4 GetMatrix(Vector3 position, Vector3 scale) =>
        Matrix4x4.TRS(position + transform.position, Quaternion.identity, scale);
}

public class BodyCube
{
    public Vector3Int coord;

    public BodyCube()
    {

    }
}