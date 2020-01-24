using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public GameObject spawnPoint;

    public int dimensionsX;
    public int dimensionsY;

    public float minX;
    public float maxX;
    public float minY;

    public GameObject[] spawnPoints;

    // Update is called once per frame
    void Start()
    {

    }

    public GameObject[] GenerateMap()
    {
        spawnPoints = new GameObject[dimensionsX * dimensionsY];
        minX = transform.position.x;
        for (int i = 0; i < dimensionsY; i++)
        {
            for (int j = 0; j < dimensionsX; j++)
            {
                spawnPoints[i * dimensionsX + j] = Instantiate(spawnPoint, new Vector3(transform.position.x + (j * 10), transform.position.y - (i * 10)), Quaternion.identity);
                if(j == dimensionsX - 1)
                {
                    maxX = transform.position.x + (j * 10);
                }
            }
            if(i == dimensionsY - 1)
            {
                minY = transform.position.y - (i * 10);
            }
        }
        return spawnPoints;
    }
}
