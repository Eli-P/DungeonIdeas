using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocalPoint : MonoBehaviour
{
    private Transform visual;
    public float max;
    private float posY;
    private Vector3 pos;
    private Transform formationCore;
    private float dist;
    // Start is called before the first frame update
    void Start()
    {
        visual = transform.GetChild(0);
        posY = Random.Range(-max, max);
        pos = new Vector3(transform.position.x, posY+ transform.position.y, transform.position.z);
        visual.position = pos;
        // dist = Vector3.Distance(formationCore.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (dist > 2){
            // formationCore.GetComponent<Forming>().Ended(transform);

        }
        // dist = Vector3.Distance(formationCore.position, transform.position);
    }
}
