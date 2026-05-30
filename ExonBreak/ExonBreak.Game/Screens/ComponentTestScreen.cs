using ExonBreak.Game.Components.UI.Buttons;
using ExonBreak.Game.Components.UI.Textbox;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osuTK;

namespace ExonBreak.Game.Screens;

public partial class ComponentTestScreen : Screen
{
    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren =
        [
            new FillFlowContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(400, 600),
                Scale = new Vector2(2f),
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 15),
                Children =
                [
                    new ExonFormTextbox
                    {
                        PlaceholderText = "Player Username..",
                        ValidatorRules =
                        [
                            TextboxInputValidators.ALPHANUMERIC,
                            TextboxInputValidators.MaxLenght(16),
                            TextboxInputValidators.MinLength(3)
                        ]
                    },
                    new ExonButton
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 40,
                        Text = "+ Add Server"
                    }
                ],
            }
        ];
    }
}
