using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zambi : MonoBehaviour
{
    private GameObject player;
    private Spawner spawner;
    public float health;
    public float speed;
    public float[] tickDamage;
    private float takeDamage;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        gameObject.GetComponent<Renderer>().material = spawner.RanMat();
        health = spawner.RanHealth();
        speed = spawner.RanSpeed();
        float scale = spawner.RanSize();
        transform.localScale = new Vector3(scale, scale, scale); 
        tickDamage = new float[2];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i = 0; i < tickDamage.Length; i++){
            if (tickDamage[i] > 0){
                tickDamage[i] -= 1;
                takeDamage = i;
                health -= takeDamage/10;
            }
        }
        if(health <= 0){
            Destroy(gameObject);
        }
        transform.LookAt(player.transform.position);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Sword"){
            other.GetComponent<SwordScript>().Damaging();
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag == "Sword"){
            other.GetComponent<SwordScript>().Damaging();
            health -= other.GetComponent<SwordScript>().DamageDone();
        }
    }
    private void OnCollisionEnter(Collision other) {
        if(other.collider.tag == "Range"){
            Please temperary = other.collider.GetComponent<Please>();
            health -= temperary.ImpactDamage();
            if (temperary.HasEffect()){
                if(tickDamage[temperary.Ticking()-1] < temperary.TickTock()){
                    tickDamage[temperary.Ticking()-1] = temperary.TickTock();
                }
            }
            temperary.End();
        }
    }
}
