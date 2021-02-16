using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigilPoint : MonoBehaviour
{
    private MeshRenderer mesh;
    public int childNum;
    // Start is called before the first frame update
    void Start()
    {
        mesh = transform.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Hide(int i){
        mesh.enabled = false;
        childNum = i;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.name == "Wand"){
            transform.parent.GetComponent<Sigil>().NextPoint(childNum);
        }
    }
}
