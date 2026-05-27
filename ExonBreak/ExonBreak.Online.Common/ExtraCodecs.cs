using Codon.Codec;
using Codon.Codec.Transcoder;

namespace ExonBreak.Online.Common;

public sealed class ExtraCodecs
{
    public static readonly Codec<Guid> GUID = new GuidCodec();
    
    public class GuidCodec : Codec<Guid>
    {
        public override TD Encode<TD>(ITranscoder<TD> transcoder, Guid value) => transcoder.EncodeString(value.ToString());
        public override Guid Decode<TD>(ITranscoder<TD> transcoder, TD value) => Guid.Parse(transcoder.DecodeString(value));
    }
}