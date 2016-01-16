using N.Package.Animation.Animations;
using N.Package.Animation;
using UnityEngine;

namespace N.Package.Layout
{
    /// A pending layout type
    public class LayoutPending
    {
        /// The set of pending animations
        IAnimation[] pending;

        /// The count of pending animations
        public int Pending { get { return count; } }
        private int count = 0;

        /// The layout
        public ILayout layout;

        /// Create a new instance
        /// @param pending The set of pending animations in total
        public LayoutPending(IAnimation[] pending, ILayout layout)
        {
            this.layout = layout;
            this.pending = pending;
            this.count = pending.Length;
        }

        /// Resolve and animation if it's a member
        /// @param animation The animation to resolve
        /// @return True if there are no pending animations left
        public bool Resolve(IAnimation animation)
        {
            if (animation != null)
            {
                for (var i = 0; i < pending.Length; ++i)
                {
                    if (pending[i] == animation)
                    {
                        pending[i] = null;
                        count -= 1;
                        break;
                    }
                }
            }
            return count == 0;
        }
    }
}
