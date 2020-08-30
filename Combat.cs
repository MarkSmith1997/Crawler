using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{
    public GameObject hand;
    public GameObject currentWeapon;
    public GameObject rangeIndicator;
    public bool canPickUp;
    public bool usingController;

    public int health;

    public bool flashing;
    public bool immune;
    public Color myColor;

    //buffs
    public int numAttackDamageUp;
    public int numAttackSpeedUp;
    public int numMoveSpeedUp;
    public int numProjUp;
    public int numCritChanceUp;
    public int numCritDamUp;
    public int numRangeUp;

    //Relix
    public bool[] relix;

    //Misc
    public int bossKeys;
    public int genericKeys;
    public int munees;

    private void Start()
    {
        myColor = GetComponent<SpriteRenderer>().color;
        immune = false;
        canPickUp = true;
        usingController = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && usingController)
        {
            usingController = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetAxis("Fire1") == 1)
        {
            if (currentWeapon != null)
            {
                currentWeapon.GetComponent<Weapon>().Fire();
            }          
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentWeapon == null)
            {
                GetComponent<UIController>().NoWeaponFade();
            }
        }
        //Uses Controller
        if (Mathf.Abs(Input.GetAxis("AimX")) == 1 || Mathf.Abs(Input.GetAxis("AimY")) == 1 && !usingController)
        {
            usingController = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (usingController)
        {
            hand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan(-Input.GetAxis("AimY") / Input.GetAxis("AimX")) * (180 * Mathf.PI) / 10f));
            hand.transform.position = transform.position + new Vector3(Input.GetAxis("AimX"), -Input.GetAxis("AimY"), 0).normalized;
        }
        if(!usingController)
        {
            hand.transform.position = transform.position + (new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y) - transform.position).normalized;
            hand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan((Input.mousePosition.y - (Screen.height / 2)) / (Input.mousePosition.x - (Screen.width / 2))) * (180 * Mathf.PI) / 10f));
        }
        //Flips the gun
        if (hand.transform.position.x < transform.position.x)
        {
            hand.transform.localScale = new Vector3(-1, 1);
        }
        else
        {
            hand.transform.localScale = new Vector3(1, 1);
        }

        if (currentWeapon != null)
        {
            if (usingController)
            {
                rangeIndicator.transform.position = transform.position + new Vector3(Input.GetAxis("AimX"), -Input.GetAxis("AimY"), 0).normalized * (currentWeapon.GetComponent<Weapon>().range + new Vector3(Input.GetAxis("AimX"), -Input.GetAxis("AimY"), 0).magnitude);
            }
            else
            {
                rangeIndicator.transform.position = transform.position + (new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y) - transform.position).normalized * (currentWeapon.GetComponent<Weapon>().range + 1);
            }
            currentWeapon.transform.position = hand.transform.position;
            currentWeapon.transform.localScale = hand.transform.localScale;
            currentWeapon.transform.rotation = hand.transform.rotation;
        }

        if (health <= 0)
        {
            if (PlayerPrefs.GetInt("highscore") < GetComponent<UIController>().score)
            {
                PlayerPrefs.SetInt("highscore", GetComponent<UIController>().score);
            }
            GetComponent<Movement>().enabled = false;
            currentWeapon = null;
            Camera.main.GetComponent<CameraMovement>().enabled = false;
            GetComponent<UIController>().SetDeathText();
            StartCoroutine(StartAgain());
        }

        if (flashing)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, Mathf.Sin(Time.time * 90) * 0.5f + 0.5f, Mathf.Sin(Time.time * 90) * 0.5f + 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Weapon" && canPickUp)
        {
            canPickUp = false;
            StartCoroutine(pickUpCoolDown());
            if (currentWeapon != null)
            {
                currentWeapon.transform.parent = null;
                currentWeapon.AddComponent<DropAnim>();
            }
            if (collision.GetComponent<Weapon>() == null)
            {
                Destroy(collision.gameObject);
                currentWeapon = null;
            }
            else
            {
                currentWeapon = collision.gameObject;
                currentWeapon.transform.position = transform.position;
                currentWeapon.transform.parent = gameObject.transform;
                currentWeapon.GetComponent<Weapon>().combat = this;
                UpdateStats();
            }
            
            if (currentWeapon != null)
            {
                 if(currentWeapon.GetComponent<DropAnim>() != null)
                {
                    Destroy(currentWeapon.GetComponent<DropAnim>());
                }
            
            }       
        }

        if (collision.transform.tag == "Munee")
        {
            munees += 1;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TakeDamage
        if (collision.transform.tag == "EnemyBullet" && !immune)
        {
            health -= 1;
            flashing = true;
            immune = true;
            StartCoroutine(flash());
        }

        //Buffs
        if (collision.transform.tag == "HealthUp" && health < 99)
        {
            health += 1;
            Destroy(collision.gameObject);
        }
        if(collision.transform.tag == "AttackDUp")
        {
            numAttackDamageUp += 1;
            UpdateStats();
            Destroy(collision.gameObject);
            GetComponent<UIController>().UpdateUI(0);
        }
        if (collision.transform.tag == "AttackSUp")
        {
            numAttackSpeedUp += 1;
            UpdateStats();
            Destroy(collision.gameObject);
            GetComponent<UIController>().UpdateUI(1);
        }
        if (collision.transform.tag == "MoveSUp")
        {
            numMoveSpeedUp += 1;
            UpdateStats();
            Destroy(collision.gameObject);
            GetComponent<UIController>().UpdateUI(2);
        }
        if (collision.transform.tag == "BulletUp")
        {
            numProjUp += 1;
            UpdateStats();
            Destroy(collision.gameObject);
            GetComponent<UIController>().UpdateUI(3);    
        }
        if (collision.transform.tag == "CritChUp")
        {
            numCritChanceUp += 1;
            UpdateStats();
            Destroy(collision.gameObject);
            GetComponent<UIController>().UpdateUI(4);
        }
        if (collision.transform.tag == "CritDUp")
        {
            numCritDamUp += 1;
            UpdateStats();
            Destroy(collision.gameObject);
            GetComponent<UIController>().UpdateUI(5);
        }
        if (collision.transform.tag == "RangeUp")
        {
            numRangeUp += 1;
            UpdateStats();
            Destroy(collision.gameObject);
            GetComponent<UIController>().UpdateUI(6);
        }

        //Misc
        if (collision.transform.tag == "BossKey")
        {
            bossKeys += 1;
            Destroy(collision.gameObject);
        }
        if (collision.transform.tag == "GenericKey")
        {
            genericKeys += 1;
            Destroy(collision.gameObject);
        }      
    }

    public void UpdateStats()
    {
        currentWeapon.GetComponent<Weapon>().damage = currentWeapon.GetComponent<Weapon>().baseDamage * (1 + numAttackDamageUp * 0.10f);
        currentWeapon.GetComponent<Weapon>().fireRate = currentWeapon.GetComponent<Weapon>().baseFireRate * (1 + numAttackSpeedUp * 0.10f);
        currentWeapon.GetComponent<Weapon>().projectiles = numProjUp + currentWeapon.GetComponent<Weapon>().baseProjectiles;
        currentWeapon.GetComponent<Weapon>().critChance = currentWeapon.GetComponent<Weapon>().baseCritChance * (1 + numCritChanceUp * 0.10f);
        currentWeapon.GetComponent<Weapon>().critDamage = currentWeapon.GetComponent<Weapon>().baseCritDamage * (1 + numCritDamUp * 0.10f);
        currentWeapon.GetComponent<Weapon>().range = currentWeapon.GetComponent<Weapon>().baseRange * (1 + numRangeUp * 0.10f);
        GetComponent<Movement>().speed = GetComponent<Movement>().baseSpeed * (1 + numMoveSpeedUp * 0.075f);

    }

    IEnumerator flash()
    {
        yield return new WaitForSeconds(0.25f);
        immune = false;
        flashing = false;
        GetComponent<SpriteRenderer>().color = myColor;
    }
    IEnumerator StartAgain()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
    IEnumerator pickUpCoolDown()
    {
        yield return new WaitForSeconds(.5f);
        canPickUp = true;
    }

}
