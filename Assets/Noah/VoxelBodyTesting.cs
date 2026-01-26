using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoxelBodyTesting : MonoBehaviour
{
    [SerializeField]
    Material material;
    [SerializeField]
    Mesh mesh;
    [SerializeField]
    Vector3Int size;

    BodyCube[] bodyCubes;

    Matrix4x4[] matrices;

    void Start()
    {
        bodyCubes = new BodyCube[size.x * size.y * size.z];
        matrices = GenerateMatrices();
    }


    private void Update()
    {
        RenderBody();
    }

    void RenderBody()
    {
        Matrix4x4[] activeMatrices = GetActiveMatrices().ToArray();

        // DrawMeshInstanced has a limit of 1023 instances per call
        int batchSize = 1023;
        for (int i = 0; i < activeMatrices.Length; i += batchSize)
        {
            int count = Mathf.Min(batchSize, matrices.Length - i);

            Graphics.DrawMeshInstanced(mesh, 0, material, activeMatrices);
        }
    }

    Matrix4x4[] GenerateMatrices()
    {
        Matrix4x4[] results = new Matrix4x4[bodyCubes.Length];

        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                for (int z = 0; z < size.z; z++)
                    results[GetIndex(x, y, z)] = Matrix4x4.TRS(new Vector3(x, y, z), Quaternion.identity, Vector3.one);

        return results;
    }

    IEnumerable<Matrix4x4> GetActiveMatrices()
    {
        for (int i = 0; i < bodyCubes.Length; i++)
            if (bodyCubes[i] != null)
                yield return matrices[i];
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

}