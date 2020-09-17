using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shells", menuName = "ScriptableObjects/Shells", order = 1)]
public class Shells : ScriptableObject
{

    public List<Shell> shells;

    public List<Shell> enemyShellsAndBullets;

    public Shell returnEnemyShell(string name)
    {
        Shell tmp = enemyShellsAndBullets[0];
       
        foreach (Shell s in enemyShellsAndBullets)
            if (s.name == name)
            {
                tmp = s;
            }
        return tmp;

    }

}
