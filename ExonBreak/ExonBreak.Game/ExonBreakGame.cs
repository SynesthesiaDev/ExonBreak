using ExonBreak.Game.Screens;
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
        AddFont(Resources, @"Fonts/TiltNeon");
        Child = screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        // screenStack.Push(new MultiplayerMenuScreen());
        screenStack.Push(new ComponentTestScreen());
    }
}
