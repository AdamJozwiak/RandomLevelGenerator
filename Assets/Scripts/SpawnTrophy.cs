using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrophy : MonoBehaviour
{
    public GameObject[] trophies;
    public LayerMask ground;
    private Collider2D groundDetection;

    public bool trophySpawned = false;
    private Vector2 spawnPlace;
    public float trophyChance;

    // Update is called once per frame
    void Update()
    {
        if (!trophySpawned && GameObject.FindGameObjectWithTag("LvlGen").GetComponent<LevelGeneration>().stopGen)
        {
            Spawn(trophyChance);
        }
    }

    public void Spawn(float trophyChance)
    {
        RandomizePosition();

        if (!groundDetection)
        {
            int randSpawn = Random.Range(1, 11);
            if (trophyChance / randSpawn > 0.3)
            {
                Instantiate(trophies[0], spawnPlace, Quaternion.identity);
            }
            else
            {
                Instantiate(trophies[1], spawnPlace, Quaternion.identity);
            }
            trophySpawned = true;
        }
    }

    private void RandomizePosition()
    {
        float randPosX = Random.Range(transform.position.x - 5.0f, transform.position.x + 5.0f);
        float randPosY = Random.Range(transform.position.y - 5.0f, transform.position.y + 5.0f);
        spawnPlace = new Vector2(randPosX, randPosY);
        groundDetection = Physics2D.OverlapCircle(spawnPlace, 1, ground);
    }
}
