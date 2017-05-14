using N.Package.Animation;
using UnityEngine;

namespace N.Package.Layout
{
  /// Generate a new animation from a layout
  public interface ILayoutFactory
  {
    /// Yield a set of locations and orientations for each target
    IAnimation Animation(LayoutObject layout);
  }
}