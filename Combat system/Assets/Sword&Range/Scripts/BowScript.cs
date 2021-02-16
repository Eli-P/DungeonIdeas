using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowScript : MonoBehaviour
{
    public float timer;
    public float timerMax;
    public GameObject arrow;
    // Start is called before the first frame update
    void Start()
    {
        timer = timerMax;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Space)){
            timer -= .1f;
        }else{
            if(timer > 0){
                timer = timerMax;
            }else{
                Instantiate(arrow, transform.position, transform.rotation);
                timer = timerMax;
            }
        }

    }
}
