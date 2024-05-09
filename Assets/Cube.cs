using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class Cube : MonoBehaviour
{

    [SerializeField]
    private Vector3 size = Vector3.one;

    [SerializeField]
    private int subMeshCount = 6;

    [SerializeField]
    private int topSquareIndex      = 0;
    [SerializeField]
    private int bottomSquareIndex   = 1;
    [SerializeField]
    private int frontSquareIndex    = 2;
    [SerializeField]
    private int backSquareIndex     = 3;
    [SerializeField]
    private int leftSquareIndex     = 4;
    [SerializeField]
    private int rightSquareIndex    = 5;

    private List<Material> cubeMaterialsList = new List<Material>();


    // Start is called before the first frame update
    void Start()
    {
        BuildCube();
    }

    public void UpdateSubmeshCount(int subMeshCount){
        this.subMeshCount = subMeshCount;
    }

    public void UpdateSubmeshIndex(int top,int bottom, 
                                   int front, int back,
                                   int left, int right){
        this.frontSquareIndex   = front;
        this.backSquareIndex    = back;
        this.topSquareIndex     = top;
        this.bottomSquareIndex  = bottom;
        this.leftSquareIndex    = left;
        this.rightSquareIndex   = right;
    }

    public void UpdateMaterialsList(List<Material> materialsList){
        this.cubeMaterialsList = materialsList;
    }

    private void BuildCube(){
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        MeshBuilder meshBuilder = new MeshBuilder(subMeshCount);

        //Assign the Vertices
        //define the top points
        Vector3 t0 = new Vector3(size.x, size.y, -size.z);
        Vector3 t1 = new Vector3(-size.x, size.y, -size.z);
        Vector3 t2 = new Vector3(-size.x, size.y, size.z);
        Vector3 t3 = new Vector3(size.x, size.y, size.z);

        //define the bottom points
        Vector3 b0 = new Vector3(size.x, -size.y, -size.z);
        Vector3 b1 = new Vector3(-size.x, -size.y, -size.z);
        Vector3 b2 = new Vector3(-size.x, -size.y, size.z);
        Vector3 b3 = new Vector3(size.x, -size.y, size.z);

        //Create the triangles
        
        //top square
        meshBuilder.BuildTriangle(t0,t1,t2, topSquareIndex);
        meshBuilder.BuildTriangle(t0,t2,t3, topSquareIndex);

        //bottom square
        meshBuilder.BuildTriangle(b2,b1,b0, bottomSquareIndex);
        meshBuilder.BuildTriangle(b3,b2,b0, bottomSquareIndex); 

        //front square
        meshBuilder.BuildTriangle(b2,t3,t2, frontSquareIndex);
        meshBuilder.BuildTriangle(b2,b3,t3, frontSquareIndex); 

        //back square
        meshBuilder.BuildTriangle(b0,t1,t0, backSquareIndex);
        meshBuilder.BuildTriangle(b0,b1,t1, backSquareIndex);   

        //left square
        meshBuilder.BuildTriangle(b1,t2,t1, leftSquareIndex);
        meshBuilder.BuildTriangle(b1,b2,t2, leftSquareIndex); 

        //right square
        meshBuilder.BuildTriangle(b3,t0,t3, rightSquareIndex);
        meshBuilder.BuildTriangle(b3,b0,t0, rightSquareIndex);   

        meshFilter.mesh = meshBuilder.CreateMesh();

        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();

        if(cubeMaterialsList.Count <= 0){
            Materials materials = new Materials();
            cubeMaterialsList = materials.GetMaterials();
        }      

        meshRenderer.materials = cubeMaterialsList.ToArray();

        MeshCollider meshCollider = this.GetComponent<MeshCollider>();

        meshCollider.sharedMesh = meshFilter.mesh;
    }

}
