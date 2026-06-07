using System;
using System.Threading.Tasks;
using ExonBreak.Protocol.Types.Player;
using ExonBreak.Protocol.Types.Text;
using osu.Framework.Bindables;
using osu.Framework.Logging;

namespace ExonBreak.Game.Protocol;

public class MultiplayerSessionController : IDisposable
{
    public readonly Bindable<State> ConnectionState = new Bindable<State>();
    public GameClient? GameClient { get; private set; }

    public IDisconnectReason? LastDisconnectReason { get; private set; }

    private int connectionAttemptId;

    public async Task Connect(Request request)
    {
        if (GameClient != null)
            throw new InvalidOperationException("Already connected or connecting.");
        if (ConnectionState.Value != State.Disconnected) return;

        ConnectionState.Value = State.Connecting;

        var client = new GameClient(request.PlayerInfo, request.IpAddress, request.Port);
        GameClient = client;

        var attemptId = ++connectionAttemptId;
        LastDisconnectReason = null;

        try
        {
            await client.Connect();

            if (attemptId != connectionAttemptId)
            {
                Disconnect(new ExceptionDisconnectReason(new InvalidOperationException("attempt id desync")));
                return;
            }

            ConnectionState.Value = State.Connecting;
        }
        catch (Exception exception)
        {
            if (GameClient == client) GameClient = null;

            LastDisconnectReason = new ExceptionDisconnectReason(exception);
            Disconnect(LastDisconnectReason);
            throw;
        }
    }

    public void MarkAsFullyConnected()
    {
        if (ConnectionState.Value != State.Negotiating)
            return;

        ConnectionState.Value = State.Connected;
    }

    public void Disconnect() => Disconnect(new ClientDisconnectReason());

    public void Disconnect(IDisconnectReason reason)
    {
        GameClient?.Disconnect();
        GameClient?.Dispose();
        GameClient = null;

        Logger.Log($"(Client) Disconnected, reason: {reason.GetStringifiedReason()}", LoggingTarget.Network, LogLevel.Important);

        ConnectionState.Value = reason switch
        {
            ClientDisconnectReason => State.Disconnected,
            ExceptionDisconnectReason => State.Failed,
            ServerDisconnectReason => State.Failed,
            _ => throw new ArgumentOutOfRangeException(nameof(reason))
        };
    }

    public enum State
    {
        Disconnected,
        Connecting,
        Negotiating,
        Connected,
        Failed
    }

    public record Request(PlayerInfo PlayerInfo, string IpAddress, int Port);

    public interface IDisconnectReason
    {
        string GetStringifiedReason();
    }

    public record ExceptionDisconnectReason(Exception Exception) : IDisconnectReason
    {
        public string GetStringifiedReason() => Exception.ToString();
    };

    public record ServerDisconnectReason(FormattedText FormattedText) : IDisconnectReason
    {
        public string GetStringifiedReason() => FormattedText.ToString();
    };

    public record ClientDisconnectReason : IDisconnectReason
    {
        public string GetStringifiedReason() => "Client Initiated Disconnect";
    }

    public void Dispose()
    {
        Disconnect(new ClientDisconnectReason());

        GameClient?.Dispose();
        ConnectionState.UnbindAll();
    }
}
