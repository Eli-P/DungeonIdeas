using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{ 
    public Material[] materials;
    public GameObject zambie;
    private Transform player;
    public float spawnRate;
    public float counter;
    private Vector3 vector;
    private Quaternion quant;
    public float minDist;
    public float maxDist;
    private float dist;
    private float z;
    private float x;
    private bool takingTickDamage;
    public bool spawning;
    public bool spawnOne;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.LeftShift)){
            spawnOne = true;
        }
        if(spawning || spawnOne){
            counter += .1f;
        }
        if(counter >= spawnRate){
            counter = 0;
            spawnOne = false;
            dist = Random.Range(minDist, maxDist);
            z = Random.Range(-dist, dist);
            x = Mathf.Sqrt((dist * dist) - (z * z));
            float negitive = Random.Range(0, 2);
            if (negitive == 1){
                x *= -1;
            }
            vector = new Vector3(x + player.position.x, 3, z + player.position.z);
            Instantiate(zambie, vector, quant);
        }
        
    }
    public Material RanMat(){
        return materials[Random.Range(0, materials.Length)];
    }
    public float RanHealth(){
        //return 20f;
        return Random.Range(1f, 40f);
    }
    public float RanSpeed(){
        return Random.Range(0.1f, 12f);
    }
    public float RanSize(){
        return Random.Range(0.5f, 3);
        //return 1f;
    }
}
