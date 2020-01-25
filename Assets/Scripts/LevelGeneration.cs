using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelGeneration : MonoBehaviour
{
    private GameObject[] startingPositions;
    public GameObject[] rooms;  // index 0 -> LR, index 1 -> LRB, index 2 -> LRT, index 3 -> LRBT
    public CreateMap map;
    private GameObject[] wallPositions;
    public GameObject wall;

    private int direction;
    public float moveAmount;

    private float timeBtwRoom;
    private float startTimeBtwRoom;

    private float minX;
    private float maxX;
    private float minY;

    public bool stopGen;
    public LayerMask room;

    private int downCounter;
    private int firstRow;

    public void Start()
    {
        startingPositions = map.GenerateMap();
        wallPositions = map.GenerateWalls();

        firstRow = map.dimensionsX;

        minX = map.minX;
        minY = map.minY;
        maxX = map.maxX;

        int randStartingPos = UnityEngine.Random.Range(0, firstRow);
        transform.position = startingPositions[randStartingPos].transform.position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);

        direction = UnityEngine.Random.Range(1, 6);

        startTimeBtwRoom = 0.25f;
        stopGen = false;
    }

    public void Update()
    {
        if(timeBtwRoom <= 0 && !stopGen)
        {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else
        {
            timeBtwRoom -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if(direction == 1 || direction == 2) //Right
        {
            if(transform.position.x < maxX)
            {
                downCounter = 0;

                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = UnityEngine.Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = UnityEngine.Random.Range(1, 6);
                if(direction == 3)
                {
                    direction = 2;
                }
                else if(direction == 4)
                {
                    direction = 5;
                }
            }
            else
            {
                direction = 5;
            }

        } else if (direction == 3 || direction == 4) //Left
        {
            if(transform.position.x > minX)
            {
                downCounter = 0;

                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = UnityEngine.Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = UnityEngine.Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }
            
        } else if (direction == 5) //Down
        {
            downCounter++;

            if(transform.position.y > minY)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1,  room);

                try
                {
                    if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3)
                    {
                        if (downCounter >= 2)
                        {
                            roomDetection.GetComponent<RoomType>().RoomDestruction();
                            Instantiate(rooms[3], transform.position, Quaternion.identity);
                        }
                        else
                        {
                            roomDetection.GetComponent<RoomType>().RoomDestruction();
                            int randBottomRoom = UnityEngine.Random.Range(1, 4);
                            if (randBottomRoom == 2)
                            {
                                randBottomRoom = 1;
                            }
                            Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                        }
                    }
                } catch(NullReferenceException e)
                {
                    Debug.Log("Blad przy tworzeniu pokoju");
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                int rand = UnityEngine.Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = UnityEngine.Random.Range(1, 6);
            }
            else
            {
                stopGen = true;
            }
        }
    }
}
