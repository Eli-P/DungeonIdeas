using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePos : MonoBehaviour
{
    public Vector3 rest;
    public Vector3 offset;
    public int number;
    private GameObject attack;
    private Vector3 currentPos;
    private bool activated;
    // Start is called before the first frame update
    void Start()
    {
        attack = GameObject.Find("AttackController");
        rest = new Vector3(0, -4, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(activated){
            gameObject.transform.localPosition = gameObject.transform.parent.localPosition;
        }else{
            gameObject.transform.position = rest;
        }
    }
    public void Active(int num){
        if(num == number){
            activated = true;
        }else{
            activated = false;
        }
    }
}
