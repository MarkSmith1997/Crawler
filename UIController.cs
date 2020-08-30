using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    /*public RectTransform healthPanel;
    public GameObject healthUnit;
    public List<GameObject> healthUnits;

    public RectTransform attdPanel;
    public GameObject attdUnit;
    public List<GameObject> attdUnits;

    public RectTransform bowPanel;
    public GameObject bowUnit;
    public List<GameObject> bowUnits;

    public RectTransform bootPanel;
    public GameObject bootUnit;
    public List<GameObject> bootUnits;*/

    public Text healthNumber;
    public Text bossKeyNumber;
    public Text genericKeyNumber;
    public Text muneeNumber;

    public int powerUps;
    public RectTransform powerPanel;
    public GameObject[] powerUnit;
    public GameObject[] powerUnits;
    public bool[] canSpawn;
    public GameObject damageNumber;
    public GameObject attackSpeedNumber;
    public GameObject moveSpeedNumber;
    public GameObject projectileNumber;
    public GameObject critChanceNumber;
    public GameObject critDamageNumber;
    public GameObject rangeNumber;

    public Text kills;
    public int highscoreInt;
    public Text highScore;

    public Text weaponStats;

    public RectTransform noWeapon;
    public bool noWeaponFading;
    public GameObject noWeaponSoundEffect;

    public Text weaponChecker;
    public RectTransform weaponCheckerPanel;
    public RectTransform reloadBar;

    public int bossesBeaten;
    public int score;
    public Text deathText;

    public GameObject inGameMenu;

    public bool atAltar;
    public GameObject relixMenu;
    public GameObject[] relix;
    public bool[] spawnable;
    public int numRelix;
    public RectTransform relixPanel;


    private void Start()
    {
        atAltar = false;
        weaponCheckerPanel.gameObject.SetActive(false);
        for (int i = 0; i < canSpawn.Length; i++)
        {
            canSpawn[i] = true;
        }
        for (int i = 0; i < spawnable.Length; i++)
        {
            spawnable[i] = true;
        }
        if(PlayerPrefs.GetFloat("difficulty") < 1f)
        {
            highScore.text = "HIGHSCORE: " + PlayerPrefs.GetInt("RelaxedHighscore").ToString() + " R";
            highscoreInt = PlayerPrefs.GetInt("RelaxedHighscore");
        }
        if (PlayerPrefs.GetFloat("difficulty") == 1f)
        {
            highScore.text = "HIGHSCORE: " + PlayerPrefs.GetInt("DifficultHighscore").ToString() + " D";
            highscoreInt = PlayerPrefs.GetInt("DifficultHighscore");
        }
        if (PlayerPrefs.GetFloat("difficulty") > 1f)
        {
            highScore.text = "HIGHSCORE: " + PlayerPrefs.GetInt("DifficulterHighscore").ToString() + " DER";
            highscoreInt = PlayerPrefs.GetInt("DifficulterHighscore");
        }


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inGameMenu.SetActive(!inGameMenu.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.E) && atAltar)
        {
            relixMenu.SetActive(!relixMenu.activeSelf);
        }

        /*healthPanel.sizeDelta = new Vector2(GetComponent<Combat>().health * 25 + 25, 50);
        if (healthPanel.transform.childCount < GetComponent<Combat>().health)
        {
            GameObject unit = Instantiate(healthUnit, healthPanel.transform);
            healthUnits.Add(unit);
        }
        if (healthPanel.transform.childCount > GetComponent<Combat>().health)
        {
            GameObject unit = healthUnits[healthUnits.Count - 1];
            Destroy(unit);
            healthUnits.RemoveAt(healthUnits.Count - 1);
        }*/

        healthNumber.text = "x" + GetComponent<Combat>().health.ToString();
        bossKeyNumber.text = "x" + GetComponent<Combat>().bossKeys.ToString();
        genericKeyNumber.text = "x" + GetComponent<Combat>().genericKeys.ToString();
        muneeNumber.text = GetComponent<Combat>().munees.ToString();

        if (score * PlayerPrefs.GetFloat("difficulty") * 10 > highscoreInt)
        {
            if (PlayerPrefs.GetFloat("difficulty") < 1f)
            {
                highScore.text = "HIGHSCORE: " + PlayerPrefs.GetInt("RelaxedHighscore").ToString() + " R";
                PlayerPrefs.SetInt("RelaxedHighscore", (int)(score * PlayerPrefs.GetFloat("difficulty") * 10));
            }
            if (PlayerPrefs.GetFloat("difficulty") == 1f)
            {
                highScore.text = "HIGHSCORE: " + PlayerPrefs.GetInt("DifficultHighscore").ToString() + " D";
                PlayerPrefs.SetInt("DifficultHighscore", (int)(score * PlayerPrefs.GetFloat("difficulty") * 10));
            }
            if (PlayerPrefs.GetFloat("difficulty") > 1f)
            {
                highScore.text = "HIGHSCORE: " + PlayerPrefs.GetInt("DifficulterHighscore").ToString() + " DER";
                PlayerPrefs.SetInt("DifficulterHighscore", (int)(score * PlayerPrefs.GetFloat("difficulty") * 10));
            }
        }

        /*attdPanel.sizeDelta = new Vector2(50, GetComponent<Combat>().numAttackDamageUp * 25 + 25);
        if (attdPanel.transform.childCount < GetComponent<Combat>().numAttackDamageUp)
        {
            GameObject unit = Instantiate(attdUnit, attdPanel.transform);
            attdUnits.Add(unit);
        }
        if (attdPanel.transform.childCount > GetComponent<Combat>().numAttackDamageUp)
        {
            if (attdUnits.Count != 0)
            {
                GameObject unit = attdUnits[attdUnits.Count - 1];
                Destroy(unit);
                attdUnits.RemoveAt(attdUnits.Count - 1);
            }
        }

        bowPanel.sizeDelta = new Vector2(50, GetComponent<Combat>().numAttackSpeedUp * 25 + 25);
        if (bowPanel.transform.childCount < GetComponent<Combat>().numAttackSpeedUp)
        {
            GameObject unit = Instantiate(bowUnit, bowPanel.transform);
            bowUnits.Add(unit);
        }
        if (bowPanel.transform.childCount > GetComponent<Combat>().numAttackSpeedUp)
        {
            if (bowUnits.Count != 0)
            {
                GameObject unit = bowUnits[bowUnits.Count - 1];
                Destroy(unit);
                bowUnits.RemoveAt(bowUnits.Count - 1);
            }
        }

        bootPanel.sizeDelta = new Vector2(50, GetComponent<Combat>().numMoveSpeedUp * 25 + 25);
        if (bootPanel.transform.childCount < GetComponent<Combat>().numMoveSpeedUp)
        {
            GameObject unit = Instantiate(bootUnit, bootPanel.transform);
            bootUnits.Add(unit);
        }
        if (bootPanel.transform.childCount > GetComponent<Combat>().numMoveSpeedUp)
        {
            if (bootUnits.Count != 0)
            {
                GameObject unit = bootUnits[bootUnits.Count - 1];
                Destroy(unit);
                bootUnits.RemoveAt(bootUnits.Count - 1);
            }
        }*/

        powerPanel.sizeDelta = new Vector2(50, powerUps * 35 + 25);
        relixPanel.sizeDelta = new Vector2(50, numRelix * 35 + 25);
        if (Mathf.Abs(Input.GetAxis("AimX")) == 1 || Mathf.Abs(Input.GetAxis("AimY")) == 1)
        {
            reloadBar.transform.position = new Vector3(Screen.width / 2, Screen.height / 2 - 30) - (Camera.main.transform.position - transform.position) * 23f;
        }
        else
        {
            reloadBar.transform.position = Input.mousePosition - new Vector3(0, 30);
        }
        if (GetComponent<Combat>().currentWeapon != null)
        {
            reloadBar.sizeDelta = new Vector2(GetComponent<Combat>().currentWeapon.GetComponent<Weapon>().counter * 5f / (1 / GetComponent<Combat>().currentWeapon.GetComponent<Weapon>().fireRate), reloadBar.sizeDelta.y);
        }

        kills.text = (score * 10 * PlayerPrefs.GetFloat("difficulty")).ToString();


        if (GetComponent<Combat>().currentWeapon != null)
        {
            weaponStats.text =
            "Damage: " + GetComponent<Combat>().currentWeapon.GetComponent<Weapon>().damage.ToString(".0") +
            "\n Crit Chance: " + GetComponent<Combat>().currentWeapon.GetComponent<Weapon>().critChance.ToString(".0") +
            "\n Crit Damage: " + GetComponent<Combat>().currentWeapon.GetComponent<Weapon>().critDamage.ToString(".0") +
            "\n Fire Rate: " + GetComponent<Combat>().currentWeapon.GetComponent<Weapon>().fireRate.ToString(".0") +
            "\n AVG DPS: " + (GetComponent<Combat>().currentWeapon.GetComponent<Weapon>().damage * (1 + GetComponent<Combat>().currentWeapon.GetComponent<Weapon>().critChance * GetComponent<Combat>().currentWeapon.GetComponent<Weapon>().critDamage) * GetComponent<Combat>().currentWeapon.GetComponent<Weapon>().fireRate).ToString(".0");
        }

        weaponCheckerPanel.transform.position = Input.mousePosition;
        noWeapon.transform.position = Input.mousePosition;

        RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
        if (hit2D)
        {
            if (hit2D.transform.GetComponent<Weapon>() != null)
            {
                weaponCheckerPanel.gameObject.SetActive(true);
                weaponChecker.text =
                "Damage: " + hit2D.transform.GetComponent<Weapon>().damage.ToString(".0") +
                "\n Crit Chance: " + hit2D.transform.GetComponent<Weapon>().critChance.ToString(".0") +
                "\n Crit Damage: " + hit2D.transform.GetComponent<Weapon>().critDamage.ToString(".0") +
                "\n Fire Rate: " + hit2D.transform.GetComponent<Weapon>().fireRate.ToString(".0") +
                "\n AVG DPS: " + (hit2D.transform.GetComponent<Weapon>().damage * (1 + hit2D.transform.GetComponent<Weapon>().critChance * hit2D.transform.GetComponent<Weapon>().critDamage) * hit2D.transform.GetComponent<Weapon>().fireRate).ToString(".0");
            }
        }
        else
        {
            weaponCheckerPanel.gameObject.SetActive(false);
        }

        if (noWeaponFading)
        {
            Color color = noWeapon.GetComponent<RawImage>().color;
            color.a -= Time.deltaTime;
            noWeapon.GetComponent<RawImage>().color = color;
            Cursor.visible = false;
        }
        if(noWeapon.GetComponent<RawImage>().color.a < 0)
        {
            noWeaponFading = false;
            Cursor.visible = true;
        }

    }

    //PowerUPUI
    public void UpdateUI(int powerUp)
    {
        if (canSpawn[powerUp])
        {
            GameObject unit = Instantiate(powerUnit[powerUp], powerPanel.transform);
            powerUnits[powerUp] = unit;
            powerUps += 1;
            canSpawn[powerUp] = false;

        }
        if(powerUp == 0)
        {
            damageNumber = powerUnits[powerUp].transform.GetChild(0).gameObject;
            damageNumber.GetComponent<Text>().text = "x" + GetComponent<Combat>().numAttackDamageUp.ToString();
            if(GetComponent<Combat>().numAttackDamageUp == 0)
            {
                Destroy(powerUnits[powerUp]);
                powerUnits[powerUp] = powerUnit[powerUp];
                canSpawn[powerUp] = true;
                powerUps -= 1;
            }
        }
        if (powerUp == 1)
        {
            attackSpeedNumber = powerUnits[powerUp].transform.GetChild(0).gameObject;
            attackSpeedNumber.GetComponent<Text>().text = "x" + GetComponent<Combat>().numAttackSpeedUp.ToString();
            if (GetComponent<Combat>().numAttackSpeedUp == 0)
            {
                Destroy(powerUnits[powerUp]);
                powerUnits[powerUp] = powerUnit[powerUp];
                canSpawn[powerUp] = true;
                powerUps -= 1;
            }
        }
        if (powerUp == 2)
        {
            moveSpeedNumber = powerUnits[powerUp].transform.GetChild(0).gameObject;
            moveSpeedNumber.GetComponent<Text>().text = "x" + GetComponent<Combat>().numMoveSpeedUp.ToString();
            if (GetComponent<Combat>().numMoveSpeedUp == 0)
            {
                Destroy(powerUnits[powerUp]);
                powerUnits[powerUp] = powerUnit[powerUp];
                canSpawn[powerUp] = true;
                powerUps -= 1;
            }
        }
        if (powerUp == 3)
        {
            projectileNumber = powerUnits[powerUp].transform.GetChild(0).gameObject;
            projectileNumber.GetComponent<Text>().text = "x" + GetComponent<Combat>().numProjUp.ToString();
            if (GetComponent<Combat>().numProjUp == 0)
            {
                Destroy(powerUnits[powerUp]);
                powerUnits[powerUp] = powerUnit[powerUp];
                canSpawn[powerUp] = true;
                powerUps -= 1;
            }
        }
        if (powerUp == 4)
        {
            critChanceNumber = powerUnits[powerUp].transform.GetChild(0).gameObject;
            critChanceNumber.GetComponent<Text>().text = "x" + GetComponent<Combat>().numCritChanceUp.ToString();
            if (GetComponent<Combat>().numCritChanceUp == 0)
            {
                Destroy(powerUnits[powerUp]);
                powerUnits[powerUp] = powerUnit[powerUp];
                canSpawn[powerUp] = true;
                powerUps -= 1;
            }
        }
        if (powerUp == 5)
        {
            critDamageNumber = powerUnits[powerUp].transform.GetChild(0).gameObject;
            critDamageNumber.GetComponent<Text>().text = "x" + GetComponent<Combat>().numCritDamUp.ToString();
            if (GetComponent<Combat>().numCritDamUp == 0)
            {
                Destroy(powerUnits[powerUp]);
                powerUnits[powerUp] = powerUnit[powerUp];
                canSpawn[powerUp] = true;
                powerUps -= 1;
            }
        }
        if (powerUp == 6)
        {
            rangeNumber = powerUnits[powerUp].transform.GetChild(0).gameObject;
            rangeNumber.GetComponent<Text>().text = "x" + GetComponent<Combat>().numRangeUp.ToString();
            if (GetComponent<Combat>().numRangeUp == 0)
            {
                Destroy(powerUnits[powerUp]);
                powerUnits[powerUp] = powerUnit[powerUp];
                canSpawn[powerUp] = true;
                powerUps -= 1;
            }
        }
        for (int i = 0; i < GetComponent<Combat>().relix.Length; i++)
        {
            if (GetComponent<Combat>().relix[i] && spawnable[i])
            {
                numRelix += 1;
                Instantiate(relix[i], relixPanel.transform);
                spawnable[i] = false;
            }
        }
    }

    public void NoWeaponFade()
    {
        GameObject sound = Instantiate(noWeaponSoundEffect);
        sound.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("soundEffectsVolume");
        noWeapon.GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
        noWeaponFading = true;
    }

    public void SetDeathText()
    {
        deathText.text = "YOU ARE DEAD." + "\n" + "\n" + "YOUR SCORE: " + (score * 10 * PlayerPrefs.GetFloat("difficulty")).ToString();
    }
}

