using UnityEngine;

public class CitySpawner : MonoBehaviour
{

    public int rows = 10;
    public int columns = 10;
    public float spacing = 200;


    void Start()
    {
        GenerateLevel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearGrid();
            GenerateLevel();
        }
    }

    void GenerateLevel()
    {
        int[,] gridLayout = GenerateRandomLayout();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = new Vector3(j * 200, 0, i * 200);
                print(position);
                GameObject gameObject = new GameObject();
                gameObject.transform.position = position;

                if (gridLayout[i, j] == 1)
                {
                    gameObject.name = "House";
                    gameObject.AddComponent<House>();
                }
                else if (gridLayout[i, j] == 2)
                {
                    gameObject.name = "Road";
                    gameObject.AddComponent<Road>();
                }
                gameObject.transform.parent = this.transform;
            }
        }
    }

    int[,] GenerateRandomLayout()
    {
        int[,] layout = new int[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0) 
                    layout[i, j] = 1;
                else if (rand == 1) 
                    layout[i, j] = 2; 
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
}