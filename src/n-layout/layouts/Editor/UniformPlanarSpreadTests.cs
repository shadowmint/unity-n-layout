#if N_LAYOUT_TESTS
using UnityEngine;
using NUnit.Framework;
using N;
using N.Package.Layout;
using N.Package.Layout.Layouts;
using N.Package.Animation;
using N.Package.Animation.Animations;
using N.Package.Animation.Curves;
using N.Package.Animation.Targets;
using N.Package.Core;
using N.Package.Core.Tests;

public class UniformPlanarSpreadTests : TestCase
{
  [Test]
  public void test_layout()
  {
    LayoutManagerTests.Reset(this);

    AnimationManager.Default.Configure(Streams.STREAM_0, AnimationStreamType.DEFER, 30);

    var layout = new UniformPlanarSpread(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), 10f, 10f);

    LayoutFactoryDelegate factory = (LayoutObject target) =>
    {
      var animation = new MoveSingle(target.position, target.rotation);
      animation.AnimationCurve = new Linear(1f);
      animation.AnimationTarget = new TargetSingle(target.gameObject);
      return animation;
    };

    var targets = new GameObject[]
    {
      this.SpawnBlank(),
      this.SpawnBlank(),
      this.SpawnBlank(),
      this.SpawnBlank(),
      this.SpawnBlank(),
      this.SpawnBlank(),
    };

    LayoutManager.Default.Add(Streams.STREAM_0, layout, factory, new TargetGroup(targets));

    Assert(AnimationManager.Default.Streams.Active(Streams.STREAM_0));

    int count = 0;
    AnimationManager.Default.Events.AddEventHandler<AnimationCompleteEvent>((evp) => { count += 1; });

    AnimationHandler.Default.Update(0.5f);

    Assert(AnimationManager.Default.Streams.Active(Streams.STREAM_0));

    AnimationHandler.Default.Update(0.5f);
    AnimationHandler.Default.Update(0.5f);

    Assert(!AnimationManager.Default.Streams.Active(Streams.STREAM_0));

    LayoutManagerTests.Reset(this);
  }
}
#endif