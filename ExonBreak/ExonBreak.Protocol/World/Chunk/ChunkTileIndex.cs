using Codon.Binary;

namespace ExonBreak.Protocol.World.Chunk;

public readonly struct ChunkTileIndex(int x, int z) : IProtocolObject
{
    private const uint magic = 0xFFFFFFFF;
    public static readonly ChunkTileIndex ZERO = new ChunkTileIndex(0, 0);

    public static readonly IBinaryCodec<ChunkTileIndex> CODEC = BinaryCodecs.LONG.Transform(c => c.Packed, FromEncoded);

    public readonly int X = x;
    public readonly int Z = z;

    public readonly long Packed = Pack(x, z);

    public static long Pack(int x, int z) => (uint)x | ((long)(uint)z << 32);
    public static int UnpackX(long encoded) => (int)(encoded & magic);
    public static int UnpackZ(long encoded) => (int)(encoded >>> 32 & magic);
    public static (int x, int z) Unpack(long encoded) => (UnpackX(encoded), UnpackZ(encoded));

    public static ChunkTileIndex FromEncoded(long encoded)
    {
        var unpacked = Unpack(encoded);
        return new ChunkTileIndex(unpacked.Item1, unpacked.Item2);
    }

    public override string ToString() => $"({X}, {Z})";
}
