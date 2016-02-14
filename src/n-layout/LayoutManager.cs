using N.Package.Animation.Animations;
using N.Package.Animation;
using System.Collections.Generic;
using UnityEngine;

namespace N.Package.Layout
{
    /// A manager type to look after layouts
    public abstract class LayoutManagerBase<TStream>
    {
        /// The animation manager we're using
        private AnimationManagerBase<TStream> manager;

        /// Pending layouts
        private List<LayoutPending> pending = new List<LayoutPending>();

        /// Create a new layout manager
        /// @param manager The IAnimationManager to animate with
        public LayoutManagerBase(AnimationManagerBase<TStream> manager)
        {
            this.manager = manager;
            this.manager.Events.AddEventHandler<AnimationCompleteEvent>((ep) =>
            {
                foreach (var p in pending)
                {
                    if (p.Resolve(ep.animation))
                    {
                        var layout_event = new LayoutCompleteEvent { layout = p.layout };
                        manager.Events.Trigger(layout_event);
                    }
                }
                pending.RemoveAll((p) => p.Pending == 0);
            });
        }

        /// Apply a layout to a specific target
        public void Add(TStream stream, ILayout layout, LayoutFactoryDelegate factory, IAnimationTarget target)
        {
            var anims = new List<IAnimation>();
            foreach (var lp in layout.Layout(target))
            {
                var animation = factory(lp);
                manager.Streams.Add(stream, animation);
                anims.Add(animation);
            }
            pending.Add(new LayoutPending(anims.ToArray(), layout));
        }
    }

    public class LayoutManager : LayoutManagerBase<Streams>
    {
        /// Create a new instance
        public LayoutManager(AnimationManagerBase<Streams> manager) : base(manager)
        {
        }

        /// Returns The default LayoutManager
        private static LayoutManager instance = null;
        public static LayoutManager Default
        {
            get
            {
                if (instance == null)
                {
                    instance = new LayoutManager(AnimationManager.Default);
                }
                return instance;
            }
        }

        /// Reset the layout manager
        public static void Reset()
        {
            instance = null;
        }
    }
}
