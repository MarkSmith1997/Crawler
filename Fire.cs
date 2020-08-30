using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<OnFire>() == null && collision.gameObject.GetComponent<MobHealth>() != null)
        {
            collision.gameObject.AddComponent<OnFire>();
        }
    }
}
