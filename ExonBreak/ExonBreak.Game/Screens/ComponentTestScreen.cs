using ExonBreak.Game.Components.UI.Buttons;
using ExonBreak.Game.Components.UI.Form;
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
                Size = new Vector2(558, 600),
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
                    },
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
                                Content = new ExonFormTextbox
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
                                Content = new ExonFormTextbox
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
                ],
            }
        ];
    }
}
