using System.Collections.Generic;
using N.Package.Animation;
using UnityEngine;

namespace N.Package.Layout
{
  /// Yield a set of locations and orientations for each target
  public delegate IAnimation LayoutFactoryDelegate(LayoutObject layout);

  /// The layout for a GameObject
  public struct LayoutObject
  {
    /// The GameObject
    public GameObject GameObject;

    /// The desired position
    public Vector3 Position;

    /// The desired rotation
    public Quaternion Rotation;
  }

  /// A generic interface for generating a layout for a set of objects
  public interface ILayout
  {
    /// Yield a set of locations and orientations for each target
    IEnumerable<LayoutObject> Layout(IAnimationTarget target);
  }
}