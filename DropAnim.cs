using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAnim : MonoBehaviour
{
    public Vector3 initialMoveVector;
    public Vector3 moveVector;
    public float gravity;
    public float floorHeight;
    public bool bounced;
    public float xDirBoundMin = -2f;
    public float xDirBoundMax = 2f;
    public float floorHeightBoundMin = -1.5f;
    public float floorHeightBoundMax = 1.5f;

    private void Start()
    {
        gravity = 9.81f;
        float xDir = Random.Range(xDirBoundMin, xDirBoundMax);
        float floorOffset = Random.Range(floorHeightBoundMin, floorHeightBoundMax);
        floorHeight = transform.position.y + floorOffset;
        initialMoveVector = new Vector3(xDir, 3f + floorOffset, 0);
        moveVector = initialMoveVector;
    }

    private void Update()
    {
        //add poof particles

        moveVector -= new Vector3(0, gravity, 0) * Time.deltaTime;
        transform.position += moveVector * Time.deltaTime;
        if(transform.position.y <= floorHeight && !bounced && moveVector.y < 0)
        {
            moveVector = new Vector3(moveVector.x, -moveVector.y, 0) * 0.7f;
            bounced = true;
            if(Mathf.Abs(moveVector.y) < 0.2f  || bounced)
            {
                if (GetComponent<Munee>() != null)
                {
                    GetComponent<Munee>().attract = true;
                }
                Destroy(this);     
            }
        }
        if(moveVector.y < 0)
        {
            bounced = false;
        }
    }
}
