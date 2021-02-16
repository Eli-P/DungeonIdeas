using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World2 : MonoBehaviour
{
    public Transform player;
    public Vector3 spawn;

    public Material material;
    public BlockType2[] blocktypes;

    Chunk2[,] chunks = new Chunk2[VoxelData.worldSizeInChunks, VoxelData.worldSizeInChunks];
    List<ChunkCoord2> activeChunks = new List<ChunkCoord2>();
    ChunkCoord2 playerLastChunkCoord;

    private void Start() {

        GenerateWorld();
        playerLastChunkCoord = GetChunkCoordFromVector3(player.transform.position);

    }

    private void Update() {

        if (!GetChunkCoordFromVector3(player.transform.position).Equals(playerLastChunkCoord))
            CheckViewDistance();

    }

    ChunkCoord2 GetChunkCoordFromVector3 (Vector3 pos) {

        int x = Mathf.FloorToInt(pos.x / VoxelData.chunkWidth);
        int z = Mathf.FloorToInt(pos.z / VoxelData.chunkWidth);
        return new ChunkCoord2(x, z);

    }

    private void GenerateWorld () {

        for (int x = VoxelData.worldSizeInChunks / 2 - VoxelData.viewDistanceInChunks / 2; x < VoxelData.worldSizeInChunks / 2 + VoxelData.viewDistanceInChunks / 2; x++) {
            for (int z = VoxelData.worldSizeInChunks / 2 - VoxelData.viewDistanceInChunks / 2; z < VoxelData.worldSizeInChunks / 2 + VoxelData.viewDistanceInChunks / 2; z++) {

                CreateChunk(new ChunkCoord2(x, z));

            }
        }

        spawn = new Vector3(VoxelData.WorldSizeInVoxels / 2, VoxelData.chunkHeight + 2, VoxelData.WorldSizeInVoxels / 2);
        player.position = spawn;

    }

    private void CheckViewDistance () {

        int chunkX = Mathf.FloorToInt(player.position.x / VoxelData.chunkWidth);
        int chunkZ = Mathf.FloorToInt(player.position.z / VoxelData.chunkWidth);

        List<ChunkCoord2> previouslyActiveChunks = new List<ChunkCoord2>(activeChunks);

        for (int x = chunkX - VoxelData.viewDistanceInChunks / 2; x < chunkX + VoxelData.viewDistanceInChunks / 2; x++) {
            for (int z = chunkZ - VoxelData.viewDistanceInChunks / 2; z < chunkZ + VoxelData.viewDistanceInChunks / 2; z++) {

                // If the chunk is within the world bounds and it has not been created.
                if (IsChunkInWorld(x, z)) {

                    ChunkCoord2 thisChunk = new ChunkCoord2(x, z);

                    if (chunks[x, z] == null)
                        CreateChunk(thisChunk);
                    else if (!chunks[x, z].isActive) {
                        chunks[x, z].isActive = true;
                        activeChunks.Add(thisChunk);
                    }
                    // Check if this chunk was already in the active chunks list.
                    for (int i = 0; i < previouslyActiveChunks.Count; i++) {

                        //if (previouslyActiveChunks[i].Equals(new ChunkCoord2(x, z)))
                        if (previouslyActiveChunks[i].x == x && previouslyActiveChunks[i].z == z)
                            previouslyActiveChunks.RemoveAt(i);

                    }

                }
            }
        }

        foreach (ChunkCoord2 coord in previouslyActiveChunks)
            chunks[coord.x, coord.z].isActive = false;

    }

    bool IsChunkInWorld(int x, int z) {

        if (x > 0 && x < VoxelData.worldSizeInChunks - 1 && z > 0 && z < VoxelData.worldSizeInChunks - 1)
            return true;
        else
            return false;

    }

    private void CreateChunk (ChunkCoord2 coord) {

        chunks[coord.x, coord.z] = new Chunk2(new ChunkCoord2(coord.x, coord.z), this);
        activeChunks.Add(new ChunkCoord2(coord.x, coord.z));


    }

    public byte GetVoxel (Vector3 pos) {

        if (pos.x < 0 || pos.x > VoxelData.WorldSizeInVoxels - 1 || pos.y < 0 || pos.y > VoxelData.chunkHeight - 1 || pos.z < 0 || pos.z > VoxelData.WorldSizeInVoxels - 1)
            return 0;
        if (pos.y < 1)
            return 1;
        else if (pos.y == VoxelData.chunkHeight - 1)
            return 3;
        else
            return 2;

    }

}

public class ChunkCoord2 {

    public int x;
    public int z;

    public ChunkCoord2 (int _x, int _z) {

        x = _x;
        z = _z;

    }

    public bool Equals(ChunkCoord2 other) {

        if (other == null)
            return false;
        else if (other.x == x && other.z == z)
            return true;
        else
            return false;

    }

}

[System.Serializable]
public class BlockType2 {

    public string blockName;
    public bool isSolid;

    [Header("Texture Values")]
    public int backFaceTexture;
    public int frontFaceTexture;
    public int topFaceTexture;
    public int bottomFaceTexture;
    public int leftFaceTexture;
    public int rightFaceTexture;

    // Back, Front, Top, Bottom, Left, Right

    public int GetTextureID (int faceIndex) {

        switch (faceIndex) {

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
                Debug.Log("Error in GetTextureID; invalid face index");
                return 0;


        }

    }

}
