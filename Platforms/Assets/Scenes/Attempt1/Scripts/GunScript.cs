using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    private Transform parent;
    private string parentName;
    public GameObject[] bullet;
    private Transform spawnPoint;
    private int magazine;
    private int magazineMax;
    private float timer;
    private int typ;
    // Start is called before the first frame update
    void Start()
    {
        timer = 10;
        parent = transform.parent;
        parentName = parent.name;
        spawnPoint = transform.GetChild(0);
        magazine = magazineMax;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (magazine == 0){
            timer -= .1f;
        }
        if (timer<=0){
            timer = 10;
            magazine = magazineMax;
        }
    }
    public void Fire(string name){
        if(parentName == name){
            if (magazine > 0){
                magazine -= 1;
                Instantiate(bullet[typ], spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
    public void Mag(int mag, int magMax){
        magazine = mag;
        magazineMax = magMax;
    }
    
}
