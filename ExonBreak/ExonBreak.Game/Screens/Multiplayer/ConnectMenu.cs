using System.Threading.Tasks;
using ExonBreak.Game.Components.UI.Buttons;
using ExonBreak.Game.Components.UI.Form;
using ExonBreak.Game.Components.UI.Textbox;
using ExonBreak.Game.Protocol;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Types;
using ExonBreak.Protocol.Types.Player;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osuTK;

namespace ExonBreak.Game.Screens.Multiplayer;

public partial class ConnectMenu : Screen
{
    private ExonFormTextbox username = null!;
    private ExonFormTextbox pronouns = null!;
    private ExonFormTextbox ip = null!;

    [Resolved]
    private MultiplayerSessionController sessionController { get; set; } = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren =
        [
            new FillFlowContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Width = 558,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 15),
                Scale = new Vector2(1.3f),
                Children =
                [
                    new FormContainer
                    {
                        Content =
                        [
                            new FormLabel
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 62,
                                Title = "// Player Username",
                                Subtitle = "Your username displayed to other players",
                                Content = username = new ExonFormTextbox
                                {
                                    PlaceholderText = "Player Username..",
                                    ValidatorRules =
                                    [
                                        TextboxInputValidators.NON_EMPTY,
                                        TextboxInputValidators.ALPHANUMERIC,
                                        TextboxInputValidators.MaxLenght(16),
                                        TextboxInputValidators.MinLength(3)
                                    ]
                                }
                            },

                            new FormLabel
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 62,
                                Title = "// Pronouns (Optional)",
                                Subtitle = "Displayed to others in front of your name",
                                Content = pronouns = new ExonFormTextbox
                                {
                                    PlaceholderText = "My/Pronouns",
                                    ValidatorRules =
                                    [
                                        TextboxInputValidators.MaxLenght(24),
                                        TextboxInputValidators.MinLength(2)
                                    ]
                                },
                            }
                        ]
                    },
                    new FormContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 62,
                        Content =
                        [
                            new FormLabel
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 62,
                                Title = "// IP Address",
                                Subtitle = "IP Address of server to connect to",
                                Content = ip = new ExonFormTextbox
                                {
                                    PlaceholderText = "IP Address..",
                                    ValidatorRules =
                                    [
                                        TextboxInputValidators.NON_EMPTY,
                                        TextboxInputValidators.IP_ADDRESS
                                    ]
                                },
                            },
                            new ExonButton
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 40,
                                Text = "-> Connect",
                                Clicked = () => _ = connect()
                            },
                            new ExonButton
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 40,
                                Text = "Dev Localhost Test",
                                Clicked = () =>
                                {
                                    username.Current.Value = "syn";
                                    pronouns.Current.Value = "she/her";
                                    ip.Current.Value = "localhost";
                                    _ = connect();
                                },
                            },
                        ]
                    }
                ],
            }
        ];
    }


    private async Task connect()
    {
        var playerInfo = new PlayerInfo
        (
            SharedConstants.PROTOCOL_VERSION,
            ExonBreakGameBase.Identity.Guid,
            username.Current.Value,
            pronouns.Current.Value,
            Platform.Commodore64
        );
        var request = new MultiplayerSessionController.Request(playerInfo, ip.Current.Value, SharedConstants.DEFAULT_PORT);
        await sessionController.Connect(request);

        this.Push(new LoadingScreen());
    }
}
