using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;

    private void Update()
    {
        if (player.GetComponent<Combat>().usingController)
        {
            transform.position = (player.transform.position + new Vector3(Input.GetAxis("AimX"), -Input.GetAxis("AimY"))) - new Vector3(0, 0, 10);
        }
        else
        {
            transform.position = (player.transform.position * 1.8f + Camera.main.ScreenToWorldPoint(Input.mousePosition) * 0.2f) / 2 - new Vector3(0, 0, 10);
        }
    }

}
