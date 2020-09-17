using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankParts", menuName = "ScriptableObjects/Parts", order = 2)]
public class TankParts : ScriptableObject
{
    public List<Tracks> tracksParts;

    public List<Hull> hullParts;

    public List<Tower> towerPars;
}

[System.Serializable]
public class Tracks
{

    public string name;

    public int cost;

    public int speedBonus;

    public Sprite sprite;
}

[System.Serializable]
public class Hull
{
    public string name;

    public int cost;

    public int armorBonus;

    public Sprite sprite;
}

[System.Serializable]
public class Tower
{
    public string name;

    public int cost;

    public int dmgBonus;

    public Sprite sprite;
}




