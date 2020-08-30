using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFire : MonoBehaviour
{
    public MobHealth mobHealth;

    private void Start()
    {
        mobHealth = GetComponent<MobHealth>();
        StartCoroutine(Extinguish());
    }

    private void Update()
    {
        mobHealth.currentHealth -= 0.05f * mobHealth.health * Time.deltaTime;
    }

    IEnumerator Extinguish()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this);
    }
}
