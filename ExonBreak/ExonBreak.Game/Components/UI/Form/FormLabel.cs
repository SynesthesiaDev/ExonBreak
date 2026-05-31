using System.Collections.Generic;
using ExonBreak.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace ExonBreak.Game.Components.UI.Form;

public partial class FormLabel : CompositeDrawable
{
    public required string Title { get; init; }
    public string? Subtitle { get; init; }

    public float LabelWidth { get; init; } = 250;

    public required Drawable Content { get; init; }

    [BackgroundDependencyLoader]
    private void load()
    {
        var labelFillFlowChildren = new List<Drawable>
        {
            new SpriteText
            {
                Font = new FontUsage("TileNeon", 24f),
                Text = Title
            }
        };

        if (Subtitle != null)
        {
            labelFillFlowChildren.Add(new SpriteText()
            {
                Font = new FontUsage("TileNeon"),
                Colour = Branding.TEXT4,
                Text = Subtitle!,
            });
        }

        Content.RelativeSizeAxes = Axes.Both;

        InternalChildren =
        [
            new Container
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                RelativeSizeAxes = Axes.Both,
                Children =
                [
                    new FillFlowContainer
                    {
                        Width = LabelWidth,
                        AutoSizeAxes = Axes.Y,
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Direction = FillDirection.Vertical,
                        Children = labelFillFlowChildren.ToArray()
                    },
                    new Container
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        RelativeSizeAxes = Axes.Both,
                        Width = 1,
                        Padding = new MarginPadding
                        {
                            Left = LabelWidth + 20
                        },
                        Child = Content.With(c =>
                        {
                            c.Anchor = Anchor.CentreRight;
                            c.Origin = Anchor.CentreRight;
                        })
                    }
                ]
            },
        ];
    }
}
