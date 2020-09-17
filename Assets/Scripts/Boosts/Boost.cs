using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{

    public usableBoost usable_Boost;
    public unusableBoost unusable_Boost;
    public bool usable = false;

    void Start()
    {
        StartCoroutine("waitFor10SecThenDestroy");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(usable == false)
            {
                unusable_Boost.Use(GameObject.FindWithTag("Player").transform);
            }
            else
            {
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().usable_boost = usable_Boost;
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().ActivateUsableBoostActionBarBtn(usable_Boost.icon);
            }
            Destroy(gameObject);
        }

    }

    IEnumerator waitFor10SecThenDestroy()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }


}
