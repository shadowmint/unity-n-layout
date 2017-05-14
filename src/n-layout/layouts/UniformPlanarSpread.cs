using System.Collections.Generic;
using N.Package.Animation;
using UnityEngine;
using System.Linq;
using N;

namespace N.Package.Layout.Layouts
{
  public class UniformPlanarSpread : ILayout
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

    public UniformPlanarSpread(GameObject alignToTarget)
    {
      Origin = alignToTarget.transform.position;
      Rotation = alignToTarget.transform.rotation;
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
          Height = 1f;
        }
      }
    }

    public UniformPlanarSpread(Vector3 origin, Vector3 normal, Vector3 up, float width, float height)
    {
      Origin = origin;
      Rotation = Quaternion.LookRotation(normal, up);
      Up = up;
      Left = -Vector3.Cross(up, normal);
      Width = width;
      Height = height;
    }

    /// Yield a set of locations and orientations for each target
    public IEnumerable<LayoutObject> Layout(IAnimationTarget target)
    {
      var count = target.GameObjects().Count();
      var n = (int) Mathf.Floor(Mathf.Sqrt(count));
      var origin_x = -Width / 2f * Left;
      var origin_y = -Height / 2f * Up;
      var interval_x = Width / (n - 1) * Left; // Number of gaps, not objects
      var interval_y = Height / (n - 1) * Up;
      var offset = 0;
      foreach (var gp in target.GameObjects())
      {
        var x = offset / n;
        var y = offset % n;
        var pos_x = origin_x + x * interval_x;
        var pos_y = origin_y + y * interval_y;
        offset += 1;
        var pos = this.Origin + pos_x + pos_y;
        yield return new LayoutObject
        {
          GameObject = gp,
          Rotation = this.Rotation,
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