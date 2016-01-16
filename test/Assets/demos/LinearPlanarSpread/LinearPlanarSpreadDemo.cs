using UnityEngine;
using N.Package.Layout;
using N.Package.Layout.Layouts;
using N.Package.Animation;
using N.Package.Animation.Animations;
using N.Package.Animation.Targets;
using N.Package.Animation.Curves;

public class LinearPlanarSpreadDemo : MonoBehaviour
{
    public GameObject plane;

    public float period;

    public void Start()
    {
        var layout = new LinearPlanarSpread(plane);

        LayoutFactoryDelegate factory = (LayoutObject target) =>
        {
            var animation = new MoveSingle(target.position, target.rotation);
            animation.AnimationCurve = new SineWave(period);
            animation.AnimationTarget = new TargetSingle(target.gameObject);
            return animation;
        };

        AnimationManager.Default.Configure(Streams.STREAM_0, AnimationStreamType.DEFER, 16);
        LayoutManager.Default.Add(Streams.STREAM_0, layout, factory, new TargetByComponent<DemoMarker>());
    }
}
