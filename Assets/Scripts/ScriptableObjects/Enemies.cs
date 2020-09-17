using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName ="Enemies",menuName = "ScriptableObjects/Enemies",order = 3)]
public class Enemies : ScriptableObject
{
    public int s;
    public List<GameObject> enemies;

    public int returnMaxLvlEnemy()
    {
        int max = 0;
        foreach (GameObject g in enemies)
            if (g.GetComponent<Enemy>().level > max)
                max = g.GetComponent<Enemy>().level;
        return max;
    }

}