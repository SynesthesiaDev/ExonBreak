using ExonBreak.Protocol.World.Chunk;

namespace ExonBreak.Protocol.World.Tile;

public abstract class TileMap
{
    protected abstract IDictionary<long, TileChunk> TileChunks { get; }

    public IReadOnlyDictionary<long, TileChunk> Chunks => (IReadOnlyDictionary<long, TileChunk>)TileChunks;

    public virtual TileChunk? GetChunk(int x, int z) => Chunks.GetValueOrDefault(ChunkTileIndex.Pack(x, z));

    public abstract TileChunk GetOrCreateChunk(int x, int z);

    public int GetTile(int worldX, int worldZ)
    {
        var chunk = GetChunk(WorldToChunkCoord(worldX), WorldToChunkCoord(worldZ));
        return chunk?.GetTile(WorldToChunkLocalCoord(worldX), WorldToChunkLocalCoord(worldZ)) ?? TileChunk.EMPTY_TILE_ID;
    }

    public void SetTile(int worldX, int worldZ, int tileId)
    {
        var chunk = GetOrCreateChunk(WorldToChunkCoord(worldX), WorldToChunkCoord(worldZ));
        chunk.SetTile(WorldToChunkLocalCoord(worldX), WorldToChunkLocalCoord(worldZ), tileId);
    }

    public static int WorldToChunkCoord(int worldCoord) => worldCoord >> 4;

    public static int WorldToChunkLocalCoord(int worldCoord) => worldCoord & 15;
}
