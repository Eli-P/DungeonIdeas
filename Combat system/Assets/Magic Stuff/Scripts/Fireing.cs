using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireing : MonoBehaviour
{
    private GameObject projectial;
    public void SetWeapon(GameObject newItem){
        projectial = newItem;
    }
    public void Fire(){
        Instantiate(projectial, transform.position, transform.rotation);
    }
}
