using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI3 : MonoBehaviour
{
    //Blue
    public GameManager gameManager;
    public AudioManager audioManager;
    public GameObject player;
    public int score;
    public GameObject[] powerUps;
    public GameObject bossKey;
    public GameObject munee;
    public int muneeValue;

    //BaseStats
    public float baseSpeed;
    public float baseHealth;
    public float baseLoopRate;
    public float loopRatio;

    //Stats
    public float speed;
    public float currentHealth;
    public float health;

    //Attacking
    public float loopRate;
    public bool canFire;
    public bool canMove;
    public GameObject bullet;

    public Vector3 meToPlayer;
    public float sightRange;
    public float baseSightRange;

    public int powerID;
    public float dropChance;

    public Color myColor;
    public bool flashing;
    public GameObject[] sounds;

    private void Start()
    {
        currentHealth = baseHealth;
        health = baseHealth;
        speed = baseSpeed;
        loopRate = baseLoopRate;
        sightRange = baseSightRange;
        myColor = GetComponent<SpriteRenderer>().color;
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
        canFire = true;
        canMove = true;
    }

    private void Update()
    {
        if (flashing)
        {
            GetComponent<SpriteRenderer>().color = new Color(Mathf.Sin(Time.time * 90) * .75f + .25f, Mathf.Sin(Time.time * 90) * .75f + .25f, 0.818f);
        }

        if (score != player.GetComponent<UIController>().score)
        {
            score = player.GetComponent<UIController>().score;
            ImproveStats();
        }

        meToPlayer = player.transform.position - transform.position;
        if (meToPlayer.magnitude < 30f)
        {
            if (meToPlayer.magnitude < 25f && canFire)
            {
                StartCoroutine(Charging());
            }
            else if (canMove)
            {
                Pathfind();
            }
        }       

        if (currentHealth <= 0)
        {
            muneeValue = Random.Range(25, 125);
            for (int i = 0; i < muneeValue; i++)
            {
                Instantiate(munee, transform.position, Quaternion.identity);
            }
            if (player.GetComponent<UIController>().score >= 100 * (gameManager.numberOfKeys +1))
            {
                Instantiate(bossKey, transform.position, Quaternion.identity);
                SpawnSound(0);
                gameManager.numberOfKeys += 1;
                player.GetComponent<UIController>().score += 5;
                Destroy(gameObject);
            }
            if (Random.value < dropChance)
            {
                var number = Random.value;
                for (int i = 0; i < powerUps.Length; i++)
                {
                    if (number > ((i * 1f) / (powerUps.Length * 1f)) && number < (((i + 1) * 1f) / (powerUps.Length * 1f)))
                    {
                        powerID = i;
                    }
                }
                Instantiate(powerUps[powerID], transform.position, Quaternion.identity);
            }
            SpawnSound(0);
            player.GetComponent<UIController>().score += 5;
            Destroy(gameObject);
        }
    }

    void Fire()
    {
        SpawnSound(1);
        for (int i = 0; i < 30; i++)
        {
            GameObject firedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            firedBullet.GetComponent<Bullet>().speed *= (1f + score * 10f / 10000f);
            firedBullet.GetComponent<Bullet>().moveVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * 12 * i), Mathf.Sin(Mathf.Deg2Rad * 12 * i), 0);
        }
        StartCoroutine(Recharge());
    }

    void Move()
    {
        Vector3 moveDir = (player.transform.position - transform.position).normalized;
        transform.position += speed * moveDir * Time.deltaTime;
    }


    void Pathfind()
    {
        int layerMask = 1 << 8;
        layerMask |= 1 << 13;
        layerMask |= 1 << 14;
        layerMask |= 1 << 15;
        layerMask = ~layerMask;
        RaycastHit2D hit2DF1 = Physics2D.Raycast(transform.position + new Vector3(-meToPlayer.y, meToPlayer.x).normalized * 1.25f, meToPlayer, sightRange, layerMask);
        RaycastHit2D hit2DF2 = Physics2D.Raycast(transform.position - new Vector3(-meToPlayer.y, meToPlayer.x).normalized * 1.25f, meToPlayer, sightRange, layerMask);
        Debug.DrawLine(transform.position + new Vector3(-meToPlayer.y, meToPlayer.x).normalized * 1.25f, transform.position + new Vector3(-meToPlayer.y, meToPlayer.x).normalized * 1.25f + meToPlayer.normalized * sightRange, Color.blue, Time.deltaTime, false);
        Debug.DrawLine(transform.position - new Vector3(-meToPlayer.y, meToPlayer.x).normalized * 1.25f, transform.position - new Vector3(-meToPlayer.y, meToPlayer.x).normalized * 1.25f + meToPlayer.normalized * sightRange, Color.blue, Time.deltaTime, false);
        bool canGoLeft = true;
        bool canGoRight = true;
        if (hit2DF1)
        {
            if (hit2DF1.transform.gameObject.GetComponent<Combat>() == null)
            {
                RaycastHit2D hit2DL = Physics2D.Raycast(transform.position, (new Vector3(-meToPlayer.y, meToPlayer.x) + meToPlayer).normalized, sightRange, layerMask);
                RaycastHit2D hit2DR = Physics2D.Raycast(transform.position, (new Vector3(meToPlayer.y, -meToPlayer.x) + meToPlayer).normalized, sightRange, layerMask);
                Debug.DrawLine(transform.position, transform.position + (new Vector3(-meToPlayer.y, meToPlayer.x) + meToPlayer).normalized * sightRange, Color.blue, Time.deltaTime, false);
                Debug.DrawLine(transform.position, transform.position + (new Vector3(meToPlayer.y, -meToPlayer.x) + meToPlayer).normalized * sightRange, Color.blue, Time.deltaTime, false);
                if (hit2DR)
                {
                    if (canGoLeft)
                    {
                        transform.position += speed * new Vector3(-meToPlayer.y, meToPlayer.x).normalized * Time.deltaTime;
                        canGoRight = false;
                    }
                }
                if (hit2DL)
                {
                    if (canGoRight)
                    {
                        transform.position += speed * new Vector3(meToPlayer.y, -meToPlayer.x).normalized * Time.deltaTime;
                        canGoLeft = false;
                    }
                }
                if (!hit2DR && !hit2DL)
                {
                    sightRange += 1;
                }
            }
            if (hit2DF1.transform.gameObject.GetComponent<Combat>() != null)
            {
                canGoLeft = true;
                canGoRight = true;
                if (sightRange > baseSightRange)
                {
                    sightRange = baseSightRange;
                }
            }
        }
        else if (hit2DF2)
        {
            if (hit2DF2.transform.gameObject.GetComponent<Combat>() == null)
            {
                RaycastHit2D hit2DL = Physics2D.Raycast(transform.position, (new Vector3(-meToPlayer.y, meToPlayer.x) + meToPlayer).normalized, sightRange, layerMask);
                RaycastHit2D hit2DR = Physics2D.Raycast(transform.position, (new Vector3(meToPlayer.y, -meToPlayer.x) + meToPlayer).normalized, sightRange, layerMask);
                Debug.DrawLine(transform.position, transform.position + (new Vector3(-meToPlayer.y, meToPlayer.x) + meToPlayer).normalized * sightRange, Color.green, Time.deltaTime, false);
                Debug.DrawLine(transform.position, transform.position + (new Vector3(meToPlayer.y, -meToPlayer.x) + meToPlayer).normalized * sightRange, Color.green, Time.deltaTime, false);
                if (hit2DR)
                {
                    if (canGoLeft)
                    {
                        transform.position += speed * new Vector3(-meToPlayer.y, meToPlayer.x).normalized * Time.deltaTime;
                        canGoRight = false;
                    }
                }
                if (hit2DL)
                {
                    if (canGoRight)
                    {
                        transform.position += speed * new Vector3(meToPlayer.y, -meToPlayer.x).normalized * Time.deltaTime;
                        canGoLeft = false;
                    }
                }
                if (!hit2DR && !hit2DL)
                {
                    sightRange += 1;
                }
            }
            if (hit2DF2.transform.gameObject.GetComponent<Combat>() != null)
            {
                canGoLeft = true;
                canGoRight = true;
                if (sightRange > baseSightRange)
                {
                    sightRange = baseSightRange;
                }
            }
        }
        //Combat
        else
        {
            Move();          
            if (sightRange > baseSightRange)
            {
                sightRange = baseSightRange;
                canGoLeft = true;
                canGoRight = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "PlayerBullet")
        {
            flashing = true;
            StartCoroutine(flash());
            transform.position += (transform.position - player.transform.position).normalized * collision.transform.GetComponent<Bullet>().knockbackStrength;
            currentHealth -= collision.transform.GetComponent<Bullet>().damage;
        }
    }

    void SpawnSound(int soundIndex)
    {
        GameObject sound = Instantiate(sounds[soundIndex], transform.position, Quaternion.identity);
        sound.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("soundEffectsVolume");
    }

    void ImproveStats()
    {
        float healthPercent = currentHealth / health;
        health = baseHealth * (1f + score * 100f / 10000f);
        speed = baseSpeed * (1f + score * 20f / 10000f);
        loopRate = baseLoopRate * (1f + score * 44f / 10000f);
        currentHealth = health * healthPercent;
    }

    IEnumerator flash()
    {
        yield return new WaitForSeconds(0.25f);
        flashing = false;
        GetComponent<SpriteRenderer>().color = myColor;
    }

    IEnumerator Charging()
    {
        canMove = false;
        canFire = false;
        yield return new WaitForSeconds(1 / loopRate);
        Fire();
    }

    IEnumerator Recharge()
    {
        canMove = true;
        yield return new WaitForSeconds(loopRatio / loopRate);
        canFire = true;
    }
    /*private void OnBecameVisible()
    {
        audioManager.weights[2] += 0.2f;
    }
    private void OnBecameInvisible()
    {
        audioManager.weights[2] -= 0.2f;
    }*/
}
