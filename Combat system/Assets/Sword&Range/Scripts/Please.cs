using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Please : MonoBehaviour
{
    public float damage;
    public float speed;
    private bool hasEffect;
    public int tickDamage;
    public float tickTime;
    public float range;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        if(tickDamage > 0 ){
            hasEffect = true;
        }
        timer = range;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void FixedUpdate() {
        timer -= 0.1f;
        if (timer <= 0){
            End();
        }
    }
    public float ImpactDamage(){
        return damage;
    }
    public void End(){
        Destroy(gameObject);
    }
    public bool HasEffect(){
        return hasEffect;
    }
    public int Ticking(){
        return tickDamage;
    }
    public float TickTock(){
        return tickTime;
    }
}
