using System.Runtime.CompilerServices;
using Codon.Binary;
using ExonBreak.Protocol.Extensions;

namespace ExonBreak.Protocol.World.Chunk;

public class TileChunk(ChunkTileIndex index, int[] tiles, Dictionary<int, ExtraTileData> extraTileData)
{
    private static readonly IBinaryCodec<Dictionary<int, ExtraTileData>> extra_tile_data_codec = BinaryCodecs.VAR_INT.MapTo(ExtraTileData.CODEC);

    public const int CHUNK_SIZE = 16;
    public const int EMPTY_TILE_ID = 0;

    public ReadOnlySpan<int> Tiles => (ReadOnlySpan<int>)tiles;

    // Extra tile data, stored by tile index
    private readonly Dictionary<int, ExtraTileData> extraTileData = extraTileData;

    public ChunkTileIndex Index = index;

    public TileChunk(ChunkTileIndex index) : this(index, new int[CHUNK_SIZE * CHUNK_SIZE], []) { }


    public delegate void DirtyEvent();

    public event DirtyEvent OnTileChunkDirty;

    public static readonly IBinaryCodec<TileChunk> CODEC = BinaryCodecExtensions.CustomCodec<TileChunk>(
        (buffer, chunk) =>
        {
            ChunkTileIndex.CODEC.Write(buffer, chunk.Index);
            extra_tile_data_codec.Write(buffer, chunk.extraTileData);

            var nonEmptyTiles = new List<(int, int)>();
            foreach (var (index, _, _, tileId) in chunk.CoordIterator())
            {
                if (tileId == EMPTY_TILE_ID) continue;
                nonEmptyTiles.Add((index, tileId));
            }

            BinaryCodecs.VAR_INT.Write(buffer, nonEmptyTiles.Count);
            foreach (var (index, tileId) in nonEmptyTiles)
            {
                BinaryCodecs.VAR_INT.Write(buffer, index);
                BinaryCodecs.VAR_INT.Write(buffer, tileId);
            }
        },
        buffer =>
        {
            var chunkIndex = ChunkTileIndex.CODEC.Read(buffer);
            var extraTileData = extra_tile_data_codec.Read(buffer);
            var size = BinaryCodecs.VAR_INT.Read(buffer);

            var tiles = new int[CHUNK_SIZE * CHUNK_SIZE];
            for (int i = 0; i < size; i++)
            {
                var index = BinaryCodecs.VAR_INT.Read(buffer);
                var tileId = BinaryCodecs.VAR_INT.Read(buffer);

                if (index < 0 || index >= tiles.Length)
                    throw new InvalidDataException($"Tile index {index} is outside the chunk bounds.");

                tiles[index] = tileId;
            }

            return new TileChunk(chunkIndex, tiles, extraTileData);
        }
    );

    public int GetTile(int index)
    {
        if (index < 0 || index >= tiles.Length)
            return EMPTY_TILE_ID;
        return tiles[index];
    }

    public void SetTile(int index, int tileId)
    {
        if (index < 0 || index >= tiles.Length)
            throw new ArgumentOutOfRangeException(nameof(index), $"Index was either negative or more than the tile chunk can hold ({index})");

        tiles[index] = tileId;
        OnTileChunkDirty?.Invoke();
    }

    public ExtraTileData? GetExtraTileData(int index)
    {
        if (index < 0 || index >= tiles.Length)
            return null;

        return extraTileData.GetValueOrDefault(index);
    }

    public void SetExtraTileData(int index, ExtraTileData data)
    {
        if (index < 0 || index >= tiles.Length)
            throw new ArgumentOutOfRangeException(nameof(index), $"Index was either negative or more than the tile chunk can hold ({index})");

        extraTileData[index] = data;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetExtraTileData(int x, int z, ExtraTileData data) => SetExtraTileData(GetTileIndex(x, z), data);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ExtraTileData? GetExtraTileData(int x, int z) => GetExtraTileData(GetTileIndex(x, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetTile(int x, int z, int tileId) => SetTile(GetTileIndex(x, z), tileId);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetTile(int x, int z) => GetTile(GetTileIndex(x, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetTileIndex(int x, int z) => x + z * CHUNK_SIZE;

    public IEnumerable<(int Index, int X, int Z, int TileId)> CoordIterator()
    {
        for (int index = 0; index < tiles.Length; index++)
        {
            int x = index % CHUNK_SIZE;
            int z = index / CHUNK_SIZE;

            yield return (index, x, z, tiles[index]);
        }
    }
}
