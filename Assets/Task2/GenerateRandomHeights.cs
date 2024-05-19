using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TerrainTextureData
{
    public Texture2D terrainTexture;
    public Vector2 tileSize;
    public float minHeight;
    public float maxHeight;
}
[System.Serializable]
public class TreeData
{
    public GameObject treeMesh;
    public float minHeight;
    public float maxHeight;
}
public class GenerateRandomHeights : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;

    [SerializeField][Range(0f, 1f)] private float minRandomWidthRange = 0f;

    [SerializeField][Range(0f, 1f)] private float maxRandomWidthRange = 0.1f;

    [SerializeField][Range(0f, 1f)] private float minRandomHeightRange = 0f;

    [SerializeField][Range(0f,1f)] private float maxRandomHeightRange = 0.1f;

    [SerializeField] private bool flattenTerrain = true;

    [SerializeField] private bool perlinNoise = false;

    [SerializeField] private float perlinNoiseWidthScale = 0.01f;

    [SerializeField] private float perlinNoiseHeightScale = 0.01f;

    [SerializeField] private List<TerrainTextureData> terrainTextureData;

    [SerializeField] bool addTerrainTexture = false;

    [SerializeField] float terrainTextureBlendOffset = 0.5f;

    [SerializeField] List<TreeData> treeData;

    [SerializeField] private int maxTrees = 2000;

    [SerializeField] private int treeSpacing = 10;

    [SerializeField] private bool addTrees = false;

    [SerializeField] private int terrainLayerIndex;

    private void Start()
    {
        if(terrainData == null) {
            terrain = GetComponent<Terrain>();
        }

        if(terrainData == null)
        {
            terrainData = terrain.terrainData;
        }
        GenerateHeights();
        addTerrainTextures();
        AddTrees();
    }

    private void Update()
    {
    }

    void GenerateHeights()
    {
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                float randomHeight = Random.Range(minRandomHeightRange, maxRandomHeightRange);
                float randomWidth = Random.Range(minRandomWidthRange, maxRandomWidthRange);

                if (perlinNoise)
                {
                    float perlinNoiseValue = Mathf.PerlinNoise(width * perlinNoiseWidthScale * randomWidth, height * perlinNoiseHeightScale * randomWidth);
                    heightMap[width, height] = perlinNoiseValue * randomHeight;
                }
                else
                {
                    heightMap[width, height] = randomHeight;
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    private void FlattenTerrain()
    {
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                heightMap[width, height] = 0;
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    private void addTerrainTextures()
    {
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureData.Count];

        for (int i = 0; i < terrainTextureData.Count; i++)
        {
            if (addTerrainTexture)
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = terrainTextureData[i].terrainTexture;
                terrainLayers[i].tileSize = terrainTextureData[i].tileSize;
            }
            else
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = null;
            }
        }
        terrainData.terrainLayers = terrainLayers;

        float[,] heightMap = terrainData.GetHeights(0,0,terrainData.heightmapResolution,terrainData.heightmapResolution);

        float[,,] alphamapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for(int height = 0; height < terrainData.alphamapHeight; height++)
        {
            for(int width = 0; width < terrainData.alphamapWidth; width++)
            {
                float[] alphamap = new float[terrainData.alphamapLayers];

                for(int i = 0; i < terrainTextureData.Count; i++)
                {
                    float heightBegin = terrainTextureData[i].minHeight - terrainTextureBlendOffset;
                    float heightEnd = terrainTextureData[i].maxHeight + terrainTextureBlendOffset;

                    if (heightMap[width, height] >= heightBegin && heightMap[width, height] <= heightEnd){
                        alphamap[i] = 1;
                    }

                }

                Blend(alphamap);

                for(int j = 0; j < terrainTextureData.Count; j++)
                {
                    alphamapList[width, height, j] = alphamap[j];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphamapList);
    }

    private void Blend(float[] alphamap)
    {
        float total = 0;

        for (int i = 0; i < alphamap.Length; i++)
        {
            total += alphamap[i];
        }

        if (total > 0)
        {
            for (int i = 0; i < alphamap.Length; i++)
            {
                alphamap[i] /= total;
            }
        }
    }

    private void AddTrees()
    {
        TreePrototype[] trees = new TreePrototype[treeData.Count];

        for(int i = 0; i < treeData.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeData[i].treeMesh;
        }

        terrainData.treePrototypes = trees;

        List<TreeInstance> treeInstanceList = new List<TreeInstance>();

        if(addTrees)
        {
            for(int z = 0; z < terrainData.size.z; z+= treeSpacing)
            {
                for(int x = 0; x < terrainData.size.x; x += treeSpacing)
                {
                    for(int treeIndex = 0; treeIndex < trees.Length; treeIndex++)
                    {
                        if(treeInstanceList.Count < maxTrees)
                        {
                            float normalizedX = (float)x / terrainData.size.x;
                            float normalizedZ = (float)z / terrainData.size.z;
                            float currentHeight = terrainData.GetHeight((int)(normalizedX * terrainData.heightmapResolution), (int)(normalizedZ * terrainData.heightmapResolution)) / terrainData.size.y;

                            if (currentHeight >= treeData[treeIndex].minHeight && currentHeight <= treeData[treeIndex].maxHeight)
                            {
                                float randomX = (x + Random.Range(-5.0f, 5.0f))/ terrainData.size.x;
                                float randomZ = (z + Random.Range(-5.0f, 5.0f)) / terrainData.size.z;

                                Vector3 treePosition = new Vector3(randomX* terrainData.size.x, currentHeight* terrainData.size.y, randomZ* terrainData.size.z)+ this.transform.position;

                                RaycastHit raycastHit;

                                int layerMask = 1 << terrainLayerIndex;

                                if(Physics.Raycast(treePosition, -Vector3.up, out raycastHit, 100, layerMask) || Physics.Raycast(treePosition, Vector3.up, out raycastHit, 100, layerMask))
                                {

                                    float treeDistance = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;
                                    TreeInstance treeInstance = new TreeInstance();

                                    treeInstance.position = new Vector3(randomX, treeDistance, randomZ);
                                    treeInstance.rotation = Random.Range(0, 360);
                                    treeInstance.prototypeIndex = treeIndex;
                                    treeInstance.color = Color.white;
                                    treeInstance.heightScale = Random.Range(0.8f, 1.2f);
                                    treeInstance.widthScale = Random.Range(0.8f, 1.2f);

                                    treeInstanceList.Add(treeInstance);
                                }
                            }
                        }
                    }
                }
            }
        }
        terrainData.treeInstances = treeInstanceList.ToArray();
    }

    private void OnDestroy()
    {
        if (flattenTerrain)
        {
            FlattenTerrain();
        }
    }


}
