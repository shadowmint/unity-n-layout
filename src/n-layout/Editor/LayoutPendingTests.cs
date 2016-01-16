#if N_LAYOUT_TESTS
using NUnit.Framework;
using N.Package.Animation;
using N.Package.Layout;
using UnityEngine;
using System.Collections.Generic;
using N;

public class LayoutPendingFakeAnimation : IAnimation
{
    public IAnimationTarget AnimationTarget { set; get; }
    public IAnimationCurve AnimationCurve { set; get; }
    public void AnimationUpdate(GameObject target) { }
}

public class LayoutPendingFakeLayout : ILayout
{
    public IEnumerable<LayoutObject> Layout(IAnimationTarget target)
    {
        yield break;
    }
}

public class LayoutPendingTests : N.Tests.Test
{
    public IAnimation[] animations_fixture()
    {
        var animations = new List<IAnimation>();
        for (var i = 0; i < 10; ++i)
        {
            animations.Add(new LayoutPendingFakeAnimation());
        }
        return animations.ToArray();
    }

    [Test]
    public void test_create_instance()
    {
        var instance = new LayoutPending(animations_fixture(), new LayoutPendingFakeLayout());
        Assert(instance.Pending == 10);
    }

    [Test]
    public void test_resolve_some()
    {
        var fixture = animations_fixture();
        var instance = new LayoutPending(fixture, new LayoutPendingFakeLayout());
        Assert(!instance.Resolve(fixture[0]));
        Assert(!instance.Resolve(fixture[0]));
        Assert(!instance.Resolve(fixture[9]));
        Assert(instance.Pending == 8);
    }

    [Test]
    public void test_resolve_all()
    {
        var fixture = animations_fixture();
        var instance = new LayoutPending(fixture, new LayoutPendingFakeLayout());
        for (var i = 0; i < 9; ++i)
        {
            Assert(!instance.Resolve(fixture[i]));
        }
        Assert(instance.Resolve(fixture[9]));
        Assert(instance.Pending == 0);
    }
}
#endif
