using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public List<GameObject> enemiesInField = new List<GameObject>();
    public void Explode()
    {
        Debug.Log("Counter: " + enemiesInField.Count);
        //InstantieteExplosionParticle
        SoundManager.instance.play_SoundExplosion();
        for (int i = enemiesInField.Count - 1; i >= 0; i--)
            if (enemiesInField[i] != null)
                enemiesInField[i].GetComponent<Enemy>().dealDmg(10);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            enemiesInField.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemiesInField.Remove(collision.gameObject);
        }
    }

}
