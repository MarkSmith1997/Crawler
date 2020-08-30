using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    public GameObject clone;
    private void Start()
    {
        clone = gameObject;
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(GetComponent<Destroy>().lifeTime * .7f);
        for (int i = 0; i < 6; i++)
        {
            GameObject firedBullet = Instantiate(gameObject, transform.position, Quaternion.identity);
            Destroy(firedBullet.GetComponent<Shatter>());
            firedBullet.GetComponent<Bullet>().speed = GetComponent<Bullet>().speed;
            firedBullet.GetComponent<Bullet>().moveVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * 60 * i), Mathf.Sin(Mathf.Deg2Rad * 60 * i), 0);
            firedBullet.GetComponent<Destroy>().lifeTime = 0.3f * GetComponent<Destroy>().lifeTime;
        }
        Destroy(gameObject);
    }

    
}
