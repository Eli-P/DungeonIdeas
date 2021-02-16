using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandMovement : MonoBehaviour
{
    public float speed;
    private float mX;
    private float mY;
    void Update()
    {
        mX = Input.GetAxis("Mouse X");
        mY = Input.GetAxis("Mouse Y");
        transform.Translate(Vector3.right * Time.deltaTime * speed * mX);
        transform.Translate(Vector3.up * Time.deltaTime * speed * mY);
    }
}
