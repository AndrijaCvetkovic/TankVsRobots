using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D ridg;

    public Transform shootingPoint;

    public GameObject shellPrefab;

    Vector2 movPos;
    Vector2 mousePos;

    [Header("Parts")]
    public TankParts partsData;
    public SpriteRenderer track1;
    public SpriteRenderer track2;
    public SpriteRenderer hull;
    public SpriteRenderer tower;
    private Tracks tracksPart;
    private Hull hullPart;
    private Tower towerPart;

    [Header("Ammo")]
    public Shells ammoInventory;
    public List<Image> actionBarBtns;
    public Image ammoInfoIcon;
    public Text ammoLeft;
    int selectedAmmoType = 0;

    [Header("InfoAndStats")]
    public int dmgBonus = 0;
    public int armor = 0;
    public float speed = 5f;
    public Image armorIcon;
    public Text amororText;

    [Header("Restrict moving ouside screen")]
    public Camera MainCamera;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    [Header("Boosts")]
    public Boost boost;

    public usableBoost usable_boost;
    public bool actionBoostToActivate = false;
    public GameObject actionSkillForBoostObject;

    public bool shieldActivated = false;
    public GameObject shieldEffect;

    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        InitParts();
        ridg = GetComponent<Rigidbody2D>();
        selectedAmmoType = 0;
        updateActionBarAndAmmoInfo(0);

        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        objectWidth = transform.GetComponent<BoxCollider2D>().bounds.extents.x;
        objectHeight = transform.GetComponent<BoxCollider2D>().bounds.extents.y;

        //test
        //armor += 50000;
    }

    // Update is called once per frame
    void Update()
    {
        amororText.text = armor.ToString();

        movPos.x = Input.GetAxis("Horizontal");
        movPos.y = Input.GetAxis("Vertical");

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        
        if(Input.GetKeyDown("1"))
        {
            selectedAmmoType = 0;
            updateActionBarAndAmmoInfo(selectedAmmoType);
        }
        if (Input.GetKeyDown("2"))
        {
            selectedAmmoType = 1;
            updateActionBarAndAmmoInfo(selectedAmmoType);
        }
        if (Input.GetKeyDown("3"))
        {
            selectedAmmoType = 2;
            updateActionBarAndAmmoInfo(selectedAmmoType);
        }
        if (Input.GetKeyDown("4"))
        {
            selectedAmmoType = 3;
            updateActionBarAndAmmoInfo(selectedAmmoType);
        }
        if(actionBoostToActivate == true && Input.GetKeyDown("5"))
        {
            //do something
            actionBoostToActivate = false;
            actionSkillForBoostObject.active = false;
            usable_boost.Use(transform);

        }

    }

    private void FixedUpdate()
    {
        ridg.MovePosition(ridg.position + movPos * speed * Time.fixedDeltaTime);

        Vector2 direction = mousePos - ridg.position;

        float angleToRotateChar = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        ridg.rotation = angleToRotateChar;

    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        transform.position = viewPos;
    }

    private void updateActionBarAndAmmoInfo(int i)
    {

        foreach(Image img in actionBarBtns)
        {
            img.color = Color.white;
        }

        if (ammoInventory.shells[i].ammoInInventory > 0 && i != 0)
        {
            ammoInfoIcon.sprite = ammoInventory.shells[i].sprite;
            ammoLeft.text = ammoInventory.shells[i].ammoInInventory.ToString();
            actionBarBtns[i].color = Color.green;
            selectedAmmoType = i;
        }
        else
        {
            ammoInfoIcon.sprite = ammoInventory.shells[0].sprite;
            ammoLeft.text = "x";
            actionBarBtns[0].color = Color.green;
            selectedAmmoType = 0;
        }

        
    }

    internal void RewardAmmoHealth(string type, int amount,int armorRew)
    {
        foreach(Shell s in ammoInventory.shells)
        {
            if (s.name == type)
                s.ammoInInventory += amount;
        }
        armor += armorRew;
    }

    private void Shoot()
    {
        GameObject shell = Instantiate(shellPrefab);

        if (ammoInventory.shells[selectedAmmoType].ammoInInventory > 0 || selectedAmmoType == 0)
        {
            SoundManager.instance.play_soundShoot();
            shell.GetComponent<ShellBehavior>().shellData = ammoInventory.shells[selectedAmmoType];
            if(ammoInventory.shells[selectedAmmoType].ammoInInventory != 0)
                ammoInventory.shells[selectedAmmoType].ammoInInventory -= 1;
            shell.GetComponent<ShellBehavior>().initShell(ammoInventory.shells[selectedAmmoType], shootingPoint.position, shootingPoint.rotation, dmgBonus, gameObject.tag);

            updateActionBarAndAmmoInfo(selectedAmmoType);
        }
        else
        {
            SoundManager.instance.play_soundShoot();
            shell.GetComponent<ShellBehavior>().initShell(ammoInventory.shells[0], shootingPoint.position, shootingPoint.rotation,dmgBonus, gameObject.tag);

            updateActionBarAndAmmoInfo(0);
        }

        shell.transform.position = shootingPoint.position;
        shell.transform.rotation = shootingPoint.rotation;

    }

    public void DealDmg(int dmg)
    {
        if (shieldActivated == false)
        {
            armor -= dmg;
            if (armor <= 0)
            {
                SoundManager.instance.play_SoundExplosion();

                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = gameObject.transform.position;
                Vector2 vect = transform.GetChild(3).transform.localScale;
                explosion.transform.localScale = new Vector3(vect.x, vect.y, 1);
                explosion.GetComponent<Animator>().enabled = true;

                setNewScore();
                int coins = PlayerPrefs.GetInt("Coins") + (Camera.main.GetComponent<GameplayController>().score / 10);
                PlayerPrefs.SetInt("Coins", coins);
                Camera.main.GetComponent<GameplayController>().GameOver();
                Destroy(gameObject);
                Debug.Log("LOST");
            }
        }
    }

    public void setNewScore()
    {
        int h_score = PlayerPrefs.GetInt("HighScore");
        int new_score = Camera.main.GetComponent<GameplayController>().score;
        if (h_score < new_score)
        {
            PlayerPrefs.SetInt("HighScore",new_score);
            Camera.main.GetComponent<GameplayController>().HighScoreShow();
        }
    }

    public void InitParts()
    {
        if(PlayerPrefs.HasKey("trackId") == false)
        {
            tracksPart = partsData.tracksParts[0];
            track1.sprite = tracksPart.sprite;
            track2.sprite = tracksPart.sprite;
            speed = tracksPart.speedBonus;
        }
        else
        {
            int partId = PlayerPrefs.GetInt("trackId");
            tracksPart = partsData.tracksParts[partId];
            track1.sprite = tracksPart.sprite;
            track2.sprite = tracksPart.sprite;
            speed = tracksPart.speedBonus;
        }

        if (PlayerPrefs.HasKey("hullId") == false)
        {
            hullPart = partsData.hullParts[0];
            hull.sprite = hullPart.sprite;
            armor = hullPart.armorBonus;
        }
        else
        {
            int partId = PlayerPrefs.GetInt("hullId");
            hullPart = partsData.hullParts[partId];
            hull.sprite = hullPart.sprite;
            armor = hullPart.armorBonus;
        }

        if (PlayerPrefs.HasKey("towerId") == false)
        {
            int partId = 0;
            towerPart = partsData.towerPars[partId];
            tower.sprite = towerPart.sprite;
            dmgBonus = towerPart.dmgBonus;
        }
        else
        {
            int partId = PlayerPrefs.GetInt("towerId");
            towerPart = partsData.towerPars[partId];
            tower.sprite = towerPart.sprite;
            dmgBonus = towerPart.dmgBonus;
        }


    }

    public void increasePlayerStat(boostStats statToBoost, Sprite iconBoost)
    {
        if(statToBoost == boostStats.hp)
        {
            armor += 10;
        }
    }

    public void startBoostPeriod(boostStats statToBoost, Sprite iconBoost)
    {
        StartCoroutine(startBoostPeriodCourutine(statToBoost,iconBoost));
    }

    IEnumerator startBoostPeriodCourutine(boostStats statToBoost,Sprite iconBoost)
    {
        int prevValueInt = 0;
        float prevValueFloat = 0;
        if(statToBoost == boostStats.dmg)
        {
            prevValueInt = dmgBonus;
            dmgBonus += 2;
        }
        else if(statToBoost == boostStats.speed)
        {
            prevValueFloat = speed;
            speed += 1f;
        }

        yield return new WaitForSeconds(10);

        if (statToBoost == boostStats.dmg)
        {
            dmgBonus = prevValueInt;
        }
        else if (statToBoost == boostStats.speed)
        {
            speed = prevValueFloat;
        }

    }

    public IEnumerator startShield()
    {
        shieldEffect.SetActive(true);
        shieldActivated = true;
        yield return new WaitForSeconds(6f);
        shieldActivated = false;
        shieldEffect.SetActive(false);
    }

    public void ActivateUsableBoostActionBarBtn(Sprite s)
    {

        actionBoostToActivate = true;
        actionSkillForBoostObject.transform.GetChild(2).GetComponent<Image>().sprite = s;
        actionSkillForBoostObject.active = true;
    }

}

