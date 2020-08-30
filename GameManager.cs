using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    //Legacy spawning
    //public List<GameObject> currentRooms;


    public GameObject world;
    public GameObject rooms;

    //Bossroom Statics
    public int bossRoomSize;
    public Tilemap floorTilemapRoom;
    public Tilemap wallTilemapRoom;
    public Tilemap ceilingTilemapRoom;
    public Tilemap minimapTilemapRoom;

    public int numberOfBosses;
    public int numberOfKeys;
    public int numberOfShops;
    public int numberOfAltars;


    public GameObject player;

    //Statics
    public int regionDiameter;
    public float frequency;
    public int seed;
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public Tilemap ceilingTilemap;
    public Tilemap minimapTilemap;
    public TileBase[] tiles;
    public int theme;
    public int themeThreshhold;
    public float[] threshhold;
    public bool[] shading;
    public bool firstUpdate;
    public int spawnRoomSize;

    //Dynamics
    public GameObject enemySpawner;
    public GameObject[] enemies;
    public int monsterID;

    public GameObject bossDoor;
    public GameObject boss;

    public GameObject relixAltar;
    public GameObject shop;
    public GameObject chest;
    public GameObject key;

    public bool spawningOn;
    public bool canSpawn;

    private void Start()
    {
        seed = Random.Range(0, 99999);
        firstUpdate = true;
        UpdateTiles();
    }

    private void Update()
    {

        //Legacy room spawning
        /*if (player.GetComponent<UIController>().score >= 100 * (numberOfBosses + 1))
        {
            numberOfBosses += 1;
            foreach (GameObject room in currentRooms)
            {
                room.GetComponent<SpawnNextRoom>().willBeBossRoom = true;
            }
        }*/

        if ((player.transform.position - transform.position).magnitude > regionDiameter / 6)
        {
            transform.position = new Vector3((int)player.transform.position.x, (int)player.transform.position.y, 0);
            UpdateTiles();
        }

    }

    float Sample(int x, int y)
    {
        float frequency_ = frequency;// ((float)theme + 1);
        return Mathf.PerlinNoise((float)x / regionDiameter * frequency_ + seed + transform.position.x / (regionDiameter / frequency_),
                                 (float)y / regionDiameter * frequency_ + seed + transform.position.y / (regionDiameter / frequency_));
    }

    bool CheckTile(int x, int y)
    {
        bool check = wallTilemap.GetTile(new Vector3Int(x, y, 0)) != null;
        return check;
    }

    void UpdateTiles()
    {        
        for (int x = 0; x < regionDiameter; x++)
        {
            for (int y = 0; y < regionDiameter; y++)
            {
                theme = (int)(new Vector3(transform.position.x - regionDiameter / 2 + x, transform.position.y - regionDiameter / 2 + y, 0).magnitude / themeThreshhold); 

                float sample = Sample(x, y);
                //Checks if the tile has already been drawn
                if (floorTilemap.GetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0)) != null)
                {
                    continue;
                }

               
                //Checks if tile is wall or roof
                if (sample > threshhold[0])
                {   
                    wallTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), tiles[11 + 14 * theme]);
                    ceilingTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y + 1, 0), tiles[12 + 14 * theme]);
                }

                //Checks if tile is floor
                if (sample <= threshhold[0])
                {
                    if (spawningOn)
                    {
                        if ((new Vector3(x, y, 0) - new Vector3(transform.position.x + regionDiameter / 2, transform.position.y + regionDiameter / 2, 0)).magnitude < spawnRoomSize * 1.5f)
                        {
                            canSpawn = false;
                        }
                        else
                        {
                            canSpawn = true;
                        }
                    }
                    

                    // Spawns PsoI
                    if (sample <= 0.25f * threshhold[0])
                    {
                        //Calculates the distance from the center
                        float distanceFromCenter = new Vector3(transform.position.x - regionDiameter / 2 + x, transform.position.y - regionDiameter / 2 + y, 0).magnitude;

                        //Spaces boss doors
                        if (distanceFromCenter >= 160 * (numberOfBosses + 1) + 100)
                        {
                            GameObject room = Instantiate(bossDoor, new Vector3(transform.position.x - regionDiameter / 2 + x + 0.5f, transform.position.y - regionDiameter / 2 + y + 0.5f, 0), Quaternion.identity);
                            room.GetComponent<BossDoor>().world = world;
                            room.GetComponent<BossDoor>().rooms = rooms;
                            GameObject boss_ = Instantiate(boss, new Vector3(transform.position.x - regionDiameter / 2 + x + 0.5f, transform.position.y - regionDiameter / 2 + y + 10.5f, 0), Quaternion.identity, rooms.transform);
                            room.GetComponent<BossDoor>().boss = boss_;
                            //Paints boss rooms
                            for (int x_ = 0; x_ < bossRoomSize; x_++)
                            {
                                for (int y_ = 0; y_ < bossRoomSize; y_++)
                                {
                                    floorTilemapRoom.SetTile(new Vector3Int((int)room.transform.position.x - bossRoomSize / 2 + x_, (int)room.transform.position.y - bossRoomSize / 2 + y_ + 10, 0), tiles[0 + 14 * theme]);
                                    if (x_ == 0 || x_ == bossRoomSize - 1)
                                    {
                                        wallTilemapRoom.SetTile(new Vector3Int((int)room.transform.position.x - bossRoomSize / 2 + x_, (int)room.transform.position.y - bossRoomSize / 2 + y_ + 10, 0), tiles[11 + 14 * theme]);
                                        ceilingTilemapRoom.SetTile(new Vector3Int((int)room.transform.position.x - bossRoomSize / 2 + x_, (int)room.transform.position.y - bossRoomSize / 2 + y_ + 11, 0), tiles[12 + 14 * theme]);
                                    }
                                    if (y_ == 0 || y_ == bossRoomSize - 1)
                                    {
                                        wallTilemapRoom.SetTile(new Vector3Int((int)room.transform.position.x - bossRoomSize / 2 + x_, (int)room.transform.position.y - bossRoomSize / 2 + y_ + 10, 0), tiles[11 + 14 * theme]);
                                        ceilingTilemapRoom.SetTile(new Vector3Int((int)room.transform.position.x - bossRoomSize / 2 + x_, (int)room.transform.position.y - bossRoomSize / 2 + y_ + 11, 0), tiles[12 + 14 * theme]);
                                    }
                                }
                            }

                            numberOfBosses += 1;
                        }

                        if (distanceFromCenter >= 160 * (numberOfShops + 1) + 50)
                        {
                            Instantiate(shop, new Vector3(transform.position.x - regionDiameter / 2 + x + 0.5f, transform.position.y - regionDiameter / 2 + y + 0.5f, 0), Quaternion.identity);
                            numberOfShops += 1;
                        }

                        if (distanceFromCenter >= 300 * (numberOfAltars + 1))
                        {
                            Instantiate(relixAltar, new Vector3((int)transform.position.x - regionDiameter / 2 + x + 0.5f, (int)transform.position.y - regionDiameter / 2 + y + 0.5f, 0), Quaternion.identity, world.transform);
                            numberOfAltars += 1;
                        }
                        //Spawns spawners
                        float value = Random.value;
                        if (value <= 0.01f && canSpawn)
                        {
                            Instantiate(enemySpawner, new Vector3((int)transform.position.x - regionDiameter / 2 + x + 0.5f, (int)transform.position.y - regionDiameter / 2 + y + 0.5f, 0), Quaternion.identity, world.transform);
                            Instantiate(enemies[4], new Vector3((int)transform.position.x - regionDiameter / 2 + x + 0.5f, (int)transform.position.y - regionDiameter / 2 + y + 0.5f, 0), Quaternion.identity, world.transform);
                            
                        }

                        //Spawns chests
                        if (value > 0.01f && value <= 0.013f && canSpawn)
                        {
                            Instantiate(chest, new Vector3((int)transform.position.x - regionDiameter / 2 + x + 0.5f, (int)transform.position.y - regionDiameter / 2 + y + 0.5f, 0), Quaternion.identity, world.transform);
                            Instantiate(key, new Vector3((int)transform.position.x - regionDiameter / 2 + x + 0.5f, (int)transform.position.y - regionDiameter / 2 + y + 0.5f, 0), Quaternion.identity, world.transform);
                        }
                    }

                    //Spawn enemies on floor tiles
                    if (Random.value < 0.004f && canSpawn)
                    {
                        var number = Random.value;
                        if (number < .4f)
                        {
                            monsterID = 0;
                        }
                        if (number > .4f && number < .8f)
                        {
                            monsterID = 1;
                        }
                        if (number > .8f && number < 0.99f)
                        {
                            monsterID = 2;
                        }
                        if( number > 0.99f)
                        {
                            monsterID = 3;
                        }
                        Instantiate(enemies[monsterID], new Vector3((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), Quaternion.identity, world.transform);
                    }

                    minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), tiles[13 + 14 * theme]);

                    //Contains shading bools and floor tile selection
                    #region
                    for (int i = 0; i < shading.Length; i++)
                    {
                        shading[i] = false;
                    }

                    //Checks Left
                    if (Sample(x - 1, y) > threshhold[0])
                    {
                        shading[0] = true;
                    }

                    //Checks TopLeft
                    if (Sample(x - 1, y + 1) > threshhold[0])
                    {
                        shading[1] = true;
                    }

                    //Checks Top
                    if (Sample(x, y + 1) > threshhold[0])
                    {
                        shading[2] = true;
                    }

                    //Checks TopRight
                    if (Sample(x + 1, y + 1) > threshhold[0])
                    {
                        shading[3] = true;
                    }

                    //Checks Right
                    if (Sample(x + 1, y) > threshhold[0])
                    {
                        shading[4] = true;
                    }

                    //Left+Top+Right
                    if (shading[0] && shading[2] && shading[4])
                    {
                        floorTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), tiles[7 + 14 * theme]);
                    }

                    //Left+Top
                    else if (shading[0] && shading[2])
                    {
                        floorTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), tiles[5 + 14 * theme]);
                    }

                    //Top+Right
                    else if (shading[2] && shading[4])
                    {
                        floorTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), tiles[6 + 14 * theme]);
                    }

                    //Left+Right
                    else if (shading[0] && shading[4])
                    {
                        floorTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), tiles[4 + 14 * theme]);
                    }

                    //Top
                    else if (shading[2])
                    {
                        floorTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), tiles[1 + 14 * theme]);
                    }

                    //Left
                    else if (shading[0])
                    {
                        floorTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), tiles[2 + 14 * theme]);
                    }

                    //Right
                    else if (shading[4])
                    {
                        floorTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), tiles[3 + 14 * theme]);
                    }

                    //Left+RightCorner
                    else if (shading[1] && shading[3])
                    {
                        floorTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), tiles[10 + 14 * theme]);
                    }

                    //LeftCorner
                    else if (shading[1])
                    {
                        floorTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), tiles[8 + 14 * theme]);
                    }

                    //RightCorner
                    else if (shading[3])
                    {
                        floorTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), tiles[9 + 14 * theme]);
                    }

                    else
                    {
                        floorTilemap.SetTile(new Vector3Int((int)transform.position.x - regionDiameter / 2 + x, (int)transform.position.y - regionDiameter / 2 + y, 0), tiles[0 + 14 * theme]);
                    }
