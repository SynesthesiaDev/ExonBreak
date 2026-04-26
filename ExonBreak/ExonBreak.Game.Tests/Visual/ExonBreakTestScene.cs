using osu.Framework.Testing;

namespace ExonBreak.Game.Tests.Visual
{
    public abstract partial class ExonBreakTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new ExonBreakTestSceneTestRunner();

        private partial class ExonBreakTestSceneTestRunner : ExonBreakGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}