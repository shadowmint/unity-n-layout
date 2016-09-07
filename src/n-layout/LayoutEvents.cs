using N.Package.Animation.Animations;
using N.Package.Animation;
using N.Package.Events;
using UnityEngine;

namespace N.Package.Layout
{
    /// A layout event that occurs when a layout is complete
    public class LayoutCompleteEvent
    {
        public ILayout Layout { get; set; }
    }
}
