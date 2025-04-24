using System.Numerics;
using Magnets;
using NUnit.Framework;

namespace MagnetTests
{
    [TestFixture]
    public class RectangularMagnetTests
    {
        [Test]
        public void BasicTest()
        {
            var magnet =
                new RectangularMagnet(Vector3.Multiply(new Vector3(.25f, .25f, .25f) /*inches*/, (float)Scale.MetresPerInch))
                {
                    SurfaceField = 4601 * Scale.TeslaPerGauss
                };

            var b = magnet.B(Vector3.Multiply(new Vector3(0f, 0f, 0.3f + magnet.Size.Z/2.0f), (float)Scale.MetresPerInch));
        }

        [Test]
        public void BasicTestNegative()
        {
            var magnet = new RectangularMagnet(Vector3.Multiply(new Vector3(.25f, .25f, .25f) /*inches*/, (float)Scale.MetresPerInch))
            {
                SurfaceField = 4601 * Scale.TeslaPerGauss
            };

            var b = magnet.B(Vector3.Multiply(new Vector3(0f, 0f, -0.3f - magnet.Size.Z / 2.0f), (float)Scale.MetresPerInch));
        }

        [Test]
        public void SetRemanenceTest()
        {
            var magnet = new RectangularMagnet(Vector3.Multiply(new Vector3(.25f, .25f, .25f) /*inches*/, (float)Scale.MetresPerInch))
            {
                SurfaceField = 4601 * Scale.TeslaPerGauss
            };
        }

        [Test]
        public void BasicTest2()
        {
            var magnet = new RectangularMagnet(new Vector3(.00635f, .00635f, .00635f) /*meters*/)
            {
                Remanence = 1.0555
            };

            //magnet.SetRemanenceFromSurfaceField(0.4601);

            var h = magnet.H(new Vector3(0f, 0f, magnet.Size.Z));
        }
    }
}
