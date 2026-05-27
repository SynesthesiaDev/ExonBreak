using ExonBreak.Protocol;
using ExonBreak.Server.Protocol.Netty;

namespace ExonBreak.Server.Extensions;

public static class PlayerConnectionExtensions
{
    extension(PlayerConnection connection)
    {
        public ServerPlayerConnection AsServer() => (ServerPlayerConnection)connection;
    }
}
