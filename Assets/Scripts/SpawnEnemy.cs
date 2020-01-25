using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] enemies;
    public LayerMask ground;
    private Collider2D groundDetection;

    public bool enemySpawned = false;
    private Vector2 spawnPlace;
    public int enemyNumber;

    private void Update()
    {
        if(!enemySpawned && GameObject.FindGameObjectWithTag("LvlGen").GetComponent<LevelGeneration>().stopGen)
        {
            Spawn(enemyNumber);
        }
    }

    public void Spawn(int enemiesPerRoom)
    {
        RandomizePosition();

        if (!groundDetection)
        {
            int randSpawn = Random.Range(0, enemiesPerRoom + 1);
            if (randSpawn % (enemiesPerRoom + 1) == 1)
            {
                Instantiate(enemies[1], spawnPlace, Quaternion.identity);
            }
            else
            {
                int previousI = 0;
                for (int i = 0; i < enemiesPerRoom; i++)
                {
                    if (groundDetection)
                    {
                        i = previousI;
                    }
                    else
                    {
                        Instantiate(enemies[0], spawnPlace, Quaternion.identity);
                        previousI++;
                    }
                    RandomizePosition();
                }
            }
            enemySpawned = true;
        }
    }

    private void RandomizePosition()
    {
        float randPosX = Random.Range(transform.position.x - 5.0f, transform.position.x + 5.0f);
        float randPosY = Random.Range(transform.position.y - 5.0f, transform.position.y + 5.0f);
        spawnPlace = new Vector2(randPosX, randPosY);
        groundDetection = Physics2D.OverlapCircle(spawnPlace, 1, ground);
    }

    public void resetSpawner()
    {
        enemySpawned = false;
    }
}
