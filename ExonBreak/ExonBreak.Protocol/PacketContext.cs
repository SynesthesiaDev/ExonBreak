using Codon.Optionals;
using DotNetty.Transport.Channels;
using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Protocol.Types.Text;
using ExonBreak.Protocol.Types.Text.Extensions;
using ExonBreak.Server.Protocol;

namespace ExonBreak.Protocol;

public class PacketContext(PlayerConnection playerConnection, Action<string> logFunction, ProtocolSide side)
{
    public IChannelHandlerContext ChannelHandlerContext { get; } = playerConnection.ChannelHandlerContext;

    public PlayerConnection PlayerConnection { get; } = playerConnection;

    public void SendPacket(IPacket packet)
    {
        logFunction.Invoke($"({side.ToString()}) ← {packet.GetType().Name}");
        ChannelHandlerContext.Channel.WriteAndFlushAsync(packet);
    }

    public void Disconnect(string reason) => Disconnect(ClientboundDisconnectPacket.Reason.Custom, reason);

    public void Disconnect(ClientboundDisconnectPacket.Reason reason, string? customReason = null)
    {
        var customReasonString = customReason != null ? $"({customReason})" : "";
        var component = customReason != null ? Optional.Of($"{reason} {customReasonString}".ToFormattedText()) : Optional.Empty<FormattedText>();

        SendPacket(new ClientboundDisconnectPacket(reason, component));

        ChannelHandlerContext.CloseAsync();
        logFunction.Invoke($"({side.ToString()}) /!\\ Connection closed with reason: {reason} {customReasonString}");
    }
}
