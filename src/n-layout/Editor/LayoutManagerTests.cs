#if N_LAYOUT_TESTS
using NUnit.Framework;
using N.Package.Animation;
using N.Package.Layout;
using UnityEngine;
using N;

public class LayoutManagerTests : N.Tests.Test
{
    public static void Reset(N.Tests.Test test)
    {
        test.TearDown();
        LayoutManager.Reset();
        AnimationHandler.Reset();
    }

    [Test]
    public void test_get_default_instances()
    {
        LayoutManagerTests.Reset(this);
        var instance = LayoutManager.Default;
        Assert(instance != null);
        LayoutManagerTests.Reset(this);
    }
}
#endif
