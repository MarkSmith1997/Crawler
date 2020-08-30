using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float speed;
    public Vector3 moveVector;
    public SpriteRenderer spriteRenderer;
    public float fadeDelay;
    public float knockbackStrength;

    private void Start()
    {
        fadeDelay = 0.8f * GetComponent<Destroy>().lifeTime;
    }

    private void Update()
    {

        //Bullet Behaviour
        StandardBulletBehaviour();

        //Controls fade
        Color tempColor = spriteRenderer.color;
        fadeDelay -= Time.deltaTime;
        if (fadeDelay <= 0)
        {
            tempColor.a -= Time.deltaTime / (GetComponent<Destroy>().lifeTime * 0.2f);
            spriteRenderer.color = tempColor;
        }
    }

    void StandardBulletBehaviour()
    {
        transform.position += speed * Time.deltaTime * moveVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

}
