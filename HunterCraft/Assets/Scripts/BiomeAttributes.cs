using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeAttributes", menuName = "MinecraftTutorials/Biome Attributes")]
public class BiomeAttributes : ScriptableObject
{
    
    public string biomeName;

    public int solidGroundHeight;
    public int terrainHeight;
    public float scale;

    public Lode[] lodes;

}

[System.Serializable]
public class Lode {

    public string nodeName;
    public byte blockID;
    public int minHeight;
    public int maxHeight;
    public float scale;
    public float threshold;
    public float noiseOffset;

}