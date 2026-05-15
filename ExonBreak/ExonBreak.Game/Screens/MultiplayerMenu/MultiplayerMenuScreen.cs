using System;
using ExonBreak.Game.Protocol;
using ExonBreak.Game.Utils;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Types;
using ExonBreak.Protocol.Types.Player;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;

namespace ExonBreak.Game.Screens.MultiplayerMenu;

public partial class MultiplayerMenuScreen : Screen
{
    private TextBox username = null!;
    private TextBox ip = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren =
        [
            new Box
            {
                Colour = Branding.BACKGROUND1,
                RelativeSizeAxes = Axes.Both,
            },
            new FillFlowContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(25),
                Children =
                [
                    username = new BasicTextBox
                    {
                        Size = new Vector2(500, 60),
                        PlaceholderText = "Player name",
                        Text = "Syn",
                    },
                    ip = new BasicTextBox
                    {
                        Size = new Vector2(500, 60),
                        PlaceholderText = "Server IP",
                        Text = "127.0.0.1"
                    },
                    new BasicButton
                    {
                        Size = new Vector2(500, 60),
                        Text = "Play",
                        Action = () =>
                        {
                            var playerInfo = new PlayerInfo(SharedConstants.PROTOCOL_VERSION, Guid.NewGuid(), username.Current.Value, "They/Them", Platform.Windows);
                            var client = new GameClient(playerInfo, ip.Current.Value);

                            _ = client.Connect();
                        }
                    }
                ],
            }
        ];
    }
}
