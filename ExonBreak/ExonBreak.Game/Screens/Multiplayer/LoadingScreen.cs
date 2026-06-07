using ExonBreak.Game.Protocol;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;

namespace ExonBreak.Game.Screens.Multiplayer;

public partial class LoadingScreen : Screen
{
    private SpriteIcon spinnerIcon = null!;

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
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(25),
                AutoSizeAxes = Axes.Both,
                Children =
                [
                    new Container
                    {
                        Size = new Vector2(500, 100),
                        Children =
                        [
                            spinnerIcon = new SpriteIcon
                            {
                                Icon = FontAwesome.Solid.Spinner,
                                Size = new Vector2(60),
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre
                            }
                        ]
                    },

                    new BasicButton
                    {
                        Size = new Vector2(500, 60),
                        Text = "Cancel",
                        Action = () =>
                        {
                            sessionController.Disconnect();
                            this.Exit();
                        }
                    }
                ]
            }
        ];
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        spinnerIcon.Spin(1000, RotationDirection.Clockwise, startRotation: 0);

        sessionController.ConnectionState.BindValueChanged(e =>
        {
            if (e.NewValue is MultiplayerSessionController.State.Failed or MultiplayerSessionController.State.Disconnected)
            {
                this.Exit();
            }
        });
    }
}
