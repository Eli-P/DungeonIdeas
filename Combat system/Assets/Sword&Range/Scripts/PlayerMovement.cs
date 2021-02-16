using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private float lr;
    private float ud;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lr = Input.GetAxis("Horizontal");
        ud = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * Time.deltaTime * speed * ud);
        transform.Translate(Vector3.right * Time.deltaTime * speed * lr);
    }
}
