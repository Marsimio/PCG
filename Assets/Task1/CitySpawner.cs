using System.Collections.Generic;
using UnityEngine;

public class CitySpawner : MonoBehaviour
{
    public int rows = 10;
    public int columns = 10;
    public float spacing = 200;

    public GameObject playerPrefab; // Reference to a player prefab
    private GameObject player;
    private Vector3 startPosition;
    private Vector3 endPosition;

    void Start()
    {
        GenerateLevel();
        CreatePlayer();
    }

    void Update()
    {
        if (player != null && Vector3.Distance(player.transform.position, endPosition) < spacing)
        {
            GenerateLevel();
            player.transform.position = startPosition;
        }
    }

    void CreatePlayer()
    {
        if (player == null)
        {
            player = new GameObject("Player");
            player.AddComponent<PlayerMovement>();
        }
        else
        {
            player.transform.position = startPosition;
        }
    }

    void GenerateLevel()
    {
        ClearGrid();
        startPosition = GetRandomPosition();
        do
        {
            endPosition = GetRandomPosition();
            CreateCube("End", endPosition, new Vector3(3, 3, 3), CubeColor());
        } while (startPosition == endPosition);

        int[,] gridLayout = GenerateRandomLayout();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = new Vector3(j * spacing, 0, i * spacing);
                GameObject tile = new GameObject("Tile");
                tile.transform.position = position;
                tile.transform.parent = this.transform;

                if (gridLayout[i, j] == 1)
                {
                    tile.name = "House";
                    tile.AddComponent<House>();
                }
                else if (gridLayout[i, j] == 2)
                {
                    tile.name = "Road";
                    tile.AddComponent<Road>();
                }

                Rigidbody rb = tile.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(0, columns) * spacing, 0, Random.Range(0, rows) * spacing);
    }

    int[,] GenerateRandomLayout()
    {
        int[,] layout = new int[rows, columns];
        List<Vector2> positions = new List<Vector2>();

        while (positions.Count < 6)
        {
            int row = Random.Range(0, rows);
            int col = Random.Range(0, columns);
            Vector2 pos = new Vector2(row, col);
            if (!positions.Contains(pos))
            {
                positions.Add(pos);
                layout[row, col] = 1;
            }
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (layout[i, j] == 0)
                {
                    layout[i, j] = 2;
                }
            }
        }

        return layout;
    }

    void ClearGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private GameObject CreateCube(string name, Vector3 localPosition, Vector3 size, List<Material> materialList)
    {
        GameObject cube = new GameObject(name);
        Cube cubeScript = cube.AddComponent<Cube>();
        cubeScript.UpdateSubmeshCount(1);
        cubeScript.UpdateSubmeshIndex(0, 0, 0, 0, 0, 0);
        cubeScript.UpdateMaterialsList(materialList);
        cube.transform.SetParent(this.transform, false);
        cube.transform.position = localPosition;
        cube.transform.rotation = Quaternion.identity;
        cube.transform.localScale = size;
        return cube;
    }

    private List<Material> CubeColor()
    {

        List<Material> cubeMaterialList = new List<Material>();

        Material magentaMaterial = new Material(Shader.Find("Specular"));
        magentaMaterial.color = Color.magenta;

        cubeMaterialList.Add(magentaMaterial);

        return cubeMaterialList;

    }
}
