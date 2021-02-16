using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectial : MonoBehaviour
{
    public float speed;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 20;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void FixedUpdate() {
        timer -= .1f;
        if (timer <= 0){
            Destroy(gameObject);
        }
    }
}
