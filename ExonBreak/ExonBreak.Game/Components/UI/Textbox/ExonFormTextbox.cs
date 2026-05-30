using System;
using ExonBreak.Game.Extensions;
using ExonBreak.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace ExonBreak.Game.Components.UI.Textbox;

public partial class ExonFormTextbox : CompositeDrawable
{
    private readonly Color4 backgroundColor = Branding.BACKGROUND0;
    private readonly Color4 borderColorActive = Branding.BLUE_BRIGHT;
    private readonly Color4 borderColorInactive = Branding.SURFACE0;
    private readonly Color4 borderColorActiveError = Branding.RED;

    private const float hover_additive_alpha = 0.03f;
    private const int error_container_rolled_out_height = 75;

    public Bindable<string> Current
    {
        get => current.Current;
        set => current.Current = value;
    }

    private readonly BindableWithCurrent<string> current = new BindableWithCurrent<string>();

    public required string PlaceholderText { get; init; }

    public IInputValidator[] ValidatorRules { get; set; } = [];

    private readonly Bindable<string?> errorText = new Bindable<string?>();
    private readonly BindableBool focused = new BindableBool();

    private Container container = null!;
    private SpriteText errorTextSprite = null!;
    private Container errorContainer = null!;
    private Container hoverLayer = null!;

    public bool HasValidatorErrors => errorText.Value != null;

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;

        InternalChildren =
        [
            errorContainer = new Container
            {
                RelativeSizeAxes = Axes.X,
                Height = 40,
                Masking = true,
                CornerRadius = 10,
                Alpha = 0,
                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = borderColorActiveError
                    },
                    errorTextSprite = new SpriteText
                    {
                        Text = "testing testing error!",
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        Colour = Color4.Black,
                        Font = new FontUsage("TiltNeon", 22)
                    }
                ]
            },

            container = new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Masking = true,
                CornerRadius = 10,
                BorderColour = borderColorInactive,
                BorderThickness = 3,
                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = backgroundColor
                    },
                    new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Padding = new MarginPadding(16),
                        Spacing = new Vector2(0, 4),
                        Children =
                        [
                            new InnerTextBox().With(textbox =>
                            {
                                textbox.RelativeSizeAxes = Axes.X;
                                textbox.Width = 1;
                                textbox.PlaceholderText = PlaceholderText;
                                textbox.Current = Current;
                                textbox.CommitOnFocusLost = true;
                                textbox.OnInputError = () => container.FlashBorderColor(Branding.RED, 300, Easing.InOutCubic);
                                textbox.Focused.BindValueChanged(e =>
                                {
                                    if (e.OldValue == e.NewValue) return;
                                    focused.Value = e.NewValue;
                                    updateStyle();
                                });
                            })
                        ]
                    }
                ]
            },
            hoverLayer = new Container
            {
                Masking = true,
                CornerRadius = 10,
                RelativeSizeAxes = Axes.Both,
                Alpha = 0,
                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Blending = BlendingParameters.Additive,
                        Colour = Color4.White
                    }
                ]
            }
        ];
    }

    private void updateStyle()
    {
        Color4 newBorderColor = focused.Value ? borderColorActive : borderColorInactive;
        if (HasValidatorErrors) newBorderColor = borderColorActiveError;

        container.FadeBorderTo(newBorderColor, 300, Easing.OutCubic);

        if (HasValidatorErrors)
        {
            errorContainer.FadeIn(100, Easing.OutCubic);
            errorContainer.ResizeHeightTo(error_container_rolled_out_height, 300, Easing.OutCubic);
        }
        else
        {
            errorContainer.ResizeHeightTo(40, 550, Easing.OutCubic);
            errorContainer.FadeOut(500, Easing.OutCubic);
        }
    }

    protected override void LoadComplete()
    {
        Current.BindValueChanged(e =>
        {
            foreach (var rule in ValidatorRules)
            {
                var result = rule.Validate(e.NewValue);
                if (result.Passed) continue;

                errorText.Value = result.Error;
                return;
            }

            errorText.Value = null;
        });

        errorText.BindValueChanged(e =>
        {
            if (e.OldValue == e.NewValue) return;
            if (e.NewValue != null) errorTextSprite.Text = e.NewValue;
            updateStyle();
        }, true);
    }

    protected override bool OnHover(HoverEvent e)
    {
        hoverLayer.FadeTo(hover_additive_alpha, 300, Easing.OutCubic);
        return true;
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        hoverLayer.FadeTo(0f, 300, Easing.OutCubic);
    }

    public interface IInputValidator
    {
        Result Validate(string input);

        record Result(bool Passed, string? Error)
        {
            public static readonly Result PASS = new Result(true, null);
            public static readonly Result NULL_OR_WHITE_SPACE = new Result(false, "Input cannot be empty");
        };
    }

    internal partial class InnerTextBox : TextBox
    {
        public BindableBool Focused { get; } = new BindableBool();

        public Action? OnInputError { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            Height = 25;
            TextContainer.Height = 1;
        }

        protected override void OnFocus(FocusEvent e)
        {
            base.OnFocus(e);

            Focused.Value = true;
        }

        protected override void OnFocusLost(FocusLostEvent e)
        {
            base.OnFocusLost(e);

            Focused.Value = false;
        }

        protected override void NotifyInputError()
        {
            // base call intentionally suppressed
            OnInputError?.Invoke();
        }

        protected override SpriteText CreatePlaceholder()
        {
            return new BasicTextBox.FadingPlaceholderText
            {
                Colour = Branding.SURFACE1,
                Font = new FontUsage("TiltNeon"),
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft
            };
        }

        protected override Caret CreateCaret() =>
            new BasicTextBox.BasicCaret().With(c =>
            {
                c.SelectionColour = Branding.BLUE_BRIGHT;
                c.CaretWidth = 2f;
            });
    }
}
