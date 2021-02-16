using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    public GameObject[] PreFabs;
    public int sigilCount;
    public Transform[] sigils;
    public int activeSigil;
    // Start is called before the first frame update
    void Start()
    {
        sigils = new Transform[transform.childCount];
        sigilCount = sigils.Length;
        for (int i = 0; i < transform.childCount; i++){
            sigils[i] = transform.GetChild(i);
            sigils[i].GetComponent<Sigil>().SigilIntiger(i);
        }
    }
    public GameObject Pre(int i){
        return PreFabs[i];
    }
    public void Selected(int sigilNum){
        activeSigil = sigilNum;
        for(int i = 0; i < sigils.Length; i++){
            if (i != sigilNum){
                sigils[i].GetComponent<Sigil>().DeActivate();
            }
        }
    }
    public void Finished(){
        for (int i = 0; i < sigils.Length; i++){
            sigils[i].GetComponent<Sigil>().ReActivate();
        }
    }
}
