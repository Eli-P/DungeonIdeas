using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    private GameObject player;
    private float dist;
    private Transform gun;
    private GunScript gunScript;
    public float fireRate;
    private float timer;
    public float range;
    // Start is called before the first frame update
    void Start()
    {
        gun = transform.GetChild(0);
        gunScript = gun.GetComponent<GunScript>();
        timer = fireRate;
        range = transform.parent.GetComponent<Enemy>().GetRange();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(dist <= range){
            if(timer<=0){
                gunScript.Fire(gameObject.name);
                timer = fireRate;
                }
            transform.LookAt(player.transform.position);
        }
    }
    private void FixedUpdate() {
        timer -= .1f;
        dist = transform.parent.GetComponent<Enemy>().GetDist();
    }
    public void Mag(int mag, int magMax, float fireRated){
        gunScript.Mag(mag, magMax);
        fireRate = fireRated;
    }
}
