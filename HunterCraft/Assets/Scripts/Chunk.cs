﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{  

    public ChunkCoord coord;    

    GameObject chunkObject;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    // MeshCollider meshCollider;
    
    int vertexIndex = 0;
    List<Vector3> vertices = new List<Vector3> ();
    List<Vector2> uvs = new List<Vector2> ();
    List<int> triangles = new List<int> ();

    public byte[,,] voxelMap = new byte[VoxelData.chunkWidth, VoxelData.chunkHeight, VoxelData.chunkWidth];

    World world;

    public Chunk(ChunkCoord _coord, World _world){

        world = _world;
        coord = _coord;

        chunkObject = new GameObject();
        meshFilter = chunkObject.AddComponent<MeshFilter>();
        meshRenderer = chunkObject.AddComponent<MeshRenderer>();
        // meshCollider = chunkObject.AddComponent<MeshCollider>();

        meshRenderer.material = world.material;
        chunkObject.transform.SetParent(world.transform);
        chunkObject.transform.position = new Vector3(coord.x * VoxelData.chunkWidth, 0f, coord.z * VoxelData.chunkWidth);
        chunkObject.name = "Chunk" + coord.x + "," + coord.z;
        
        PopulateVoxelMap();
        CreateMeshData();
        CreateMesh();

        // meshCollider.sharedMesh = meshFilter.mesh;

    }

    void PopulateVoxelMap(){

        for(int y = 0; y < VoxelData.chunkHeight; y++){
            for(int x = 0; x < VoxelData.chunkWidth; x++){
                for(int z = 0; z < VoxelData.chunkWidth; z++){ 
                    voxelMap[x,y,z] = world.GetVoxel(new Vector3(x,y,z) + position);
                }
            }
        }

    }
    void CreateMeshData(){
        
        for(int y = 0; y < VoxelData.chunkHeight; y++){
            for(int x = 0; x < VoxelData.chunkWidth; x++){
                for(int z = 0; z < VoxelData.chunkWidth; z++){ 

                    if (world.blockType[voxelMap[x,y,z]].isSolid)
                        AddVoxelDataToChunk(new Vector3(x, y, z));
                }
            }
        }
    }

    public bool isActive{

        get{return chunkObject.activeSelf;}
        set{chunkObject.SetActive(value);}

    }

    public Vector3 position {

        get{return chunkObject.transform.position;}

    }

    bool IsVoxelInChunk(int x, int y, int z){

        if (x < 0 || x > VoxelData.chunkWidth - 1 || y < 0 || y > VoxelData.chunkHeight - 1 || z < 0 || z > VoxelData.chunkWidth - 1){
            return false;
        }
        else
        {
            return true;
        }

    }

    bool CheckVoxel(Vector3 pos){

        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        if (!IsVoxelInChunk(x, y, z)){
            return world.blockType[world.GetVoxel(pos + position)].isSolid;
        }
        return world.blockType[voxelMap[x,y,z]].isSolid;
    }

    void AddVoxelDataToChunk(Vector3 pos){

        for(int p = 0; p < 6; p++){
            if(!CheckVoxel(pos+VoxelData.faceChecks[p])){
                for(int i = 0; i < 6; i++){

                    byte blockID = voxelMap[(int)pos.x, (int)pos.y, (int)pos.z];
                    vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 0]]);
                    vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 1]]);
                    vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 2]]);
                    vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 3]]);

                    AddTexture(world.blockType[blockID].GetTextureID(p));

                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex+1);
                    triangles.Add(vertexIndex+2);
                    triangles.Add(vertexIndex+2);
                    triangles.Add(vertexIndex+1);
                    triangles.Add(vertexIndex+3);
                    vertexIndex += 4;
                }
            }
        }
    }

    void CreateMesh(){
        
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    void AddTexture(int TextureID){
        float y = TextureID / VoxelData.TextureAtlasSizeInBlocks;
        float x = TextureID - (y * VoxelData.TextureAtlasSizeInBlocks);

        x *= VoxelData.NormalizedBlockTextureSize;
        y *= VoxelData.NormalizedBlockTextureSize;

        y = 1f - y - VoxelData.NormalizedBlockTextureSize;
        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + VoxelData.NormalizedBlockTextureSize));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize));
    }
}

public class ChunkCoord {
    
    public int x;
    public int z;

    public ChunkCoord(int _x, int _z){

        x = _x;
        z = _z;

    }

    public bool Equals (ChunkCoord other){
        if (other == null){
            return false;
        }else if (other.x == x && other.z == z)
        {
            return true;
        }else
        {
            return false;
        }
    }
}