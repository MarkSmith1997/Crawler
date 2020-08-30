using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frozen : MonoBehaviour
{
    public MonoBehaviour[] comps;
    private void Start()
    {
        comps = GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour c in comps)
        {
            c.enabled = false;
        }
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<MobHealth>().enabled = true;
        if(GetComponent<BossAI1>() != null)
        {
            GetComponent<BossAI1>().enabled = true;
        }
        enabled = true;
        StartCoroutine(Thaw());
    }

    IEnumerator Thaw()
    {
        yield return new WaitForSeconds(2f);
        foreach(MonoBehaviour c in comps)
        {
            if(c == null)
            {
                continue;
            }
            c.enabled = true;
        }
        Destroy(this);
    }
}
