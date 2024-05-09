using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{

    [SerializeField] private Vector3 floorSize = new Vector3(100f, 0.1f, 100f);
    [SerializeField] private Vector3 floorPosition = new Vector3(0f, 0.5f, 0f);

    [SerializeField] private Vector3 roofSize = new Vector3(100f, 0.5f, 100f);
    [SerializeField] private Vector3 roofPosition = new Vector3(0, 120f, 0);

    [SerializeField] private Vector3 frontBackWallSize = new Vector3(100f, 80f, 4f);
    [SerializeField] private Vector3 sideWallSize = new Vector3(4f, 80f, 100f);
    [SerializeField] private Vector3 leftWallPosition = new Vector3(-100f, 40f, 0f);
    [SerializeField] private Vector3 rightWallPosition = new Vector3(100f, 40f, 0f);
    [SerializeField] private Vector3 frontWallPosition = new Vector3(0f, 40f, -100f);
    [SerializeField] private Vector3 backWallPosition = new Vector3(0f, 40f, 100f);

    [SerializeField] private Vector3 frontDoorSize = new Vector3(40f, 45f, 4f);
    [SerializeField] private Vector3 frontDoorPosition = new Vector3(0f, 22.5f, -100.1f);

    [SerializeField] private Vector3 frontLeftWindowSize = new Vector3(20f, 20f, 4f);
    [SerializeField] private Vector3 frontLeftWindowPosition = new Vector3(-60f, 62f, -100.1f);
    [SerializeField] private Vector3 frontRightWindowSize = new Vector3(20f, 20f, 4f);
    [SerializeField] private Vector3 frontRightWindowPosition = new Vector3(60f, 62f, -100.1f);
    [SerializeField] private Vector3 leftWindowSize = new Vector3(4f, 20f, 20f);
    [SerializeField] private Vector3 leftWindowPosition = new Vector3(-100.1f, 62f, 0f);
    [SerializeField] private Vector3 rightWindowSize = new Vector3(4f, 20f, 20f);
    [SerializeField] private Vector3 rightWindowPosition = new Vector3(100.1f, 62f, 0f);

    private GameObject houseGameObject;

    void Start()
    {
        InitializeHouse();
        CreateHouse();
    }

    private void InitializeHouse()
    {
        houseGameObject = new GameObject("House");
        houseGameObject.transform.SetParent(this.transform, false);
        houseGameObject.transform.localPosition = Vector3.zero;
        houseGameObject.transform.localRotation = Quaternion.identity;
        houseGameObject.transform.localScale = Vector3.one;
    }

    private void CreateHouse()
    {
        Floor();
        Roof();
        Wall("Left Wall", leftWallPosition, sideWallSize, SideWallMaterialList());
        Wall("Right Wall", rightWallPosition, sideWallSize, SideWallMaterialList());
        Wall("Front Wall", frontWallPosition, frontBackWallSize, FrontBackWallMaterialList());
        Wall("Back Wall", backWallPosition, frontBackWallSize, FrontBackWallMaterialList());
        Aperture("Front Door", frontDoorPosition, frontDoorSize, DoorMaterialList());
        Aperture("Front Left Window", frontLeftWindowPosition, frontLeftWindowSize, WindowMaterialList());
        Aperture("Front Right Window", frontRightWindowPosition, frontRightWindowSize, WindowMaterialList());
        Aperture("Left Window", leftWindowPosition, leftWindowSize, WindowMaterialList());
        Aperture("Right Window", rightWindowPosition, rightWindowSize, WindowMaterialList());
    }



    private void Floor()
    {
        CreateCube("Floor", floorPosition, floorSize, FloorMaterialList());
    }

    private void Roof()
    {
        CreateCube("Roof", roofPosition, roofSize, RoofMaterialList());
    }

    private void Wall(string name, Vector3 localPosition, Vector3 size, List<Material> materialList)
    {
        CreateCube(name, localPosition, size, materialList);
    }

    private void Aperture(string name, Vector3 localPosition, Vector3 size, List<Material> materialList)
    {
        CreateCube(name, localPosition, size, materialList);
    }

    private GameObject CreateCube(string name, Vector3 localPosition, Vector3 size, List<Material> materialList)
    {
        GameObject cube = new GameObject(name);
        Cube cubeScript = cube.AddComponent<Cube>();
        cubeScript.UpdateSubmeshCount(1);
        cubeScript.UpdateSubmeshIndex(0, 0, 0, 0, 0, 0);
        cubeScript.UpdateMaterialsList(materialList);
        cube.transform.SetParent(houseGameObject.transform, false);
        cube.transform.localPosition = localPosition;
        cube.transform.rotation = Quaternion.identity;
        cube.transform.localScale = size;
        return cube;
    }

    private List<Material> FloorMaterialList(){

        List<Material> floorMaterialList = new List<Material>();

        Material blackMaterial = new Material(Shader.Find("Specular"));
        blackMaterial.color = Color.black; 

        floorMaterialList.Add(blackMaterial);

        return floorMaterialList;

    }

    private List<Material> RoofMaterialList(){

        List<Material> roofMaterialList = new List<Material>();

        Material magentaMaterial = new Material(Shader.Find("Specular"));
        magentaMaterial.color = Color.magenta; 

        roofMaterialList.Add(magentaMaterial);

        return roofMaterialList;

    }    

    private List<Material> SideWallMaterialList(){

        List<Material> sideWallMaterialList = new List<Material>();

        Material yellowMaterial = new Material(Shader.Find("Specular"));
        yellowMaterial.color = Color.yellow; 

        sideWallMaterialList.Add(yellowMaterial);

        return sideWallMaterialList;

    }

    private List<Material> FrontBackWallMaterialList(){

        List<Material> frontBackWallMaterialList = new List<Material>();

        Material whiteMaterial = new Material(Shader.Find("Specular"));
        whiteMaterial.color = Color.white; 

        frontBackWallMaterialList.Add(whiteMaterial);

        return frontBackWallMaterialList;

    }


        private List<Material> DoorMaterialList(){

        List<Material> doorMaterialList = new List<Material>();

        Material redMaterial = new Material(Shader.Find("Specular"));
        redMaterial.color = Color.red; 

        doorMaterialList.Add(redMaterial);

        return doorMaterialList;

    }

    private List<Material> WindowMaterialList(){

        List<Material> windowMaterialList = new List<Material>();

        Material grayMaterial = new Material(Shader.Find("Specular"));
        grayMaterial.color = Color.gray; 

        windowMaterialList.Add(grayMaterial);

        return windowMaterialList;

    }        
}