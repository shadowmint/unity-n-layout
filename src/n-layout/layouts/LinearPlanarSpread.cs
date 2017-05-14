using System.Collections.Generic;
using N.Package.Animation;
using UnityEngine;
using System.Linq;
using N;

namespace N.Package.Layout.Layouts
{
  public class LinearPlanarSpread : ILayout
  {
    /// The origin
    public Vector3 Origin;

    /// The rotation for the plane
    public Quaternion Rotation;

    /// Up vector
    public Vector3 Up;

    /// Left vector
    public Vector3 Left;

    /// The width of the layout
    public float Width;

    /// The height of the layout
    public float Height;

    public LinearPlanarSpread(GameObject alignToTarget)
    {
      this.Origin = alignToTarget.transform.position;
      this.Rotation = alignToTarget.transform.rotation;
      var renderer = alignToTarget.GetComponent<Renderer>();
      Up = alignToTarget.transform.forward.normalized;
      Left = -alignToTarget.transform.right.normalized;
      if (renderer != null)
      {
        GuessSize(renderer.bounds.size);
      }
      else
      {
        var collider = alignToTarget.GetComponent<Collider>();
        if (collider != null)
        {
          GuessSize(collider.bounds.size);
        }
        else
        {
          Width = 1f;
          Height = 1;
        }
      }
    }

    /// Yield a set of locations and orientations for each target
    public IEnumerable<LayoutObject> Layout(IAnimationTarget target)
    {
      var count = target.GameObjects().Count();
      var originX = -Width / 2f * Left;
      var intervalX = Width / (count - 1) * Left; // Number of gaps, not objects
      var offset = 0;
      foreach (var gp in target.GameObjects())
      {
        var posX = originX + offset * intervalX;
        var pos = Origin + posX;
        offset += 1;
        yield return new LayoutObject
        {
          GameObject = gp,
          Rotation = Rotation,
          Position = pos
        };
      }
    }

    /// Calculate the x and y size from a bounds object
    private void GuessSize(Vector3 bounds)
    {
      Width = bounds.x;
      Height = bounds.y;
      if (bounds.z > Width)
      {
        Width = bounds.z;
      }
      else if (bounds.z > Height)
      {
        Height = bounds.z;
      }
    }
  }
}