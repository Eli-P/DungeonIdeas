using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sigil : MonoBehaviour
{
    private string tag;
    private GameObject center;
    public GameObject spawnObject;
    private GameObject barrel;
    private MeshRenderer mesh;
    public Transform[] children;
    public int progress;
    private bool active;
    private int sigilNum;
    // Start is called before the first frame update
    void Start()
    {
        progress = 0;
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++){
            children[i] = transform.GetChild(i);
            children[i].GetComponent<SigilPoint>().Hide(i+1);
        }
        mesh = gameObject.GetComponent<MeshRenderer>();
        center = GameObject.Find("Center");
        tag = gameObject.tag;
        if(tag == "lvl1"){
            if(gameObject.name == "SigilCube"){
                spawnObject = center.GetComponent<Center>().Pre(0);
            }else if(gameObject.name == "SigilCylinder"){
                spawnObject = center.GetComponent<Center>().Pre(1);
            }else if(gameObject.name == "SigilProjectial"){
                spawnObject = center.GetComponent<Center>().Pre(2);
                barrel = GameObject.Find("FirePos");
            }
        }else{
            mesh.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(progress == transform.childCount){
            progress = 0;
            Finished();
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Magic"){
            if (active){
                progress = 1;
                center.GetComponent<Center>().Selected(sigilNum);
            }
        }
    }
    public void NextPoint(int k){
        if (progress == k){
            if (active){
                progress = k+1;
            }
        }
    }
    private void Finished(){
        if(gameObject.name == "SigilProjectial"){
            barrel.GetComponent<Fireing>().SetWeapon(spawnObject);
            barrel.GetComponent<Fireing>().Fire();
        }

        center.GetComponent<Center>().Finished();
    }
    public void DeActivate(){
        active = false;
    }
    public void ReActivate(){
        active = true;
    }
    public void SigilIntiger(int k){
        sigilNum = k;
    }
}
