using Codon.Codec.Transcoder;
using MPack;

namespace ExonBreak.Online.Common.Serialization;

public class MessagePackTranscoder : ITranscoder<MToken>
{
    public static readonly MessagePackTranscoder INSTANCE = new MessagePackTranscoder();

    public sealed class MessagePackListBuilder(MArray array) : IListBuilder<MToken>
    {
        public IListBuilder<MToken> Add(MToken value)
        {
            array.Add(value);
            return this;
        }

        public MToken Build() => array;
    }

    public sealed class MessagePackVirtualMapBuilder(MDict dict) : IVirtualMapBuilder<MToken>
    {
        public IVirtualMapBuilder<MToken> Put(MToken key, MToken value)
        {
            dict.Add(key, value);
            return this;
        }

        public IVirtualMapBuilder<MToken> Put(string key, MToken value)
        {
            dict.Add(INSTANCE.EncodeString(key), value);
            return this;
        }

        public MToken Build() => dict;
    }

    public sealed class MessagePackVirtualMap(MDict dict) : IVirtualMap<MToken>
    {
        public List<string> GetKeys() => dict.Keys.ToList().ConvertAll(l => Convert.ToString(l.Value)!);

        public bool HasValue(string key) => dict.ContainsKey(key);

        public MToken GetValue(string key) => dict[key];
    }

    public MToken EncodeNull() => MToken.Null();
    public MToken EncodeInt(int value) => MToken.From(value);
    public MToken EncodeBool(bool value) => MToken.From(value);
    public MToken EncodeByte(byte value) => MToken.From(value);
    public MToken EncodeShort(short value) => MToken.From(value);
    public MToken EncodeLong(long value) => MToken.From(value);
    public MToken EncodeFloat(float value) => MToken.From(value);
    public MToken EncodeDouble(double value) => MToken.From(value);
    public MToken EncodeString(string value) => MToken.From(value);

    public IListBuilder<MToken> EncodeList(int size) => new MessagePackListBuilder(new MArray());
    public IVirtualMapBuilder<MToken> EncodeMap() => new MessagePackVirtualMapBuilder(new MDict());
    
    public bool DecodeBool(MToken value) => Convert.ToBoolean(value.Value);
    public byte DecodeByte(MToken value) => Convert.ToByte(value.Value);
    public short DecodeShort(MToken value) => Convert.ToInt16(value.Value);
    public int DecodeInt(MToken value) => Convert.ToInt32(value.Value);
    public long DecodeLong(MToken value) => Convert.ToInt64(value.Value);
    public float DecodeFloat(MToken value) => Convert.ToSingle(value.Value);
    public double DecodeDouble(MToken value) => Convert.ToDouble(value.Value);
    public string DecodeString(MToken value) => Convert.ToString(value.Value)!;

    public List<MToken> DecodeList(MToken value) => ((MArray)value).ToList();
    public IVirtualMap<MToken> DecodeMap(MToken value) => new MessagePackVirtualMap((MDict)value);
}