using ExonBreak.Game.Protocol;
using ExonBreak.Game.Utils;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Types;
using ExonBreak.Protocol.Types.Player;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;

namespace ExonBreak.Game.Screens.MultiplayerMenu;

public partial class MultiplayerMenuScreen : Screen
{
    public ServerDetailsComponent ServerDetailsComponent = null!;
    public LoadingComponent LoadingComponent = null!;

    private readonly BindableBool connecting = new BindableBool();

    public PlayerInfo? PlayerInfo;

    public GameClient? GameClient;

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren =
        [
            new Box
            {
                Colour = Branding.BACKGROUND1,
                RelativeSizeAxes = Axes.Both
            },
            new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AutoSizeAxes = Axes.Both,
                Margin = new MarginPadding { Top = 40, Bottom = 40, Left = 20, Right = 20 },
                Masking = true,
                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Branding.SURFACE1
                    },

                    new FillFlowContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Children =
                        [
                            ServerDetailsComponent = new ServerDetailsComponent
                            {
                                OnFormSubmitted = form =>
                                {
                                    PlayerInfo = new PlayerInfo(
                                        SharedConstants.PROTOCOL_VERSION,
                                        ExonBreakGameBase.Identity.Guid,
                                        form.Username,
                                        form.Pronouns,
                                        Platform.Windows
                                    );
                                    GameClient = new GameClient(PlayerInfo, form.IpAddress);
                                    _ = GameClient.Connect();
                                    connecting.Value = true;

                                    GameClient.OnDisconnected.Subscribe(_ =>
                                    {
                                        connecting.Value = false;
                                    });
                                }
                            },

                            LoadingComponent = new LoadingComponent
                            {
                                OnCancel = () =>
                                {
                                    GameClient?.Disconnect();
                                }
                            }
                        ]
                    }
                ]
            }
        ];
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        connecting.BindValueChanged(e =>
        {
            if (!e.NewValue)
            {
                GameClient?.Dispose();
                GameClient = null;
            }

            ServerDetailsComponent.FadeTo(!e.NewValue ? 1 : 0, 0, Easing.OutQuint);
            LoadingComponent.FadeTo(e.NewValue ? 1 : 0, 0, Easing.OutQuint);
        }, true);
    }
}
