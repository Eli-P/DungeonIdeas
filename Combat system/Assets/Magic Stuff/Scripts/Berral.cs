using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berral : MonoBehaviour
{
    private float ud;
    public float turnSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ud = Input.GetAxis("Vertical");
        transform.Rotate(Vector3.left * Time.deltaTime * turnSpeed * ud);
        
    }
}
