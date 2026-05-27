using ExonBreak.Protocol.World.Chunk;

namespace ExonBreak.Protocol.World.Tile;

public sealed class FixedTileMap : TileMap
{
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required bool PreallocateChunks { get; init; }

    public FixedTileMap()
    {
        if (!PreallocateChunks) return;

        for (int x = 0; x < Width; x++)
        for (int z = 0; z < Height; z++)
        {
            var index = new ChunkTileIndex(x, z);
            TileChunks[index.Packed] = new TileChunk(index);
        }
    }

    public override TileChunk? GetChunk(int x, int z)
    {
        if (x < 0 || x >= Width || z < 0 || z >= Height)
            return null;

        TileChunks.TryGetValue(new ChunkTileIndex(x, z).Packed, out var chunk);
        return chunk;
    }

    protected override IDictionary<long, TileChunk> TileChunks { get; } = new Dictionary<long, TileChunk>();

    public override TileChunk GetOrCreateChunk(int x, int z)
    {
        if (x < 0 || x >= Width || z < 0 || z >= Height)
            throw new ArgumentOutOfRangeException($"Chunk ({x}, {z}) is outside fixed tilemap bounds");

        return TileChunks[new ChunkTileIndex(x, z).Packed];
    }
}
