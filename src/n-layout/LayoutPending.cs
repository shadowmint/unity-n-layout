using N.Package.Animation.Animations;
using N.Package.Animation;
using UnityEngine;

namespace N.Package.Layout
{
  /// A pending layout type
  public class LayoutPending
  {
    /// The set of pending animations
    private IAnimation[] _pending;

    /// The count of pending animations
    public int Pending
    {
      get { return _count; }
    }

    private int _count = 0;

    /// The layout
    public ILayout Layout;

    /// Create a new instance
    /// @param pending The set of pending animations in total
    public LayoutPending(IAnimation[] pending, ILayout layout)
    {
      Layout = layout;
      _pending = pending;
      _count = pending.Length;
    }

    /// Resolve and animation if it's a member
    /// @param animation The animation to resolve
    /// @return True if there are no pending animations left
    public bool Resolve(IAnimation animation)
    {
      if (animation == null) return _count == 0;

      for (var i = 0; i < _pending.Length; ++i)
      {
        if (_pending[i] != animation) continue;
        _pending[i] = null;
        _count -= 1;
        break;
      }

      return _count == 0;
    }
  }
}