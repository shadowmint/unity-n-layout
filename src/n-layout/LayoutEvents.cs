using N.Package.Animation.Animations;
using N.Package.Animation;
using UnityEngine;

namespace N.Package.Layout
{
    /// A layout event that occurs when a layout is complete
    public class LayoutCompleteEvent : Event
    {
        public ILayout layout;
    }
}
