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
        public Vector3 origin;

        /// The rotation for the plane
        public Quaternion rotation;

        /// Up vector
        public Vector3 up;

        /// Left vector
        public Vector3 left;

        /// The width of the layout
        public float width;

        /// The height of the layout
        public float height;

        public UniformPlanarSpread(GameObject alignToTarget)
        {
            this.origin = alignToTarget.transform.position;
            this.rotation = alignToTarget.transform.rotation;
            var renderer = alignToTarget.GetComponent<Renderer>();
            up = alignToTarget.transform.forward.normalized;
            left = -alignToTarget.transform.right.normalized;
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
                    width = 1f;
                    height = 1f;
                }
            }
        }

        public UniformPlanarSpread(Vector3 origin, Vector3 normal, Vector3 up, float width, float height)
        {
            this.origin = origin;
            this.rotation = Quaternion.LookRotation(normal, up);
            this.up = up;
            this.left = -Vector3.Cross(up, normal);
            this.width = width;
            this.height = height;
        }

        /// Yield a set of locations and orientations for each target
        public IEnumerable<LayoutObject> Layout(IAnimationTarget target)
        {
            var count = target.GameObjects().Count();
            var n = (int)Mathf.Floor(Mathf.Sqrt(count));
            var origin_x = -width / 2f * left;
            var origin_y = -height / 2f * up;
            var interval_x = width / (n - 1) * left;  // Number of gaps, not objects
            var interval_y = height / (n - 1) * up;
            var offset = 0;
            foreach (var gp in target.GameObjects())
            {
                var x = offset / n;
                var y = offset % n;
                var pos_x = origin_x + x * interval_x;
                var pos_y = origin_y + y * interval_y;
                offset += 1;
                var pos = this.origin + pos_x + pos_y;
                yield return new LayoutObject
                {
                    gameObject = gp,
                    rotation = this.rotation,
                    position = pos
                };
            }
        }

        /// Calculate the x and y size from a bounds object
        private void GuessSize(Vector3 bounds)
        {
            width = bounds.x;
            height = bounds.y;
            if (bounds.z > width)
            {
                width = bounds.z;
            }
            else if (bounds.z > height)
            {
                height = bounds.z;
            }
        }
    }
}
