using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI1 : MonoBehaviour
{
    public GameObject player;
    public GameManager gameManager;
    public MobHealth mobHealth;
    public int score;
    public GameObject ghost;

    //BaseStats
    public float baseFireRate;

    //Attacking
    public float counter;
    public bool canFire;
    public float counter2;
    public bool canBurst;
    public float fireRate;
    public GameObject bullet;

    public GameObject healthBar;
    public RectTransform healthBarTransform;
    public bool isControllingHealthBar;

    public AudioSource myAudio;

    public GameObject[] weapon;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = FindObjectOfType<GameManager>();
        mobHealth = GetComponent<MobHealth>();
        baseFireRate *= PlayerPrefs.GetFloat("difficulty");
        fireRate = baseFireRate;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar");
        healthBarTransform = healthBar.GetComponent<RectTransform>();
        canFire = true;
        canBurst = true;
    }

    private void Update()
    {
        if (mobHealth.currentHealth <= 0)
        {
            SpawnRandomWeapon();
            player.GetComponent<UIController>().bossesBeaten += 1;
            Instantiate(ghost, Vector3.zero, Quaternion.identity);
        }

        if (score != player.GetComponent<UIController>().bossesBeaten)
        {
            score = player.GetComponent<UIController>().bossesBeaten;
            ImproveStats();
        }
        if (isControllingHealthBar)
        {
            healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(100 * mobHealth.currentHealth / (mobHealth.health), healthBarTransform.sizeDelta.y);
            myAudio.volume = PlayerPrefs.GetFloat("musicVolume");
        }
        Fire();

        if (counter > 0)
        {
            counter -= Time.deltaTime;
        }
        if (counter < 0)
        {
            canFire = true;
        }
        if (counter2 > 0)
        {
            counter2 -= Time.deltaTime;
        }
        if (counter2 < 0)
        {
            canBurst = true;
        }
    }

    void Fire()
    {
        if (canFire)
        {
            canFire = false;
            counter = 1 / fireRate;
            GameObject firedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            firedBullet.GetComponent<Bullet>().moveVector = (player.transform.position - transform.position).normalized;
            if(mobHealth.currentHealth < 0.66f * mobHealth.health)
            {
                GameObject firedBullet2 = Instantiate(bullet, transform.position, Quaternion.identity);
                var offset = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * player.GetComponent<Movement>().speed;
                firedBullet2.GetComponent<Bullet>().moveVector = (offset * 2f + player.transform.position - transform.position).normalized;
            }
        }
        if (canBurst)
        {
            canBurst = false;
            counter2 = 10 / fireRate;
            if (mobHealth.currentHealth < 0.33f * mobHealth.health)
            {
                for (int i = 0; i < 30; i++)
                {
                    GameObject firedBullet3 = Instantiate(bullet, transform.position, Quaternion.identity);
                    firedBullet3.GetComponent<Bullet>().moveVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * 12 * i), Mathf.Sin(Mathf.Deg2Rad * 12 * i), 0);
                }
            }
        }
    }

    void ImproveStats()
    {
        fireRate = baseFireRate * (player.GetComponent<UIController>().bossesBeaten * 0.44f + 1);
    }

    void SpawnRandomWeapon()
    {
        int id = Random.Range(0, weapon.Length - 1);
        GameObject randomWeapon = Instantiate(weapon[id], transform.position, Quaternion.identity);
        randomWeapon.AddComponent<DropAnim>();
        float baseDamage = randomWeapon.GetComponent<Weapon>().baseDamage * 2f;
        float baseCritChance = randomWeapon.GetComponent<Weapon>().baseCritChance * 2f;
        float baseCritDamage = randomWeapon.GetComponent<Weapon>().baseCritDamage * 2f;
        float baseFireRate = randomWeapon.GetComponent<Weapon>().baseFireRate * 2f;
        randomWeapon.GetComponent<Weapon>().baseDamage = Random.Range(randomWeapon.GetComponent<Weapon>().baseDamage * 1.8f, randomWeapon.GetComponent<Weapon>().baseDamage * 2f);
        randomWeapon.GetComponent<Weapon>().baseCritChance = Random.Range(randomWeapon.GetComponent<Weapon>().baseCritChance * 1.8f, randomWeapon.GetComponent<Weapon>().baseCritChance * 2f);
        randomWeapon.GetComponent<Weapon>().baseCritDamage = Random.Range(randomWeapon.GetComponent<Weapon>().baseCritDamage * 1.8f, randomWeapon.GetComponent<Weapon>().baseCritDamage * 2f);
        randomWeapon.GetComponent<Weapon>().baseFireRate = Random.Range(randomWeapon.GetComponent<Weapon>().baseFireRate * 1.8f, randomWeapon.GetComponent<Weapon>().baseFireRate * 2f);
        randomWeapon.GetComponent<Weapon>().gunQuality = ((randomWeapon.GetComponent<Weapon>().baseDamage / baseDamage) + (randomWeapon.GetComponent<Weapon>().baseCritChance / baseCritChance)
                                                        + (randomWeapon.GetComponent<Weapon>().baseCritDamage / baseCritDamage) + (randomWeapon.GetComponent<Weapon>().baseFireRate / baseFireRate)) / 4f;
    }

    private void OnBecameVisible()
    {
        Camera.main.GetComponent<AudioSource>().mute = true;
        myAudio.mute = false;
        isControllingHealthBar = true;
    }

    private void OnBecameInvisible()
    {
        Camera.main.GetComponent<AudioSource>().mute = false;
    }
}