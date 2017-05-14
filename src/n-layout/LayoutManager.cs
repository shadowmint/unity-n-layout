using N.Package.Animation.Animations;
using N.Package.Animation;
using System.Collections.Generic;
using UnityEngine;

namespace N.Package.Layout
{
  /// A manager type to look after layouts
  public abstract class LayoutManagerBase<TStream>
  {
    /// The animation manager we're using
    private readonly AnimationManagerBase<TStream> _manager;

    /// Pending layouts
    private readonly List<LayoutPending> _pending = new List<LayoutPending>();

    /// Create a new layout manager
    /// @param manager The IAnimationManager to animate with
    protected LayoutManagerBase(AnimationManagerBase<TStream> manager)
    {
      _manager = manager;
      _manager.Events.AddEventHandler<AnimationCompleteEvent>((ep) =>
      {
        foreach (var p in _pending)
        {
          if (!p.Resolve(ep.Animation)) continue;
          var layoutEvent = new LayoutCompleteEvent {Layout = p.Layout};
          manager.Events.Trigger(layoutEvent);
        }
        _pending.RemoveAll((p) => p.Pending == 0);
      });
    }

    /// Apply a layout to a specific target
    public void Add(TStream stream, ILayout layout, LayoutFactoryDelegate factory, IAnimationTarget target)
    {
      var anims = new List<IAnimation>();
      foreach (var lp in layout.Layout(target))
      {
        var animation = factory(lp);
        _manager.Streams.Add(stream, animation);
        anims.Add(animation);
      }
      _pending.Add(new LayoutPending(anims.ToArray(), layout));
    }
  }

  public class LayoutManager : LayoutManagerBase<Streams>
  {
    /// Create a new instance
    public LayoutManager(AnimationManagerBase<Streams> manager) : base(manager)
    {
    }

    /// Returns The default LayoutManager
    private static LayoutManager instance = null;

    public static LayoutManager Default
    {
      get { return instance ?? (instance = new LayoutManager(AnimationManager.Default)); }
    }

    /// Reset the layout manager
    public static void Reset()
    {
      instance = null;
    }
  }
}