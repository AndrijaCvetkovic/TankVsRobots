using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBehavior : MonoBehaviour
{

    public Shell shellData;
    Rigidbody2D ridg;
    string whoShoot;
    public GameObject explosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        ridg = GetComponent<Rigidbody2D>();
        StartCoroutine("wait20secThenDestory");
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.up * Time.deltaTime * 4;
        ridg.velocity = transform.up * 5;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(whoShoot == "Player")
            if (collision.tag == "Enemy")
            {
                SoundManager.instance.play_shellHit();
                collision.GetComponent<Enemy>().dealDmg(shellData.dmg);
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = gameObject.transform.position;
                explosion.GetComponent<Animator>().enabled = true;
                Explode();
            }
        if(whoShoot == "Enemy")
            if(collision.tag == "Player")
            {
                SoundManager.instance.play_shellHit();
                //Debug.Log("HIT: " + collision.gameObject.name);
                collision.GetComponent<PlayerController>().DealDmg(shellData.dmg);
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = gameObject.transform.position;
                explosion.GetComponent<Animator>().enabled = true;
                Explode();
            }
    }

    private void Explode()
    {
        Destroy(gameObject);
    }

    IEnumerator wait20secThenDestory()
    {
        yield return new WaitForSeconds(6);
        Destroy(gameObject);
    }

    public void initShell(Shell shell_Data,Vector3 position,Quaternion rotationQuat,int dmgBonusFromPlayer,string whoShoots)
    {
        transform.position = position;
        transform.rotation = rotationQuat;

        shellData = shell_Data;
        GetComponent<SpriteRenderer>().sprite = shellData.sprite;
        whoShoot = whoShoots;
    }

}

