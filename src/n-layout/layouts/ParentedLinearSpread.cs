using System.Collections.Generic;
using N.Package.Animation;
using N.Package.Animation.Targets;
using N.Package.Animation.Animations;
using N.Package.Layout;
using N.Package.Layout.Layouts;
using UnityEngine;
using System.Linq;
using N;

namespace N.Package.Layout.Layouts
{
  /// This class is designed to layout child elements inside a parent container.
  /// The items are aligned inside the container with a `gap` distance between them
  /// from the left to the right without complex processing.
  public class ParentedLinearSpread : ILayout
  {
    /// Gap between elements
    private readonly float _gap;

    /// Offset to normal positions in local space
    private readonly Vector3 _offset;

    /// Create a new instance
    /// @param gap The gap size in 'local' space
    /// @param offset The position offset.
    public ParentedLinearSpread(float gap, Vector3 offset)
    {
      this._gap = gap;
      this._offset = offset;
    }

    /// Yield a set of locations and orientations for each target
    public IEnumerable<LayoutObject> Layout(IAnimationTarget target)
    {
      // Since the parent controls the layout, the left is always the same
      var left = Vector3.left;

      // Find the origin for layout
      var origin = new Vector3(0f, 0f, 0f);

      // Layout towards the given target~
      var offset = 0;
      foreach (var gp in target.GameObjects())
      {
        var pos = origin + offset * _gap * left + this._offset;
        yield return new LayoutObject
        {
          GameObject = gp,
          Rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up),
          Position = pos
        };
        offset += 1;
      }
    }
  }
}