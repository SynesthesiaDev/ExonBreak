using Codon.Binary;
using ExonBreak.Protocol.Extensions;

namespace ExonBreak.Protocol.World.Chunk;

public record ExtraTileData(Dictionary<string, ExtraTileData.DataHolder> Holders)
{
    public ExtraTileData() : this(new Dictionary<string, DataHolder>()) { }

    public static readonly IBinaryCodec<ExtraTileData> CODEC = BinaryCodecs.For<ExtraTileData>()
        .Field(BinaryCodecs.STRING.MapTo(DataHolder.CODEC), e => e.Holders)
        .Build(holders => new ExtraTileData(holders));

    public void Add<T>(string name, Type type, T value) where T : notnull => Holders[name] = new DataHolder(name, type, value);
    public T Get<T>(string name) where T : notnull => (T)Holders[name].Value;

    public void AddInt(string name, int value) => Add(name, Type.Int, value);
    public void AddBoolean(string name, bool value) => Add(name, Type.Boolean, value);
    public void AddString(string name, string value) => Add(name, Type.String, value);
    public void AddFloat(string name, float value) => Add(name, Type.Float, value);
    public void AddDouble(string name, double value) => Add(name, Type.Double, value);

    public void GetInt(string name) => Get<int>(name);
    public void GetBoolean(string name) => Get<bool>(name);
    public void GetString(string name) => Get<string>(name);
    public void GetFloat(string name) => Get<float>(name);
    public void GetDouble(string name) => Get<double>(name);

    public record DataHolder(string Name, Type Type, object Value)
    {
        public static readonly IBinaryCodec<DataHolder> CODEC = BinaryCodecExtensions.CustomCodec<DataHolder>(
            (buffer, data) =>
            {
                BinaryCodecs.STRING.Write(buffer, data.Name);
                BinaryCodecs.Enum<Type>().Write(buffer, data.Type);
                switch (data.Type)
                {
                    case Type.Int:
                        BinaryCodecs.VAR_INT.Write(buffer, (int)data.Value);
                        break;
                    case Type.Boolean:
                        BinaryCodecs.BOOLEAN.Write(buffer, (bool)data.Value);
                        break;
                    case Type.String:
                        BinaryCodecs.STRING.Write(buffer, (string)data.Value);
                        break;
                    case Type.Float:
                        BinaryCodecs.FLOAT.Write(buffer, (float)data.Value);
                        break;
                    case Type.Double:
                        BinaryCodecs.DOUBLE.Write(buffer, (double)data.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            },
            buffer =>
            {
                var name = BinaryCodecs.STRING.Read(buffer);
                var type = BinaryCodecs.Enum<Type>().Read(buffer);

                object returnObject = type switch
                {
                    Type.Int => BinaryCodecs.VAR_INT.Read(buffer),
                    Type.Boolean => BinaryCodecs.BOOLEAN.Read(buffer),
                    Type.String => BinaryCodecs.STRING.Read(buffer),
                    Type.Float => BinaryCodecs.FLOAT.Read(buffer),
                    Type.Double => BinaryCodecs.DOUBLE.Read(buffer),
                    _ => throw new ArgumentOutOfRangeException()
                };

                return new DataHolder(name, type, returnObject);
            }
        );
    }

    public enum Type
    {
        Int,
        Boolean,
        String,
        Float,
        Double,
    }
}
