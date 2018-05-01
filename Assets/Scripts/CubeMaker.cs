using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class CubeMaker : MonoBehaviour
{

    public Vector3 size = Vector3.one;
    MeshCreator mc = new MeshCreator();
    int gridCubes = 20;
    double distBetween = 1.2;
    public Color altColor = Color.black;
    public Color orColor = Color.black;
    public Renderer rend;

    public GameObject particleDeathEffect;

    bool partSpawn = false;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        //Set the initial color (0f,0f,0f,0f)
        rend.material.color = altColor;
    }

    // Update is called once per frame
    void Update()
    {

        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        // one submesh for each face
        Vector3 center = new Vector3(0, 0, 0);

        mc.Clear(); // Clear internal lists and mesh

        //Inputs to increase and decrese grid numbers
        if (Input.GetKeyDown(KeyCode.Space))
            gridCubes += 10;

        if (Input.GetKeyDown(KeyCode.Backspace))
            gridCubes -= 10;

        //Inputs to increase and decrease the spacing between the cubes
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            distBetween -= 0.2;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            distBetween += 0.2;

        //Inputs for switching color
        if (Input.GetKeyDown(KeyCode.G))
        {
            //Alter the color          
            altColor.g += 0.1f;
            //Assign the changed color to the material.
            rend.material.color = altColor;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Alter the color           
            altColor.r += 0.1f;
            //Assign the changed color to the material. 
            rend.material.color = altColor;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //Alter the color            
            altColor.b += 0.1f;
            //Assign the changed color to the material. 
            rend.material.color = altColor;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Alter the color          
            altColor.a += 0.1f;
            //Assign the changed color to the material. 
            rend.material.color = altColor;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            //Alter the color          
            altColor.a += 0.1f;
            //Assign the changed color to the material. 
            altColor = orColor;
            rend.material.color = altColor;
         }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            partSpawn = true;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            partSpawn = false;
        }

        //Creating the cubes grid
        for (int row = 0; row < gridCubes; row++)
        {
            for (int col = 0; col < gridCubes; col++)
            {
                center.Set(col * size.x * (float)distBetween, 0, row * size.z * (float)distBetween);
                CreateCube(center);
            }
        }

        meshFilter.mesh = mc.CreateMesh();

    }

    void CreateCube(Vector3 center)
    {
        Vector3 cubeSize = size * 0.5f;

        // Call the noise function
        //float offsetY = Perlin.Noise(center.x / 20, center.y / 20, center.z / 20);
        //float offsetY = Perlin.Noise(Random.Range(.1f,.5f)/15, Random.Range(.1f, .5f)/15, Random.Range(.1f, .5f)/15);
        float offsetY = Perlin.Noise(center.x/20 + Random.Range(0.1f, 10f), center.y/20 + Random.Range(0.1f, 10f), center.z/20 + Random.Range(0.1f, 10f));
        //float offsetY = Perlin.Noise((center.x + Random.Range(0.1f, 10f))/20, (center.y / 20 + Random.Range(0.1f, 10f))/20, (center.z / 20 + Random.Range(0.1f, 10f))/20);

        //float offsetY = Perlin.Noise(center.x, center.y, center.z);
        //float offsetZ = Perlin.Noise(center.x, center.y, center.z);

        Debug.Log("this is" + offsetY);
        if (partSpawn)
        {
            GameObject deatheffect = (GameObject)Instantiate(particleDeathEffect, center, Quaternion.identity);
            Destroy(deatheffect, 5f);
        }

        // top of the cube
        // t0 is top left point
        Vector3 t0 = new Vector3(center.x + cubeSize.x, center.y + cubeSize.y + offsetY, center.z - cubeSize.z);
        Vector3 t1 = new Vector3(center.x - cubeSize.x, center.y + cubeSize.y + offsetY, center.z - cubeSize.z);
        Vector3 t2 = new Vector3(center.x - cubeSize.x, center.y + cubeSize.y + offsetY, center.z + cubeSize.z);
        Vector3 t3 = new Vector3(center.x + cubeSize.x, center.y + cubeSize.y + offsetY, center.z + cubeSize.z);

        // bottom of the cube
        Vector3 b0 = new Vector3(center.x + cubeSize.x , center.y - cubeSize.y, center.z - cubeSize.z);
        Vector3 b1 = new Vector3(center.x - cubeSize.x , center.y - cubeSize.y, center.z - cubeSize.z);
        Vector3 b2 = new Vector3(center.x - cubeSize.x , center.y - cubeSize.y, center.z + cubeSize.z);
        Vector3 b3 = new Vector3(center.x + cubeSize.x , center.y - cubeSize.y, center.z + cubeSize.z);

        // Top square
        mc.BuildTriangle(t0, t1, t2);
        mc.BuildTriangle(t0, t2, t3);

        // Bottom square
        mc.BuildTriangle(b2, b1, b0);
        mc.BuildTriangle(b3, b2, b0);

        // Back square
        mc.BuildTriangle(b0, t1, t0);
        mc.BuildTriangle(b0, b1, t1);

        mc.BuildTriangle(b1, t2, t1);
        mc.BuildTriangle(b1, b2, t2);

        mc.BuildTriangle(b2, t3, t2);
        mc.BuildTriangle(b2, b3, t3);

        mc.BuildTriangle(b3, t0, t3);
        mc.BuildTriangle(b3, b0, t0);
    }
}
