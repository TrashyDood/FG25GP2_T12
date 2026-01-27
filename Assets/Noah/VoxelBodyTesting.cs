using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VoxelBodyTesting : MonoBehaviour
{
    [SerializeField]
    Vector3Int size;
    [SerializeField]
    Vector3 slotScale = Vector3.one;

    [Header("Rendering")]
    [SerializeField]
    Material cubeMaterial;
    [SerializeField]
    Material slotMaterial;
    [SerializeField]
    Mesh cubeMesh,
        slotMesh;
    [SerializeField]
    bool renderSlots;

    BodyCube[] bodyCubes;
    List<BodyCube> activeCubes = new();
    List<Vector3Int> slots = new();
    List<Matrix4x4> cubeMatrices = new(),
        slotMatrices = new();

    RenderParams cuberRenderParams,
        slotRenderParams;

    public bool RenderSlots { get => renderSlots; set => renderSlots = value; }
    public Vector3 SlotSize => new Vector3(slotScale.x * transform.localScale.x, slotScale.y * transform.localScale.y, slotScale.z * transform.localScale.z);

    void Start()
    {
        bodyCubes = new BodyCube[size.x * size.y * size.z];
        cuberRenderParams = new RenderParams(cubeMaterial)
        {
            worldBounds = new Bounds(Vector3.zero, Vector3.one * 1000),
            shadowCastingMode = ShadowCastingMode.On
        };

        slotRenderParams = new RenderParams(slotMaterial)
        {
            worldBounds = new Bounds(Vector3.zero, Vector3.one * 1000),
            shadowCastingMode = ShadowCastingMode.Off
        };

        AddCube(new());
        AddCube(new(1, 0, 0));
    }

    private void Update()
    {
        // Cube matrices
        for (int i = 0; i < cubeMatrices.Count; ++i)
            cubeMatrices[i] = Matrix4x4.TRS(GetWorldPos(activeCubes[i].coords), Quaternion.identity, SlotSize);

        // Render cubes
        for (int i = 0; i < cubeMatrices.Count; i += 1023)
            Graphics.RenderMeshInstanced(cuberRenderParams, cubeMesh, 0, cubeMatrices, Mathf.Min(1023, activeCubes.Count - i), i);

        if (renderSlots)
        {
            // Slot Matrices
            for (int i = 0; i < slotMatrices.Count; i++)
                slotMatrices[i] = Matrix4x4.TRS(GetWorldPos(slots[i]), Quaternion.identity, Vector3.one);

            // Render slots
            for (int i = 0; i < slotMatrices.Count; i += 1023)
                Graphics.RenderMeshInstanced(slotRenderParams, slotMesh, 0, slotMatrices, Mathf.Min(1023, slotMatrices.Count - i), i);
        }
    }

    public void AddCube(Vector3Int coords)
    {
        if (!IsValid(coords)) return;

        int index = GetIndex(coords);
        if (bodyCubes[index] != null)
            return;

        cubeMatrices.Add(new());
        activeCubes.Add(bodyCubes[index] = new()
        {
            coords = coords
        });

        FindSlots();
    }

    public void RemoveCube(Vector3Int coords)
    {
        int gridIndex = GetIndex(coords);
        if (bodyCubes[gridIndex] == null)
            return;

        int listIndex = activeCubes.IndexOf(bodyCubes[gridIndex]);

        activeCubes.RemoveAt(listIndex);
        cubeMatrices.RemoveAt(listIndex);
        bodyCubes[gridIndex] = null;
    }

    public BodyCube GetCube(Vector3Int coords)
    {
        int index = GetIndex(coords);
        return IsValid(coords) ? bodyCubes[index] : null;
    }

    int GetIndex(Vector3Int coords) =>
        coords.x + coords.y * size.x + coords.z * size.x * size.y;

    bool IsValid(Vector3Int coords) =>
        coords.x >= 0 && coords.x < size.x && coords.y >= 0 && coords.y < size.y && coords.z >= 0 && coords.z < size.z;

    /// <summary>
    /// Finds all empty spaces that are adjacent to a cube.
    /// </summary>
    void FindSlots()
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
                Vector3Int coords = cube.coords + dir;

                if (!visited.Contains(coords) && IsValid(coords) && GetCube(coords) == null)
                {
                    slots.Add(cube.coords + dir);
                    slotMatrices.Add(new());
                    visited.Add(coords);
                }
            }
        }
    }

    Vector3 GetWorldPos(Vector3Int coords) =>
        new Vector3(coords.x * slotScale.x * transform.localScale.x,
            coords.y * slotScale.y * transform.localScale.y,
            coords.z * slotScale.z * transform.localScale.z) + transform.position;
}

public class BodyCube
{
    public Vector3Int coords;

    public BodyCube()
    {

    }
}