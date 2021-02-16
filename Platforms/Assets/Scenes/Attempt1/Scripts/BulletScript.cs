using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    private void FixedUpdate() {
        timer -= .1f;
        if (timer <= 0){
            End("Enemy");
        }
    }
    public void End(string tag){
        if (tag == "Player" || tag == "Enemy"){
            Destroy(gameObject);
        }
    }
}
