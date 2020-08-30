using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorParent : MonoBehaviour
{
    public GameObject player;
    public GameObject child;
    public float distance;

    private void Update()
    {
        distance = (player.transform.position - transform.position).magnitude;

        if (distance < 20)
        {
            child.SetActive(true);
        }
    }
}
