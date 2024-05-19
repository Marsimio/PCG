using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshBuilder 
{
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private List<Vector3> normals = new List<Vector3>();

    private List<Vector2> uvs= new List<Vector2>();

    private List<int>[] submeshes = new List<int>[]{};

    public MeshBuilder(int submeshCount){

        submeshes = new List<int>[submeshCount];

        for(int i = 0; i < submeshCount; i++){
            submeshes[i] = new List<int>();
        }

    }

    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2,
                             int submesh){
        //Calculate the normal which is the perpendicular angle of a triangle
        Vector3 normal = Vector3.Cross(p1 - p0, p2 - p0).normalized;
        BuildTriangle(p0, p1, p2, normal, submesh);
    }

    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2,
                              Vector3 normal, int submesh){

        //Get next available indices from vertices list
        int p0Index = vertices.Count; 
        int p1Index = vertices.Count + 1;
        int p2Index = vertices.Count + 2;

        //fill vertices list with points
        vertices.Add(p0);
        vertices.Add(p1);
        vertices.Add(p2);

        //Add index of each point to the triangles list
        triangles.Add(p0Index);
        triangles.Add(p1Index);
        triangles.Add(p2Index);
        
        //Add normal for each point that make up the triangle
        normals.Add(normal);
        normals.Add(normal);
        normals.Add(normal);

        //Add each UV coordinate foe each triangle
        uvs.Add(new Vector2(0,0));
        uvs.Add(new Vector2(0,1));
        uvs.Add(new Vector2(1,1));

        submeshes[submesh].Add(p0Index);
        submeshes[submesh].Add(p1Index);
        submeshes[submesh].Add(p2Index);

    }

    public Mesh CreateMesh(){
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();

        mesh.triangles = triangles.ToArray();

        mesh.normals = normals.ToArray();

        mesh.uv = uvs.ToArray();

        mesh.subMeshCount = submeshes.Length;

        for(int i = 0; i < submeshes.Length; i++){
            if(submeshes[i].Count < 3){
                mesh.SetTriangles(new int[3]{0,0,0},i);
            }else{
                mesh.SetTriangles(submeshes[i].ToArray(),i);
            }
        }

        return mesh;
    }
}
