using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator02 : MonoBehaviour {

    public float width = 50f;
    public float height = 50f;

    public int Resolution = 129;

	// Use this for initialization
	void Start () {

        Generate();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Generate()
    {
        TerrainData terrainData = new TerrainData();
        terrainData.alphamapResolution = Resolution;

        //Set the heights
        terrainData.heightmapResolution = Resolution;
        terrainData.SetHeights(0, 0, MakeHeightmap());
        terrainData.size = new Vector3(width, height, width);

        //Create the terrain
        GameObject TerrainObject = Terrain.CreateTerrainGameObject(terrainData);
        TerrainObject.GetComponent<Terrain>().Flush();
    }

    public float[,] MakeHeightmap()
    {
        float[,] Heightmap = new float[Resolution, Resolution];

        for(int x = 0; x < Resolution; x++)
        {
            for(int y = 0; y < Resolution; y++)
            {
                Heightmap[x, y] = GetNormalizedHeight((float)x, (float)y);
            }
        }

        return Heightmap;
    }

    public float GetNormalizedHeight(float x, float y)
    {
        return Mathf.Clamp(Mathf.PerlinNoise(x * 0.05f, y * 0.05f), 0f,0.4f) *0.95f + Mathf.PerlinNoise(x * 0.1f, y * 0.1f) *0.05f;
    }
}
