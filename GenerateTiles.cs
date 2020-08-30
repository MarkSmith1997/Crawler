using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GenerateTiles : MonoBehaviour
{
    public int regionDiameter;
    public int frequency;
    public int seed;
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public TileBase[] tiles;

    public float[] threshhold;

    private void Start()
    {
        UpdateTiles();
    }

    float Sample(int x, int y)
    {
        return Mathf.PerlinNoise(((float)x / regionDiameter) * frequency + seed + transform.position.x/(2*frequency), (float)y / regionDiameter * frequency + seed + transform.position.y/ (2 * frequency));
    }

    void UpdateTiles()
    {
        for (int x = 0; x < regionDiameter; x++)
        {
            for (int y = 0; y < regionDiameter; y++)
            {
                float sample = Sample(x, y);



                //Checks if tile is floor
                if (sample <= threshhold[0])
                {
                    floorTilemap.SetTile(new Vector3Int(-regionDiameter / 2 + x, -regionDiameter / 2 + y, 0), tiles[0]);
                }
                //Checks if tile is wall or roof
                if (sample > threshhold[0] && sample < threshhold[1])
                {
                    //Checks if tile is roof
                    if (Sample(x, y - 1) > threshhold[0])
                    {
                        wallTilemap.SetTile(new Vector3Int(-regionDiameter / 2 + x, -regionDiameter / 2 + y, 0), tiles[2]);
                    }
                    //else its a wall
                    else
                    {
                        wallTilemap.SetTile(new Vector3Int(-regionDiameter / 2 + x, -regionDiameter / 2 + y, 0), tiles[1]);
                    }
                }
            }
        }
    }

}
