using ExonBreak.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace ExonBreak.Game.Components.UI.Form;

public sealed partial class FormContainer : CompositeDrawable
{
    public required Drawable[] Content { get; init; }

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;

        InternalChildren =
        [
            new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Masking = true,
                BorderColour = Branding.SURFACE0,
                BorderThickness = 2,
                CornerRadius = 10,
                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Branding.SURFACE0.Opacity(0.005f),
                        AlwaysPresent = true,
                    },
                    new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 12),
                        Padding = new MarginPadding(20),
                        Children = Content
                    }
                ]
            },
        ];
    }
}
