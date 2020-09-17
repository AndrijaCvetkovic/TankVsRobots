using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnClick : MonoBehaviour
{
    public void clickAnimation()
    {
        GetComponent<Animator>().Play("BtnAnimation");
    }
}
