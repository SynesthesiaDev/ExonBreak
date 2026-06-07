using ExonBreak.Game.Persistent;
using ExonBreak.Game.Protocol;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using ExonBreak.Resources;
using osu.Framework.Platform;

namespace ExonBreak.Game;

public partial class ExonBreakGameBase : osu.Framework.Game
{
    // Anything in this class is shared between the test browser and the game implementation.
    // It allows for caching global dependencies that should be accessible to tests, or changing
    // the screen scaling for all components including the test browser and framework overlays.

    protected override Container<Drawable> Content { get; }

    private DependencyContainer dependencies;

    protected Storage Storage { get; set; } = null!;
    public static PlayerIdentity Identity { get; private set; } = null!;

    public static readonly MultiplayerSessionController MULTIPLAYER_SESSION_CONTROLLER = new MultiplayerSessionController();

    protected ExonBreakGameBase()
    {
        base.Content.Add(Content = new Container()
        {
            RelativeSizeAxes = Axes.Both
        });
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Resources.AddStore(new DllResourceStore(typeof(ExonBreakResources).Assembly));

        dependencies.CacheAs(MULTIPLAYER_SESSION_CONTROLLER);

        Storage = Host.Storage;
        Identity = PlayerIdentity.LoadOrCreate(Storage);
    }


    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
}
