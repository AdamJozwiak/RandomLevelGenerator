using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject wallsPoint;
    public Camera camera;
    public InputField enemiesNumber;
    public InputField trophyChance;

    public int dimensionsX;
    public int dimensionsY;

    public float minX;
    public float maxX;
    public float minY;

    public float minWallX;
    public float maxWallX;
    public float minWallY;

    public GameObject[] spawnPoints;
    public GameObject[] wallPoints;

    private void Start()
    {
        enemiesNumber.text = "1";
        trophyChance.text = "1";
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
                if (i == dimensionsY / 2 && j == dimensionsX / 2)
                {
                    camera.transform.position = new Vector3(transform.position.x + (j * 10) - 5, transform.position.y - (i * 10) + 5, -10.0f);
                }
            }
            if(i == dimensionsY - 1)
            {
                minY = transform.position.y - (i * 10);
            }
        }
        return spawnPoints;
    }

    public GameObject[] GenerateWalls()
    {
        wallPoints = new GameObject[2 * (dimensionsX + 1) + 2 * (dimensionsY + 1)];
        minWallX = transform.position.x - 10.0f;
        int it = 0;
        for (int i = -1; i <= dimensionsX; i++)
        {
            wallPoints[it++] = Instantiate(wallsPoint, new Vector3(transform.position.x + (i * 10), transform.position.y + 10.0f), Quaternion.identity);
            wallPoints[it++] = Instantiate(wallsPoint, new Vector3(transform.position.x + (i * 10), transform.position.y - (dimensionsY * 10)), Quaternion.identity);
        }
        for(int i = 0; i < dimensionsY; i++)
        {
            wallPoints[it++] = Instantiate(wallsPoint, new Vector3(transform.position.x - 10.0f, transform.position.y - (i * 10.0f)), Quaternion.identity);
            wallPoints[it++] = Instantiate(wallsPoint, new Vector3(transform.position.x + (dimensionsX * 10), transform.position.y - (i * 10)), Quaternion.identity);
        }

        return wallPoints;
    }

    public void dimensionXChanged(string size)
    {
        dimensionsX = int.Parse(size);
        StartCoroutine(waitTilGenerated(enemiesNumber.text, trophyChance.text));
    }

    public void dimensionYChanged(string size)
    {
        dimensionsY = int.Parse(size);
        StartCoroutine(waitTilGenerated(enemiesNumber.text, trophyChance.text));
    }

    public void changeEnemiesNumber(string number)
    {
        StartCoroutine(waitTilGenerated(number, trophyChance.text));
    }

    public void changeTrophyChance(string number)
    {
        StartCoroutine(waitTilGenerated(enemiesNumber.text, number));
    }

    public void adjustCamera(float size)
    {
        camera.orthographicSize = size;
    }

    private void ReloadMap()
    {
        GameObject[] toReload = GameObject.FindGameObjectsWithTag("ToReload");
        foreach (GameObject rObject in toReload)
        {
            GameObject.Destroy(rObject);
        }
        GameObject.FindGameObjectWithTag("LvlGen").GetComponent<LevelGeneration>().Start();
    }

    private IEnumerator waitTilGenerated(string enemyNumber, string trophyChance)
    {
        ReloadMap();

        while (!GameObject.FindGameObjectWithTag("LvlGen").GetComponent<LevelGeneration>().stopGen)
        {
            yield return null;
        }

        SpawnEnemy[] enemySpawners = GameObject.FindObjectsOfType<SpawnEnemy>();
        foreach (SpawnEnemy enemySpawner in enemySpawners)
        {
            enemySpawner.GetComponent<SpawnEnemy>().Spawn(int.Parse(enemyNumber));
        }

        SpawnTrophy[] trophySpawners = GameObject.FindObjectsOfType<SpawnTrophy>();
        foreach (SpawnTrophy trophySpawner in trophySpawners)
        {
            trophySpawner.GetComponent<SpawnTrophy>().Spawn(int.Parse(trophyChance));
        }
    }
}
