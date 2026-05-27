using System.Collections.Generic;
using Codon.Codec;
using ExonBreak.Online.Common.Serialization;
using MPack;
using NUnit.Framework;

namespace ExonBreak.Tests.OnlineServices;

[TestFixture]
public class MessagePackTests
{
    private static MessagePackTranscoder Transcoder => MessagePackTranscoder.INSTANCE;

    [Test]
    public void EncodeNullReturnsNullToken()
    {
        var token = Transcoder.EncodeNull();

        Assert.That(token, Is.EqualTo(MToken.Null()));
    }

    [Test]
    public void EncodeAndDecodeBoolRoundTrips()
    {
        var encoded = Transcoder.EncodeBool(true);

        var decoded = Transcoder.DecodeBool(encoded);

        Assert.That(decoded, Is.True);
    }

    [Test]
    public void EncodeAndDecodeByteRoundTrips()
    {
        const byte value = 123;

        var encoded = Transcoder.EncodeByte(value);

        Assert.That(Transcoder.DecodeByte(encoded), Is.EqualTo(value));
    }

    [Test]
    public void EncodeAndDecodeShortRoundTrips()
    {
        const short value = -12345;

        var encoded = Transcoder.EncodeShort(value);

        Assert.That(Transcoder.DecodeShort(encoded), Is.EqualTo(value));
    }

    [Test]
    public void EncodeAndDecodeIntRoundTrips()
    {
        const int value = -123456789;

        var encoded = Transcoder.EncodeInt(value);

        Assert.That(Transcoder.DecodeInt(encoded), Is.EqualTo(value));
    }

    [Test]
    public void EncodeAndDecodeLongRoundTrips()
    {
        const long value = -1234567890123456789L;

        var encoded = Transcoder.EncodeLong(value);

        Assert.That(Transcoder.DecodeLong(encoded), Is.EqualTo(value));
    }

    [Test]
    public void EncodeAndDecodeFloatRoundTrips()
    {
        const float value = 123.456f;

        var encoded = Transcoder.EncodeFloat(value);

        Assert.That(Transcoder.DecodeFloat(encoded), Is.EqualTo(value));
    }

    [Test]
    public void EncodeAndDecodeDoubleRoundTrips()
    {
        const double value = 123.456789;

        var encoded = Transcoder.EncodeDouble(value);

        Assert.That(Transcoder.DecodeDouble(encoded), Is.EqualTo(value));
    }

    [Test]
    public void EncodeAndDecodeStringRoundTrips()
    {
        const string value = "hello messagepack";

        var encoded = Transcoder.EncodeString(value);

        Assert.That(Transcoder.DecodeString(encoded), Is.EqualTo(value));
    }

    [Test]
    public void EncodeListBuildsMessagePackArrayInInsertionOrder()
    {
        var token = Transcoder.EncodeList(3)
            .Add(Transcoder.EncodeInt(1))
            .Add(Transcoder.EncodeString("two"))
            .Add(Transcoder.EncodeBool(true))
            .Build();

        var array = assertAndCast<MArray>(token);

        Assert.That(array, Has.Count.EqualTo(3));
        Assert.That(Transcoder.DecodeInt(array[0]), Is.EqualTo(1));
        Assert.That(Transcoder.DecodeString(array[1]), Is.EqualTo("two"));
        Assert.That(Transcoder.DecodeBool(array[2]), Is.True);
    }

    [Test]
    public void EncodeMapBuildsMessagePackDictionaryWithStringKeys()
    {
        var token = Transcoder.EncodeMap()
            .Put("number", Transcoder.EncodeInt(42))
            .Put("text", Transcoder.EncodeString("value"))
            .Build();

        var dict = assertAndCast<MDict>(token);

        Assert.That(dict, Has.Count.EqualTo(2));
        Assert.That(Transcoder.DecodeInt(dict[Transcoder.EncodeString("number")]), Is.EqualTo(42));
        Assert.That(Transcoder.DecodeString(dict[Transcoder.EncodeString("text")]), Is.EqualTo("value"));
    }

    [Test]
    public void EncodeMapBuildsMessagePackDictionaryWithTokenKeys()
    {
        var key = Transcoder.EncodeInt(7);
        var value = Transcoder.EncodeString("seven");

        var token = Transcoder.EncodeMap()
            .Put(key, value)
            .Build();

        var dict = assertAndCast<MDict>(token);

        Assert.That(dict, Has.Count.EqualTo(1));
        Assert.That(Transcoder.DecodeString(dict[key]), Is.EqualTo("seven"));
    }

    private static TToken assertAndCast<TToken>(MToken token)
        where TToken : MToken
    {
        Assert.That(token, Is.TypeOf<TToken>());
        return (TToken)token;
    }

    [Test]
    public void StructCodecCanRoundTripComplexObjectThroughMessagePackTranscoder()
    {
        var expected = new TestPlayerState(
            Id: 42,
            Name: "Test Player",
            IsOnline: true,
            Position: new TestPosition(128, -64),
            InventoryItemIds: [1, 2, 3, 99]
        );

        var encoded = TestPlayerState.CODEC.Encode(MessagePackTranscoder.INSTANCE, expected);

        var decoded = TestPlayerState.CODEC.Decode(MessagePackTranscoder.INSTANCE, encoded);

        Assert.That(decoded.ToString(), Is.EqualTo(expected.ToString()));
    }


    private record TestPlayerState(
        int Id,
        string Name,
        bool IsOnline,
        TestPosition Position,
        List<int> InventoryItemIds
    )
    {
        public static readonly StructCodec<TestPlayerState> CODEC = StructCodec.For<TestPlayerState>()
            .Field("id", Codecs.INT, state => state.Id)
            .Field("name", Codecs.STRING, state => state.Name)
            .Field("is_online", Codecs.BOOLEAN, state => state.IsOnline)
            .Field("position", TestPosition.CODEC, state => state.Position)
            .Field("inventory_item_ids", Codecs.INT.List(), state => state.InventoryItemIds)
            .Build((id, name, isOnline, position, inventoryItemIds) =>
                new TestPlayerState(id, name, isOnline, position, inventoryItemIds));
    }

    private record TestPosition(int X, int Z)
    {
        public static readonly StructCodec<TestPosition> CODEC = StructCodec.For<TestPosition>()
            .Field("x", Codecs.INT, position => position.X)
            .Field("z", Codecs.INT, position => position.Z)
            .Build((x, z) => new TestPosition(x, z));
    }
}

