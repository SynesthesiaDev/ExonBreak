using ExonBreak.Protocol.World.Chunk;

namespace ExonBreak.Protocol.World.Tile;

public class InfiniteTileMap : TileMap
{
    protected override IDictionary<long, TileChunk> TileChunks { get; } = new Dictionary<long, TileChunk>();

    public override TileChunk GetOrCreateChunk(int x, int z)
    {
        var index = new ChunkTileIndex(x, z);
        if (TileChunks.TryGetValue(index.Packed, out var value)) return value;

        var chunk = new TileChunk(index);
        TileChunks[index.Packed] = chunk;
        return chunk;
    }
}
