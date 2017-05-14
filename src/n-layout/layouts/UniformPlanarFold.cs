using System.Collections.Generic;
using N.Package.Animation;
using UnityEngine;
using System.Linq;
using N;

namespace N.Package.Layout.Layouts
{
  public class UniformPlanarFold : ILayout
  {
    /// The origin
    public Vector3 Origin;

    /// The rotation for the plane
    public Quaternion Rotation;

    /// Up vector
    public Vector3 Up;

    /// Left vector
    public Vector3 Left;

    /// The Fold arc in degrees
    public float Arc;

    /// The arc length between instances
    public float Length;

    /// Vertical height increment
    public float VerticalIncrement = 0f;

    public UniformPlanarFold(GameObject alignToTarget, float arc, float length, float verticalIncrement = 0f)
    {
      Arc = arc;
      Length = length;
      VerticalIncrement = verticalIncrement;
      Origin = alignToTarget.transform.position;
      Rotation = alignToTarget.transform.rotation;
      Up = alignToTarget.transform.forward.normalized;
      Left = -alignToTarget.transform.right.normalized;
    }

    /// Yield a set of locations and orientations for each target
    public IEnumerable<LayoutObject> Layout(IAnimationTarget target)
    {
      var count = target.GameObjects().Count();
      var baseArc = Mathf.Deg2Rad * (180f - (Arc * (count - 1))) / 2f;
      var step = Arc * Mathf.Deg2Rad;
      var normal = Vector3.Cross(Left, Up);
      var offset = 0;
      foreach (var gp in target.GameObjects())
      {
        var angle = offset * step;
        var posX = Mathf.Cos(baseArc + angle) * Length * Left;
        var posY = Mathf.Sin(baseArc + angle) * Length * Up;
        var newNormal = (posX + posY).normalized;
        var pos = Origin + posX + posY + normal * VerticalIncrement * offset;
        offset += 1;
        yield return new LayoutObject
        {
          GameObject = gp,
          Rotation = Quaternion.LookRotation(newNormal, normal),
          Position = pos
        };
      }
    }
  }
}