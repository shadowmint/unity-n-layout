# n-layout

Layout is a package to automate the layout of objects in world space.

## Usage

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

See the tests in the `Editor/` folder for each class for usage examples.

The `test` folder has a number of example scenes in it.

## Install

From your unity project folder:

    npm init
    npm install shadowmint/unity-n-layout --save
    echo Assets/packages >> .gitignore
    echo Assets/packages.meta >> .gitignore

The package and all its dependencies will be installed in
your Assets/packages folder.

## Development

Setup and run tests:

    npm install
    npm install ..
    cd test
    npm install
    gulp

Remember that changes made to the test folder are not saved to the package
unless they are copied back into the source folder.

To reinstall the files from the src folder, run `npm install ..` again.

### Tests

All tests are wrapped in `#if ...` blocks to prevent test spam.

You can enable tests in: Player settings > Other Settings > Scripting Define Symbols

The test key for this package is: N_LAYOUT_TESTS