#endregion 
                }
            }
        }

        if (firstUpdate)
        {
            //structure
            for (int x = 1; x < spawnRoomSize - 1; x++)
            {
                for (int y = 1; y < spawnRoomSize - 1; y++)
                {
                    wallTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), null);
                    ceilingTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y + 1, 0), null);
                }
            }
            //shading
            for (int x = 0; x < spawnRoomSize; x++)
            {
                for (int y = 0; y < spawnRoomSize; y++)
                {
                    if (x == 1 || x == spawnRoomSize - 2 || y == 1 || y == spawnRoomSize - 2)
                    {
                        
                        for (int i = 0; i < shading.Length; i++)
                        {
                            shading[i] = false;
                        }
                        //Checks Left
                        if (CheckTile((int)transform.position.x - spawnRoomSize / 2 + x - 1, (int)transform.position.y - spawnRoomSize / 2 + y))
                        {
                            shading[0] = true;
                        }
                        //Checks TopLeft
                        if (CheckTile((int)transform.position.x - spawnRoomSize / 2 + x - 1, (int)transform.position.y - spawnRoomSize / 2 + y + 1))
                        {
                            shading[1] = true;
                        }
                        //Checks Top
                        if (CheckTile((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y + 1))
                        {
                            shading[2] = true;
                        }
                        //Checks TopRight
                        if (CheckTile((int)transform.position.x - spawnRoomSize / 2 + x + 1, (int)transform.position.y - spawnRoomSize / 2 + y + 1))
                        {
                            shading[3] = true;
                        }
                        //Checks Right
                        if (CheckTile((int)transform.position.x - spawnRoomSize / 2 + x + 1, (int)transform.position.y - spawnRoomSize / 2 + y))
                        {
                            shading[4] = true;
                        }
                        //Left+Top+Right
                        if (shading[0] && shading[2] && shading[4])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[7 + 14 * theme]);                            
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Left+Top
                        else if (shading[0] && shading[2])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[5 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Top+Right
                        else if (shading[2] && shading[4])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[6 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Left+Right
                        else if (shading[0] && shading[4])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[4 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Top
                        else if (shading[2])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[1 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Left
                        else if (shading[0])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[2 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Right
                        else if (shading[4])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[3 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Left+RightCorner
                        else if (shading[1] && shading[3])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[10 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //LeftCorner
                        else if (shading[1])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[8 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //RightCorner
                        else if (shading[3])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[9 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        else
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[0 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }

                    }
                    else
                    {
                        floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[0 + 14 * theme]);
                        minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                    }
                    if (x == 0 || x == spawnRoomSize - 1 || y == 0 || y == spawnRoomSize - 1)
                    {

                        for (int i = 0; i < shading.Length; i++)
                        {
                            shading[i] = false;
                        }
                        //Checks Left
                        if (CheckTile((int)transform.position.x - spawnRoomSize / 2 + x - 1, (int)transform.position.y - spawnRoomSize / 2 + y))
                        {
                            shading[0] = true;
                        }
                        //Checks TopLeft
                        if (CheckTile((int)transform.position.x - spawnRoomSize / 2 + x - 1, (int)transform.position.y - spawnRoomSize / 2 + y + 1))
                        {
                            shading[1] = true;
                        }
                        //Checks Top
                        if (CheckTile((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y + 1))
                        {
                            shading[2] = true;
                        }
                        //Checks TopRight
                        if (CheckTile((int)transform.position.x - spawnRoomSize / 2 + x + 1, (int)transform.position.y - spawnRoomSize / 2 + y + 1))
                        {
                            shading[3] = true;
                        }
                        //Checks Right
                        if (CheckTile((int)transform.position.x - spawnRoomSize / 2 + x + 1, (int)transform.position.y - spawnRoomSize / 2 + y))
                        {
                            shading[4] = true;
                        }
                        //Left+Top+Right
                        if (shading[0] && shading[2] && shading[4])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[7 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Left+Top
                        else if (shading[0] && shading[2])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[5 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Top+Right
                        else if (shading[2] && shading[4])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[6 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Left+Right
                        else if (shading[0] && shading[4])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[4 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Top
                        else if (shading[2])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[1 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Left
                        else if (shading[0])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[2 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Right
                        else if (shading[4])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[3 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //Left+RightCorner
                        else if (shading[1] && shading[3])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[10 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //LeftCorner
                        else if (shading[1])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[8 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        //RightCorner
                        else if (shading[3])
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[9 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }
                        else
                        {
                            floorTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[0 + 14 * theme]);
                            minimapTilemap.SetTile(new Vector3Int((int)transform.position.x - spawnRoomSize / 2 + x, (int)transform.position.y - spawnRoomSize / 2 + y, 0), tiles[13 + 14 * theme]);
                        }

                    }

                }
            }
            firstUpdate = false;
        }
    }
}
