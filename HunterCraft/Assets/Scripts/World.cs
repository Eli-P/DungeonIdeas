using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public int seed;
    public BiomeAttributes biome;

    public Transform player;
    public Vector3 spawnPosition;

    public Material material;
    public BlockType[] blockType;


    Chunk[,] chunks = new Chunk[VoxelData.worldSizeInChunks, VoxelData.worldSizeInChunks];

    List<ChunkCoord> activeChunks = new List<ChunkCoord>();

    


    ChunkCoord playerChunkCoord;
    ChunkCoord playerLastChunkCoord;

    private void Start() {

        Random.InitState(seed);
        spawnPosition = new Vector3((VoxelData.worldSizeInChunks * VoxelData.chunkWidth) / 2f, VoxelData.chunkHeight - 50f, (VoxelData.worldSizeInChunks * VoxelData.chunkWidth) / 2f);

        GenerateWorld();
        playerLastChunkCoord = GetChunkCoordFromVector3(player.position);

    }

    private void Update() {

        playerChunkCoord = GetChunkCoordFromVector3(player.position);
        
        if(!playerChunkCoord.Equals(playerLastChunkCoord))
            CheckViewDistance();

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
        playerLastChunkCoord = playerChunkCoord;

        List<ChunkCoord> previouslyActiveChunks = new List<ChunkCoord>(activeChunks);

        for(int x = coord.x - VoxelData.viewDistanceInChunks; x < coord.x + VoxelData.viewDistanceInChunks; x++){
            for(int z = coord.z - VoxelData.viewDistanceInChunks; z < coord.z + VoxelData.viewDistanceInChunks; z++){

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
                
                // inactiveChunkCount = previouslyActiveChunks.Count;
                for (int i = 0; i < previouslyActiveChunks.Count; i++){
                    
                    if (previouslyActiveChunks[i].x == x && previouslyActiveChunks[i].z == z){

                        previouslyActiveChunks.RemoveAt(i);

                    }

                }
                // inactiveChunkCount2 = previouslyActiveChunks.Count;
            }
        }
        
        Deactivate(previouslyActiveChunks);

    }

    public bool CheckForVoxel (float _x, float _y, float _z){

        int xCheck = Mathf.FloorToInt(_x);
        int yCheck = Mathf.FloorToInt(_y);
        int zCheck = Mathf.FloorToInt(_z);

        int xChunk = xCheck / VoxelData.chunkWidth;
        int zChunk = zCheck / VoxelData.chunkWidth;

        xCheck -= (xChunk * VoxelData.chunkWidth);
        zCheck -= (zChunk * VoxelData.chunkWidth);

        return blockType[chunks[xChunk, zChunk].voxelMap[xCheck, yCheck, zCheck]].isSolid;

    }



    public void Deactivate(List<ChunkCoord> PAC){
        
        foreach (ChunkCoord c in PAC)
        {
            chunks[c.x, c.z].isActive = false;
        }

    }

    public byte GetVoxel(Vector3 pos){
        
        int yPos = Mathf.FloorToInt(pos.y);


        if (!IsVoxelInWorld(pos)){
            return 0;
        }

        if (yPos == 0)
            return 1;

        /* FIRST PASS */

        int terrainHieght = Mathf.FloorToInt(biome.terrainHeight * Noise.Get2DPerlin(new Vector3(pos.x, pos.z), 0, biome.scale)) + biome.solidGroundHeight;
        byte voxelValue = 0;

        if (yPos == terrainHieght)
            voxelValue = 3;
        else if (yPos < terrainHieght && yPos > terrainHieght - 4)
            voxelValue = 5;
        else if (yPos > terrainHieght)
            return 0;
        else
        {
            voxelValue = 2;
        }

        /* SECOND PASS */
        if (voxelValue == 2){

            foreach (Lode lode in biome.lodes)
            {
                
                if(yPos > lode.minHeight && yPos < lode.maxHeight)
                    if(Noise.Get3DPerlin(pos, lode.noiseOffset, lode.scale, lode.threshold))
                        voxelValue = lode.blockID;

            }

        }
        
        return voxelValue;
         

    }
    

    void CreateNewChunk(int x, int z){

        chunks[x,z] = new Chunk(new ChunkCoord(x,z), this);
        activeChunks.Add(new ChunkCoord(x,z));
        // generatedChunkCount += 1;

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