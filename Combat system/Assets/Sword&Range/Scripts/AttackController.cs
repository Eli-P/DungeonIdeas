using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private Vector3 inActive;
    private int active;
    private int updateCheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateCheck = active;
        if(Input.GetKeyUp(KeyCode.Alpha1)){
            active = 0;
        }
        if(Input.GetKeyUp(KeyCode.Alpha2)){
            active = 1;
        }
        if(updateCheck != active){
            for(int i = 0; i < transform.childCount; i++){
                transform.GetChild(i).SendMessage("Active", active);
            }
        }
    }
}
 