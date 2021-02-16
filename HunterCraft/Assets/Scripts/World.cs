using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public int seed;

    public Transform player;
    public Vector3 spawnPosition;

    public Material material;
    public BlockType[] blockType;

    public int activeChunkCount;
    public int generatedChunkCount;
    public int inactiveChunkCount;
    public int inactiveChunkCount2;
    public int inactiveChunkCount3;

    Chunk[,] chunks = new Chunk[VoxelData.worldSizeInChunks, VoxelData.worldSizeInChunks];

    List<ChunkCoord> activeChunks = new List<ChunkCoord>();
    ChunkCoord playerChunkCoord;
    ChunkCoord playerLastChunkCoord;

    private void Start() {

        Random.InitState(seed);

        spawnPosition = new Vector3((VoxelData.worldSizeInChunks * VoxelData.chunkWidth) / 2f, VoxelData.chunkHeight + 2f, (VoxelData.worldSizeInChunks * VoxelData.chunkWidth) / 2f);

        GenerateWorld();
        playerLastChunkCoord = GetChunkCoordFromVector3(player.position);

    }

    private void Update() {

        playerChunkCoord = GetChunkCoordFromVector3(player.position);
        
        if(!playerChunkCoord.Equals(playerLastChunkCoord)){
            CheckViewDistance();
        }
        activeChunkCount = activeChunks.Count;

    }

    void GenerateWorld(){
        for(int x = (VoxelData.worldSizeInChunks / 2) - VoxelData.viewDistanceInChunks; x <(VoxelData.worldSizeInChunks / 2) + VoxelData.viewDistanceInChunks; x++){
            for(int z = (VoxelData.worldSizeInChunks / 2) - VoxelData.viewDistanceInChunks; z < (VoxelData.worldSizeInChunks / 2) + VoxelData.viewDistanceInChunks; z++){

                CreateNewChunk(x,z);

            }
        }

        player.position = spawnPosition;
    }

    ChunkCoord GetChunkCoordFromVector3 (Vector3 pos){

        int x = Mathf.FloorToInt(pos.x / VoxelData.chunkWidth);
        int z = Mathf.FloorToInt(pos.z / VoxelData.chunkWidth);
        return new ChunkCoord(x, z);

    }

    void CheckViewDistance(){

        

        ChunkCoord coord = GetChunkCoordFromVector3(player.position);

        List<ChunkCoord> previouslyActiveChunks = new List<ChunkCoord>(activeChunks);
        inactiveChunkCount3 = previouslyActiveChunks.Count;

        for(int x = coord.x - VoxelData.viewDistanceInChunks; x < coord.x - VoxelData.viewDistanceInChunks; x++){
            for(int z = coord.z - VoxelData.viewDistanceInChunks; z < coord.z - VoxelData.viewDistanceInChunks; z++){

                if (IsChunkInWorld(new ChunkCoord(x,z))){

                    if(chunks[x, z] == null){

                        CreateNewChunk(x, z);

                    }
                    else if (!chunks[x,z].isActive)
                    {
                        chunks[x,z].isActive = true;
                        activeChunks.Add(new ChunkCoord(x,z));
                    }

                }
                
                inactiveChunkCount = previouslyActiveChunks.Count;
                for (int i = 0; i < previouslyActiveChunks.Count; i++){
                    
                    if (previouslyActiveChunks[i].x == x && previouslyActiveChunks[i].z == z){

                        previouslyActiveChunks.RemoveAt(i);

                    }

                }
                inactiveChunkCount2 = previouslyActiveChunks.Count;
            }
        }
        
        Deactivate(previouslyActiveChunks);

    }

    public void Deactivate(List<ChunkCoord> PAC){
        
        foreach (ChunkCoord c in PAC)
        {
            chunks[c.x, c.z].isActive = false;
        }

    }

    public byte GetVoxel(Vector3 pos){
        
        if (!IsVoxelInWorld(pos)){
            return 0;
        }
        if(pos.y < 1){
            return 1;
        }else if (pos.y == VoxelData.chunkHeight-1)
        {
            float tempNoise = Noise.Get2DPerlin(new Vector2(pos.x, pos.z), 0f, .1f);
            if (tempNoise < 0.5f){
                return 3;
            }else
            {
                return 4;
            }
        }else
        {
            return 2;
        }
    }

    void CreateNewChunk(int x, int z){

        chunks[x,z] = new Chunk(new ChunkCoord(x,z), this);
        activeChunks.Add(new ChunkCoord(x,z));
        generatedChunkCount += 1;

    }

    bool IsChunkInWorld (ChunkCoord coord) {

        if(coord.x > 0 && coord.x < VoxelData.worldSizeInChunks - 1 && coord.z > 0 && coord.z < VoxelData.worldSizeInChunks-1){
            return true;
        }
        else
        {
            return false;
        }

    }

    bool IsVoxelInWorld(Vector3 pos){

        if (pos.x >= 0 && pos.x < VoxelData.WorldSizeInVoxels && pos.y >= 0 && pos.y < VoxelData.chunkHeight && pos.z >= 0 && pos.z < VoxelData.WorldSizeInVoxels){
            return true;
        }
        else
        {
            return false;
        }

    }

}

[System.Serializable]
public class BlockType {

    public string name;
    public bool isSolid;

    public int backFaceTexture;
    public int frontFaceTexture;
    public int topFaceTexture;
    public int bottomFaceTexture;
    public int leftFaceTexture;
    public int rightFaceTexture;
    public int GetTextureID(int faceIndex){

        switch (faceIndex){

            case 0:
                return backFaceTexture;
            case 1:
                return frontFaceTexture;
            case 2:
                return topFaceTexture;
            case 3:
                return bottomFaceTexture;
            case 4:
                return leftFaceTexture;
            case 5:
                return rightFaceTexture;
            default:
                Debug.Log("You done messed up");
                return 0;
        }

    }
}