using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Frozen>() == null && collision.gameObject.GetComponent<MobHealth>() != null)
        {
            collision.gameObject.AddComponent<Frozen>();
        }
    }
}
