using UnityEngine;

public class CitySpawner: MonoBehaviour
{

    public int rows = 10;
    public int columns = 10;
    public float spacing = 200;

    private int[,] gridLayout = new int[,]
    {
        { 1, 2, 1, 2, 2, 2, 1, 2, 1, 2 },
        { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
        { 1, 2, 1, 2, 2, 2, 1, 2, 1, 2 },
        { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
        { 1, 2, 1, 2, 2, 2, 1, 2, 1, 2 },
        { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
        { 1, 2, 1, 2, 2, 2, 1, 2, 1, 2 },
        { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
        { 1, 2, 1, 2, 2, 2, 1, 2, 1, 2 },
        { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }
    };

    void Start()
    {
        BuildGrid();
    }

    void BuildGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = new Vector3(j * 200, 0, i * 200);
                print(position);
                GameObject gameObject = new GameObject();
                gameObject.transform.position = position; 
                switch (gridLayout[i, j])
                {
                    case 1:
                        gameObject.name = "House";
                        gameObject.AddComponent<House>();
                        break;
                    case 2:
                        gameObject.name = "Road";
                        gameObject.AddComponent<Road>();
                        break;
                }
                gameObject.transform.parent = this.transform;
            }
        }
    }
}