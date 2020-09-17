using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy : MonoBehaviour
{
    [Header("EnemyStats")]
    public int dmg;
    int startHealth;
    public int hp;
    public bool can_shooting = true;
    public int pointsReward;
    public int level;

    [Header("Shooting")]
    public int attackSpeed = 1;
    public int range;
    public bool in_range = false;
    bool shoot_enabled = true;
    public List<Transform> shooting_points;
    public GameObject sheelPrefab;
    public Shell shellData;


    Transform player;
    public float speed = 2;
    private Rigidbody2D rb;
    private Vector2 movement;
    bool insideScene = false;
    public bool onScreen = false;
    public float distance_tmp;

    public GameObject explosionPrefab;
    public List<int> amountsOfAmmoReward;
    public string typeOfAmmoReward = string.Empty;
    public int amountOfArmorReward = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        if (Camera.main.GetComponent<GameplayController>().gameOver == false)
        {
            player = GameObject.FindWithTag("Player").transform;
            if (can_shooting)
                StartCoroutine("shootCourutine");
            startHealth = hp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            rb.rotation = angle;
            direction.Normalize();
            movement = direction;

            distance_tmp = Vector2.Distance(transform.position, player.position);

            if (distance_tmp <= range + 0.5f)
            {
                if (onScreen)
                {
                    in_range = true;
                    if (shoot_enabled)
                        shoot();
                }
                else
                {
                    in_range = false;
                    //Debug.Log("NE RADI PUCANJE ZBOG onScreen");
                }
            }
            else
            {
                in_range = false;
                distance_tmp = Vector2.Distance(transform.position, player.position);
                moveCharacter(movement);
                //Debug.Log("NE RADI PUCANJE ZBOG DISTANCE");
            }
        }
        /*else
        {
            StopCoroutine("shootCourutine");
        }*/
    }

    void insideScreen()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.transform.position.z);

        Vector3 screenHeight = new Vector3(Screen.width / 2, Screen.height, Camera.main.transform.position.z);

        Vector3 screenWidth = new Vector3(Screen.width, Screen.height / 2, Camera.main.transform.position.z);

        Vector3 goscreen = Camera.main.WorldToScreenPoint(transform.position);

        float distX = Vector3.Distance(new Vector3(Screen.width / 2, 0f, 0f), new Vector3(goscreen.x, 0f, 0f));

        float distY = Vector3.Distance(new Vector3(0f, Screen.height / 2, 0f), new Vector3(0f, goscreen.y, 0f));

        if (distX > Screen.width / 2 - GetComponent<SpriteRenderer>().bounds.size.x / 2 * 100 || distY > Screen.height / 2 - GetComponent<SpriteRenderer>().bounds.size.y / 2 * 100)
        {

            onScreen = false;
        }
        else
        {

            onScreen = true;
        }

    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            if (can_shooting)
                if (!in_range)
                    moveCharacter(movement);
                else if (!can_shooting)
                    moveCharacter(movement);
        }
    }

    void moveCharacter(Vector2 direction)
    {
        if (player != null)
        {
            rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
            insideScreen();
        }
    }

    public void dealDmg(int dmg)
    {
        hp -= dmg;
        if (player != null)
            hp -= player.GetComponent<PlayerController>().dmgBonus;
        if (hp <= 0)
        {
            int amountNumber = amountsOfAmmoReward[Random.Range(0, amountsOfAmmoReward.Count - 1)];
            Camera.main.GetComponent<GameplayController>().player_contoller.RewardAmmoHealth(typeOfAmmoReward, amountNumber, amountOfArmorReward);
            destroyEnemy();
        }
    }

    private void destroyEnemy()
    {

        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = gameObject.transform.position;
        explosion.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
        explosion.GetComponent<Animator>().enabled = true;

        Camera.main.GetComponent<GameplayController>().score += pointsReward;
        SoundManager.instance.play_SoundExplosion();
        Destroy(gameObject);

    }

    IEnumerator shootCourutine()
    {
        yield return new WaitForSeconds(attackSpeed);
        shoot_enabled = true;

    }

    public void shoot()
    {

        shoot_enabled = false;
        SoundManager.instance.play_soundShoot();
        shellData.dmg += dmg;
        foreach (Transform t in shooting_points)
        {
            GameObject enemy_shell = Instantiate(sheelPrefab);
            enemy_shell.GetComponent<ShellBehavior>().initShell(shellData, t.position, t.rotation, 0, gameObject.tag);
        }
        StopAllCoroutines();
        StartCoroutine("shootCourutine");
    }

    public void explode(PlayerController pc)
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = gameObject.transform.position;
        explosion.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
        explosion.GetComponent<Animator>().enabled = true;

        pc.DealDmg(startHealth / 2);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("ENEMY HIT");
            SoundManager.instance.play_SoundExplosion();
            //collision.transform.GetComponent<PlayerController>().DealDmg(startHealth);
            explode(collision.transform.GetComponent<PlayerController>());
        }
    }
}
