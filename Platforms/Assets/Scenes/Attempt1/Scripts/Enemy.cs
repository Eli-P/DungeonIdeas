using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    public float range;
    private Vector3 aiming;
    public float health;
    private float dist;
    public string typ;
    private int magazine;
    private int magazineMax;
    private float rate;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        transform.GetChild(0).GetComponent<EnemyGun>().Mag(magazine, magazineMax, rate);
        
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(transform.position, player.transform.position);
        if(dist < range){
            aiming = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(aiming);
        }
        if(health <= 0){
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision other) {
        if (other.collider.tag == "Bullet"){
            other.collider.GetComponent<BulletScript>().End(gameObject.tag);
            health -= .1f;
        }
    }
    public float GetRange(){
        if(typ == "Uzi"){
            magazineMax = 32;
            rate = .1f;
            range = 20f;
        }else if(typ == "Sniper"){
            magazineMax = 2;
            rate = 10;
            range = 200f;
        }
        return range;
    }
    public float GetDist(){
        return dist;
    }
}
