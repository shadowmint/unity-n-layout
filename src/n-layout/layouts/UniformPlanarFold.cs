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
        public Vector3 origin;

        /// The rotation for the plane
        public Quaternion rotation;

        /// Up vector
        public Vector3 up;

        /// Left vector
        public Vector3 left;

        /// The Fold arc in degrees
        public float arc;

        /// The arc length between instances
        public float length;

        /// Vertical height increment
        public float verticalIncrement = 0f;

        public UniformPlanarFold(GameObject alignToTarget, float arc, float length, float verticalIncrement = 0f)
        {
            this.arc = arc;
            this.length = length;
            this.verticalIncrement = verticalIncrement;
            this.origin = alignToTarget.transform.position;
            this.rotation = alignToTarget.transform.rotation;
            up = alignToTarget.transform.forward.normalized;
            left = -alignToTarget.transform.right.normalized;
        }

        /// Yield a set of locations and orientations for each target
        public IEnumerable<LayoutObject> Layout(IAnimationTarget target)
        {
            var count = target.GameObjects().Count();
            var base_arc = Mathf.Deg2Rad * (180f - (arc * (count - 1))) / 2f;
            var step = arc * Mathf.Deg2Rad;
            var normal = Vector3.Cross(left, up);
            var offset = 0;
            foreach (var gp in target.GameObjects())
            {
                var angle = offset * step;
                var pos_x = Mathf.Cos(base_arc + angle) * length * left;
                var pos_y = Mathf.Sin(base_arc + angle) * length * up;
                var new_normal = (pos_x + pos_y).normalized;
                var pos = this.origin + pos_x + pos_y + normal * verticalIncrement * offset;
                offset += 1;
                yield return new LayoutObject
                {
                    gameObject = gp,
                    rotation = Quaternion.LookRotation(new_normal, normal),
                    position = pos
                };
            }
        }
    }
}
