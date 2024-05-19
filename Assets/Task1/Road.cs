using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private GameObject carPrefab;

    [SerializeField] private Vector3 groundSize = new Vector3(100f, 0.1f, 100f);

    private GameObject roadObject;

    private void Start()
    {
        CreateRoad();
    }



    private void CreateRoad()
    {

        GameObject cube = new GameObject();
        cube.name = "Road";

        Cube cubeScript = cube.AddComponent<Cube>();
        cubeScript.UpdateSubmeshCount(1);
        cubeScript.UpdateSubmeshIndex(0, 0, 0, 0, 0, 0);
        cubeScript.UpdateMaterialsList(GroundMaterialList());

        cube.transform.rotation = this.transform.rotation;
        cube.transform.position = this.transform.position;
        cube.transform.localScale = groundSize;
        cube.transform.parent = this.transform;
    }
    private List<Material> GroundMaterialList()
    {

        List<Material> groundMaterialList = new List<Material>();

        Material greenMaterial = new Material(Shader.Find("Specular"));
        greenMaterial.color = Color.black;

        groundMaterialList.Add(greenMaterial);

        return groundMaterialList;

    }
}