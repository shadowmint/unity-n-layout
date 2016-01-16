using UnityEngine;
using N.Package.Layout;
using N.Package.Layout.Layouts;
using N.Package.Animation;
using N.Package.Animation.Animations;
using N.Package.Animation.Targets;
using N.Package.Animation.Curves;

public class UniformPlanarFoldDemo : MonoBehaviour
{
    public GameObject alignToTarget;

    public float period;

    public float arc;

    public float length;

    public float phase;

    public void Start()
    {
        var layout = new UniformPlanarFold(alignToTarget, arc, length, phase);

        LayoutFactoryDelegate factory = (LayoutObject target) =>
        {
            var animation = new MoveSingle(target.position, target.rotation);
            animation.AnimationCurve = new SineWave(period);
            animation.AnimationTarget = new TargetSingle(target.gameObject);
            return animation;
        };

        AnimationManager.Default.Configure(Streams.STREAM_0, AnimationStreamType.DEFER, 16);
        LayoutManager.Default.Add(Streams.STREAM_0, layout, factory, new TargetByComponent<UniformPlanarFoldDemoMarker>());
    }
}
