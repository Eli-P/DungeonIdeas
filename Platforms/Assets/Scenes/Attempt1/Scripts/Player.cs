using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    private float lr;
    private float ud;
    private float h;
    public float jumpPower;
    private float jump;
    private Rigidbody rigid;
    private bool onGround;
    private GameObject respawn;
    public float health;
    // Start is called before the first frame update
    void Start()
    {
        rigid = transform.GetComponent<Rigidbody>();
        onGround = true;
        respawn = GameObject.Find("Respawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (health<= 0){
            transform.position = respawn.transform.position;
            health = 20;
        }
        lr = Input.GetAxis("Horizontal");
        ud = Input.GetAxis("Vertical");
        jump = Input.GetAxis("Jump");
        if (onGround && jump > 0){
            onGround = false;
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        transform.Translate(Vector3.right * Time.deltaTime * speed * lr);
        transform.Translate(Vector3.forward * Time.deltaTime * speed * ud);
        h = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * h);

    }
    private void OnCollisionEnter(Collision other) {
        if (other.collider.tag == "Ground"){
            onGround = true;
        }
        if(other.collider.tag == "Death"){
            transform.position = respawn.transform.position;
        }
        if (other.collider.tag == "Bullet"){
            other.collider.GetComponent<BulletScript>().End(gameObject.tag);
            health -= .1f;
        }
    }
}
