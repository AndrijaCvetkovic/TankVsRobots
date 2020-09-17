using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boosts", menuName = "ScriptableObjects/Boosts", order = 4)]
public class Boosts : ScriptableObject
{
    public List<unusableBoost> unusableBoosts;
    public List<usableBoost> usableBoosts;
}

[System.Serializable]
public abstract class BoostTemplate
{
    public bool waitForActivation = false;
    public Sprite icon;
    public abstract void Use(Transform player);

}

[System.Serializable]
public class usableBoost : BoostTemplate
{
    public GameObject prefab;
    
    public override void Use(Transform player)
    { 
        GameObject g = GameObject.Instantiate(prefab, player.transform.parent);
        g.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);

    }
}

[System.Serializable]
public class unusableBoost : BoostTemplate
{

    public boostStats boostStat;

    public bool expireTime = false;

    public override void Use(Transform player)
    {
        if (boostStat == boostStats.shield)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().StartCoroutine("startShield");
        }
        else
        {
            if (expireTime == true)
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().startBoostPeriod(boostStat, icon);
            else
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().increasePlayerStat(boostStat, icon);
        }
    }
}

public enum boostStats
{
    hp = 1,
    dmg = 2,
    speed = 3,
    shield = 4
}