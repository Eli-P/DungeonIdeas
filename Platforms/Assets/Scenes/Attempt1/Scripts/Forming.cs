using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forming : MonoBehaviour
{
    public float hight;
    private float height;
    private bool form;
    private GameObject player;
    private Rigidbody playerRigid;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerRigid = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        form = Input.GetKey(KeyCode.LeftShift);
        if(!form){
            if(playerRigid.velocity.y <= 0){
                height = hight + player.transform.position.y;
            }
            pos = new Vector3(player.transform.position.x, height, player.transform.position.z);
        }
        transform.position = pos;
    }
    public void Ended(Vector3 oldPos){


    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == ""){

        }
    }
}
