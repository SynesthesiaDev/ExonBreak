using ExonBreak.Game.Protocol;
using ExonBreak.Protocol;

namespace ExonBreak.Game.Extensions;

public static class PlayerConnectionExtensions
{
    extension(PlayerConnection connection)
    {
        public ClientPlayerConnection AsClient() => (ClientPlayerConnection)connection;
    }
}
