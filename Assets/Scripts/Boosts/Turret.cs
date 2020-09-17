using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject shellPreafab;
    public Shell shellData;
    public Transform shooting_point;

    public void Shoot()
    {
        GameObject enemy_shell = Instantiate(shellPreafab);
        enemy_shell.GetComponent<ShellBehavior>().initShell(shellData, shooting_point.position, shooting_point.rotation, 0, "Player");
    }

    public void Destroy()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

}
