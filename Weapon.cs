using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float gunQuality;
    public ParticleSystem qualityEffect;
    public Color qualityColor;
    //BaseStats
    public float baseDamage;
    public float baseCritChance;
    public float baseCritDamage;
    public float baseFireRate;

    public float baseRange;

    public int baseProjectiles;

    //CurrentStats
    public float damage;
    public float critChance;
    public float critDamage;
    public float fireRate;

    public float range;
    public float knockbackStrength;
    
    public int projectiles;

    //Mechs
    public float counter;
    public GameObject bullet;
    public bool canFire;
    public Vector3 startPosition;

    public GameObject weaponSound;

    public Combat combat;

    private void Start()
    {
        canFire = true;
        damage = baseDamage;
        critChance = baseCritChance;
        critDamage = baseCritDamage;
        fireRate = baseFireRate;
        projectiles = baseProjectiles;
        range = baseRange;

        var renderer = qualityEffect.GetComponent<ParticleSystemRenderer>();
        if (gunQuality == 1)
        {
            qualityColor = Color.red;
        }
        if (gunQuality >= 0.9f && gunQuality < 1f)
        {
            qualityColor = new Color(1f, 0.6f, 0.2f);
        }
        if (gunQuality >= 0.8f && gunQuality < 0.9f)
        {
            qualityColor = new Color(0.6f, 0.2f, 1f);
        }
        if (gunQuality >= 0.7f && gunQuality < 0.8f)
        {
            qualityColor = new Color(0.2f, 0.2f, 1f);
        }
        if (gunQuality >= 0.6f && gunQuality < 0.7f)
        {
            qualityColor = new Color(0.2f, 1f, .2f);
        }
        if (gunQuality >= 0.5f && gunQuality < 0.6f)
        {
            qualityColor = new Color(.85f, .85f, .85f);
        }
        renderer.material.color = qualityColor;
    }

    private void Update()
    {
        if (counter > 0)
        {
            counter -= Time.deltaTime;
        }
        if(counter <= 0)
        {
            canFire = true;
        }

        if (transform.parent != null)
        {
            qualityEffect.gameObject.SetActive(false);
        }
        else if (!qualityEffect.gameObject.activeSelf)
        {
            qualityEffect.gameObject.SetActive(true);
        }

    }

    public void Fire()
    {

        if (canFire)
        {
            GameObject shot = Instantiate(weaponSound, transform.position, Quaternion.identity);
            shot.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("soundEffectsVolume");
            canFire = false;
            counter = 1 / fireRate;
            for (int i = 0; i < projectiles; i++)
            {
                /*Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                startPosition = transform.position + (new Vector3(-(mousePos.y - transform.position.y), mousePos.x - transform.position.x, 0).normalized * (projectiles - 1) * 0.5f);
                GameObject firedBullet = Instantiate(bullet, startPosition, Quaternion.identity);
                firedBullet.transform.position -=new Vector3(-(mousePos.y - transform.position.y), mousePos.x - transform.position.x, 0).normalized * i;
                firedBullet.GetComponent<Bullet>().moveVector = ((new Vector3(mousePos.x, mousePos.y)) - transform.position).normalized;*/

                Vector3 mousePos = new Vector3();
                GameObject firedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                if (transform.parent.GetComponent<Combat>().usingController)
                {
                    mousePos = transform.position + (transform.position - transform.parent.position);
                }
                else
                {
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                firedBullet.GetComponent<Bullet>().moveVector = (((new Vector3(mousePos.x, mousePos.y)) + (new Vector3(-(mousePos.y - transform.position.y), mousePos.x - transform.position.x, 0) * (0.25f / projectiles) * (projectiles - 1) * .5f) - new Vector3(-(mousePos.y - transform.position.y), mousePos.x - transform.position.x, 0) * (0.25f / projectiles) * i) - transform.position).normalized;
                firedBullet.GetComponent<Bullet>().knockbackStrength = knockbackStrength;
                firedBullet.GetComponent<Destroy>().lifeTime = range / firedBullet.GetComponent<Bullet>().speed;
                if (Random.value < critChance)
                {
                    firedBullet.GetComponent<Bullet>().damage = damage * critDamage;
                    firedBullet.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                else
                {
                    firedBullet.GetComponent<Bullet>().damage = damage;
                }

                if (combat.relix[0])
                {
                    firedBullet.AddComponent<Fire>();
                }
                if (combat.relix[1])
                {
                    firedBullet.AddComponent<Freeze>();
                }
                if (combat.relix[2])
                {
                    firedBullet.AddComponent<Shatter>();
                }
                if (combat.relix[3])
                {
                    firedBullet.AddComponent<Lucky>();
                }
            }           
        }
    }
}
