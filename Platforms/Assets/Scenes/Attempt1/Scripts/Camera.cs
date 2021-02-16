using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{ 
    public float turnSpeed;
    private float v;
    private Quaternion quant;
    private float scrollTemp;
    private float scroll;
    private Transform gun;
    private GunScript gunScript;
    public int magazine;
    public int magazineMax;
    // Start is called before the first frame update
    void Start()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel");
        scrollTemp = scroll;
        gun = transform.GetChild(0);
        gunScript = gun.GetComponent<GunScript>();
        gunScript.Mag(magazine, magazineMax);   
    }

    // Update is called once per frame
    void Update()
    {
        v = Input.GetAxis("Mouse Y");
        transform.Rotate(Vector3.right * Time.deltaTime * turnSpeed * v);
        scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != scrollTemp){
            gunScript.Fire(gameObject.name);
            scrollTemp = scroll;
        }
        // quant = new Quaternion(Mathf.Clamp(transform.rotation.x, -90, 90), 0, 0, 1);
        // transform.rotation = quant;
        
        
    }
    
}
