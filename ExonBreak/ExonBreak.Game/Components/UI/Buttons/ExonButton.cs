using System;
using ExonBreak.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace ExonBreak.Game.Components.UI.Buttons;

public partial class ExonButton : CompositeDrawable
{
    private readonly Color4 backgroundColor = Branding.BLUE_BRIGHT;
    private const float hover_additive_alpha = 0.1f;

    private Box hoverLayer = null!;
    private Container contentContainer = null!;

    public required string Text { get; init; }

    public required Action Clicked { get; init; }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren =
        [
            contentContainer = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Children =
                [
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Children =
                        [
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Masking = true,
                                CornerRadius = 10,
                                BorderColour = ColourInfo.GradientVertical(Branding.BLUE_BRIGHT_HIGHLIGHT, backgroundColor),
                                BorderThickness = 2,
                                Children =
                                [
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = backgroundColor
                                    },
                                    new SpriteText
                                    {
                                        Font = new FontUsage("TiltNeon", 22),
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Text = Text,
                                        Colour = Color4.Black
                                    },
                                ]
                            },

                            hoverLayer = new Box
                            {
                                Blending = BlendingParameters.Additive,
                                Alpha = 0,
                                RelativeSizeAxes = Axes.Both
                            }
                        ]
                    }
                ]
            },
        ];
    }

    protected override bool OnMouseDown(MouseDownEvent e)
    {
        contentContainer.ScaleTo(0.9f, 2000, Easing.OutQuint);
        return true;
    }

    protected override void OnMouseUp(MouseUpEvent e)
    {
        contentContainer.ScaleTo(1, 1000, Easing.OutElastic);
        Clicked.Invoke();
    }

    protected override bool OnHover(HoverEvent e)
    {
        hoverLayer.FadeTo(hover_additive_alpha, 150, Easing.OutCubic);
        return true;
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        hoverLayer.FadeTo(0f, 300, Easing.OutCubic);
    }
}
