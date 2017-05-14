using System.Collections.Generic;
using N.Package.Animation;
using UnityEngine;
using System.Linq;

namespace N.Package.Layout.Layouts
{
  /// Puts the elements on the given canvas equally spread around the ring.
  public class RingCanvasSpread : ILayout
  {
    /// Associated camera for this layout (defaults to main camera)
    public Camera Camera { get; set; }

    /// The canvas to place this UI elements on
    public GameObject WorldObject { get; set; }

    /// The radius of the ring to place objects with.
    public float Radius { get; set; }

    /// Yield a set of locations and orientations for each target
    public IEnumerable<LayoutObject> Layout(IAnimationTarget target)
    {
      // Get objects
      var objects = target.GameObjects().ToArray();
      if (objects.Length == 0) yield break;

      // Get camera
      if (Camera == null)
      {
        Camera = Camera.main;
      }

      // Angle between instances
      var spread = 2 * Mathf.PI / objects.Length;

      // Cast to origin point; yes, we do this every frame
      var origin = Camera.WorldToScreenPoint(WorldObject.transform.position);

      // Place on the ring
      for (var i = 0; i < objects.Length; ++i)
      {
        var x = Radius * Mathf.Cos(i * spread);
        var y = Radius * Mathf.Sin(i * spread);
        var trans = WorldObject.transform;
        yield return new LayoutObject
        {
          GameObject = objects[i],
          Rotation = trans.rotation,
          Position = origin + x * trans.right + y * trans.up
        };
      }
    }
  }
}