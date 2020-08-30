using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucky : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MobHealth>() != null && collision.gameObject.GetComponent<IncreasedLuck>() == null)
        {
            collision.gameObject.AddComponent<IncreasedLuck>();
            collision.gameObject.GetComponent<MobHealth>().dropChance *= 2f;
        }
    }
}
