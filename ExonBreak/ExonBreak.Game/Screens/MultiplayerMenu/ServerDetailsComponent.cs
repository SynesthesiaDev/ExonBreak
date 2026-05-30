using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace ExonBreak.Game.Screens.MultiplayerMenu;

public sealed partial class ServerDetailsComponent : CompositeDrawable
{
    private TextBox usernameText = null!;
    private TextBox ipText = null!;
    private TextBox pronounsText = null!;

    public required Action<FormData> OnFormSubmitted;

    public record FormData(string Username, string Pronouns, string IpAddress);

    public ServerDetailsComponent()
    {
        AutoSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren =
        [
            new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(25),
                Children =
                [
                    usernameText = new BasicTextBox
                    {
                        Size = new Vector2(500, 60),
                        PlaceholderText = "Player name",
                        Text = "Syn"
                    },
                    pronounsText = new BasicTextBox
                    {
                        Size = new Vector2(500, 60),
                        PlaceholderText = "Pronouns",
                        Text = "She/Her"
                    },
                    ipText = new BasicTextBox
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
                            var formData = new FormData(usernameText.Current.Value, pronounsText.Current.Value, ipText.Current.Value);
                            OnFormSubmitted.Invoke(formData);
                        }
                    }
                ]
            }
        ];
    }

    protected override void Dispose(bool isDisposing)
    {
        OnFormSubmitted = null;
        base.Dispose(isDisposing);
    }
}
