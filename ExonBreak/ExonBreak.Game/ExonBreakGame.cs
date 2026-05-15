using ExonBreak.Game.Screens.MultiplayerMenu;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace ExonBreak.Game;

public partial class ExonBreakGame : ExonBreakGameBase
{
    private ScreenStack screenStack;

    [BackgroundDependencyLoader]
    private void load()
    {
        Child = screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        screenStack.Push(new MultiplayerMenuScreen());
    }
}
