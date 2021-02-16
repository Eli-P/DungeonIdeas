using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    private bool isDamaging;
    private float totalDamage;
    public float maxDamage;
    public float damageRate;

    // Start is called before the first frame update
    void Start()
    {
        isDamaging = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(totalDamage < maxDamage)
        {
            if(isDamaging)
            {
                totalDamage += damageRate; 
            }
        }
    }
    public void Damaging()
    {
        isDamaging = !isDamaging;
    }
    public float DamageDone()
    {
        float temp = totalDamage;
        totalDamage = 0;
        return temp;
    }
}
