using UnityEngine;
using N;
using N.Package.Layout;
using N.Package.Layout.Layouts;
using N.Package.Animation;
using N.Package.Animation.Animations;
using N.Package.Animation.Targets;
using N.Package.Animation.Curves;

public class LayoutSequenceDemo : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;
    public GameObject target4;

    public float duration;

    public int offset = 0;

    public bool idle = true;

    /// Common factory type
    LayoutFactoryDelegate factory;

    public void Start()
    {
        factory = (LayoutObject target) =>
        {
             var animation = new MoveSingle(target.position, target.rotation);
             animation.AnimationCurve = new Linear(duration);
             animation.AnimationTarget = new TargetSingle(target.gameObject);
             return animation;
        };

        AnimationManager.Default.Events.AddEventHandler<LayoutCompleteEvent>((ep) =>
        {
            offset += 1;
            if (offset >= 4)
            {
                offset = 0;
            }
            idle = true;
        });

        AnimationManager.Default.Configure(Streams.STREAM_0, AnimationStreamType.DEFER, 16);
    }

    public void Update()
    {
        if (idle)
        {
            if (offset == 0)
            {
                RunLayoutOne();
                idle = false;
            }
            else if (offset == 1)
            {
                RunLayoutTwo();
                idle = false;
            }
            else if (offset == 2)
            {
                RunLayoutThree();
                idle = false;
            }
            else if (offset == 3)
            {
                RunLayoutFour();
                idle = false;
            }
        }
    }

    public void RunLayoutOne()
    {
        _.Log("Layout one");
        var layout = new UniformPlanarFold(target3, 30f, 4f);
        LayoutManager.Default.Add(Streams.STREAM_0, layout, factory, new TargetByComponent<DemoMarker>());
    }

    public void RunLayoutTwo()
    {
        _.Log("Layout two");
        var layout = new LinearPlanarSpread(target1);
        LayoutManager.Default.Add(Streams.STREAM_0, layout, factory, new TargetByComponent<DemoMarker>());
    }

    public void RunLayoutThree()
    {
        _.Log("Layout three");
        var layout = new UniformPlanarSpread(target2);
        LayoutManager.Default.Add(Streams.STREAM_0, layout, factory, new TargetByComponent<DemoMarker>());
    }

    public void RunLayoutFour()
    {
        _.Log("Layout four");
        var layout = new LinearPlanarSpread(target4);
        LayoutManager.Default.Add(Streams.STREAM_0, layout, factory, new TargetByComponent<DemoMarker>());
    }
}
