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
    
    public Vector3 this[int x, int y, int z] =>
        new Vector3(x, y, z) + transform.position;
    public Vector3 this[Vector3Int coords] =>
        new Vector3(coords.x * slotScale.x * transform.localRotation.x,
            coords.y * slotScale.y * transform.localRotation.x,
            coords.z * slotScale.z * transform.localRotation.x) + transform.position;

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

        AddCube(0, 0, 0);
        AddCube(1, 0, 0);
    }

    private void Update()
    {
        // Cube matrices
        for (int i = 0; i < cubeMatrices.Count; ++i)
            cubeMatrices[i] = Matrix4x4.TRS(this[activeCubes[i].coord], Quaternion.identity, SlotSize);

        // Render cubes
        for (int i = 0; i < cubeMatrices.Count; i += 1023)
            Graphics.RenderMeshInstanced(cuberRenderParams, cubeMesh, 0, cubeMatrices, Mathf.Min(1023, activeCubes.Count - i), i);
        
        if (renderSlots)
        {
            // Slot Matrices
            for (int i = 0; i < slotMatrices.Count; i++)
                slotMatrices[i] = Matrix4x4.TRS(this[slots[i]], Quaternion.identity, Vector3.one);
            
            // Render slots
            for (int i = 0; i < slotMatrices.Count; i += 1023)
                Graphics.RenderMeshInstanced(slotRenderParams, slotMesh, 0, slotMatrices, Mathf.Min(1023, slotMatrices.Count - i), i);
        }
    }
    
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
        
        FindSlots();
    }
    
    public void AddCube(Vector3Int coords) => AddCube(coords.x, coords.y, coords.z);

    public void RemoveCube(int x, int y, int z)
    {
        int gridIndex = GetIndex(x, y, z);
        if (bodyCubes[gridIndex] == null)
            return;

        int listIndex = activeCubes.IndexOf(bodyCubes[gridIndex]);

        activeCubes.RemoveAt(listIndex);
        cubeMatrices.RemoveAt(listIndex);
        bodyCubes[gridIndex] = null;
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
                Vector3Int coord = cube.coord + dir;

                if (!visited.Contains(coord) && IsValid(coord.x, coord.y, coord.z) && GetCube(coord.x, coord.y, coord.z) == null)
                {
                    slots.Add(cube.coord + dir);
                    slotMatrices.Add(new());
                    visited.Add(coord);
                }
            }
        }
    }
}

public class BodyCube
{
    public Vector3Int coord;

    public BodyCube()
    {

    }
}