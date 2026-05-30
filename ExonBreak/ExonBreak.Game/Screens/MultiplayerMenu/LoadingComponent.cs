using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace ExonBreak.Game.Screens.MultiplayerMenu;

public partial class LoadingComponent : CompositeDrawable
{
    public required Action OnCancel;

    private SpriteIcon spinnerIcon = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        AutoSizeAxes = Axes.Both;
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
                    new Container()
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
                        Action = () => OnCancel.Invoke()
                    }
                ]
            }
        ];
    }

    protected override void LoadComplete()
    {
        spinnerIcon.Spin(1000, RotationDirection.Clockwise, startRotation: 0);
        base.LoadComplete();
    }
}
